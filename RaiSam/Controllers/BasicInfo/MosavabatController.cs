using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;
using RaiSam.Models;


namespace RaiSam.Controllers.BasicInfo
{
    public class MosavabatController : Controller
    {
        //
        // GET: /Mosavabat/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (servic.Permission(44, Session["Username"].ToString(), Session["Password"].ToString(), out Err))
            //{
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;
            //}
            //else
            //{
            //    return null;
            //}

        }
        public ActionResult New(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTypeContract()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblContractTypeSelect("", "", 0).Select(l => new { fldId = l.fldId, fldName = l.fldTitle });
                return this.Store(q);
            }
        }
        public ActionResult Upload()
        {
            string Msg = "";
            try
            {
                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }

                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                var IsWord = FileInfoExtensions.IsWord(Request.Files[0]);

                var Passvand = Path.GetExtension(Request.Files[0].FileName);

                //if (Request.Files[0].ContentType == "application/pdf"||Request.Files[0].ContentType == "application/msword" || Request.Files[0].ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                if (IsPDF == true || IsWord == true /*|| Passvand.ToLower() == ".rar" || Passvand.ToLower() == ".zip"*/)
                {
                    if (Request.Files[0].ContentLength <= 10240000)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileName"] = file.FileName;
                        Session["savePath"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "خطا",
                            Message = "حجم فایل انتخابی می بایست کمتر از 10 مگابایت باشد."
                        });
                        DirectResult result = new DirectResult();
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده غیر مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = Msg
                });
                DirectResult result = new DirectResult();
                return result;
            }
        }
        public FileContentResult DownloadFile(int FileId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null && Session["FristRegisterId"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFileSelect("fldId", FileId.ToString(), 1).FirstOrDefault();

            if (q != null && q.fldFile != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
        public ActionResult Save(Models.prs_tblMosavabatSelect Mosavabat)
        {
            string Msg = "", MsgTitle = ""; var Er = 0; int? FileId = 0;
            byte[] report_file = null; string FileName = ""; string Passvand = "";
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (Mosavabat.fldId == 0)
                {
                    //ذخیره

                    if (Session["savePath"] != null)
                    {
                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                        report_file = stream.ToArray();
                        FileName = Session["FileName"].ToString();
                        Passvand = Path.GetExtension(Session["savePath"].ToString());
                    }
                    else
                    {
                        return Json(new
                        {
                            Msg = "لطفا ابتدا فایل را وارد نمایید.",
                            MsgTitle = "خطا",
                            Err = 1,
                        }, JsonRequestBehavior.AllowGet);
                    }
                    if (Permission.haveAccess(259, "", "0"))
                    {
                        MsgTitle = "ذخیره موفق";
                        Msg = "ذخیره با موفقیت انجام شد.";
                        FileId = m.prs_tblMosavabatInsert(report_file,Passvand,  FileName, Mosavabat.fldTypeContractId, Mosavabat.fldTitle, user.UserInputId).FirstOrDefault().fldFileId;

                    }
                    else
                    {
                        MsgTitle = "خطا";
                        Msg = "شما مجاز به ذخیره اطلاعات نمی باشد.";
                        Er = 1;
                    }
                }
                else
                {
                    //ویرایش
                    if (Permission.haveAccess(260, "", "0"))
                    {
                        if (Session["savePath"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                            report_file = stream.ToArray();
                            FileName = Session["FileName"].ToString();
                            Passvand = Path.GetExtension(Session["savePath"].ToString());
                        }
                        FileId = Mosavabat.fldFileId;
                        Msg = "ویرایش با موفقیت انجام شد.";
                        if (Session["savePath"] != null)
                        {
                            MsgTitle = "ویرایش موفق";
                            m.prs_tblMosavabatUpdate(Mosavabat.fldId,FileId, report_file, Passvand, FileName, Mosavabat.fldTypeContractId, Mosavabat.fldTitle, user.UserInputId);
                        }
                        else
                        {
                            MsgTitle = "ویرایش موفق";
                            m.prs_tblMosavabatUpdate(Mosavabat.fldId, FileId, null, Passvand, FileName, Mosavabat.fldTypeContractId, Mosavabat.fldTitle, user.UserInputId);
                        }
                    }
                    else
                    {
                        MsgTitle = "خطا";
                        Msg = "شما مجاز به ویرایش اطلاعات نمی باشد.";
                        Er = 1;
                    }
                }
               
                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
                Er = 1;
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er,
                FileId = FileId
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            try
            {
                //حذف

                if (Permission.haveAccess(261, "", "0"))
                {
                    MsgTitle = "حذف موفق";
                    m.prs_tblMosavabatDelete(id, user.UserInputId);
                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "شما مجاز به حذف اطلاعات نمی باشد.";
                    Er = 1;
                }
              
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblMosavabatSelect("fldId",Id.ToString(), 0).FirstOrDefault();

            return Json(new
            {
                fldId = q.fldId,
                fldTitle = q.fldTitle,
                fldFileId = q.fldFileId,
                fldTypeContractId = q.fldTypeContractId.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Permission.haveAccess(258, "", "0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                List<Models.prs_tblMosavabatSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<Models.prs_tblMosavabatSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldTitle": 
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTitle";
                                break;
                            case "fldTitleType": 
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTitleType";
                                break;

                        }
                        if (data != null)
                            data1 = m.prs_tblMosavabatSelect(field, searchtext, 100).ToList();
                        else
                            data = m.prs_tblMosavabatSelect(field, searchtext, 100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblMosavabatSelect("", "", 100).ToList();
                }

                var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                //FilterConditions fc = parameters.GridFilters;

                //-- start filtering ------------------------------------------------------------
                //if (fc != null)
                //{
                //    foreach (var condition in fc.Conditions)
                //    {
                //        string field = condition.FilterProperty.Name;
                //        var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

                //        data.RemoveAll(
                //            item =>
                //            {
                //                object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                //                return !oValue.ToString().Contains(value.ToString());
                //            }
                //        );
                //    }
                //}
                //-- end filtering ------------------------------------------------------------

                //-- start paging ------------------------------------------------------------
                int limit = parameters.Limit;

                if ((parameters.Start + parameters.Limit) > data.Count)
                {
                    limit = data.Count - parameters.Start;
                }

                List<Models.prs_tblMosavabatSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);

            }
            else
                return null;
        }

    }
}
