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
    public class SearchVagonController : Controller
    {
        //
        // GET: /SearchVagon/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.state = state;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters, byte state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(177, "", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblVagonSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblVagonSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTypeVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTypeVagon";
                                    break;
                                case "fldNameCompany":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameCompany";
                                    break;
                                case "fldShomareVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldShomareVagon";
                                    break;
                                case "fldKarbariVagonName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldKarbariVagonName";
                                    break;
                                case "fldToolVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldToolVagon";
                                    break;
                                case "fldVaznKhali":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldVaznKhali";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblVagonSelect(field, searchtext,"","", 100).ToList();
                            else
                                data = m.prs_tblVagonSelect(field, searchtext, "", "", 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblVagonSelect("", "", "", "", 100).ToList();
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

                    List<prs_tblVagonSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult IndexChecked(int state, string Malek, string Pro,string Vagons,int? headerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.state = state;
            result.ViewBag.Malek = Malek;
            result.ViewBag.Pro = Pro;
            result.ViewBag.Vagons = Vagons;
            result.ViewBag.headerId = headerId;
            return result;
        }
        public ActionResult ReadChecked(StoreRequestParameters parameters, byte state, string Malek, string Pro)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                //if (Permission.haveAccess(17, "", "0"))
                //{
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblVagonSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {

                    string field = "";
                    string searchtext = "";
                    List<prs_tblVagonSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId_Malek_Proj";
                                break;
                            case "fldTypeVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTypeVagon_Malek_Proj";
                                break;
                            case "fldNameCompany":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNameCompany_Malek_Proj";
                                break;
                            case "fldShomareVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldShomareVagon_Malek_Proj";
                                break;
                            case "fldKarbariVagonName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldKarbariVagonName_Malek_Proj";
                                break;
                            case "fldToolVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldToolVagon_Malek_Proj";
                                break;
                            case "fldVaznKhali":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldVaznKhali_Malek_Proj";
                                break;
                        }

                        if (data != null)
                            data1 = m.prs_tblVagonSelect(field, searchtext, Malek,Pro, 0).ToList();
                        else
                            data = m.prs_tblVagonSelect(field, searchtext, Malek, Pro, 0).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);

                }
                else
                {
                    data = m.prs_tblVagonSelect("Malek_Proj", "", Malek, Pro, 0).ToList();
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

                List<prs_tblVagonSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }
    }
}
