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

namespace RaiSam.Areas.Faces.Controllers
{
    public class NavaghesController : Controller
    {
        //
        // GET: /Faces/Navaghes/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index()
        {//باز شدن تب جدید
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            return new Ext.Net.MVC.PartialViewResult();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetProject()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblRegistrationFirstContractSelect("Request_Enter", (Session["RankingId"]).ToString(), 0).Select(c => new { Id = c.fldId, Name = c.fldTitle });
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Mosavabat(string containerId)
        {//باز شدن پرونده جدید
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account");
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            // this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadNavaghes(StoreRequestParameters parameters, int fldFirstContractId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblTaeedatSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<prs_tblTaeedatSelect> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        case "fldId":
                            searchtext = ConditionValue.Value.ToString();
                            field = "fldId";
                            break;
                    }
                    if (data != null)
                        data1 = m.prs_tblTaeedatSelect(field, searchtext,"","",0).ToList();
                    else
                        data = m.prs_tblTaeedatSelect(field, searchtext,"","", 0).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_tblTaeedatSelect("fldFirstContractId", fldFirstContractId.ToString(),"","", 0).Where(l=>l.fldType==0).ToList();
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

            List<prs_tblTaeedatSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        public ActionResult Print()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;


        }
        public ActionResult GeneratePDF(int fldFirstContractId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblTaeedatSelectTableAdapter Taeedat = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblTaeedatSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblRegistrationFirstContractSelectTableAdapter Contract = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblRegistrationFirstContractSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter _File = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter Date = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();

                Taeedat.Fill(dt.prs_tblTaeedatSelect, "fldFirstContractId", fldFirstContractId.ToString(), 0);
                _File.Fill(dt.prs_tblSettingSelect, "fldId", "1", 0);
                Contract.Fill(dt.prs_tblRegistrationFirstContractSelect, "fldId", fldFirstContractId.ToString(), 0);
                Date.Fill(dt.prs_GetDate);

                dt.EnforceConstraints = false;

                FastReport.Report Report = new FastReport.Report();
                //var Rp = servic.GetReportWithFilter("fldId", "14", 0, out Err).FirstOrDefault();
                //MemoryStream stream1 = new MemoryStream(Rp.fldFile);
                //Report.Load(stream1);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptNavaghes.frx");

                Report.RegisterData(dt, "dataSet1");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();

                
            Models.RaiSamEntities m = new RaiSamEntities();
                var d = m.prs_GetDate().FirstOrDefault();
                Report.SetParameterValue("Date", d.fldTarikh);
                Report.SetParameterValue("Time", d.fldTime);

                Report.Prepare();
                Report.Export(pdf, stream);
                return File(stream.ToArray(), "application/pdf");
            }
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ShowMsg(string Message, int fldId, int App_Id)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            m.prs_UpdateSeenTaeedat( fldId, true);
            PartialView.ViewBag.Message = Message;
            return PartialView;
        }
        public ActionResult ReadMosavabat(StoreRequestParameters parameters)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                List<Models.prs_tblMosavabatSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<Models.prs_tblMosavabatSelect> data1 = null;
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
                                field = "fldTitle";
                                break;
                            case "fldTitleType":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTitleType";
                                break;

                        }
                        if (data != null)
                            data1 = m.prs_tblMosavabatSelect(field, searchtext, 100).ToList();
                        else
                            data = m.prs_tblMosavabatSelect(field, searchtext, 100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblMosavabatSelect("", "", 100).ToList();
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

                List<Models.prs_tblMosavabatSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);

   
        }
        public FileContentResult DownloadMosavab(int FileId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null && Session["FristRegisterId"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFileSelect("fldId", FileId.ToString(), 1).FirstOrDefault();

            if (q != null && q.fldFile != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
    }
}
