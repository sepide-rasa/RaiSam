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
using Aspose.Cells;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Net.Http;
using System.Text;
using System.Web.Services;



namespace RaiSam.Controllers.Report
{
    public class RptBarnameController : Controller
    {
        //
        // GET: /RptBarname/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
           
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult NewIndex(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            ViewData.Model = new RptBarname();
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = ViewData
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();

            if (Session["FristRegisterId"] != null)
                partial.ViewBag.IsMalek = 1;

            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_GetDate().FirstOrDefault();
            partial.ViewBag.fldTarikh = q.fldDateTime.ToString("yyyy-MM-dd");
            partial.ViewBag.fldTarikh_S = q.fldTarikh;
            int currMounth = Convert.ToInt32(q.fldTarikh.Split('/')[1]);
            int currYear = Convert.ToInt32(q.fldTarikh.Split('/')[0]);
            int TaMah = currMounth - 1;
            int TaSal = currYear;
            if (TaMah == 0)
            {
                TaMah = 12;
                TaSal = TaSal - 1;
            }
            int AzMah = TaMah - 1;
            int AzSal = TaSal;
            if (AzMah == 0)
            {
                AzMah = 12;
                AzSal = AzSal - 1;
            }
            partial.ViewBag.fldTarikh_az = AzSal.ToString() + "/" + AzMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.fldTarikh_ta = TaSal.ToString() + "/" + TaMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.AzSal = AzSal.ToString();
            partial.ViewBag.TaSal = TaSal.ToString();
            partial.ViewBag.AzMah = AzMah.ToString().PadLeft(2, '0');
            partial.ViewBag.TaMah = TaMah.ToString().PadLeft(2, '0');
                
            return partial;
        }
        public ActionResult IndexLoco(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            ViewData.Model = new RptBarname();
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = ViewData
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();

            if (Session["FristRegisterId"] != null)
                partial.ViewBag.IsMalek = 1;

            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_GetDate().FirstOrDefault();
            partial.ViewBag.fldTarikh = q.fldDateTime.ToString("yyyy-MM-dd");
            partial.ViewBag.fldTarikh_S = q.fldTarikh;
            int currMounth = Convert.ToInt32(q.fldTarikh.Split('/')[1]);
            int currYear = Convert.ToInt32(q.fldTarikh.Split('/')[0]);
            int TaMah = currMounth - 1;
            int TaSal = currYear;
            if (TaMah == 0)
            {
                TaMah = 12;
                TaSal = TaSal - 1;
            }
            int AzMah = TaMah - 1;
            int AzSal = TaSal;
            if (AzMah == 0)
            {
                AzMah = 12;
                AzSal = AzSal - 1;
            }
            partial.ViewBag.fldTarikh_az = AzSal.ToString() + "/" + AzMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.fldTarikh_ta = TaSal.ToString() + "/" + TaMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.AzSal = AzSal.ToString();
            partial.ViewBag.TaSal = TaSal.ToString();
            partial.ViewBag.AzMah = AzMah.ToString().PadLeft(2, '0');
            partial.ViewBag.TaMah = TaMah.ToString().PadLeft(2, '0');

            return partial;
        }
        public ActionResult IndexKoli(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult NewIndexKoli(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new RptBarname();
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = ViewData
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();

            if (Session["FristRegisterId"] != null)
                partial.ViewBag.IsMalek = 1;

            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_GetDate().FirstOrDefault();
            partial.ViewBag.fldTarikh = q.fldDateTime.ToString("yyyy-MM-dd");
            partial.ViewBag.fldTarikh_S = q.fldTarikh;
            int currMounth = Convert.ToInt32(q.fldTarikh.Split('/')[1]);
            int currYear = Convert.ToInt32(q.fldTarikh.Split('/')[0]);
            int TaMah = currMounth - 1;
            int TaSal = currYear;
            if (TaMah == 0)
            {
                TaMah = 12;
                TaSal = TaSal - 1;
            }
            int AzMah = TaMah - 1;
            int AzSal = TaSal;
            if (AzMah == 0)
            {
                AzMah = 12;
                AzSal = AzSal - 1;
            }

            partial.ViewBag.fldTarikh_az = AzSal.ToString() + "/" + AzMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.fldTarikh_ta = TaSal.ToString() + "/" + TaMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.AzSal=AzSal.ToString();
            partial.ViewBag.TaSal = TaSal.ToString();
            partial.ViewBag.AzMah = AzMah.ToString().PadLeft(2, '0');
            partial.ViewBag.TaMah = TaMah.ToString().PadLeft(2, '0');
            return partial;
        }
        public ActionResult GetSal()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);

            List<ClassYear> sh = new List<ClassYear>();
            var q = m.prs_GetDate().FirstOrDefault();
            int fldsal = Convert.ToInt32(q.fldTarikh.Substring(0, 4)) - 10;
            for (int i = fldsal; i < fldsal + 19; i++)
            {
                ClassYear CboSal = new ClassYear();

                CboSal.fldYear = i;
                sh.Add(CboSal);
            }
            return Json(sh, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeneratePDF(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon/*, bool CheckZero*/)
        {

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
               


                
                var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

                var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');

                List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);

                FastReport.Report Report = new FastReport.Report();

                if (TypeVagon == "0")
                {
                    List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
                }

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", 0);
                Report.SetParameterValue("OnlyData", 0);
                Report.Prepare();


                if (cc.Length > 2)
                {
                    for (int i = 1; i < cc.Length - 1; i++)
                        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon,0);
                }

                Report.Export(pdf, stream);
                return File(stream.ToArray(), "application/pdf");


                //FastReport.Export.Xml.XMLExport excel = new FastReport.Export.Xml.XMLExport();
                //MemoryStream stream = new MemoryStream();
                //Report.Prepare();
                //Report.Export(excel, stream);
                //return File(stream.ToArray(), "application/vnd.ms-excel", "form5-1.xls");
            }
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult GenerateXls(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State)
        {

            if (Session["User"] == null)
                return null;
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);




                var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

                var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');

