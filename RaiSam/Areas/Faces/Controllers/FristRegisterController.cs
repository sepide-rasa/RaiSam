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
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using RaiSam.Models;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace RaiSam.Areas.Faces.Controllers
{
    public class FristRegisterController : Controller
    {
        //
        // GET: /Faces/FristRegister/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index()
        {
            //if (Session["User"] == null)
            //    return RedirectToAction("Login", "Account");

            return View();

        }
        public ActionResult GetHadafSabtNam()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblHadafSabtNamSelect("", "", 0).ToList().Select(n => new { ID = n.fldId, Name = n.fldTitle });
            return this.Store(q);
        }
        public ActionResult ForgetPass()
        {
            //if (Session["User"] == null)
            //    return RedirectToAction("Login", "Account");

            return View();

        }

        /// <summary>
        ///  فرم ارسال کد تایید
        /// </summary>
        /// <returns></returns>
        public ActionResult VerificationCode()
        {
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
       
        public ActionResult HelpForgetPass()
        {//باز شدن پنجره

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        /// <summary>
        /// تابع ایجاد کد امنیتی
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public FileContentResult generateCaptcha(string dc)
        {
            System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
            CaptchaImage img = new CaptchaImage(90, 40, family);
            string text = img.CreateRandomText(5);
            text = text.ToUpper();
            text = text.Replace("O", "P").Replace("0", "2").Replace("1", "3").Replace("I", "M");
            img.SetText(text);
            img.GenerateImage();
            MemoryStream stream = new MemoryStream();
            img.Image.Save(stream,
            System.Drawing.Imaging.ImageFormat.Png);
            Session["captchaText"] = text;
            return File(stream.ToArray(), "jpg");
        }
        /// <summary>
        /// تابع ارسال پیامک
        /// </summary>
        /// <param name="Mobile">شماره موبایل برای ارسال</param>
        /// <param name="CodeTaaid">کد امنیتی</param>
        [NonAction]
        public string SendMessage(string Mobile, string CodeTaaid, string UserName)
        {
            Models.RaiSamEntities m = new RaiSamEntities();

            var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();

            RaiSms.Service w = new RaiSms.Service();
            Random rand = new Random();
            string crand = rand.Next(11111111, 99999999).ToString();


            //RaiSam.SMSPanel.RasaSMSPanel_Send Sms = new RaiSam.SMSPanel.RasaSMSPanel_Send();
            //Sms.Timeout = 500000000;
            var MatnSMS = "ثبت نام شما با موفقیت انجام شد برای تکمیل ثبت نام کد زیر را در سامانه وارد نمایید." + Environment.NewLine + "کد تائید: " + CodeTaaid + Environment.NewLine + "راه آهن جمهوری اسلامی ایران";
            if (haveSmsPanel != null)
            {
                if (Mobile != "")
                {
                    if (CheckMobileNumber(Mobile))
                    {
                        var returnMsg = "";
                        //Sms.SendMessage(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, new string[] { Mobile }, MatnSMS, 1, haveSmsPanel.fldShomare);
                        var smsMsg = "ارسال موفق";
                        var smsst = true;
                        try
                        {
                            string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS, Mobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                            if (returnCode.Length <= 3)
                            {
                                smsMsg = w.ShowError(returnCode, "FA");
                                smsst = false;
                                //return (w.ShowError(returnCode, "FA"));
                            }
                            //else
                            //    return "";
                            m.prs_tblSMSEmailCachInsert(MatnSMS, Mobile, "", smsMsg, UserName, false, false, null, smsst, null);
                        }
                        catch (Exception)
                        {
                            smsMsg = "قطع ارتباط با سرور.";
                            smsst = false;
                            returnMsg = "خطا";
                            m.prs_tblSMSEmailCachInsert(MatnSMS, Mobile, "", smsMsg, UserName, false, false, null, smsst, null);
                        }

                        return returnMsg;
                    }
                    else
                        return "شماره موبایل وارد شده نامعتبر است";
                }
                else
                    return "لطفا شماره موبایل خود را وارد نمایید.";
            }
            else
            {
                return "خطا";
            }
        }
        /// <summary>
        /// تابع چک کردن معتبر بودن شماره موبایل
        /// </summary>
        /// <param name="MobileNumber">شماره موبایل</param>
        /// <returns>در صورت معتبر بودن مقدار true و در غیر این صورت مقدار false برمیگرداند</returns>
        [NonAction]
        public bool CheckMobileNumber(string MobileNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(MobileNumber, "(^(09|9)[0-9][0-9]\\d{7}$)");
        }

        public ActionResult MohtavaHtml()
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            var PageHrml = m.prs_tblPageHtmlSelect("fldId", "3", 1).FirstOrDefault();
            return Json(new { MohtavaHTML = PageHrml.fldMohtavaHtml }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckCaptcha(string Capthatext)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Capthatext == "")
            {
                Session["captchaText"] = "Error";
                MsgTitle = "خطا";
                Msg = "لطفا کد امنیتی را وارد نمایید.";
                Er = 1;
            }
            else
            {

                if (Capthatext.ToLower() != Session["captchaText"].ToString().ToLower())
                {
                    Session["captchaText"] = "Error";
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
        /// متدی برای چک کردن کد امنیتی
        /// </summary>
        /// <param name="Captha">کد امنیتی</param>
        /// <returns>در صورت درست بودن کد امنیتی، er=0 در غیر این صورت er=1</returns>
        public ActionResult CheckCaptha(string Captha)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Captha == null)
            {
                Session["captchaText"] = "Error";
                MsgTitle = "خطا";
                Msg = "لطفا کد امنیتی را وارد نمایید.";
                Er = 1;
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Err = Er
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                if (Captha != Session["captchaText"].ToString().ToLower())
                {
                    Session["captchaText"] = "Error";
                    MsgTitle = "خطا";
                    Msg = "لطفا کد امنیتی را صحیح وارد نمایید.";
                    Er = 1;
                    return Json(new
                    {
                        Msg = Msg,
                        MsgTitle = MsgTitle,
                        Err = Er
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    Err = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// .از این متد برای  ذخیره اطلاعات جدید استفاده می شود
        /// </summary>
        /// <param name="SabteName"></param>
        /// <returns>.موفقیت آمیز باشد پیغام ذخیره موفق یا ویرایش موفق را نمایش میدهد</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblFirstRegisterSelect SabteName)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0; 
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {


                System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
                CaptchaImage img = new CaptchaImage(110, 40, family);
                string CodeTaeed = img.CreateRandomText(5).Replace('l', 's').Replace('i', 's').Replace('I', 'S').Replace('L', 'S');
                string s = "";
                if (SabteName.fldId == 0)
                {
                    //ذخیره
                    //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 3))
                    //{
                    bool q1 = m.prs_tblFirstRegisterSelect("fldEmail", SabteName.fldEmail, "", 0).Any();
                    bool q2 = m.prs_tblFirstRegisterSelect("fldUserName", SabteName.fldUserName, "", 0).Any();
                    bool q3 = m.prs_tblFirstRegisterSelect("fldMobile", SabteName.fldMobile, "", 0).Any();
                    if (q1 == true || q2 == true || q3 == true)
                    {
                        MsgTitle = "خطا";
                        Msg = "موبایل یا نام کاربری یا ایمیل وارد شده تکراری است";
                    }

                    else
                    {
                        MsgTitle = "ذخیره موفق";
                        Msg = "ثبت نام شما با موفقیت انجام شد و رمز عبور شما به ایمیل وارد شده ارسال گردید .";
                        System.Data.Entity.Core.Objects.ObjectParameter fldID = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));

                        m.prs_tblFirstRegisterInsert(fldID, SabteName.fldEmail, SabteName.fldUserName, SabteName.fldMobile, "", CodeTaeed);
                        int FirstId = Convert.ToInt32(fldID.Value);

                        var TreeIds = SabteName.fldSelectedHadaf.Split(';');
                        foreach (var item in TreeIds)
                        {
                            if (item != "")
                                m.prs_tblFirstRegister_SelectedHadafInsert(FirstId, Convert.ToInt32(item),null);
                        }


                        s = SendMessage(SabteName.fldMobile, CodeTaeed, SabteName.fldUserName);
                        if (s != "")
                        {
                            MsgTitle = "خطا";
                            Er = 1;
                            Msg = s;
                        }
                        else
                            m.prs_UpdateVerificationCode(CodeTaeed, SabteName.fldMobile, SabteName.fldEmail, SabteName.fldUserName);

                        Msg = "ثبت نام شما با موفقیت انجام شد و رمز عبور شما به ایمیل وارد شده ارسال گردید . " + CodeTaeed + " )";
                    }

                   
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

                /// متدی برای چک کردن کد تایید ارسال شده به موبایل
                /// </summary>
                /// <param name="CodeTaaid"></param>
                /// <param name="Mobail"></param>              
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckCodeTaaid(string CodeTaaid, string Mobail)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                var q = m.prs_tblFirstRegisterSelect("fldMobile", Mobail, "", 0).FirstOrDefault();
                var p = m.prs_tblPageHtmlSelect("fldId", "1", 0).FirstOrDefault();
                var address = Regex.Replace(p.fldMohtavaHtml, "<.*?>", String.Empty);

                System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
                CaptchaImage img = new CaptchaImage(110, 40, family);
                string Password = img.CreateRandomText(5).Replace('l', 's').Replace('i', 's').Replace('I', 'S').Replace('L', 'S');
                var Pas = CodeDecode.ComputeSha256Hash(Password);

                if (q != null)
                {
                    if (q.fldCodeTaeed == CodeTaaid)
                    {
                        if (q.fldFlag == false)
                        {


                            var EmailSetting = m.prs_tblEmailSettingSelect("", "", 1).FirstOrDefault();
                            if (EmailSetting == null)
                            {
                                Msg = "نداشتن سرویس ایمیل و ارسال شناسه کاربری و گذرواژه با پیامک.";
                                MsgTitle = "خطا";

                                //ارسال نام کاربر و رمز عبور با پیامک در صورت قطع بودن ایمیل
                                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                                RaiSms.Service w = new RaiSms.Service();
                                //RaiTrainRating.SMSPanel.RasaSMSPanel_Send Sms = new RaiTrainRating.SMSPanel.RasaSMSPanel_Send();
                                //Sms.Timeout = 500000000;
                                var MatnSMS = "کاربر گرامی، شناسه کاربری و گذرواژه شما به ترتیب زیر است:" + Environment.NewLine
                                + "شناسه کاربری:" + q.fldUserName + Environment.NewLine + "گذرواژه:" + Password + Environment.NewLine + "راه آهن جمهوری اسلامی ایران";
                                var smsMsg = "ارسال موفق";
                                var smsst = true;
                                try
                                {
                                    string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS, Mobail, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                                    if (returnCode.Length <= 3)
                                    {
                                        smsMsg = w.ShowError(returnCode, "FA");
                                        smsst = false;
                                        Er = 1;
                                    }
                                    else
                                    {
                                        m.prs_UpdatePasswordFirstRegister(Pas, q.fldId);
                                        m.prs_tblSMSEmailCachInsert("نام کاربری و رمزعبور ثبت نام اولیه", Mobail, "", smsMsg, "", false, false, null, smsst, null);
                                    }
                                }
                                catch (Exception)
                                {
                                    smsMsg = "قطع ارتباط با سرور پیامک.";
                                    smsst = false;
                                    Er = 1;
                                }
                            }
                            else
                            {

                                /*MailAddress to = new MailAddress(q.fldEmail);
                                MailAddress from = new MailAddress(EmailSetting.fldAddressEmail);
                                MailMessage mail = new MailMessage(from, to);
                             
                               
                                mail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";
                                string savePath = Server.MapPath(@"~\Content\header.png");
                                mail.IsBodyHtml = true;
                                var inlineLogo = new LinkedResource(savePath);
                                inlineLogo.ContentId = Guid.NewGuid().ToString();
                                string body = string.Format(@"
                            <img src=""cid:{0}"" />
                            <p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>کاربر گرامی ثبت نام شما با موفقیت انجام شد، شناسه کاربری و گذرواژه شما به صورت زیر است :</p>
                            <p dir='rtl' align='right'  style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>شناسه کاربری شما: " + q.fldUserName + ' ' + "<br />" + "گذرواژه شما: " + Password + "</p>" +
                                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>با احترام،</p>" +
                                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
                                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
                                , inlineLogo.ContentId);


                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = EmailSetting.fldSendServer;
                                smtp.Port = EmailSetting.fldSendPort;
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new NetworkCredential(
                                 EmailSetting.fldAddressEmail, EmailSetting.fldPassword);
                                Console.WriteLine("authentication is ok ...");

                                smtp.EnableSsl = EmailSetting.fldSSL;

                                try
                                {
                                    Console.WriteLine("Sending email...");
                                    smtp.Send(mail);
                                    Console.WriteLine("Send Mail is !");

                                    Msg = "ثبت نام شما با موفقیت تکمیل شد و رمز عبور به ایمیل شما ارسال گردید.";
                                    MsgTitle = "عملیات موفق";
                                    m.prs_UpdatePasswordFirstRegister(Pas, q.fldId);
                                    m.prs_tblSMSEmailCachInsert("نام کاربری و رمزعبور ثبت نام اولیه", "", q.fldEmail, Msg, "", false, true, null, false, null);

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

                                            Msg = "ثبت نام شما با موفقیت تکمیل شد و رمز عبور به ایمیل شما ارسال گردید.";
                                           MsgTitle = "عملیات موفق";
                                           m.prs_UpdatePasswordFirstRegister(Pas, q.fldId);

                                
                                        }
                                        else
                                        {
                                            Msg = "ServerErr : " + ex.InnerExceptions[i].FailedRecipient;
                                            Er = 1;
                                        }
                                    }
                                    m.prs_tblSMSEmailCachInsert("نام کاربری و رمزعبور ثبت نام اولیه", "", q.fldEmail, Msg, "", false, true, null, false, null);
                                }
                                catch (Exception ex)
                                {
                                    Msg = "CatchErr : " + ex.ToString();
                                    Er = 1;
                                    m.prs_tblSMSEmailCachInsert("نام کاربری و رمزعبور ثبت نام اولیه", "", q.fldEmail, Msg, "", false, true, null, false, null);
                                }
                                return Json(new
                                    {
                                        Msg = Msg,
                                        MsgTitle = MsgTitle,
                                        Err = Er
                                    }, JsonRequestBehavior.AllowGet);*/

                                MailAddress from = new MailAddress(EmailSetting.fldAddressEmail);

                                MailAddress to = new MailAddress(q.fldEmail);

                                MailMessage mail = new MailMessage(from, to);

                                string savePath = Server.MapPath(@"~\Content\header.png");
                                mail.IsBodyHtml = true;
                                var inlineLogo = new LinkedResource(savePath);
                                inlineLogo.ContentId = Guid.NewGuid().ToString();
                                mail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";
                                string body = string.Format(@"
                            <img src=""cid:{0}"" />
                            <p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>کاربر گرامی ثبت نام شما با موفقیت انجام شد، شناسه کاربری و گذرواژه شما به صورت زیر است :</p>
                            <p dir='rtl' align='right'  style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>شناسه کاربری شما: " + q.fldUserName + ' ' + "<br />" + "گذرواژه شما: " + Password + "</p>" +
                                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>با احترام،</p>" +
                                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
                                    "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
                                , inlineLogo.ContentId);

                                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                                view.LinkedResources.Add(inlineLogo);
                                mail.AlternateViews.Add(view);

                                mail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";
                                mail.Body = "کاربر گرامی، شناسه کاربری و گذرواژه شما به ترتیب زیر است:" + Environment.NewLine
                                    + "شناسه کاربری:" + q.fldUserName + Environment.NewLine + "گذرواژه:" + Password + Environment.NewLine;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = EmailSetting.fldSendServer;
                                smtp.Port = EmailSetting.fldSendPort;
                                smtp.EnableSsl = EmailSetting.fldSSL;
                                smtp.UseDefaultCredentials = false;
                                try
                                {
                                    smtp.Credentials = new NetworkCredential(
                                       EmailSetting.fldAddressEmail, EmailSetting.fldPassword);
                                    ServicePointManager.ServerCertificateValidationCallback =
                                        delegate(object s, X509Certificate certificate,
                                                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
                                        { return true; };

                                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                                    smtp.Send(mail);
                                    m.prs_UpdatePasswordFirstRegister(Pas, q.fldId);
                                    Msg = "ثبت نام شما با موفقیت تکمیل شد و رمز عبور به ایمیل شما ارسال گردید.";
                                  //  Msg = "ثبت نام شما با موفقیت تکمیل شد و رمز عبور به ایمیل شما ارسال گردید.(رمز عبور: " + Password + " )";
                                    MsgTitle = "عملیات موفق";

                                    m.prs_tblSMSEmailCachInsert("نام کاربری و رمزعبور ثبت نام اولیه", "", q.fldEmail, Msg, "", false, true, null, true, null);
                                }
                                catch (Exception x)
                                {
                                    m.prs_tblSMSEmailCachInsert("نام کاربری و رمزعبور ثبت نام اولیه", "", q.fldEmail, "قطع ارتباط با سرویس ایمیل", "", false, true, null, false, null);

                                    var Mmsg = "";
                                    if (x.InnerException != null)
                                        Mmsg = x.InnerException.Message;
                                    else
                                        Msg = x.Message;
                                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                    m.prs_tblErrorInsert(ErrorId, Mmsg, DateTime.Now, null, "خطای ارسال ایمیل");

                                    

                                    Msg = "قطع ارتباط با سرور ایمیل و ارسال شناسه کاربری و گذرواژه با پیامک.";
                                    MsgTitle = "خطا";

                                    //ارسال نام کاربر و رمز عبور با پیامک در صورت قطع بودن ایمیل
                                    var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                                    RaiSms.Service w = new RaiSms.Service();
                                    //RaiTrainRating.SMSPanel.RasaSMSPanel_Send Sms = new RaiTrainRating.SMSPanel.RasaSMSPanel_Send();
                                    //Sms.Timeout = 500000000;
                                    var MatnSMS = "کاربر گرامی، شناسه کاربری و گذرواژه شما به ترتیب زیر است:" + Environment.NewLine
                                    + "شناسه کاربری:" + q.fldUserName + Environment.NewLine + "گذرواژه:" + Password + Environment.NewLine + "راه آهن جمهوری اسلامی ایران";
                                    var smsMsg = "ارسال موفق";
                                    var smsst = true;
                                    try
                                    {
                                        string returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, MatnSMS, Mobail, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                                        if (returnCode.Length <= 3)
                                        {
                                            smsMsg = w.ShowError(returnCode, "FA");
                                            smsst = false;
                                            Er = 1;
                                        }
                                        else
                                        {
                                            m.prs_UpdatePasswordFirstRegister(Pas, q.fldId);
                                            m.prs_tblSMSEmailCachInsert("نام کاربری و رمزعبور ثبت نام اولیه", Mobail, "", smsMsg, "", false, false, null, smsst, null);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        smsMsg = "قطع ارتباط با سرور پیامک.";
                                        smsst = false;
                                        Er = 1;
                                    }

                                    return Json(new
                                    {
                                        Msg = Msg,
                                        MsgTitle = MsgTitle,
                                        Err = Er
                                    }, JsonRequestBehavior.AllowGet);

                                }
                            }
                        }
                        else if (q.fldFlag == true)
                        {
                            return Json(new
                            {
                                Msg = "قبلا مشخصات برای شما ایمیل شده است.",
                                MsgTitle = "اخطار",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        Msg = "کد تائید وارد شده صحیح نمی باشد.";
                        MsgTitle = "خطا";
                        Er = 1;
                        return Json(new
                        {
                            Msg = Msg,
                            MsgTitle = MsgTitle,
                            Err = Er
                        }, JsonRequestBehavior.AllowGet);

                    }

                }
                else
                {
                    return Json(new
                    {
                        Err = 2
                    }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = "";
            Models.RaiSamEntities m = new RaiSamEntities();

            try
            {
                //حذف

                //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 5))
                //{

                MsgTitle = "حذف موفق"; Msg = "حذف با موفقیت انجام شد.";
                 m.prs_tblFirstRegisterDelete(id);

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
                MsgTitle = MsgTitle
            }, JsonRequestBehavior.AllowGet);
        }
        bool invalid = false;
        public ActionResult checkEmail(string Email)
        {
            var flag = false;
            var ir = false;
            var captchaEnd = false;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["captchaText"] == null)
            {
                captchaEnd = true;
            }
            var s = Email.Split('@');
            var q = m.prs_tblFirstRegisterSelect("fldEmail", Email, "", 0).FirstOrDefault();
            if (q != null)
                flag = true;
            else
            {
                var q1 = m.prs_tblCompanyLocationSelect("fldEmail", Email, 0).FirstOrDefault();
                if (q1 != null)
                    flag = true;
            }

            if (String.IsNullOrEmpty(Email))
                invalid = false;
            else if (s.Length > 1)
            {
                if (Regex.IsMatch(s[1], ".ir"))
                    ir = true;
                Email = Regex.Replace(Email, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);

                invalid = Regex.IsMatch(Email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            }

            return Json(new { captchaEnd = captchaEnd, flag = flag, valid = invalid, ir = ir }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult checkMobail(string Mobail)
        {
            var flag = false;
            var captchaEnd = false;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["captchaText"] == null)
            {
                captchaEnd = true;
            }
            var q = m.prs_tblFirstRegisterSelect("fldMobile", Mobail, "", 0).FirstOrDefault();
            if (q != null)
                flag = true;
            return Json(new { captchaEnd = captchaEnd, flag = flag }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult checkUserName(string UserName)
        {
            var flag = false;
            var captchaEnd = false;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["captchaText"] == null)
            {
                captchaEnd = true;
            }
            var q = m.prs_tblFirstRegisterSelect("fldUserName", UserName,"", 0).FirstOrDefault();
            if (q != null)
                flag = true;
            return Json(new { captchaEnd = captchaEnd, flag = flag }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFirstRegisterSelect("fldId", Id.ToString(), "", 0).FirstOrDefault();
            return Json(new
            {
                fldId = q.fldId,
                fldEmail = q.fldEmail,
                fldMobile = q.fldMobile,
                fldUserName = q.fldUserName
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidEmail(string Email)
        {

            if (String.IsNullOrEmpty(Email))
                invalid = false;

            else
            {
                Email = Regex.Replace(Email, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);

                invalid = Regex.IsMatch(Email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            }
            return Json(new
            {
                valid = invalid,
                Email = Email
            }, JsonRequestBehavior.AllowGet);
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

        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<Models.prs_tblFirstRegisterSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<Models.prs_tblFirstRegisterSelect> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        case "fldId":
                            searchtext = ConditionValue.Value.ToString();
                            field = "fldId";
                            break;
                        case "fldName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldName";
                            break;
                        case "fldCodeSystemMabda":
                            searchtext = ConditionValue.Value.ToString();
                            field = "fldCodeSystemMabda";
                            break;
                        case "fldDesc":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldDesc";
                            break;

                    }
                    if (data != null)
                        data1 = m.prs_tblFirstRegisterSelect(field, searchtext, "", 100).ToList();
                    else
                        data = m.prs_tblFirstRegisterSelect(field, searchtext, "", 100).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_tblFirstRegisterSelect("", "", "", 100).ToList();
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

            List<Models.prs_tblFirstRegisterSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PassSend(string fldEmail)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                var q = m.prs_tblFirstRegisterSelect("fldEmail", fldEmail, "", 1).FirstOrDefault();
                var p = m.prs_tblPageHtmlSelect("fldId", "1", 1).FirstOrDefault();
                var address = Regex.Replace(p.fldMohtavaHtml, "<.*?>", String.Empty);

                System.Drawing.FontFamily family = new System.Drawing.FontFamily("tahoma");
                CaptchaImage img = new CaptchaImage(110, 40, family);
                string Password = img.CreateRandomText(5).Replace('l', 's').Replace('i', 's').Replace('I', 'S').Replace('L', 'S');
                var Pas = CodeDecode.ComputeSha256Hash(Password);

                if (q != null)
                {
                    if (q.fldFlag == true)
                    {

                        //MsgTitle = "ذخیره موفق";
                        //Msg = servic.Email_SendEmail(q.fldEmail, "کاربر گرامی، شناسه کاربری و گذرواژه شما به ترتیب زیر است:" + Environment.NewLine
                        //    + "شناسه کاربری:" + q.fldUserName + Environment.NewLine + "گذرواژه:" + Password + Environment.NewLine, "سامانه جامع هوشمند تشخیص صلاحیت ذینفعان راه آهن جمهوری اسلامی ایران ", "", out Err);
                     //   var Matn = "کاربر گرامی، شناسه کاربری و گذرواژه شما به ترتیب زیر است:" + "شناسه کاربری:" + q.fldUserName + Environment.NewLine + "گذرواژه:" + Password + Environment.NewLine;

                        var EmailSetting = m.prs_tblEmailSettingSelect("", "", 1).FirstOrDefault();
                        MailAddress from = new MailAddress(EmailSetting.fldAddressEmail);

                        MailAddress to = new MailAddress(q.fldEmail);

                        MailMessage mail = new MailMessage(from, to);
                        string savePath = Server.MapPath(@"~\Content\header.png");
                        mail.IsBodyHtml = true;
                        var inlineLogo = new LinkedResource(savePath);
                        inlineLogo.ContentId = Guid.NewGuid().ToString();
                        mail.Subject = "سامانه ماده 12 راه آهن جمهوری اسلامی ایران";
                        string body = string.Format(@"
                            <img src=""cid:{0}"" />
                            <p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>کاربر گرامی در خواست شما مبنی بر تغییر رمز عبور انجام شد، شناسه کاربری و گذرواژه شما به صورت زیر است :</p>
                            <p dir='rtl' align='right'  style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>شناسه کاربری شما: " + q.fldUserName + ' ' + "<br />" + "گذرواژه شما: " + Password + "</p>" +
                            "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>با احترام،</p>" +
                            "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>سامانه ماده 12 راه آهن جمهوری اسلامی ایران</p>" +
                            "<p dir='rtl' align='right' style ='font-family:Tahoma;font-size: 11px;font-weight:bold;'>" + address + "</p>"
                        , inlineLogo.ContentId);

                        var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                        view.LinkedResources.Add(inlineLogo);
                        mail.AlternateViews.Add(view);


                        MsgTitle = "عملیات موفق";
                        var st = true;
                        try
                        {
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = EmailSetting.fldSendServer;
                            smtp.Port = EmailSetting.fldSendPort;
                            smtp.EnableSsl = EmailSetting.fldSSL;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(
                            EmailSetting.fldAddressEmail, EmailSetting.fldPassword);

                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                            smtp.Send(mail);
                            m.prs_UpdatePasswordFirstRegister(Pas, q.fldId);
                            Msg = "بازنشانی رمز عبور شما با موفقیت انجام شد و رمز عبور به ایمیل شما ارسال گردید.";
                        }
                        catch (Exception x)
                        {
                            var Mmsg = "";
                            if (x.InnerException != null)
                                Mmsg = x.InnerException.Message;
                            else
                                Mmsg = x.Message;
                            System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                            m.prs_tblErrorInsert(ErrorId, Mmsg, DateTime.Now, null, "خطای ارسال ایمیل");

                            MsgTitle = "قطع ارتباط با سرور.";
                            st = false;
                        }
                        m.prs_tblSMSEmailCachInsert("بازنشانی رمز عبور", "", q.fldEmail, MsgTitle, q.fldUserName, false, true, null, st, null);


                    }
                    else
                    {
                        MsgTitle = "خطا";
                        Msg = "ابتدا ثبت نام خود را تکمیل نمایید.";
                        Er = 1;
                    }
                }
                else
                {
                    Msg = "شما با پست الکترونیک وارد شده ثبت نام نکرده اید";
                    MsgTitle = "اخطار";
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
        
        public ActionResult TakmilSabtName()
        {
            return View();
        }
        public ActionResult HelpTakmilSabtName()
        {//باز شدن پنجره

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadHadaf(string Trees)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            List<Models.prs_HadafListForFirstRegister> data = null;

            if (Trees == null)
                Trees = "";
            data = m.prs_HadafListForFirstRegister(Trees).ToList();
            return this.Store(data);
        }
        public ActionResult Help()
        {//باز شدن پنجره
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpFristRegister()
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "39", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinFristRegister()
        {
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoFristRegister()
        {
            
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "39", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
    }
}
