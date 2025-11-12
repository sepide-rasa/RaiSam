using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Timers;
using System.Web.Configuration;
using RaiSam.Models;
using System.Threading;

namespace RaiSam.Areas.Faces.Controllers
{
    public class CompanyProfileController : Controller
    {
        //
        // GET: /Faces/CompanyProfile/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        public ActionResult Index(int SabtState, string EnterSicleIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            // var c = servic.GetFirstRegisterWithFilter("fldId", Session["FristRegisterId"].ToString(),"", 0, out Err).FirstOrDefault();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            var q = m.prs_GetDate().FirstOrDefault();
            PartialView.ViewBag.fldDateB = q.fldDateTime.AddYears(-15).ToString("yyyy-MM-dd");
            PartialView.ViewBag.fldAzTarikh = q.fldDateTime.AddDays(-1).ToString("yyyy-MM-dd");
            PartialView.ViewBag.fldTarikh = q.fldTarikh;
           
            PartialView.ViewBag.SabtState = SabtState;

            var set = m.prs_tblSettingSelect("fldId", "1", 0).FirstOrDefault();
            string FromService = "0";
            if (set.fldCompanyFromService == true)
                FromService = "1";
            PartialView.ViewBag.FromService = FromService;
            PartialView.ViewBag.TypePerson = 2;

            var IsInClient=1;
            int req = 0;

            if (EnterSicleIds != null)
            {
                var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
                var k = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
                PartialView.ViewBag.FirstId = k.fldFirstRegisterId;
                req = k.fldRequestId;
            }
            else
            {
                PartialView.ViewBag.FirstId = Convert.ToInt32(Session["FristRegisterId"]);
                if (Session["RankingId"] != null)
                {
                    req = Convert.ToInt32(Session["RankingId"]);
                   
                }
            }
            PartialView.ViewBag.ReqId = req;
            var qq = m.prs_tblRequestRankingSelect("fldId", req.ToString(), 0).FirstOrDefault();
            if (qq != null)
                if (qq.fldKartablId != 100) IsInClient = 0;
            PartialView.ViewBag.IsInClient = IsInClient;

