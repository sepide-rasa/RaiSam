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
//using EASendMail;

namespace RaiSam.Controllers
{
    public class MonitoringRegisterCompleteController : Controller
    {
        //
        // GET: /MonitoringRegisterComplete/
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult partialView = new Ext.Net.MVC.PartialViewResult
                {
                    WrapByScriptTag = true,
                    ContainerId = containerId,
                    RenderMode = RenderMode.AddTo
                };
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_GetDate().FirstOrDefault();
                partialView.ViewBag.fldTarikhE = q.fldDateTime.ToString("yyyy-MM-dd");
                partialView.ViewBag.fldTarikh_Sh = q.fldTarikh;
                return partialView;
            }
        }
        public ActionResult Help()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpMonitor()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "36", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinMonitor()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoMonitor()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "36", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult TimeLine(string RequestId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Session["RequestId_TimeLine"] = RequestId;
            PartialView.ViewBag.RequestId = RequestId;
            return PartialView;
        }
        public ActionResult HistoryLogin_Comoany(string IdUser, string state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.IdUser = IdUser;
            PartialView.ViewBag.state = state;
            return PartialView;
        }

        public JsonResult TimeLineDetails(int id)
        {//نمایش اطلاعات جهت رویت کاربر

            if (Session["User"] == null)
                return null;

            //var q = servic.GetHistoryRequestWithFilter(id, Session["Username"].ToString(), Session["Password"].ToString(), out Err).ToList();
            var NameType = "";
            var type = "";
            var tarikh = "";
            var UserName = "";

            //foreach (var item in q)
            //{
            //    NameType = NameType + item.NameType + ',';
            //    type = type + item.fldType + ',';
            //    tarikh = tarikh + item.TarikhDashboard + ',';
            //    UserName = UserName + item.nameuser.Replace(',', ' ') + ',';
            //}
            return Json(new
            {
                NameType = NameType,
                type = type,
                tarikh = tarikh,
                UserName = UserName

            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ShowRequest(string Id, string State)
        {//State=1 -> Id=idCompaniyProfile, State=2 -> Id=idRealPerson

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.Id = Id;
                PartialView.ViewBag.State = State;
                return PartialView;
            }
        }
        public ActionResult ShowCacheSmsEmail(string Id, string State)
        {//State=1 -> Id=idCompaniyProfile, State=2 -> Id=idRealPerson

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                int? fldFirstRegisterId = 0;
                if (State == "1")
                {
                    var q = m.prs_tblCompanyProfileSelect("fldId", Id, 0).FirstOrDefault();
                    fldFirstRegisterId = q.fldFirstRegisterUser;
                }

                //else if (State == "2")
                //{
                //    var p = m.GetRealPersonLocationWithFilter("fldId", Id, 0, out Err1).FirstOrDefault();
                //    fldFirstRegisterId = p.fldFirstRegisterUser;
                //}
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.fldFirstRegisterId = fldFirstRegisterId;
                return PartialView;
            }
        }
        public ActionResult ReadCacheSmsEmail(StoreRequestParameters parameters, int fldFirstRegisterId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<Models.prs_tblSMSEmailCachSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<Models.prs_tblSMSEmailCachSelect> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        case "fldId":
                            searchtext = ConditionValue.Value.ToString();
                            field = "fldId";
                            break;
                        case "fldVazeyat":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldVazeyat";
                            break;
                        case "fldMatn":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldMatn";
                            break;
                        case "fldNameShakhs":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldNameShakhs";
                            break;
                        case "fldFullName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldFullName";
                            break;
                        case "fldShMobile":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldShMobile";
                            break;
                        case "fldEmail":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldEmail";
                            break;
                        case "fldNoeePayamName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldNoeePayamName";
                            break;
                        case "fldNoeeShakhsName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldNoeeShakhsName";
                            break;
                        case "fldUserName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldUserName";
                            break;
                    }
                    if (data != null)
                        data1 = m.prs_tblSMSEmailCachSelect(field, searchtext, 0).Where(l => l.fldFirstRegisterID == fldFirstRegisterId).ToList();
                    else
                        data = m.prs_tblSMSEmailCachSelect(field, searchtext, 0).Where(l => l.fldFirstRegisterID == fldFirstRegisterId).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_tblSMSEmailCachSelect("", "", 0).Where(l => l.fldFirstRegisterID == fldFirstRegisterId).ToList();
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

            List<Models.prs_tblSMSEmailCachSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        public ActionResult LoadShowRequest(string Id, string State)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Models.RaiSamEntities m = new RaiSamEntities();
            List<Models.prs_tblRequestRankingSelect> info = null;
            if (State == "1")
            {
                var q = m.prs_tblCompanyProfileSelect("fldId", Id, 0).FirstOrDefault();
                info = m.prs_tblRequestRankingSelect("fldFirstRegisterId", q.fldFirstRegisterId.ToString(), 0).ToList();
            }

            //else if (State == "2")
            //{
            //    var p = servic1.GetRealPersonLocationWithFilter("fldId", Id, 0, out Err1).FirstOrDefault();
            //    info = servic.GetRequestRankingWithFilter("fldFirstRegisterId", p.fldFirstRegisterUser.ToString(), false, "", 0, out Err).ToList();
            //}


            return Json(new { info = info }, JsonRequestBehavior.AllowGet);
        }
        public FileResult CreateExcel(string Checked, string AzTarikh, string TaTarikh, int Type, string Status)
        {
            if (Session["User"] == null)
                return null;
            string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
            int index = 0;
            var StatusCheck = Checked.Split(';');
            var Check = "";
            var CompanyStatus = ""; var FullName = ""; var GroupName = ""; var NameModirAmel = ""; var NationalCode = ""; var Namayande = "";
            var Sh_Sabt = ""; var Tarikh = ""; var Time = ""; var CodeEghtesadi = ""; var MobileModirAmel = ""; var Mobile = "";
            var FatherName = ""; string[] MahallSodoor = new string[3]; string[] MahallTavalod = new string[3]; var ShenasnameNo = "";
            var Namabar = ""; var Telphon = ""; var DateTasis = ""; var DateSabt = ""; var MablaghSaham = "";
            var Sh_Bime = ""; var Address = ""; string[] MahallSherkat = new string[3]; var CodePosti = ""; var AddressWebSite = ""; var EmailCompany = "";
            string[] MahallSabt = new string[3]; var NickName = ""; var TedadKolSaham = ""; var TypeCompany = ""; var TitleAllowed = "";
            List<Models.prs_ListCompanyForExcel> ListCompany = null;
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Type == 1)
            {
                if (Status == "0" || Status == "")
                {
                        ListCompany = m.prs_ListCompanyForExcel("Company", "", 0, AzTarikh, TaTarikh, "").ToList();
                }
                else//if (Convert.ToInt32(Status) < 5)
                {
                        ListCompany = m.prs_ListCompanyForExcel("Company", "", 0, AzTarikh, TaTarikh, Status).ToList();
                }
            }
            else if (Type == 2)
            {
                if (Status == "0" || Status == "")
                {
                        ListCompany = m.prs_ListCompanyForExcel("RealPerson", "", 0, AzTarikh, TaTarikh, "").ToList();
                }
                else if (Convert.ToInt32(Status) < 5)
                {
                        ListCompany = m.prs_ListCompanyForExcel("RealPerson", "", 0, AzTarikh, TaTarikh, Status).ToList();
                }
                else
                {
                        ListCompany = m.prs_ListCompanyForExcel("RealPerson_Status5", "", 0, AzTarikh, TaTarikh, Status).ToList();
                }
            }

            //Type=1 ->حقوقی  Type=2 ->حقیقی
            foreach (var item in StatusCheck)
            {
                switch (item)
                {
                    case "CompanyStatus":
                        Check = "وضعیت";
                        Cell cell = sheet.Cells[alpha[index] + "1"];
                        cell.PutValue(Check);
                        int i = 0;
                        foreach (var _item in ListCompany)
                        {
                            CompanyStatus = _item.fldCompanyStatus;
                            if (CompanyStatus == "1")
                                CompanyStatus = "ثبت نام موقت انجام شده";
                            else if (CompanyStatus == "2")
                                CompanyStatus = "ثبت قطعی در انتظار تائید وصول";
                            else if (CompanyStatus == "3")
                                CompanyStatus = "تائید وصول در انتظار تائید قطعی";
                            else if (CompanyStatus == "4")
                                CompanyStatus = "تائید قطعی انجام شده";
                            else if (CompanyStatus == "5")
                                CompanyStatus = "درخواست ثبت شده";
                            else if (CompanyStatus == "6")
                                CompanyStatus = "درخواست ارسال شده";
                            else if (CompanyStatus == "7")
                                CompanyStatus = "درخواست ارجاع داده شده";
                            else if (CompanyStatus == "8")
                                CompanyStatus = "یک درخواست دارای رتبه";
                            Cell Cell = sheet.Cells[alpha[index] + (i + 2)];
                            Cell.PutValue(CompanyStatus);
                            i++;
                        }
                        index++;
                        break;
                    case "FullName":
                        if (Type == 1)
                            Check = "نام شرکت";
                        else if (Type == 2)
                            Check = "نام اختصاری";
                        Cell cell1 = sheet.Cells[alpha[index] + "1"];
                        cell1.PutValue(Check);
                        int j = 0;
                        foreach (var _item in ListCompany)
                        {
                            FullName = _item.fldFullName;
                            Cell Cell = sheet.Cells[alpha[index] + (j + 2)];
                            Cell.PutValue(FullName);
                            j++;
                        }
                        index++;
                        break;
                    case "GroupName":
                        Check = "هدف از ثبت نام";
                        Cell cell2 = sheet.Cells[alpha[index] + "1"];
                        cell2.PutValue(Check);
                        int k = 0;
                        foreach (var _item in ListCompany)
                        {
                            GroupName = _item.fldHadafSabtName;
                            Cell Cell = sheet.Cells[alpha[index] + (k + 2)];
                            Cell.PutValue(GroupName);
                            k++;
                        }
                        index++;
                        break;
                    case "NameModirAmel":
                        if (Type == 1)
                            Check = "مدیر عامل";
                        else if (Type == 2)
                            Check = "نام";
                        Cell cell3 = sheet.Cells[alpha[index] + "1"];
                        cell3.PutValue(Check);
                        int q = 0;
                        foreach (var _item in ListCompany)
                        {
                            NameModirAmel = _item.NameModirAmel;
                            Cell Cell = sheet.Cells[alpha[index] + (q + 2)];
                            Cell.PutValue(NameModirAmel);
                            q++;
                        }
                        index++;
                        break;
                    case "NationalCode":
                        if (Type == 1)
                            Check = "شناسه ملی";
                        else if (Type == 2)
                            Check = "کد ملی";
                        Cell cell4 = sheet.Cells[alpha[index] + "1"];
                        cell4.PutValue(Check);
                        int w = 0;
                        foreach (var _item in ListCompany)
                        {
                            NationalCode = _item.fldNationalCode;
                            Cell Cell = sheet.Cells[alpha[index] + (w + 2)];
                            Cell.PutValue(NationalCode);
                            w++;
                        }
                        index++;
                        break;
                    case "Sh_Sabt":
                        Check = "شماره ثبت";
                        Cell cell5 = sheet.Cells[alpha[index] + "1"];
                        cell5.PutValue(Check);
                        int z = 0;
                        foreach (var _item in ListCompany)
                        {
                            Sh_Sabt = _item.fldSh_Sabt;
                            Cell Cell = sheet.Cells[alpha[index] + (z + 2)];
                            Cell.PutValue(Sh_Sabt);
                            z++;
                        }
                        index++;
                        break;
                    case "CodeEghtesadi":
                        Check = "کد اقتصادی";
                        Cell cell6 = sheet.Cells[alpha[index] + "1"];
                        cell6.PutValue(Check);
                        int x = 0;
                        foreach (var _item in ListCompany)
                        {
                            CodeEghtesadi = _item.fldCodeEghtesadi;
                            Cell Cell = sheet.Cells[alpha[index] + (x + 2)];
                            Cell.PutValue(CodeEghtesadi);
                            x++;
                        }
                        index++;
                        break;
                    case "Tarikh":
                        Check = "تاریخ";
                        Cell cell7 = sheet.Cells[alpha[index] + "1"];
                        cell7.PutValue(Check);
                        int t = 0;
                        foreach (var _item in ListCompany)
                        {
                            Tarikh = _item.fldTarikh;
                            Cell Cell = sheet.Cells[alpha[index] + (t + 2)];
                            Cell.PutValue(Tarikh);
                            t++;
                        }
                        index++;
                        break;
                    case "Time":
                        Check = "زمان";
                        Cell cell8 = sheet.Cells[alpha[index] + "1"];
                        cell8.PutValue(Check);
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
                    case "MobileModirAmel":
                        Check = "شماره موبایل";
                        Cell cell9 = sheet.Cells[alpha[index] + "1"];
                        cell9.PutValue(Check);
                        int U = 0;
                        foreach (var _item in ListCompany)
                        {
                            MobileModirAmel = _item.fldMobileModirAmel;
                            Cell Cell = sheet.Cells[alpha[index] + (U + 2)];
                            Cell.PutValue(MobileModirAmel);
                            U++;
                        }
                        index++;
                        break;
                    case "FatherName":
                        Check = "نام پدر";
                        Cell cell10 = sheet.Cells[alpha[index] + "1"];
                        cell10.PutValue(Check);
                        int A = 0;
                        foreach (var _item in ListCompany)
                        {
                            FatherName = _item.fldFatherName;
                            Cell Cell = sheet.Cells[alpha[index] + (A + 2)];
                            Cell.PutValue(FatherName);
                            A++;
                        }
                        index++;
                        break;
                    case "Namabar":
                        Check = "نمابر";
                        Cell cell18 = sheet.Cells[alpha[index] + "1"];
                        cell18.PutValue(Check);
                        int E = 0;
                        foreach (var _item in ListCompany)
                        {
                            Namabar = _item.fldNamabar;
                            Cell Cell = sheet.Cells[alpha[index] + (E + 2)];
                            Cell.PutValue(Namabar);
                            E++;
                        }
                        index++;
                        break;
                    case "Telphon":
                        Check = "تلفن";
                        Cell cell19 = sheet.Cells[alpha[index] + "1"];
                        cell19.PutValue(Check);
                        int K = 0;
                        foreach (var _item in ListCompany)
                        {
                            Telphon = _item.fldTelphon;
                            Cell Cell = sheet.Cells[alpha[index] + (K + 2)];
                            Cell.PutValue(Telphon);
                            K++;
                        }
                        index++;
                        break;
                    case "Address":
                        Check = "آدرس";
                        Cell cell21 = sheet.Cells[alpha[index] + "1"];
                        cell21.PutValue(Check);
                        int M = 0;
                        foreach (var _item in ListCompany)
                        {
                            Address = _item.fldAddress;
                            Cell Cell = sheet.Cells[alpha[index] + (M + 2)];
                            Cell.PutValue(Address);
                            M++;
                        }
                        index++;
                        break;
                    case "CountryMahallSherkat":
                        if (Type == 1)
                            Check = "کشور محل شرکت";
                        else if (Type == 2)
                            Check = "کشور محل فعالیت";
                        Cell cell22 = sheet.Cells[alpha[index] + "1"];
                        cell22.PutValue(Check);
                        int L = 0;
                        foreach (var _item in ListCompany)
                        {
                            MahallSherkat = _item.fldMahallSherkat.Split('_');
                            Cell Cell = sheet.Cells[alpha[index] + (L + 2)];
                            Cell.PutValue(MahallSherkat[0]);
                            L++;
                        }
                        index++;
                        break;
                    case "StateMahallSherkat":
                        if (Type == 1)
                            Check = "استان محل شرکت";
                        else if (Type == 2)
                            Check = "استان محل فعالیت";
                        Cell cell23 = sheet.Cells[alpha[index] + "1"];
                        cell23.PutValue(Check);
                        int LL = 0;
                        foreach (var _item in ListCompany)
                        {
                            MahallSherkat = _item.fldMahallSherkat.Split('_');
                            Cell Cell = sheet.Cells[alpha[index] + (LL + 2)];
                            Cell.PutValue(MahallSherkat[1]);
                            LL++;
                        }
                        index++;
                        break;
                    case "CityMahallSherkat":
                        if (Type == 1)
                            Check = "شهر محل شرکت";
                        else if (Type == 2)
                            Check = "شهر محل فعالیت";
                        Cell cell24 = sheet.Cells[alpha[index] + "1"];
                        cell24.PutValue(Check);
                        int CC = 0;
                        foreach (var _item in ListCompany)
                        {
                            MahallSherkat = _item.fldMahallSherkat.Split('_');
                            Cell Cell = sheet.Cells[alpha[index] + (CC + 2)];
                            Cell.PutValue(MahallSherkat[2]);
                            CC++;
                        }
                        index++;
                        break;
                    case "DateTasis":
                        Check = "تاریخ تاسیس";
                        Cell cell25 = sheet.Cells[alpha[index] + "1"];
                        cell25.PutValue(Check);
                        int D = 0;
                        foreach (var _item in ListCompany)
                        {
                            DateTasis = _item.fldDateTasis.ToString();
                            Cell Cell = sheet.Cells[alpha[index] + (D + 2)];
                            Cell.PutValue(DateTasis);
                            D++;
                        }
                        index++;
                        break;
                    case "DateSabt":
                        Check = "تاریخ ثبت";
                        Cell cell26 = sheet.Cells[alpha[index] + "1"];
                        cell26.PutValue(Check);
                        int R = 0;
                        foreach (var _item in ListCompany)
                        {
                            DateSabt = _item.fldDateSabt;
                            Cell Cell = sheet.Cells[alpha[index] + (R + 2)];
                            Cell.PutValue(DateSabt);
                            R++;
                        }
                        index++;
                        break;
                    case "MablaghSaham":
                        Check = "مبلغ هر سهام";
                        Cell cell27 = sheet.Cells[alpha[index] + "1"];
                        cell27.PutValue(Check);
                        int I = 0;
                        foreach (var _item in ListCompany)
                        {
                            MablaghSaham = _item.fldMablaghSaham.ToString();
                            Cell Cell = sheet.Cells[alpha[index] + (I + 2)];
                            Cell.PutValue(MablaghSaham);
                            I++;
                        }
                        index++;
                        break;
                    case "CountryMahallSabt":
                        Check = "کشور محل ثبت";
                        Cell cell28 = sheet.Cells[alpha[index] + "1"];
                        cell28.PutValue(Check);
                        int S = 0;
                        foreach (var _item in ListCompany)
                        {
                            MahallSabt = _item.fldMahallSabt.Split('_');
                            Cell Cell = sheet.Cells[alpha[index] + (S + 2)];
                            Cell.PutValue(MahallSabt[0]);
                            S++;
                        }
                        index++;
                        break;
                    case "StateMahallSabt":
                        Check = "استان محل ثبت";
                        Cell cell29 = sheet.Cells[alpha[index] + "1"];
                        cell29.PutValue(Check);
                        int Y = 0;
                        foreach (var _item in ListCompany)
                        {
                            MahallSabt = _item.fldMahallSabt.Split('_');
                            Cell Cell = sheet.Cells[alpha[index] + (Y + 2)];
                            Cell.PutValue(MahallSabt[1]);
                            Y++;
                        }
                        index++;
                        break;
                    case "CityMahallSabt":
                        Check = "شهر محل ثبت";
                        Cell cell30 = sheet.Cells[alpha[index] + "1"];
                        cell30.PutValue(Check);
                        int G = 0;
                        foreach (var _item in ListCompany)
                        {
                            MahallSabt = _item.fldMahallSabt.Split('_');
                            Cell Cell = sheet.Cells[alpha[index] + (G + 2)];
                            Cell.PutValue(MahallSabt[2]);
                            G++;
                        }
                        index++;
                        break;
                    case "NickName":
                        Check = "نام اختصاری";
                        Cell cell31 = sheet.Cells[alpha[index] + "1"];
                        cell31.PutValue(Check);
                        int O = 0;
                        foreach (var _item in ListCompany)
                        {
                            NickName = _item.fldNickName;
                            Cell Cell = sheet.Cells[alpha[index] + (O + 2)];
                            Cell.PutValue(NickName);
                            O++;
                        }
                        index++;
                        break;
                    case "TedadKolSaham":
                        Check = "تعداد کل سهام";
                        Cell cell32 = sheet.Cells[alpha[index] + "1"];
                        cell32.PutValue(Check);
                        int J = 0;
                        foreach (var _item in ListCompany)
                        {
                            TedadKolSaham = _item.fldTedadKolSaham.ToString();
                            Cell Cell = sheet.Cells[alpha[index] + (J + 2)];
                            Cell.PutValue(TedadKolSaham);
                            J++;
                        }
                        index++;
                        break;
                    case "TypeCompany":
                        Check = "نوع ثبتی شرکت";
                        Cell cell33 = sheet.Cells[alpha[index] + "1"];
                        cell33.PutValue(Check);
                        int Z = 0;
                        foreach (var _item in ListCompany)
                        {
                            TypeCompany = _item.fldTypeCompany;
                            Cell Cell = sheet.Cells[alpha[index] + (Z + 2)];
                            Cell.PutValue(TypeCompany);
                            Z++;
                        }
                        index++;
                        break;
                    case "CodePosti":
                        Check = "کد پستی";
                        Cell cell34 = sheet.Cells[alpha[index] + "1"];
                        cell34.PutValue(Check);
                        int P = 0;
                        foreach (var _item in ListCompany)
                        {
                            CodePosti = _item.fldCodePosti;
                            Cell Cell = sheet.Cells[alpha[index] + (P + 2)];
                            Cell.PutValue(CodePosti);
                            P++;
                        }
                        index++;
                        break;
                    case "AddressWebSite":
                        Check = "وب سایت";
                        Cell cell35 = sheet.Cells[alpha[index] + "1"];
                        cell35.PutValue(Check);
                        int PP = 0;
                        foreach (var _item in ListCompany)
                        {
                            AddressWebSite = _item.fldAddressWebSite;
                            Cell Cell = sheet.Cells[alpha[index] + (PP + 2)];
                            Cell.PutValue(AddressWebSite);
                            PP++;
                        }
                        index++;
                        break;
                    case "EmailCompany":
                        Check = "پست الکترونیک";
                        Cell cell36 = sheet.Cells[alpha[index] + "1"];
                        cell36.PutValue(Check);
                        int H = 0;
                        foreach (var _item in ListCompany)
                        {
                            EmailCompany = _item.fldEmailCompany;
                            Cell Cell = sheet.Cells[alpha[index] + (H + 2)];
                            Cell.PutValue(EmailCompany);
                            H++;
                        }
                        index++;
                        break;
                    case "TitleAllowed":
                        Check = "ذینفع های مجاز";
                        Cell cell37 = sheet.Cells[alpha[index] + "1"];
                        cell37.PutValue(Check);
                        int hh = 0;
                        foreach (var _item in ListCompany)
                        {
                            TitleAllowed = _item.AllowedTitle;
                            Cell Cell = sheet.Cells[alpha[index] + (hh + 2)];
                            Cell.PutValue(TitleAllowed);
                            hh++;
                        }
                        index++;
                        break;
                    case "Mobile":
                        Check = "موبایل نماینده";
                        Cell cell38 = sheet.Cells[alpha[index] + "1"];
                        cell38.PutValue(Check);
                        int uu = 0;
                        foreach (var _item in ListCompany)
                        {
                            Mobile = _item.fldMobile;
                            Cell Cell = sheet.Cells[alpha[index] + (uu + 2)];
                            Cell.PutValue(Mobile);
                            uu++;
                        }
                        index++;
                        break;
                    case "Name_Family":
                        Check = "نام نماینده";
                        Cell cell39 = sheet.Cells[alpha[index] + "1"];
                        cell39.PutValue(Check);
                        int qq = 0;
                        foreach (var _item in ListCompany)
                        {
                            Namayande = _item.fldName_Family;
                            Cell Cell = sheet.Cells[alpha[index] + (qq + 2)];
                            Cell.PutValue(Namayande);
                            qq++;
                        }
                        index++;
                        break;
                    case "NameRequest":
                        Check = "نام درخواست";
                        Cell cell40 = sheet.Cells[alpha[index] + "1"];
                        cell40.PutValue(Check);
                        int r1 = 0;
                        foreach (var _item in ListCompany)
                        {
                            Namayande = _item.fldRequestHadafName;
                            Cell Cell = sheet.Cells[alpha[index] + (r1 + 2)];
                            Cell.PutValue(Namayande);
                            r1++;
                        }
                        index++;
                        break;
                    case "TypeRequest":
                        Check = "نوع درخواست";
                        Cell cell41 = sheet.Cells[alpha[index] + "1"];
                        cell41.PutValue(Check);
                        int r2 = 0;
                        foreach (var _item in ListCompany)
                        {
                            Namayande = _item.fldRequestTypeName;
                            Cell Cell = sheet.Cells[alpha[index] + (r2 + 2)];
                            Cell.PutValue(Namayande);
                            r2++;
                        }
                        index++;
                        break;
                }
            }
            MemoryStream stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Excel97To2003);
            return File(stream.ToArray(), "xls", "MonitoringRegisterComplete.xls");
        }

        public ActionResult CheckHaveRequest(int HadafId, int FirstRegisterId)
        {
            string Msg = ""; string MsgTitle = ""; byte Er = 0; bool? HaveRequest = false;

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFirstRegister_HadafAllowedSelect("fldFirstRegisterId", FirstRegisterId.ToString(), 0).Where(l => l.fldHadafId == HadafId).FirstOrDefault();
            if (q != null)
            {
                HaveRequest = m.prs_CheckRequest_HadafAllowedId(q.fldId, FirstRegisterId).FirstOrDefault().Resulte;

            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er,
                HaveRequest = HaveRequest
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ShowInfoCompanyProfile(string CompanyProfileId, string Final_Sabt, string PersonType, string state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Final_Sabt = Final_Sabt;
            PartialView.ViewBag.CompanyProfileId = CompanyProfileId;
            PartialView.ViewBag.PersonType = PersonType;//اگر حقیقی باشد=1
            PartialView.ViewBag.state = state;
            return PartialView;
        }
        public ActionResult ShowRelativeStakeholders(string FirstRegisterIdd, string CompanyProfileIdd, string PersonType)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFirstRegisterSelect("fldId", FirstRegisterIdd, "", 1).FirstOrDefault();

            ViewData.Model = new Monitorring();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ViewData = ViewData
            };
            //PartialView.ViewBag.GroupNameId = q.fldGroupingId;
            result.ViewBag.FirstRegisterIdd = FirstRegisterIdd;
            result.ViewBag.CompanyProfileIdd = CompanyProfileIdd;
            result.ViewBag.PersonType = PersonType;//اگر حقیقی باشد=1
            return result;
        }
        public ActionResult ShowComment(string Id, string PersonType)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            int? fldFirstRegisterId = 0;
            //if (PersonType == "2")
            fldFirstRegisterId = m.prs_tblCompanyProfileSelect("fldId", Id, 0).FirstOrDefault().fldFirstRegisterUser;
            //else
            //    fldFirstRegisterId = servic1.GetRealPersonLocationWithFilter("fldId", Id, 1, out Err1).FirstOrDefault().fldFirstRegisterUser;

            PartialView.ViewBag.FirstRegisterId = fldFirstRegisterId;
            return PartialView;
        }

        public ActionResult SendMessage(string CompanyProfileIds, string Person)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.CompanyProfileIds = CompanyProfileIds;
            PartialView.ViewBag.Person = Person;
            return PartialView;
        }

        public ActionResult SendShMessage(string CompanyProfileIds, string fldMatn, bool fldPersonSend, bool fldType, string Person)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                fldMatn = fldMatn.Replace("<div>", Environment.NewLine);
                var Matn = Regex.Replace(fldMatn, "<.*?>", String.Empty);
                RaiSms.Service w = new RaiSms.Service();
                var CompanyProfileIdSplit = CompanyProfileIds.Split(';').Reverse().Skip(1).Reverse().ToList();
                MsgTitle = "ارسال موفق";
                Msg = "با موفقیت ارسال شد.";

                if (fldPersonSend)//ارسال به نماینده
                {
                    foreach (var item in CompanyProfileIdSplit)
                    {
                        var FirstRegisterId = m.prs_tblCompanyProfileSelect("fldId", item, 0).FirstOrDefault().fldFirstRegisterUser;
                        //if (Person == "1")
                        //    FirstRegisterId = servic1.GetRealPersonLocationDetail(Convert.ToInt32(item), IP, out Err1).fldFirstRegisterUser;

                        var q = m.prs_tblFirstRegisterSelect("fldId", FirstRegisterId.ToString(), "", 1).FirstOrDefault();

                        if (fldType)
                        {
                            Msg = "ارسال موفق";
                            var st = true;
                            try
                            {
                                var returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, q.fldMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                                if (returnCode.Length <= 3)
                                {
                                    MsgTitle = "خطا";
                                    Msg = w.ShowError(returnCode, "FA");
                                    Er = 1;
                                    st = false;
                                }
                            }
                            catch (Exception)
                            {
                                Msg = "قطع ارتباط با سرور پیامک.";
                                st = false;
                            }
                            m.prs_tblSMSEmailCachInsert(Matn, q.fldMobile, "", Msg, q.fldName + " " + q.fldFamily, false, false, q.fldId, st, user.UserInputId);
                        }
                        else
                        {
                            var st = true;
                            Msg = "ارسال موفق";
                            try
                            {
                                st = SendEmail(q.fldEmail, Matn);
                            }
                            catch (Exception x)
                            {
                                var Msgg = "";
                                if (x.InnerException != null)
                                    Msgg = x.InnerException.Message;
                                else
                                    Msgg = x.Message;
                                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                m.prs_tblErrorInsert(ErrorId, Msgg, m.prs_GetDate().FirstOrDefault().fldDateTime, user.UserInputId, "");
                                Msg = " قطع ارتباط با سرور ایمیل.";
                                st = false;
                            }
                            if (st == false)
                                Msg = "قطع ارتباط با سرور ایمیل.";
                            m.prs_tblSMSEmailCachInsert(Matn, "", q.fldEmail, Msg, q.fldName + " " + q.fldFamily, false, true, q.fldId, st, user.UserInputId);
                        }
                    }
                }
                else//ارسال به مدیرعامل
                {
                    foreach (var item in CompanyProfileIdSplit)
                    {
                        var q = m.prs_tblCompanyLocationSelect("fldCompanyId", item, 1).FirstOrDefault();
                        var c = m.prs_tblCompanyProfileSelect("fldId", item, 0).FirstOrDefault();
                        var mobileModir = q.fldMobileModir;
                        var NameModir = c.fldNameModirAmel + " " + c.fldFamilyModirAmel;
                        var EmailModir = q.fldEmail1;
                        var FirstRegister = q.fldFirstRegisterUser;

                        //if (Person == "1")
                        //{
                        //    var q1 = servic1.GetRealPersonLocationWithFilter("fldId", item, 1, out Err1).FirstOrDefault();
                        //    mobileModir = q1.fldMobile;
                        //    NameModir = q1.fldName_Family;
                        //    EmailModir = q1.fldEmail1;
                        //    FirstRegister = q1.fldFirstRegisterUser;
                        //}


                        if (fldType)
                        {
                            Msg = "ارسال موفق";
                            var st = true;
                            try
                            {
                                var returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, mobileModir, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                                if (returnCode.Length <= 3)
                                {
                                    MsgTitle = "خطا";
                                    Msg = w.ShowError(returnCode, "FA");
                                    Er = 1;
                                    st = false;
                                }
                            }
                            catch (Exception)
                            {
                                Msg = "قطع ارتباط با سرور پیامک.";
                                st = false;
                            }

                            m.prs_tblSMSEmailCachInsert(Matn, mobileModir, "", Msg, NameModir, true, false, FirstRegister, st, user.UserInputId);
                        }
                        else
                        {
                            Msg = "ارسال موفق";
                            var st = true;
                            try
                            {
                                st = SendEmail(EmailModir, Matn);
                            }
                            catch (Exception x)
                            {
                                var Msgg = "";
                                if (x.InnerException != null)
                                    Msgg = x.InnerException.Message;
                                else
                                    Msgg = x.Message;
                                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                m.prs_tblErrorInsert(ErrorId, Msgg, m.prs_GetDate().FirstOrDefault().fldDateTime, user.UserInputId, "");

                                Msg = "قطع ارتباط با سرور ایمیل.";
                                st = false;
                            }
                            if (st == false)
                                Msg = "قطع ارتباط با سرور ایمیل.";
                            m.prs_tblSMSEmailCachInsert(Matn, "", EmailModir, Msg, NameModir, true, true, FirstRegister, st, user.UserInputId);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                string InnerException = "";
                if (x.Message != null)
                    InnerException = x.Message;
                return Json(new
                {
                    Msg = "خطا در دسترسی به سرور برای ارسال( " + InnerException + " )",
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
        public FileContentResult Download(int FileId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFileSelect("fldId", FileId.ToString(), 1).FirstOrDefault();

            if (q != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
        public ActionResult ReadCommentSent(StoreRequestParameters parameters, string FirstRegisterId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            List<Models.prs_tblCommentSentSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<Models.prs_tblCommentSentSelect> data1 = null;
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
                        case "fldDesc":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldDesc";
                            break;

                    }
                    if (data != null)
                        data1 = m.prs_tblCommentSentSelect(field, searchtext, 100).Where(l => l.fldFirsteRegisterUser == Convert.ToInt32(FirstRegisterId)).ToList();
                    else
                        data = m.prs_tblCommentSentSelect(field, searchtext, 100).Where(l => l.fldFirsteRegisterUser == Convert.ToInt32(FirstRegisterId)).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_tblCommentSentSelect("fldFirsteRegisterUser", FirstRegisterId, 100).ToList();
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
                            return !oValue.ToString().Contains(value.ToString());
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

            List<prs_tblCommentSentSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        public ActionResult DetailsCompanyProfile(string Id, string Type)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            //if (Type == "1")
            //{
            //    var realL = servic1.GetRealPersonLocationWithFilter("fldId", Id, 1, out Err1).FirstOrDefault();
            //    var real = servic1.GetCompanyPersonalWithFilter("fldId", realL.fldCompanyPersonalId.ToString(), "", "", "", "", "", 1, out Err1).FirstOrDefault();
            //    var F = servic1.GetFirstRegisterWithFilter("fldId", realL.fldFirstRegisterUser.ToString(), "", 1, out Err1).FirstOrDefault();
            //    var TelNamayande = F.fldMobile;
            //    var EmailNamayande = F.fldEmail;
            //    if (realL != null)
            //    {
            //        return Json(new
            //        {
            //            Er = 0,
            //            fldAddressR = realL.fldAddress,
            //            fldAddressWebSiteR = realL.fldAddressWebSite,
            //            fldCityTitleR = realL.fldCityTitle,
            //            fldCodeEghtesadiR = realL.fldCodeEghtesadi,
            //            fldCodePostiR = realL.fldCodePosti,
            //            fldCountrynameR = realL.fldCountryname,
            //            fldEmail2R = realL.fldEmail1,
            //            fldEmail3R = realL.fldEmail2,
            //            fldMobileR = realL.fldMobile,
            //            fldNamabarR = realL.fldNamabar,
            //            fldNickNameR = realL.fldNickName,
            //            fldPishShomare_NamabarR = realL.fldPishShomare_Namabar,
            //            fldPishShomare_TelR = realL.fldPishShomare_Tel,
            //            fldStateTitleR = realL.fldStateTitle,
            //            fldTelphonR = realL.fldTelphon,
            //            fldFirstRegisterUserR = realL.fldFirstRegisterUser,
            //            fldFileIdR = realL.fldFileId,
            //            LogoFileId = realL.fldLogoId,
            //            fldCodeMojavez = realL.fldCodeMojavez,
            //            fldTypeMojavezId = realL.fldTypeMojavezId,
            //            fldTypeMojavezFaaliatTitle = realL.fldTypeMojavezFaaliatTitle,

            //            fldFirstRegisterUserreal = real.fldFirstRegisterUser,
            //            fldEmail1real = real.fldEmail,
            //            fldNamereal = real.fldName,
            //            fldFamilyreal = real.fldFamily,
            //            fldFatherNamereal = real.fldFatherName,
            //            fldCodeMelireal = real.fldCodeMeli,
            //            fldShenasnameNoreal = real.fldShenasnameNo,
            //            fldBDatereal = real.fldBDate,
            //            fldMeliyatNamereal = real.fldMeliyatName,
            //            fldSh_Bimereal = real.fldSh_Bime,
            //            fldCityTavalodreal = real.fldCityTavalod,
            //            fldStateTavalodreal = real.fldStateTavalod,
            //            fldCountryTavaloodreal = real.fldCountryTavalood,
            //            fldStateSodoorreal = real.fldStateSodoor,
            //            fldCountrySodoorreal = real.fldCountrySodoor,
            //            fldCitySodoorreal = real.fldCitySodoor,
            //            fldMashmolGhanonreal = real.fldMashmolGhanon,

            //            fldNameNamayande = F.fldName,
            //            fldFamilyNamayande = F.fldFamily,
            //            fldGroupName = F.fldGroupName,
            //            TelNamayande = TelNamayande,
            //            EmailNamayande = EmailNamayande,

            //            Username = Session["Username"].ToString(),
            //            Password = Session["Password"].ToString()

            //        }, JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //        return Json(new
            //        {
            //            Er = 1,
            //            FileId = 0
            //        }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //else
            //{
            var c = m.prs_tblCompanyProfileSelect("fldId", Id, 0).FirstOrDefault();
            if (c != null)
            {
                var c2 = m.prs_tblCompanyProfile_DetailSelect("fldHeaderId", Id, 0).FirstOrDefault();
                var L = m.prs_tblCompanyLocationSelect("fldCompanyId", c.fldId.ToString(), 1).FirstOrDefault();
                var S = m.prs_tblStatuteFirstPageSelect("fldCompanyId", c.fldId.ToString(), 1).FirstOrDefault();
                var F1 = m.prs_tblFirstRegisterSelect("fldId", c.fldFirstRegisterUser.ToString(), "", 1).FirstOrDefault();
                var TelNamayande = F1.fldMobile;
                var EmailNamayande = F1.fldEmail;
                var sh = m.prs_tblSharesSelect("fldCompanyId", c.fldId.ToString(), 1).FirstOrDefault();
                return Json(new
                {
                    Er = 0,
                    fldCodeEghtesadi = c2.fldCodeEghtesadi,
                    fldDateSabt = c.fldDateSabt,
                    fldDateTasis = c2.fldDateTasis,
                    fldFirstRegisterId = c.fldFirstRegisterUser,
                    fldFullName = c.fldFullName,
                    fldNationalCode = c.fldNationalCode,
                    fldNickName = c2.fldNickName,
                    fldSh_Sabt = c.fldSh_Sabt,
                    fldName = F1.fldName,
                    fldNameModirAmel = c.fldNameModirAmel,
                    fldFamilyModirAmel = c.fldFamilyModirAmel,
                    fldFamily = F1.fldFamily,
                    fldCityName = c.fldCityName,
                    fldCountryName = c.fldCountryName,
                    fldStateName = c.fldStateName,
                    fldTypeCompanyName = c.fldTypeCompanyName,

                    fldAddress = L.fldAddress,
                    fldAddressWebSite = L.fldAddressWebSite,
                    fldCodePosti = L.fldCodePosti,
                    fldEmail1 = L.fldEmail1,
                    fldEmail2 = L.fldEmail2,
                    fldEmail3 = L.fldEmail3,
                    fldMobileModir = L.fldMobileModir,
                    fldNamabar = L.fldNamabar,
                    fldPishShomare_Namabar = L.fldPishShomare_Namabar,
                    fldPishShomare_Tel = L.fldPishShomare_Tel,
                    fldTelphon = L.fldTelphon,

                    FileId = S.fldFileId,
                    LogoFileId = c.fldLogoId,

                    fldMablaghSaham = sh.fldMablaghSaham,
                    fldTedadKolSaham = sh.fldTedadKolSaham,

                      fldGroupName = F1.fldSelectedHadafName,
                    TelNamayande = TelNamayande,
                    EmailNamayande = EmailNamayande,

                    Username = u.UserName.ToString(),
                    Password = u.Pass.ToString()

                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    Er = 1,
                    FileId = 0
                }, JsonRequestBehavior.AllowGet);
            }
            //}



        }

        public FileContentResult DownloadAsasnameh(int FileId, string UserName, string Password)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblUploadFileCompanySelect("fldId", FileId.ToString(), 1).FirstOrDefault();
            if (q.fldFile != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPassvand), "DownloadFile" + q.fldPassvand);
            }
            return null;
        }

        public ActionResult ReadHistoryLogin_Comoany(StoreRequestParameters parameters, string IdUser, string state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            var fieldName = "fldFirstRegisterUser_log";
            if (state == "1")
                fieldName = "fldUserId_log";

            List<Models.prs_tblInputInfoSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<Models.prs_tblInputInfoSelect> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        //case "fldId":
                        //    searchtext = ConditionValue.Value.ToString();
                        //    field = "fldId";
                        //    break;
                        case "fldDateTime":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldDateTime";
                            break;
                        case "fldTime":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldTime";
                            break;
                        case "fldIP":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldIP";
                            break;
                        case "fldLoginTypeName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldLoginTypeName";
                            break;

                    }
                    if (data != null)
                    {
                        data1 = m.prs_tblInputInfoSelect(field, searchtext, "", false, 100).Where(l => l.fldFirstRigesterId == Convert.ToInt32(IdUser)).ToList();
                        if (state == "1")
                            data1 = m.prs_tblInputInfoSelect(field, searchtext, "", false, 100).Where(l => l.fldUserID == Convert.ToInt32(IdUser)).ToList();
                    }
                    else
                    {
                        data = m.prs_tblInputInfoSelect(field, searchtext, "", false, 100).Where(l => l.fldFirstRigesterId == Convert.ToInt32(IdUser)).ToList();
                        if (state == "1")
                            data = m.prs_tblInputInfoSelect(field, searchtext, "", false, 100).Where(l => l.fldUserID == Convert.ToInt32(IdUser)).ToList();
                    }
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_tblInputInfoSelect(fieldName, IdUser, "", false, 100).ToList();
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

            List<Models.prs_tblInputInfoSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        public ActionResult Read(StoreRequestParameters parameters, string AzTarikh, string TaTarikh, string Status)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            List<Models.prs_ListCompany> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<Models.prs_ListCompany> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        case "fldId":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldId";
                            break;
                        case "fldNationalCode":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldNationalCode";
                            break;
                        case "fldFullName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldFullName";
                            break;
                        case "fldSh_Sabt":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldSh_Sabt";
                            break;
                        case "fldCodeEghtesadi":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldCodeEghtesadi";
                            break;
                        case "fldName_Family":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldName_Family";
                            break;
                        case "fldTarikh":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldTarikh";
                            break;
                        case "fldTime":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldTime";
                            break;
                        case "fldHadafSabtName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldHadafSabtName";
                            break;
                        case "NameModirAmel":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_NameModirAmel";
                            break;
                        case "fldAddress":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldAddress";
                            break;
                        case "fldAddressWebSite":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldAddressWebSite";
                            break;
                        case "fldCodePosti":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldCodePosti";
                            break;
                        case "fldDateSabt":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldDateSabt";
                            break;
                        case "fldDateTasis":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldDateTasis";
                            break;
                        case "fldEmailCompany":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldEmailCompany";
                            break;
                        case "fldMablaghSaham":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldMablaghSaham";
                            break;
                        case "fldMahallSabt":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldMahallSabt";
                            break;
                        case "fldMahallSherkat":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldMahallSherkat";
                            break;
                        case "fldNamabar":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldNamabar";
                            break;
                        case "fldNickName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldNickName";
                            break;
                        case "fldTedadKolSaham":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldTedadKolSaham";
                            break;
                        case "fldTelphon":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldTelphon";
                            break;
                        case "fldTypeCompany":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "Company_fldTypeCompany";
                            break;

                    }
                    if (data != null)
                    {
                        if (Status == "0" || Status == "")
                        {
                            data1 = m.prs_ListCompany(field, searchtext, 0, AzTarikh, TaTarikh, Status).ToList();

                        }
                        else //if (Convert.ToInt32(Status) < 5)
                            data1 = m.prs_ListCompany(field, searchtext, 0, AzTarikh, TaTarikh, Status)/*.Where(l => l.fldCompanyStatus == Status)*/.ToList();
                        //  else 
                        //    data1 = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Convert.ToInt32(l.fldCompanyStatus) > 4).ToList();
                    }
                    else
                    {
                        if (Status == "0" || Status == "")
                        {
                            data = m.prs_ListCompany(field, searchtext, 0, AzTarikh, TaTarikh, Status).ToList();
                        }
                        else//if (Convert.ToInt32(Status) < 5)
                            data = m.prs_ListCompany(field, searchtext, 0, AzTarikh, TaTarikh, Status)/*.Where(l => l.fldCompanyStatus == Status)*/.ToList();
                        //  else
                        //      data = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh,"", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Convert.ToInt32(l.fldCompanyStatus) > 4).ToList();
                    }
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                if (Status == "0" || Status == "")
                {
                    data = m.prs_ListCompany("Company", "", 0, AzTarikh, TaTarikh, Status).ToList();
                }
                else  //if (Convert.ToInt32(Status) < 5)
                    data = m.prs_ListCompany("Company", "", 0, AzTarikh, TaTarikh, Status)/*.Where(l => l.fldCompanyStatus == Status)*/.ToList();
                // else 
                //   data = servic.GetListCompanyWithFilter("Company", "", 0, AzTarikh, TaTarikh, Status, Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Convert.ToInt32(l.fldCompanyStatus) > 4).ToList();
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

            List<Models.prs_ListCompany> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        //public ActionResult Read_Shakhs(StoreRequestParameters parameters, string AzTarikh, string TaTarikh, string Status, int Allowed)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

        //    List<RaiTrainAdminWS.OBJ_ListCompany> data = null;
        //    if (filterHeaders.Conditions.Count > 0)
        //    {
        //        string field = "";
        //        string searchtext = "";
        //        List<RaiTrainAdminWS.OBJ_ListCompany> data1 = null;
        //        foreach (var item in filterHeaders.Conditions)
        //        {
        //            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

        //            switch (item.FilterProperty.Name)
        //            {
        //                case "fldId":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldId";
        //                    break;
        //                case "fldNationalCode":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldNationalCode";
        //                    break;
        //                case "fldFullName":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldFullName";
        //                    break;
        //                case "fldMobileModirAmel":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldMobileModirAmel";
        //                    break;
        //                case "fldCodeEghtesadi":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldCodeEghtesadi";
        //                    break;
        //                case "fldName_Family":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldName_Family";
        //                    break;
        //                case "fldTarikh":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldTarikh";
        //                    break;
        //                case "fldTime":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldTime";
        //                    break;
        //                case "fldGroupName":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldGroupName";
        //                    break;
        //                case "NameModirAmel":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_NameModirAmel";
        //                    break;
        //                case "fldFatherName":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldFatherName";
        //                    break;
        //                case "fldMahallSodoor":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldMahallSodoor";
        //                    break;
        //                case "fldMahallTavalod":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldMahallTavalod";
        //                    break;
        //                case "fldShenasnameNo":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldShenasnameNo";
        //                    break;
        //                case "fldNamabar":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldNamabar";
        //                    break;
        //                case "fldTelphon":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldTelphon";
        //                    break;
        //                case "fldAddress":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldAddress";
        //                    break;
        //                case "fldMahallSherkat":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldMahallSherkat";
        //                    break;
        //                case "fldCodePosti":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldCodePosti";
        //                    break;
        //                case "fldAddressWebSite":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldAddressWebSite";
        //                    break;
        //                case "fldEmailCompany":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldEmailCompany";
        //                    break;
        //                case "fldSh_Bime":
        //                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
        //                    field = "RealPerson_fldSh_Bime";
        //                    break;
        //            }
        //            if (data != null)
        //            {
        //                if (Status == "0" || Status == "")
        //                {
        //                    if (Allowed == 0)
        //                        data1 = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).ToList();
        //                    else
        //                        data1 = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3).ToList();
        //                }
        //                else if (Convert.ToInt32(Status) < 5)
        //                    data1 = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Allowed == 0 ? (l.fldCompanyStatus == Status) : ((l.fldCompanyStatus == Status) && (l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3))).ToList();
        //                else
        //                    data1 = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Allowed == 0 ? (Convert.ToInt32(l.fldCompanyStatus) > 4) : ((Convert.ToInt32(l.fldCompanyStatus) > 4) && (l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3))).ToList();
        //            }
        //            else
        //            {
        //                if (Status == "0" || Status == "")
        //                {
        //                    if (Allowed == 0)
        //                        data = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).ToList();
        //                    else
        //                        data = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3).ToList();
        //                }
        //                else if (Convert.ToInt32(Status) < 5)
        //                    data = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Allowed == 0 ? (l.fldCompanyStatus == Status) : ((l.fldCompanyStatus == Status) && (l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3))).ToList();
        //                else
        //                    data = servic.GetListCompanyWithFilter(field, searchtext, 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Allowed == 0 ? (Convert.ToInt32(l.fldCompanyStatus) > 4) : ((Convert.ToInt32(l.fldCompanyStatus) > 4) && (l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3))).ToList();
        //            }
        //        }
        //        if (data != null && data1 != null)
        //            data.Intersect(data1);
        //    }
        //    else
        //    {
        //        if (Status == "0" || Status == "")
        //        {
        //            if (Allowed == 0)
        //                data = servic.GetListCompanyWithFilter("RealPerson", "", 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).ToList();
        //            else
        //                data = servic.GetListCompanyWithFilter("RealPerson", "", 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3).ToList();
        //        }
        //        else if (Convert.ToInt32(Status) < 5)
        //            data = servic.GetListCompanyWithFilter("RealPerson", "", 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Allowed == 0 ? (l.fldCompanyStatus == Status) : ((l.fldCompanyStatus == Status) && (l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3))).ToList();
        //        else
        //            data = servic.GetListCompanyWithFilter("RealPerson", "", 0, AzTarikh, TaTarikh, "", Session["Username"].ToString(), (Session["Password"].ToString()), out Err).Where(l => Allowed == 0 ? (Convert.ToInt32(l.fldCompanyStatus) > 4) : ((Convert.ToInt32(l.fldCompanyStatus) > 4) && (l.StakeholderAllowed_Edarekol == Allowed || l.StakeholderAllowed_Edarekol == 3))).ToList();
        //    }
        //    List<RaiTrainAdminWS.OBJ_ListCompany> rangeData2 = (parameters.Start < 0 ? data : data);
        //    var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

        //    //FilterConditions fc = parameters.GridFilters;

        //    //-- start filtering ------------------------------------------------------------
        //    if (fc != null)
        //    {
        //        foreach (var condition in fc.Conditions)
        //        {
        //            string field = condition.FilterProperty.Name;
        //            var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

        //            data.RemoveAll(
        //                item =>
        //                {
        //                    object oValue = item.GetType().GetProperty(field).GetValue(item, null);
        //                    return !oValue.ToString().Contains(value.ToString());
        //                }
        //            );
        //        }
        //    }
        //    //-- end filtering ------------------------------------------------------------

        //    //-- start paging ------------------------------------------------------------
        //    int limit = parameters.Limit;

        //    if ((parameters.Start + parameters.Limit) > data.Count)
        //    {
        //        limit = data.Count - parameters.Start;
        //    }

        //    List<RaiTrainAdminWS.OBJ_ListCompany> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
        //    //-- end paging ------------------------------------------------------------

        //    return this.Store(rangeData, data.Count);
        //}

        public ActionResult ReadRelativeStakeholders(/*string GroupNameId, */string PersonType, string FirstRegisterId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var f = m.prs_tblFirstRegisterSelect("fldId", FirstRegisterId, "", 0).FirstOrDefault();
            //List<RaiTrainAdminWS.OBJ_StakeholderGrouping> data = null;
            //data = servic.GetStakeholderGroupingWithFilter("fldGroupNameId_Name", GroupNameId, "", 0, Session["Username"].ToString(), Session["Password"].ToString(), out Err).ToList();
            //if (PersonType=="1")
            //    data = servic.GetStakeholderGroupingWithFilter("fldGroupNameId_Name", GroupNameId, "", 0, Session["Username"].ToString(), Session["Password"].ToString(), out Err).Where(l => l.fldStakeholderTreeId == 28 || l.fldStakeholderTreeId == 12 || l.fldStakeholderTreeId == 32 || l.fldStakeholderTreeId == 47).ToList();

            List<Models.prs_HadafListForFirstRegister> data = null;
            data = m.prs_HadafListForFirstRegister(f.fldSelectedHadaf).ToList();
            //if (PersonType == "1")
            //    data = m.prs_HadafListForFirstRegister(f.fldSelectedHadaf).Where(l => l.fldId == 28 || l.fldId == 12 || l.fldId == 32 || l.fldId == 47).ToList();

            //if (d.fldEdare == 0)
            return this.Store(data);
            //else
            //    return this.Store(data.Where(l => l.fldEdareKol == d.fldEdare));
        }
        public ActionResult UpdateStatus(int CompanyProfileId, string PersonType)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var Mobile = "";
                var Email = "";
                var FieldName = "";
                var FieldNameUpdate = "";
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                if (PersonType == "2")
                {
                    FieldName = "Company_fldId";
                    FieldNameUpdate = "CompanyProfile";
                }
                else
                {
                    FieldName = "RealPerson_fldId";
                    FieldNameUpdate = "RealPerson";
                }

                var r = m.prs_ListCompany(FieldName, CompanyProfileId.ToString(), 0, "", "","").FirstOrDefault();
                Mobile = r.fldMobile;
                Email = r.fldEmail;
                var MatnSMS = "ثبت نام اولیه شما در سامانه ماده12 تایید شد، لطفا با مراجعه به سامانه ماده 12، پرونده الکترونیکی را تکمیل نمایید.";
                RaiSms.Service w = new RaiSms.Service();

                m.prs_UpdateAllStatus(FieldNameUpdate, 4, u.UserSecondId, CompanyProfileId, "تایید شده", IP);

                Msg = "تایید ثبت نام اولیه با موفقیت انجام شد";
                MsgTitle = "ذخیره موفق";


                var ReturnMsg = "ارسال موفق";
                var st = true;
                try
                {
                    string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS + Environment.NewLine + "راه آهن جمهوری اسلامی ایران", Mobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                    if (returnCode.Length <= 3)
                    {
                        MsgTitle = "خطا";
                        ReturnMsg = w.ShowError(returnCode, "FA");
                        Er = 1;
                    }
                }
                catch (Exception)
                {
                    Msg = " قطع ارتباط با سرور پیامک.";
                    st = false;
                }
                m.prs_tblSMSEmailCachInsert(MatnSMS, Mobile, "", ReturnMsg, r.fldName_Family, false, false, r.fldFirstRegisterUser, st, u.UserInputId);

                ReturnMsg = "ارسال موفق";
                st = true;
                try
                {
                    st = SendEmail(Email, MatnSMS);
                }
                catch (Exception x)
                {
                    var Msgg = "";
                    if (x.InnerException != null)
                        Msgg = x.InnerException.Message;
                    else
                        Msgg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblErrorInsert(ErrorId, Msgg, DateTime.Now, u.UserInputId, "");
                    Msg = "قطع ارتباط با سرور ایمیل.";
                    st = false;
                }
                if (st == false)
                    Msg = "قطع ارتباط با سرور ایمیل.";
                m.prs_tblSMSEmailCachInsert(MatnSMS, "", Email, ReturnMsg, r.fldName_Family, false, true, r.fldFirstRegisterUser, st, u.UserInputId);

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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        string jsonstr = "";
        public bool SendEmail(string Email, string Matn)
        {
                Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                var p = m.prs_tblPageHtmlSelect("fldId", "1", 1).FirstOrDefault();
                var address = Regex.Replace(p.fldMohtavaHtml, "<.*?>", String.Empty);
                var EmailSetting = m.prs_tblEmailSettingSelect("", "", 1).FirstOrDefault();
                //MailAddress from = new MailAddress(EmailSetting.fldAddressEmail);

                //MailAddress to = new MailAddress(Email);
                //MailMessage mail = new MailMessage(from, to);

                string savePath = Server.MapPath(@"~\Content\header.png");
//                mail.IsBodyHtml = true;
                var inlineLogo = new LinkedResource(savePath);
                inlineLogo.ContentId = Guid.NewGuid().ToString();
//                mail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";
//                string body = string.Format(@"
//                                    <img src=""cid:{0}"" />" +
//                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + Matn + "</p>" +
//                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
//                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
//                , inlineLogo.ContentId);
//                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
//                view.LinkedResources.Add(inlineLogo);
//                mail.AlternateViews.Add(view);
//                SmtpClient smtp = new SmtpClient();
//                smtp.Host = EmailSetting.fldSendServer;
//                smtp.Port = EmailSetting.fldSendPort;
//                smtp.EnableSsl = EmailSetting.fldSSL;
//                smtp.UseDefaultCredentials = false;
//                smtp.Credentials = new NetworkCredential(
//                   EmailSetting.fldAddressEmail, EmailSetting.fldPassword);
//                ServicePointManager.ServerCertificateValidationCallback =
//                    delegate(object s, X509Certificate certificate,
//                             X509Chain chain, SslPolicyErrors sslPolicyErrors)
//                    { return true; };

                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                //smtp.Send(mail);
                //**************************************************
                /*
                SmtpMail oMail = new SmtpMail("TryIt");

                // Set sender email address, please change it to yours
                oMail.From = EmailSetting.fldAddressEmail;

                // Set recipient email address, please change it to yours
                oMail.To = Email;

                // Set email subject
                oMail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";

                // Set email body
                oMail.TextBody = string.Format(@"
                                    <img src=""cid:{0}"" />" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + Matn + "</p>" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
                , inlineLogo.ContentId);

                // Your SMTP server address
                SmtpServer oServer = new SmtpServer(EmailSetting.fldSendServer);

                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                oServer.User = EmailSetting.fldAddressEmail;
                oServer.Password = EmailSetting.fldPassword;

                // Set 25 or 587 port.
                oServer.Port = EmailSetting.fldSendPort;

                // detect TLS connection automatically
                oServer.ConnectType = SmtpConnectType.ConnectSTARTTLS;

                Console.WriteLine("start to send email ...");

                EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();

                oSmtp.SendMail(oServer, oMail);

                Console.WriteLine("email was sent successfully!");
                  //**************************************************
                */

               /* string SMTPHost = "";

                string UserName = "";
                string PassWord = "";
                int SMTPPort = 465;
                bool EnableSSL = true;

                string header = "X-ECE_SEND";
                //---------------------

                int EceEmailSendId = 0, haveErrFlg = 0, NotAttachFlg = 0, Receipt = 0;
                string subject = "", EFrom = "", ETo = "", Body = "", ErorText = "";

                MailAddress from = new MailAddress(EmailSetting.fldAddressEmail);

                MailAddress to = new MailAddress(Email);
                MailMessage mail = new MailMessage(from, to);
             

                 
                    Console.WriteLine("set config value...");
                    mail.Headers.Add(header, "1.01");
                    mail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";
                    mail.Body = string.Format(@"
                                    <img src=""cid:{0}"" />" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + Matn + "</p>" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
                , inlineLogo.ContentId); 


                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = EmailSetting.fldSendServer;
                    smtp.Port = EmailSetting.fldSendPort;
                    smtp.EnableSsl = EmailSetting.fldSSL;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(
                       EmailSetting.fldAddressEmail, EmailSetting.fldPassword);
                   
                    Console.WriteLine("authentication is ok ...");


                    try
                    {
                        Console.WriteLine("Sending email...");
                        smtp.Send(mail);
                        Console.WriteLine("Send Mail is !");

                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;

                            if (status == SmtpStatusCode.MailboxBusy ||
                                status == SmtpStatusCode.MailboxUnavailable)
                            {
                                Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                                System.Threading.Thread.Sleep(5000);

                                smtp.Send(mail);
                            }
                            else
                            {
                                ErorText = "ServerErr : " + ex.InnerExceptions[i].FailedRecipient;
                                haveErrFlg = 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErorText = "CatchErr : " + ex.ToString();
                        haveErrFlg = 1;
                    }


                    if (haveErrFlg == 1)
                    {
                        Console.WriteLine("send mail Fail: ");
                        Console.WriteLine(ErorText);
                        System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                        m.prs_tblErrorInsert(ErrorId, ErorText, DateTime.Now, null, "send mail Fail");
                        return false;
                    }

                    Console.WriteLine();
                    smtp.Dispose();


                    Console.WriteLine("starting for next mail !");
                */
               
                MailAddress from = new MailAddress(EmailSetting.fldAddressEmail);

                MailAddress to = new MailAddress(Email);

                MailMessage mail = new MailMessage(from, to);
              
                mail.IsBodyHtml = true;
            
                inlineLogo.ContentId = Guid.NewGuid().ToString();
                mail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";
                string body = string.Format(@"
                                                    <img src=""cid:{0}"" />" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + Matn + "</p>" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
                , inlineLogo.ContentId);

                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                view.LinkedResources.Add(inlineLogo);
                mail.AlternateViews.Add(view);


                try
                {
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = EmailSetting.fldSendServer;
                    smtp.Port = EmailSetting.fldSendPort;
                    smtp.EnableSsl = EmailSetting.fldSSL;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(
                    EmailSetting.fldAddressEmail, EmailSetting.fldPassword);

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                    smtp.Send(mail);
                    return true;
                }
                catch (Exception x)
                {
                    var Mmsg = "";
                    if (x.InnerException != null)
                        Mmsg = x.InnerException.Message;
                    else
                        Mmsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblErrorInsert(ErrorId, Mmsg, DateTime.Now, null, "خطای ارسال ایمیل مانیتورینگ");

                    return false;
                }

                return true;
            }
            catch (Exception x)
            {
                var Msg = "";
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                m.prs_tblErrorInsert(ErrorId, Msg, DateTime.Now,null, "خطای ارسال ایمیل");
                return false;
            }
            

            //

        }
        public ActionResult SaveRelativeStakeholders(string StakeholdersGroupingIds, int fldFirstRegisterId, string RG, int CompanyProfileId, string SelectedMalek)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                var StakeholdersGroupingIdSplit = StakeholdersGroupingIds.Split(';').Reverse().Skip(1).Reverse().ToList();
                if (RG == "0")
                {
                    //حذف

                    //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 5))
                    //{


                    for (int i = 0; i < StakeholdersGroupingIdSplit.Count; i++)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("هدف", Convert.ToInt32(StakeholdersGroupingIdSplit[i]));
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_tblFirstRegister_HadafAllowedInsert(Convert.ToInt32(StakeholdersGroupingIdSplit[i]), fldFirstRegisterId, u.UserInputId, jsonstr);
                    }
                    MsgTitle = "ذخیره موفق";

                    //}
                    //else
                    //{
                    //    return null;
                    //}

                }
                else
                {
                    var SaveData = m.prs_tblFirstRegister_HadafAllowedSelect("fldFirstRegisterId", fldFirstRegisterId.ToString(), 0).ToList();

                    string[] SaveDataId = new string[SaveData.Count];
                    for (int i = 0; i < SaveData.Count; i++)
                    {
                        SaveDataId[i] = SaveData[i].fldHadafId.ToString();
                    }
                    var DeleteRow = SaveDataId.Except(StakeholdersGroupingIdSplit).ToArray();
                    var InsertRow = StakeholdersGroupingIdSplit.Except(SaveDataId).ToArray();

                    if (DeleteRow.Length != 0)
                    {
                        for (int i = 0; i < DeleteRow.Count(); i++)
                        {
                            m.prs_tblFirstRegister_HadafAllowedDelete(Convert.ToInt32(DeleteRow[i]));
                            Msg = "ویرایش با موفقیت انجام شد.";
                            MsgTitle = "ویرایش موفق";

                        }

                    }
                    if (InsertRow.Length != 0)
                    {
                        for (int i = 0; i < InsertRow.Count(); i++)
                        {
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("هدف", Convert.ToInt32(StakeholdersGroupingIdSplit[i]));
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblFirstRegister_HadafAllowedInsert(Convert.ToInt32(InsertRow[i]), fldFirstRegisterId, u.UserInputId, jsonstr);
                            Msg = "ویرایش با موفقیت انجام شد.";
                            MsgTitle = "ویرایش موفق";

                        }
                    }
                }

                m.prs_tblMalek_CompanyProfileDelete(CompanyProfileId);
                var SelectedMalekSplit = SelectedMalek.Split(';').Reverse().Skip(1).Reverse().ToList();
                for (int i = 0; i < SelectedMalekSplit.Count; i++)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("مالک", Convert.ToInt32(SelectedMalekSplit[i]));
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_tblMalek_CompanyProfileInsert(CompanyProfileId, Convert.ToInt32(SelectedMalekSplit[i]),  u.UserInputId, jsonstr);
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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string id, string PersonType)
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

                MsgTitle = "حذف موفق";
                Msg = "حذف با موفقیت انجام شد.";
                //if (PersonType == "2")
                var s = m.prs_DeleteWithCompanyId(Convert.ToInt32(id)).FirstOrDefault();
                if (s.ErrorCode != 0)
                {
                    MsgTitle = "خطا";
                    Msg = s.ErrorMessage;
                }
                //else
                //{
                //    Msg = servic.DeleteRealPerson(Convert.ToInt32(id), Session["Username"].ToString(), (Session["Password"].ToString()), IP, out Err);
                //}


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
        public ActionResult RecieveReport(string id, string PersonType)
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
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                var MatnSMS = "ارسال فیزیکی مرحله پیش ثبت نام شما به پروژه ماده 12 واصل گردید.";

                RaiSms.Service w = new RaiSms.Service();
                var Mobile = "";
                var FieldName = "";
                var Email = "";
                //if (PersonType == "2")
                //{
                FieldName = "Company_fldId";
                m.prs_UpdateReceiveReport("CompanyProfile", Convert.ToInt32(id), u.UserSecondId);
                Msg = "عملیات با موفقیت انجام شد.";
                //}
                //else
                //{
                //    FieldName = "RealPerson_fldId";
                //    Msg = servic.UpdateReceiveReport_RealPerson(Convert.ToInt32(id), Session["Username"].ToString(), (Session["Password"].ToString()), out Err);
                //}
                var r = m.prs_ListCompany(FieldName, id.ToString(), 0, "", "","").FirstOrDefault();
                Mobile = r.fldMobile;
                Email = r.fldEmail;

                Msg = "ارسال موفق";
                var st = true;
                try
                {
                    string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS + Environment.NewLine + "راه آهن جمهوری اسلامی ایران", Mobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                    if (returnCode.Length <= 3)
                    {
                        MsgTitle = "خطا";
                        Msg = w.ShowError(returnCode, "FA");
                        Er = 1;
                        st = false;
                    }
                }
                catch (Exception)
                {
                    Msg = "قطع ارتباط با سرور پیامک.";
                    st = false;
                }
                m.prs_tblSMSEmailCachInsert(MatnSMS, Mobile, "", Msg, r.fldName_Family, false, false, r.fldFirstRegisterUser, st, u.UserInputId);

                Msg = "ارسال موفق";
                try
                {
                    st = SendEmail(Email, MatnSMS);
                }
                catch (Exception x)
                {
                    var Msgg = "";
                    if (x.InnerException != null)
                        Msgg = x.InnerException.Message;
                    else
                        Msgg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblErrorInsert(ErrorId, Msgg, DateTime.Now, u.UserInputId, "");
                    Msg = "قطع ارتباط با سرور ایمیل.";
                    st = false;
                }
                if (st == false)
                    Msg = "قطع ارتباط با سرور ایمیل.";
                m.prs_tblSMSEmailCachInsert(MatnSMS, "", Email, Msg, r.fldName_Family, false, true, r.fldFirstRegisterUser, st, u.UserInputId);

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
        public ActionResult GeneratePDF(string AzTarikh, string TaTarikh, string PersonType, string Status, string NameStatus)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                if (AzTarikh == null)
                    AzTarikh = "";
                if (TaTarikh == null)
                    TaTarikh = "";
                if (Status == "0")
                    Status = "";
                var q = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(), "", 1).FirstOrDefault();
                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);
                RaiSam.DataSet.DataSet1TableAdapters.prs_ListCompanyTableAdapter ListCompany = new RaiSam.DataSet.DataSet1TableAdapters.prs_ListCompanyTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();

                dt.EnforceConstraints = false;
                Pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 1);
                dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
                var FielName = "Company";
                var ReportId = "8";
                //if (Allowed != 0)
                //    FielName = "Company_Allowed";
                //if (PersonType == "1")
                //{
                //    FielName = "RealPerson";
                //    ReportId = "22";
                //    if (Status != "" && Convert.ToInt32(Status) > 4)
                //    {
                //        if (Allowed == 0)
                //            FielName = "RealPerson_Status5";
                //        else
                //            FielName = "RealPerson_Status5_Allowed";
                //    }
                //    else
                //    {
                //        if (Allowed == 0)
                //            FielName = "RealPerson";
                //        else
                //            FielName = "RealPerson_Allowed";
                //    }
                //}
                ListCompany.Fill(dt.prs_ListCompany, FielName, "", 0, AzTarikh, TaTarikh, Status);
                FastReport.Report Report = new FastReport.Report();
                //var Rp = servic.GetReportWithFilter("fldId", ReportId, 0, Session["Username"].ToString(), (Session["Password"].ToString()), out Err).FirstOrDefault();
                // MemoryStream stream1 = new MemoryStream(Rp.fldFile);
                //  Report.Load(stream1);
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\MonitoringRegisterComplete.frx");

                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                pdf.EmbeddingFonts = true;
                MemoryStream stream = new MemoryStream();
                Report.SetParameterValue("UserName", q.fldNamePersonal);
                Report.SetParameterValue("NameStatus", NameStatus);
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

        public ActionResult Print(string containerId, string AzTarikh, string TaTarikh, string PersonType, string Status, string NameStatus)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {

                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.AzTarikh = AzTarikh;
                PartialView.ViewBag.TaTarikh = TaTarikh;
                PartialView.ViewBag.PersonType = PersonType;
                PartialView.ViewBag.Status = Status;
                PartialView.ViewBag.NameStatus = NameStatus;
                return PartialView;
            }
        }

        public ActionResult PrintDarsadCompanyProfile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        //public ActionResult GeneratePDFDarsadCompanyProfile(int Allowed)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    try
        //    {
        //        Models.RaiSamEntities m = new RaiSamEntities();
        //        var q = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(), "", 1).FirstOrDefault();
        //        RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
        //        RaiSam.DataSet.DataSet1TableAdapters.prs_RptDarsadCompanyProfileTableAdapter DarsadCompany = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptDarsadCompanyProfileTableAdapter();
        //        RaiSam.DataSet.DataSet1TableAdapters.prs_RptDarsadCompanyProfile1TableAdapter DarsadCompany1 = new RaiSam.DataSet.DataSet1TableAdapters.prs_RptDarsadCompanyProfile1TableAdapter();
        //        RaiSam.DataSet.DataSet1TableAdapters.prs_tblFileSelectTableAdapter Pic = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblFileSelectTableAdapter();

        //        dt.EnforceConstraints = false;
        //        Pic.Fill(dt.prs_tblFileSelect, "fldId", "1", 1);
        //        var FieldName = "Tedad_Darsad";
        //        var FieldName1 = "Tedad_Group";
        //        //if (Allowed != 0)
        //        //{
        //        //    FieldName = "Tedad_Darsad_Allowed";
        //        //    FieldName1 = "Tedad_Group_Allowed";
        //        //}
        //        DarsadCompany.Fill(dt.prs_RptDarsadCompanyProfile, FieldName, Allowed);
        //        DarsadCompany1.Fill(dt.prs_RptDarsadCompanyProfile1, FieldName1, Allowed);
        //        FastReport.Report Report = new FastReport.Report();
        //        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptDarsadCompanyProfile.frx");

        //        Report.RegisterData(dt, "dataSet1");
        //        FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
        //        pdf.EmbeddingFonts = true;
        //        MemoryStream stream = new MemoryStream();
        //        Report.SetParameterValue("UserName", q.fldNamePersonal);
        //        Report.Prepare();
        //        Report.Export(pdf, stream);
        //        return File(stream.ToArray(), "application/pdf");
        //    }
        //    catch (Exception x)
        //    {
        //        return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult GetChecked(string FirstRegisterId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFirstRegister_HadafAllowedSelect("fldFirstRegisterId", FirstRegisterId, 0).ToList();


            int[] CheckedItem = new int[q.Count];
            for (int i = 0; i < q.Count; i++)
            {
                CheckedItem[i] = Convert.ToInt32(q[i].fldHadafId);
            }

            return Json(new
            {
                CheckedItem = CheckedItem,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResetPassword(int id, string PersonType, string State)
        {
            string Msg = "", MsgTitle = ""; var Er = 0; var FId = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                //حذف

                Models.RaiSamEntities m = new RaiSamEntities();
                if (Permission.haveAccess(203, "", "0"))
                {
                    //if (PersonType == "1")
                    //    FId = servic1.GetRealPersonLocationWithFilter("fldId", id.ToString(), 1, out Err1).FirstOrDefault().fldFirstRegisterUser;
                    //else
                    FId = m.prs_tblCompanyProfileSelect("fldId", id.ToString(), 0).FirstOrDefault().fldFirstRegisterUser;


                    System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
                    CaptchaImage img = new CaptchaImage(110, 40, family);
                    string pass = img.CreateRandomText(5).Replace('l', 's').Replace('i', 's').Replace('I', 'S').Replace('L', 'S');

                    if (State == "3")
                    {
                        m.prs_UpdatePasswordFirstRegister(CodeDecode.ComputeSha256Hash(pass), FId);
                        MsgTitle = "عملیات موفق";
                        Msg = "رمز عبور جدید: " + pass;
                        Er = 0;
                    }
                    else
                    {


                        var F = m.prs_tblFirstRegisterSelect("fldId", FId.ToString(), "", 0).FirstOrDefault();
                        string Mobile = F.fldMobile;
                        string Email = F.fldEmail;
                        string Name = F.fldName_Family;
                        int FirstRegisterUser = F.fldId;
                        bool T = false;
                        if (State == "2")
                        {
                            var z = m.prs_tblCompanyLocationSelect("fldCompanyId", id.ToString(), 0).FirstOrDefault();
                            var c = m.prs_tblCompanyProfile_DetailSelect("fldId", id.ToString(), 1).FirstOrDefault();
                            Mobile = z.fldMobileModir;
                            Email = z.fldEmail1;
                            Name = c.fldNameModir + " " + c.fldFamilyModir;
                            FirstRegisterUser = z.fldFirstRegisterUser;
                            T = true;
                        }
                        m.prs_UpdatePasswordFirstRegister(CodeDecode.ComputeSha256Hash(pass), FId);
                        var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();

                        var MatnSMS = "تغییر رمز عبور در سامانه ماده12 با موفقیت انجام شد." + Environment.NewLine + "نام کاربری: " + F.fldUserName + Environment.NewLine + "رمزعبور جدید: " + pass + Environment.NewLine;
                        var MatnEmail = "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>تغییر رمز عبور در سامانه ماده12 با موفقیت انجام شد .</p> <p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>نام کاربری: " + F.fldUserName + "</p>" + "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>رمزعبور جدید:" + pass + "</p>";

                        RaiSms.Service w = new RaiSms.Service();

                        Msg = "ارسال موفق";
                        var st = true;
                        try
                        {
                            string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS + "راه آهن جمهوری اسلامی ایران", Mobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                            if (returnCode.Length <= 3)
                            {
                                MsgTitle = "خطا";
                                Msg = w.ShowError(returnCode, "FA");
                                Er = 1;
                                st = false;
                            }
                            else
                            {
                                MsgTitle = "عملیات موفق";
                                Msg = "رمز عبور جدید برای نماینده ارسال شد.";
                                if (State == "2")
                                    Msg = "رمز عبور جدید برای مدیرعامل ارسال شد.";
                            }
                        }
                        catch (Exception)
                        {
                            Msg = "قطع ارتباط با سرور پیامک";
                            st = false;
                        }
                        m.prs_tblSMSEmailCachInsert("تغییر رمز عبور", Mobile, "", Msg, Name, T, false, FirstRegisterUser, st, u.UserInputId);

                        Msg = "ارسال موفق";
                        st = true;
                        try
                        {
                            st = SendEmail(Email, MatnEmail);
                        }
                        catch (Exception x)
                        {
                            var Msgg = "";
                            if (x.InnerException != null)
                                Msgg = x.InnerException.Message;
                            else
                                Msgg = x.Message;
                            System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                            m.prs_tblErrorInsert(ErrorId, Msgg, DateTime.Now, u.UserInputId, "");
                            //if (x.InnerException != null)
                            //    Msg = x.InnerException.Message;
                            //else
                            //    Msg = x.Message;
                            Msg = "قطع ارتباط با سرور ایمیل.";
                            st = false;
                        }
                        if (st == false)
                            Msg = "قطع ارتباط با سرور ایمیل.";
                        m.prs_tblSMSEmailCachInsert("تغییر رمز عبور", "", Email, Msg, Name, T, true, FirstRegisterUser, st, u.UserInputId);

                        //if (returnCode.Length < 3)
                        //{
                        //    MsgTitle = "خطا";
                        //    Msg = w.ShowError(returnCode, "FA");
                        //    Er = 1;
                        //}
                        //else
                        //{
                        //    MsgTitle = "عملیات موفق";
                        //    Msg = "رمز عبور جدید برای نماینده ارسال شد.";
                        //    if (State == "2")
                        //        Msg = "رمز عبور جدید برای مدیرعامل ارسال شد.";
                        //}
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
        public ActionResult DelAcc(int id, string PersonType)
        {
            string Msg = "", MsgTitle = ""; var Er = 0; int FId = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                //حذف

                Models.RaiSamEntities m = new RaiSamEntities();
                if (Permission.haveAccess(204, "", "0"))
                {
                    //if (PersonType == "1")
                    //    FId = servic1.GetRealPersonLocationWithFilter("fldId", id.ToString(), 1, out Err1).FirstOrDefault().fldFirstRegisterUser;
                    //else
                    FId = m.prs_tblCompanyProfileSelect("fldId", id.ToString(), 0).FirstOrDefault().fldFirstRegisterUser;

                    var R = m.prs_DeleteHadafAllowed_NoRequest(FId).FirstOrDefault();

                    if (R.Resulte == true)
                    {
                        MsgTitle = "خطا";
                        Msg = "برای شرکت مورد نظر درخواست ثبت شده است.";
                        Er = 1;
                    }
                    else
                    {
                        MsgTitle = "عملیات موفق";
                        Msg = "ذینفع های تایید شده برای شرکت مورد نظر، حذف شد.";
                    }
                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "شما مجاز به حذف نمی باشید.";
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
        //    reportViewer.ServerReport.ReportPath = "/RegisterCompany";
        //    ReportParameter fieldname = new ReportParameter("fieldname", "Company");
        //    ReportParameter Value = new ReportParameter("Value", " ");
        //    ReportParameter h = new ReportParameter("h", "0"); 
        //    ReportParameter Uploadfile = new ReportParameter("Uploadfile", "fldid");
        //    ReportParameter UploadfileId = new ReportParameter("UploadfileId", "32");
        //    ReportParameter NameUser = new ReportParameter("NameUser", q.fldNamePersonal);
        //    reportViewer.ServerReport.SetParameters(new ReportParameter[] { fieldname, Value, h, Uploadfile, UploadfileId, NameUser });
        //    ViewBag.ReportViewer = reportViewer;
        //    return View();
        //}
        public ActionResult BackToClientPanel(int CompanyProfileId, string PersonType)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.CompanyProfileId = CompanyProfileId;
            PartialView.ViewBag.PersonType = PersonType;
            return PartialView;
        }
        public ActionResult UpdateBackToClientPanel(int CompanyProfileId, string PersonType, string Chat)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var Mobile = "";
                var Email = "";
                var FieldName = "";
                var FieldNameUpdate = "";
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                //if (PersonType == "2")
                //{
                FieldName = "Company_fldId";
                FieldNameUpdate = "BackToClient_Company";
                //}
                //else
                //{
                //    FieldName = "RealPerson_fldId";
                //    FieldNameUpdate = "BackToClient_Person";
                //}

                var r = m.prs_ListCompany(FieldName, CompanyProfileId.ToString(), 0, "", "","").FirstOrDefault();
                Mobile = r.fldMobile;
                Email = r.fldEmail;
                var MatnSMS = "ثبت نام شما در سامانه ماده 12، تایید نشد. لطفا نسبت به بازبینی اطلاعات خود اقدام نمایید.";
                RaiSms.Service w = new RaiSms.Service();

                Msg = "ارسال موفق";
                var st = true;
                try
                {
                    string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS + Environment.NewLine + "راه آهن جمهوری اسلامی ایران", Mobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                    if (returnCode.Length <= 3)
                    {
                        MsgTitle = "خطا";
                        Msg = w.ShowError(returnCode, "FA");
                        Er = 1;
                        st = false;
                    }
                }
                catch (Exception)
                {
                    Msg = "قطع ارتباط با سرور پیامک.";
                    st = false;
                }
                m.prs_tblSMSEmailCachInsert(MatnSMS, Mobile, "", Msg, r.fldName_Family, false, false, r.fldFirstRegisterUser, st, u.UserInputId);

                try
                {
                    Msg = "ارسال موفق";
                    st = true;
                    st = SendEmail(Email, MatnSMS);
                }
                catch (Exception x)
                {
                    var Msgg = "";
                    if (x.InnerException != null)
                        Msgg = x.InnerException.Message;
                    else
                        Msgg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblErrorInsert(ErrorId, Msgg, DateTime.Now, u.UserInputId, "");
                    Msg = "قطع ارتباط با سرور ایمیل.";
                    st = false;
                }
                if (st == false)
                    Msg = "قطع ارتباط با سرور ایمیل.";
                m.prs_tblSMSEmailCachInsert(MatnSMS, "", Email, Msg, r.fldName_Family, false, true, r.fldFirstRegisterUser, st, u.UserInputId);

                m.prs_UpdateAllStatus(FieldNameUpdate, 5, u.UserSecondId, CompanyProfileId, Chat, IP);

                Msg = "بازگشت به مرحله تکمیل ثبت نام با موفقیت انجام شد";
                MsgTitle = "عملیات موفق";

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
        public ActionResult ShowTarikhche(string RequestId)
        {

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.RequestId = RequestId;
                return PartialView;
            }
        }
        //public ActionResult LoadShowTarikhche(int RequestId)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    List<RaiTrainAdminWS.OBJ_HistoryRequestInDashboard> info = null;
        //    info = servic.GetHistoryRequestInDashboardWithFilter(RequestId, Session["Username"].ToString(), (Session["Password"].ToString()), out Err).ToList();


        //    return Json(new { info = info }, JsonRequestBehavior.AllowGet);
        //}
        public FileContentResult ShowLogo(string dc, int FileId)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblUploadFileCompanySelect("fldId", FileId.ToString(), 1).FirstOrDefault();
            if (q != null)
            {
                if (q.fldFile != null)
                {
                    //return File(PDF(q.fldPic),".pdf");
                    return File((q.fldFile), q.fldPassvand);
                }

            }
            return null;

        }
        public ActionResult ShowMatnPayam()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public ActionResult ReadMaleks(string CompanyProfileId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            List<Models.prs_tblMalek_VagonInfoSelect> data = null;
            data = m.prs_tblMalek_VagonInfoSelect("", "", 0).ToList();
       
            return this.Store(data);
        }
        public ActionResult GetCheckedMalek(string CompanyProfileId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblMalek_CompanyProfileSelect("fldCompanyProfileId", CompanyProfileId, 0).ToList();


            int[] CheckedItem = new int[q.Count];
            for (int i = 0; i < q.Count; i++)
            {
                CheckedItem[i] = Convert.ToInt32(q[i].fldMalekInfoId);
            }

            return Json(new
            {
                CheckedItem = CheckedItem,
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
