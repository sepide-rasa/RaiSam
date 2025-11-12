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

namespace RaiSam.Controllers.Ticketing
{
    public class TicketTicketFormatFileController : Controller
    {
        //
        // GET: /TicketFormatFile/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
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
                return RedirectToAction("Logon", "Account");
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpTicketTicketFormatFile()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "40", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinTicketTicketFormatFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoTicketTicketFormatFile()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "40", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblTicketFormatFileSelect Format, bool DeleteFileF)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; byte[] file = null; string NameFile = ""; string Pasvand = ""; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
              
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نام فرمت", Format.fldName);
                            parameters.Add("پسوند", Format.fldPassvand);
                            parameters.Add("نام فایل", Format.fldIconName);

                            if (Format.fldId == 0)
                            {
                                if (Permission.haveAccess(220, "tblTicketFormatFile", null))
                                {
                                    //ذخیره
                                    if (Session["savePathFormat"] != null)
                                    {
                                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathFormat"].ToString()));
                                        Format.fldIcon = stream.ToArray();
                                        Format.fldIconName = Session["FileName"].ToString();
                                    }
                                    else
                                    {
                                        Format.fldIcon = null;
                                        Format.fldIconName = "";
                                        //var Image = Server.MapPath("~/Content/Blank.jpg");
                                        //MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                                        //file = stream.ToArray();
                                    }
                                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                                    var s = m.prs_tblTicketFormatFileInsert(Format.fldName, Format.fldPassvand, Format.fldIcon, Format.fldIconName, user.UserInputId, jsonstr);
                                    if (Session["savePathFormat"] != null)
                                    {
                                        string physicalPath = System.IO.Path.Combine(Session["savePathFormat"].ToString());
                                        Session.Remove("savePathFormat");
                                        Session.Remove("FileName");
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
                                if (Permission.haveAccess(221, "tblTicketFormatFile", Format.fldId.ToString()))
                                {
                                    if (Session["savePathFormat"] != null)
                                    {
                                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathFormat"].ToString()));
                                        file = stream.ToArray();
                                        NameFile = Session["FileName"].ToString();
                                    }
                                    else
                                    {
                                        if (DeleteFileF == true)
                                        {
                                            file = null;
                                            NameFile = "";
                                        }
                                        else
                                        {
                                            var TicketFormatFile = m.prs_tblTicketFormatFileSelect("fldId", Format.fldId.ToString(), 0).FirstOrDefault();

                                            if (TicketFormatFile != null)
                                            {
                                                NameFile = TicketFormatFile.fldIconName;
                                                file = TicketFormatFile.fldIcon;
                                            }
                                        }
                                    }
                                    Format.fldIcon = file;
                                    Format.fldIconName = NameFile;
                                    var q = m.prs_tblTicketFormatFileUpdate(Format.fldId, Format.fldName, Format.fldPassvand, Format.fldIcon, Format.fldIconName, user.UserInputId, Format.fldTimeStamp).FirstOrDefault();
                                    if (Session["savePathFormat"] != null)
                                    {
                                        string physicalPath = System.IO.Path.Combine(Session["savePathFormat"].ToString());
                                        Session.Remove("savePathFormat");
                                        Session.Remove("FileName");
                                        System.IO.File.Delete(physicalPath);
                                    }
                                    if (q.flag == 1)
                                    {
                                        Msg = "ویرایش با موفقیت انجام شد";
                                        MsgTitle = "ویرایش موفق";
                                    }
                                    else if (q.flag == 0)
                                    {
                                        Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                        MsgTitle = "هشدار";
                                        Er = 1;
                                        Change = 1;
                                    }
                                    else if (q.flag == 2)
                                    {
                                        Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                        MsgTitle = "خطا";
                                        Er = 1;
                                        Change = 2;
                                    }


                                    if (Er == 1)
                                    {
                                        parameters.Add("متن خطا", "TicketFormatFile:Save: " + Msg);
                                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                        m.prs_LogUpdate(user.UserInputId, "dbo.tblTicketFormatFile", jsonstr, false, Format.fldId);
                                    }

                                    else
                                    {
                                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                        m.prs_LogUpdate(user.UserInputId, "dbo.tblTicketFormatFile", jsonstr, true, Format.fldId);
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
                    string ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                    if (Session["savePathFormat"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePathFormat"].ToString());
                        Session.Remove("savePathFormat");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }
                    
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("عنوان", Format.fldName);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "TicketFormatFile:Save: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (Format.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "dbo.tblTicketFormatFile", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblTicketFormatFile", jsonstr, false, Format.fldId);
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
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "", title = "";
                title = m.prs_tblTicketFormatFileSelect("fldId", id.ToString(), 0).FirstOrDefault().fldName;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نوع واگن", title);
                try
                {
                    if (Permission.haveAccess(222, "tblTicketFormatFile", id.ToString()))
                    {
                        var q = m.prs_tblTicketFormatFileDelete(id, TimeStamp).FirstOrDefault();

                        if (q.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (q.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (q.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }

                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "TicketFormatFile:Delete: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblTicketFormatFile", jsonstr, false, id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblTicketFormatFile", jsonstr, true, id);
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
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "TicketFormatFile:Delete: " + ErMsg);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblTicketFormatFile", jsonstr, false, id);
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
                    Change = Change,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public FileContentResult ShowPic(int Id, string dc)
        {//برگرداندن عکس 
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblTicketFormatFileSelect("fldId", Id.ToString(),0).FirstOrDefault();
            byte[] report_file = null;
            if (Session["savePathFormat"] != null)
            {
                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathFormat"].ToString()));
                report_file = stream.ToArray();
                return File((report_file), ".jpg");
            }
            else if (q != null)
            {
                return File((q.fldIcon), ".jpg");
            }

            var Image = Server.MapPath("~/Content/icon/Blank.jpg");
            MemoryStream stream1 = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
            return File((stream1.ToArray()), ".jpg");

        }
        public ActionResult ShowPicUpload(string dc)
        {//برگرداندن عکس 
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathFormat"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }
        public void DeltePic()
        {//حذف عکس 
            Session["savePathFormat"] = null;
        }
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
            string Msg = "";
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePathFormat"] != null)
                {
                    System.IO.File.Delete(Session["savePathFormat"].ToString());
                    Session.Remove("savePathFormat");
                    Session.Remove("FileName");
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                if (IsImage == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 307200)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["FileName"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                        Session["savePathFormat"] = savePath;
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
                            Message = "حجم فایل انتخابی می بایست کمتر از 300 کیلوبایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));

                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از 300 کیلوبایت باشد."
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
                string ErMsg = "";
                if (x.InnerException != null)
                    ErMsg = x.InnerException.Message;
                else
                    ErMsg = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
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
        public ActionResult Details(int Id)
            {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblTicketFormatFileSelect("fldId", Id.ToString(), 0).FirstOrDefault();
                var Iconn = "";
                if (q.fldIcon != null)
                {
                    Iconn = Convert.ToBase64String(q.fldIcon);
                }
                return Json(new
                {
                   fldId = q.fldId,
                    fldTitle = q.fldName,
                    fldPassvand = q.fldPassvand,
                    fldIcon = Iconn,
                    NameFile = q.fldIconName,
                    fldTimeStamp = q.fldTimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account");
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(219, "", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    List<prs_tblTicketFormatFileSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblTicketFormatFileSelect> data1 = null;
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
                                case "fldPassvand":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldPassvand";
                                    break;
                                case "fldSize":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldSize";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblTicketFormatFileSelect(field, searchtext, 100).ToList();
                            else
                                data = m.prs_tblTicketFormatFileSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblTicketFormatFileSelect("", "", 100).ToList();
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

                    List<prs_tblTicketFormatFileSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
