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


namespace RaiSam.Controllers.BasicInfo
{
    public class TitleRatingDynamicController : Controller
    {
        //
        // GET: /TitleRatingDynamic/
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

        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpTitleRatingDynamic()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "46", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinTitleRatingDynamic()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoTitleRatingDynamic()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "46", 1).FirstOrDefault();
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

        public ActionResult GetTitleGeneralRating()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblTitleGeneralRatingSelect("", "", 0).ToList().Select(n => new { ID = n.fldId, Name = n.fldTitleFA + "(" + n.fldTitleEN + ")" });
            return this.Store(q);
        }
        public ActionResult GetFormatFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFormatFileSelect("", "", 0).ToList().Select(n => new { ID = n.fldId, Name = n.fldFormatName});
            return this.Store(q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblTitleRatingDynamicSelect T)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            var Change = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (T.fldId == 0)
                {
                    //ذخیره
                    if (Permission.haveAccess(224, "tblTitleRatingDynamic", null))
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();

                        parameters.Add("عنوان فارسی", T.fldTitleFA);
                        parameters.Add("عنوان انگلیسی", T.fldTitleEN);
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                        Msg = "ذخیره با موفقیت انجام شد.";
                        MsgTitle = "دخیره موفق";
                        m.prs_tblTitleRatingDynamicInsert(T.fldTitleFA, T.fldTitleEN, T.fldGeneralRatingId,T.fldFormatFileId, u.UserInputId, jsonstr);

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
                    if (Permission.haveAccess(225, "tblTitleRatingDynamic", T.fldId.ToString()))
                    {
                        var q = m.prs_tblTitleRatingDynamicUpdate(T.fldId, T.fldTitleFA, T.fldTitleEN, T.fldGeneralRatingId, T.fldFormatFileId,u.UserInputId, T.fldTimeStamp).FirstOrDefault();
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
                        parameters.Add("عنوان فارسی", T.fldTitleFA);
                        parameters.Add("عنوان انگلیسی", T.fldTitleEN);

                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "TitleRatingDynamic:Save: " + Msg);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblTitleRatingDynamic", jsonstr, false, T.fldId);
                        }

                        else
                        {
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblTitleRatingDynamic", jsonstr, true, T.fldId);
                        }
                    }
                    else
                    {
                        MsgTitle = "خطا";
                        Msg = "شما مجاز به ویرایش اطلاعات نمی باشد.";
                        Er = 1;
                    }
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
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان فارسی", T.fldTitleFA);
                parameters.Add("عنوان انگلیسی", T.fldTitleEN);
                parameters.Add("کد خطا", ErrorId.Value);
                parameters.Add("متن خطا", "TitleRatingDynamic:Save: " + ErMsg);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (T.fldId == 0)
                {
                    m.prs_LogInsert(u.UserInputId, "dbo.tblTitleRatingDynamic", jsonstr, false);
                }
                else
                {
                    m.prs_LogUpdate(u.UserInputId, "dbo.tblTitleRatingDynamic", jsonstr, false, T.fldId);
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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int TimeStamp)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            string jsonstr = "", title = ""; var Change = 0;
            title = m.prs_tblTitleRatingDynamicSelect("fldId", id.ToString(), 0).FirstOrDefault().fldTitleFA;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("عنوان", title);
            try
            {
                //حذف

                if (Permission.haveAccess(226, "tblTitleRatingDynamic", id.ToString()))
                {
                    var q = m.prs_tblTitleRatingDynamicDelete(id, TimeStamp).FirstOrDefault();

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
                        parameters.Add("متن خطا", "TitleRatingDynamic:Delete: " + Msg);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblTitleRatingDynamic", jsonstr, false, id);
                    }

                    else
                    {
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblTitleRatingDynamic", jsonstr, true, id);
                    }
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
                var ErMsg = "";
                if (x.InnerException != null)
                    ErMsg = x.InnerException.Message;
                else
                    ErMsg = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                parameters.Add("کد خطا", ErrorId.Value);
                parameters.Add("متن خطا", "TitleRatingDynamic:Delete: " + ErMsg);
                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                m.prs_LogDelete(u.UserInputId, "dbo.tblTitleRatingDynamic", jsonstr, false, id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblTitleRatingDynamicSelect("fldId",Id.ToString(), 0).FirstOrDefault();
            return Json(new
            {
                fldId = q.fldId,
                fldTitleFA = q.fldTitleFA,
                fldTitleEN = q.fldTitleEN,
                fldTimeStamp = q.fldTimeStamp,
                fldGeneralRatingId = q.fldGeneralRatingId.ToString(),
                fldFormatFileId = q.fldFormatFileId.ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Permission.haveAccess(223, "", "0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                List<prs_tblTitleRatingDynamicSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<prs_tblTitleRatingDynamicSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldTitleFA":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTitleFA";
                                break;
                            case "fldTitleEN":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTitleEN";
                                break;
                            case "fldTitleGeneral":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTitleGeneral";
                                break;
                            case "fldFormatName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldFormatName";
                                break;
                                
                        }
                        if (data != null)
                            data1 = m.prs_tblTitleRatingDynamicSelect(field, searchtext, 100).ToList();
                        else
                            data = m.prs_tblTitleRatingDynamicSelect(field, searchtext, 100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblTitleRatingDynamicSelect("", "", 100).ToList();
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

                List<prs_tblTitleRatingDynamicSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
                return null;
        }
        public ActionResult DetailsTitleGeneralRating()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblTitleGeneralRatingSelect("", "", 1).FirstOrDefault();
            return Json(new
            {
                fldGeneralRatingId = q.fldId.ToString(),

            }, JsonRequestBehavior.AllowGet);
        }

    }
}
