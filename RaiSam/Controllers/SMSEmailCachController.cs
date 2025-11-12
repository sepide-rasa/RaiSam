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
    public class SMSEmailCachController : Controller
    {
        //
        // GET: /SMSEmailCach/
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index()
        {//باز شدن تب جدید
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 17))
            //{
            return new Ext.Net.MVC.PartialViewResult();
            //}
            //else
            //{
            //    return null;
            //}
        }
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblSMSEmailCachSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<prs_tblSMSEmailCachSelect> data1 = null;
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
                        case "fldEmail_Mobile":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldEmail_Mobile";
                            break;
                        case "fldDeliverName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldDeliverName";
                            break;
                    }
                    if (data != null)
                        data1 = m.prs_tblSMSEmailCachSelect(field, searchtext, 0).ToList();
                    else
                        data = m.prs_tblSMSEmailCachSelect(field, searchtext, 0).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_tblSMSEmailCachSelect("", "", 0).ToList();
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

            List<prs_tblSMSEmailCachSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(rangeData, data.Count);
        }
        public ActionResult ShowMatnPayam()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public ActionResult ReSend(string ID)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                var cach = m.prs_tblSMSEmailCachSelect("fldId", ID, 0).FirstOrDefault();
                var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                RaiSms.Service w = new RaiSms.Service();

                MsgTitle = "ارسال موفق";
                Msg = "با موفقیت ارسال شد.";

                if (cach.fldShMobile != "")
                {
                    Msg = "ارسال موفق";
                    var st = true;
                    try
                    {
                        var returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, cach.fldMatn, cach.fldShMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                        if (returnCode.Length < 3)
                        {
                            MsgTitle = "خطا";
                            Msg = w.ShowError(returnCode, "FA");
                            Er = 1;
                            st = false;
                        }
                    }
                    catch (Exception x)
                    {
                        //if (x.InnerException != null)
                        //    Msg = x.InnerException.Message;
                        //else
                        //    Msg = x.Message;
                        Msg = "قطع ارتباط با سرور.";
                        // Msg = haveSmsPanel.fldUserName + "*" + haveSmsPanel.fldPassword + "*" + cach.fldMatn + "*" + cach.fldShMobile + "*" + null + "*" + 1 + "*" + 2 + "*" + null + "*" + "RailWay" + "*" + null + "*" + 0 + "*" + 0 + "*" + "" + "*" + "";
                        st = false;
                    }
                    m.prs_tblSMSEmailCachUpdateStatus(Convert.ToInt32(ID), Msg, st,u.UserInputId);
                }
                else
                {
                    var st = true;
                    Msg = "ارسال موفق";
                    try
                    {
                        SendEmail(cach.fldEmail, cach.fldMatn);
                    }
                    catch (Exception)
                    {
                        Msg = "قطع ارتباط با سرور.";
                        st = false;
                    }
                    m.prs_tblSMSEmailCachUpdateStatus(Convert.ToInt32(ID), Msg, st, u.UserInputId);
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
        public void SendEmail(string Email, string Matn)
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
        }

    }
}
