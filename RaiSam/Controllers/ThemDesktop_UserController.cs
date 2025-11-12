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

namespace RaiSam.Controllers
{
    public class ThemDesktop_UserController : Controller
    {
        //
        // GET: /ThemDesktop_User/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            var userInf = (UserInfo)(Session["User"]);
            PartialView.ViewBag.UserId = userInf.UserSecondId;
            return PartialView;
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
        public FileContentResult ShowHelpThemDesktop_User()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "28", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinThemDesktop()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoThemDesktop()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "28", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
           
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowPic(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathDesktop"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            try
            {
                if (Session["savePathDesktop"] != null)
                {
                    System.IO.File.Delete(Session["savePathDesktop"].ToString());
                    Session.Remove("FileName");
                    Session.Remove("savePathDesktop");
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    if (Request.Files[0].ContentLength <= 10485760)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathDesktop"] = savePath;
                        Session["FileName"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
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
                            Message = "حجم فایل انتخابی می بایست کمتر از  مگابایت باشد.",
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
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                Models.RaiSamEntities m = new RaiSamEntities();
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Err = 1
                });
            }
        }

        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathDesktop"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathDesktop"].ToString());
                Session.Remove("savePathDesktop");
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
        public ActionResult Save(prs_tblThemDesktop_UserSelect them)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0;
                byte[] file = null; string Pasvand = ""; string NameFile = "";
                try
                {
                    if (them.fldDesc == null)
                        them.fldDesc = "";

                    if (them.fldType == 3)
                    {
                        if (Session["savePathDesktop"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathDesktop"].ToString()));
                            file = stream.ToArray();
                            Pasvand = Path.GetExtension(Session["savePathDesktop"].ToString());
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            return Json(new
                            {
                                Er = 1,
                                Msg = "لطفا تصویر پس زمینه را انتخاب نمایید.",
                                MsgTitle = "خطا"
                            }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    m.prs_tblThemDesktop_UserInsert(file,Pasvand,NameFile,them.fldType,user.UserSecondId,them.fldDesc,them.fldThem,user.UserInputId);
                    if (Session["savePathDesktop"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePathDesktop"].ToString());
                        Session.Remove("savePathDesktop");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }

                    Msg = "ذخیره با موفقیت انجام شد.";
                    MsgTitle = "ذخیره موفق";


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
                    if (Session["savePathDesktop"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePathDesktop"].ToString());
                        Session.Remove("savePathDesktop");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Err = 1
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
        public ActionResult Details()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblThemDesktop_UserSelect("fldUserId", user.UserSecondId.ToString(),1).FirstOrDefault();
                byte[] file = null; string FileName = "";
                string pic = "";
                string them = "1";
                string ttype = "1";
                if (q != null)
                {
                    them = q.fldThem.ToString();
                    ttype = q.fldType.ToString();

                    if (q.fldFileDesktopId != null)
                    {
                        var f = m.prs_tblFileDesktop_UserSelect("fldId", q.fldFileDesktopId.ToString(),1).FirstOrDefault();
                        file = f.fldFileDesktop;
                        FileName = f.fldFileName;
                    }

                    if (file != null)
                    {
                        pic = Convert.ToBase64String(file);
                    }
                }
                return new JsonResult()
                {
                    Data = new
                    {
                        fldType = ttype,
                        fldThem = them,
                        NameFile = FileName,
                        pic = pic
                    },
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
               
            }
        }
    }
}
