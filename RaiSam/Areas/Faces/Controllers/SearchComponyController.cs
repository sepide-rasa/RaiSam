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
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace RaiSam.Areas.Faces.Controllers
{
    public class SearchComponyController : Controller
    {
        //
        // GET: /Faces/SearchCompony/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index(int State, string CompanyPersonalId, string Status, string rowIdx, string ReqId, string FirstId)
        {//باز شدن تب جدید
            //if (Session["Username"] == null)
            //    return RedirectToAction("Account", "logon");
            //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 17))
            //{
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Status == "null")
                Status = "";
            if (CompanyPersonalId == "null")
                CompanyPersonalId = "";
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.FristRegisterId = Convert.ToInt32(FirstId);
            PartialView.ViewBag.State = State;
            PartialView.ViewBag.CompanyPersonalId = CompanyPersonalId;
            PartialView.ViewBag.Status = Status;
            PartialView.ViewBag.rowIdx = rowIdx;
            PartialView.ViewBag.ReqId = ReqId;
            return PartialView;
            //}
            //else
            //{
            //    return null;
            //}
        }
        public ActionResult New(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = id;
            return PartialView;
        }
        public ActionResult NewWithService(int id, int FristId, string ReqId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            RaiSamEntities m = new RaiSamEntities();
            PartialView.ViewBag.Id = id;

            var set = m.prs_tblSettingSelect("fldId", "1", 0).FirstOrDefault();
            string FromService = "0";
            if (set.fldCompanyFromService == true)
                FromService = "1";
            PartialView.ViewBag.FromService = FromService;
            PartialView.ViewBag.FristId = FristId;
            PartialView.ViewBag.ReqId = ReqId;
            return PartialView;
        }
        public ActionResult MatnHtmlCompany(string ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            
            var MatnHtmlCompany = "";
            RaiSamEntities m = new RaiSamEntities();
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                var s = m.prs_tblRequestRankingSelect("fldId", ReqId, 0).FirstOrDefault();
                PageHtml = m.prs_tblHtmlViewerSelect("Tree-NameTable", "33", s.fldHadafId, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlCompany = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlCompany = "";
            }

            return Json(new { MatnHtmlCompany = MatnHtmlCompany }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCompanyType()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblCompanyTypeSelect("", "", 0).ToList().Select(c => new { fldComId = c.fldId, fldComName = c.fldName });
            return this.Store(q);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters, string FristRegisterId, string Status)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            RaiSamEntities m = new RaiSamEntities();
            List<prs_tblCompanyProfileSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<prs_tblCompanyProfileSelect> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        case "fldId":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldId";
                            break;
                        case "fldFullName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldFullName";
                            break;
                        case "fldNickName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldNickName";
                            break;
                        case "fldSh_Sabt":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldSh_Sabt";
                            break;
                        case "fldNationalCode":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldNationalCode";
                            break;

                    }
                    if (data != null)
                        data1 = m.prs_tblCompanyProfileSelect(field, searchtext, 100).ToList();
                    else
                        data = m.prs_tblCompanyProfileSelect(field, searchtext, 100).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                if (Status != "")
                    data = m.prs_tblCompanyProfileSelect("", "", 100).Where(l => l.fldFirstRegisterId != Convert.ToInt32(FristRegisterId)).ToList();
                else
                    data = m.prs_tblCompanyProfileSelect("", "", 100).ToList();
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

            List<prs_tblCompanyProfileSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblCompanyProfileSelect c)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (c.fldId == 0)
                {
                    //ذخیره
                    //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 3))
                    //{
                    MsgTitle = "ذخیره موفق";
                    var ss = m.prs_tblCompanyProfileInsert_Header(c.fldNationalCode, c.fldFullName, c.fldSh_Sabt, Convert.ToInt32(c.fldDateSabt.Replace("/", "")), c.fldTypeCompanyId, IP, c.fldFirstRegisterUser).FirstOrDefault();
                    Msg = "ذخیره با موفقیت انجام شد.";

                    if (ss.ErrorCode!=0)
                    {
                        MsgTitle = "خطا";
                        Msg = ss.Error_Msg;
                        Er = 1;
                    }
                    //}
                    //else
                    //{
                    //    return null;
                    //}
                }
                else
                {
                    //ویرایش
                    //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 4))
                    //{
                    MsgTitle = "ویرایش موفق";
                    var ss = m.prs_tblCompanyProfileUpdate_Header(c.fldId, c.fldFullName, c.fldSh_Sabt, Convert.ToInt32(c.fldDateSabt.Replace("/", "")), c.fldTypeCompanyId, IP, c.fldFirstRegisterUser).FirstOrDefault();
                    Msg = "ویرایش با موفقیت انجام شد.";

                    if (ss.ErrorCode != 0)
                    {
                        MsgTitle = "خطا";
                        Msg = ss.Error_Msg;
                        Er = 1;
                    }
                    //}
                    //else
                    //{
                    //    return null;
                    //}
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
        public ActionResult Details(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblCompanyProfileSelect("fldId",Id.ToString(), 0).FirstOrDefault();
            return Json(new
            {
                fldId = q.fldId,
                fldDateSabt = q.fldDateSabt,
                fldTypeCompanyId = q.fldTypeCompanyId.ToString(),
                fldNationalCode = q.fldNationalCode,
                fldFullName = q.fldFullName,
                fldSh_Sabt = q.fldSh_Sabt,
             //   fldDesc = q.fldDesc
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadFromService(int state, string data)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            try
            {
                int? k = null;
                if (state == 1)
                {
                    var q = RaiSam.Areas.Faces.Controllers.CompanyProfileController.GetInquiryByNationalCode(data);
                    if (q != null)
                        return Json(new
                        {
                            fldDateSabt = q.RegisterDate,
                            fldNationalCode = q.NationalCode,
                            fldFullName = q.Name,
                            fldSh_Sabt = q.RegisterNumber,
                            fldDateTasis = q.EstablishmentDate,
                            fldAddress = q.Address,
                            fldCodePosti = q.PostCode,
                            Err = q.Err
                        }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new
                        {
                            fldNationalCode = k,
                            Err = 0
                        }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var q = RaiSam.Areas.Faces.Controllers.CompanyProfileController.GetInquiryByName(data);
                    if (q != null)
                        return Json(new
                        {
                            fldDateSabt = q.RegisterDate,
                            fldNationalCode = q.NationalCode,
                            fldFullName = q.Name,
                            fldSh_Sabt = q.RegisterNumber,
                            fldDateTasis = q.EstablishmentDate,
                            fldAddress = q.Address,
                            fldCodePosti = q.PostCode,
                            Err = q.Err
                        }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new
                        {
                            fldNationalCode = k,
                            Err = 0
                        }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    msgErr = "عدم برقراری ارتباط با سرور",
                    Err = 1
                }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
