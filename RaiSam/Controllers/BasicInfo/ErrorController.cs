using RaiSam.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.Utilities;
using Ext.Net.MVC;
using System.Data.OleDb;

namespace RaiSam.Controllers.BasicInfo
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

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

        //    string fileName = @"\Uploaded\bari.xlsx";
        //    string connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;" +
        //"Data Source={0};Extended Properties='Excel 12.0;HDR=YES;IMEX=0'", fileName);

        //    using (OleDbConnection cn = new OleDbConnection(connectionString))
        //    {
        //        cn.Open();
        //        OleDbCommand cmd1 = new OleDbCommand("INSERT INTO [Sheet2$] " +
        //             "([Column1],[Column2],[Column3],[Column4]) " +
        //             "VALUES(@value1, @value2, @value3, @value4)", cn);
        //        cmd1.Parameters.AddWithValue("@value1", "Key1");
        //        cmd1.Parameters.AddWithValue("@value2", "Sample1");
        //        cmd1.Parameters.AddWithValue("@value3", 1);
        //        cmd1.Parameters.AddWithValue("@value4", 9);
        //        cmd1.ExecuteNonQuery();
        //    }

            return result;
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpError()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "11", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinError()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoError()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "11", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Permission.haveAccess(69, "tblError", "0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblErrorSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<Models.prs_tblErrorSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;
                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldTarikhShamsi":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTarikhShamsi";
                                break;
                            case "fldName_Family":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldName_Family";
                                break;
                            case "fldMatn":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldMatn";
                                break;
                            case "fldDesc":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldDesc";
                                break;
                        }
                        if (data != null)
                            data1 = m.prs_tblErrorSelect(field, searchtext,100).ToList();
                        else
                            data = m.prs_tblErrorSelect(field, searchtext, 100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblErrorSelect("", "",100).ToList();
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

                List<Models.prs_tblErrorSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
