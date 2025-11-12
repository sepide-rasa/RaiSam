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
    public class FinancialValueController : Controller
    {
        //
        // GET: /FinancialValue/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (servic.Permission(44, Session["Username"].ToString(), Session["Password"].ToString(), out Err))
            //{
            ViewData.Model = new RaiSam.Models.FinancialValue();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = this.ViewData
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
        public FileContentResult ShowHelpFinancialValue()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "45", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinFinancialValue()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoFinancialValue()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "45", 1).FirstOrDefault();
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
                PartialView.ViewBag.id = id;//->TreeId
                return PartialView;
            }
        }
      
        //public ActionResult GetTree()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    var q = servic.GetStakeholdersTreeWithFilter("fldTreeId", "", 0, Session["Username"].ToString(), Session["Password"].ToString(), out Err).ToList().Select(n => new { ID = n.fldId, Name = n.fldName });
        //    return this.Store(q);
        //}
        public ActionResult Save(List<prs_tblFinancialValue_DetailSelect> ItemDetail, string Tarikh)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            RaiSamEntities m = new RaiSamEntities();
            try
            {
                var HeaderId=0;
                var k=m.prs_tblFinancialValue_HeaderSelect("fldTarikh",Tarikh,0).FirstOrDefault();
                if(k!=null)
                    HeaderId=k.fldId;
                else{
                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));
                  
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("تاریخ", Tarikh);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_tblFinancialValue_HeaderInsert(Id, Convert.ToInt32(Tarikh.Replace("/", "")), u.UserInputId, jsonstr);
                    HeaderId = Convert.ToInt32(Id.Value);
                }
                foreach (var item in ItemDetail)
                {
                    if (item.fldId == 0)
                    {
                        if (Permission.haveAccess(241, "tblFinancialValue_Header", null))
                        {
                            MsgTitle = "ذخیره موفق";
                            Msg = "ذخیره با موفقیت انجام شد.";

                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نوع واگن", item.fldTypeVagonId);
                            parameters.Add("مبلغ ریالی", item.fldMablaghReyali);
                            parameters.Add("مبلغ ارزی", item.fldMablaghArzi);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblFinancialValue_DetailInsert(HeaderId, item.fldTypeVagonId, item.fldMablaghReyali, item.fldMablaghArzi, u.UserInputId, jsonstr);
                            
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به ذخیره اطلاعات نمی باشد.",
                                MsgTitle = "خطا",
                                Err = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (Permission.haveAccess(242, "tblFinancialValue_Header", item.fldId.ToString()))
                        {
                            MsgTitle = "ویرایش موفق";
                            Msg = "ویرایش با موفقیت انجام شد.";
                            m.prs_tblFinancialValue_DetailUpdate(item.fldId, HeaderId, item.fldTypeVagonId, item.fldMablaghReyali, item.fldMablaghArzi, u.UserInputId);
                            
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به ویرایش اطلاعات نمی باشد.",
                                MsgTitle = "خطا",
                                Err = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
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
            //string Msg = "", MsgTitle = ""; var Er = 0;
            //if (Session["Username"] == null)
            //    return RedirectToAction("Login", "Admin", new { area = "faces" });
            //try
            //{
            //    servic.DeleteItemDynamicRating(TreeId, "fldTreeId", Session["Username"].ToString(), (Session["Password"].ToString()), IP, out Err);
            //    foreach (var item in ItemDynamic)
            //    {
            //        //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 4))
            //        //{
            //        MsgTitle = "ذخیره موفق";
            //        Msg = servic.InsertItemDynamicRating(item.fldTreeId, item.fldDynamicRatingId, item.fldMaxRate, item.fldRateItem,item.fldDesc, Session["Username"].ToString(), (Session["Password"].ToString()), IP, out Err);
            //        //}
            //        //else
            //        //{
            //        //    return null;
            //        //}
            //    }
            //    if (Err.ErrorType)
            //    {
            //        MsgTitle = "خطا";
            //        Msg = Err.ErrorMsg;
            //        Er = 1;
            //    }
            //}
            //catch (Exception x)
            //{
            //    if (x.InnerException != null)
            //        Msg = x.InnerException.Message;
            //    else
            //        Msg = x.Message;

            //    MsgTitle = "خطا";
            //    Er = 1;
            //}
            //return Json(new
            //{
            //    Msg = Msg,
            //    MsgTitle = MsgTitle,
            //    Err = Er
            //}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFinancialValue(int id)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            RaiSamEntities m = new RaiSamEntities();
            //State=1 ->id=کد جدول ,State=2 ->id=کد درخت
            try
            {
                //حذف

                if (Permission.haveAccess(243, "tblFinancialValue_Header", id.ToString()))
                {
                        MsgTitle = "حذف موفق";
                        Msg = "حذف با موفقیت انجام شد.";
                            m.prs_tblFinancialValue_HeaderDelete(id);
                 
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
       
        //public ActionResult Details(int TreeId)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    var q = servic.GetItemDynamicRatingWithFilter("fldTreeId", TreeId.ToString(), 100, Session["Username"].ToString(), (Session["Password"].ToString()), out Err).ToList();
        //    decimal RateItem = 0; int Id = 0; string fldTitleDynamic = ""; decimal MaxRate = 0; string fldDynamicRatingId = ""; string TreeID = ""; string Desc = "";
        //    var ItemDynamicRatingID = "";
        //    if (q.Count != 0)
        //    {
        //        foreach (var item in q)
        //        {
        //            RateItem = item.fldRateItem;
        //            Id = item.fldId;
        //            fldTitleDynamic = fldTitleDynamic + item.fldTitleDynamic + ";";
        //            MaxRate = item.fldMaxRate;
        //            fldDynamicRatingId = fldDynamicRatingId + item.fldDynamicRatingId + ";";
        //            TreeID = item.fldTreeId.ToString();
        //            Desc = item.fldDesc;
        //            ItemDynamicRatingID = ItemDynamicRatingID + item.fldId + ";";
        //        }
        //    }
        //    return Json(new
        //    {
        //        fldGrade = RateItem,
        // 
        //        ItemDynamicRatingID = ItemDynamicRatingID,
        //        fldDesc = Desc
        //    }, JsonRequestBehavior.AllowGet);       fldId = Id,
        //        fldTitleDynamic = fldTitleDynamic,
        //        fldMaxRate = MaxRate,
        //        fldDynamicRatingId = fldDynamicRatingId,
        //        fldTreeId = TreeID.ToString(),
        //}

        public ActionResult ReloadDetail(string Tarikh)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            List<prs_tblFinancialValue_DetailSelect> data = null;
            if (Permission.haveAccess(240, "", "0"))
                data = m.prs_tblFinancialValue_DetailSelect("ListTypeVagon_Tarikh", Tarikh, 0).ToList();
            return Json(data.Select(c => new { fldId = c.fldId, fldTypeVagonId = c.fldTypeVagonId, fldMablaghArzi = c.fldMablaghArzi, fldMablaghReyali = c.fldMablaghReyali, fldTypeVagon = c.fldTypeVagon }), JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Permission.haveAccess(240, "", "0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                List<prs_tblFinancialValue_HeaderSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<prs_tblFinancialValue_HeaderSelect> data1 = null;
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
                            case "fldNameTypeVogon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNameTypeVogon";
                                break;

                        }
                        if (data != null)
                            data1 = m.prs_tblFinancialValue_HeaderSelect(field, searchtext, 0).ToList();
                        else
                            data = m.prs_tblFinancialValue_HeaderSelect(field, searchtext,0).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblFinancialValue_HeaderSelect("", "",0).ToList();
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

                List<prs_tblFinancialValue_HeaderSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
                return null;
        }
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFinancialValue_HeaderSelect("fldId", Id.ToString(), 1).FirstOrDefault();
            return Json(new
            {
                fldTarikh = q.fldTarikh.ToString(),

            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadDetail(string fldHeaderId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<RaiSam.Models.prs_tblFinancialValue_DetailSelect> data = null;
            data = m.prs_tblFinancialValue_DetailSelect("fldHeaderId", fldHeaderId, 0).ToList();
            return this.Store(data);
        }
    }
}
