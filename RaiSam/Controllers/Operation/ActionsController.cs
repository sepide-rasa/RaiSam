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

namespace RaiSam.Controllers.Operation
{
    public class ActionsController : Controller
    {
        //
        // GET: /Actions/
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
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                return PartialView;
            }
        }
        public FileContentResult ShowHelpAction()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "12", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinAction()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoAction()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "12", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
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
        public FileContentResult Download(int fldFilePdfId)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFileSelect("fldId",fldFilePdfId.ToString(),1).FirstOrDefault();
            if (q != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
        public ActionResult GetRotatingAgent()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblAnavinGhabelCharkheshSelect("","",0).Select(l => new { ID = l.fldId, fldName = l.fldName });
            return this.Store(q);
        }
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
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
                        Session["savePath"] = savePath;
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
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخاب شده غیرمجاز است."
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
                        Message = "فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
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

        public ActionResult UploadPdf()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            try
            {
                if (Session["savePathPdf"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathPdf"].ToString());
                    Session.Remove("savePathPdf");
                    Session.Remove("FileNamePdf");
                    System.IO.File.Delete(physicalPath);
                }
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (extension == ".pdf")
                {
                    if (Request.Files[0].ContentLength <= 2097152)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNamePdf"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                        Session["savePathPdf"] = savePath;
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
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخاب شده غیرمجاز است."
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
                        Message = "فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
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

        public ActionResult ShowPicUpload(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }
        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePath"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                Session.Remove("savePath");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSessionFilePdf()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathPdf"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathPdf"].ToString());
                Session.Remove("savePathPdf");
                Session.Remove("FileNamePdf");
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
        public ActionResult Save(prs_tblActionsSelect Actions, bool DeleteFile, bool DeleteFilePdf)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            string Msg = "", MsgTitle = ""; var Er = 0; byte[] file = null; byte[] filePdf = null; string Pasvand = ""; string NameFile = "", NameFilePdf = ""; var Change = 0;
            bool CheckRepeatedRow = false; string jsonstr = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Actions.fldDesc == null)
                    Actions.fldDesc = "";

                if (Actions.fldId == 0)
                {

                    if (Permission.haveAccess(94, "dbo.tblActions", null))
                    {
                        //ذخیره
                        if (Session["savePath"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePath"].ToString());
                            file = stream.ToArray();
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            Pasvand = "";
                            file = null;
                            NameFile = "";
                        }
                        /*help pdf*/
                        if (Session["savePathPdf"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathPdf"].ToString()));
                            filePdf = stream.ToArray();
                            NameFilePdf = Session["FileNamePdf"].ToString();
                        }
                        else
                        {
                            filePdf = null;
                            NameFilePdf = "";
                        }

                        var Act = m.prs_tblActionsSelect("fldName", Actions.fldName, 0).Where(l => l.fldNoeGhabelCharkheshId == Actions.fldNoeGhabelCharkheshId).FirstOrDefault();
                        if (Act != null) CheckRepeatedRow = true;


                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("نام اقدام", Actions.fldName);
                        parameters.Add("پسوند اقدام", Pasvand);
                        parameters.Add("نام فایل اقدام", NameFile);
                        parameters.Add("وضعیت", Actions.fldActive_DeactiveName);
                        parameters.Add("توضیحات", Actions.fldDesc);
                        parameters.Add("نام انواع قابل چرخش", Actions.fldTitleGhabelCharkhesh);

                        if (CheckRepeatedRow == false)
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_tblActionsInsert(Actions.fldName, Actions.fldDesc, u.UserInputId, file, Pasvand, NameFile, filePdf, NameFilePdf, Actions.fldActive_Deactive, jsonstr, Actions.fldNoeGhabelCharkheshId);
                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "ذخیره موفق";

                        }
                        else
                        {
                            Msg = "نام اقدام وارده شده تکراری است.";
                            MsgTitle = "خطا";
                            Er = 1;

                            parameters.Add("متن خطا", "Actions:Save: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogInsert(u.UserInputId, "dbo.tblActions", jsonstr, false);
                        }
                        if (Session["savePath"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                            Session.Remove("savePath");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }
                        /*help pdf*/
                        if (Session["savePathPdf"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathPdf"].ToString());
                            Session.Remove("savePathPdf");
                            Session.Remove("FileNamePdf");
                            System.IO.File.Delete(physicalPath);
                        }
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

                    if (Permission.haveAccess(95, "dbo.tblActions", Actions.fldId.ToString()))
                    {

                        //ویرایش
                        if (Session["savePath"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePath"].ToString());
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
                            else if (Actions.fldFileId != null)
                            {
                                var Ac = m.prs_tblFileSelect("fldId", Actions.fldFileId.ToString(), 0).FirstOrDefault();

                                if (Ac != null)
                                {
                                    file = Ac.fldFile;
                                    Pasvand = Ac.fldPasvand;
                                    NameFile = Ac.fldFileName;
                                }
                            }
                        }

                        /*help pdf*/
                        if (Session["savePathPdf"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathPdf"].ToString()));
                            filePdf = stream.ToArray();
                            NameFilePdf = Session["FileNamePdf"].ToString();
                        }
                        else
                        {
                            if (DeleteFilePdf == true)
                            {
                                filePdf = null;
                                NameFilePdf = "";
                            }
                            else if (Actions.fldFilePdfId != null)
                            {
                                var Ac = m.prs_tblFileSelect("fldId", Actions.fldFilePdfId.ToString(), 0).FirstOrDefault();

                                if (Ac != null)
                                {
                                    filePdf = Ac.fldFile;
                                    NameFilePdf = Ac.fldFileName;
                                }
                            }
                        }

                        var A = m.prs_tblActionsSelect("fldName", Actions.fldName, 0).Where(l => l.fldNoeGhabelCharkheshId == Actions.fldNoeGhabelCharkheshId).FirstOrDefault();
                        if (A != null && A.fldId != Actions.fldId)
                            CheckRepeatedRow = true;


                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("نام اقدام", Actions.fldName);
                        parameters.Add("پسوند اقدام", Pasvand);
                        parameters.Add("نام فایل اقدام", NameFile);
                        parameters.Add("وضعیت", Actions.fldActive_DeactiveName);
                        parameters.Add("توضیحات", Actions.fldDesc);
                        parameters.Add("نام انواع قابل چرخش", Actions.fldTitleGhabelCharkhesh);

                        if (CheckRepeatedRow == true)
                        {
                            Msg = "نام اقدام وارده شده تکراری است.";
                            MsgTitle = "خطا";
                            Er = 1;

                            parameters.Add("متن خطا", Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblActions", jsonstr, false, Actions.fldId);

                        }
                        else
                        {

                            var s = m.prs_tblActionsUpdate(Actions.fldId, Actions.fldName, Actions.fldFileId, Actions.fldDesc, file, Pasvand, NameFile, Actions.fldFilePdfId, filePdf, NameFilePdf, Actions.fldActive_Deactive, Actions.fldTimeStamp, Actions.fldNoeGhabelCharkheshId, u.UserInputId).FirstOrDefault();


                            if (s.flag == 1)
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                            }
                            else if (s.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (s.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            var Logstatus = true;
                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Actions:Edit: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblActions", jsonstr, Logstatus, Actions.fldId);
                        }


                        if (Session["savePath"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                            Session.Remove("savePath");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }
                        /*help pdf*/
                        if (Session["savePathPdf"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathPdf"].ToString());
                            Session.Remove("savePathPdf");
                            Session.Remove("FileNamePdf");
                            System.IO.File.Delete(physicalPath);
                        }
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
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                /*help pdf*/
                if (Session["savePathPdf"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathPdf"].ToString());
                    Session.Remove("savePathPdf");
                    Session.Remove("FileNamePdf");
                    System.IO.File.Delete(physicalPath);
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نام اقدام", Actions.fldName);
                parameters.Add("پسوند اقدام", Pasvand);
                parameters.Add("نام فایل اقدام", NameFile);
                parameters.Add("نام فایل راهنما", NameFilePdf);
                parameters.Add("وضعیت", Actions.fldActive_DeactiveName);
                parameters.Add("توضیحات", Actions.fldDesc);
                parameters.Add("نام انواع قابل چرخش", Actions.fldTitleGhabelCharkhesh);
                parameters.Add("کد خطا", ErrorId.Value);
                if (Actions.fldId == 0)
                {
                    parameters.Add("متن خطا", "Actions:Save: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogInsert(u.UserInputId, "dbo.tblActions", jsonstr, false);
                }
                else
                {
                    parameters.Add("متن خطا", "Actions:Edit: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogUpdate(u.UserInputId, "dbo.tblActions", jsonstr, false, Actions.fldId);
                }
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1,
                    Change = Change
                });
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er,
                Change = Change
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0;
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                
                var q = m.prs_tblActionsSelect("fldId", id.ToString(),0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نام اقدام", q.fldName);
                parameters.Add("نام فایل اقدام", q.fldFileName);
                parameters.Add("وضعیت", q.fldActive_DeactiveName);
                parameters.Add("توضیحات", q.fldDesc);
                parameters.Add("نام انواع قابل چرخش", q.fldTitleGhabelCharkhesh);
                try
                {//حذف

                    if (Permission.haveAccess(96, "dbo.tblActions", id.ToString()))
                    {
                        //service.DeleteOpertion_Action(id, user.UserKey, IP);
                        var s = m.prs_tblActionsDelete(id, TimeStamp).FirstOrDefault();
                        if (s.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (s.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (s.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }
                        var Logstatus = true;
                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "Actions:Delete: " + Msg);
                            Logstatus = false;
                        }

                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblActions", jsonstr, Logstatus, id);
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Actions:Delete: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(u.UserInputId, "dbo.tblActions", jsonstr, false, id);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        Change = Change
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    Change = Change
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
                var q = m.prs_tblActionsSelect("fldId", Id.ToString(),1).FirstOrDefault();

                var Iconn = "";
                var file = m.prs_tblFileSelect("fldId", q.fldFileId.ToString(),1).FirstOrDefault();
                if (file != null)
                {
                    Iconn = Convert.ToBase64String(file.fldFile);
                }
                var pdf = "";
                var pdfName = "";
                var filepdf = m.prs_tblFileSelect("fldId", q.fldFilePdfId.ToString(),1).FirstOrDefault();
                if (filepdf != null)
                {
                    pdf = Convert.ToBase64String(file.fldFile);
                    pdfName = filepdf.fldFileName;
                }
                return Json(new
                {
                    fldId = q.fldId,
                    fldName = q.fldName,
                    fldDesc = q.fldDesc,
                    fldFileId = q.fldFileId,
                    fldFilePdfId = q.fldFilePdfId,
                    fldActive_Deactive = Convert.ToByte(q.fldActive_Deactive).ToString(),
                    fldIcon = Iconn,
                    pdf = pdf,
                    NameFile = q.fldFileName,
                    pdfName = pdfName,
                    fldTimeStamp = q.fldTimeStamp,
                    fldNoeGhabelCharkheshId = q.fldNoeGhabelCharkheshId.ToString()
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
                if (Permission.haveAccess(93, "tblActions", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblActionsSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblActionsSelect> data1 = null;
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
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDesc";
                                    break;
                                case "fldActive_DeactiveName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldActive_DeactiveName";
                                    break;
                                case "fldTitleGhabelCharkhesh":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitleGhabelCharkhesh";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblActionsSelect(field, searchtext,100).ToList();
                            else
                                data = m.prs_tblActionsSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblActionsSelect("","",100).ToList();
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

                    List<prs_tblActionsSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
