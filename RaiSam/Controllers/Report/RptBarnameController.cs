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
using System.Threading.Tasks;
using System.Web.UI;
using System.Drawing;
//using GemBox.Spreadsheet;



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
        public ActionResult IndexBarnameLoco(string containerId)
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
         

            return partial;
        }
        public ActionResult IndexKhat(string containerId)
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

                if (TypeVagon == "3")
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
                        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon,0,0);
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

                if (TypeVagon == "3")
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
                        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 1,0);
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
        public FileResult GenerateXlsJadid(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State)
        {

            if (Session["User"] == null)
                return null;

            Models.RaiSamEntities m = new RaiSamEntities();
            int? logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
            FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\BariJozee.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
             if (TypeVagon == "1")
            {
                fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

                UserInfo user = (UserInfo)(Session["User"]);
                SignalrHub h = new SignalrHub();
                /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/

                MemoryStream LogoPic = null;
                if (logoId != null)
                {
                    var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                    if (sk != null)
                        LogoPic = new MemoryStream(sk.fldFile);
                }

                //  Workbook workbook2 = new Workbook();

                //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
                //var worksheet = workbook2.Worksheets["Sheet1"];


                Workbook workbook2 = new Workbook(fstreamExcelJozee);

                // Accessing the first worksheet in the Excel file
                Worksheet worksheet = workbook2.Worksheets[0];


               
                    Models.ProgressBar pro = new Models.ProgressBar();

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
                    workbook2 = new Workbook(fstreamExcelJozee);

                    int fixrow = 12;

                    var k = 0;
                    try
                    {
                        if (TypeVagon == "3")
                        {
                            k = List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                            int i = 0;
                            decimal s1 = 0;
                            decimal s2 = 0;
                            for (i = 0; i < dt.prs_RptBarname.Count; i++)
                            {
                                worksheet.Cells[i + fixrow, 0].Value = i + 1;
                                worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptBarname.Rows[i]["fldShomareVagon"];

                                if ((dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString() == "" || (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString() == null || (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString() == "null")
                                    worksheet.Cells[i + fixrow, 2].Value = "";
                                else
                                    worksheet.Cells[i + fixrow, 2].Value = (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString().Substring(6, 2);

                                worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptBarname.Rows[i]["fldShmareBarname"];

                                if ((dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString() == "" || (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString() == null || (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString() == "null")
                                    worksheet.Cells[i + fixrow, 4].Value = "";
                                else
                                    worksheet.Cells[i + fixrow, 4].Value = (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString().Substring(6, 2);

                                worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptBarname.Rows[i]["fldSeri"];
                                worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptBarname.Rows[i]["fldNameMabda"];
                                worksheet.Cells[i + fixrow, 7].Value = dt.prs_RptBarname.Rows[i]["fldNameMaghsad"];
                                worksheet.Cells[i + fixrow, 8].Value = dt.prs_RptBarname.Rows[i]["fldNoeBar"];
                                worksheet.Cells[i + fixrow, 9].Value = dt.prs_RptBarname.Rows[i]["fldMasaft"];
                                worksheet.Cells[i + fixrow, 10].Value = dt.prs_RptBarname.Rows[i]["fldVaznMahsob"];
                                worksheet.Cells[i + fixrow, 11].Value = dt.prs_RptBarname.Rows[i]["fldVaznVagheiiINT"];
                                worksheet.Cells[i + fixrow, 12].Value = dt.prs_RptBarname.Rows[i]["fldTonKilometrINT"];
                                s1 = s1 + Convert.ToDecimal(dt.prs_RptBarname.Rows[i]["fldVaznVagheiiINT"]);
                                s2 = s2 + Convert.ToDecimal(dt.prs_RptBarname.Rows[i]["fldTonKilometrINT"]);
                            }
                            //worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptBarname.Count).ToString() + ")";
                            //worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptBarname.Count).ToString() + ")";
                            worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 11].Value = s1;
                            worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 12].Value = s2;
                            worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 10].Value = "جمـــــــع";
                            worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                            worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                            worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                            worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                            worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                            worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                            worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                            worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                            worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];

                            //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                            //rng1.Merge();
                            //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                            if (LogoPic != null)
                                worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                        }

                        else if (TypeVagon == "1")
                        {
                            k = ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                            int i = 0;
                            int s1 = 0;
                            int s2 = 0;
                            for (i = 0; i < dt.prs_RptAmalkardMosaferi.Count; i++)
                            {
                                worksheet.Cells[i + fixrow, 0].Value = i + 1;
                                worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptAmalkardMosaferi.Rows[i][0];
                                worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptAmalkardMosaferi.Rows[i][1];
                                worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptAmalkardMosaferi.Rows[i][3];
                                worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptAmalkardMosaferi.Rows[i][4];
                                worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptAmalkardMosaferi.Rows[i][5];
                                worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptAmalkardMosaferi.Rows[i][6];
                                if ((dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString() == "" || (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString() == null || (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString() == "null")
                                    worksheet.Cells[i + fixrow, 7].Value = "";
                                else
                                    worksheet.Cells[i + fixrow, 7].Value = (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(6, 2);

                                if ((dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString() == "" || (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString() == null || (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString() == "null")
                                    worksheet.Cells[i + fixrow, 8].Value = "";
                                else
                                    worksheet.Cells[i + fixrow, 8].Value = (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString().Substring(6, 2);

                                
                                worksheet.Cells[i + fixrow, 9].Value = dt.prs_RptAmalkardMosaferi.Rows[i][7];
                                worksheet.Cells[i + fixrow, 10].Value = dt.prs_RptAmalkardMosaferi.Rows[i][8];
                                worksheet.Cells[i + fixrow, 11].Value = dt.prs_RptAmalkardMosaferi.Rows[i][11];
                                worksheet.Cells[i + fixrow, 12].Value = dt.prs_RptAmalkardMosaferi.Rows[i][10];
                                worksheet.Cells[i + fixrow, 13].Value = dt.prs_RptAmalkardMosaferi.Rows[i][11];
                                s1 = s1 + Convert.ToInt32(dt.prs_RptAmalkardMosaferi.Rows[i][10]);
                                s2 = s2 + Convert.ToInt32(dt.prs_RptAmalkardMosaferi.Rows[i][11]);
                            }
                            //worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptAmalkardMosaferi.Count).ToString() + ")";
                            //worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptAmalkardMosaferi.Count).ToString() + ")";
                            worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 12].Value = s1;
                            worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 13].Value = s2;
                            worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 11].Value = "جمـــــــع";
                            worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                            worksheet.Cells["L1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                            worksheet.Cells["L3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                            worksheet.Cells["L5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                            worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                            worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                            worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                            worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                            worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                            if (LogoPic != null)
                                worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);

                            //worksheet.Tables.Add("kk", "A" + fixrow.ToString() + ":E" + i.ToString(), true);
                            //worksheet.Tables["kk"].BuiltInStyle = worksheet.Tables["Table1"].BuiltInStyle;
                            //worksheet.Tables["kk"].StyleOptions = GemBox.Spreadsheet.Tables.TableStyleOptions.BandedRows;

                            //if (i > 0)
                            //{
                            //    Aspose.Cells.Tables.ListObjectCollection listObjects = worksheet.ListObjects;

                            //    // Add a List based on the data source range with headers on.
                            //    listObjects.Add(fixrow - 1, 0, fixrow + dt.prs_RptAmalkardMosaferi.Count - 1, 12, true);

                            //    // Show the total row for the List.
                            //    listObjects[0].ShowTotals = true;

                            //    // Calculate the total of the last (5th ) list column.

                            //    listObjects[0].ListColumns[11].TotalsCalculation = Aspose.Cells.Tables.TotalsCalculation.Sum;
                            //}
                        }
                      
                        /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                        m.prs_tblErrorInsert(ErrorId, dt.prs_RptBarname_Info.Rows[0][5] + " از تاریخ" + StartDate + " تا تاریخ " + EndDate +"*"+ ErMsg, DateTime.Now, Input.fldId, "");

                        worksheet.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                        worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                        worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                        worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                        worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                        worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                        worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                        worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                        worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];

                        //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                        //rng1.Merge();
                        //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                        if (LogoPic != null)
                            worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                    }

                    var stream2 = new MemoryStream();
                    Workbook workbook = new Workbook();

                    workbook.Worksheets["Sheet1"].Copy(worksheet);

                    workbook.Save(stream2, SaveFormat.Xlsx);

                    return File(stream2.ToArray(), "xlsx", "Barname.xlsx");


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

            if (TypeVagon == "3")
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
        FastReport.Report CombineMultipleReports(FastReport.Report Report, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel,byte TypePrintVagon)
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
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter ListKhat = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);

            if (TypeVagon == "3")
            {
                List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
            }
            else if (TypeVagon == "1")
            {
                ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
            }
            else if (TypeVagon == "4")
            {
                ListKhat.Fill(dt.prs_RptBarnameStation, StartDate, EndDate, ContractId, TypePrintVagon);
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
            var fldTrainMaghsad = ""; decimal? fldBarKhales = 0; decimal? fldMasafat = 0; decimal fldBarNakhales = 0; decimal fldTonKhales=0; decimal? fldTonNakhales = 0; 
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];


                var ListCompany = m.prs_RptDizel(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).ToList();
            foreach (var item in StatusCheck)
            {
                switch (item)
                {
                    /*case "fldTrain_NO":
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
                        break;*/
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
                    /*case "fldTrainDate":
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
                        break;*/
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
                   /* case "fldBarNakhales":
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
                        break;*/
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
                    /*case "fldTonKhales":
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
                        break;*/
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

                if (TypeVagon == "3")
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

                if (TypeVagon == "3")
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
        public FileResult GenerateXlsKoliJadid(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State)
        {

            if (Session["User"] == null)
                return null;

            Models.RaiSamEntities m = new RaiSamEntities();
            int? logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
           
            FileStream fstreamExcelKoli = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\BariKoli.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (TypeVagon == "1")
            {
                fstreamExcelKoli = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiKoli.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/

            MemoryStream LogoPic = null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            //  Workbook workbook2 = new Workbook();

            //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
            //var worksheet = workbook2.Worksheets["Sheet1"];


            Workbook workbook2 = new Workbook(fstreamExcelKoli);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet = workbook2.Worksheets[0];



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

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
            workbook2 = new Workbook(fstreamExcelKoli);

            int fixrow = 12;

            var k = 0;
            try
            {

                if (TypeVagon == "3")
                {
                    k = List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    int i = 0;
                    decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptKoliBarname.Count; i++)
                    {
                        worksheet.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptKoliBarname.Rows[i][0];
                        worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptKoliBarname.Rows[i][8];
                        worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptKoliBarname.Rows[i][2];
                        worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptKoliBarname.Rows[i][3];
                        worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptKoliBarname.Rows[i][6];
                        worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptKoliBarname.Rows[i][5];
                        s1 = s1 + Convert.ToDecimal(dt.prs_RptKoliBarname.Rows[i][6]);
                        s2 = s2 + Convert.ToDecimal(dt.prs_RptKoliBarname.Rows[i][5]);
                    }
                    //worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 5].Formula = "=SUM(F" + (fixrow + 1) + ":F" + (fixrow + dt.prs_RptKoliBarname.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptKoliBarname.Count +  fixrow, 6].Formula = "=SUM(G" + (fixrow + 1) + ":G" + (fixrow + dt.prs_RptKoliBarname.Count).ToString() + ")";
                    worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 5].Value = s1;
                    worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 6].Value = s2;
                    worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 4].Value = "جمـــــــع";
                    worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                    worksheet.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                    worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                    worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                    worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                    worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                    if (LogoPic != null)
                        worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                }
                else if (TypeVagon == "1")
                {
                    k = ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    int i = 0;
                    int s1 = 0;
                    int s2 = 0;
                    for (i = 0; i < dt.prs_RptAmalKardKoliMosaferi.Count; i++)
                    {
                        worksheet.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][4];
                        worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][3];

                        if ((dt.prs_RptAmalKardKoliMosaferi.Rows[i]["minHarekat"]).ToString() == "" || (dt.prs_RptAmalKardKoliMosaferi.Rows[i]["minHarekat"]).ToString() == null || (dt.prs_RptAmalKardKoliMosaferi.Rows[i]["minHarekat"]).ToString() == "null")
                            worksheet.Cells[i + fixrow, 3].Value = "";
                        else
                            worksheet.Cells[i + fixrow, 3].Value = (dt.prs_RptAmalKardKoliMosaferi.Rows[i]["minHarekat"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAmalKardKoliMosaferi.Rows[i]["minHarekat"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAmalKardKoliMosaferi.Rows[i]["minHarekat"]).ToString().Substring(6, 2);


                        worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][1];
                        worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][3];
                        worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][5];
                        s1 = s1 + Convert.ToInt32(dt.prs_RptAmalKardKoliMosaferi.Rows[i][3]);
                        s2 = s2 + Convert.ToInt32(dt.prs_RptAmalKardKoliMosaferi.Rows[i][5]);
                    }
                    //worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 4].Formula = "=SUM(E" + (fixrow + 1) + ":E" + (fixrow + dt.prs_RptAmalKardKoliMosaferi.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count +  fixrow, 5].Formula = "=SUM(F" + (fixrow + 1) + ":F" + (fixrow + dt.prs_RptAmalKardKoliMosaferi.Count).ToString() + ")";
                    worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 5].Value = s1;
                    worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 6].Value = s2;
                    worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 4].Value = "جمـــــــع";
                    worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                    worksheet.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                    worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                    worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                    worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                    worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                    if (LogoPic != null)
                        worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                }

                /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                m.prs_tblErrorInsert(ErrorId, dt.prs_RptBarname_Info.Rows[0][5] + " از تاریخ" + StartDate + " تا تاریخ " + EndDate +"*"+ ErMsg, DateTime.Now, Input.fldId, "");

                worksheet.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                worksheet.Cells["F1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet.Cells["F3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet.Cells["F5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                if (LogoPic != null)
                    worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
            }

            var stream2 = new MemoryStream();
            Workbook workbook = new Workbook();

            workbook.Worksheets["Sheet1"].Copy(worksheet);

            workbook.Save(stream2, SaveFormat.Xlsx);

            return File(stream2.ToArray(), "xlsx", "Barnamekoli.xlsx");


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

            if (TypeVagon == "3")
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

        async Task<Worksheet> Sheetjozee(Workbook workbook, string NameFile, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int? logoId, CancellationToken ct,int idprog)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);
            Workbook workbook2 = new Workbook();
            Models.RaiSamEntities m = new RaiSamEntities();

            //IProgress<Models.ProgressBar> progress = new Progress<Models.ProgressBar>(p =>
            //{
            //SignalrHub h = new SignalrHub();
            //h.AddProgress(NameFile,(decimal)0.25,user.UserInputId);
            //});

            await System.Threading.Tasks.Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                Models.ProgressBar pro = new Models.ProgressBar();

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
                if (TypeVagon == "3")
                {
                    k = List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    k = ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
                }

                if (ct.IsCancellationRequested)
                {
                    // Clean up here, then...
                    ct.ThrowIfCancellationRequested();
                }
                h.AddProgress("جزئی " + NameFile, (decimal)0.5, user.UserInputId, idprog, 2);

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
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    h.AddProgress("جزئی " + NameFile, (decimal)0.75, user.UserInputId, idprog, 2);


                    Report.Export(xlsExport, stream);
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);

                    workbook2 = new Workbook(stream);

                    /* workbook.Worksheets.Add("جزئی " + NameFile);
                     workbook.Worksheets["جزئی " + NameFile].Copy(workbook2.Worksheets[0]);*/
                }
            }
                    );
            return workbook2.Worksheets[0];

        }
        async Task<Worksheet> SheetKoli(Workbook workbook, string NameFile, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int? logoId, CancellationToken ct, int idprog)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            h.AddProgress("کلی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);
            Models.RaiSamEntities m = new RaiSamEntities();
            Workbook workbook2 = new Workbook();

            await System.Threading.Tasks.Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

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
                if (TypeVagon == "3")
                {
                    k = List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    k = ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarnameMosaferi.frx");
                }

                if (ct.IsCancellationRequested)
                {
                    // Clean up here, then...
                    ct.ThrowIfCancellationRequested();
                }
                h.AddProgress("کلی " + NameFile, (decimal)0.5, user.UserInputId, idprog, 2);

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
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    h.AddProgress("کلی " + NameFile, (decimal)0.75, user.UserInputId, idprog, 2);

                    Report.Export(xlsExport, stream);
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    h.AddProgress("کلی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);

                    workbook2 = new Workbook(stream);

                    /* workbook.Worksheets.Add("کلی " + NameFile);
                     workbook.Worksheets["کلی " + NameFile].Copy(workbook2.Worksheets[0]);*/
                }
            }
                    );
            return workbook2.Worksheets[0];

        }
        public async Task<FileResult> GenerateXlsGroup(string SafDlId,int sal, int Tasal, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State, bool JozeeM, bool KoliM, bool JozeeF, bool KoliF, bool JozeeY, bool KoliY)
        {

            //if (Session["User"] == null)
            //    return null;
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                m.prs_tblSafDownloadUpdate("fldId", Convert.ToInt64(SafDlId), 2);
             //   m.prs_tblProgressDelete(user.UserInputId);
             
                var tokenSource2 = new CancellationTokenSource();
                CancellationToken ct = tokenSource2.Token;

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

                int tedad=0;
                if(JozeeM) tedad=tedad+12;
                if(KoliM) tedad=tedad+12;
                if(JozeeF) tedad=tedad+4;
                if(KoliF) tedad=tedad+4;
                if(JozeeY) tedad=tedad+1;
                if(KoliY) tedad=tedad+1;
                Task<Worksheet>[] Taskworkbook = new Task<Worksheet>[tedad];
                string[] Nameworkbook = new string[tedad];
                int z = 0;
                string sss = "";
                SignalrHub h = new SignalrHub();

                Workbook workbook = new Workbook();

             //  var workbook =  ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
                FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\BariJozee.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileStream fstreamExcelKoli = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\BariKoli.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (TypeVagon == "1")
                {
                     fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                     fstreamExcelKoli = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiKoli.xlsx", FileMode.Open,  FileAccess.Read, FileShare.ReadWrite);
                }
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
                        {
                            sss = "11" + SalHa+ counter.ToString();
                            /*h.AddProgress("جزئی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);*/
                            Taskworkbook[z] = SheetjozeeGemBox(fstreamExcelJozee,Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId, ct, Convert.ToInt32(sss));
                            Nameworkbook[z] = "جزئی " + Namemah;
                            z++;
                        }
                        if (KoliM)
                        {
                            sss = "21" + SalHa + counter.ToString();
                          /*  h.AddProgress("کلی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);*/
                            Taskworkbook[z] = SheetKoliGemBox(fstreamExcelKoli, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId, ct, Convert.ToInt32(sss));
                            Nameworkbook[z] = "کلی " + Namemah;
                             z++;
                        }

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
                            {
                                sss = "12" + SalHa + fasl.ToString();
                                /*h.AddProgress("جزئی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);*/
                                Taskworkbook[z] = SheetjozeeGemBox(fstreamExcelJozee,Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId, ct, Convert.ToInt32(sss)); 
                                Nameworkbook[z] = "جزئی " + Namemah;
                                 z++;
                            }
                            if (KoliF)
                            {
                                sss = "22" + SalHa + fasl.ToString();
                               /* h.AddProgress("کلی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);*/
                                Taskworkbook[z] = SheetKoliGemBox(fstreamExcelKoli, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId, ct, Convert.ToInt32(sss));
                                Nameworkbook[z] = "کلی " + Namemah;
                                 z++;
                            }
                        }

                        counter++;
                    }

                    //***********************sal
                    if (JozeeY)
                    {
                        sss = "13" + SalHa ;
                       /* h.AddProgress("جزئی " + SalHa, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);*/
                        Taskworkbook[z] = SheetjozeeGemBox(fstreamExcelJozee, SalHa, SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId, ct, Convert.ToInt32(sss));
                        Nameworkbook[z] = "جزئی " + SalHa;
                        z++;
                    }
                    if (KoliY)
                    {
                        sss = "23" + SalHa;
                       /* h.AddProgress("کلی " + SalHa, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);*/
                        Taskworkbook[z] = SheetKoliGemBox(fstreamExcelKoli, SalHa, SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 1, logoId, ct, Convert.ToInt32(sss));
                        Nameworkbook[z] = "کلی " + SalHa;
                         z++;
                    }
                }

                //for (int i = 0; i < z; i++)
                //{
                //    workbook.Worksheets.Add(Nameworkbook[i]);
                //    workbook.Worksheets[Nameworkbook[i]].Copy(await Taskworkbook[i]);
                //}


                var stream2 = new MemoryStream();

                //var h = System.Threading.Tasks.Task.WhenAll(Taskworkbook).ContinueWith((taskt) =>
                //  {
                //      for (int i = 0; i < z; i++)
                //      {
                //          workbook.Worksheets.Add(Nameworkbook[i]);
                //          workbook.Worksheets[Nameworkbook[i]].Copy(taskt.Result[i]);
                //      }
                //      workbook.Save(stream2, SaveFormat.Excel97To2003);
                //      ScriptManager.RegisterStartupScript(Page, Page.GetType(), "close", "DlInBackGround(" + stream2.ToArray() + ");", true);   
                //      return File(stream2.ToArray(), "xls", "BarnameGroup.xls");
                //  }
                //  );

                var hh = await System.Threading.Tasks.Task.WhenAll(Taskworkbook);
                for (int i = 0; i < z; i++)
                {
                    workbook.Worksheets.Add(Nameworkbook[i]);
                    workbook.Worksheets[Nameworkbook[i]].Copy(Taskworkbook[i].Result);
                   // workbook.Worksheets.AddCopy(Nameworkbook[i], Taskworkbook[i].Result);
                }
               workbook.Save(stream2, SaveFormat.Xlsx);
             //   workbook.Save(stream2, GemBox.Spreadsheet.SaveOptions.XlsDefault);

               //  m.prs_tblProgressDelete(user.UserInputId);
               m.prs_tblSafDownloadUpdate("fldId", Convert.ToInt64(SafDlId), 3);

                return File(stream2.ToArray(), "xlsx", "BarnameGroup.xlsx");
               

            }
            catch (Exception x)
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
                m.prs_tblSafDownloadUpdate("fldId",Convert.ToInt64(SafDlId), 4);

                var ErMsg = "";
                if (x.InnerException != null)
                    ErMsg = x.InnerException.Message;
                else
                    ErMsg = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
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

                if (Session["FristRegisterId"] != null)
                    PartialView.ViewBag.IsMalek = 1;
             
                return PartialView;
            }
        }
        public ActionResult ProgressWin(int k)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.k = k;
                // X.Msg.Show(new MessageBoxConfig
                //{
                //    Buttons = MessageBox.Button.OK,
                //    Icon = MessageBox.Icon.ERROR,
                //    Title = "هشدار",
                //     ProgressText= "Initializing...",
                //         Progress=true
                //});
                return PartialView;
            }
        }
        public ActionResult ReadProgress(StoreRequestParameters parameters)
        {
            
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblProgressSelect> data = null;
             data = m.prs_tblProgressSelect("inputId", user.UserInputId.ToString(), 0).ToList();
            //    List<Models.ProgressBar> data = new List<Models.ProgressBar>();

            //    Models.ProgressBar it =new Models.ProgressBar();

            //    it.id = 1;
            //it.Percentage=(float)0.25;
            //data.Add(it);

            //it = new Models.ProgressBar();
            //it.id = 2;
            //it.Percentage = (float)0.75;
            //data.Add(it);


                return this.Store(data, data.Count);
            
        }
        public async Task<FileResult> GeneratePDFGroup(int sal, int Tasal, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State, bool JozeeM, bool KoliM, bool JozeeF, bool KoliF, bool JozeeY, bool KoliY)
        {

            if (Session["User"] == null)
                return null;
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                
                SignalrHub h = new SignalrHub();


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


                int tedad = 0;
                if (JozeeM) tedad = tedad + 12;
                if (KoliM) tedad = tedad + 12;
                if (JozeeF) tedad = tedad + 4;
                if (KoliF) tedad = tedad + 4;
                if (JozeeY) tedad = tedad + 1;
                if (KoliY) tedad = tedad + 1;
                Task<MemoryStream>[] Taskworkbook = new Task<MemoryStream>[tedad];
                var sss = "";
                int z = 0;
                var tokenSource2 = new CancellationTokenSource();
                CancellationToken ct = tokenSource2.Token;

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
                        {
                            sss = "11" + SalHa + counter.ToString();
                          //  h.AddProgress("جزئی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);
                            /*Taskworkbook[z] =*/
                            Report = CombineMultipleReportsInProgress(Report, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0, Convert.ToInt32(sss), ct);
                            z++;
                        }
                        if (KoliM)
                        {
                            sss = "21" + SalHa + counter.ToString();
                          //  h.AddProgress("کلی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);
                            /*Taskworkbook[z] =*/
                            Report = CombineMultipleReportsKoliInProgress(Report, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0, Convert.ToInt32(sss), ct);
                            z++;
                        }
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
                            {
                                sss = "12" + SalHa + fasl.ToString();
                               // h.AddProgress("جزئی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);
                                /*Taskworkbook[z] =*/
                                Report = CombineMultipleReportsInProgress(Report, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0, Convert.ToInt32(sss), ct);
                                z++;
                            }
                            if (KoliF)
                            {
                                sss = "22" + SalHa + fasl.ToString();
                               // h.AddProgress("کلی " + Namemah, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);
                                /* Taskworkbook[z] =*/
                                Report = CombineMultipleReportsKoliInProgress(Report, Namemah, StartDate, EndDate, MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0, Convert.ToInt32(sss), ct);
                                z++;
                            }
                        }

                        counter++;
                    }

                    //***********************sal
                    if (JozeeY)
                    {
                        sss = "13" + SalHa;
                     //    h.AddProgress("جزئی " + SalHa, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);
                        /*Taskworkbook[z] =*/
                        Report = CombineMultipleReportsInProgress(Report, SalHa, SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0, Convert.ToInt32(sss), ct);
                        z++;
                    }
                    if (KoliY)
                    {
                        sss = "23" + SalHa;
                         //h.AddProgress("جزئی " + SalHa, (decimal)0, user.UserInputId, Convert.ToInt32(sss), 1);
                        /*Taskworkbook[z] =*/
                        Report=CombineMultipleReportsKoliInProgress(Report, SalHa, SalHa + "/01/01", SalHa + "/12/30", MalekId, cc[0], ShVagon, AzVagon, TaVagon, TypeVagon, 0, Convert.ToInt32(sss), ct);
                        z++;
                    }
                }

   /*             Aspose.Pdf.Document pdfDocument1 = new Aspose.Pdf.Document();

                var hh = await System.Threading.Tasks.Task.WhenAll(Taskworkbook);
                for (int i = 0; i < z; i++)
                {
                    Aspose.Pdf.Document pdfDocument2 = new Aspose.Pdf.Document(Taskworkbook[i].Result);
                    pdfDocument1.Pages.Add(pdfDocument2.Pages);


                    // workbook.Worksheets.AddCopy(Nameworkbook[i], Taskworkbook[i].Result);
                }
                pdfDocument1.Save(stream);*/
                Report.SavePrepared(stream);
               // Report.LoadPrepared(stream);
                return File(stream.ToArray(), "image/vnd");
               // return File(stream2.ToArray(), "xls", "BarnameGroup.xls");
            }
            catch (Exception x)
            {
                return null;
            }
        }
        FastReport.Report CombineMultipleReportsInProgress(FastReport.Report Report, string Namemah, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int idprog, CancellationToken ct)
        {

          //  FastReport.Report Report = new FastReport.Report();
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            MemoryStream stream = new MemoryStream();

            SignalrHub h = new SignalrHub();
            h.AddProgress("جزئی " + Namemah, (decimal)0.25, user.UserInputId, idprog, 2);



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


          
                if (TypeVagon == "3")
                {
                    List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
                }
              
                h.AddProgress("جزئی " + Namemah, (decimal)0.5, user.UserInputId, idprog, 2);

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", IsExcel);
                Report.SetParameterValue("OnlyData", 0);
                Report.Prepare(true);
                if (ct.IsCancellationRequested)
                {
                    // Clean up here, then...
                    ct.ThrowIfCancellationRequested();
                }
                h.AddProgress("جزئی " + Namemah, (decimal)0.75, user.UserInputId, idprog, 2);

                /*  Report.Export(pdf, stream);

                  if (ct.IsCancellationRequested)
                  {
                      // Clean up here, then...
                      ct.ThrowIfCancellationRequested();
                  }
                  h.AddProgress("جزئی " + Namemah, (decimal)1, user.UserInputId, idprog, 2);*/
         
            return Report;

        }
        FastReport.Report CombineMultipleReportsKoliInProgress(FastReport.Report Report, string Namemah, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int idprog, CancellationToken ct)
        {

           // FastReport.Report Report = new FastReport.Report();
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            MemoryStream stream = new MemoryStream();


            SignalrHub h = new SignalrHub();
            h.AddProgress("کلی " + Namemah, (decimal)0.25, user.UserInputId, idprog, 2);


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

              if (TypeVagon == "3")
              {
                  List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                  Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarname.frx");
              }
              else if (TypeVagon == "1")
              {
                  ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                  Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarnameMosaferi.frx");
              }

           
              h.AddProgress("کلی " + Namemah, (decimal)0.5, user.UserInputId, idprog, 2);

              Report.RegisterData(dt, "raiSamDataSet");
              FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
              pdf.EmbeddingFonts = true;
              Report.SetParameterValue("UserName", q.fldNamePersonal);
              Report.SetParameterValue("AzTarikh", StartDate);
              Report.SetParameterValue("TaTarikh", EndDate);
              Report.SetParameterValue("IsExcel", IsExcel);
              Report.SetParameterValue("OnlyData", 0);
              Report.Prepare(true);

        
              h.AddProgress("کلی " + Namemah, (decimal)0.75, user.UserInputId, idprog, 2);
              /*  Report.Export(pdf, stream);

                if (ct.IsCancellationRequested)
                {
                    // Clean up here, then...
                    ct.ThrowIfCancellationRequested();
                }
                h.AddProgress("کلی " + Namemah, (decimal)1, user.UserInputId, idprog, 2);*/

              return Report;

        }
        async Task<MemoryStream> CombineMultipleReportsInProgress2(/*FastReport.Report Report,*/ string Namemah, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int idprog, CancellationToken ct)
        {

            FastReport.Report Report = new FastReport.Report();
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            MemoryStream stream = new MemoryStream();

            SignalrHub h = new SignalrHub();

            await System.Threading.Tasks.Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();


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

                try
                {


                    if (TypeVagon == "3")
                    {
                        List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarname.frx");
                    }
                    else if (TypeVagon == "1")
                    {
                        ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameMosaferi.frx");
                    }

                    h.AddProgress("جزئی " + Namemah, (decimal)0, user.UserInputId, idprog, 1);

                    Report.RegisterData(dt, "raiSamDataSet");
                    FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                    pdf.EmbeddingFonts = true;
                    Report.SetParameterValue("UserName", q.fldNamePersonal);
                    Report.SetParameterValue("AzTarikh", StartDate);
                    Report.SetParameterValue("TaTarikh", EndDate);
                    Report.SetParameterValue("IsExcel", IsExcel);
                    Report.SetParameterValue("OnlyData", 0);
                    Report.Prepare();

                    h.AddProgress("جزئی " + Namemah, (decimal)1, user.UserInputId, idprog, 2);

                    int tedad = Report.PreparedPages.Count;
                    int i = 0;


                    Aspose.Pdf.Document pdfDocument1 = new Aspose.Pdf.Document();

                    while (i <= tedad)
                    {

                        string k = i + "-" + (i + 300);
                        if (i + 300 > tedad)
                        {
                            k = i + "-" + tedad;
                        }

                        pdf.PageNumbers = k;

                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }
                        int iidprog = Convert.ToInt32(idprog.ToString() + i);
                        h.AddProgress("جزئی " + Namemah + "(" + k + ")" + tedad, (decimal)0, user.UserInputId, iidprog, 1);

                        MemoryStream stream2 = new MemoryStream();

                        Report.Export(pdf, stream2);
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }
                        h.AddProgress("جزئی " + Namemah + "(" + k + ")" + tedad, (decimal)0.75, user.UserInputId, iidprog, 2);

                        Aspose.Pdf.Document pdfDocument2 = new Aspose.Pdf.Document(stream2);
                        pdfDocument1.Pages.Add(pdfDocument2.Pages);

                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }
                        h.AddProgress("جزئی " + Namemah + "(" + k + ")" + tedad, (decimal)1, user.UserInputId, iidprog, 2);

                        i = i + 300;
                    }

                    pdfDocument1.Save(stream);


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
                    m.prs_tblErrorInsert(ErrorId, "جزئی " + Namemah + "*" + ContractId + "*" + ErMsg, DateTime.Now, Input.fldId, "");
                }
            }
            );
            return stream;

        }
        async Task<MemoryStream> CombineMultipleReportsKoliInProgress2(/*FastReport.Report Report, */string Namemah, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int idprog, CancellationToken ct)
        {

            FastReport.Report Report = new FastReport.Report();
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            MemoryStream stream = new MemoryStream();


            SignalrHub h = new SignalrHub();

            await System.Threading.Tasks.Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

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

                try
                {

                if (TypeVagon == "3")
                {
                    List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarname.frx");
                }
                else if (TypeVagon == "1")
                {
                    ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliBarnameMosaferi.frx");
                }


                h.AddProgress("کلی " + Namemah, (decimal)0, user.UserInputId, idprog, 1);

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", IsExcel);
                Report.SetParameterValue("OnlyData", 0);
                Report.Prepare();



                h.AddProgress("کلی " + Namemah, (decimal)1, user.UserInputId, idprog, 2);

                int tedad = Report.PreparedPages.Count;
                int i = 0;


                Aspose.Pdf.Document pdfDocument1 = new Aspose.Pdf.Document();

                while (i <= tedad)
                {

                    string k = i + "-" + (i + 300);
                    if (i + 300 > tedad)
                    {
                        k = i + "-" + tedad;
                    }

                    pdf.PageNumbers = k;

                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    int iidprog = Convert.ToInt32(idprog.ToString() + i);
                    h.AddProgress("کلی " + Namemah + "(" + k + ")" + tedad, (decimal)0, user.UserInputId, iidprog, 1);

                    MemoryStream stream2 = new MemoryStream();

                    Report.Export(pdf, stream2);
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    h.AddProgress("کلی " + Namemah + "(" + k + ")" + tedad, (decimal)0.75, user.UserInputId, iidprog, 2);

                    Aspose.Pdf.Document pdfDocument2 = new Aspose.Pdf.Document(stream2);
                    pdfDocument1.Pages.Add(pdfDocument2.Pages);

                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    h.AddProgress("کلی " + Namemah + "(" + k + ")" + tedad, (decimal)1, user.UserInputId, iidprog, 2);

                    i = i + 300;
                }

                pdfDocument1.Save(stream);

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
                    m.prs_tblErrorInsert(ErrorId, "کلی " + Namemah + "*" + ContractId +"*"+ ErMsg, DateTime.Now, Input.fldId, "");
                }
            });
            return stream;

        }
        public ActionResult GenerateRptTest(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                if (Id == 1)
                {
                    var uu = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();

                    FastReport.Report rep = new FastReport.Report();
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\test.frx";
                    rep.Load(path);

                    rep.SetParameterValue("ConectionStr", System.Configuration.ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString);

                    if (rep.Report.Prepare())
                    {
                        // Set PDF export props
                        FastReport.Export.Pdf.PDFExport pdfExport = new FastReport.Export.Pdf.PDFExport();
                        pdfExport.ShowProgress = false;
                        pdfExport.Compressed = true;
                        pdfExport.AllowPrint = true;
                        pdfExport.EmbeddingFonts = true;


                        MemoryStream strm = new MemoryStream();
                        rep.Report.Export(pdfExport, strm);
                        rep.Dispose();
                        pdfExport.Dispose();
                        strm.Position = 0;

                        // return stream in browser
                        return File(strm.ToArray(), "application/pdf");
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
           
               


                
                var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarname_InfoTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter ListMosaferi = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAmalkardMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();


                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

               

                FastReport.Report Report = new FastReport.Report();

            
                    ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, "1399/01/01", "1399/12/29", "", "", "28", "", "");
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\test2.frx");
                

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.Prepare();



                Report.Export(pdf, stream);
                return File(stream.ToArray(), "application/pdf");


                //FastReport.Export.Xml.XMLExport excel = new FastReport.Export.Xml.XMLExport();
                //MemoryStream stream = new MemoryStream();
                //Report.Prepare();
                //Report.Export(excel, stream);
                //return File(stream.ToArray(), "application/vnd.ms-excel", "form5-1.xls");
           
                }
            }
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }
        async Task<Worksheet> SheetjozeeGemBox(FileStream fstreamExcelJozee, string NameFile, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int? logoId, CancellationToken ct, int idprog)
        {
            

           
            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/
            Models.RaiSamEntities m = new RaiSamEntities();

            MemoryStream LogoPic = null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            //  Workbook workbook2 = new Workbook();

            //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
            //var worksheet = workbook2.Worksheets["Sheet1"];


            Workbook workbook2 = new Workbook(fstreamExcelJozee);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet = workbook2.Worksheets[0];


            await System.Threading.Tasks.Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                Models.ProgressBar pro = new Models.ProgressBar();

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
                workbook2 = new Workbook(fstreamExcelJozee);

                int fixrow = 12;

                var k = 0;
                try
                {
                    if (TypeVagon == "3")
                    {
                        k = List.Fill(dt.prs_RptBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                        int i = 0;
                        decimal s1 = 0;
                        decimal s2 = 0;
                        for (i = 0; i < dt.prs_RptBarname.Count; i++)
                        {
                            worksheet.Cells[i + fixrow, 0].Value = i + 1;
                            worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptBarname.Rows[i]["fldShomareVagon"];

                            if ((dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString() == "" || (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString() == null || (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString() == "null")
                                worksheet.Cells[i + fixrow, 2].Value = "";
                            else
                                worksheet.Cells[i + fixrow, 2].Value = (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBahreBardari"]).ToString().Substring(6, 2);

                            worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptBarname.Rows[i]["fldShmareBarname"];

                            if ((dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString() == "" || (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString() == null || (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString() == "null")
                                worksheet.Cells[i + fixrow, 4].Value = "";
                            else
                                worksheet.Cells[i + fixrow, 4].Value = (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptBarname.Rows[i]["fldTarikhBarname"]).ToString().Substring(6, 2);

                            worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptBarname.Rows[i]["fldSeri"];
                            worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptBarname.Rows[i]["fldNameMabda"];
                            worksheet.Cells[i + fixrow, 7].Value = dt.prs_RptBarname.Rows[i]["fldNameMaghsad"];
                            worksheet.Cells[i + fixrow, 8].Value = dt.prs_RptBarname.Rows[i]["fldNoeBar"];
                            worksheet.Cells[i + fixrow, 9].Value = dt.prs_RptBarname.Rows[i]["fldMasaft"];
                            worksheet.Cells[i + fixrow, 10].Value = dt.prs_RptBarname.Rows[i]["fldVaznMahsob"];
                            worksheet.Cells[i + fixrow, 11].Value = dt.prs_RptBarname.Rows[i]["fldVaznVagheiiINT"];
                            worksheet.Cells[i + fixrow, 12].Value = dt.prs_RptBarname.Rows[i]["fldTonKilometrINT"];
                            s1 = s1 + Convert.ToDecimal(dt.prs_RptBarname.Rows[i]["fldVaznVagheiiINT"]);
                            s2 = s2 + Convert.ToDecimal(dt.prs_RptBarname.Rows[i]["fldTonKilometrINT"]);
                        }
                        //worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptBarname.Count).ToString() + ")";
                        //worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptBarname.Count).ToString() + ")";
                        worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 11].Value = s1;
                        worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 12].Value = s2;
                        worksheet.Cells[dt.prs_RptBarname.Count + fixrow, 10].Value = "جمـــــــع";
                        worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                        worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                        worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                        worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                        worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                        worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                        worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                        worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                        worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];

                        //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                        //rng1.Merge();
                        //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                        if (LogoPic != null)
                            worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                    }

                    else if (TypeVagon == "1")
                    {
                        k = ListMosaferi.Fill(dt.prs_RptAmalkardMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                        int i = 0;
                        int s1 = 0;
                        int s2 = 0;
                        for (i = 0; i < dt.prs_RptAmalkardMosaferi.Count; i++)
                        {
                            worksheet.Cells[i + fixrow, 0].Value = i + 1;
                            worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptAmalkardMosaferi.Rows[i][0];
                            worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptAmalkardMosaferi.Rows[i][1];
                            worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptAmalkardMosaferi.Rows[i][3];
                            worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptAmalkardMosaferi.Rows[i][4];
                            worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptAmalkardMosaferi.Rows[i][5];
                            worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptAmalkardMosaferi.Rows[i][6];
                            if ((dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString() == "" || (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString() == null || (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString() == "null")
                                worksheet.Cells[i + fixrow, 7].Value = "";
                            else
                                worksheet.Cells[i + fixrow, 7].Value = (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(6, 2);

                            if ((dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString() == "" || (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString() == null || (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString() == "null")
                                worksheet.Cells[i + fixrow, 8].Value = "";
                            else
                                worksheet.Cells[i + fixrow, 8].Value = (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["minHarekat"]).ToString().Substring(6, 2);

                                
                            //else
                            //    worksheet.Cells[i + fixrow, 7].Value = (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAmalkardMosaferi.Rows[i]["fldTarikhHarekat"]).ToString().Substring(6, 2);
                            worksheet.Cells[i + fixrow, 9].Value = dt.prs_RptAmalkardMosaferi.Rows[i][7];
                            worksheet.Cells[i + fixrow, 10].Value = dt.prs_RptAmalkardMosaferi.Rows[i][8];
                            worksheet.Cells[i + fixrow, 11].Value = dt.prs_RptAmalkardMosaferi.Rows[i][11];
                            worksheet.Cells[i + fixrow, 12].Value = dt.prs_RptAmalkardMosaferi.Rows[i][10];
                            worksheet.Cells[i + fixrow, 13].Value = dt.prs_RptAmalkardMosaferi.Rows[i][11];
                            s1 = s1 + Convert.ToInt32(dt.prs_RptAmalkardMosaferi.Rows[i][10]);
                            s2 = s2 + Convert.ToInt32(dt.prs_RptAmalkardMosaferi.Rows[i][11]);
                        }
                        //worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptAmalkardMosaferi.Count).ToString() + ")";
                        //worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptAmalkardMosaferi.Count).ToString() + ")";
                        worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 12].Value = s1;
                        worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 13].Value = s2;
                        worksheet.Cells[dt.prs_RptAmalkardMosaferi.Count + fixrow, 11].Value = "جمـــــــع";
                        worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                        worksheet.Cells["L1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                        worksheet.Cells["L3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                        worksheet.Cells["L5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                        worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                        worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                        worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                        worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                        worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                        if (LogoPic != null)
                            worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);

                        //worksheet.Tables.Add("kk", "A" + fixrow.ToString() + ":E" + i.ToString(), true);
                        //worksheet.Tables["kk"].BuiltInStyle = worksheet.Tables["Table1"].BuiltInStyle;
                        //worksheet.Tables["kk"].StyleOptions = GemBox.Spreadsheet.Tables.TableStyleOptions.BandedRows;

                        //if (i > 0)
                        //{
                        //    Aspose.Cells.Tables.ListObjectCollection listObjects = worksheet.ListObjects;

                        //    // Add a List based on the data source range with headers on.
                        //    listObjects.Add(fixrow - 1, 0, fixrow + dt.prs_RptAmalkardMosaferi.Count - 1, 12, true);

                        //    // Show the total row for the List.
                        //    listObjects[0].ShowTotals = true;

                        //    // Calculate the total of the last (5th ) list column.

                        //    listObjects[0].ListColumns[11].TotalsCalculation = Aspose.Cells.Tables.TotalsCalculation.Sum;
                        //}
                    }
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                    m.prs_tblErrorInsert(ErrorId, dt.prs_RptBarname_Info.Rows[0][5] + " از تاریخ" + StartDate + " تا تاریخ " + EndDate + "*" + ErMsg, DateTime.Now, Input.fldId, "");

                    worksheet.Cells["C1"].Value = "خطا " + ErrorId .Value+ " در بارگذاری اطلاعات";
                    worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                    worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                    worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                    worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                    worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];

                    //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                    //rng1.Merge();
                    //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                    if (LogoPic != null)
                        worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                }

            }
                    );
            return worksheet;

        }
        async Task<Worksheet> SheetKoliGemBox(FileStream fstreamExcelKoli, string NameFile, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int IsExcel, int? logoId, CancellationToken ct, int idprog)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
           /* h.AddProgress("کلی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/
            Models.RaiSamEntities m = new RaiSamEntities();

            MemoryStream LogoPic =null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            Workbook workbook2 = new Workbook(fstreamExcelKoli);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet = workbook2.Worksheets[0];


            await System.Threading.Tasks.Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                Models.ProgressBar pro = new Models.ProgressBar();

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

                FastReport.Report Report = new FastReport.Report();
                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
                List_info.Fill(dt.prs_RptBarname_Info, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                workbook2 = new Workbook(fstreamExcelKoli);

                int fixrow = 12;

                var k = 0;
                try
                {

                    if (TypeVagon == "3")
                    {
                        k = List.Fill(dt.prs_RptKoliBarname, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                        int i = 0;
                        decimal s1 = 0;
                        decimal s2 = 0;
                        for (i = 0; i < dt.prs_RptKoliBarname.Count; i++)
                        {
                            worksheet.Cells[i + fixrow, 0].Value = i + 1;
                            worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptKoliBarname.Rows[i][0];
                            worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptKoliBarname.Rows[i][8];
                            worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptKoliBarname.Rows[i][2];
                            worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptKoliBarname.Rows[i][3];
                            worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptKoliBarname.Rows[i][6];
                            worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptKoliBarname.Rows[i][5];
                            s1 = s1 + Convert.ToDecimal(dt.prs_RptKoliBarname.Rows[i][6]);
                            s2 = s2 + Convert.ToDecimal(dt.prs_RptKoliBarname.Rows[i][5]);
                        }
                        //worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 5].Formula = "=SUM(F" + (fixrow + 1) + ":F" + (fixrow + dt.prs_RptKoliBarname.Count).ToString() + ")";
                        //worksheet.Cells[dt.prs_RptKoliBarname.Count +  fixrow, 6].Formula = "=SUM(G" + (fixrow + 1) + ":G" + (fixrow + dt.prs_RptKoliBarname.Count).ToString() + ")";
                        worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 5].Value = s1;
                        worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 6].Value = s2;
                        worksheet.Cells[dt.prs_RptKoliBarname.Count + fixrow, 4].Value = "جمـــــــع";
                        worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                        worksheet.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                        worksheet.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                        worksheet.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                        worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                        worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                        worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                        worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                        worksheet.Cells["F10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                        if (LogoPic != null)
                            worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                    }
                    else if (TypeVagon == "1")
                    {
                        k = ListMosaferi.Fill(dt.prs_RptAmalKardKoliMosaferi, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                        int i = 0;
                        int s1 = 0;
                        int s2 = 0;
                        for (i = 0; i < dt.prs_RptAmalKardKoliMosaferi.Count; i++)
                        {
                            worksheet.Cells[i + fixrow, 0].Value = i + 1;
                            worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][4];
                            worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][3];
                            worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][1];
                            worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][3];
                            worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptAmalKardKoliMosaferi.Rows[i][5];
                            s1 = s1 + Convert.ToInt32(dt.prs_RptAmalKardKoliMosaferi.Rows[i][3]);
                            s2 = s2 + Convert.ToInt32(dt.prs_RptAmalKardKoliMosaferi.Rows[i][5]);
                        }
                        //worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 4].Formula = "=SUM(E" + (fixrow + 1) + ":E" + (fixrow + dt.prs_RptAmalKardKoliMosaferi.Count).ToString() + ")";
                        //worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count +  fixrow, 5].Formula = "=SUM(F" + (fixrow + 1) + ":F" + (fixrow + dt.prs_RptAmalKardKoliMosaferi.Count).ToString() + ")";
                        worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 4].Value = s1;
                        worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 5].Value = s2;
                        worksheet.Cells[dt.prs_RptAmalKardKoliMosaferi.Count + fixrow, 3].Value = "جمـــــــع";
                        worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد ماهانه طرح توسعه حمل و نقل ریلی بار و مسافر" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                        worksheet.Cells["F1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                        worksheet.Cells["F3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                        worksheet.Cells["F5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                        worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                        worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                        worksheet.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                        worksheet.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                        worksheet.Cells["E10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                        if (LogoPic != null)
                            worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                    }



                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    /*  h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/


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
                    m.prs_tblErrorInsert(ErrorId, dt.prs_RptBarname_Info.Rows[0][5] + " از تاریخ" + StartDate + " تا تاریخ " + EndDate +"*"+ ErMsg, DateTime.Now, Input.fldId, "");

                    worksheet.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                    worksheet.Cells["F1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet.Cells["F3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet.Cells["F5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet.Cells["A9"].Value = dt.prs_RptBarname_Info.Rows[0][5];
                    worksheet.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptBarname_Info.Rows[0][2] + "-" + dt.prs_RptBarname_Info.Rows[0][4];
                    worksheet.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptBarname_Info.Rows[0][1];
                    worksheet.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptBarname_Info.Rows[0][0];
                    worksheet.Cells["E10"].Value = "ظرفیت واگن: " + dt.prs_RptBarname_Info.Rows[0][3];
                    if (LogoPic != null)
                        worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                }
            }
                    );
            return worksheet;

        }
        public ActionResult ReadIstgah(string azTarikh, string tatarikh, int? fldHeaderId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<RaiSam.Models.prs_tblContract_StationSelect> data = null;
            data = m.prs_tblContract_StationSelect("fldContractId", fldHeaderId.ToString(),0).ToList();
            return this.Store(data);
        }
        public ActionResult PrintPageKhat(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, byte TypevagonPrint, int noe)
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
            partial.ViewBag.TypevagonPrint = TypevagonPrint;
            partial.ViewBag.noe = noe;

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult GeneratePDFKhat(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, byte TypevagonPrint, int noe)
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

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter ListKhat = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameStationTableAdapter ListKhatKoli = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameStationTableAdapter();

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_StationTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_StationTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();


                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');

                var logoId = m.prs_RptHeaderContract_Station(cc[0]).FirstOrDefault().fldLogoId;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);
                List_info.Fill(dt.prs_RptHeaderContract_Station,cc[0]);

                FastReport.Report Report = new FastReport.Report();

                if (TypeVagon == "4")
                {
                    if (noe == 1)
                    {
                        ListKhat.Fill(dt.prs_RptBarnameStation, StartDate, EndDate, cc[0], TypevagonPrint);
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameKhat.frx");
                    }
                    else if (noe == 2)
                    {
                        ListKhatKoli.Fill(dt.prs_RptKoliBarnameStation, StartDate, EndDate, cc[0], TypevagonPrint);
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliKhat.frx");
                    }
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
                        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 0, TypevagonPrint);
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
        public FileResult GenerateXlsKhat(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, byte TypevagonPrint, int noe)
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
                
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameStationTableAdapter ListKoli = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameStationTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_StationTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_StationTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

                var logoId = m.prs_RptBarname_Info(StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon).FirstOrDefault().fldLogoid;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');

                List_info.Fill(dt.prs_RptHeaderContract_Station,  cc[0]);

                FastReport.Report Report = new FastReport.Report();

                if (TypeVagon == "4")
                {
                    if (noe == 1)
                    {
                        List.Fill(dt.prs_RptBarnameStation, StartDate, EndDate, cc[0], TypevagonPrint);
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameKhat.frx");
                    }

                    else if (noe == 2)
                    {
                        ListKoli.Fill(dt.prs_RptKoliBarnameStation, StartDate, EndDate, cc[0], TypevagonPrint);
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKoliKhat.frx");
                    }
                }
                

                Report.RegisterData(dt, "raiSamDataSet");
                // FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();



                MemoryStream stream = new MemoryStream();
          
                    Report.SetParameterValue("OnlyData", 0);
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", StartDate);
                Report.SetParameterValue("TaTarikh", EndDate);
                Report.SetParameterValue("IsExcel", 1);
                Report.Prepare();


                if (cc.Length > 2)
                {
                    for (int i = 1; i < cc.Length - 1; i++)
                        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 1, TypevagonPrint);
                }
                Report.Export(xlsExport, stream);

                return File(stream.ToArray(), "xls", "BarnameKhat.xls");

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
        public FileResult GenerateXlsKhatJadid(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, byte TypevagonPrint, int noe)
        {

            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            int? logoId = m.prs_RptHeaderContract_Station( ContractId).FirstOrDefault().fldLogoId;
            FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\KhatJozee.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (noe == 2)
            {
                fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\KhatKoli.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/

            MemoryStream LogoPic = null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            //  Workbook workbook2 = new Workbook();

            //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
            //var worksheet = workbook2.Worksheets["Sheet1"];


            Workbook workbook2 = new Workbook(fstreamExcelJozee);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet = workbook2.Worksheets[0];



            Models.ProgressBar pro = new Models.ProgressBar();

            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptBarnameStationTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameStationTableAdapter ListKoli = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliBarnameStationTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_StationTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_StationTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptHeaderContract_Station,  ContractId);
            workbook2 = new Workbook(fstreamExcelJozee);

            int fixrow = 12;

            var k = 0;
            try
            {
                if (noe == 1)
                {
                    k = List.Fill(dt.prs_RptBarnameStation, StartDate, EndDate, ContractId, TypevagonPrint);
                    int i = 0;
                    decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptBarnameStation.Count; i++)
                    {
                        worksheet.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptBarnameStation.Rows[i]["fldNameMabda"];
                        worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptBarnameStation.Rows[i]["fldNameMaghsad"];
                        worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptBarnameStation.Rows[i]["fldFareeMabda"];
                        worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptBarnameStation.Rows[i]["fldFareeMaghsad"];
                        worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptBarnameStation.Rows[i]["fldShomareVagon"];
                        worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptBarnameStation.Rows[i]["fldNoeVagon"];
                        worksheet.Cells[i + fixrow, 7].Value = dt.prs_RptBarnameStation.Rows[i]["fldDizel_No"];
                        worksheet.Cells[i + fixrow, 8].Value = dt.prs_RptBarnameStation.Rows[i]["fldExistsDizel"];

                        worksheet.Cells[i + fixrow, 9].Value = dt.prs_RptBarnameStation.Rows[i]["fldShmareBarname"];

                        if ((dt.prs_RptBarnameStation.Rows[i]["fldTarikhBarname"]).ToString() == "" || (dt.prs_RptBarnameStation.Rows[i]["fldTarikhBarname"]).ToString() == null || (dt.prs_RptBarnameStation.Rows[i]["fldTarikhBarname"]).ToString() == "null")
                            worksheet.Cells[i + fixrow, 10].Value = "";
                        else
                            worksheet.Cells[i + fixrow, 10].Value = (dt.prs_RptBarnameStation.Rows[i]["fldTarikhBarname"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptBarnameStation.Rows[i]["fldTarikhBarname"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptBarnameStation.Rows[i]["fldTarikhBarname"]).ToString().Substring(6, 2);

                        worksheet.Cells[i + fixrow, 11].Value = dt.prs_RptBarnameStation.Rows[i]["fldSeri"];
                        worksheet.Cells[i + fixrow, 12].Value = dt.prs_RptBarnameStation.Rows[i]["fldNoeBar"];
                        worksheet.Cells[i + fixrow, 13].Value = dt.prs_RptBarnameStation.Rows[i]["fldVaznMahsobINT"];
                        worksheet.Cells[i + fixrow, 14].Value = dt.prs_RptBarnameStation.Rows[i]["fldVaznVagheiiINT"];
                        worksheet.Cells[i + fixrow, 15].Value = dt.prs_RptBarnameStation.Rows[i]["fldMasaft"];
                        worksheet.Cells[i + fixrow, 16].Value = dt.prs_RptBarnameStation.Rows[i]["fldTonKilometrINT"];
                        s1 = s1 + Convert.ToDecimal(dt.prs_RptBarnameStation.Rows[i]["fldMasaft"]);
                        s2 = s2 + Convert.ToDecimal(dt.prs_RptBarnameStation.Rows[i]["fldTonKilometrINT"]);
                    }
                    //worksheet.Cells[dt.prs_RptBarnameStation.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptBarnameStation.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptBarnameStation.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptBarnameStation.Count).ToString() + ")";
                    worksheet.Cells[dt.prs_RptBarnameStation.Count + fixrow, 15].Value = s1;
                    worksheet.Cells[dt.prs_RptBarnameStation.Count + fixrow, 16].Value = s2;
                    worksheet.Cells[dt.prs_RptBarnameStation.Count + fixrow, 14].Value = "جمـــــــع";
                    worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد طرح توسعه زیرساخت حمل و نقل ریلی " + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                    worksheet.Cells["N1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet.Cells["N3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet.Cells["N5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Station.Rows[0]["fldNameCompany"];
                    worksheet.Cells["A10"].Value = "پروژه ایستگاه خط فرعی: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldTitle"];
                    worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldShomare_GH"];
                    worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldTarikh_GH"];
                    worksheet.Cells["F10"].Value = "ایستگاه انشعاب: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldNameStation"];

                    //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarnameStation.Count).ToString(), "K" + (fixrow + dt.prs_RptBarnameStation.Count).ToString());
                    //rng1.Merge();
                    //worksheet.Cells["A" + (fixrow + dt.prs_RptBarnameStation.Count+1).ToString()].Value = "جمـــــــع";

                    if (LogoPic != null)
                        worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                }

                else
                {
                    k = ListKoli.Fill(dt.prs_RptKoliBarnameStation, StartDate, EndDate, ContractId, TypevagonPrint);
                    int i = 0;
                    decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptKoliBarnameStation.Count; i++)
                    {
                        worksheet.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldNameMabda"];
                        worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldNameMaghsad"];
                        worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldFareeMabda"];
                        worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldFareeMaghsad"];
                        worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldTedad"];
                        worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldSumMasafat"];
                        worksheet.Cells[i + fixrow, 7].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldSumTonazhBarINT"];
                        worksheet.Cells[i + fixrow, 8].Value = dt.prs_RptKoliBarnameStation.Rows[i]["fldSumKilometrINT"];
                        s1 = s1 + Convert.ToDecimal(dt.prs_RptKoliBarnameStation.Rows[i]["fldSumTonazhBarINT"]);
                        s2 = s2 + Convert.ToDecimal(dt.prs_RptKoliBarnameStation.Rows[i]["fldSumKilometrINT"]);
                    }
                    //worksheet.Cells[dt.prs_RptKoliBarnameStation.Count + fixrow, 5].Formula = "=SUM(F" + (fixrow + 1) + ":F" + (fixrow + dt.prs_RptKoliBarnameStation.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptKoliBarnameStation.Count +  fixrow, 6].Formula = "=SUM(G" + (fixrow + 1) + ":G" + (fixrow + dt.prs_RptKoliBarnameStation.Count).ToString() + ")";
                    worksheet.Cells[dt.prs_RptKoliBarnameStation.Count + fixrow, 7].Value = s1;
                    worksheet.Cells[dt.prs_RptKoliBarnameStation.Count + fixrow, 8].Value = s2;
                    worksheet.Cells[dt.prs_RptKoliBarnameStation.Count + fixrow, 6].Value = "جمـــــــع";
                    worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد طرح توسعه زیرساخت حمل و نقل ریلی" + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                    worksheet.Cells["I1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet.Cells["I3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet.Cells["I5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Station.Rows[0]["fldNameCompany"];
                    worksheet.Cells["A10"].Value = "پروژه ایستگاه خط فرعی: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldTitle"];
                    worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldShomare_GH"];
                    worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldTarikh_GH"];
                    worksheet.Cells["F10"].Value = "ایستگاه انشعاب: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldNameStation"];

                    if (LogoPic != null)
                        worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
                }

                /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                m.prs_tblErrorInsert(ErrorId, dt.prs_RptHeaderContract_Station.Rows[0]["fldTitle"] + " از تاریخ" + StartDate + " تا تاریخ " + EndDate +"*"+ ErMsg, DateTime.Now, Input.fldId, "");

                worksheet.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Station.Rows[0]["fldNameCompany"];
                worksheet.Cells["A10"].Value = "پروژه ایستگاه خط فرعی: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldTitle"];
                worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldShomare_GH"];
                worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldTarikh_GH"];
                worksheet.Cells["F10"].Value = "ایستگاه انشعاب: " + dt.prs_RptHeaderContract_Station.Rows[0]["fldNameStation"];

                //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                //rng1.Merge();
                //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                if (LogoPic != null)
                    worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
            }

            var stream2 = new MemoryStream();
            Workbook workbook = new Workbook();

            workbook.Worksheets["Sheet1"].Copy(worksheet);

            workbook.Save(stream2, SaveFormat.Xlsx);

            return File(stream2.ToArray(), "xlsx", "BarnameKhat.xlsx");

        }

        public ActionResult SaveToSafDl(int sal, int Tasal, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon, int State, bool JozeeM, bool KoliM, bool JozeeF, bool KoliF, bool JozeeY, bool KoliY)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
                System.Data.Entity.Core.Objects.ObjectParameter SafId = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));
                var q = m.prs_tblSafDownloadInsert(SafId, "/RptBarname/GenerateXlsGroup", sal, Tasal, MalekId, ContractId, ShVagon, AzVagon, TaVagon, TypeVagon, State, JozeeM, KoliM, JozeeF, KoliF, JozeeY, KoliY, 1,user.UserInputId);

                return Json(new
                {
                    msg="درخواست شما در صف دانلود قرارگرفت و به زودی بارگیری آغاز می شود."
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckSafDl()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
                System.Data.Entity.Core.Objects.ObjectParameter SafId = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));
                var q = m.prs_tblSafDownloadSelect("LastNotDownload",user.UserInputId.ToString(),0).FirstOrDefault();
                if (q != null)
                {
                    return Json(new
                    {
                        Id = q.fldId,
                        fldAzVagon=q.fldAzVagon,
                        fldContractId=q.fldContractId,
                        fldJozeeF=q.fldJozeeF,
                        fldJozeeM=q.fldJozeeM,
                        fldJozeeY=q.fldJozeeY,
                        fldKoliF=q.fldKoliF,
                        fldKoliM=q.fldKoliM,
                        fldKoliY=q.fldKoliY,
                        fldMalekId=q.fldMalekId,
                        fldNameFunc=q.fldNameFunc,
                        fldsal=q.fldsal,
                        fldShVagon=q.fldShVagon,
                        fldState=q.fldState,
                        fldTasal=q.fldTasal,
                        fldTaVagon=q.fldTaVagon,
                        fldTypeVagon=q.fldTypeVagon
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                    {
                        Id = 0
                    }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintPageLoco(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
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
        public ActionResult GeneratePDFLoco(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
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

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelTableAdapter ListKhat = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelTableAdapter();

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();


                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');

                var logoId = m.prs_RptHeaderContract_Dizel(cc[0]).FirstOrDefault().fldLogoId;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                List_info.Fill(dt.prs_RptHeaderContract_Dizel, cc[0]);

                FastReport.Report Report = new FastReport.Report();

                
                    ListKhat.Fill(dt.prs_RptDizel, StartDate, EndDate, MalekId, ShVagon,  cc[0], AzVagon, TaVagon);
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptLoco.frx");
              

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


                //if (cc.Length > 2)
                //{
                //    for (int i = 1; i < cc.Length - 1; i++)
                //        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 0, TypevagonPrint);
                //}

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
        public FileResult GenerateXlsLoco(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
        {

            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var cc = ContractId.Split(',');
            ContractId = cc[0];
            var RptHeaderContract=m.prs_RptHeaderContract_Dizel(ContractId).FirstOrDefault();
            int? logoId = RptHeaderContract.fldLogoId;
            FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\Loco.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
           
            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/

            MemoryStream LogoPic = null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            //  Workbook workbook2 = new Workbook();

            //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
            //var worksheet = workbook2.Worksheets["Sheet1"];


            Workbook workbook2 = new Workbook(fstreamExcelJozee);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet = workbook2.Worksheets[0];



            Models.ProgressBar pro = new Models.ProgressBar();

            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptHeaderContract_Dizel, ContractId);
            workbook2 = new Workbook(fstreamExcelJozee);

            int fixrow = 12;

            var k = 0;
            try
            {

                k = List.Fill(dt.prs_RptDizel, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                    int i = 0;
                    decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptDizel.Count; i++)
                    {
                        worksheet.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptDizel.Rows[i]["fldDizel_No"];
                        /*worksheet.Cells[i + fixrow, 2].Val"];
                        worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptDizel.Rows[i]["fldTrainKind"];

                        if ((dt.prs_RptDizel.Rows[i]["fldTrainDate"]).ToString() == "" || (dt.prs_RptDizel.Rows[i]["fldTrainDate"]).ToString() == null || (dt.prs_RptDizel.Rows[i]["fldTrainDate"]).ToString() == "null")
                            worksheet.Cells[i + fixrow, 4].Value = "";
                        else
                            worksheet.Cells[i + fixrow, 4].Value = (dt.prs_RptDizel.Rows[i]["fldTrainDate"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptDizel.Rows[i]["fldTrainDate"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptDizel.Rows[i]["fldTrainDate"]).ToString().Substring(6, 2);
                        */
                        worksheet.Cells[i + fixrow, 2].Value = dt.prs_RptDizel.Rows[i]["fldWagon_No"];

                        worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptDizel.Rows[i]["fldIsMade12"];

                        if ((dt.prs_RptDizel.Rows[i]["MinTarikhBar"]).ToString() == "0" || (dt.prs_RptDizel.Rows[i]["MinTarikhBar"]).ToString() == null || (dt.prs_RptDizel.Rows[i]["MinTarikhBar"]).ToString() == "null")
                            worksheet.Cells[i + fixrow, 4].Value = "";
                        else
                            worksheet.Cells[i + fixrow, 4].Value = (dt.prs_RptDizel.Rows[i]["MinTarikhBar"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptDizel.Rows[i]["MinTarikhBar"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptDizel.Rows[i]["MinTarikhBar"]).ToString().Substring(6, 2);

                       /* if ((dt.prs_RptDizel.Rows[i]["fldTarikhAvalin"]).ToString() == "0" || (dt.prs_RptDizel.Rows[i]["fldTarikhAvalin"]).ToString() == null || (dt.prs_RptDizel.Rows[i]["fldTarikhAvalin"]).ToString() == "null")
                            worksheet.Cells[i + fixrow, 8].Value = "";
                        else
                            worksheet.Cells[i + fixrow, 8].Value = (dt.prs_RptDizel.Rows[i]["fldTarikhAvalin"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptDizel.Rows[i]["fldTarikhAvalin"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptDizel.Rows[i]["fldTarikhAvalin"]).ToString().Substring(6, 2);
*/
                        worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptDizel.Rows[i]["fldBarnameId"];
                       /* worksheet.Cells[i + fixrow, 10].Value = dt.prs_RptDizel.Rows[i]["fldNameMalek"];*/
                       // worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptDizel.Rows[i]["fldSeri"];
                        worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptDizel.Rows[i]["fldTrainMabda"];
                        worksheet.Cells[i + fixrow, 7].Value = dt.prs_RptDizel.Rows[i]["fldTrainMaghsad"];
                        worksheet.Cells[i + fixrow, 8].Value = dt.prs_RptDizel.Rows[i]["fldNoeBar"];
                        worksheet.Cells[i + fixrow, 9].Value = dt.prs_RptDizel.Rows[i]["fldBarKhales"];
                        worksheet.Cells[i + fixrow, 10].Value = dt.prs_RptDizel.Rows[i]["fldBarNakhales"];
                        worksheet.Cells[i + fixrow, 11].Value = dt.prs_RptDizel.Rows[i]["fldMasafat"];
                        worksheet.Cells[i + fixrow, 12].Value = dt.prs_RptDizel.Rows[i]["fldTonNakhales"];
                        s1 = s1 + Convert.ToDecimal(dt.prs_RptDizel.Rows[i]["fldMasafat"]);
                        s2 = s2 + Convert.ToDecimal(dt.prs_RptDizel.Rows[i]["fldTonNakhales"]);
                    }
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";
                    worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 12].Value = s1;
                    worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 11].Value = s2;
                    worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 10].Value = "جمـــــــع";
                    worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش عملکرد طرح توسعه زیرساخت حمل و نقل ریلی " + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                    worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                    worksheet.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad;
                    worksheet.Cells["G8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                    worksheet.Cells["G9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                    worksheet.Cells["G10"].Value = "توان لکوموتیو: " ;

                    //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptDizel.Count).ToString(), "K" + (fixrow + dt.prs_RptDizel.Count).ToString());
                    //rng1.Merge();
                    //worksheet.Cells["A" + (fixrow + dt.prs_RptBarnameStation.Count+1).ToString()].Value = "جمـــــــع";

                    if (LogoPic != null)
                        worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
               

                /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                m.prs_tblErrorInsert(ErrorId, dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"] + " از تاریخ" + StartDate + " تا تاریخ " + EndDate + "*" + ErMsg, DateTime.Now, Input.fldId, "");

                worksheet.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                worksheet.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad; 
                worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                worksheet.Cells["F10"].Value = "توان لکوموتیو: " ;

                //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                //rng1.Merge();
                //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                if (LogoPic != null)
                    worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
            }

            var stream2 = new MemoryStream();
            Workbook workbook = new Workbook();

            workbook.Worksheets["Sheet1"].Copy(worksheet);

            workbook.Save(stream2, SaveFormat.Xlsx);

            return File(stream2.ToArray(), "xlsx", "BarnameLoco.xlsx");

        }
        //***
        public ActionResult PrintPageLocoKoli(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
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
        public ActionResult GeneratePDFLocoKoli(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
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

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliDizelTableAdapter ListKhat = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliDizelTableAdapter();

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();


                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');

                var logoId = m.prs_RptHeaderContract_Dizel(cc[0]).FirstOrDefault().fldLogoId;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                List_info.Fill(dt.prs_RptHeaderContract_Dizel, cc[0]);

                FastReport.Report Report = new FastReport.Report();


                ListKhat.Fill(dt.prs_RptKoliDizel, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptLocoKoli.frx");


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


                //if (cc.Length > 2)
                //{
                //    for (int i = 1; i < cc.Length - 1; i++)
                //        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 0, TypevagonPrint);
                //}

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
        public FileResult GenerateXlsLocoKoli(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
        {

            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var cc = ContractId.Split(',');
            ContractId = cc[0];
            var RptHeaderContract = m.prs_RptHeaderContract_Dizel(ContractId).FirstOrDefault();
            int? logoId = RptHeaderContract.fldLogoId;
            FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\LocoKoli.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/

            MemoryStream LogoPic = null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            //  Workbook workbook2 = new Workbook();

            //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
            //var worksheet = workbook2.Worksheets["Sheet1"];


            Workbook workbook2 = new Workbook(fstreamExcelJozee);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet = workbook2.Worksheets[0];



            Models.ProgressBar pro = new Models.ProgressBar();

            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliDizelTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptKoliDizelTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptHeaderContract_Dizel, ContractId);
            workbook2 = new Workbook(fstreamExcelJozee);

            int fixrow = 12;

            var k = 0;
            try
            {

                k = List.Fill(dt.prs_RptKoliDizel, StartDate, EndDate, MalekId, ShVagon, ContractId, AzVagon, TaVagon);
                int i = 0;
                decimal s1 = 0;
                decimal s2 = 0;
                for (i = 0; i < dt.prs_RptKoliDizel.Count; i++)
                {
                    worksheet.Cells[i + fixrow, 0].Value = i + 1;
                    worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptKoliDizel.Rows[i]["fldDizel_No"];

                    if ((dt.prs_RptKoliDizel.Rows[i]["MinTarikhBar"]).ToString() == "0" || (dt.prs_RptKoliDizel.Rows[i]["MinTarikhBar"]).ToString() == null || (dt.prs_RptKoliDizel.Rows[i]["MinTarikhBar"]).ToString() == "null")
                        worksheet.Cells[i + fixrow, 2].Value = "";
                    else
                        worksheet.Cells[i + fixrow, 2].Value = (dt.prs_RptKoliDizel.Rows[i]["MinTarikhBar"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptKoliDizel.Rows[i]["MinTarikhBar"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptKoliDizel.Rows[i]["MinTarikhBar"]).ToString().Substring(6, 2);


                    worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptKoliDizel.Rows[i]["fldTedadBarname"];
                    worksheet.Cells[i + fixrow, 4].Value = dt.prs_RptKoliDizel.Rows[i]["fldSumTonazhBar"];
                    worksheet.Cells[i + fixrow, 5].Value = dt.prs_RptKoliDizel.Rows[i]["fldSumMasafat"];
                    worksheet.Cells[i + fixrow, 6].Value = dt.prs_RptKoliDizel.Rows[i]["fldSumTonKolimetr"];
                    s1 = s1 + Convert.ToDecimal(dt.prs_RptKoliDizel.Rows[i]["fldSumMasafat"]);
                    s2 = s2 + Convert.ToDecimal(dt.prs_RptKoliDizel.Rows[i]["fldSumTonKolimetr"]);
                }
                worksheet.Cells[dt.prs_RptKoliDizel.Count + fixrow, 5].Value = s1;
                worksheet.Cells[dt.prs_RptKoliDizel.Count + fixrow, 6].Value = s2;
                worksheet.Cells[dt.prs_RptKoliDizel.Count + fixrow, 4].Value = "جمـــــــع";
                worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش کلی عملکرد طرح توسعه زیرساخت حمل و نقل ریلی " + "\n" + " از تاریخ" + StartDate + " تا تاریخ " + EndDate;
                worksheet.Cells["F1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet.Cells["F3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet.Cells["F5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                worksheet.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad;
                worksheet.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                worksheet.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                worksheet.Cells["E10"].Value = "توان لکوموتیو: ";

               
                if (LogoPic != null)
                    worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);


                /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                m.prs_tblErrorInsert(ErrorId, dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"] + " از تاریخ" + StartDate + " تا تاریخ " + EndDate + "*" + ErMsg, DateTime.Now, Input.fldId, "");

                worksheet.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                worksheet.Cells["K1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet.Cells["K3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet.Cells["K5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                worksheet.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad;
                worksheet.Cells["F8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                worksheet.Cells["F9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                worksheet.Cells["F10"].Value = "توان لکوموتیو: ";

                //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                //rng1.Merge();
                //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                if (LogoPic != null)
                    worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
            }

            var stream2 = new MemoryStream();
            Workbook workbook = new Workbook();

            workbook.Worksheets["Sheet1"].Copy(worksheet);

            workbook.Save(stream2, SaveFormat.Xlsx);

            return File(stream2.ToArray(), "xlsx", "BarnameLoco.xlsx");

        }
        //***
        public ActionResult PrintPageBarnameLoco(string containerId, string ContractId,  string TypeVagon)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            partial.ViewBag.ContractId = ContractId;
            partial.ViewBag.TypeVagon = TypeVagon;

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult GeneratePDFBarnameLoco( string ContractId, string TypeVagon)
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

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelFirstBarnameTableAdapter ListKhat = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelFirstBarnameTableAdapter();

                RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter();
                
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameBariTableAdapter List2 = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameBariTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameMosaferiTableAdapter List3 = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameMosaferiTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderForAvalinTarikhBarnameTableAdapter List_info2 = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderForAvalinTarikhBarnameTableAdapter(); 
                
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();


                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);

                var cc = ContractId.Split(',');

                var logoId = m.prs_RptHeaderContract_Dizel(cc[0]).FirstOrDefault().fldLogoId;
                logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

                List_info.Fill(dt.prs_RptHeaderContract_Dizel, cc[0]);
                List_info2.Fill(dt.prs_RptHeaderForAvalinTarikhBarname, Convert.ToInt32(cc[0]));

                FastReport.Report Report = new FastReport.Report();

                if (TypeVagon == "2")
                {
                    ListKhat.Fill(dt.prs_RptDizelFirstBarname, cc[0]);
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptBarnameLoco.frx");
                }
                else if (TypeVagon == "1")
                    {
                        List3.Fill(dt.prs_RptAvalinTarikhBarnameMosaferi, Convert.ToInt32(cc[0]));
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptFirstBarnameMosaferi.frx");
                    }
                else if (TypeVagon == "3")
                {
                    List2.Fill(dt.prs_RptAvalinTarikhBarnameBari, Convert.ToInt32(cc[0]));
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptFirstBarnameBari.frx");
                }


                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("IsExcel", 0);
                Report.SetParameterValue("OnlyData", 0);
                Report.Prepare();


                //if (cc.Length > 2)
                //{
                //    for (int i = 1; i < cc.Length - 1; i++)
                //        Report = CombineMultipleReports(Report, StartDate, EndDate, MalekId, cc[i], ShVagon, AzVagon, TaVagon, TypeVagon, 0, TypevagonPrint);
                //}

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
        public FileResult GenerateXlsBarnameLoco( string ContractId,  string TypeVagon)
        {

            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var cc = ContractId.Split(',');
            ContractId = cc[0];

            var RptHeaderContract = m.prs_RptHeaderContract_Dizel(ContractId).FirstOrDefault();
            int? logoId = RptHeaderContract.fldLogoId;
            FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\FirstBarnameLoco.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            
                FileStream fstreamExcelJozee1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\FirstBarnameMosaferi.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
         
                FileStream fstreamExcelJozee3 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\FirstBarnameBari.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/

            MemoryStream LogoPic = null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            //  Workbook workbook2 = new Workbook();

            //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
            //var worksheet = workbook2.Worksheets["Sheet1"];


            Workbook workbook2 = new Workbook(fstreamExcelJozee);
            Workbook workbook1 = new Workbook(fstreamExcelJozee1);
            Workbook workbook3 = new Workbook(fstreamExcelJozee3);




            Models.ProgressBar pro = new Models.ProgressBar();

            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelFirstBarnameTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptDizelFirstBarnameTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter();

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameBariTableAdapter List2 = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameBariTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameMosaferiTableAdapter List3 = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptAvalinTarikhBarnameMosaferiTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderForAvalinTarikhBarnameTableAdapter List_info2 = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderForAvalinTarikhBarnameTableAdapter(); 
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptHeaderContract_Dizel, ContractId);

            List_info2.Fill(dt.prs_RptHeaderForAvalinTarikhBarname, Convert.ToInt32(ContractId));

            workbook2 = new Workbook(fstreamExcelJozee);
            workbook1 = new Workbook(fstreamExcelJozee1);
            workbook3 = new Workbook(fstreamExcelJozee3);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet2 = workbook2.Worksheets[0];
            Worksheet worksheet1 = workbook1.Worksheets[0];
            Worksheet worksheet3 = workbook3.Worksheets[0];

            int fixrow = 12;

            var k = 0;
            try
            {
                if (TypeVagon == "2")
                {
                    k = List.Fill(dt.prs_RptDizelFirstBarname, ContractId);
                    int i = 0;
                    decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptDizelFirstBarname.Count; i++)
                    {
                        worksheet2.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet2.Cells[i + fixrow, 1].Value = dt.prs_RptDizelFirstBarname.Rows[i]["fldDizel_No"];
                        worksheet2.Cells[i + fixrow, 2].Value = dt.prs_RptDizelFirstBarname.Rows[i]["fldTrain_NO"];
                        //worksheet.Cells[i + fixrow, 3].Value = dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainKind"];

                        if ((dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString() == "" || (dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString() == "0" || (dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString() == "_" || (dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString() == null || (dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString() == "null")
                            worksheet2.Cells[i + fixrow, 3].Value = "";
                        else
                            worksheet2.Cells[i + fixrow, 3].Value = (dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptDizelFirstBarname.Rows[i]["fldTrainDate"]).ToString().Substring(6, 2);

                        worksheet2.Cells[i + fixrow, 4].Value = dt.prs_RptDizelFirstBarname.Rows[i]["fldWagon_No"];
                        worksheet2.Cells[i + fixrow, 5].Value = dt.prs_RptDizelFirstBarname.Rows[i]["fldIsMade12"];

                        if ((dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString() == "0" || (dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString() == "" || (dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString() == "_" || (dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString() == null || (dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString() == "null")
                            worksheet2.Cells[i + fixrow, 6].Value = "";
                        else
                            worksheet2.Cells[i + fixrow, 6].Value = (dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptDizelFirstBarname.Rows[i]["MinTarikhBar"]).ToString().Substring(6, 2);

                        worksheet2.Cells[i + fixrow, 7].Value = dt.prs_RptDizelFirstBarname.Rows[i]["fldBarnameId"];
                    }
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";

                    worksheet2.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش اولین بارنامه لوکوموتیو ";
                    worksheet2.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet2.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet2.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet2.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                    worksheet2.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad;
                    worksheet2.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                    worksheet2.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                    worksheet2.Cells["E10"].Value = "توان لکوموتیو: ";

                    //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptDizel.Count).ToString(), "K" + (fixrow + dt.prs_RptDizel.Count).ToString());
                    //rng1.Merge();
                    //worksheet.Cells["A" + (fixrow + dt.prs_RptBarnameStation.Count+1).ToString()].Value = "جمـــــــع";

                    if (LogoPic != null)
                        worksheet2.Pictures.Add(1, 1, 6, 2, LogoPic);
                }
                else if (TypeVagon == "1")
                {
                    k = List3.Fill(dt.prs_RptAvalinTarikhBarnameMosaferi, Convert.ToInt32(ContractId));
                    int i = 0;
                    decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptAvalinTarikhBarnameMosaferi.Count; i++)
                    {
                        worksheet1.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet1.Cells[i + fixrow, 1].Value = dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldShomareVagon"];

                        if ((dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString() == "" || (dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString() == "0" || (dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString() == "_" || (dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString() == null || (dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString() == "null")
                            worksheet1.Cells[i + fixrow, 2].Value = "";
                        else
                            worksheet1.Cells[i + fixrow, 2].Value = (dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAvalinTarikhBarnameMosaferi.Rows[i]["fldPersianTrainDate"]).ToString().Substring(6, 2);

                       
                    }
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";

                    worksheet1.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش اولین بارنامه مسافری ";
                    worksheet1.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet1.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet1.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet1.Cells["A9"].Value = dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["nameMalek"];
                    worksheet1.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldmehvar"] + "-" + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldTedad"];
                    worksheet1.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldShomareGharadad"];
                    worksheet1.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldTarikhGhrardad"];
                    worksheet1.Cells["E10"].Value = "ظرفیت واگن: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldZarfiyatVagon"];

                    //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptDizel.Count).ToString(), "K" + (fixrow + dt.prs_RptDizel.Count).ToString());
                    //rng1.Merge();
                    //worksheet.Cells["A" + (fixrow + dt.prs_RptBarnameStation.Count+1).ToString()].Value = "جمـــــــع";

                    if (LogoPic != null)
                        worksheet1.Pictures.Add(1, 1, 6, 2, LogoPic);
                }
                else if (TypeVagon == "3")
                {
                    k = List2.Fill(dt.prs_RptAvalinTarikhBarnameBari, Convert.ToInt32(ContractId));
                    int i = 0;
                    decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptAvalinTarikhBarnameBari.Count; i++)
                    {
                        worksheet3.Cells[i + fixrow, 0].Value = i + 1;
                        worksheet3.Cells[i + fixrow, 1].Value = dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldShomareVagon"];
                        worksheet3.Cells[i + fixrow, 2].Value = dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldSh_barname"];

                        if ((dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString() == "" || (dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString() == "0" || (dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString() == "_" || (dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString() == null || (dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString() == "null")
                            worksheet3.Cells[i + fixrow, 3].Value = "";
                        else
                            worksheet3.Cells[i + fixrow, 3].Value = (dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptAvalinTarikhBarnameBari.Rows[i]["fldDateBarname"]).ToString().Substring(6, 2);


                    }
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 11].Formula = "=SUM(L" + (fixrow + 1) + ":L" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";
                    //worksheet.Cells[dt.prs_RptDizel.Count + fixrow, 12].Formula = "=SUM(M" + (fixrow + 1) + ":M" + (fixrow + dt.prs_RptDizel.Count).ToString() + ")";

                    worksheet3.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش اولین بارنامه باری ";
                    worksheet3.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                    worksheet3.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                    worksheet3.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                    worksheet3.Cells["A9"].Value = dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["nameMalek"];
                    worksheet3.Cells["A10"].Value = "نوع و تعداد واگن: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldmehvar"] + "-" + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldTedad"];
                    worksheet3.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldShomareGharadad"];
                    worksheet3.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldTarikhGhrardad"];
                    worksheet3.Cells["E10"].Value = "ظرفیت واگن: " + dt.prs_RptHeaderForAvalinTarikhBarname.Rows[0]["fldZarfiyatVagon"];

                    //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptDizel.Count).ToString(), "K" + (fixrow + dt.prs_RptDizel.Count).ToString());
                    //rng1.Merge();
                    //worksheet.Cells["A" + (fixrow + dt.prs_RptBarnameStation.Count+1).ToString()].Value = "جمـــــــع";

                    if (LogoPic != null)
                        worksheet3.Pictures.Add(1, 1, 6, 2, LogoPic);
                }

                /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                m.prs_tblErrorInsert(ErrorId, dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"]  + "*" + ErMsg, DateTime.Now, Input.fldId, "");

                worksheet2.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                worksheet2.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet2.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet2.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet2.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                worksheet2.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad;
                worksheet2.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                worksheet2.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                worksheet2.Cells["E10"].Value = "توان لکوموتیو: ";

                //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                //rng1.Merge();
                //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                if (LogoPic != null)
                    worksheet2.Pictures.Add(1, 1, 6, 2, LogoPic);
            }

            var stream2 = new MemoryStream();
            Workbook workbook = new Workbook();

            if (TypeVagon == "2")
            workbook.Worksheets["Sheet1"].Copy(worksheet2);
            else if(TypeVagon == "1")
                workbook.Worksheets["Sheet1"].Copy(worksheet1);
            else if (TypeVagon == "3")
                workbook.Worksheets["Sheet1"].Copy(worksheet3);

          //  workbook=GenerateXlsBarnameLocoGroup(ContractId, TypeVagon, workbook);

            workbook.Save(stream2, SaveFormat.Xlsx);

            return File(stream2.ToArray(), "xlsx", "FirstBarname.xlsx");

        }
        public FileResult GenerateXlsBarnameLocoGroup(string ContractId, string TypeVagon)
        {

            Models.RaiSamEntities m = new RaiSamEntities();
            var cc = ContractId.Split(',');
            ContractId = cc[0];
            var RptHeaderContract = m.prs_RptHeaderContract_Dizel(ContractId).FirstOrDefault();
            int? logoId = RptHeaderContract.fldLogoId;
            FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\FirstBarnameLoco.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/

            MemoryStream LogoPic = null;
            if (logoId != null)
            {
                var sk = m.prs_tblUploadFileCompanySelect("fldId", logoId.ToString(), 1).FirstOrDefault();
                if (sk != null)
                    LogoPic = new MemoryStream(sk.fldFile);
            }

            //  Workbook workbook2 = new Workbook();

            //var workbook2 = ExcelFile.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MosaferiJozee.xlsx");
            //var worksheet = workbook2.Worksheets["Sheet1"];


            Workbook workbook2 = new Workbook(fstreamExcelJozee);

            // Accessing the first worksheet in the Excel file
            Worksheet worksheet = workbook2.Worksheets[1];



            Models.ProgressBar pro = new Models.ProgressBar();

            var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
            dd.Fill(dt.prs_GetDate);

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptFirstTarikhOneDizelTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptFirstTarikhOneDizelTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptHeaderContract_DizelTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_RptHeaderContract_Dizel, ContractId);
            

            int fixrow = 12;

            var k = 0;
            try
            {

                k = List.Fill(dt.prs_RptFirstTarikhOneDizel, ContractId);
                int i = 0;
                decimal s1 = 0;
                decimal s2 = 0;
                for (i = 0; i < dt.prs_RptFirstTarikhOneDizel.Count; i++)
                {
                    worksheet.Cells[i + fixrow, 0].Value = i + 1;
                    worksheet.Cells[i + fixrow, 1].Value = dt.prs_RptFirstTarikhOneDizel.Rows[i]["fldDizel_No"];

                    if ((dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString() == "" || (dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString() == "0" || (dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString() == "_" || (dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString() == null || (dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString() == "null")
                        worksheet.Cells[i + fixrow, 2].Value = "";
                    else
                        worksheet.Cells[i + fixrow, 2].Value = (dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString().Substring(0, 4) + "/" + (dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString().Substring(4, 2) + "/" + (dt.prs_RptFirstTarikhOneDizel.Rows[i]["Tarikh"]).ToString().Substring(6, 2);

                   
                }
               

                worksheet.Cells["C1"].Value = "سامانه جامع و هوشمند ماده 12" + "\n" + " گزارش اولین بارنامه لوکوموتیو ";
                worksheet.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                worksheet.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad;
                worksheet.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                worksheet.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                worksheet.Cells["E10"].Value = "توان لکوموتیو: ";

                //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptDizel.Count).ToString(), "K" + (fixrow + dt.prs_RptDizel.Count).ToString());
                //rng1.Merge();
                //worksheet.Cells["A" + (fixrow + dt.prs_RptBarnameStation.Count+1).ToString()].Value = "جمـــــــع";

                if (LogoPic != null)
                    worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);


                /* h.AddProgress("جزئی " + NameFile, (decimal)1, user.UserInputId, idprog, 2);*/
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
                m.prs_tblErrorInsert(ErrorId, dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"] + "*" + ErMsg, DateTime.Now, Input.fldId, "");

                worksheet.Cells["C1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                worksheet.Cells["G1"].Value = "تاریخ چاپ: " + m.prs_GetDate().FirstOrDefault().fldTarikh;
                worksheet.Cells["G3"].Value = "ساعت چاپ: " + m.prs_GetDate().FirstOrDefault().fldTime;
                worksheet.Cells["G5"].Value = "کاربر چاپ کننده: " + q.fldNamePersonal;
                worksheet.Cells["A9"].Value = dt.prs_RptHeaderContract_Dizel.Rows[0]["fldNameCompany"];
                worksheet.Cells["A10"].Value = "نوع و تعداد لکوموتیو: " + RptHeaderContract.fldTedad;
                worksheet.Cells["E8"].Value = "شماره قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldShomare_GH"];
                worksheet.Cells["E9"].Value = "تارخ قرارداد: " + dt.prs_RptHeaderContract_Dizel.Rows[0]["fldTarikh_GH"];
                worksheet.Cells["E10"].Value = "توان لکوموتیو: ";

                //Aspose.Cells.Range rng1 = worksheet.Cells.CreateRange("A" + (fixrow + dt.prs_RptBarname.Count).ToString(), "K" + (fixrow + dt.prs_RptBarname.Count).ToString());
                //rng1.Merge();
                //worksheet.Cells["A" + (fixrow + dt.prs_RptBarname.Count+1).ToString()].Value = "جمـــــــع";

                if (LogoPic != null)
                    worksheet.Pictures.Add(1, 1, 6, 2, LogoPic);
            }

            var stream2 = new MemoryStream();
            Workbook workbook = new Workbook();

            workbook.Worksheets["Sheet1"].Copy(worksheet);

            //  workbook=GenerateXlsBarnameLocoGroup(ContractId, TypeVagon, workbook);

            workbook.Save(stream2, SaveFormat.Xlsx);

            return File(stream2.ToArray(), "xlsx", "FirstBarnameLocoGroup.xlsx");

            

        }
    }
}