                List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);

                FastReport.Report Report = new FastReport.Report();

                if (TypeVagon == "0")
                {
                    List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
                }

                Report.RegisterData(dt, "raiSamDataSet");
               // FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();


           
                MemoryStream stream = new MemoryStream();
                if (State == 2)
                {
                    xlsExport.DataOnly = true;
                    Report.SetParameterValue("OnlyData", 1);
                }
                else
                    Report.SetParameterValue("OnlyData", 0);
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", 1);
                Report.Prepare();


                if (cc.Length > 2)
                {
                    for (int i = 1; i < cc.Length - 1; i++)
                        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 1);
                }
                Report.Export(xlsExport, stream);

                return File(stream.ToArray(), "xls", "Barname.xls");

               /* Report.Export(xlsExport, stream);

                Workbook workbook = new Workbook(stream);

                if (cc.Length > 2)
                {
                    for (int i = 1; i < cc.Length - 1; i++)
                        workbook = CombineMultipleReports2(workbook, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 1);
 }
                var stream2 = new MemoryStream();
                workbook.Save(stream2, SaveFormat.Excel97To2003);


                return File(stream2.ToArray(), "xls", "Barname.xls");*/

            }
            catch (Exception x)
            {
                return null;
            }
        }
        Workbook CombineMultipleReports2(Workbook workbook, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter(); 
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);


            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);

            if (TypeVagon == "0")
            {
                List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
            }
            else if (TypeVagon == "1")
            {
                ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
            }

            Report.RegisterData(dt, "raiSamDataSet");
            FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();

            FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();

            pdf.EmbeddingFonts = true;
            MemoryStream stream = new MemoryStream();
            Report.SetParameterValue("UserName", q.fldNamePersonal);
            Report.SetParameterValue("AzTarikh", StartDate);
            Report.SetParameterValue("TaTarikh", EndDate);
            Report.SetParameterValue("IsExcel", IsExcel);
            Report.SetParameterValue("OnlyData", 0);
            Report.Prepare();
            Report.Export(xlsExport, stream);

            Workbook workbook2 = new Workbook(stream);
            //Report.Export(xlsExport, stream);
            workbook.Worksheets.Add("Sheet1_Copy");
            workbook.Worksheets[1].Copy(workbook2.Worksheets[0]);

            return workbook;

        }
        FastReport.Report CombineMultipleReports(FastReport.Report Report, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);

            if (TypeVagon == "0")
            {
                List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
            }
            else if (TypeVagon == "1")
            {
                ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
            }

            Report.RegisterData(dt, "raiSamDataSet");
            FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
            pdf.EmbeddingFonts = true;
            MemoryStream stream = new MemoryStream();
            Report.SetParameterValue("UserName", q.fldNamePersonal);
            Report.SetParameterValue("AzTarikh", StartDate);
            Report.SetParameterValue("TaTarikh", EndDate);
            Report.SetParameterValue("IsExcel", IsExcel);
            Report.SetParameterValue("OnlyData", 0);
            Report.Prepare(true);

            return Report;

        }
        public /*Stream*/FileResult CreateExcel(string Checked, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC" };
            int index = 0;
            var StatusCheck = Checked.Split(';');
            var Check = "";
            var fldShomareVagon = ""; var fldShmareBarname = ""; var fldTarikhBarname = ""; var fldSeri = ""; var fldNameMabda = "";
            var fldNameMaghsad = ""; var fldNoeBar = ""; var fldMasaft = ""; var fldVaznMahsob = ""; double? fldVaznVagheii = 0; double? fldTonKilometr = 0;
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

                var ListCompany = m.prs_RptBarname(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();
            foreach (var item in StatusCheck)
            {
                switch (item)
                {
                    case "fldShomareVagon":
                        Check = "شماره سریال واگن";
                        Cell cell = sheet.Cells[alpha[index] + "1"];
                        cell.PutValue(Check);
                        int i = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldShomareVagon = _item.fldShomareVagon;
                            Cell Cell = sheet.Cells[alpha[index] + (i + 2)];
                            Cell.PutValue(fldShomareVagon);
                            i++;
                        }
                        index++;
                        break;
                    case "fldShmareBarname":
                        Check = "شماره بارنامه";
                        Cell cell1 = sheet.Cells[alpha[index] + "1"];
                        cell1.PutValue(Check);
                        int j = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldShmareBarname = _item.fldShmareBarname;
                            Cell Cell = sheet.Cells[alpha[index] + (j + 2)];
                            Cell.PutValue(fldShmareBarname);
                            j++;
                        }
                        index++;
                        break;
                    case "fldTarikhBarname":
                        Check = "تاریخ بارنامه";
                        Cell cell2 = sheet.Cells[alpha[index] + "1"];
                        cell2.PutValue(Check);
                        int k = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTarikhBarname = _item.fldTarikhBarname;
                            Cell Cell = sheet.Cells[alpha[index] + (k + 2)];
                            Cell.PutValue(fldTarikhBarname);
                            k++;
                        }
                        index++;
                        break;
                    case "fldSeri":
                        Check = "سری بارنامه";
                        Cell cell3 = sheet.Cells[alpha[index] + "1"];
                        cell3.PutValue(Check);
                        int q = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldSeri = _item.fldSeri;
                            Cell Cell = sheet.Cells[alpha[index] + (q + 2)];
                            Cell.PutValue(fldSeri);
                            q++;
                        }
                        index++;
                        break;
                    case "fldNameMabda":
                        Check = "ایستگاه مبدا";
                        Cell cell4 = sheet.Cells[alpha[index] + "1"];
                        cell4.PutValue(Check);
                        int w = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldNameMabda = _item.fldNameMabda;
                            Cell Cell = sheet.Cells[alpha[index] + (w + 2)];
                            Cell.PutValue(fldNameMabda);
                            w++;
                        }
                        index++;
                        break;
                    case "fldNameMaghsad":
                        Check = "ایستگاه مقصد";
                        Cell cell6 = sheet.Cells[alpha[index] + "1"];
                        cell6.PutValue(Check);
                        int x = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldNameMaghsad = _item.fldNameMaghsad;
                            Cell Cell = sheet.Cells[alpha[index] + (x + 2)];
                            Cell.PutValue(fldNameMaghsad);
                            x++;
                        }
                        index++;
                        break;
                    case "fldNoeBar":
                        Check = "نوع بار";
                        Cell cell7 = sheet.Cells[alpha[index] + "1"];
                        cell7.PutValue(Check);
                        int y = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldNoeBar = _item.fldNoeBar;
                            Cell Cell = sheet.Cells[alpha[index] + (y + 2)];
                            Cell.PutValue(fldNoeBar);
                            y++;
                        }
                        index++;
                        break;
                    case "fldMasaft":
                        Check = "مسافت";
                        Cell cell8 = sheet.Cells[alpha[index] + "1"];
                        cell8.PutValue(Check);
                        int y2 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldMasaft = _item.fldMasaft;
                            Cell Cell = sheet.Cells[alpha[index] + (y2 + 2)];
                            Cell.PutValue(fldMasaft);
                            y2++;
                        }
                        index++;
                        break;
                    case "fldVaznMahsob":
                        Check = "وزن محسوب(تن)";
                        Cell cell9 = sheet.Cells[alpha[index] + "1"];
                        cell9.PutValue(Check);
                        int y3 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldVaznMahsob = _item.fldVaznMahsob;
                            Cell Cell = sheet.Cells[alpha[index] + (y3 + 2)];
                            Cell.PutValue(fldVaznMahsob);
                            y3++;
                        }
                        index++;
                        break;
                    case "fldVaznVagheii":
                        Check = "وزن واقعی(تن)";
                        Cell cell10 = sheet.Cells[alpha[index] + "1"];
                        cell10.PutValue(Check);
                        int y4 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldVaznVagheii = _item.fldVaznVagheiiINT;
                            Cell Cell = sheet.Cells[alpha[index] + (y4 + 2)];
                            Cell.PutValue(fldVaznVagheii);
                            y4++;
                        }
                        index++;
                        break;
                    case "fldTonKilometr":
                        Check = "تن کیلومتر";
                        Cell cell11 = sheet.Cells[alpha[index] + "1"];
                        cell11.PutValue(Check);
                        int y5 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTonKilometr = _item.fldTonKilometrINT;
                            Cell Cell = sheet.Cells[alpha[index] + (y5 + 2)];
                            Cell.PutValue(fldTonKilometr);
                            y5++;
                        }
                        index++;
                        break;
                }
            }
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Excel97To2003);
            return File(stream.ToArray(), "xls", "BArname.xls");
           // return stream;
        }
        public FileResult CreateExcelMosaferi(string Checked, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC" };
            int index = 0;
            var StatusCheck = Checked.Split(';');
            var Check = "";
            var fldShomareVagon = ""; int? fldTrainNumber = 0; short? fldWagonNumber = 0; short? fldCompartmentNumber = 0; short? fldSeatNumber = 0;
            int? fldTicketNumber = 0; var fldTarikhHarekat = ""; var fldSeirStartStation = ""; var fldSeirEndStation = ""; string fldTotalDistance = ""; double? fldTotalDistance2 = 0; int fldMosafer = 0;
            Workbook wb = new Workbook();
                 
            Worksheet sheet = wb.Worksheets[0];

                var ListCompany = m.prs_RptAmalkardMosaferi(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();
            foreach (var item in StatusCheck)
            {
                switch (item)
                {
                    case "fldShomareVagon":
                        Check = "شماره سریال واگن";
                        Cell cell = sheet.Cells[alpha[index] + "1"];
                        cell.PutValue(Check);
                        int i = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldShomareVagon = _item.fldShomareVagon;
                            Cell Cell = sheet.Cells[alpha[index] + (i + 2)];
                            Cell.PutValue(fldShomareVagon);
                            i++;
                        }
                        index++;
                        break;
                    case "fldTrainNumber":
                        Check = "شماره قطار";
                        Cell cell1 = sheet.Cells[alpha[index] + "1"];
                        cell1.PutValue(Check);
                        int j = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTrainNumber = _item.fldTrainNumber;
                            Cell Cell = sheet.Cells[alpha[index] + (j + 2)];
                            Cell.PutValue(fldTrainNumber);
                            j++;
                        }
                        index++;
                        break;
                    case "fldWagonNumber":
                        Check = "شماره سالن";
                        Cell cell2 = sheet.Cells[alpha[index] + "1"];
                        cell2.PutValue(Check);
                        int k = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldWagonNumber = _item.fldWagonNumber;
                            Cell Cell = sheet.Cells[alpha[index] + (k + 2)];
                            Cell.PutValue(fldWagonNumber);
                            k++;
                        }
                        index++;
                        break;
                    case "fldCompartmentNumber":
                        Check = "شماره کوپه";
                        Cell cell3 = sheet.Cells[alpha[index] + "1"];
                        cell3.PutValue(Check);
                        int q = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldCompartmentNumber = _item.fldCompartmentNumber;
                            Cell Cell = sheet.Cells[alpha[index] + (q + 2)];
                            Cell.PutValue(fldCompartmentNumber);
                            q++;
                        }
                        index++;
                        break;
                    case "fldSeatNumber":
                        Check = "شماره صندلی";
                        Cell cell4 = sheet.Cells[alpha[index] + "1"];
                        cell4.PutValue(Check);
                        int w = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldSeatNumber = _item.fldSeatNumber;
                            Cell Cell = sheet.Cells[alpha[index] + (w + 2)];
                            Cell.PutValue(fldSeatNumber);
                            w++;
                        }
                        index++;
                        break;
                    case "fldTicketNumber":
                        Check = "سریال بلیط";
                        Cell cell6 = sheet.Cells[alpha[index] + "1"];
                        cell6.PutValue(Check);
                        int x = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTicketNumber = _item.fldTicketNumber;
                            Cell Cell = sheet.Cells[alpha[index] + (x + 2)];
                            Cell.PutValue(fldTicketNumber);
                            x++;
                        }
                        index++;
                        break;
                    case "fldTarikhHarekat":
                        Check = "تاریخ حرکت";
                        Cell cell7 = sheet.Cells[alpha[index] + "1"];
                        cell7.PutValue(Check);
                        int y = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTarikhHarekat = _item.fldTarikhHarekat;
                            Cell Cell = sheet.Cells[alpha[index] + (y + 2)];
                            Cell.PutValue(fldTarikhHarekat);
                            y++;
                        }
                        index++;
                        break;
                    case "fldSeirStartStation":
                        Check = "ایستگاه مبدا";
                        Cell cell8 = sheet.Cells[alpha[index] + "1"];
                        cell8.PutValue(Check);
                        int y2 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldSeirStartStation = _item.fldSeirStartStation;
                            Cell Cell = sheet.Cells[alpha[index] + (y2 + 2)];
                            Cell.PutValue(fldSeirStartStation);
                            y2++;
                        }
                        index++;
                        break;
                    case "fldSeirEndStation":
                        Check = "ایستگاه مقصد";
                        Cell cell9 = sheet.Cells[alpha[index] + "1"];
                        cell9.PutValue(Check);
                        int y3 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldSeirEndStation = _item.fldSeirEndStation;
                            Cell Cell = sheet.Cells[alpha[index] + (y3 + 2)];
                            Cell.PutValue(fldSeirEndStation);
                            y3++;
                        }
                        index++;
                        break;
                    case "fldTotalDistance":
                        Check = "مسافت(کیلومتر)";
                        Cell cell10 = sheet.Cells[alpha[index] + "1"];
                        cell10.PutValue(Check);
                        int y4 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTotalDistance = _item.fldTotalDistance;
                            Cell Cell = sheet.Cells[alpha[index] + (y4 + 2)];
                            Cell.PutValue(fldTotalDistance);
                            y4++;
                        }
                        index++;
                        break;
                    case "fldMosafer":
                        Check = "مسافر(نفر)";
                        Cell cell11 = sheet.Cells[alpha[index] + "1"];
                        cell11.PutValue(Check);
                        int y5 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldMosafer = _item.fldMosafer;
                            Cell Cell = sheet.Cells[alpha[index] + (y5 + 2)];
                            Cell.PutValue(fldMosafer);
                            y5++;
                        }
                        index++;
                        break;
                    case "fldTotalDistance2":
                        Check = "نفر کیلومتر مورد تایید";
                        Cell cell12 = sheet.Cells[alpha[index] + "1"];
                        cell12.PutValue(Check);
                        int y6 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTotalDistance2 = _item.fldTotalDistanceINT;
                            Cell Cell = sheet.Cells[alpha[index] + (y6 + 2)];
                            Cell.PutValue(fldTotalDistance2);
                            y6++;
                        }
                        index++;
                        break;
                }
            }
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Excel97To2003);
            return File(stream.ToArray(), "xls", "AmalkardMosaferi.xls");
        }
        public FileResult CreateExcelLoco(string Checked, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC" };
            int index = 0;
            var StatusCheck = Checked.Split(';');
            var Check = "";
            int? fldTrain_NO = 0; var fldTrainDate = ""; var fldTrainKind = ""; var fldTrainMabda = ""; var fldDizel_No = ""; var Wagon_No = "";
            var fldTrainMaghsad = ""; decimal fldBarKhales = 0; decimal? fldMasafat = 0; decimal fldBarNakhales = 0; decimal fldTonKhales=0; decimal fldTonNakhales = 0; 
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];


                var ListCompany = m.prs_RptDizel(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();
            foreach (var item in StatusCheck)
            {
                switch (item)
                {
                    case "fldTrain_NO":
                        Check = "شماره قطار";
                        Cell cell = sheet.Cells[alpha[index] + "1"];
                        cell.PutValue(Check);
                        int i = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTrain_NO = _item.fldTrain_NO;
                            Cell Cell = sheet.Cells[alpha[index] + (i + 2)];
                            Cell.PutValue(fldTrain_NO);
                            i++;
                        }
                        index++;
                        break;
                    case "fldWagon_No":
                        Check = "شماره واگن";
                        Cell cell5 = sheet.Cells[alpha[index] + "1"];
                        cell5.PutValue(Check);
                        int i2 = 0;
                        foreach (var _item in ListCompany)
                        {
                            Wagon_No = _item.fldWagon_No;
                            Cell Cell = sheet.Cells[alpha[index] + (i2 + 2)];
                            Cell.PutValue(Wagon_No);
                            i2++;
                        }
                        index++;
                        break;
                    case "fldTrainDate":
                        Check = "تاریخ تشکیل قطار";
                        Cell cell1 = sheet.Cells[alpha[index] + "1"];
                        cell1.PutValue(Check);
                        int j = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTrainDate = _item.fldTrainDate;
                            Cell Cell = sheet.Cells[alpha[index] + (j + 2)];
                            Cell.PutValue(fldTrainDate);
                            j++;
                        }
                        index++;
                        break;
                    case "fldTrainKind":
                        Check = "نوع قطار";
                        Cell cell2 = sheet.Cells[alpha[index] + "1"];
                        cell2.PutValue(Check);
                        int k = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTrainKind = _item.fldTrainKind;
                            Cell Cell = sheet.Cells[alpha[index] + (k + 2)];
                            Cell.PutValue(fldTrainKind);
                            k++;
                        }
                        index++;
                        break;
                    case "fldDizel_No":
                        Check = "شماره لکوموتیو";
                        Cell cell4 = sheet.Cells[alpha[index] + "1"];
                        cell4.PutValue(Check);
                        int w = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldDizel_No = _item.fldDizel_No;
                            Cell Cell = sheet.Cells[alpha[index] + (w + 2)];
                            Cell.PutValue(fldDizel_No);
                            w++;
                        }
                        index++;
                        break;
                    case "fldTrainMabda":
                        Check = "مبدا";
                        Cell cell3 = sheet.Cells[alpha[index] + "1"];
                        cell3.PutValue(Check);
                        int q = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTrainMabda = _item.fldTrainMabda;
                            Cell Cell = sheet.Cells[alpha[index] + (q + 2)];
                            Cell.PutValue(fldTrainMabda);
                            q++;
                        }
                        index++;
                        break;
                    case "fldTrainMaghsad":
                        Check = "مقصد";
                        Cell cell6 = sheet.Cells[alpha[index] + "1"];
                        cell6.PutValue(Check);
                        int x = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTrainMaghsad = _item.fldTrainMaghsad;
                            Cell Cell = sheet.Cells[alpha[index] + (x + 2)];
                            Cell.PutValue(fldTrainMaghsad);
                            x++;
                        }
                        index++;
                        break;
                    case "fldBarKhales":
                        Check = "تناژ بار خالص";
                        Cell cell7 = sheet.Cells[alpha[index] + "1"];
                        cell7.PutValue(Check);
                        int y = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldBarKhales = _item.fldBarKhales;
                            Cell Cell = sheet.Cells[alpha[index] + (y + 2)];
                            Cell.PutValue(fldBarKhales);
                            y++;
                        }
                        index++;
                        break;
                    case "fldBarNakhales":
                        Check = "تناژ بار ناخالص";
                        Cell cell9 = sheet.Cells[alpha[index] + "1"];
                        cell9.PutValue(Check);
                        int y3 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldBarNakhales = _item.fldBarNakhales;
                            Cell Cell = sheet.Cells[alpha[index] + (y3 + 2)];
                            Cell.PutValue(fldBarNakhales);
                            y3++;
                        }
                        index++;
                        break;
                    case "fldMasafat":
                        Check = "مسافت";
                        Cell cell8 = sheet.Cells[alpha[index] + "1"];
                        cell8.PutValue(Check);
                        int y2 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldMasafat = _item.fldMasafat;
                            Cell Cell = sheet.Cells[alpha[index] + (y2 + 2)];
                            Cell.PutValue(fldMasafat);
                            y2++;
                        }
                        index++;
                        break;
                    case "fldTonKhales":
                        Check = "تن کیلومتر خالص";
                        Cell cell10 = sheet.Cells[alpha[index] + "1"];
                        cell10.PutValue(Check);
                        int y4 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTonKhales = _item.fldTonKhales;
                            Cell Cell = sheet.Cells[alpha[index] + (y4 + 2)];
                            Cell.PutValue(fldTonKhales);
                            y4++;
                        }
                        index++;
                        break;
                    case "fldTonNakhales":
                        Check = "تن کیلومتر ناخالص";
                        Cell cell11 = sheet.Cells[alpha[index] + "1"];
                        cell11.PutValue(Check);
                        int y5 = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTonNakhales = _item.fldTonNakhales;
                            Cell Cell = sheet.Cells[alpha[index] + (y5 + 2)];
                            Cell.PutValue(fldTonNakhales);
                            y5++;
                        }
                        index++;
                        break;
                }
            }
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Excel97To2003);
            return File(stream.ToArray(), "xls", "Loco.xls");
        }
        public ActionResult UploadExcelFileVagon()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                if (Session["savePathExcelVagon"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathExcelVagon"].ToString());
                    Session.Remove("savePathExcelVagon");
                    System.IO.File.Delete(physicalPath);
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var FileNamee = Path.GetFileNameWithoutExtension(Request.Files[0].FileName) + user.UserName + extension;
                if (extension == ".xls" || extension == ".xlsx")
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string savePath = Server.MapPath(@"~\Uploaded\" + FileNamee);
                    file.SaveAs(savePath);
                    Session["savePathExcelVagon"] = savePath;
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده معتبر نمی باشد."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (Session["savePathExcelVagon"] != null)
                {
                    System.IO.File.Delete(Session["savePathExcelVagon"].ToString());
                    Session.Remove("savePathExcelVagon");
                }
                return null;
            }
        }
        public ActionResult ProcessXlsVagon()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    string Msg = "", MsgTitle = ""; var Er = 0;var VagonIds = "";
                    string Vagon = "";
                try
                {
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(Session["savePathExcelVagon"].ToString());
                    for (int i = 1; i < wb.Worksheets[0].Cells.MaxDataRow + 1; i++)
                    {
                        int count = 0;
                        for (int j = wb.Worksheets[0].Cells.MinColumn; j <= wb.Worksheets[0].Cells.MaxDataColumn; j++)
                        {
                            var str = wb.Worksheets[0].Cells[i, j].StringValue.Split(',').Join("");
                            switch (count)
                            {
                                case 0:
                                    Vagon = str ;
                                    break;
                            }
                            count++;
                        }
                        VagonIds = VagonIds + Vagon + ",";
                        Vagon = "";
                    }
                    
                }
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");


                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    VagonIds = VagonIds
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GeneratePDFKoli(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
        {

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

                var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');
                List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);

                FastReport.Report Report = new FastReport.Report();

                if (TypeVagon == "0")
                {
                    List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarnameMosaferi.frx");
                }

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", 0);
                Report.SetParameterValue("OnlyData", 0);
                Report.Prepare();

                if (cc.Length > 2)
                {
                    for (int i = 1; i < cc.Length - 1; i++)
                        Report = CombineMultipleReportsKoli(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon,0);
                }

                Report.Export(pdf, stream);
                return File(stream.ToArray(), "application/pdf");
            }
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult GenerateXlsKoli(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon,int State)
        {

            if (Session["User"] == null)
                return null;
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

                var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');
                List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);

                FastReport.Report Report = new FastReport.Report();

                if (TypeVagon == "0")
                {
                    List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarnameMosaferi.frx");
                }

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();


                MemoryStream stream = new MemoryStream();
                if (State == 2)
                {
                    xlsExport.DataOnly = true;
                    Report.SetParameterValue("OnlyData", 1);
                }
                else
                    Report.SetParameterValue("OnlyData", 0);
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", 1);
                Report.Prepare();

                if (cc.Length > 2)
                {
                    for (int i = 1; i < cc.Length - 1; i++)
                        Report = CombineMultipleReportsKoli(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon,1);
                }

                Report.Export(xlsExport, stream);
                return File(stream.ToArray(), "xls", "BarnameKoli.xls");
            }
            catch (Exception x)
            {
                return null;
            }
        }
        FastReport.Report CombineMultipleReportsKoli(FastReport.Report Report, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);

            if (TypeVagon == "0")
            {
                List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarname.frx");
            }
            else if (TypeVagon == "1")
            {
                ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarnameMosaferi.frx");
            }

            Report.RegisterData(dt, "raiSamDataSet");
            FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
            pdf.EmbeddingFonts = true;
            MemoryStream stream = new MemoryStream();
            Report.SetParameterValue("UserName", q.fldNamePersonal);
            Report.SetParameterValue("AzTarikh", StartDate);
            Report.SetParameterValue("TaTarikh", EndDate);
            Report.SetParameterValue("IsExcel", IsExcel);
            Report.SetParameterValue("OnlyData", 0);
            Report.Prepare(true);

            return Report;

        }
        public FileResult CreateExcelKoli(string Checked, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon/*, bool CheckZero*/)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC" };
            int index = 0;
            var StatusCheck = Checked.Split(';');
            var Check = "";
            var fldShomareVagon = ""; int? fldTedad = 0; string fldSumMasafat = ""; double? fldSumKilometr = 0; double? fldSumTonazhBar = 0;
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

                var ListCompany = m.prs_RptKoliBarname(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();
            foreach (var item in StatusCheck)
            {
                switch (item)
                {
                    case "fldShomareVagon":
                        Check = "شماره سریال واگن";
                        Cell cell = sheet.Cells[alpha[index] + "1"];
                        cell.PutValue(Check);
                        int i = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldShomareVagon = _item.fldShomareVagon;
                            Cell Cell = sheet.Cells[alpha[index] + (i + 2)];
                            Cell.PutValue(fldShomareVagon);
                            i++;
                        }
                        index++;
                        break;
                    case "fldTedad":
                        Check = "تعداد بارنامه";
                        Cell cell1 = sheet.Cells[alpha[index] + "1"];
                        cell1.PutValue(Check);
                        int j = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTedad = _item.fldTedad;
                            Cell Cell = sheet.Cells[alpha[index] + (j + 2)];
                            Cell.PutValue(fldTedad);
                            j++;
                        }
                        index++;
                        break;
                    case "fldSumMasafat":
                        Check = "مجموع مسافت";
                        Cell cell2 = sheet.Cells[alpha[index] + "1"];
                        cell2.PutValue(Check);
                        int k = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldSumMasafat = _item.fldSumMasafat;
                            Cell Cell = sheet.Cells[alpha[index] + (k + 2)];
                            Cell.PutValue(fldSumMasafat);
                            k++;
                        }
                        index++;
                        break;
                    case "fldSumTonazhBar":
                        Check = "مجموع تناژ بار";
                        Cell cell3 = sheet.Cells[alpha[index] + "1"];
                        cell3.PutValue(Check);
                        int q = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldSumTonazhBar = _item.fldSumTonazhBarINT;
                            Cell Cell = sheet.Cells[alpha[index] + (q + 2)];
                            Cell.PutValue(fldSumTonazhBar);
                            q++;
                        }
                        index++;
                        break;
                    case "fldSumKilometr":
                        Check = "مجموع عملکرد مورد تایید";
                        Cell cell4 = sheet.Cells[alpha[index] + "1"];
                        cell4.PutValue(Check);
                        int w = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldSumKilometr = _item.fldSumKilometrINT;
                            Cell Cell = sheet.Cells[alpha[index] + (w + 2)];
                            Cell.PutValue(fldSumKilometr);
                            w++;
                        }
                        index++;
                        break;
                   
                }
            }
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Excel97To2003);
            return File(stream.ToArray(), "xls", "BArnameKoli.xls");
        }
        public FileResult CreateExcelKoliMosaferi(string Checked, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon/*, bool CheckZero*/)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC" };
            int index = 0;
            var StatusCheck = Checked.Split(';');
            var Check = "";
            var fldShomareVagon = ""; int? fldTedadBelit = 0; string fldMasafatTeyShode = "";  double? fldMasafat_Nafar = 0;
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

                var ListCompany = m.prs_RptAmalKardKoliMosaferi(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();
            foreach (var item in StatusCheck)
            {
                switch (item)
                {
                    //    "" + ";" + "" + ";" + "" + ";" + "" + ";" + "" + ";" ;
                    case "fldShomareVagon":
                        Check = "شماره سریال واگن";
                        Cell cell = sheet.Cells[alpha[index] + "1"];
                        cell.PutValue(Check);
                        int i = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldShomareVagon = _item.fldShomareVagon;
                            Cell Cell = sheet.Cells[alpha[index] + (i + 2)];
                            Cell.PutValue(fldShomareVagon);
                            i++;
                        }
                        index++;
                        break;
                    case "fldTedadBelit":
                        Check = "تعداد بلیط های صادر شده";
                        Cell cell1 = sheet.Cells[alpha[index] + "1"];
                        cell1.PutValue(Check);
                        int j = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTedadBelit = _item.fldTedadBelit;
                            Cell Cell = sheet.Cells[alpha[index] + (j + 2)];
                            Cell.PutValue(fldTedadBelit);
                            j++;
                        }
                        index++;
                        break;
                    case "fldMasafatTeyShode":
                        Check = "مجموع مسافت طی شده(کیلومتر)";
                        Cell cell2 = sheet.Cells[alpha[index] + "1"];
                        cell2.PutValue(Check);
                        int k = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldMasafatTeyShode = _item.fldMasafatTeyShode;
                            Cell Cell = sheet.Cells[alpha[index] + (k + 2)];
                            Cell.PutValue(fldMasafatTeyShode);
                            k++;
                        }
                        index++;
                        break;
                    case "fldTedadBelit2":
                        Check = "مجموع تعداد مسافرین(نفر)";
                        Cell cell3 = sheet.Cells[alpha[index] + "1"];
                        cell3.PutValue(Check);
                        int q = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldTedadBelit = _item.fldTedadBelit;
                            Cell Cell = sheet.Cells[alpha[index] + (q + 2)];
                            Cell.PutValue(fldTedadBelit);
                            q++;
                        }
                        index++;
                        break;
                    case "fldMasafat_Nafar":
                        Check = "مجموع عملکرد مورد تایید(نفر کیلومتر)";
                        Cell cell4 = sheet.Cells[alpha[index] + "1"];
                        cell4.PutValue(Check);
                        int w = 0;
                        foreach (var _item in ListCompany)
                        {
                            fldMasafat_Nafar = _item.fldMasafat_NafarINT;
                            Cell Cell = sheet.Cells[alpha[index] + (w + 2)];
                            Cell.PutValue(fldMasafat_Nafar);
                            w++;
                        }
                        index++;
                        break;

                }
            }
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Excel97To2003);
            return File(stream.ToArray(), "xls", "AmalkardMosaferiKoli.xls");
        }
        public ActionResult UploadExcelFileVagonKoli()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                if (Session["savePathExcelVagonKoli"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathExcelVagonKoli"].ToString());
                    Session.Remove("savePathExcelVagonKoli");
                    System.IO.File.Delete(physicalPath);
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var FileNamee = Path.GetFileNameWithoutExtension(Request.Files[0].FileName) + user.UserName + extension;
                if (extension == ".xls" || extension == ".xlsx")
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string savePath = Server.MapPath(@"~\Uploaded\" + FileNamee);
                    file.SaveAs(savePath);
                    Session["savePathExcelVagonKoli"] = savePath;
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده معتبر نمی باشد."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (Session["savePathExcelVagonKoli"] != null)
                {
                    System.IO.File.Delete(Session["savePathExcelVagonKoli"].ToString());
                    Session.Remove("savePathExcelVagonKoli");
                }
                return null;
            }
        }
        public ActionResult ProcessXlsVagonKoli()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0; var VagonIds = "";
                string Vagon = "";
                try
                {
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(Session["savePathExcelVagonKoli"].ToString());
                    for (int i = 1; i < wb.Worksheets[0].Cells.MaxDataRow + 1; i++)
                    {
                        int count = 0;
                        for (int j = wb.Worksheets[0].Cells.MinColumn; j <= wb.Worksheets[0].Cells.MaxDataColumn; j++)
                        {
                            var str = wb.Worksheets[0].Cells[i, j].StringValue.Split(',').Join("");
                            switch (count)
                            {
                                case 0:
                                    Vagon = str;
                                    break;
                            }
                            count++;
                        }
                        VagonIds = VagonIds + Vagon + ",";
                        Vagon = "";
                    }

                }
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");


                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    VagonIds = VagonIds
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReadMalek(StoreRequestParameters parameters)
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
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblMalek_VagonInfoSelect> data = null;

                var FirstId = "";
                var fieldName = "";
                if (Session["FristRegisterId"] != null)
                {
                    FirstId = Session["FristRegisterId"].ToString();
                    fieldName = "fldFirstRegisterUser";
                }

                data = m.prs_tblMalek_VagonInfoSelect(fieldName, FirstId, 0).ToList();

                return this.Store(data, data.Count);

            }
        }
        public ActionResult ReadContract(StoreRequestParameters parameters, string MalekId, string TypeVagon)
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
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblContract_ProjectSelect> data = null;

                var FirstId = "";
                var fieldName = "Malek";
                if (Session["FristRegisterId"] != null)
                {
                    FirstId = Session["FristRegisterId"].ToString();
                    fieldName = "fldFirstRegisterUser_Malek";
                }

                data = m.prs_tblContract_ProjectSelect(fieldName, FirstId, MalekId, Convert.ToByte(TypeVagon), 0).ToList();

                return this.Store(data, data.Count);
            }
        }
        public ActionResult ReadVagon(StoreRequestParameters parameters, string Projj, string azTarikh, string tatarikh, string TypeVagon)
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
                Models.RaiSamEntities m = new RaiSamEntities();
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                
                List<Models.prs_SelectVagonForRptBarname> data = null;

                var FirstId = 0;
               
               

                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<prs_SelectVagonForRptBarname> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldShomareVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldShomareVagon";
                                break;
                            case "fldKarbariVagonName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldKarbariVagonName";
                                break;
                            case "fldNameCompany":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNameCompany";
                                break;
                            case "fldToolVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldToolVagon";
                                break;
                            case "fldTypeVagon":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTypeVagon";
                                break;
                            case "fldVaznKhali":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldVaznKhali";
                                break;
                            case "fldZarfiyatBargiri":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldZarfiyatBargiri";
                                break;

                        }
                        if (Session["FristRegisterId"] != null)
                        {
                            FirstId = Convert.ToInt32(Session["FristRegisterId"]);
                            field = "fldFirstRegisterUser_" + field;
                        }
                        if (data != null)
                            data1 = m.prs_SelectVagonForRptBarname(field, Projj, searchtext, azTarikh, tatarikh, Convert.ToByte(TypeVagon), FirstId,500).ToList();
                        else
                            data = m.prs_SelectVagonForRptBarname(field, Projj, searchtext, azTarikh, tatarikh, Convert.ToByte(TypeVagon), FirstId,500).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                    {
                        var field2 = "";
                        if (Session["FristRegisterId"] != null)
                        {
                            FirstId = Convert.ToInt32(Session["FristRegisterId"]);
                             field2="fldFirstRegisterUser";
                        }
                        data = m.prs_SelectVagonForRptBarname(field2, Projj, "", azTarikh, tatarikh, Convert.ToByte(TypeVagon), FirstId, 500).ToList();
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

                    List<prs_SelectVagonForRptBarname> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
            }
        }
        public ActionResult ReadVagonsDetail(string azTarikh, string tatarikh, int? fldHeaderId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<RaiSam.Models.prs_SelectContractDetailLastVagon> data = null;
            data = m.prs_SelectContractDetailLastVagon(azTarikh,tatarikh, fldHeaderId).ToList();
            return this.Store(data);
        }
        public ActionResult PrintPage(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon/*, bool CheckZero*/)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            partial.ViewBag.StartDate = StartDate;
            partial.ViewBag.EndDate = EndDate;
            partial.ViewBag.MalekId = MalekId;
            partial.ViewBag.ContractId = ContractId;
            partial.ViewBag.ShVagon = ShVagon;
            partial.ViewBag.AzVagon = AzVagon;
            partial.ViewBag.TaVagon = TaVagon;
            partial.ViewBag.TypeVagon = TypeVagon;
            /*partial.ViewBag.CheckZero = CheckZero;*/
            
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult PrintPageKoli(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            partial.ViewBag.StartDate = StartDate;
            partial.ViewBag.EndDate = EndDate;
            partial.ViewBag.MalekId = MalekId;
            partial.ViewBag.ContractId = ContractId;
            partial.ViewBag.ShVagon = ShVagon;
            partial.ViewBag.AzVagon = AzVagon;
            partial.ViewBag.TaVagon = TaVagon;
            partial.ViewBag.TypeVagon = TypeVagon;
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult HelpBarname()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpBarname()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "37", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinBarname()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoBarname()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "37", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult HelpBarnameKoli()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpBarnameKoli()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "38", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinBarnameKoli()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoBarnameKoli()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "38", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult ShowPageKoli(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            ViewData.Model = new RptBarname();
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = ViewData
            };

            partial.ViewBag.StartDate = StartDate;
            partial.ViewBag.EndDate = EndDate;
            partial.ViewBag.MalekId = MalekId;
            partial.ViewBag.ContractId = ContractId;
            partial.ViewBag.ShVagon = ShVagon;
            partial.ViewBag.AzVagon = AzVagon;
            partial.ViewBag.TaVagon = TaVagon;
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult ReadRptKoli(StoreRequestParameters parameters, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon/*, bool CheckZero*/)
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
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_RptKoliBarname> data = null;

                data = m.prs_RptKoliBarname(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();

                return this.Store(data, data.Count);
            }
        }
        public ActionResult ReadRptKoliDetail(string Barnameha)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<RaiSam.Models.prs_SelectBarnameDetail> data = null;
            data = m.prs_SelectBarnameDetail(Barnameha).ToList();
            return this.Store(data);
        }
        public ActionResult ShowPageKoliMosaferi(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            ViewData.Model = new RptBarname();
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = ViewData
            };

            partial.ViewBag.StartDate = StartDate;
            partial.ViewBag.EndDate = EndDate;
            partial.ViewBag.MalekId = MalekId;
            partial.ViewBag.ContractId = ContractId;
            partial.ViewBag.ShVagon = ShVagon;
            partial.ViewBag.AzVagon = AzVagon;
            partial.ViewBag.TaVagon = TaVagon;
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult ReadRptKoliMosaferi(StoreRequestParameters parameters, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon/*, bool CheckZero*/)
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
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_RptAmalKardKoliMosaferi> data = null;

                data = m.prs_RptAmalKardKoliMosaferi(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();

                return this.Store(data, data.Count);
            }
        }
        public ActionResult ReadRptKoliDetailMosaferi(string fldVagonId, string StartDate, string EndDate)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<RaiSam.Models.prs_SelectMosaferiDetail> data = null;
            data = m.prs_SelectMosaferiDetail(Convert.ToInt32(fldVagonId), StartDate, EndDate).ToList();
            return this.Store(data);
        }
        public Output_Mosaferi GetMosaferi(string FromDate, string ToDate, string Owner)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            Output_Mosaferi MosaferiInfo = new Output_Mosaferi();
            var ErMsg = 0;
            try
            {

                RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir/");
                Models.User AuthorizeUser = new Models.User();
                AuthorizeUser.UserName = "Made12";
                AuthorizeUser.Password = "Made12.IT.14011126.583.I4@a540KGAa5m50oA34%pCx42D";
                Input_Mosaferi InputDto = new Input_Mosaferi();
                InputDto.FromDate = FromDate;
                InputDto.ToDate = ToDate;
                InputDto.Owner = Owner;
                ErMsg = 1;
                var r = RaiClient.GetMosaferi(AuthorizeUser, InputDto, new CancellationToken());
                ErMsg = 2;

                //foreach (var item in r.Result.Data.data)
                //m.prs_MosaferiInfoInsert(item.uicWagonNo, item.trainNumber, item.trainDate, item.moveTime, item.persianTrainDate, item.wagonNumber, item.compartmentNumber, item.seatNumber, item.ticketNumber, item.seirStartStationCode, item.seirStartStation, item.seirEndStationCode, item.seirEndStation, item.nameCompany, item.totalDistance);

                if (r.Result.IsSuccess)
                {
                    ErMsg = 4;
                    MosaferiInfo = r.Result.Data;
                    MosaferiInfo.Err = 0;
                }
                else
                {
                    ErMsg = 5;
                    MosaferiInfo.Err = 1;
                    MosaferiInfo.Msg = "-خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                    return MosaferiInfo;
                }

                return MosaferiInfo;

            }
            catch (Exception x)
            {
                var ErMsg2 = "";
                if (x.InnerException != null)
                    ErMsg2 = x.InnerException.Message;

                UserInfo user = (UserInfo)(Session["User"]);
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, ErMsg.ToString() + "  " + ErMsg2, DateTime.Now, Input.fldId, "");

                MosaferiInfo.Err = 1;
                MosaferiInfo.message = "قطع ارتباط با وب سرویس ";
                return MosaferiInfo;
            }

        }
        public ActionResult EstelamMosaferi(string FromDate, string ToDate, string Owner)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
              //  EstelamSabtService sabt = new EstelamSabtService();
                //OutputDto_GraduateInfo_VezaratOlum PerInfo = new OutputDto_GraduateInfo_VezaratOlum();
                //RTRSabtService.EstelamSabtService sabt = new RTRSabtService.EstelamSabtService();
                // RTRSabtService.OutputDto_GraduateInfo_VezaratOlum DegreeInfo = new RTRSabtService.OutputDto_GraduateInfo_VezaratOlum();

               // var DegreeInfo = sabt.GetGraduateInfo_VezaratOlum(CodeMeli, CodeRahgiri);

                var mosaferiInfo = GetMosaferi(FromDate, ToDate, Owner);

                List<Output_Mosaferi> a = new List<Output_Mosaferi>();
                if (mosaferiInfo.Err == 0)
                    foreach (var item in mosaferiInfo.data)
                    {
                      //  Output_Mosaferi e = new Output_Mosaferi();
                        //if (item.errorCode == 0)
                        //{
                            //e.UniversityDesc = item.compartmentNumber;
                            //e.TotalAverage = item.studentInfo;
                            //e.StudyLevelDesc = item.studentUniInfo;
                            //e.CourseStudyDesc = item.studentUniInfo;
                            //e.StopDate = item.studentUniInfo;
                            //e.StudentStatusDesc = item.studentUniInfo;
                            //e.StudyLevelDesc_v = item.studentMsrtInfo;
                            //e.CourseStudyDesc_v = item.studentMsrtInfo;
                            //e.StopDate_v = item.studentMsrtInfo;
                            //e.StudentStatusDesc_v = item.studentMsrtInfo;

                            m.prs_MosaferiInfoInsert(item.uicWagonNo, item.trainNumber, item.trainDate, item.moveTime, item.persianTrainDate, item.wagonNumber, item.compartmentNumber, item.seatNumber, item.ticketNumber, item.seirStartStationCode, item.seirStartStation, item.seirEndStationCode, item.seirEndStation, item.nameCompany, item.totalDistance);
                        //}
                        //else
                        //{
                        //    e.UniversityDesc = item.errorCode.ToString();
                        //}
                      //  a.Add(e);
                    }

                return Json(new
                {
                    Er = mosaferiInfo.Err,
                    Message = mosaferiInfo.Msg,
                    degreeInfo = a.ToList()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                return Json(new
                {
                    Er = 1,
                    Message = x.InnerException.Message
                }, JsonRequestBehavior.AllowGet);
            }


        }

        string GetToken()
        {
            string requestUri = "https://externalapi.rai.ir/api/v1/Users/TokenBody";
            var bodyContent = new
            {
                username = "Made12",
                password = "Made12.IT.14011126.583.I4@a540KGAa5m50oA34%pCx42D",
                grant_type = "password"
            };
            var myJson = JsonConvert.SerializeObject(bodyContent);


            HttpClient client = new HttpClient();
            var result = new StringContent(myJson.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(requestUri, result).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            var token = json.access_token;
            return token;
        }

        //[WebMethod]
        //public Output_Mosaferi GetMosaferi(string FromDate, string ToDate, string Owner)
        //{
        //    var ErMsg = 0;
        //    Models.RaiSamEntities m = new RaiSamEntities();
        //    Output_Mosaferi MosaferiInfo = new Output_Mosaferi();
        //    try
        //    {

        //        string requestUri = "https://externalapi.rai.ir/api/v1/Made12/Mosaferi";
        //        var bodyContent = new
        //        {
        //            FromDate = FromDate,
        //            ToDate = ToDate,
        //            Owner = Owner
        //        };
        //        var myJson = JsonConvert.SerializeObject(bodyContent);
        //        Output_Mosaferi Result = new Output_Mosaferi();
        //        HttpClient client = new HttpClient();
        //        var res = new StringContent(myJson.ToString(), Encoding.UTF8, "application/json");
        //        client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", GetToken()));
        //        HttpResponseMessage response = client.PostAsync(requestUri, res).Result;
        //        var content = response.Content.ReadAsStringAsync().Result;
        //        ErMsg = 1;
        //        Result = Newtonsoft.Json.JsonConvert.DeserializeObject<Output_Mosaferi>(content);
        //        ErMsg = 2;
        //        if (Result.isSuccess)
        //        {
        //            ErMsg = 3;
        //            MosaferiInfo = Result;
        //            MosaferiInfo.Err = 0;
        //        }
        //        else
        //        {
        //            ErMsg = 4;
        //            MosaferiInfo.Err = 1;
        //            MosaferiInfo.Msg = "-خطایی با شماره " + Result.statusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
        //            if (Result.statusCode == 1)
        //                MosaferiInfo.Msg = MosaferiInfo.Msg + "(نامعتبر بودن کدملی یا رهگیری یا منقضی شدن کد رهگیری)";
        //            return MosaferiInfo;
        //        }

        //        return MosaferiInfo;
        //    }
        //    catch (Exception x)
        //    {
        //        var ErMsg2 = "";
        //        if (x.InnerException != null)
        //            ErMsg2 = x.InnerException.Message;

        //        UserInfo user = (UserInfo)(Session["User"]);
        //        System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
        //        var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
        //        m.prs_tblErrorInsert(ErrorId, ErMsg.ToString() + "  " + ErMsg2, DateTime.Now, Input.fldId, "");

        //        MosaferiInfo.Err = 1;
        //        MosaferiInfo.message = "قطع ارتباط با وب سرویس ";
        //        return MosaferiInfo;
        //    }

        //}
        public ActionResult IndexGroup(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            ViewData.Model = new RptBarname();
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = ViewData
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();

            if (Session["FristRegisterId"] != null)
                partial.ViewBag.IsMalek = 1;

            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_GetDate().FirstOrDefault();
            partial.ViewBag.fldTarikh = q.fldDateTime.ToString("yyyy-MM-dd");
            partial.ViewBag.fldTarikh_S = q.fldTarikh;
            int currMounth = Convert.ToInt32(q.fldTarikh.Split('/')[1]);
            int currYear = Convert.ToInt32(q.fldTarikh.Split('/')[0]);
            int TaMah = currMounth - 1;
            int TaSal = currYear;
            if (TaMah == 0)
            {
                TaMah = 12;
                TaSal = TaSal - 1;
            }
            int AzMah = TaMah - 1;
            int AzSal = TaSal;
            if (AzMah == 0)
            {
                AzMah = 12;
                AzSal = AzSal - 1;
            }
            partial.ViewBag.fldTarikh_az = AzSal.ToString() + "/" + AzMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.fldTarikh_ta = TaSal.ToString() + "/" + TaMah.ToString().PadLeft(2, '0') + "/01";
            partial.ViewBag.AzSal = AzSal.ToString();
            partial.ViewBag.TaSal = TaSal.ToString();
            partial.ViewBag.AzMah = AzMah.ToString().PadLeft(2, '0');
            partial.ViewBag.TaMah = TaMah.ToString().PadLeft(2, '0');

            return partial;
        }

        Workbook Sheetjozee(Workbook workbook, string NameFile, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int? logoId)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);

            var k = 0;
            if (TypeVagon == "0")
            {
                 k=List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
            }
            else if (TypeVagon == "1")
            {
                k=ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
            }

            if (k != 0)
            {
                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();

                FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();

                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", IsExcel);
                Report.SetParameterValue("OnlyData", 0);
                Report.Prepare();
                Report.Export(xlsExport, stream);

                Workbook workbook2 = new Workbook(stream);
                //Report.Export(xlsExport, stream);
                workbook.Worksheets.Add("جزئی " + NameFile);
                workbook.Worksheets["جزئی " + NameFile].Copy(workbook2.Worksheets[0]);
            }
            return workbook;

        }
        Workbook SheetKoli(Workbook workbook, string NameFile, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int? logoId)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalKardKoliMosaferiTableAdapter(); 
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

           
            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

            var cc = ContractId.Split(',');
            List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);

            FastReport.Report Report = new FastReport.Report();


            var k = 0;
            if (TypeVagon == "0")
            {
                k=List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarname.frx");
            }
            else if (TypeVagon == "1")
            {
                k=ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarnameMosaferi.frx");
            }

            if (k != 0)
            {
                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();

                FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();

                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", IsExcel);
                Report.SetParameterValue("OnlyData", 0);
                Report.Prepare();
                Report.Export(xlsExport, stream);

                Workbook workbook2 = new Workbook(stream);
                //Report.Export(xlsExport, stream);
                workbook.Worksheets.Add("کلی " + NameFile);
                workbook.Worksheets["کلی " + NameFile].Copy(workbook2.Worksheets[0]);
            }
            return workbook;

        }
        public FileResult GenerateXlsGroup(int sal, int Tasal, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State, bool JozeeM, bool KoliM, bool JozeeF, bool KoliF, bool JozeeY, bool KoliY)
        {

            if (Session["User"] == null)
                return null;
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);




                var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

                int? logoId = m.prs_RptBarname_Info(sal + "/01/01", Tasal + "/12/30", MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');





                Workbook workbook = new Workbook();

                for (int y = sal; y <= Tasal; y++)
                {
                    string SalHa = y.ToString();

                    var counter = 1;
                    while (counter <= 12)
                    {
                        //***************** mah
                        string StartDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/01";
                        string EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/31";
                        if (counter > 6)
                            EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/30";

                        var mo = m.prs_SelectMonth().ToList().Where(l => l.fldCode == counter.ToString().PadLeft('0', 2)).FirstOrDefault();
                        string Namemah = mo.fldName + " " + SalHa;

                        if (JozeeM)
                            workbook = Sheetjozee(workbook, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId);
                        if (KoliM)
                            workbook = SheetKoli(workbook, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId);

                        //************fasl
                        var fasl = 0;
                        Namemah = "";
                        if (counter == 3)
                        {
                            fasl = 1;
                            Namemah = "اول";
                        }
                        else if (counter == 6)
                        {
                            fasl = 2;
                            Namemah = "دوم";
                        }
                        else if (counter == 9)
                        {
                            fasl = 3;
                            Namemah = "سوم";
                        }
                        else if (counter == 12)
                        {
                            fasl = 4;
                            Namemah = "چهارم";
                        }

                        if (fasl != 0)
                        {
                            Namemah = "سه ماهه " + Namemah + " " + SalHa;
                            StartDate = SalHa + "/" + (counter - 2).ToString().PadLeft('0', 2) + "/01";
                            EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/31";
                            if (fasl > 2)
                                EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/30";

                            if (JozeeF)
                                workbook = Sheetjozee(workbook, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId);
                            if (KoliF)
                                workbook = SheetKoli(workbook, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId);
                        }

                        counter++;
                    }

                    //***********************sal
                    if (JozeeY)
                        workbook = Sheetjozee(workbook, SalHa, SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId);
                    if (KoliY)
                        workbook = SheetKoli(workbook, SalHa, SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId);
                }

                var stream2 = new MemoryStream();
                workbook.Save(stream2, SaveFormat.Excel97To2003);


                return File(stream2.ToArray(), "xls", "BarnameGroup.xls");

            }
            catch (Exception x)
            {
                return null;
            }
        }
        public ActionResult CheckedWin(int sal, int Tasal, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.sal = sal;
                PartialView.ViewBag.Tasal = Tasal;
                PartialView.ViewBag.MalekId = MalekId;
                PartialView.ViewBag.ContractId = ContractId;
                PartialView.ViewBag.ShVagon = ShVagon;
                PartialView.ViewBag.AzVagon = AzVagon;
                PartialView.ViewBag.TaVagon = TaVagon;
                PartialView.ViewBag.TypeVagon = TypeVagon;
                PartialView.ViewBag.State = State;
                return PartialView;
            }
        }
        public FileResult GeneratePDFGroup(int sal, int Tasal, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State, bool JozeeM, bool KoliM, bool JozeeF, bool KoliF, bool JozeeY, bool KoliY)
        {

            if (Session["User"] == null)
                return null;
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);




                var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

                int? logoId = m.prs_RptBarname_Info(sal + "/01/01", Tasal + "/12/30", MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');





                FastReport.Report Report = new FastReport.Report();
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
               MemoryStream stream = new MemoryStream();

                for (int y = sal; y <= Tasal; y++)
                {
                    string SalHa = y.ToString();

                    var counter = 1;
                    while (counter <= 12)
                    {
                        //***************** mah
                        string StartDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/01";
                        string EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/31";
                        if (counter > 6)
                            EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/30";

                        var mo = m.prs_SelectMonth().ToList().Where(l => l.fldCode == counter.ToString().PadLeft('0', 2)).FirstOrDefault();
                        string Namemah = mo.fldName + " " + SalHa;

                        if (JozeeM)
                            Report = CombineMultipleReports(Report,  StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon,0);
                        if (KoliM)
                            Report = CombineMultipleReportsKoli(Report,  StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon,0);

                        //************fasl
                        var fasl = 0;
                        Namemah = "";
                        if (counter == 3)
                        {
                            fasl = 1;
                            Namemah = "اول";
                        }
                        else if (counter == 6)
                        {
                            fasl = 2;
                            Namemah = "دوم";
                        }
                        else if (counter == 9)
                        {
                            fasl = 3;
                            Namemah = "سوم";
                        }
                        else if (counter == 12)
                        {
                            fasl = 4;
                            Namemah = "چهارم";
                        }

                        if (fasl != 0)
                        {
                            Namemah = "سه ماهه " + Namemah + " " + SalHa;
                            StartDate = SalHa + "/" + (counter - 2).ToString().PadLeft('0', 2) + "/01";
                            EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/31";
                            if (fasl > 2)
                                EndDate = SalHa + "/" + counter.ToString().PadLeft('0', 2) + "/30";

                            if (JozeeF)
                                Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon,0);
                            if (KoliF)
                                Report = CombineMultipleReportsKoli(Report,  StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0);
                        }

                        counter++;
                    }

                    //***********************sal
                    if (JozeeY)
                        Report = CombineMultipleReports(Report,  SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0);
                    if (KoliY)
                        Report = CombineMultipleReportsKoli(Report,  SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon,0);
                }

                Report.Export(pdf, stream);
                return File(stream.ToArray(), "application/pdf");
               // return File(stream2.ToArray(), "xls", "BarnameGroup.xls");
            }
            catch (Exception x)
            {
                return null;
            }
        }
    }
}
