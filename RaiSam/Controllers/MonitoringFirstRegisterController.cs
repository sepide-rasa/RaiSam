using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.IO;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text.RegularExpressions;
using Aspose.Cells;
using RaiSam.Models;

namespace RaiSam.Controllers
{
    public class MonitoringFirstRegisterController : Controller
    {
        //
        // GET: /MonitoringFirstRegister/

        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                var q = m.prs_GetDate().FirstOrDefault();
                PartialView.ViewBag.fldTarikhE = q.fldDateTime.ToString("yyyy-MM-dd");
                PartialView.ViewBag.fldTarikh_Sh = q.fldTarikh;
                return PartialView;
            }

        }
        public ActionResult Help()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpFirst()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "35", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinFirst()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoFirst()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "35", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult newPayam()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                return PartialView;
            }
        }
        public FileResult CreateExcel(string Checked, string AzTarikh, string TaTarikh)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC" };
            int index = 0;
            var StatusCheck = Checked.Split(';');
            var Check = "";
            var Flag = ""; var Sms = ""; var Email = ""; var UserName = ""; var Mobile = "";
            var GroupName = ""; var Tarikh = ""; var Time = "";
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

            foreach (var item in StatusCheck)
            {
                var ListCompany = m.prs_ListCompany_First("FirstRegister", "", 0, AzTarikh, TaTarikh).ToList();
                switch (item)
                {
                    case "Flag":
                        Check = "وضعیت ایمیل";
                        Cell cell = sheet.Cells[alpha[index] + "1"];
                        cell.PutValue(Check);
                        int i = 0;
                        foreach (var _item in ListCompany)
                        {
                            Flag = _item.fldFlag;
                            if (Flag == "0")
                                Flag = "ارسال نشده";
                            else
                                Flag = "ارسال شده";
                            Cell Cell = sheet.Cells[alpha[index] + (i + 2)];
                            Cell.PutValue(Flag);
                            i++;
                        }
                        index++;
                        break;
                    case "Sms":
                        Check = "وضعیت پیامک";
                        Cell cell1 = sheet.Cells[alpha[index] + "1"];
                        cell1.PutValue(Check);
                        int j = 0;
                        foreach (var _item in ListCompany)
                        {
                            Sms = _item.fldSms;
                            if (Sms == "0")
                                Sms = "ارسال نشده";
                            else
                                Sms = "ارسال شده";
                            Cell Cell = sheet.Cells[alpha[index] + (j + 2)];
                            Cell.PutValue(Sms);
                            j++;
                        }
                        index++;
                        break;
                    case "Email":
                        Check = "پست الکترونیک";
                        Cell cell2 = sheet.Cells[alpha[index] + "1"];
                        cell2.PutValue(Check);
                        int k = 0;
                        foreach (var _item in ListCompany)
                        {
                            Email = _item.fldEmail;
                            Cell Cell = sheet.Cells[alpha[index] + (k + 2)];
                            Cell.PutValue(Email);
                            k++;
                        }
                        index++;
                        break;
                    case "UserName":
                        Check = "نام کاربری";
                        Cell cell3 = sheet.Cells[alpha[index] + "1"];
                        cell3.PutValue(Check);
                        int q = 0;
                        foreach (var _item in ListCompany)
                        {
                            UserName = _item.fldUserName;
                            Cell Cell = sheet.Cells[alpha[index] + (q + 2)];
                            Cell.PutValue(UserName);
                            q++;
                        }
                        index++;
                        break;
                    case "Mobile":
                        Check = "شماره موبایل";
                        Cell cell4 = sheet.Cells[alpha[index] + "1"];
                        cell4.PutValue(Check);
                        int w = 0;
                        foreach (var _item in ListCompany)
                        {
                            Mobile = _item.fldMobile;
                            Cell Cell = sheet.Cells[alpha[index] + (w + 2)];
                            Cell.PutValue(Mobile);
                            w++;
                        }
                        index++;
                        break;
                    //case "GroupName":
                    //    Check = "هدف از ثبت نام";
                    //    Cell cell5 = sheet.Cells[alpha[index] + "1"];
                    //    cell5.PutValue(Check);
                    //    int z = 0;
                    //    foreach (var _item in ListCompany)
                    //    {
                    //        GroupName = _item.fldGroupName;
                    //        Cell Cell = sheet.Cells[alpha[index] + (z + 2)];
                    //        Cell.PutValue(GroupName);
                    //        z++;
                    //    }
                    //    index++;
                    //    break;
                    case "Tarikh":
                        Check = "تاریخ";
                        Cell cell6 = sheet.Cells[alpha[index] + "1"];
                        cell6.PutValue(Check);
                        int x = 0;
                        foreach (var _item in ListCompany)
                        {
                            Tarikh = _item.fldTarikh;
                            Cell Cell = sheet.Cells[alpha[index] + (x + 2)];
                            Cell.PutValue(Tarikh);
                            x++;
                        }
                        index++;
                        break;
                    case "Time":
                        Check = "زمان";
                        Cell cell7 = sheet.Cells[alpha[index] + "1"];
                        cell7.PutValue(Check);
                        int y = 0;
                        foreach (var _item in ListCompany)
                        {
                            Time = _item.fldTime;
                            Cell Cell = sheet.Cells[alpha[index] + (y + 2)];
                            Cell.PutValue(Time);
                            y++;
                        }
                        index++;
                        break;
                }
            }
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Excel97To2003);
            return File(stream.ToArray(), "xls", "MonitoringFirstRegister.xls");
        }
        public ActionResult ersalnewPayam(string Desc)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                MsgTitle = "ارسال موفق";
                Msg = "با موفقیت ارسال شد.";
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                var MatnPayam = "احتراما" + Environment.NewLine + Desc + Environment.NewLine + "راه آهن جمهوری اسلامی ایران";

                RaiSms.Service w = new RaiSms.Service();
                var F = m.prs_tblFirstRegisterSelect("", "", "", 0).ToList();
                foreach (var item in F)
                {
                    var ReturnMsg = "ارسال موفق";
                    var st = true;
                    try
                    {
                        string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnPayam, item.fldMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");
                        if (returnCode.Length <= 3)
                        {
                            ReturnMsg = w.ShowError(returnCode, "FA");
                            st = false;
                        }
                    }
                    catch (Exception)
                    {
                        ReturnMsg = "قطع ارتباط با سرور.";
                        st = false;
                    }
                    m.prs_tblSMSEmailCachInsert(MatnPayam, item.fldMobile, "", ReturnMsg, item.fldName + " " + item.fldFamily, false, false,  item.fldId,st,user.UserInputId);
                }



                //if (Err.ErrorType)
                //{
                //    MsgTitle = "خطا";
                //    Msg = Err.ErrorMsg;
                //    Er = 1;
                //}
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
                Er = 1;
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Read(StoreRequestParameters parameters, string AzTarikh, string TaTarikh)
        {

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            if (AzTarikh == null)
                AzTarikh = "";
            if (TaTarikh == null)
                TaTarikh = "";
            List<Models.prs_ListCompany_First> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<Models.prs_ListCompany_First> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        case "fldId":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "FirstRegister_fldId";
                            break;
                        case "fldEmail":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "FirstRegister_fldEmail";
                            break;
                        case "fldUserName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "FirstRegister_fldUserName";
                            break;
                        case "fldMobile":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "FirstRegister_fldMobile";
                            break;
                        case "fldGroupName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "FirstRegister_fldGroupName";
                            break;
                        case "fldTarikh":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "FirstRegister_fldTarikh";
                            break;
                        case "fldTime":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "FirstRegister_fldTime";
                            break;

                    }
                    if (data != null)
                        data1 = m.prs_ListCompany_First(field, searchtext, 100, AzTarikh, TaTarikh).ToList();
                    else
                        data = m.prs_ListCompany_First(field, searchtext, 100, AzTarikh, TaTarikh).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_ListCompany_First("FirstRegister", "", 100, AzTarikh, TaTarikh).ToList();
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

            List<Models.prs_ListCompany_First> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        public ActionResult Delete(int id)
        {
            string Msg = "حذف با موفقیت انجام شد", MsgTitle = ""; var Er = 0;

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                //حذف

                //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 5))
                //{
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                MsgTitle = "حذف موفق";
                var co = m.prs_tblFirstRegister_UserIdDelete(id).FirstOrDefault();
                if (co.ErrorCode != 0)
                    Msg = co.ErrorMessage;
                //}
                //else
                //{
                //    return null;
                //}
                
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeneratePDF(string AzTarikh, string TaTarikh)
        {

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
                if (AzTarikh == null)
                    AzTarikh = "";
                if (TaTarikh == null)
                    TaTarikh = "";
                var q = m.prs_tblUserSelect("fldId",user.UserSecondId.ToString(),"",0).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();

                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);

                RaiSam.DataSet.DataSet1TableAdapters.prs_ListCompany_FirstTableAdapter ListCompany = new RaiSam.DataSet.DataSet1TableAdapters.prs_ListCompany_FirstTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();

                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
                ListCompany.Fill(dt.prs_ListCompany_First, "FirstRegister", "", 0, AzTarikh, TaTarikh);

                FastReport.Report Report = new FastReport.Report();
                //var Rp = servic.GetReportWithFilter("fldId", "7", 0, Session["Username"].ToString(), (Session["Password"].ToString()), out Err).FirstOrDefault();
                //MemoryStream stream1 = new MemoryStream(Rp.fldFile);
                //Report.Load(stream1);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MonitoringFirstRegister.frx");

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("AzTarikh", AzTarikh);
                Report.SetParameterValue("TaTarikh", TaTarikh);
                Report.Prepare();
                Report.Export(pdf, stream);
                return File(stream.ToArray(), "application/pdf");
            }
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Print(string containerId, string AzTarikh, string TaTarikh)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {

                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.AzTarikh = AzTarikh;
                PartialView.ViewBag.TaTarikh = TaTarikh;
                return PartialView;
            }
        }
        //public ActionResult Print()
        //{
        //    if (Session["UserName"] == null)
        //        return RedirectToAction("Login", "Admin");
        //    ReportViewer reportViewer = new ReportViewer();
        //    reportViewer.ProcessingMode = ProcessingMode.Remote;
        //    reportViewer.SizeToReportContent = true;
        //    reportViewer.ShowBackButton = true;
        //    reportViewer.ShowExportControls = true;
        //    reportViewer.ShowCredentialPrompts = true;
        //    reportViewer.ShowDocumentMapButton = true;
        //    reportViewer.ShowFindControls = true;
        //    reportViewer.ShowPrintButton = true;
        //    reportViewer.ShowZoomControl = true;
        //    reportViewer.ShowRefreshButton = true;
        //    reportViewer.ShowParameterPrompts = false;
        //    reportViewer.ServerReport.ReportServerUrl = new Uri(WebConfigurationManager.AppSettings["ReportServerUrl"]);
        //    var q = servic.GetUserDetail(Convert.ToInt32(Session["UserId"]), Session["Username"].ToString(), (Session["Password"].ToString()), IP, out Err);
        //    reportViewer.ServerReport.ReportPath = "/FirstRegisterListCompany";
        //    ReportParameter fieldname = new ReportParameter("fieldname", "FirstRegister");
        //    ReportParameter Value = new ReportParameter("Value", " ");
        //    ReportParameter h = new ReportParameter("h", "0");
        //    ReportParameter Uploadfile = new ReportParameter("Uploadfile", "fldid");
        //    ReportParameter UploadfileId = new ReportParameter("UploadfileId", "32");
        //    ReportParameter NameUser = new ReportParameter("NameUser", q.fldNamePersonal);
        //    reportViewer.ServerReport.SetParameters(new ReportParameter[] { fieldname, Value, h, Uploadfile, UploadfileId, NameUser });
        //    ViewBag.ReportViewer = reportViewer;
        //    return View();
        //} 
        public ActionResult SendSms(int id)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                //حذف

                //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 5))
                //{
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
                CaptchaImage img = new CaptchaImage(110, 40, family);
                string CodeTaeed = img.CreateRandomText(5).Replace('l', 's').Replace('i', 's').Replace('I', 'S').Replace('L', 'S');

                var F = m.prs_tblFirstRegisterSelect("fldId", id.ToString(),"",0).FirstOrDefault();
                m.prs_UpdateVerificationCode(CodeTaeed, F.fldMobile, F.fldEmail, F.fldUserName);
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();

                var MatnSMS = "ثبت نام شما با موفقیت انجام شد برای تکمیل ثبت نام کد زیر را در سامانه وارد نمایید." + Environment.NewLine + "کد تائید: " + CodeTaeed + Environment.NewLine + "راه آهن جمهوری اسلامی ایران";

                RaiSms.Service w = new RaiSms.Service();

                var ReturnMsg = "ارسال موفق";
                var st = true;
                try
                {
                    string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS, F.fldMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                    if (returnCode.Length <= 3)
                    {
                        MsgTitle = "خطا";
                        Msg = w.ShowError(returnCode, "FA");
                        ReturnMsg = w.ShowError(returnCode, "FA");
                        Er = 1;
                        st = false;
                    }
                    else
                    {
                        MsgTitle = "ارسال موفق";
                        Msg = "پیامک فعالسازی مجددا ارسال شد.";
                    }
                }
                catch (Exception)
                {
                    ReturnMsg = "قطع ارتباط با سرور.";
                    st = false;
                }
                m.prs_tblSMSEmailCachInsert(MatnSMS, F.fldMobile, "", ReturnMsg, F.fldName + " " + F.fldFamily, false, false,  F.fldId,st,user.UserInputId);
                //}
                //else
                //{
                //    return null;
                //}
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResetPassword(int id, string State)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
                //حذف

                if (Permission.haveAccess(195, "", "0"))
                {
                    System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
                    CaptchaImage img = new CaptchaImage(110, 40, family);
                    string pass = img.CreateRandomText(5).Replace('l', 's').Replace('i', 's').Replace('I', 'S').Replace('L', 'S');

                    if (State == "3")
                    {
                        m.prs_UpdatePasswordFirstRegister(CodeDecode.ComputeSha256Hash(pass), id);
                        MsgTitle = "عملیات موفق";
                        Msg = "رمز عبور جدید: " + pass;
                        Er = 0;
                    }
                    else
                    {

                        var F = m.prs_tblFirstRegisterSelect("fldId", id.ToString(), "", 0).FirstOrDefault();
                        m.prs_UpdatePasswordFirstRegister(CodeDecode.ComputeSha256Hash(pass), id);
                        var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();

                        var MatnSMS = "تغییر رمز عبور در سامانه ماده12 با موفقیت انجام شد." + Environment.NewLine + "نام کاربری: " + F.fldUserName + Environment.NewLine + "رمزعبور جدید: " + pass + Environment.NewLine;
                        var MatnEmail = "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>تغییر رمز عبور در سامانه ماده12 با موفقیت انجام شد .</p> <p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>رمزعبور جدید: " + pass + "</p>";
                        RaiSms.Service w = new RaiSms.Service();
                        var ReturnMsg = "ارسال موفق";
                        var st = true;
                        try
                        {
                            string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS + "راه آهن جمهوری اسلامی ایران", F.fldMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                            if (returnCode.Length <= 3)
                            {
                                MsgTitle = "خطا";
                                Msg = w.ShowError(returnCode, "FA");
                                ReturnMsg = w.ShowError(returnCode, "FA");
                                Er = 1;
                                st = false;
                            }
                            else
                            {
                                MsgTitle = "عملیات موفق";
                                Msg = "رمز عبور جدید برای نماینده ارسال شد." ;
                            }
                        }
                        catch (Exception)
                        {
                            ReturnMsg = "قطع ارتباط با سرور.";
                            st = false;
                        }
                        m.prs_tblSMSEmailCachInsert(Msg, F.fldMobile, "", ReturnMsg, F.fldName + " " + F.fldFamily, false, false, F.fldId, st,user.UserInputId);

                        //try
                        //{
                        //    ReturnMsg = "ارسال موفق";
                        //    st = true;
                        //    st = SendEmail(F.fldEmail, MatnEmail);
                        //}
                        //catch (Exception)
                        //{
                        //    ReturnMsg = "قطع ارتباط با سرور.";
                        //    st = false;
                        //}
                        //if (st == false)
                        //    ReturnMsg = "قطع ارتباط با سرور.";
                        //m.prs_tblSMSEmailCachInsert(Msg, "", F.fldEmail, ReturnMsg, F.fldName + " " + F.fldFamily, false, true, F.fldId, st, user.UserInputId);
                    }
                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "شما مجاز به تغییر رمزعبور نمی باشید.";
                    Er = 1;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
                Er = 1;
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        public bool SendEmail(string Email, string Matn)
        {

            Models.RaiSamEntities m = new RaiSamEntities();
            var p = m.prs_tblPageHtmlSelect("fldId", "1", 1).FirstOrDefault();
            var address = Regex.Replace(p.fldMohtavaHtml, "<.*?>", String.Empty);
            var EmailSetting = m.prs_tblEmailSettingSelect("", "", 1).FirstOrDefault();
            MailAddress from = new MailAddress(EmailSetting.fldAddressEmail);

            MailAddress to = new MailAddress(Email);
            MailMessage mail = new MailMessage(from, to);

            string savePath = Server.MapPath(@"~\Content\header.png");
            mail.IsBodyHtml = true;
            var inlineLogo = new LinkedResource(savePath);
            inlineLogo.ContentId = Guid.NewGuid().ToString();
            mail.Subject = "سامانه تشخیص ماده 12 راه آهن جمهوری اسلامی ایران";
            string body = string.Format(@"
                                    <img src=""cid:{0}"" />" +
                "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + Matn + "</p>" +
                "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
                "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
            , inlineLogo.ContentId);

            var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            view.LinkedResources.Add(inlineLogo);
            mail.AlternateViews.Add(view);
            SmtpClient smtp = new SmtpClient();
            smtp.Host = EmailSetting.fldSendServer;
            smtp.Port = EmailSetting.fldSendPort;
            smtp.EnableSsl = EmailSetting.fldSSL;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(
               EmailSetting.fldAddressEmail, EmailSetting.fldPassword);
            ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate,
                         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            smtp.Send(mail);
            return true;

            //return false;
        }

        public ActionResult SendMessage(string FirstRegisterIdS)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.FirstRegisterIdS = FirstRegisterIdS;
            return PartialView;
        }

        public ActionResult SendShMessage(string FirstRegisterIdS, string fldMatn, bool fldType)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                fldMatn = fldMatn.Replace("<div>", Environment.NewLine);
                var Matn = Regex.Replace(fldMatn, "<.*?>", String.Empty);
                RaiSms.Service w = new RaiSms.Service();
                var FirstRegisterIdSplit = FirstRegisterIdS.Split(';').Reverse().Skip(1).Reverse().ToList();
                MsgTitle = "ارسال موفق";
                Msg = "با موفقیت ارسال شد.";

                foreach (var item in FirstRegisterIdSplit)
                {

                    var q = m.prs_tblFirstRegisterSelect("fldId", item, "", 1).FirstOrDefault();
                    
                    if (fldType)
                    {
                        var ReturnMsg = "ارسال موفق";
                        var st = true;
                        try
                        {
                            var returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, q.fldMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                            if (returnCode.Length <= 3)
                            {
                                MsgTitle = "خطا";
                                Msg = w.ShowError(returnCode, "FA");
                                ReturnMsg = w.ShowError(returnCode, "FA");
                                Er = 1;
                                st = false;
                            }
                        }
                        catch (Exception)
                        {
                            ReturnMsg = "قطع ارتباط با سرور.";
                            st = false;
                        }
                        m.prs_tblSMSEmailCachInsert(Matn, q.fldMobile, "", ReturnMsg, q.fldName + " " + q.fldFamily, false, false,  q.fldId,st,user.UserInputId);
                    }
                    else
                    {
                        var ReturnMsg = "ارسال موفق";
                        var st = true;
                        try
                        {
                            st = SendEmail(q.fldEmail, Matn);
                        }
                        catch (Exception)
                        {
                            ReturnMsg = "قطع ارتباط با سرور.";
                            st = false;
                        }
                        if (st == false)
                            ReturnMsg = "قطع ارتباط با سرور.";
                        m.prs_tblSMSEmailCachInsert(Matn, "", q.fldEmail, ReturnMsg, q.fldName + " " + q.fldFamily, false, true, q.fldId, st, user.UserInputId);
                    }
                }

            }
            catch (Exception x)
            {
                return Json(new
                {
                    Msg = "خطا در دسترسی به سرور ارسال پیامک",
                    MsgTitle = "خطا",
                    Er = 1
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Help()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //    return PartialView;
        //}

    }
}
