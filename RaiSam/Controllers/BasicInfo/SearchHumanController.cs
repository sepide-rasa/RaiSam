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
    public class SearchHumanController : Controller
    {
        //
        // GET: /SearchHuman/

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
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                if (Permission.haveAccess(17,"","0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<Models.prs_tblAshkhasSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        if (state == 1 || state == 9 || state == 6)
                        {
                            string field = "";
                            string searchtext = "";
                            List<prs_tblAshkhasSelect> data1 = null;
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
                                    case "fldFamily":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFamily";
                                        break;
                                    case "fldFatherName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFatherName";
                                        break;
                                    case "fldCodeMeli":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeMeli";
                                        break;
                                    case "fldCodeEnhesari":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeEnhesari";
                                        break;
                                    case "fldMovazaf":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldMovazaf";
                                        break;
                                    //case "fldDesc":
                                    //    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    //    field = "fldDesc";
                                    //    break;
                                }

                                if (data != null)
                                    data1 = m.prs_tblAshkhasSelect(field, searchtext, "", "", "", 100).ToList();
                                else
                                    data = m.prs_tblAshkhasSelect(field, searchtext, "", "", "", 100).ToList();
                            }
                            if (data != null && data1 != null)
                                data.Intersect(data1);
                        }
                        else if (state == 4 || state == 5 || state == 7)
                        {
                            string field = "";
                            string searchtext = "";
                            List<prs_tblAshkhasSelect> data1 = null;
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
                                        field = "fldName_fldNotPersonal";
                                        break;
                                    case "fldFamily":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFamily_fldNotPersonal";
                                        break;
                                    case "fldFatherName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFatherName_fldNotPersonal";
                                        break;
                                    case "fldCodeMeli":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeMeli_fldNotPersonal";
                                        break;
                                }

                                if (data != null)
                                    data1 = m.prs_tblAshkhasSelect(field, searchtext, "", "", "", 100).ToList();
                                else
                                    data = m.prs_tblAshkhasSelect(field, searchtext, "", "", "", 100).ToList();
                            }
                            if (data != null && data1 != null)
                                data.Intersect(data1);
                        }
                        else
                        {
                            string field = "";
                            string searchtext = "";
                            List<prs_tblAshkhasSelect> data1 = null;
                            foreach (var item in filterHeaders.Conditions)
                            {
                                var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                                switch (item.FilterProperty.Name)
                                {
                                    case "fldId":
                                        searchtext = ConditionValue.Value.ToString();
                                        field = "fldId_UserId";
                                        break;
                                    case "fldName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldName_UserId";
                                        break;
                                    case "fldFamily":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFamily_UserId";
                                        break;
                                    case "fldFatherName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFatherName_UserId";
                                        break;
                                    case "fldCodeMeli":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeMeli_UserId";
                                        break;
                                    case "fldCodeEnhesari":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeEnhesari_UserId";
                                        break;
                                    case "fldMovazaf":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldMovazaf_UserId";
                                        break;
                                    //case "fldDesc":
                                    //    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    //    field = "fldDesc";
                                    //    break;
                                }

                                if (data != null)
                                    data1 = m.prs_tblAshkhasSelect(field, searchtext, user.UserSecondId.ToString(), "", "", 100).ToList();
                                else
                                    data = m.prs_tblAshkhasSelect(field, searchtext, user.UserSecondId.ToString(), "", "", 100).ToList();
                            }
                            if (data != null && data1 != null)
                                data.Intersect(data1);
                        }
                    }
                    else
                    {
                        if (state == 4 || state == 5 || state == 7)
                        {
                            data = m.prs_tblAshkhasSelect("fldNotPersonal", "", "", "", "", 100).ToList();
                        }
                        else if (state == 1 || state == 9 || state == 6)
                        {
                            data = m.prs_tblAshkhasSelect("", "", "", "", "", 100).ToList();
                        }
                        else
                        {
                            data = m.prs_tblAshkhasSelect("UserId", "", user.UserSecondId.ToString(), "", "", 100).ToList();
                        }
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

                    List<prs_tblAshkhasSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
