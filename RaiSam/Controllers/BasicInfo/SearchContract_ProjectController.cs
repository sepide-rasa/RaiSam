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
    public class SearchContract_ProjectController : Controller
    {
        //
        // GET: /SearchContract_Project/

        public ActionResult IndexChecked(int state,string Malek)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.state = state;
            result.ViewBag.Malek = Malek;
            return result;
        }
        public ActionResult ReadChecked(StoreRequestParameters parameters, byte state, string Malek)
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
                List<Models.prs_tblContract_ProjectSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {

                    string field = "";
                    string searchtext = "";
                    List<prs_tblContract_ProjectSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldTitle":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTitle_Malek";
                                break;
                            case "fldNameCompany":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNameCompany_Malek";
                                break;
                            case "fldShomareContract":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldShomareContract_Malek";
                                break;
                            case "fldTarikhContract":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTarikhContract_Malek";
                                break;
                            case "fldShomareMovafeghat":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldShomareMovafeghat_Malek";
                                break;
                            case "fldTarikhMovafeghat":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTarikhMovafeghat_Malek";
                                break;
                            case "fldShomareVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldShomareVagon_Malek";
                                break;
                            case "fldVaznKhali":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldVaznKhali_Malek";
                                break;
                            case "fldTypeVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTypeVagon_Malek";
                                break;
                        }

                        if (data != null)
                            data1 = m.prs_tblContract_ProjectSelect(field, searchtext, Malek, 0, 0).ToList();
                        else
                            data = m.prs_tblContract_ProjectSelect(field, searchtext, Malek, 0, 0).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);

                }
                else
                {
                    data = m.prs_tblContract_ProjectSelect("Malek", "", Malek, 0, 0).ToList();
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

                List<prs_tblContract_ProjectSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
