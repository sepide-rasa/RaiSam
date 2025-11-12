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

namespace RaiSam.Controllers.Report
{
    public class RptCarbargController : Controller
    {
        //
        // GET: /RptCarbarg/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
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
        public ActionResult PrintPage(string containerId, string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
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
        public ActionResult GeneratePDF(string StartDate, string EndDate, string MalekId, string ContractId, string ShVagon, string AzVagon, string TaVagon, string TypeVagon)
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


                ListKhat.Fill(dt.prs_RptDizel, StartDate, EndDate, MalekId, ShVagon, cc[0], AzVagon, TaVagon);
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
        public FileResult GenerateXls(string StartDate, string EndDate, string ContractId, string TypeVagon)
        {

            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var cc = ContractId.Split(',');
            ContractId = cc[0];
            int? logoId = m.prs_RptHeaderContract_Dizel(ContractId).FirstOrDefault().fldLogoId;

            string FileN = @"\Reports\Carbarg.xlsx";
            if (TypeVagon == "1")
                FileN = @"\Reports\Carbarg-Mosaferi.xlsx";
            FileStream fstreamExcelJozee = new FileStream(AppDomain.CurrentDomain.BaseDirectory + FileN, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            UserInfo user = (UserInfo)(Session["User"]);
            SignalrHub h = new SignalrHub();
            /*  h.AddProgress("جزئی " + NameFile, (decimal)0.25, user.UserInputId, idprog, 2);*/


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

            RaiSam.DataSet.DataSet1TableAdapters.prs_RptCabBargTableAdapter List = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptCabBargTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblContract_ProjectSelectTableAdapter List_info = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblContract_ProjectSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter logo = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblUploadFileCompanySelectTableAdapter();

            logo.Fill(dt.prs_tblUploadFileCompanySelect, "fldId", logoId.ToString(), 1);

            FastReport.Report Report = new FastReport.Report();
            dt.EnforceConstraints = false;
            Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
            dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
            List_info.Fill(dt.prs_tblContract_ProjectSelect,"fldId", ContractId,"",0,0);
            workbook2 = new Workbook(fstreamExcelJozee);

            int fixcol= 2;

            var k = 0;
            try
            {
                if (TypeVagon == "3")
                    k = List.Fill(dt.prs_RptCabBarg, "Bari", StartDate, EndDate, Convert.ToInt32(ContractId), user.UserInputId);
                else if (TypeVagon == "1")
                    k = List.Fill(dt.prs_RptCabBarg, "Mosaferi", StartDate, EndDate, Convert.ToInt32(ContractId), user.UserInputId);
                else if (TypeVagon == "2")
                    k = List.Fill(dt.prs_RptCabBarg, "Dizel", StartDate, EndDate, Convert.ToInt32(ContractId), user.UserInputId);

                int i = 0;
                decimal s1 = 0;
                    decimal s2 = 0;
                    for (i = 0; i < dt.prs_RptCabBarg.Count; i++)
                    {
                        worksheet.Cells[10, fixcol + i].Value = dt.prs_RptCabBarg.Rows[i]["fldfasl"] + "" + dt.prs_RptCabBarg.Rows[i]["fldSal"];
                        worksheet.Cells[11, fixcol + i].Value = dt.prs_RptCabBarg.Rows[i]["fldTonkilometr"];
                        worksheet.Cells[12, fixcol + i].Value = dt.prs_RptCabBarg.Rows[i]["fldTon_sarfeJooie"];
                        worksheet.Cells[13, fixcol + i].Value = dt.prs_RptCabBarg.Rows[i]["fldDolar"];

                    }

                    worksheet.Cells["C3"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldTitle"];
                    worksheet.Cells["C4"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldTarikhContract"];
                    worksheet.Cells["J4"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldShomareContract"];
                    worksheet.Cells["C5"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldTarikhMovafeghat"];
                    worksheet.Cells["J5"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldShomareMovafeghat"];
                    worksheet.Cells["C6"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldSaghfSarmayeGozari"];

               

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
                m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                worksheet.Cells["A1"].Value = "خطا " + ErrorId.Value + " در بارگذاری اطلاعات";
                worksheet.Cells["C3"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldTitle"];
                worksheet.Cells["C4"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldTarikhContract"];
                worksheet.Cells["J4"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldShomareContract"];
                worksheet.Cells["C5"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldTarikhMovafeghat"];
                worksheet.Cells["J5"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldShomareMovafeghat"];
                worksheet.Cells["C6"].Value = dt.prs_tblContract_ProjectSelect.Rows[0]["fldSaghfSarmayeGozari"];

            }

            var stream2 = new MemoryStream();
            Workbook workbook = new Workbook();

            workbook.Worksheets["Sheet1"].Copy(worksheet);

            workbook.Save(stream2, SaveFormat.Xlsx);

            return File(stream2.ToArray(), "xlsx", "کاربرگ.xlsx");

        }
    }
}
