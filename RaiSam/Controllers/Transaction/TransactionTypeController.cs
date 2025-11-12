using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System.IO;
using Newtonsoft.Json;

namespace RaiSam.Controllers.Transaction
{
    public class TransactionTypeController : Controller
    {
        //
        // GET: /TransactionType/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;

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
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                if (Session["savePathTransType"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathTransType"].ToString());
                    Session.Remove("savePathTransType");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (IsImage == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 307200)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileName"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                        Session["savePathTransType"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "فایل با موفقیت آپلود شد.",
                            IsValid = true
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "حجم فایل انتخاب شده غیرمجاز است.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));

                    }
                }
                else
                {
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName,
                        Message = "فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                }
            }
            catch (Exception x)
            {
                var Msg = "";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowPicUpload(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathTransType"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }
        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathTransType"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathTransType"].ToString());
                Session.Remove("savePathTransType");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTransactionGroup()
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
                UserInfo user = (UserInfo)(Session["User"]);
                
                var q = m.prs_tblTransactionGroupSelect("","",0).Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblTransactionTypeSelect TransactionType, bool DeleteFile)
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
                string Msg = "", MsgTitle = ""; var Er = 0; byte[] file = null; string Pasvand = ""; string NameFile = "";
                UserInfo user = (UserInfo)(Session["User"]);
                try
                {

                    if (TransactionType.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(43, "tblTransactionType", null))
                        {
                            if (Session["savePathTransType"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathTransType"].ToString()));
                                Pasvand = Path.GetExtension(Session["savePathTransType"].ToString());
                                file = stream.ToArray();
                                NameFile = Session["FileName"].ToString();
                            }
                            else
                            {
                                Pasvand = "";
                                file = null;
                                NameFile = "";
                            }
                            m.prs_tblTransactionTypeInsert(TransactionType.fldName, TransactionType.fldTransactionGroupId, file, Pasvand, NameFile, "");
                            if (Session["savePathTransType"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathTransType"].ToString());
                                Session.Remove("savePathTransType");
                                Session.Remove("FileName");
                                System.IO.File.Delete(physicalPath);
                            }

                            Msg = "ذخیره با موفقیت انجام شد";
                            MsgTitle = "دخیره موفق";
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //ویرایش
                        if (Permission.haveAccess(44, "tblTransactionType", TransactionType.fldId.ToString()))
                        {
                            if (Session["savePathTransType"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathTransType"].ToString()));
                                Pasvand = Path.GetExtension(Session["savePathTransType"].ToString());
                                file = stream.ToArray();
                                NameFile = Session["FileName"].ToString();
                            }
                            else
                            {
                                if (DeleteFile == true)
                                {
                                    file = null;
                                    Pasvand = "";
                                    NameFile = "";
                                }
                                else
                                {

                                    var f = m.prs_tblFileSelect("fldId", TransactionType.fldFileId.ToString(), 1).FirstOrDefault();

                                    if (f != null)
                                    {
                                        file = f.fldFile;
                                        Pasvand = f.fldPasvand;
                                        NameFile = f.fldFileName;
                                    }
                                }
                            }
                            TransactionType.fldFile = file;
                            TransactionType.fldPasvand = Pasvand;
                            TransactionType.fldFileName = NameFile;
                            m.prs_tblTransactionTypeUpdate(TransactionType.fldId, TransactionType.fldName, TransactionType.fldTransactionGroupId, TransactionType.fldFileId, file, Pasvand, NameFile);
                            if (Session["savePathTransType"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathTransType"].ToString());
                                Session.Remove("savePathTransType");
                                Session.Remove("FileName");
                                System.IO.File.Delete(physicalPath);
                            }

                            Msg = "ویرایش با موفقیت انجام شد";
                            MsgTitle = "ویرایش موفق";
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");


                    if (Session["savePathTransType"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePathTransType"].ToString());
                        Session.Remove("savePathTransType");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
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
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                try
                {//حذف
                    if (Permission.haveAccess(44, "tblTransactionType", id.ToString()))
                    {
                    m.prs_tblTransactionTypeDelete(id);

                    Msg = "حدف با موفقیت انجام شد";
                    MsgTitle = "حذف موفق";
                    }
                    else
                    {
                        return Json(new
                        {
                            Msg = "شما مجاز به دسترسی نمی باشید.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");

                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
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
                UserInfo user = (UserInfo)(Session["User"]);
               
                var q = m.prs_tblTransactionTypeSelect("fldId", Id.ToString(), 1).FirstOrDefault();
                var Iconn = "";
                if (q.fldFile != null)
                {
                    Iconn = Convert.ToBase64String(q.fldFile);
                }
                return Json(new
                {
                    fldId = q.fldId,
                    fldFileId = q.fldFileId,
                    fldIcon = Iconn,
                    NameFile = q.fldFileName,
                    fldTitle = q.fldName,
                    fldNameGroup = q.fldNameGroup,
                    fldTransactionGroupId = q.fldTransactionGroupId.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
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
                UserInfo user = (UserInfo)(Session["User"]);
                if (Permission.haveAccess(41,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<Models.prs_tblTransactionTypeSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblTransactionTypeSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldName";
                                    break;
                                case "fldNameGroup":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameGroup";
                                    break;

                            }
                            
                            if (data != null)
                                data1 = m.prs_tblTransactionTypeSelect(field, searchtext,100).ToList();
                            else
                                data = m.prs_tblTransactionTypeSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        
                        data = m.prs_tblTransactionTypeSelect("","",100).ToList();
                    }

                    var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    //FilterConditions fc = parameters.GridFilters;

                    //-- start filtering ------------------------------------------------------------
                    if (fc != null)
                    {
                        foreach (var condition in fc.Conditions)
                        {
                            string field = condition.FilterProperty.Name;
                            var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

                            data.RemoveAll(
                                item =>
                                {
                                    object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                    return !oValue.ToString().Contains(value.ToString());
                                }
                            );
                        }
                    }
                    //-- end filtering ------------------------------------------------------------

                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblTransactionTypeSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
    }
}
