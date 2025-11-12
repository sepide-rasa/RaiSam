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
    public class VersionController : Controller
    {
        //
        // GET: /Version/

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
        public ActionResult HelpVersion()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpVersion()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "26",1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),0).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinVersion()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoVersion()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "26", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 0).FirstOrDefault();
           
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
        public ActionResult Save(Models.prs_tblVersionSelect Version)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (Request.IsAjaxRequest() == false)
                {
                    return null;
                }
                else
                {

                    if (Version.fldId == 0)
                    {//ذخیره
                        if (Permission.haveAccess(85, "tblVersion", null))
                        {
                            Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                            parameters1.Add("ورژن", Version.fldVersion);
                            parameters1.Add("تاریخ ورژن", Version.fldTarikh);
                            parameters1.Add("نیاز به بررسی امنیت", Version.fldNiyazTestAmniyatName);
                            string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);


                            m.prs_tblVersionInsert(MiladiDate.GetMiladiDate(Version.fldTarikh + " " + Version.fldTime), Version.fldVersion, Version.fldNiyazTestAmniyat, user.UserInputId, jsonstr1);


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
                        if (Permission.haveAccess(86, "tblVersion", Version.fldId.ToString()))
                        {
                        var q = m.prs_tblVersionUpdate(Version.fldId, MiladiDate.GetMiladiDate(Version.fldTarikh + " " + Version.fldTime), Version.fldVersion, Version.fldNiyazTestAmniyat, user.UserInputId, Version.fldTimeStamp).FirstOrDefault();


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
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("ورژن", Version.fldVersion);
                        parameters.Add("تاریخ ورژن", Version.fldTarikh);
                        parameters.Add("نیاز به بررسی امنیت", Version.fldNiyazTestAmniyatName);


                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "Version:Save: " + Msg);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(user.UserInputId, "crm.tblVersion", jsonstr, false, Version.fldId);
                        }

                        else
                        {
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(user.UserInputId, "crm.tblVersion", jsonstr, true, Version.fldId);
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

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ورژن", Version.fldVersion);
                parameters.Add("تاریخ ورژن", Version.fldTarikh);
                parameters.Add("نیاز به بررسی امنیت", Version.fldNiyazTestAmniyatName);
                parameters.Add("متن خطا", "Version:Save: " + InnerException);
                parameters.Add("کد خطا", ErrorId.Value);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (Version.fldId == 0)
                {
                    m.prs_LogInsert(user.UserInputId, "crm.tblVersion", jsonstr, false);
                }
                else
                {
                    m.prs_LogUpdate(user.UserInputId, "crm.tblVersion", jsonstr, false, Version.fldId);
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
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0; var checkDel = false;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var Version = m.prs_tblVersionSelect("fldId", id.ToString(),0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ورژن", Version.fldVersion);
                parameters.Add("تاریخ ورژن", Version.fldTarikh);
                parameters.Add("نیاز به بررسی امنیت", Version.fldNiyazTestAmniyatName);
                try
                {//حذف

                    if (Permission.haveAccess(87, "tblVersion", Version.fldId.ToString()))
                    {
                        var Na = m.prs_tblVersionMostanadatSelect("fldVersionId", id.ToString(), 0).FirstOrDefault();
                        if (Na != null)
                            checkDel = true;

                        if (checkDel)
                        {
                            parameters.Add("متن خطا", "Version:Delete: " + "ایتم انتخاب شده قبلا در سیستم استفاده شده است.");
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_LogInsert(user.UserInputId, "crm.tblVersion", jsonstr, false);
                            return Json(new
                            {
                                Msg = "آیتم انتخاب شده قبلا در سیستم استفاده شده است.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                        var q = m.prs_tblVersionDelete(id, TimeStamp).FirstOrDefault();

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
                            parameters.Add("متن خطا", "Version:Delete: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "crm.tblVersion", jsonstr, false, id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "crm.tblVersion", jsonstr, true, id);
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
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");

                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Version:Delete: " + InnerException);

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "crm.tblVersion", jsonstr, false, id);
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblVersionSelect("fldId", Id.ToString(),0).FirstOrDefault();

                return Json(new
                {
                    fldId = q.fldId,
                    fldTarikh = q.fldTarikh,
                    fldTimeStamp = q.fldTimeStamp,
                    fldVersion = q.fldVersion,
                    fldNiyazTestAmniyat = q.fldNiyazTestAmniyat,
                    fldTime = q.fldTime.Replace(":", ""),
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                if (Permission.haveAccess(84,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<prs_tblVersionSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblVersionSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTarikh":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikh";
                                    break;
                                case "fldVersion":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldVersion";
                                    break;
                                case "fldNiyazTestAmniyatName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNiyazTestAmniyatName";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblVersionSelect(field, searchtext,0).ToList();
                            else
                                data = m.prs_tblVersionSelect(field, searchtext, 0).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblVersionSelect("","",0).ToList();
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

                    List<prs_tblVersionSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult VersionMostanadat(int VersionId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.VersionId = VersionId;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadVersionMostanadat(StoreRequestParameters parameters, int VersionId)
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
                if (Permission.haveAccess(88,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<Models.prs_tblVersionMostanadatSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblVersionMostanadatSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldSharhMostanadat":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldSharhMostanadat";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblVersionMostanadatSelect(field, searchtext,0).Where(l => l.fldVersionId == VersionId).ToList();
                            else
                                data = m.prs_tblVersionMostanadatSelect(field, searchtext, 0).Where(l => l.fldVersionId == VersionId).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblVersionMostanadatSelect("fldVersionId", VersionId.ToString(),0).ToList();
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

                    List<prs_tblVersionMostanadatSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathVersion_M"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathVersion_M"].ToString());
                Session.Remove("savePathVersion_M");
                Session.Remove("FileNameVersion_M");
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
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "";
            try
            {
                if (Session["savePathVersion_M"] != null)
                {
                    System.IO.File.Delete(Session["savePathVersion_M"].ToString());
                    Session.Remove("savePathVersion_M");
                    Session.Remove("FileNameVersion_M");
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
                        Session["savePathVersion_M"] = savePath;
                        Session["FileNameVersion_M"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
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
        public ActionResult SaveVersionMostanadat(Models.prs_tblVersionMostanadatSelect VersionM)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0; byte[] File = null; string Passvand = "", FileName = "";
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblVersionSelect("fldId", VersionM.fldVersionId.ToString(), 0).FirstOrDefault();
            try
            {

                if (Request.IsAjaxRequest() == false)
                {
                    return null;
                }
                else
                {
                    if (VersionM.fldId == 0)
                    {//ذخیره
                        if (Permission.haveAccess(89, "tblVersionMostanadat", null))
                        {
                            if (Session["savePathVersion_M"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathVersion_M"].ToString()));
                                File = stream.ToArray();
                                Passvand = Path.GetExtension(Session["savePathVersion_M"].ToString());
                                FileName = Session["FileNameVersion_M"].ToString();
                            }
                            else
                            {
                                return Json(new
                                {
                                    Er = 1,
                                    Msg = "لطفا فایل مورد نظر را وارد نمایید.",
                                    MsgTitle = "خطا",
                                }, JsonRequestBehavior.AllowGet);
                            }

                            Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                            parameters1.Add("ورژن", q.fldVersion);
                            parameters1.Add("شرح مستندات", VersionM.fldSharhMostanadat);
                            parameters1.Add("تاریخ ورژن", q.fldTarikh);
                            string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);


                            m.prs_tblVersionMostanadatInsert(VersionM.fldVersionId, VersionM.fldSharhMostanadat, File, Passvand, FileName, user.UserInputId, jsonstr1);

                            if (Session["savePathVersion_M"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathVersion_M"].ToString());
                                Session.Remove("savePathVersion_M");
                                Session.Remove("FileNameVersion_M");
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
                        if (Permission.haveAccess(90, "tblVersionMostanadat", VersionM.fldId.ToString()))
                        {
                            if (Session["savePathVersion_M"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathVersion_M"].ToString()));
                                File = stream.ToArray();
                                Passvand = Path.GetExtension(Session["savePathVersion_M"].ToString());
                                FileName = Session["FileNameVersion_M"].ToString();
                            }
                            else
                            {

                                var file = m.prs_tblFileSelect("fldId", VersionM.fldFileId.ToString(), 0).FirstOrDefault();
                                File = file.fldFile;
                                Passvand = file.fldPasvand;
                                FileName = file.fldFileName;
                            }
                            var q1 = m.prs_tblVersionMostanadatUpdate(VersionM.fldId, VersionM.fldVersionId, VersionM.fldFileId, VersionM.fldSharhMostanadat, File, Passvand, FileName, user.UserInputId, VersionM.fldTimestamp).FirstOrDefault();

                            if (Session["savePathVersion_M"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathVersion_M"].ToString());
                                Session.Remove("savePathVersion_M");
                                Session.Remove("FileNameVersion_M");
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
                            parameters.Add("ورژن", q.fldVersion);
                            parameters.Add("شرح مستندات", VersionM.fldSharhMostanadat);
                            parameters.Add("تاریخ ورژن", q.fldTarikh);


                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Version:SaveVersionMostanadat: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblVersionMostanadat", jsonstr, false, VersionM.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblVersionMostanadat", jsonstr, true, VersionM.fldId);
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
                if (Session["savePathVersion_M"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathVersion_M"].ToString());
                    Session.Remove("savePathVersion_M");
                    Session.Remove("FileNameVersion_M");
                    System.IO.File.Delete(physicalPath);
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ورژن", q.fldVersion);
                parameters.Add("شرح مستندات", VersionM.fldSharhMostanadat);
                parameters.Add("تاریخ ورژن", q.fldTarikh);
                parameters.Add("متن خطا", "Version:SaveVersionMostanadat: " + InnerException);
                parameters.Add("کد خطا", ErrorId.Value);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (VersionM.fldId == 0)
                {
                    m.prs_LogInsert(user.UserInputId, "crm.tblVersionMostanadat", jsonstr, false);
                }
                else
                {
                    m.prs_LogUpdate(user.UserInputId, "crm.tblVersionMostanadat", jsonstr, false, VersionM.fldId);
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
        public ActionResult DeleteVersionMostanadat(int id, int TimeStamp)
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var VersionMo = m.prs_tblVersionMostanadatSelect("fldId", id.ToString(),0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ورژن", VersionMo.fldversion);
                parameters.Add("شرح مستندات", VersionMo.fldSharhMostanadat);
                parameters.Add("تاریخ ورژن", VersionMo.fldTarikhVersion);
                try
                {//حذف

                    if (Permission.haveAccess(91, "tblVersionMostanadat", id.ToString()))
                    {
                        
                        var q = m.prs_tblVersionMostanadatDelete(id, TimeStamp).FirstOrDefault();

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
                            parameters.Add("متن خطا", "Version:DeleteVersionMostanadat: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "crm.tblVersionMostanadat", jsonstr, false, id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "crm.tblVersionMostanadat", jsonstr, true, id);
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
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");

                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Version:DeleteVersionMostanadat: " + InnerException);

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "crm.tblVersionMostanadat", jsonstr, false, id);
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
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsVersionMostanadat(int Id)
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
                var q = m.prs_tblVersionMostanadatSelect("fldId", Id.ToString(),0).FirstOrDefault();

                return Json(new
                {
                    fldId = q.fldId,
                    fldSharhMostanadat = q.fldSharhMostanadat,
                    fldTimeStamp = q.fldTimestamp,
                    fldFileId = q.fldFileId,
                    fldFileName = q.fldFileName + q.fldPasvand
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ListMostanad(int VersionId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.VersionId = VersionId;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult DownloadMostanad(int FileIdMostanad)
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFileSelect("fldId", FileIdMostanad.ToString(), 1).FirstOrDefault();
            if (q != null && q.fldFile != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
        public ActionResult DetailVersion(string VersionId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                var setReadOnly = "0";
                if (Permission.haveAccess(97,"","0") && Permission.haveAccess(98,"","0"))
                {
                    setReadOnly = "0";//همه ورژن ها
                }
                else if (Permission.haveAccess(97,"","0"))
                {
                    setReadOnly = "1";//اخرین ورژن
                }
                else if (Permission.haveAccess(98,"","0"))
                {
                    setReadOnly = "0";
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "شما مجاز به مشاهده اطلاعات ورژن ها نمی باشید."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
                PartialView.ViewBag.setReadOnly = setReadOnly;
                PartialView.ViewBag.VersionId = VersionId;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetVersion()
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
                var q = m.prs_tblVersionSelect("","",0).Select(l => new { ID = l.fldId, Version = l.fldVersion });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadDetailVersion(StoreRequestParameters parameters, int VersionId)
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
                List<Models.prs_SelectVersionInformation> data = null;
                data = m.prs_SelectVersionInformation(VersionId).ToList();


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

                List<prs_SelectVersionInformation> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
        }
        public ActionResult GotoShowVersionLogin(int VersionId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.VersionId = VersionId;
                return PartialView;
            }
        }
        [HttpPost]
        public ActionResult DetailsInfoPage(int VersionId)
        {//نمایش اطلاعات جهت رویت کاربر
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)Session["User"];
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                bool SharhShow = false;
                var q = m.prs_SelectVersionInformation(VersionId).FirstOrDefault();
                if (q != null)
                {
                    SharhShow = true;
                }
                var v = m.prs_tblVersionSelect("fldId", VersionId.ToString(),0).FirstOrDefault();
                return new JsonResult()
                {
                    Data = new
                    {
                        fldTarikh = v.fldTarikh + " " + v.fldTime,
                        SharhShow = SharhShow,
                        fldVersion = v.fldVersion,
                        HaveMostanad = v.HaveMostanad,
                    },
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
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
                    Er = 1,
                });
            }
        }
        public ActionResult DescVersion(int VersionId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.VersionId = VersionId;
                return PartialView;
            }
        }
        [HttpPost]
        public ActionResult SaveVersion_Seen(int VersionId, bool Check)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; var Er = 0; var CheckName = "";
            var user = (UserInfo)Session["User"];
            Models.RaiSamEntities m = new RaiSamEntities();
            var v = m.prs_tblVersionSelect("fldId", VersionId.ToString(), 1).FirstOrDefault();

            var U = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 1).FirstOrDefault();
            if (Check == true)
                CheckName = "عدم نمایش در دفعات آتی";
            else
                CheckName = "نمایش در دفعات آتی";
            try
            {


                Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                parameters1.Add("ورژن", v.fldVersion);
                parameters1.Add("تاریخ", m.prs_GetDate().FirstOrDefault().fldTarikh);
                parameters1.Add("نام کاربر", U.fldNamePersonal);
                parameters1.Add("کد ملی", U.fldCodeMeli);
                parameters1.Add("نمایش اطلاعات ورژن", CheckName);
                string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);

                int? Tarikh = null;
                Tarikh = Convert.ToInt32(m.prs_GetDate().FirstOrDefault().fldTarikh.Replace("/", ""));


                var output = m.prs_tblVersion_SeenInsert(user.UserSecondId, Check, Tarikh, VersionId, user.UserInputId, jsonstr1);

                MsgTitle = "ذخیره با موفقیت انجام شد."; ;
                Msg = "ذخیره موفق";
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

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ورژن", v.fldVersion);
                parameters.Add("تاریخ", m.prs_GetDate().FirstOrDefault().fldTarikh);
                parameters.Add("نام کاربر", U.fldNamePersonal);
                parameters.Add("کد ملی", U.fldCodeMeli);
                parameters.Add("نمایش اطلاعات ورژن", CheckName);
                parameters.Add("متن خطا", "Version:SaveVersion_Seen: " + ErMsg);
                parameters.Add("کد خطا", ErrorId.Value);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                m.prs_LogInsert(user.UserInputId, "crm.tblVersion_Seen", jsonstr, false);

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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
