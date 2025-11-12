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

namespace RaiSam.Controllers.CRM
{
    public class ShowVersionController : Controller
    {
        //
        // GET: /ShowVersion/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult New(int NatijeId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.NatijeId = NatijeId;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblShowVersionSelect ShowVersion, bool DeleteFileVer)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0; byte[] File = null; string Passvand = "", FileName = "";
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblNatijeErjaSelect("fldId", ShowVersion.fldNatijeErjaId.ToString(),0).FirstOrDefault();
            try
            {

                if (Request.IsAjaxRequest() == false)
                {
                    return null;
                }
                else
                {
                    if (ShowVersion.fldId == 0)
                    {//ذخیره
                        if (Permission.haveAccess(82, "tblShowVersion", null))
                        {
                            if (Session["savePathShowVersion"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathShowVersion"].ToString()));
                                File = stream.ToArray();
                                Passvand = Path.GetExtension(Session["savePathShowVersion"].ToString());
                                FileName = Session["FileNameShowVersion"].ToString();
                            }
                            else
                            {
                                Passvand = "";
                                File = null;
                                FileName = "";
                            }

                            Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                            parameters1.Add("شرح ورژن", ShowVersion.fldSharhVersion);
                            parameters1.Add("شرح نتیجه", q.fldSharh);
                            parameters1.Add("نتیجه ارجاع", q.fldNatijeName);
                            string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);


                            m.prs_tblShowVersionInsert(ShowVersion.fldNatijeErjaId, ShowVersion.fldSharhVersion, File, Passvand, FileName, user.UserInputId, jsonstr1);

                            if (Session["savePathShowVersion"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathShowVersion"].ToString());
                                Session.Remove("savePathShowVersion");
                                Session.Remove("FileNameShowVersion");
                                System.IO.File.Delete(physicalPath);
                            }

                            Msg = "ذخیره با موفقیت انجام شد.";
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
                        if (Permission.haveAccess(83, "tblShowVersion", ShowVersion.fldId.ToString()))
                        {
                            if (Session["savePathShowVersion"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathShowVersion"].ToString()));
                                File = stream.ToArray();
                                Passvand = Path.GetExtension(Session["savePathShowVersion"].ToString());
                                FileName = Session["FileNameShowVersion"].ToString();
                            }
                            else
                            {
                                if (DeleteFileVer == true)
                                {
                                    File = null;
                                    Passvand = "";
                                    FileName = "";
                                }
                                else
                                {
                                    var pic = m.prs_tblFileSelect("fldId", ShowVersion.fldFileId.ToString(), 1).FirstOrDefault();
                                    if (pic != null && pic.fldFile != null)
                                    {
                                        File = pic.fldFile;
                                        Passvand = pic.fldPasvand;
                                        FileName = pic.fldFileName;
                                    }
                                }
                            }
                            var q1 = m.prs_tblShowVersionUpdate(ShowVersion.fldId, ShowVersion.fldNatijeErjaId, ShowVersion.fldSharhVersion, ShowVersion.fldFileId, File, Passvand, FileName, user.UserInputId, ShowVersion.fldTimeStamp).FirstOrDefault();

                            if (Session["savePathShowVersion"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathShowVersion"].ToString());
                                Session.Remove("savePathShowVersion");
                                Session.Remove("FileNameShowVersion");
                                System.IO.File.Delete(physicalPath);
                            }


                            if (q1.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                            }
                            else if (q1.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q1.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("شرح ورژن", ShowVersion.fldSharhVersion);
                            parameters.Add("شرح نتیجه", q.fldSharh);
                            parameters.Add("نتیجه ارجاع", q.fldNatijeName);


                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "ShowVersion:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblShowVersion", jsonstr, false, ShowVersion.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblShowVersion", jsonstr, true, ShowVersion.fldId);
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
                if (Session["savePathShowVersion"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathShowVersion"].ToString());
                    Session.Remove("savePathShowVersion");
                    Session.Remove("FileNameShowVersion");
                    System.IO.File.Delete(physicalPath);
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("شرح ورژن", ShowVersion.fldSharhVersion);
                parameters.Add("شرح نتیجه", q.fldSharh);
                parameters.Add("نتیجه ارجاع", q.fldNatijeName);
                parameters.Add("متن خطا", "ShowVersion:Save: " + InnerException);
                parameters.Add("کد خطا",ErrorId.Value);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (ShowVersion.fldId == 0)
                {
                    m.prs_LogInsert(user.UserInputId, "crm.tblShowVersion", jsonstr, false);
                }
                else
                {
                    m.prs_LogUpdate(user.UserInputId, "crm.tblShowVersion", jsonstr, false, ShowVersion.fldId);
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
        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathShowVersion"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathShowVersion"].ToString());
                Session.Remove("savePathShowVersion");
                Session.Remove("FileNameShowVersion");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            string Msg = "";
            try
            {
                if (Session["savePathShowVersion"] != null)
                {
                    System.IO.File.Delete(Session["savePathShowVersion"].ToString());
                    Session.Remove("savePathShowVersion");
                    Session.Remove("FileNameShowVersion");
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var IsWord = FileInfoExtensions.IsWord(Request.Files[0]);
                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                var IsExcel = FileInfoExtensions.IsExcel(Request.Files[0]);
                var IsPowerPoint = FileInfoExtensions.IsPowerPoint(Request.Files[0]);

                if (IsWord == true || IsPDF == true || IsExcel == true || IsPowerPoint == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff"*/)
                {
                    if (Request.Files[0].ContentLength <= 31457280)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathShowVersion"] = savePath;
                        Session["FileNameShowVersion"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
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
                        //    Message = "حجم فایل انتخابی می بایست کمتر از یک مگابایت باشد."
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
                    Er = 1,
                });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int NatijeErjaId)
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
                var q = m.prs_tblShowVersionSelect("fldNatijeErjaId", NatijeErjaId.ToString(),0).FirstOrDefault();
                if (q != null)
                {
                    return Json(new
                    {
                        fldId = q.fldId,
                        fldSharhVersion = q.fldSharhVersion,
                        fldTimeStamp = q.fldTimeStamp,
                        fldFileId = q.fldFileId,
                        fldFileName = q.fldFileName + q.fldPasvand
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        fldId = 0,
                        fldSharhVersion = "",
                        fldTimeStamp = 0,
                        fldFileId = 0,
                        fldFileName = ""
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
