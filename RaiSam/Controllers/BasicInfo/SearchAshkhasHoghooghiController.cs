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
    public class SearchAshkhasHoghooghiController : Controller
    {
        //
        // GET: /SearchAshkhasHoghooghi/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(int state, string rowIdx)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.state = state;
            result.ViewBag.rowIdx = rowIdx;
            return result;
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
        public ActionResult Read(StoreRequestParameters parameters, string state)
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
                //if (Permission.haveAccess(56,"","0"))
                //{
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblAshkhasHoghooghiSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblAshkhasHoghooghiSelect> data1 = null;
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
                                case "fldTarikhTasis":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikhTasis";
                                    break;
                                case "fldNationalCode":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNationalCode";
                                    break;

                            }
                            if (data != null)
                            {
                                data1 = m.prs_tblAshkhasHoghooghiSelect(field, searchtext, 100).ToList();
                            }
                            else
                                data = m.prs_tblAshkhasHoghooghiSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblAshkhasHoghooghiSelect("","",100).ToList();
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

                    List<prs_tblAshkhasHoghooghiSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }
        public ActionResult NewWithService(int id)
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
            return PartialView;
        }
    }
}
