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
    public class ItemDynamicPropertiesController : Controller
    {
        //
        // GET: /ItemDynamicProperties/
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

        public ActionResult GetTitleRatingDynamic()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblTitleRatingDynamicSelect("", "", 0).Select(c => new { TitleRatingDynamicID = c.fldId, TitleRatingDynamicName = c.fldTitleFA });
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpItemDynamicProperties()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "47", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinItemDynamicProperties()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoItemDynamicProperties()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "47", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        //public ActionResult New(int id)
        //{//باز شدن پنجره
        //    if (Session["Username"] == null)
        //        return RedirectToAction("Login", "Admin");
        //    else
        //    {
        //        Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //        PartialView.ViewBag.Id = id;
        //        return PartialView;
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckMahdoode(int id, string NameKhasiyat_Fa, string NameKhasiyat_En, string TitleRatingDynamicId)
        {
            string Msg = "", MsgTitle = ""; var Er = 0; var flag = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();

                // var SaveData = servic.GetItemDynamicPropertiesWithFilter("fldStakeholderTreeId", StakeholdersTreeId, 0, Session["UserName"].ToString(), Session["Password"].ToString(), out Err).ToList();
                var F = m.prs_tblItemDynamicPropertiesSelect("fldNameKhasiyat_Fa", NameKhasiyat_Fa,"", 0).Where(l => l.fldId != id).FirstOrDefault();
                var E = m.prs_tblItemDynamicPropertiesSelect("fldNameKhasiyat_En", NameKhasiyat_En,"",  0).Where(l => l.fldId != id).FirstOrDefault();

                if (F == null && E == null)
                {
                    Er = 2;
                }
                else if (F != null && E != null)
                {

                    Msg = "نام فارسی و انگلیسی خاصیت تکراری است.";
                    MsgTitle = "اخطار";
                    flag = 1;
                }
                else if (F != null && E == null)
                {

                    Msg = "نام فارسی خاصیت تکراری است.";
                    MsgTitle = "اخطار";
                    flag = 2;
                }
                else
                {
                    Msg = "نام انگلیسی خاصیت تکراری است.";
                    MsgTitle = "اخطار";
                    flag = 3;
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
                flag = flag

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string Properties1, string TitleRatingDynamicId)
        {

            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                List<prs_tblItemDynamicPropertiesSelect> Properties = JsonConvert.DeserializeObject<List<prs_tblItemDynamicPropertiesSelect>>(Properties1);

                Models.RaiSamEntities m = new RaiSamEntities();
                if (Permission.haveAccess(228, "tblItemDynamicProperties", null))
                {
                    foreach (var item in Properties)
                    {
                        /*var F = servic.GetItemDynamicPropertiesWithFilter("fldNameKhasiyat_Fa", item.fldNameKhasiyat_Fa, 0, Session["UserName"].ToString(), Session["Password"].ToString(), out Err).Where(l => l.fldId != item.fldId).FirstOrDefault();
                        var E = servic.GetItemDynamicPropertiesWithFilter("fldNameKhasiyat_En", item.fldNameKhasiyat_En, 0, Session["UserName"].ToString(), Session["Password"].ToString(), out Err).Where(l => l.fldId != item.fldId).FirstOrDefault();

                    
                        if (F != null && E != null)
                        {

                            Msg = "نام فارسی و انگلیسی خاصیت تکراری است.";
                            MsgTitle = "اخطار";
                            Er = 1;
                        }
                        else if (F != null && E == null)
                        {

                            Msg = "نام فارسی خاصیت تکراری است.";
                            MsgTitle = "اخطار";
                            Er = 1;

                        }
                        else if (F == null && E != null)
                        {
                            Msg = "نام انگلیسی خاصیت تکراری است.";
                            MsgTitle = "اخطار";
                            Er = 1;
                        }*/
                        //else if (F == null && E == null)
                        //{

                          Dictionary<string, object> parameters = new Dictionary<string, object>();

                        parameters.Add("عنوان فارسی", item.fldNameKhasiyat_Fa);
                        parameters.Add("عنوان انگلیسی", item.fldNameKhasiyat_En);
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);


                        if (item.fldId == 0)
                            m.prs_tblItemDynamicPropertiesInsert(Convert.ToInt32(TitleRatingDynamicId), item.fldNameKhasiyat_En, item.fldNameKhasiyat_Fa, Convert.ToByte(item.fldJenseKhasiyat), item.fldType, u.UserInputId, jsonstr);
                        else
                            m.prs_tblItemDynamicPropertiesUpdate(item.fldId, Convert.ToInt32(TitleRatingDynamicId), item.fldNameKhasiyat_En, item.fldNameKhasiyat_Fa, Convert.ToByte(item.fldJenseKhasiyat), item.fldType, u.UserInputId);
                        //}

                        Msg = "ذخیره با موفقیت انجام شد.";
                        MsgTitle = "دخیره موفق";
                    }


                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "شما مجاز به ذخیره اطلاعات نمی باشد.";
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
                Er = 1;
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters, string TitleRatingDynamicId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Permission.haveAccess(228, "", "0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                List<prs_tblItemDynamicPropertiesSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<prs_tblItemDynamicPropertiesSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldNameKhasiyat_En":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNameKhasiyat_En";
                                break;
                        }
                        if (data != null)
                            data1 = m.prs_tblItemDynamicPropertiesSelect(field, searchtext,"", 100).ToList();
                        else
                            data = m.prs_tblItemDynamicPropertiesSelect(field, searchtext, "",  100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblItemDynamicPropertiesSelect("fldTitleDynamicId_admin", TitleRatingDynamicId, "", 100).ToList();
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

                List<prs_tblItemDynamicPropertiesSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
                return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                //حذف

                if (Permission.haveAccess(229, "tblItemDynamicProperties", null))
                {

                Msg = "حذف با موفقیت انجام شد";
                MsgTitle = "حذف موفق";
               m.prs_tblItemDynamicPropertiesDelete(id);
                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "شما مجاز به ذخیره اطلاعات نمی باشد.";
                    Er = 1;
                }
                //}
                //else
                //{
                //    return null;
                //}
               
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

    }
}