            return PartialView;
        }
        public FileContentResult DownloadV(string State)
        {
            if (Session["Username"] == null && Session["FristRegisterId"] == null)
                return null;
            string savePath = Server.MapPath(@"~\Uploaded\Help\CompanyProfile.mp4");
            var name = "CompanyProfile";
            if (State == "2")
            {
                savePath = Server.MapPath(@"~\Uploaded\Help\CompanyInfo.mp4");
                name = "CompanyInfo";
            }
            MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
            return File(st.ToArray(), MimeType.Get(".mp4"), "Help_" + name + ".mp4");
        }
        public ActionResult ShowConversationCompanyProfile()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.FirstRegisterId = Session["FristRegisterId"].ToString();
            return PartialView;
        }
        public ActionResult ShowMsgCompanyProfile(string Message, string SenderId, int Id)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            if (SenderId != "0")
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                m.prs_UpdateStatusMsg("CompanyProfile", Id, true);
            }
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Message = Message;
            return PartialView;
        }

        public ActionResult NewCh_CompanyProfile(string FirstRegisterId)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();

            PartialView.ViewBag.FirstRegisterId = FirstRegisterId;
            return PartialView;
        }
        public ActionResult ReadCh_CompanyProfile(string FristRegisterId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<Models.prs_tblCh_CompanyProfileSelect> data = null;
            data = m.prs_tblCh_CompanyProfileSelect("fldFristRegisterId", FristRegisterId, "", 100).ToList();
            return this.Store(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNewCh_CompanyProfile(prs_tblCh_CompanyProfileSelect ChatCP)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            string Msg = "";
            string MsgTitle = ""; byte Er = 0;
            var Date = m.prs_GetDate().FirstOrDefault().fldTarikh;
            var Time = m.prs_GetDate().FirstOrDefault().fldTime;
            try
            {
                 m.prs_tblCh_CompanyProfileInsert(false, null, ChatCP.fldMsg, Convert.ToInt32(Date.Replace("/","")), Time, 1, ChatCP.fldFirstRegisterId, IP);
                 Msg = "ذخیره با موفقیت انجام شد.";
                MsgTitle = "ذخیره موفق";
               
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
        public ActionResult checkCap(string Cap)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["captchaHefazati"] == null)
                return RedirectToAction("Index", "CompanyProfile");
            if (Cap == "")
            {
                Session["captchaHefazati"] = "Error";
                MsgTitle = "خطا";
                Msg = "لطفا کد امنیتی را وارد نمایید.";
                Er = 1;
            }
            else
            {

                if (Cap.ToLower() != Session["captchaHefazati"].ToString().ToLower())
                {
                    Session["captchaHefazati"] = "Error";
                    MsgTitle = "خطا";
                    Msg = "لطفا کد امنیتی را صحیح وارد نمایید.";
                    Er = 1;
                }
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// تابع ایجاد کد امنیتی
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public FileContentResult generateCaptcha(string dc)
        {
            System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
            CaptchaImage img = new CaptchaImage(110, 40, family);
            string text = img.CreateRandomText(5);
            text = text.ToUpper();
            text = text.Replace("O", "P").Replace("0", "2").Replace("1", "3").Replace("I", "M");
            img.SetText(text);
            img.GenerateImage();
            MemoryStream stream = new MemoryStream();
            img.Image.Save(stream,
            System.Drawing.Imaging.ImageFormat.Png);
            Session["captchaHefazati"] = text;
            return File(stream.ToArray(), "jpg");
        }
        /// <summary>
        ///فرم پیش نمایش گزارش
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintForm()
        {
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;

        }
        /// <summary>
        /// تابع ایجاد فایل گزارش جهت چاپ
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateForm()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            try
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                // UniThesis.DataSet.DataSet1 dt = new DataSet.DataSet1();

                var q = m.prs_tblSettingSelect("fldId", "1", 0).FirstOrDefault();
                var c = m.prs_tblCompanyProfileSelect("fldFirstRegisterId_FinalSabt", Session["FristRegisterId"].ToString(), 0).FirstOrDefault();
                var f = m.prs_tblFirstRegisterSelect("fldId", Session["FristRegisterId"].ToString(), "", 0).FirstOrDefault();

                RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
                dt.EnforceConstraints = false;

                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter dd = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                dd.Fill(dt.prs_GetDate);
                //RaiSam.DataSet.DataSet1TableAdapters.prs_rptRealPersonLocationTableAdapter Companypersonal = new RaiSam.DataSet.DataSet1TableAdapters.prs_rptRealPersonLocationTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_ReportCompanyTableAdapter ReportCompany = new RaiSam.DataSet.DataSet1TableAdapters.prs_ReportCompanyTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter _File = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter Date = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();

                ReportCompany.Fill(dt.prs_ReportCompany, Convert.ToInt32(Session["FristRegisterId"].ToString()));
                //Companypersonal.Fill(dt.prs_rptRealPersonLocation, Convert.ToInt32(Session["FristRegisterId"].ToString()));
                _File.Fill(dt.prs_tblSettingSelect, "fldId", "1", 0);
                Date.Fill(dt.prs_GetDate);
                FastReport.Report Report = new FastReport.Report();

                string barcode = WebConfigurationManager.AppSettings["PrintURL"] + "/CompanyProfile/Get?Id=";

                    barcode = barcode + c.fldId;
                    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptFirstRegister.frx");
               
               
                Report.SetParameterValue("barcode", barcode);
                Report.RegisterData(dt, "raiSamDataSet");
                FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                MemoryStream stream = new MemoryStream();
                Report.Prepare();
                Report.Export(pdf, stream);
                return File(stream.ToArray(), "application/pdf");

            }
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Get(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public FileResult DownloadPishSabt(string Id)
        {
            //if (Request.IsAjaxRequest() == false)
            //{
            //    return null;
            //}
            //if (Session["FristRegisterId"] == null)
            //    return null;

            Models.RaiSamEntities m = new RaiSamEntities();

            var s = Id.Substring(0, 1);
            int? FirstR = 0;
            //if (s == "S")
            //{
            //    Id = Id.Substring(2);
            //    FirstR = servic.GetRealPersonLocationWithFilter("fldId", Id, 0, out Err).FirstOrDefault().fldFirstRegisterUser;
            //}
            //else
                FirstR = m.prs_tblCompanyProfileSelect("fldId", Id, 0).FirstOrDefault().fldFirstRegisterUser;

            var q = m.prs_tblSettingSelect("fldId", "1", 0).FirstOrDefault();
            var f = m.prs_tblFirstRegisterSelect("fldId", FirstR.ToString(), "", 0).FirstOrDefault();

            RaiSam.DataSet.DataSet1 dt = new DataSet.DataSet1();
            dt.EnforceConstraints = false;
            RaiSam.DataSet.DataSet1TableAdapters.prs_ReportCompanyTableAdapter ReportCompany = new RaiSam.DataSet.DataSet1TableAdapters.prs_ReportCompanyTableAdapter();
            //RaiTrainRating.DataSet.DataSet1TableAdapters.prs_rptRealPersonLocationTableAdapter ReportPerson = new RaiTrainRating.DataSet.DataSet1TableAdapters.prs_rptRealPersonLocationTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter _File = new RaiSam.DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
            RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter Date = new RaiSam.DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();

            ReportCompany.Fill(dt.prs_ReportCompany, Convert.ToInt32(FirstR));
            _File.Fill(dt.prs_tblSettingSelect, "fldId", "1", 0);
            Date.Fill(dt.prs_GetDate);
            FastReport.Report Report = new FastReport.Report();

            string barcode = WebConfigurationManager.AppSettings["PrintURL"] + "/CompanyProfile/Get?Id=";
            //if (f.fldPersonalType == false)
            //{
                barcode = barcode + Id;
                Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptFirstRegister.frx");
            //}
            //else
            //{
            //    barcode = barcode + "S_" + Id;
            //    Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\rptRealPerson.frx");
            //}
            Report.SetParameterValue("barcode", barcode);
            Report.RegisterData(dt, "raiSamDataSet");
            FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
            MemoryStream stream = new MemoryStream();
            Report.Prepare();
            Report.Export(pdf, stream);
            //  return File(stream.ToArray(), "application/pdf");

            return File(stream.ToArray(), "application/pdf", "PrintForm.pdf");
            //else
            //    return null;
        }
        /// <summary>
        /// تمامی اطلاعات انواع شرکت ها را در لیست بازشو نمایش می دهد.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCompanyType()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            var q = m.prs_tblCompanyTypeSelect("", "", 0).ToList().Select(c => new { fldComId = c.fldId, fldComName = c.fldName });
            return this.Store(q);
        }
        /// <summary>
        /// تمامی اطلاعات جدول کشورها را در لیست بازشو نمایش می دهد.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCountry()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblCountrySelect("", "", 0).ToList().Select(c => new { fldCountryId = c.fldId, fldCountryName = c.fldNameCountry });
            return this.Store(q);
        }
        /// <summary>
        ///   استانهای مربوط به یک کشور را در لیست بازشو نمایش می دهد.
        /// </summary>
        /// <param name="ID">کد کشور</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetState(string ID)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblStateSelect("fldCountryId", ID, 0).ToList().Select(c => new { fldStateId = c.fldId, fldStateName = c.fldName });
            return this.Store(q);
        }
        /// <summary>
        /// شهرهای مربوط به یک استان را در لیست بازشو نمایش می دهد.
        /// </summary>
        /// <param name="ID">کد استان</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCity(string ID)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblShahrSelect("fldStateId", ID, 0).ToList().Select(c => new { fldCityId = c.fldId, fldCityName = c.fldName });
            return this.Store(q);
        }
        /// <summary>
        /// تمامی اطلاعات جدول کشورها را در لیست بازشو نمایش می دهد.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCountryLoc()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblCountrySelect("", "", 0).ToList().Select(c => new { fldCountryLocId = c.fldId, fldCountryLocName = c.fldNameCountry });
            return this.Store(q);
        }
        /// <summary>
        ///   استانهای مربوط به یک کشور را در لیست بازشو نمایش می دهد.
        /// </summary>
        /// <param name="ID">کد کشور</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetStateLoc(string ID)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblStateSelect("fldCountryId", ID, 0).ToList().Select(c => new { fldStateLocId = c.fldId, fldStateLocName = c.fldName });
            return this.Store(q);
        }
        /// <summary>
        /// شهرهای مربوط به یک استان را در لیست بازشو نمایش می دهد.
        /// </summary>
        /// <param name="ID">کد استان</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCityLoc(string ID)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblShahrSelect("fldStateId", ID, 0).ToList().Select(c => new { fldCityLocId = c.fldId, fldCityLocName = c.fldName });
            return this.Store(q);
        }
        /// <summary>
        /// تابعی برای چک کردن وجود فایل
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult HaveFile(int? Id)
        {

            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            bool HaveFile = false;
            if ((Id != null && Id != 0) || Session["savePath"] != null)
                HaveFile = true;
            return Json(new
            {
                HaveFile = HaveFile
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HaveFileLogo(int? Id)
        {

            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            bool HaveFile = false;
            if ((Id != null && Id != 0) || Session["savePathLogo"] != null)
                HaveFile = true;
            return Json(new
            {
                HaveFile = HaveFile
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// تابع آپلود فایل
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            //if (Request.Files[0].ContentType == "application/pdf")
            var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
            if (IsPDF == true)
            {
                if (Request.Files[0].ContentLength <= 102400000)
                {
                    if (Session["savePath"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                        Session.Remove("savePath");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }

                    HttpPostedFileBase file = Request.Files[0];
                    string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                    file.SaveAs(savePath);
                    Session["FileName"] = file.FileName;
                    Session["savePath"] = savePath;
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
                        Message = "حجم فایل بیشتر از حد مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "فایل غیرمجاز"
                });
                DirectResult result = new DirectResult();
                result.IsUpload = true;
                return result;
                //return null;
            }
        }
        public ActionResult HaveFileMohr(int? Id)
        {

            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            bool HaveFile = false;
            if ((Id != null && Id != 0) || Session["savePathMohr"] != null)
                HaveFile = true;
            return Json(new
            {
                HaveFile = HaveFile
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload_shakhs()
        {
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });

            //if (Request.Files[2].ContentType == "application/pdf")
            var IsPDF = FileInfoExtensions.IsPDF(Request.Files[2]);
            if (IsPDF == true)
            {
                if (Request.Files[2].ContentLength <= 102400000)
                {
                    if (Session["savePath"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                        Session.Remove("savePath");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }

                    HttpPostedFileBase file = Request.Files[2];
                    string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                    file.SaveAs(savePath);
                    Session["FileName"] = file.FileName;
                    Session["savePath"] = savePath;
                    object r = new
                    {
                        success = true,
                        name = Request.Files[2].FileName
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
                        Message = "حجم فایل بیشتر از حد مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "فایل غیرمجاز"
                });
                DirectResult result = new DirectResult();
                result.IsUpload = true;
                return result;
                //return null;
            }
        }
        public ActionResult UploadLogo()
        {
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            if (Request.Files[1].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[1].FileName) == ".jpg" || Path.GetExtension(Request.Files[1].FileName) == ".jpeg" || Path.GetExtension(Request.Files[1].FileName) == ".png")
                //if (Request.Files[1].ContentType == "image/jpg" || Request.Files[1].ContentType == "image/jpeg")
                {
                    if (Request.Files[1].ContentLength <= 102400000)
                    {
                        if (Session["savePathLogo"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathLogo"].ToString());
                            Session.Remove("savePathLogo");
                            Session.Remove("FileNameLogo");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[1];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameLogo"] = file.FileName;
                        Session["savePathLogo"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[1].FileName
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else if (Request.Files[3].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[3].FileName) == ".jpg" || Path.GetExtension(Request.Files[3].FileName) == ".jpeg" || Path.GetExtension(Request.Files[3].FileName) == ".png")
                {
                    if (Request.Files[3].ContentLength <= 102400000)
                    {
                        if (Session["savePathLogo"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathLogo"].ToString());
                            Session.Remove("savePathLogo");
                            Session.Remove("FileNameLogo");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[3];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameLogo"] = file.FileName;
                        Session["savePathLogo"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[3].FileName
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else if (Request.Files[0].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[0].FileName) == ".jpg" || Path.GetExtension(Request.Files[0].FileName) == ".jpeg" || Path.GetExtension(Request.Files[0].FileName) == ".png")
                {
                    if (Request.Files[0].ContentLength <= 102400000)
                    {
                        if (Session["savePathLogo"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathLogo"].ToString());
                            Session.Remove("savePathLogo");
                            Session.Remove("FileNameLogo");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameLogo"] = file.FileName;
                        Session["savePathLogo"] = savePath;
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else if (Request.Files[2].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[2].FileName) == ".jpg" || Path.GetExtension(Request.Files[2].FileName) == ".jpeg" || Path.GetExtension(Request.Files[2].FileName) == ".png")
                {
                    if (Request.Files[2].ContentLength <= 102400000)
                    {
                        if (Session["savePathLogo"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathLogo"].ToString());
                            Session.Remove("savePathLogo");
                            Session.Remove("FileNameLogo");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[2];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameLogo"] = file.FileName;
                        Session["savePathLogo"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[2].FileName
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "فایل غیرمجاز"
                });
                DirectResult result = new DirectResult();
                result.IsUpload = true;
                return result;
                //return null;
            }
        }
        public ActionResult UploadMohr()
        {
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            if (Request.Files[1].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[1].FileName) == ".jpg" || Path.GetExtension(Request.Files[1].FileName) == ".jpeg" || Path.GetExtension(Request.Files[1].FileName) == ".png")
                //if (Request.Files[1].ContentType == "image/jpg" || Request.Files[1].ContentType == "image/jpeg")
                {
                    if (Request.Files[1].ContentLength <= 102400000)
                    {
                        if (Session["savePathMohr"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathMohr"].ToString());
                            Session.Remove("savePathMohr");
                            Session.Remove("FileNameMohr");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[1];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameMohr"] = file.FileName;
                        Session["savePathMohr"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[1].FileName
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else if (Request.Files[3].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[3].FileName) == ".jpg" || Path.GetExtension(Request.Files[3].FileName) == ".jpeg" || Path.GetExtension(Request.Files[3].FileName) == ".png")
                {
                    if (Request.Files[3].ContentLength <= 102400000)
                    {
                        if (Session["savePathMohr"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathMohr"].ToString());
                            Session.Remove("savePathMohr");
                            Session.Remove("FileNameMohr");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[3];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameMohr"] = file.FileName;
                        Session["savePathMohr"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[3].FileName
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else if (Request.Files[0].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[0].FileName) == ".jpg" || Path.GetExtension(Request.Files[0].FileName) == ".jpeg" || Path.GetExtension(Request.Files[0].FileName) == ".png")
                {
                    if (Request.Files[0].ContentLength <= 102400000)
                    {
                        if (Session["savePathMohr"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathMohr"].ToString());
                            Session.Remove("savePathMohr");
                            Session.Remove("FileNameMohr");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameMohr"] = file.FileName;
                        Session["savePathMohr"] = savePath;
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else if (Request.Files[2].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[2].FileName) == ".jpg" || Path.GetExtension(Request.Files[2].FileName) == ".jpeg" || Path.GetExtension(Request.Files[2].FileName) == ".png")
                {
                    if (Request.Files[2].ContentLength <= 102400000)
                    {
                        if (Session["savePathMohr"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathMohr"].ToString());
                            Session.Remove("savePathMohr");
                            Session.Remove("FileNameMohr");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[2];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameMohr"] = file.FileName;
                        Session["savePathMohr"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[2].FileName
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else if (Request.Files[5].ContentType != "application/octet-stream")
            {
                if (Path.GetExtension(Request.Files[5].FileName) == ".jpg" || Path.GetExtension(Request.Files[5].FileName) == ".jpeg" || Path.GetExtension(Request.Files[5].FileName) == ".png")
                {
                    if (Request.Files[5].ContentLength <= 102400000)
                    {
                        if (Session["savePathMohr"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathMohr"].ToString());
                            Session.Remove("savePathMohr");
                            Session.Remove("FileNameMohr");
                            System.IO.File.Delete(physicalPath);
                        }

                        HttpPostedFileBase file = Request.Files[5];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameMohr"] = file.FileName;
                        Session["savePathMohr"] = savePath;
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
                            Message = "حجم فایل بیشتر از حد مجاز است."
                        });
                        DirectResult result = new DirectResult();
                        result.IsUpload = true;
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل غیرمجاز"
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
                    return result;
                    //return null;
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "فایل غیرمجاز"
                });
                DirectResult result = new DirectResult();
                result.IsUpload = true;
                return result;
                //return null;
            }
        }
        /// <summary>
        /// تابع دانلود فایل
        /// </summary>
        /// <param name="id">کد جدول صفحه اول اساسنامه</param>
        /// <returns></returns>
        public FileContentResult Download(int id, int state)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (id == 0)
            {
                if (Session["savePath"] != null)
                {
                    string savePath = Server.MapPath(Session["savePath"].ToString());
                    MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
                    return File(st.ToArray(), MimeType.Get(".pdf"), "fileUpload.pdf");
                }
                else return null;
            }
            else
            {
                int? FId = 0;
                //if (state == 1)
                    FId = m.prs_tblStatuteFirstPageSelect("fldCompanyId", id.ToString(), 0).FirstOrDefault().fldFileId;
                //if (state == 0)
                //    FId = servic.GetRealPersonLocationWithFilter("fldCompanyPersonalId", id.ToString(), 0, out Err).FirstOrDefault().fldFileId;
                var q = m.prs_tblUploadFileCompanySelect("fldId", FId.ToString(), 1).FirstOrDefault();
                //if (Session["savePath"] != null)
                //{
                //    string pdfPath = Session["savePath"].ToString();
                //    string FileName = Session["FileName"].ToString();
                //    string e = FileName.Substring(FileName.IndexOf('.') + 1);
                //    MemoryStream st = new MemoryStream(q.fldFile);
                //    return File(st.ToArray(), MimeType.Get(e), "fileUpload." + e);
                //}
                if (q != null)
                {
                    /*var qq = servic.GetUploadFileWithFilter("fldId", q.fldId.ToString(), 1, out Err).FirstOrDefault();
                    if (qq.fldFile != null)
                    {*/
                    string e1 = q.fldPassvand;
                    if (e1 == "")
                        e1 = "pdf";
                    MemoryStream st = new MemoryStream(q.fldFile);
                    return File(st.ToArray(), MimeType.Get(e1), "fileUpload" + e1);
                    /*}*/

                }
            }
            return null;
        }
        public FileContentResult DownloadLogo(int id, int state)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            if (id == 0)
            {
                if (Session["savePath"] != null)
                {
                    string savePath = Server.MapPath(Session["savePathLogo"].ToString());
                    MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
                    return File(st.ToArray(), MimeType.Get(".jpg"), "LogoUpload.jpg");
                }
                else return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();

                int? FId = 0;
                //if (state == 1)
                FId = m.prs_tblCompanyProfile_DetailSelect("fldHeaderId", id.ToString(), 0).FirstOrDefault().fldLogoId;
                //if (state == 2)
                //    FId = servic.GetRealPersonLocationWithFilter("fldCompanyPersonalId", id.ToString(), 0, out Err).FirstOrDefault().fldLogoId;
                var q = m.prs_tblUploadFileCompanySelect("fldId", FId.ToString(), 1).FirstOrDefault();
                //if (Session["savePath"] != null)
                //{
                //    string pdfPath = Session["savePath"].ToString();
                //    string FileName = Session["FileName"].ToString();
                //    string e = FileName.Substring(FileName.IndexOf('.') + 1);
                //    MemoryStream st = new MemoryStream(q.fldFile);
                //    return File(st.ToArray(), MimeType.Get(e), "fileUpload." + e);
                //}
                if (q != null)
                {
                    /*var qq = servic.GetUploadFileWithFilter("fldId", q.fldId.ToString(), 1, out Err).FirstOrDefault();
                    if (qq.fldFile != null)
                    {*/
                    string e1 = q.fldPassvand;
                    MemoryStream st = new MemoryStream(q.fldFile);
                    return File(st.ToArray(), MimeType.Get(e1), "LogoUpload" + e1);
                    /*}*/

                }
            }
            return null;
        }
        public FileContentResult DownloadMohr(int id, int state)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            if (id == 0)
            {
                if (Session["savePathMohr"] != null)
                {
                    string savePath = Server.MapPath(Session["savePathMohr"].ToString());
                    MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
                    return File(st.ToArray(), MimeType.Get(".jpg"), "MohrUpload.jpg");
                }
                else return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();

                int? FId = 0;
                //if (state == 1)
                FId = m.prs_tblSign_StampDigitalSelect("fldFirstRegisterId", Session["FristRegisterId"].ToString(), 0).FirstOrDefault().fldFileStampId;
                //if (state == 2)
                //    FId = servic.GetRealPersonLocationWithFilter("fldCompanyPersonalId", id.ToString(), 0, out Err).FirstOrDefault().fldLogoId;
                var q = m.prs_tblFileSelect("fldId", FId.ToString(), 1).FirstOrDefault();
                //if (Session["savePath"] != null)
                //{
                //    string pdfPath = Session["savePath"].ToString();
                //    string FileName = Session["FileName"].ToString();
                //    string e = FileName.Substring(FileName.IndexOf('.') + 1);
                //    MemoryStream st = new MemoryStream(q.fldFile);
                //    return File(st.ToArray(), MimeType.Get(e), "fileUpload." + e);
                //}
                if (q != null)
                {
                    /*var qq = servic.GetUploadFileWithFilter("fldId", q.fldId.ToString(), 1, out Err).FirstOrDefault();
                    if (qq.fldFile != null)
                    {*/
                    string e1 = q.fldPasvand;
                    MemoryStream st = new MemoryStream(q.fldFile);
                    return File(st.ToArray(), MimeType.Get(e1), "MohrUpload" + e1);
                    /*}*/

                }
            }
            return null;
        }
        /// <summary>
        /// .از این متد برای  ذخیره اطلاعات جدید یا اطلاعات ویرایش شده استفاده می شود
        /// </summary>
        /// <example>
        /// <code>
        /// if (p.fldId == 0)
        /// {
        ///    Insert
        /// }
        /// else{
        ///    Update
        /// }
        /// </code>
        /// </example>
        /// <param name="p">.که شامل تمامی فیلد های جدول مشخصات شرکت به همراه مقادیر آنها میشود object</param>
        /// <param name="l">.که شامل تمامی فیلد های جدول مشخصات محل شرکت به همراه مقادیر آنها میشود object</param>
        /// <param name="f">.که شامل تمامی فیلد های جدول فایل صفحه اول اساسنامه به همراه مقادیر آنها میشود object</param>
        /// <param name="FileId">کد فایل اساسنامه</param>
        /// <param name="captcha">کد حفاظتی</param>
        /// <returns>.موفقیت آمیز باشد پیغام ذخیره موفق یا ویرایش موفق را نمایش میدهد Update و Insert در صورتی که</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblCompanyProfileSelect p, prs_tblCompanyProfile_DetailSelect d, prs_tblCompanyLocationSelect l, prs_tblFirstRegisterSelect f, prs_tblSharesSelect s, prs_tblAshkhasSelect c, int FileId, int LogoFileId,int MohrFileId, string captcha, string TavalodNamayande, string MeliNamayande, string NameNamayande, string FamilyNamayande, string Mobile, string Email, bool fldEstelamNamayande, int RealPersonLocId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);

            /*var id = Convert.ToInt32(Session["FristRegisterId"]);
            p.fldFirstRegisterUser = id;*/
            var id = p.fldFirstRegisterUser;


            //l.fldEmali1 = email1;
            //l.fldEmali2 = email2;
            //l.fldEmali3 = email3;
            string Msg = "", MsgTitle = ""; var Er = 0; int? IDFile = 0; int? IDLogoFile = 0; int Id = 0; int? IDMohrFile = 0;
            string CodeFileId = "";
            string CodeFileIdLogo = "";
            bool CheckNationalCode = false;
            bool CheckCodeEghtesadi = false;
            try
            {
                if (l.fldAddressWebSite == null)
                    l.fldAddressWebSite = "";
                if (l.fldEmail2 == null)
                    l.fldEmail2 = "";
                if (l.fldEmail3 == null)
                    l.fldEmail3 = "";

                ////if (captcha == Session["captchaHefazati"].ToString().ToLower())
                ////{
                //var file=servic.GetStatuteFirstPageWithFilter("",Convert.ToInt32( Session["FristRegisterId"]),1).FirstOrDefault();
                byte[] report_file = null; string FileName = ""; string e = "";
                if (Session["savePath"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                    report_file = stream.ToArray();
                    FileName = Session["FileName"].ToString();
                    e = Path.GetExtension(Session["savePath"].ToString());// FileName.Substring(FileName.IndexOf('.'));

                }
                else if ((p.fldId == 0))
                {
                    Msg = "فایل انتخاب نشده است.";
                    MsgTitle = "اخطار";

                    return Json(new
                    {
                        Msg = Msg,
                        MsgTitle = MsgTitle,
                        Err = 1
                    }, JsonRequestBehavior.AllowGet);
                    //    var Image = Server.MapPath("~/Content/Blank.jpg");
                    //    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                    //    report_file = stream.ToArray();
                    //    FileName = "Blank.jpg";
                    //    e = "jpg";
                }
                byte[] report_fileLogo = null; string FileNameLogo = ""; string eLogo = "";
                if (Session["savePathLogo"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathLogo"].ToString()));
                    report_fileLogo = stream.ToArray();
                    FileNameLogo = Session["FileNameLogo"].ToString();
                    eLogo = Path.GetExtension(Session["savePathLogo"].ToString());// FileName.Substring(FileName.IndexOf('.'));

                }
                else if ( (c.fldId == 0)||( c.fldId != 0 && LogoFileId == 0))
                {
                    Msg = "فایل لوگوانتخاب نشده است.";
                    MsgTitle = "اخطار";

                    return Json(new
                    {
                        Msg = Msg,
                        MsgTitle = MsgTitle,
                        Err = 1
                    }, JsonRequestBehavior.AllowGet);
                    //    var Image = Server.MapPath("~/Content/Blank.jpg");
                    //    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                    //    report_file = stream.ToArray();
                    //    FileName = "Blank.jpg";
                    //    e = "jpg";
                }
                byte[] report_fileMohr = null; string FileNameMohr = ""; string eMohr = "";
                if (Session["savePathMohr"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathMohr"].ToString()));
                    report_fileMohr = stream.ToArray();
                    FileNameMohr = Session["FileNameMohr"].ToString();
                    eMohr = Path.GetExtension(Session["savePathMohr"].ToString());// FileName.Substring(FileName.IndexOf('.'));

                }
                else if ((c.fldId == 0) || (c.fldId != 0 && MohrFileId == 0))
                {
                    Msg = "فایل مهر شرکت انتخاب نشده است.";
                    MsgTitle = "اخطار";

                    return Json(new
                    {
                        Msg = Msg,
                        MsgTitle = MsgTitle,
                        Err = 1
                    }, JsonRequestBehavior.AllowGet);
                    //    var Image = Server.MapPath("~/Content/Blank.jpg");
                    //    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                    //    report_file = stream.ToArray();
                    //    FileName = "Blank.jpg";
                    //    e = "jpg";
                }

                if (p.fldId == 0)
                {
                    CheckNationalCode = m.prs_tblCompanyProfileSelect("fldNationalCode", p.fldNationalCode, 0).Where(k => k.fldFinal_Sabt ==true).Any();
                    if(CheckNationalCode){
                        return Json(new
                        {
                            Msg = "شناسه ملی وارد شده تکراری است",
                            MsgTitle = "خطا",
                            Err = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    CheckCodeEghtesadi = m.prs_tblCompanyProfileSelect("fldCodeEghtesadi", d.fldCodeEghtesadi, 0).Where(k => k.fldFinal_Sabt == true).Any();
                    if (CheckCodeEghtesadi)
                    {
                        return Json(new
                        {
                            Msg = "کد اقتصادی وارد شده تکراری است",
                            MsgTitle = "خطا",
                            Err = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    //ذخیره
                    //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 3))
                    //{
                    MsgTitle = "ذخیره موفق";
                    Msg = "ذخیره با موفقیت انجام شد.";


                    //if ((p.fldDateTasis != null && p.fldDateSabt != null && MyLib.Shamsi.Shamsi2miladiDateTime(p.fldDateTasis) < MyLib.Shamsi.Shamsi2miladiDateTime(p.fldDateSabt)))
                    //{
                    //    return Json(new
                    //    {
                    //        Msg = "تاریخ تاسیس شرکت باید بعد از تاریخ ثبت یا برابر آن باشد.",
                    //        MsgTitle = "خطا",
                    //        Err = 1
                    //    }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    System.Data.Entity.Core.Objects.ObjectParameter _FileId = new System.Data.Entity.Core.Objects.ObjectParameter("FileId", typeof(int));
                    var ss=m.prs_tblCompanyProfileInsert(p.fldNationalCode, p.fldFirstRegisterUser, p.fldFullName, d.fldNickName, d.fldCityId, Convert.ToInt32(d.fldDateTasis.Replace("/", "")),
                       p.fldSh_Sabt, Convert.ToInt32(p.fldDateSabt.Replace("/", "")), p.fldTypeCompanyId, p.fldFinal_Sabt, d.fldCodeEghtesadi, l.fldShahrId, l.fldAddress, l.fldCodePosti,
                       l.fldTelphon, l.fldPishShomare_Tel, l.fldMobileModir, l.fldNamabar, l.fldPishShomare_Namabar, l.fldAddressWebSite, l.fldEmail1, l.fldEmail2, l.fldEmail3, report_file, e,FileName,
                       s.fldTedadKolSaham, s.fldMablaghSaham, s.fldFaraBoors, IP,  d.fldNameModir, d.fldFamilyModir, report_fileLogo, eLogo,FileNameLogo,
                        Convert.ToInt32(c.fldTarikhTavalod.Replace("/", "")), c.fldCodeMeli, c.fldEstelam).FirstOrDefault();

                    if (ss.ErrorCode == 0)
                    {
                        IDFile = ss.FileId;
                        m.prs_tblSign_StampDigitalInsert(FileNameMohr, eMohr, report_fileMohr, null, null, null, null, Convert.ToInt32(Session["FristRegisterId"]), user.UserInputId);
                    }

                    var checkEmail = false;
                     var query = m.prs_tblFirstRegisterSelect("fldEmail", Email, "", 0).FirstOrDefault();
                    var query2 = m.prs_tblFirstRegisterSelect("fldMobile", Mobile.ToString(), "", 0).FirstOrDefault();
                    if (query != null && query.fldId != id || query2 != null && query2.fldId != id)
                        checkEmail = true;
                    
                    if (checkEmail)
                    {
                        return Json(new
                            {
                                Msg = "موبایل یا ایمیل وارد شده تکراری است",
                                MsgTitle = "خطا",
                                Err = 1
                            }, JsonRequestBehavior.AllowGet);
                    }
                    m.prs_UpdateName_Family(id, NameNamayande, FamilyNamayande, Mobile, Email, MeliNamayande, Convert.ToInt32(TavalodNamayande.Replace("/", "")), fldEstelamNamayande, IP);
                    Id = m.prs_tblStatuteFirstPageSelect("fldFileId", IDFile.ToString(), 0).FirstOrDefault().fldCompanyId;
                    IDLogoFile = m.prs_tblCompanyProfile_DetailSelect("fldHeaderId", Id.ToString(), 0).FirstOrDefault().fldLogoId;
                    IDMohrFile = m.prs_tblSign_StampDigitalSelect("fldFirstRegisterId", Session["FristRegisterId"].ToString(), 0).FirstOrDefault().fldFileStampId;

                    //}

                    //}
                    //else
                    //{
                    //    return null;
                    //}
                }
                else
                {
                    var pp = ""; byte[] ff = null; var mpp = ""; byte[] mff = null;
                    var q1 = m.prs_tblCompanyProfileSelect("fldNationalCode", p.fldNationalCode, 0).Where(k => k.fldFinal_Sabt == true).FirstOrDefault();
                    if (q1 != null && q1.fldId != p.fldId)
                        CheckNationalCode = true;
                    if (CheckNationalCode)
                    {
                        return Json(new
                        {
                            Msg = "شناسه ملی وارد شده تکراری است",
                            MsgTitle = "خطا",
                            Err = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    var q2 = m.prs_tblCompanyProfileSelect("fldCodeEghtesadi", d.fldCodeEghtesadi, 0).Where(k => k.fldFinal_Sabt == true).FirstOrDefault();
                    if (q2 != null && q2.fldId != p.fldId)
                        CheckCodeEghtesadi = true;
                    if (CheckCodeEghtesadi)
                    {
                        return Json(new
                        {
                            Msg = "کد اقتصادی وارد شده تکراری است",
                            MsgTitle = "خطا",
                            Err = 1
                        }, JsonRequestBehavior.AllowGet);
                    }

                    //ویرایش
                    //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 4))
                    //{

                    //if ((p.fldDateTasis != null && p.fldDateSabt != null && MyLib.Shamsi.Shamsi2miladiDateTime(p.fldDateTasis) < MyLib.Shamsi.Shamsi2miladiDateTime(p.fldDateSabt)))
                    //{
                    //    return Json(new
                    //    {
                    //        Msg = "تاریخ تاسیس شرکت باید بعد از تاریخ ثبت یا برابر آن باشد.",
                    //        MsgTitle = "خطا",
                    //        Err = 1
                    //    }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    MsgTitle = "ویرایش موفق";
                    Msg = "ویرایش با موفقیت انجام شد.";
                
                    if (Session["savePathMohr"] != null)
                    {
                        m.prs_tblSign_StampDigitalInsert(FileNameMohr, eMohr, report_fileMohr, null, null, null, null, Convert.ToInt32(Session["FristRegisterId"]), user.UserInputId);
                    }

                    if (Session["savePath"] != null)
                    {
                        var lfile = m.prs_tblUploadFileCompanySelect("fldId", LogoFileId.ToString(), 0).FirstOrDefault();
                        if (lfile != null)
                        {
                            ff = lfile.fldFile;
                            pp = lfile.fldPassvand;
                            FileName = lfile.fldFileName;
                        }
                      
                        if (Session["savePathLogo"] != null)
                        {
                            ff = report_fileLogo;
                            pp = eLogo;
                        }
                        m.prs_tblCompanyProfileUpdate(p.fldId, p.fldNationalCode, p.fldFirstRegisterUser, p.fldFullName, d.fldNickName, d.fldCityId, Convert.ToInt32(d.fldDateTasis.Replace("/", "")),
                      p.fldSh_Sabt, Convert.ToInt32(p.fldDateSabt.Replace("/", "")), p.fldTypeCompanyId, p.fldFinal_Sabt, d.fldCodeEghtesadi, l.fldShahrId, l.fldAddress, l.fldCodePosti,
                      l.fldTelphon, l.fldPishShomare_Tel, l.fldMobileModir, l.fldNamabar, l.fldPishShomare_Namabar, l.fldAddressWebSite, l.fldEmail1, l.fldEmail2, l.fldEmail3, FileId, report_file, e,FileName,
                      s.fldTedadKolSaham, s.fldMablaghSaham, s.fldFaraBoors, IP, d.fldNameModir, d.fldFamilyModir, ff, pp,FileNameLogo, LogoFileId,
                       Convert.ToInt32(c.fldTarikhTavalod.Replace("/", "")), c.fldCodeMeli, c.fldEstelam);
                            
                        
                    }
                    else 
                    {
                        var file = m.prs_tblUploadFileCompanySelect("fldId", FileId.ToString(), 0).FirstOrDefault();
                        var lfile = m.prs_tblUploadFileCompanySelect("fldId", LogoFileId.ToString(), 0).FirstOrDefault();
                        if (lfile != null)
                        {
                            ff = lfile.fldFile;
                            pp = lfile.fldPassvand;
                            FileNameLogo = lfile.fldFileName;
                        }
                        if (Session["savePathLogo"] != null)
                        {
                            ff = report_fileLogo;
                            pp = eLogo;
                        }
                        m.prs_tblCompanyProfileUpdate(p.fldId, p.fldNationalCode, p.fldFirstRegisterUser, p.fldFullName, d.fldNickName, d.fldCityId, Convert.ToInt32(d.fldDateTasis.Replace("/", "")),
                     p.fldSh_Sabt, Convert.ToInt32(p.fldDateSabt.Replace("/", "")), p.fldTypeCompanyId, p.fldFinal_Sabt, d.fldCodeEghtesadi, l.fldShahrId, l.fldAddress, l.fldCodePosti,
                     l.fldTelphon, l.fldPishShomare_Tel, l.fldMobileModir, l.fldNamabar, l.fldPishShomare_Namabar, l.fldAddressWebSite, l.fldEmail1, l.fldEmail2, l.fldEmail3, FileId, file.fldFile, file.fldPassvand, file.fldFileName,
                     s.fldTedadKolSaham, s.fldMablaghSaham, s.fldFaraBoors, IP,  d.fldNameModir, d.fldFamilyModir, ff, pp,FileNameLogo, LogoFileId,
                      Convert.ToInt32(c.fldTarikhTavalod.Replace("/", "")), c.fldCodeMeli, c.fldEstelam);
                    }

                    var checkEmail = false;
                    var query = m.prs_tblFirstRegisterSelect("fldEmail", Email, "", 0).FirstOrDefault();
                    var query2 = m.prs_tblFirstRegisterSelect("fldMobile", Mobile.ToString(), "", 0).FirstOrDefault();
                    if (query != null && query.fldId != id || query2 != null && query2.fldId != id)
                     checkEmail = true;
                    
                    if (checkEmail)
                    {
                        return Json(new
                        {
                            Msg = "موبایل یا ایمیل وارد شده تکراری است",
                            MsgTitle = "خطا",
                            Err = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    m.prs_UpdateName_Family(id, NameNamayande, FamilyNamayande, Mobile, Email,MeliNamayande,Convert.ToInt32(TavalodNamayande.Replace("/","")),fldEstelamNamayande,  IP);
                        IDFile = FileId;
                        IDLogoFile = LogoFileId;
                    if(LogoFileId==0)
                        IDLogoFile = m.prs_tblCompanyProfile_DetailSelect("fldHeaderId", p.fldId.ToString(), 0).FirstOrDefault().fldLogoId;
                        Id = p.fldId;

                        IDMohrFile = MohrFileId;
                        if (MohrFileId == 0)
                            IDMohrFile = m.prs_tblSign_StampDigitalSelect("fldFirstRegisterId", Session["FristRegisterId"].ToString(), 0).FirstOrDefault().fldFileStampId;
                    
                    //}

                }

                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    System.IO.File.Delete(physicalPath);
                    Session.Remove("savePath");
                }
                if (Session["savePathLogo"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathLogo"].ToString());
                    System.IO.File.Delete(physicalPath);
                    Session.Remove("savePathLogo");
                }
                if (Session["savePathMohr"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathMohr"].ToString());
                    System.IO.File.Delete(physicalPath);
                    Session.Remove("savePathMohr");
                }
                //}
                //else
                //{
                //    return Json(new
                //    {
                //        Msg = "کد حفاظتی نادرست است.",
                //        MsgTitle = "خطا",
                //        Err = 1
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var cc = m.prs_tblUploadFileCompanySelect("fldId", IDFile.ToString(), 1).FirstOrDefault();
                CodeFileId = cc.fldCodeFileId;

                cc = m.prs_tblUploadFileCompanySelect("fldId", IDLogoFile.ToString(), 1).FirstOrDefault();
                CodeFileIdLogo = cc.fldCodeFileId;
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
                Err = Er,
                IDFile = IDFile,
                IDLogoFile = IDLogoFile,
                IDMohrFile = IDMohrFile,
                Id = Id
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// . از این متد برای قسمت ویرایش فرم استفاده میشود.زمانی که فرم در حالت ویرایش باشد اطلاعات مربوط به آن قسمت را نشان میدهد
        /// </summary>
        /// <returns>اطلاعات مربوط به یک شرکت را بر میگرداند</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int FirstId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();

            int FileId = 0;
            int? LogoFileId = 0;
            int? MohrFileId = 0;
            int? shakhs_FileId = 0;
            var f = m.prs_tblFirstRegisterSelect("fldId", FirstId.ToString(), "", 0).FirstOrDefault();


            var c = m.prs_tblCompanyProfileSelect("fldFirstRegisterId_FinalSabt", FirstId.ToString(), 0).FirstOrDefault();
                if (c != null)
                {
                    var d = m.prs_tblCompanyProfile_DetailSelect("fldHeaderId", c.fldId.ToString(), 0).FirstOrDefault();
                    var L = m.prs_tblCompanyLocationSelect("fldCompanyId", c.fldId.ToString(), 0).FirstOrDefault();
                    var S = m.prs_tblStatuteFirstPageSelect("fldCompanyId", c.fldId.ToString(), 0).FirstOrDefault();
                    var sh = m.prs_tblSharesSelect("fldCompanyId", c.fldId.ToString(), 0).FirstOrDefault();
                    var ss = m.prs_tblSign_StampDigitalSelect("fldFirstRegisterId", FirstId.ToString(), 1).FirstOrDefault();
                    if (S.fldFileId != null)
                        FileId = S.fldFileId;
                    if (d.fldLogoId != null)
                        LogoFileId = d.fldLogoId;
                    if (ss != null)
                        MohrFileId = ss.fldFileStampId;
                    return Json(new
                    {
                        Er = 0,
                        fldId = c.fldId,
                        fldCityId = d.fldCityId.ToString(),
                        fldCountryId = d.fldCountryId.ToString(),
                        fldStateId = d.fldStateId.ToString(),
                        fldCodeEghtesadi = d.fldCodeEghtesadi,
                        fldDateSabt = c.fldDateSabt,
                        fldDateTasis = d.fldDateTasis,
                        fldFirstRegisterId = c.fldFirstRegisterUser,
                        fldFullName = c.fldFullName,
                        fldNationalCode = c.fldNationalCode,
                        fldNickName = d.fldNickName,
                        fldSh_Sabt = c.fldSh_Sabt,
                        fldTypeCompanyId = c.fldTypeCompanyId.ToString(),
                        fldFirstRegisterUser = c.fldFirstRegisterUser,
                        fldStatus = c.fldStatus,
                        fldFinal_Sabt = c.fldFinal_Sabt,
                        fldNameModirAmel = c.fldNameModirAmel,
                        fldFamilyModirAmel = c.fldFamilyModirAmel,
                        CodeMeliModir=d.CodeMeliModir,
                        fldTarikhTavalodModir=d.fldTarikhTavalodModir,
                        fldUserAccept = c.fldUserAccept,
                        CountryIdLoc = L.CountryIdLoc.ToString(),
                        StateIdLoc = L.StateIdLoc.ToString(),
                        fldShahrId = L.fldShahrId.ToString(),
                        fldAddress = L.fldAddress,
                        fldAddressWebSite = L.fldAddressWebSite,
                        fldCodePosti = L.fldCodePosti,
                        fldCompanyId = L.fldCompanyId,
                        fldEmail1 = L.fldEmail1,
                        fldEmail2 = L.fldEmail2,
                        fldEmail3 = L.fldEmail3,
                        fldMobileModir = L.fldMobileModir,
                        fldNamabar = L.fldNamabar,
                        fldPishShomare_Namabar = L.fldPishShomare_Namabar,
                        fldPishShomare_Tel = L.fldPishShomare_Tel,
                        fldTelphon = L.fldTelphon,
                        FileId = S.fldFileId,
                        LogoFileId = LogoFileId,
                        MohrFileId=MohrFileId,

                        fldMablaghSaham = sh.fldMablaghSaham,
                        fldTedadKolSaham = sh.fldTedadKolSaham,
                        fldFaraBoors = sh.fldFaraBoors,

                        fldName = f.fldName,
                        fldFamily = f.fldFamily,
                        MobileNamayande = f.fldMobile,
                        EmailNamayande = f.fldEmail,
                        fldPersonalType = "2",

                        Tavalod_Namayande=f.fldTarikhTavalod,
                        Melicode_Namayande=f.fldCodeMeli,
                        fldNameNamayande = f.fldName,
                        fldFamilyNamayande = f.fldFamily
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Er = 1,
                        shakhs_FileId = 0,
                        FileId = 0
                    }, JsonRequestBehavior.AllowGet);
                }
           
        }
        bool invalid = false;
        bool invalid2 = false;
        /// <summary>
        /// این متد برای تشخیص معتبر بودن ایمیل استفاده می شود.
        /// </summary>
        /// <param name="Email">ایمیل وارد شده</param>
        /// <returns></returns>
        public ActionResult checkEmail(string Email, string EmailNamayande)
        {

            if (String.IsNullOrEmpty(Email))
                invalid = false;

            else
            {
                Email = Regex.Replace(Email, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);

                invalid = Regex.IsMatch(Email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            }


            if (String.IsNullOrEmpty(EmailNamayande))
                invalid2 = false;

            else
            {
                EmailNamayande = Regex.Replace(EmailNamayande, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);

                invalid2 = Regex.IsMatch(EmailNamayande, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            }
            return Json(new { valid = invalid, validNamayande = invalid2 }, JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstelamSitaad(string BirthDate, string NationalCode)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            try
            {
                //EstelamSabtService sabt = new EstelamSabtService();
                //EstelamOut PerInfo = new EstelamOut();
                //RTRSabtService.EstelamSabtService sabt = new RTRSabtService.EstelamSabtService();
                //RTRSabtService.EstelamOut PerInfo = new RTRSabtService.EstelamOut();
                OutputDto_SabtAhvalSitad PerInfo = new OutputDto_SabtAhvalSitad();
                PerInfo = GetPersonInfoFromSabt(BirthDate, NationalCode);
                var msg = PerInfo.Message;
                
                if (PerInfo.Err == 1)
                    msg = PerInfo.Message + "در غیر اینصورت اطلاعات را به صورت دستی وارد نمایید.";
                var shsh = PerInfo.ShenasnameNo.ToString();
                if (shsh == "0")
                    shsh = PerInfo.NationalCode.ToString();
                return Json(new
                {
                    Er = PerInfo.Err,
                    Message = msg,
                    Name = PerInfo.Name,
                    Family = PerInfo.Family,
                    FatherName = PerInfo.FatherName,
                    BirthDate = PerInfo.BirthDate,
                    BookNo = PerInfo.BookNo,
                    BookRow = PerInfo.BookRow,
                    DeathDate = PerInfo.deathDate,
                    DeathStatus = PerInfo.DeathStatus,
                    Gender = PerInfo.Gender.ToString(),
                    NationalCode = PerInfo.NationalCode,
                    OfficeCode = PerInfo.OfficeCode,
                    ShenasnameNo = shsh,
                    ShenasnameSerial = PerInfo.shenasnameSerial
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckHaveCompany(string Shenase)
        {

            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            //var q = m.prs_tblCompanyProfileSelect("fldNationalCode", Shenase, 0).Where(l => l.fldFinal_Sabt == null).FirstOrDefault();
            var q = m.prs_tblAshkhasHoghooghiSelect("fldNationalCode", Shenase, 0).Where(l => l.fldFinal_Sabt == null).FirstOrDefault();
            if (q != null)
            {
                return Json(new
                {
                    Error = 0,
                    fldId = q.fldId,
                    fldDateSabt = q.fldDateSabt,
                   // fldTypeCompanyId = q.fldTypeCompanyId.ToString(),
                    fldNationalCode = q.fldNationalCode,
                    fldFullName = q.fldName,
                    fldSh_Sabt = q.fldShomareSabt,
                    fldTarikhTasis=q.fldTarikhTasis,
                    fldCodeEghtesadi=q.fldCodeEghtesadi,
                    fldAddress=q.fldAddress,
                    fldCodePosti=q.fldCodePosti,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Error = 1
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadFromCompanyPersons(string MeliCode)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }

            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_tblAshkhasSelect("fldCodeMeli", MeliCode, "","","", 0).FirstOrDefault();
            if (q != null)
            {
                return Json(new
                {
                    Error = 0,
                    fldId = q.fldId,
                    fldName = q.fldName,
                    fldFamily = q.fldFamily,
                    fldFatherName = q.fldFatherName,
                    fldShenasnameNo = q.fldSh_Shenasname,
                    fldBDate = q.fldTarikhTavalod,
                    fldEmail = q.fldEmail,
                    fldMahalTavalodId = q.fldCodeMahalTavalod.ToString(),
                    fldMahalSodoorId = q.fldCodeMahalSodoor.ToString(),
                    fldFromSabtServer = q.fldEstelam
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Error = 1
                }, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EstelamCodePosti(string CodePosti)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    if (Session["FristRegisterId"] == null)
        //        return RedirectToAction("LogOn", "Account", new { area = "" });
        //    Models.RaiSamEntities m = new RaiSamEntities();
        //    try
        //    {
        //        EstelamSabtService sabt = new EstelamSabtService();
        //        OutputDto_Address AddressInfo = new OutputDto_Address();
        //        //RTRSabtService.EstelamSabtService sabt = new RTRSabtService.EstelamSabtService();
        //        //RTRSabtService.OutputDto_Address AddressInfo = new RTRSabtService.OutputDto_Address();
        //        AddressInfo = sabt.GetAddressByPostcode(CodePosti);
        //        var msg = AddressInfo.Message;
        //        if (AddressInfo.Err == 1)
        //            msg = AddressInfo.Message + "در غیر اینصورت اطلاعات را به صورت دستی وارد نمایید.";

        //        var stateId = 0;
        //        var cityId = 0;

        //        var s = m.prs_tblStateSelect("fldTitle", AddressInfo.state, 0).FirstOrDefault();
        //        if (s != null)
        //            stateId = s.fldId;
        //        var c = m.prs_tblSharesSelect("fldTitle", AddressInfo.townShip.Split('-')[0], 0).FirstOrDefault();
        //        if (c != null)
        //            cityId = c.fldId;
        //        var add = "";
        //        if (AddressInfo.zone != "")
        //            add = add + "ناحیه " + AddressInfo.zone;
        //        if (AddressInfo.preAvenue != "")
        //            add = add + "-" + AddressInfo.preAvenue;
        //        if (AddressInfo.houseNo != "")
        //            add = add + "-پلاک " + AddressInfo.houseNo;
        //        if (AddressInfo.floorNo != "")
        //            add = add + "-طبقه " + AddressInfo.floorNo;
        //        if (AddressInfo.sideFloor != "")
        //            add = add + "-واحد " + AddressInfo.sideFloor;

        //        return Json(new
        //        {
        //            Er = AddressInfo.Err,
        //            Message = msg,
        //            stateId = stateId.ToString(),
        //            cityId = cityId.ToString(),
        //            address = add
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception x)
        //    {
        //        return Json(new
        //        {
        //            Er = 1,
        //            Message = x.InnerException.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }


        //}
        public ActionResult GetTypeMojavezFaaliat()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_tblTypeMojavezFaaliatSelect("", "", 0).ToList().Select(c => new { fldId = c.fldId, fldTitle = c.fldTitle });
            return this.Store(q);
        }
        public ActionResult EstelamMojavezWin(string Code)
        {//باز شدن پنجره
            //if (Session["FristRegisterId"] == null)
            //    return RedirectToAction("LogOn", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Code = Code;
            return PartialView;
            //}
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DetailsEstelamMojavez(string Code)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }

        //    if (Session["Username"] == null && Session["FristRegisterId"] == null)
        //        return null;
        //    Models.RaiSamEntities m = new RaiSamEntities();
        //    try
        //    {
        //        EstelamSabtService sabt = new EstelamSabtService();
        //        OutputDto_ParvaneKasb ParvaneInfo = new OutputDto_ParvaneKasb();
        //        //RTRSabtService.EstelamSabtService sabt = new RTRSabtService.EstelamSabtService();
        //        //RTRSabtService.OutputDto_ParvaneKasb ParvaneInfo = new RTRSabtService.OutputDto_ParvaneKasb();
        //        ParvaneInfo = sabt.InquiryUnitByCorporateIdentity(Code);

        //        var ExpireLetter = "منقضی شده";
        //        if (ParvaneInfo.HasExpireLetter != null && ParvaneInfo.HasExpireLetter == "false")
        //            ExpireLetter = "منقضی نشده";

        //        var Tavalod = "";
        //        if (ParvaneInfo.BirthDate != null && ParvaneInfo.BirthDate != "")
        //            Tavalod = ParvaneInfo.BirthDate.Substring(0, 4) + "/" + ParvaneInfo.BirthDate.Substring(4, 2) + "/" + ParvaneInfo.BirthDate.Substring(6, 2);

        //        var sodoor = "";
        //        if (ParvaneInfo.LicIssueDate != null && ParvaneInfo.LicIssueDate != 0)
        //            sodoor = ParvaneInfo.LicIssueDate.ToString().Substring(0, 4) + "/" + ParvaneInfo.LicIssueDate.ToString().Substring(4, 2) + "/" + ParvaneInfo.LicIssueDate.ToString().Substring(6, 2);

        //        var err = ParvaneInfo.Err;
        //        var msg = ParvaneInfo.Message;
        //        if (ParvaneInfo.CorporateIdentity == null)
        //        {
        //            if (ParvaneInfo.Err == 0)
        //            {
        //                err = 2;
        //                msg = "پروانه ای با این مشخصات یافت نشد.";
        //            }
        //            ExpireLetter = "";
        //        }

        //        return Json(new
        //        {
        //            Code = Code,
        //            Er = err,
        //            Message = msg,
        //            Address = ParvaneInfo.Address,
        //            PostalCode = ParvaneInfo.PostalCode,
        //            UnionName = ParvaneInfo.UnionName,
        //            CorporateIdentity = ParvaneInfo.CorporateIdentity,
        //            Isic = ParvaneInfo.Isic,
        //            Name = ParvaneInfo.Name,
        //            Family = ParvaneInfo.Family,
        //            Father = ParvaneInfo.Father,
        //            NationalCode = ParvaneInfo.NationalCode,
        //            BirthDate = Tavalod,
        //            LicIssueDate = sodoor,
        //            IdentityNo = ParvaneInfo.IdentityNo,
        //            Religion = ParvaneInfo.Religion,
        //            MilitaryState = ParvaneInfo.MilitaryState,
        //            BoardTitle = ParvaneInfo.BoardTitle,
        //            ExpireLetter = ExpireLetter,
        //            HasExpireLetter = ParvaneInfo.HasExpireLetter
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception x)
        //    {
        //        return Json(new
        //        {
        //            Er = 1,
        //            Message = x.InnerException + "-" + x.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }


        //}
        public ActionResult OpenMap(int FirstId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = FirstId;
            return PartialView;
            //}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadFromService(int state, string data)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            try
            {
                int? k = null;
                if (state == 1)
                {
                    var q = GetInquiryByNationalCode(data);
                    if (q != null)
                        return Json(new
                        {
                            fldDateSabt = q.RegisterDate,
                            fldNationalCode = q.NationalCode,
                            fldFullName = q.Name,
                            fldSh_Sabt = q.RegisterNumber,
                            fldDateTasis = q.EstablishmentDate,
                            fldAddress = q.Address,
                            fldCodePosti = q.PostCode,
                            Err = q.Err
                        }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new
                        {
                            fldNationalCode = k,
                            Err = 0
                        }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var q = GetInquiryByName(data);
                    if (q != null)
                        return Json(new
                        {
                            fldDateSabt = q.RegisterDate,
                            fldNationalCode = q.NationalCode,
                            fldFullName = q.Name,
                            fldSh_Sabt = q.RegisterNumber,
                            fldDateTasis = q.EstablishmentDate,
                            fldAddress = q.Address,
                            fldCodePosti = q.PostCode,
                            Err = q.Err
                        }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new
                        {
                            fldNationalCode = k,
                            Err = 0
                        }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    msgErr = "عدم برقراری ارتباط با سرور",
                    Err = 1
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstelamCodePosti(string CodePosti)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            try
            {
                var stateId = 0;
                var cityId = 0;
                var add = "";
                OutputDto_Address AddressInfo = new OutputDto_Address();
                AddressInfo = GetAddressByPostcode(CodePosti);
                var msg = AddressInfo.Message;
                if (AddressInfo.Err == 1)
                    msg = AddressInfo.Message + "در غیر اینصورت اطلاعات را به صورت دستی وارد نمایید.";
                else
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var s = m.prs_tblStateSelect("fldName", AddressInfo.state, 0).FirstOrDefault();
                    if (s != null)
                        stateId = s.fldId;
                    var c = m.prs_tblShahrSelect("fldName", "شهر"+AddressInfo.townShip.Split('-')[0], 0).FirstOrDefault();
                    if (c != null)
                        cityId = c.fldId;

                    if (AddressInfo.zone != "")
                        add = add + "ناحیه " + AddressInfo.zone;
                    if (AddressInfo.preAvenue != "")
                        add = add + "-" + AddressInfo.preAvenue;
                    if (AddressInfo.houseNo != "")
                        add = add + "-پلاک " + AddressInfo.houseNo;
                    if (AddressInfo.floorNo != "")
                        add = add + "-طبقه " + AddressInfo.floorNo;
                    if (AddressInfo.sideFloor != "")
                        add = add + "-واحد " + AddressInfo.sideFloor;
                }
                return Json(new
                {
                    Er = AddressInfo.Err,
                    Message = msg,
                    stateId = stateId.ToString(),
                    cityId = cityId.ToString(),
                    address = add
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
        public static companyOut GetInquiryByNationalCode(string NationalCode)
        {
            companyOut CompanyInfo = new companyOut();
            try
            {
                RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir/");
                User AuthorizeUser = new User();
                AuthorizeUser.UserName = "Made12_LegalPerson";
                AuthorizeUser.Password = "Made12_LegalPerson.IT.14010908.583.PO43@sx9C0D$4Zws$1267UknM";
                Input_Company InputDto = new Input_Company();
                InputDto.NationalCode = NationalCode;
                var r = RaiClient.InquiryByNationalCode(AuthorizeUser, InputDto, new CancellationToken());
                if (r.Result.IsSuccess)
                {
                    if (r.Result.Data.Message != "" && r.Result.Data.Message != null && r.Result.Data.Message != "null")
                    {
                        if (r.Result.Data.Message == "err.record.not.found")
                        {
                            CompanyInfo.Message = "شناسه ملی اشتباه است.";
                            CompanyInfo.Err = 2;
                            return CompanyInfo;
                        }
                        else
                        {
                            CompanyInfo.Err = 1;
                            CompanyInfo.Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                            return CompanyInfo;
                        }
                    }
                    else
                    {
                        CompanyInfo.Err = 0;
                        CompanyInfo.Name = r.Result.Data.Name;
                        CompanyInfo.Address = r.Result.Data.Address;
                        CompanyInfo.EstablishmentDate = r.Result.Data.EstablishmentDate;
                        CompanyInfo.FollowUpNo = r.Result.Data.FollowUpNo;
                        CompanyInfo.LastChangeDate = r.Result.Data.LastChangeDate;
                        CompanyInfo.PostCode = r.Result.Data.PostCode;
                        CompanyInfo.NationalCode = r.Result.Data.NationalCode;
                        CompanyInfo.RegisterDate = r.Result.Data.RegisterDate;
                        CompanyInfo.RegisterNumber = r.Result.Data.RegisterNumber;
                        CompanyInfo.Message = r.Result.Data.Message;
                        CompanyInfo.State = r.Result.Data.State;
                        CompanyInfo.Successful = r.Result.Data.Successful;
                    }
                }
                else
                {
                    CompanyInfo.Err = 1;
                    CompanyInfo.Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                    return CompanyInfo;
                }

                return CompanyInfo;
            }
            catch (Exception)
            {
                CompanyInfo.Err = 1;
                CompanyInfo.Message = "قطع ارتباط با وب سرویس ";
                return CompanyInfo;
            }

        }
        public static companyOut GetInquiryByName(string Name)
        {
            companyOut CompanyInfo = new companyOut();
            try
            {
                RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir/");
                User AuthorizeUser = new User();
                AuthorizeUser.UserName = "Made12_LegalPerson";
                AuthorizeUser.Password = "Made12_LegalPerson.IT.14010908.583.PO43@sx9C0D$4Zws$1267UknM";
                Input_Company InputDto = new Input_Company();
                InputDto.CompanyName = Name;
                var r = RaiClient.InquiryByName(AuthorizeUser, InputDto, new CancellationToken());
                if (r.Result.IsSuccess)
                {
                    if (r.Result.Data.Message != "" && r.Result.Data.Message != null && r.Result.Data.Message != "null")
                    {
                        if (r.Result.Data.Message == "err.record.not.found")
                        {
                            CompanyInfo.Message = "شناسه ملی اشتباه است.";
                            CompanyInfo.Err = 2;
                            return CompanyInfo;
                        }
                        else
                        {
                            CompanyInfo.Err = 1;
                            CompanyInfo.Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                            return CompanyInfo;
                        }
                    }
                    else
                    {
                        CompanyInfo.Err = 0;
                        CompanyInfo.Name = r.Result.Data.Name;
                        CompanyInfo.Address = r.Result.Data.Address;
                        CompanyInfo.EstablishmentDate = r.Result.Data.EstablishmentDate;
                        CompanyInfo.FollowUpNo = r.Result.Data.FollowUpNo;
                        CompanyInfo.LastChangeDate = r.Result.Data.LastChangeDate;
                        CompanyInfo.PostCode = r.Result.Data.PostCode;
                        CompanyInfo.NationalCode = r.Result.Data.NationalCode;
                        CompanyInfo.RegisterDate = r.Result.Data.RegisterDate;
                        CompanyInfo.RegisterNumber = r.Result.Data.RegisterNumber;
                        CompanyInfo.Message = r.Result.Data.Message;
                        CompanyInfo.State = r.Result.Data.State;
                        CompanyInfo.Successful = r.Result.Data.Successful;
                    }
                }
                else
                {
                    CompanyInfo.Err = 1;
                    CompanyInfo.Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                    return CompanyInfo;
                }

                return CompanyInfo;
            }
            catch (Exception)
            {
                CompanyInfo.Err = 1;
                CompanyInfo.Message = "قطع ارتباط با وب سرویس ";
                return CompanyInfo;
            }

        }
        public OutputDto_SabtAhvalSitad GetPersonInfoFromSabt(string BirthDate, string NationalCode)
        {
            OutputDto_SabtAhvalSitad PersonInfo = new OutputDto_SabtAhvalSitad();
            try
            {
                RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir/");
                User AuthorizeUser = new User();
                AuthorizeUser.UserName = "Made12_Sitaad";
                AuthorizeUser.Password = "Made12_Sitaad.IT.14010908.583.OI23W@Aa1*vC@N87$f985Gik";
                InputDto_SabtAhval InputDto = new InputDto_SabtAhval();
                InputDto.birthDate = BirthDate.Replace(@"/", string.Empty);
                InputDto.nationalCode = NationalCode;
                var r = RaiClient.RegistryOfficeInquiry(AuthorizeUser, InputDto, new CancellationToken());
                if (r.Result.IsSuccess)
                {
                    if (r.Result.Data.type == "Exception")
                    {
                        if (r.Result.Data.exceptionCode == "ix.micro.server.unreachable")
                        {
                            PersonInfo.Message = "سرویس از سمت سرویس دهنده در دسترس نیست.";
                            PersonInfo.Err = 1;
                            return PersonInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "ix.timout.occurred")
                        {
                            PersonInfo.Message = "خطای زمانی رخ داده است.";
                            PersonInfo.Err = 1;
                            return PersonInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "ix.required.fields.not.found")
                        {
                            PersonInfo.Message = "یکی از فیلدهای اجباری درخواست ارسال نشده است.";
                            PersonInfo.Err = 1;
                            return PersonInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "ix.system.error")
                        {
                            PersonInfo.Message = "ایراد سیستمی.";
                            PersonInfo.Err = 1;
                            return PersonInfo;
                        }
                        else
                        {
                            PersonInfo.Err = 1;
                            PersonInfo.Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                            return PersonInfo;
                        }
                    }
                    else
                    {
                        PersonInfo.Err = 0;
                        PersonInfo.Name = r.Result.Data.Name;
                        PersonInfo.Family = r.Result.Data.Family;
                        PersonInfo.FatherName = r.Result.Data.FatherName;
                        PersonInfo.BookNo = r.Result.Data.BookNo;
                        PersonInfo.BookRow = r.Result.Data.BookRow;
                        PersonInfo.deathDate = r.Result.Data.deathDate;
                        PersonInfo.DeathStatus = r.Result.Data.DeathStatus;
                        PersonInfo.Gender = r.Result.Data.Gender;
                        PersonInfo.Message = r.Result.Data.exceptionMessage;
                        PersonInfo.NationalCode = r.Result.Data.NationalCode;
                        PersonInfo.OfficeCode = r.Result.Data.OfficeCode;
                        PersonInfo.ShenasnameNo = r.Result.Data.ShenasnameNo;
                        PersonInfo.shenasnameSerial = r.Result.Data.shenasnameSerial;
                        PersonInfo.shenasnameSeri = r.Result.Data.shenasnameSeri;
                    }
                }
                else
                {
                    PersonInfo.Err = 1;
                    PersonInfo.Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                    return PersonInfo;
                }

                return PersonInfo;
            }
            catch (Exception)
            {
                PersonInfo.Err = 1;
                PersonInfo.Message = "قطع ارتباط با وب سرویس ";
                return PersonInfo;
            }

        }
        public OutputDto_Address GetAddressByPostcode(string postcode)
        {
            OutputDto_Address AddressInfo = new OutputDto_Address();
            try
            {
                RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir/");
                User AuthorizeUser = new User();
                AuthorizeUser.UserName = "Made12_Sitaad";
                AuthorizeUser.Password = "Made12_Sitaad.IT.14010908.583.OI23W@Aa1*vC@N87$f985Gik";
                InputDto_Address InputDto = new InputDto_Address();
                InputDto.postcode = postcode;
                var r = RaiClient.GetAddressByPostcode(AuthorizeUser, InputDto, new CancellationToken());
                if (r.Result.IsSuccess)
                {
                    if (r.Result.Data.type == "Exception")
                    {
                        if (r.Result.Data.exceptionCode == "post.code.is.null")
                        {
                            AddressInfo.Message = "لطفا کد پستی را مشخص کنید.";
                            AddressInfo.Err = 2;
                            return AddressInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "post.response.with.error")
                        {
                            AddressInfo.Message = "سرویس پست پاسخ خطا می دهد.";
                            AddressInfo.Err = 2;
                            return AddressInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "post.no.data.found")
                        {
                            AddressInfo.Message = "اطلاعاتی برای داده ورودی پیدا نشد.";
                            AddressInfo.Err = 2;
                            return AddressInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "postcode.is.wrong")
                        {
                            AddressInfo.Message = "فرمت کدپستی ارسال شده اشتباه است.";
                            AddressInfo.Err = 2;
                            return AddressInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "ix.micro.server.unreachable")
                        {
                            AddressInfo.Message = "سرویس از سمت سرویس دهنده در دسترس نیست.";
                            AddressInfo.Err = 1;
                            return AddressInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "ix.timout.occurred")
                        {
                            AddressInfo.Message = "خطای زمانی رخ داده است.";
                            AddressInfo.Err = 1;
                            return AddressInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "ix.required.fields.not.found")
                        {
                            AddressInfo.Message = "یکی از فیلدهای اجباری درخواست ارسال نشده است.";
                            AddressInfo.Err = 2;
                            return AddressInfo;
                        }
                        else if (r.Result.Data.exceptionCode == "ix.system.error")
                        {
                            AddressInfo.Message = "ایراد سیستمی.";
                            AddressInfo.Err = 1;
                            return AddressInfo;
                        }
                        else
                        {
                            AddressInfo.exceptionCode = r.Result.Data.exceptionCode;
                            AddressInfo.Err = 1;
                            AddressInfo.Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                            return AddressInfo;
                        }
                    }
                    else
                    {
                        AddressInfo.Err = 0;
                        AddressInfo.Message = r.Result.Data.Message;
                        AddressInfo.conversationId = r.Result.Data.conversationId;
                        AddressInfo.payloadClass = r.Result.Data.payloadClass;
                        AddressInfo.type = r.Result.Data.type;
                        AddressInfo.exceptionCode = r.Result.Data.exceptionCode;
                        AddressInfo.exceptionMessage = r.Result.Data.exceptionMessage;
                        AddressInfo.houseNo = r.Result.Data.houseNo;
                        AddressInfo.floorNo = r.Result.Data.floorNo;
                        AddressInfo.location = r.Result.Data.location;
                        AddressInfo.locationType = r.Result.Data.locationType;
                        AddressInfo.parish = r.Result.Data.parish;
                        AddressInfo.postCode = r.Result.Data.postCode;
                        AddressInfo.preAvenue = r.Result.Data.preAvenue;
                        AddressInfo.sideFloor = r.Result.Data.sideFloor;
                        AddressInfo.state = r.Result.Data.state;
                        AddressInfo.townShip = r.Result.Data.townShip;
                        AddressInfo.village = r.Result.Data.village;
                        AddressInfo.zone = r.Result.Data.zone;
                        AddressInfo.buildingName = r.Result.Data.buildingName;
                        AddressInfo.description = r.Result.Data.description;
                        AddressInfo.locationCode = r.Result.Data.locationCode;
                    }
                }
                else
                {
                    AddressInfo.Err = 1;
                    AddressInfo.Message = "-خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید. در غیر اینصورت اطلاعات را به صورت دستی وارد نمایید.";
                    return AddressInfo;
                }

                return AddressInfo;
            }
            catch (Exception)
            {
                AddressInfo.Err = 1;
                AddressInfo.Message = "قطع ارتباط با وب سرویس ";
                return AddressInfo;
            }

        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpCompanyProfile()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "41", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinCompanyProfile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoCompanyProfile()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "41", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
    }
}
