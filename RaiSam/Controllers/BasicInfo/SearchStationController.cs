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
    public class SearchStationController : Controller
    {
        //
        // GET: /SearchStation/

        public ActionResult IndexChecked(int state,  string Stations, int? headerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.state = state;
            result.ViewBag.Stations = Stations;
            result.ViewBag.headerId = headerId;
            return result;
        }
        public ActionResult ReadChecked(StoreRequestParameters parameters, byte state)
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
                List<Models.prs_tblStationFareeFromBarSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {

                    string field = "";
                    string searchtext = "";
                    List<prs_tblStationFareeFromBarSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldCode":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldCode";
                                break;
                            case "fldNameStation_Nahi":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNameStation_Nahi";
                                break;
                        }

                        if (data != null)
                            data1 = m.prs_tblStationFareeFromBarSelect(field, searchtext, 0).ToList();
                        else
                            data = m.prs_tblStationFareeFromBarSelect(field, searchtext, 0).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);

                }
                else
                {
                    data = m.prs_tblStationFareeFromBarSelect("", "", 0).ToList();
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

                List<prs_tblStationFareeFromBarSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
