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

namespace RaiSam.Controllers.BasicInfo
{
    public class HelpController : Controller
    {
        //
        // GET: /Help/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

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
        public ActionResult New(int Id, string NameForm)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.Id = Id;
            result.ViewBag.NameForm = NameForm;
            return result;
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelp()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "42", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinHelp()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoHelp()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "42", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Permission.haveAccess(79, "tblHelp","0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<prs_tblHelpSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<prs_tblHelpSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldFormName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldFormName";
                                break;
                            case "fldFileSize_PDF":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldFileSize_PDF";
                                break;
                            case "fldFileSize_Video":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldFileSize_Video";
                                break;

                        }
                        if (data != null)
                            data1 = m.prs_tblHelpSelect(field, searchtext,100).ToList();
                        else
                            data = m.prs_tblHelpSelect(field, searchtext, 100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblHelpSelect("","",100).ToList();
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
                                return !(oValue.ToString().IndexOf(value.ToString(), StringComparison.OrdinalIgnoreCase) >= 0);
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

                List<prs_tblHelpSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { Code = "403" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblHelpSelect("fldId", Id.ToString(),1).FirstOrDefault();

            return Json(new
            {
                fldId = q.fldId,

                NameFilePdf = q.fldFileName_PDF,
                NameFileVideo = q.fldFileName_Video
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearSession()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Session["savePathPdf"] != null)
            {
                System.IO.File.Delete(Session["savePathPdf"].ToString());
                Session.Remove("savePathPdf");
                Session.Remove("FileNamePdf");
            }
            if (Session["savePathVideo"] != null)
            {
                System.IO.File.Delete(Session["savePathVideo"].ToString());
                Session.Remove("savePathVideo");
                Session.Remove("FileNameVideo");
            }
            return null;
        }
        public ActionResult UploadPdf()
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePathPdf"] != null)
                {
                    System.IO.File.Delete(Session["savePathPdf"].ToString());
                    Session.Remove("savePathPdf");
                    Session.Remove("FileNamePdf");
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                if (IsPDF == true/*extension == ".pdf"*/)
                {
                    if (Request.Files[0].ContentLength <= 10485760)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathPdf"] = savePath;
                        Session["FileNamePdf"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
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
                            Message = "حجم فایل انتخابی می بایست کمتر از 10 مگابایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از 10 مگابایت باشد."
                        //});
                        //DirectResult result = new DirectResult();
                        //return result;
                    }
                }
                else
                {
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName,
                        Message = "فرمت فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فرمت فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
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
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
        }
        public ActionResult UploadVideo()
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePathVideo"] != null)
                {
                    System.IO.File.Delete(Session["savePathVideo"].ToString());
                    Session.Remove("savePathVideo");
                    Session.Remove("FileNameVideo");
                }
                var extension = Path.GetExtension(Request.Files[1].FileName).ToLower();
                var IsMP4 = FileInfoExtensions.IsMP4(Request.Files[1]);
                if (IsMP4 == true/*extension == ".mp4"*/)
                {
                    if (Request.Files[1].ContentLength <= 104857600)
                    {
                        HttpPostedFileBase file = Request.Files[1];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathVideo"] = savePath;
                        Session["FileNameVideo"] = Path.GetFileNameWithoutExtension(Request.Files[1].FileName);
                        object r = new
                        {
                            success = true,
                            name = Request.Files[1].FileName,
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
                            name = Request.Files[1].FileName,
                            Message = "حجم فایل انتخابی می بایست کمتر از 100 مگابایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از 100 مگابایت باشد."
                        //});
                        //DirectResult result = new DirectResult();
                        //return result;
                    }
                }
                else
                {
                    object r = new
                    {
                        success = true,
                        name = Request.Files[1].FileName,
                        Message = "فرمت فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فرمت فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
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
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFile(prs_tblHelpSelect Help)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "", MsgTitle = ""; var Er = 0;
            byte[] fldFilePdf = null;
            byte[] fldFileVideo = null;
            try
            {
                if (Session["savePathPdf"] == null && Session["savePathVideo"] == null)
                {
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "لطفا حداقل یک فایل را آپلود نمایید.",
                        Er = 1
                    });
                }
                if (Session["savePathPdf"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathPdf"].ToString()));
                    fldFilePdf = stream.ToArray();
                    Help.fldPasvand_Pdf = Path.GetExtension(Session["savePathPdf"].ToString());
                    Help.fldFileName_PDF = Session["FileNamePdf"].ToString();
                }
                else
                {
                    Help.fldPasvand_Pdf = "";
                    fldFilePdf = null;
                    Help.fldFileName_PDF = "";
                }
                if (Session["savePathVideo"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathVideo"].ToString()));
                    fldFileVideo = stream.ToArray();
                    Help.fldPasvand_Video = Path.GetExtension(Session["savePathVideo"].ToString());
                    Help.fldFileName_Video = Session["FileNameVideo"].ToString();
                }
                else
                {
                    Help.fldPasvand_Video = "";
                    fldFileVideo = null;
                    Help.fldFileName_Video = "";
                }
                var H = m.prs_tblHelpSelect("fldId",Help.fldId.ToString(),1).FirstOrDefault();
                Help.fldFileVideoId = H.fldFileVideoId;
                Help.fldFilePdfId = H.fldFilePdfId;
                var s = m.prs_tblHelpUpdate(Help.fldId, Help.fldFormName, fldFilePdf, Help.fldPasvand_Pdf, Help.fldFileName_PDF, fldFileVideo, Help.fldPasvand_Video, Help.fldFileName_Video,"",u.UserInputId,Help.fldFilePdfId,Help.fldFileVideoId);
              

                if (Session["savePathPdf"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathPdf"].ToString());
                    Session.Remove("savePathPdf");
                    Session.Remove("FileNamePdf");
                    System.IO.File.Delete(physicalPath);
                }
                if (Session["savePathVideo"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathVideo"].ToString());
                    Session.Remove("savePathVideo");
                    Session.Remove("FileNameVideo");
                    System.IO.File.Delete(physicalPath);
                }
                Msg = "ذخیره با موفقیت انجام شد.";
                MsgTitle ="ذخیره موفق";
            }
            catch (Exception x)
            {
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                if (Session["savePathPdf"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathPdf"].ToString());
                    Session.Remove("savePathPdf");
                    Session.Remove("FileNamePdf");
                    System.IO.File.Delete(physicalPath);
                }
                if (Session["savePathVideo"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathVideo"].ToString());
                    Session.Remove("savePathVideo");
                    Session.Remove("FileNameVideo");
                    System.IO.File.Delete(physicalPath);
                }
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1,
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
}
