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

namespace RaiSam.Areas.Faces.Controllers
{
    public class ShareholdersController : Controller
    {
        //
        // GET: /Faces/Shareholders/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string EnterSicleIds)
        {//باز شدن تب جدید
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
         
            //var k = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
            //if (k.fldStatusId == 2 || k.fldStatusId == 4 || k.fldStatusId == 5)
            //{
            //    SentToAdmin = 1;
            //}
            //else
            //{
            //    var z = servic.SelectBeforeDashboard(Convert.ToInt32(Session["RankingId"]), 4, IP, Convert.ToInt32(Session["FristRegisterId"]), out Err);
            //    if (z == false)
            //        SentToAdmin = 0;
            //    else
            //        SentToAdmin = 1;
            //}
            var IsInClient = 0;
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
                req = Convert.ToInt32(Session["RankingId"]);


            }
            var Req = m.prs_tblRequestRankingSelect("fldId", req.ToString(), 0).FirstOrDefault();
            if (Req.fldKartablId == 100) IsInClient = 1;
            PartialView.ViewBag.ReqId = req;
            PartialView.ViewBag.IsInClient = IsInClient;
            return PartialView;
        }
        public FileContentResult DownloadV()
        {
            if (Session["User"] == null)
                return null;
            string savePath = Server.MapPath(@"~\Uploaded\Help\Shareholders.mp4");

            MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
            return File(st.ToArray(), MimeType.Get(".mp4"), "Help_" + "Shareholders" + ".mp4");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DarsadSaham(long TedadSaham, string contractId, int FirstId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            double Darsad = 0.0;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            double TedadKolSaham  = 0.0;
            
            if (contractId == "")
            {
                var q = m.prs_tblSharesSelect("fldFirstRegisterUser", FirstId.ToString(), 1).FirstOrDefault();
                if (q != null)
                    TedadKolSaham = Convert.ToDouble(q.fldTedadKolSaham);
            }
            else
            {
                var k = m.prs_tblRegistrationFirstContractSelect("fldId", contractId, 0).FirstOrDefault();
                var q = m.prs_tblSharesSelect("fldFirstRegisterUser", k.fldFirstRegisterId.ToString(), 1).FirstOrDefault();
                if (q != null)
                    TedadKolSaham = Convert.ToDouble(q.fldTedadKolSaham);
            }

            if (TedadKolSaham >0)
            {
                Darsad = (TedadSaham / TedadKolSaham) * 100;
            }
            return Json(new { DarsadSaham = Darsad }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MatnHtmlShareholdersIn(string ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
           
            var MatnHtmlShareholdersIn = "";
            prs_tblHtmlViewerSelect PageHtml = null;
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                var s=m.prs_tblRequestRankingSelect("fldId", ReqId, 0).FirstOrDefault();
                PageHtml = m.prs_tblHtmlViewerSelect("Tree-NameTable", "30",s.fldHadafId, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlShareholdersIn = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlShareholdersIn = "";
            }

            return Json(new { MatnHtmlShareholdersIn = MatnHtmlShareholdersIn }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult New(int id, int FirstId, int ReqId)
        {//باز شدن پنجره
            //if (Session["Username"] == null)
            //    return RedirectToAction("Login", "Admin");
            //else
            //{
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_GetDate().FirstOrDefault();
            var a = q.fldDateTime.AddDays(-1).ToString("yyyy-MM-dd");
            a = a.Substring(0, 10);
            var saham = m.prs_MinusSaham(FirstId, ReqId).FirstOrDefault();
            var Minus = saham.Minus;
            if (id != 0)
            {
                var s = m.prs_tblShareholderSelect("fldId",id.ToString(),"","", 0).FirstOrDefault();
                Minus = Minus + s.fldTedadSaham;
            }
            PartialView.ViewBag.fldTarikhSaham = a;
            PartialView.ViewBag.fldTarikh = q.fldTarikh;
            PartialView.ViewBag.Id = id;
            PartialView.ViewBag.TedadSaham = saham.TedadSaham;
            PartialView.ViewBag.SumSaham = saham.SumSaham;
            PartialView.ViewBag.Minus = Minus;
            PartialView.ViewBag.FirstId = FirstId;
            PartialView.ViewBag.ReqId = ReqId;
            return PartialView;
            //}
        }

        public ActionResult MatnHtmlShareholders(string ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var MatnHtmlShareholders = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                var s = m.prs_tblRequestRankingSelect("fldId", ReqId, 0).FirstOrDefault();
                PageHtml = m.prs_tblHtmlViewerSelect("Tree-NameTable", "6", s.fldHadafId, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlShareholders = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlShareholders = "";
            }

            return Json(new { MatnHtmlShareholders = MatnHtmlShareholders }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MatnHtmlHoghoghiKhososi(string ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var MatnHtmlHoghoghiKhososi = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                var s = m.prs_tblRequestRankingSelect("fldId", ReqId, 0).FirstOrDefault();
                PageHtml = m.prs_tblHtmlViewerSelect("Tree-NameTable", "7", s.fldHadafId, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlHoghoghiKhososi = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlHoghoghiKhososi = "";
            }

            return Json(new { MatnHtmlHoghoghiKhososi = MatnHtmlHoghoghiKhososi }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MatnHtmlGieyrKhososi(string ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var MatnHtmlGieyrKhososi = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                var s = m.prs_tblRequestRankingSelect("fldId", ReqId, 0).FirstOrDefault();
                PageHtml = m.prs_tblHtmlViewerSelect("Tree-NameTable", "8", s.fldHadafId, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlGieyrKhososi = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlGieyrKhososi = "";
            }

            return Json(new { MatnHtmlGieyrKhososi = MatnHtmlGieyrKhososi }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewHoghoghiGheyrKhososi(int id, int FirstId, int ReqId)
        {//باز شدن پنجره
            //if (Session["Username"] == null)
            //    return RedirectToAction("Login", "Admin");
            //else
            //{
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            var q = m.prs_GetDate().FirstOrDefault();
            var a = q.fldDateTime.AddDays(-1).ToString("yyyy-MM-dd");
            a = a.Substring(0, 10);
            var saham = m.prs_MinusSaham(FirstId, ReqId).FirstOrDefault();

            PartialView.ViewBag.fldTarikhSa = a;
            PartialView.ViewBag.fldTarikh = q.fldTarikh;
            PartialView.ViewBag.Id = id;
            PartialView.ViewBag.TedadSaham3 = saham.TedadSaham;
            PartialView.ViewBag.SumSaham3 = saham.SumSaham;
            PartialView.ViewBag.Minus3 = saham.Minus;
            PartialView.ViewBag.FirstId = FirstId;
            PartialView.ViewBag.ReqId = ReqId;
            return PartialView;
            //}
        }
        public ActionResult ShowInfoShareholders(string Id, string CompanyPersonalId, string CompanyId, string OrganId, string EnterSicleIds, string contractId, int FirstId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            PartialView.ViewBag.FirstId = FirstId;
            PartialView.ViewBag.CompanyPersonalId = CompanyPersonalId;
            PartialView.ViewBag.CompanyId = CompanyId;
            PartialView.ViewBag.OrganId = OrganId;

            var HaveTaeed = false;
            if (contractId != "")
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var k = m.prs_tblRegistrationFirstContractSelect("fldId", contractId, 0).FirstOrDefault();
                HaveTaeed = m.prs_tblTaeedItemsSelect("fldHadafId", k.fldHadafId.ToString(), 0).Where(l => l.fldApp_ClientId == 10).Any();
                PartialView.ViewBag.contractId = contractId;
            }
            PartialView.ViewBag.HaveTaeed = HaveTaeed;
            return PartialView;
        }

        //public ActionResult ShowShareholdersConversation(string Status, string ShareholderId, string StateAORC, string StatusRequest, string UserNameAdmin, string PassAdmin)
        //{
        //    int Permission = 0;
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //    }
        //    if (Status != "")
        //    {
        //        if (StatusRequest == "Completed")
        //        {
        //            if (!servic1.Permission(228, UserNameAdmin, PassAdmin, out Err1))
        //            {
        //                Permission = 1;
        //            }
        //        }
        //        if (Permission == 1)
        //        {
        //            X.Msg.Show(new MessageBoxConfig
        //            {
        //                Buttons = MessageBox.Button.OK,
        //                Icon = MessageBox.Icon.ERROR,
        //                Title = "خطا",
        //                Message = "شما مجاز به دسترسی نمی باشید."
        //            });
        //            DirectResult result = new DirectResult();
        //            return result;
        //        }
        //    }

        //    Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //    PartialView.ViewBag.ShareholderId = ShareholderId;
        //    PartialView.ViewBag.StateAORC = StateAORC;
        //    PartialView.ViewBag.StatusRequest = StatusRequest;
        //    PartialView.ViewBag.UserNameAdmin = UserNameAdmin;
        //    PartialView.ViewBag.PassAdmin = PassAdmin;
        //    PartialView.ViewBag.Status = Status;
        //    return PartialView;
        //}
        //public ActionResult ShowMsgShareholders(string Message, string SenderId, string TypeMsgName, string TypeMsg, bool IsGrade, int Id, int FileId, string StateAORC)
        //{
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        if (SenderId != "0")
        //        {
        //            servic.UpdateStatusMsg("Shareholders", Id, true, out Err);
        //        }
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //        if (SenderId == "0")
        //        {
        //            servic.UpdateStatusMsg("Shareholders", Id, true, out Err);
        //        }
        //    }
        //    Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //    PartialView.ViewBag.Message = Message;
        //    PartialView.ViewBag.TypeMsgName = TypeMsgName;
        //    PartialView.ViewBag.IsGrade = IsGrade;
        //    PartialView.ViewBag.StateAORC = StateAORC;
        //    PartialView.ViewBag.SenderId = SenderId;
        //    PartialView.ViewBag.TypeMsg = TypeMsg;
        //    PartialView.ViewBag.FileId = FileId;
        //    return PartialView;
        //}
        //public ActionResult GetStatusChatSH(string RequestId, string ShareholderId)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    string UserName = ""; string Pass = "";
        //    if (Session["Username"] != null || Session["Password"] != null)
        //    {
        //        UserName = Session["Username"].ToString();
        //        Pass = Session["Password"].ToString();
        //    }
        //    var q = servic1.GetStatusChat("Shareholders", Convert.ToInt32(ShareholderId), Convert.ToInt32(RequestId), 4, UserName, Pass, out Err1).ToList().Select(c => new { fldStatusId = c.Id, fldStatusName = c.fldName });
        //    if (Err1.ErrorType)
        //    {
        //        return null;
        //    }
        //    return this.Store(q);
        //}

        //public ActionResult NewCh_Shareholder(string Status, string ShareholderIdd, string StateAORC, string StatusRequest, string UserNameAdmin, string PassAdmin)
        //{
        //    int Permission = 0;
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //    }
        //    if (Status != "")
        //    {
        //        if (StatusRequest == "Completed")
        //        {
        //            if (!servic1.Permission(228, UserNameAdmin, PassAdmin, out Err1))
        //            {
        //                Permission = 1;
        //            }
        //        }
        //        if (Permission == 1)
        //        {
        //            X.Msg.Show(new MessageBoxConfig
        //            {
        //                Buttons = MessageBox.Button.OK,
        //                Icon = MessageBox.Icon.ERROR,
        //                Title = "خطا",
        //                Message = "شما مجاز به دسترسی نمی باشید."
        //            });
        //            DirectResult result = new DirectResult();
        //            return result;
        //        }
        //    }

        //    Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //    PartialView.ViewBag.ShareholderIdd = ShareholderIdd;
        //    PartialView.ViewBag.StateAORC = StateAORC;
        //    return PartialView;
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetOrgan()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblOrganSelect("", "", 0).ToList().Select(l => new { fldId = l.fldId, fldTitle = l.fldTitle });
            return this.Store(q);
        }
        public ActionResult CheckPersonalHaghighi(int PersonalId, int FirstId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "", MsgTitle = ""; var Er = 0;
            //var Shareholders = servic.GetShareholdersWithFilter("fldCompanyPersonalId", PersonalId.ToString(),Session["RankingId"].ToString(),"", 0, out Err).OrderByDescending(l => l.fldId).FirstOrDefault();

            var Shareholders = m.prs_tblShareholderSelect("fldCompanyPersonalId_Check", PersonalId.ToString(), ReqId.ToString(), "1", 0).OrderByDescending(l => l.fldId).FirstOrDefault();
            if (Shareholders != null)
            {
                if (Shareholders.fldFirstRegisterId == ReqId)
                {
                    Msg = "فرد مورد نظر قبلا ثبت شده است.";
                    MsgTitle = "خطا";
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SaveNewCh_Shareholders(prs_tblShareholderSelect Chat, string StateAORC)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    string Msg = "";
        //    string MsgTitle = ""; byte Er = 0; string fldMsg = "";
        //    var Date = servic.Getdate().fldTarikh;
        //    var Time = servic.Getdate().fldTime;
        //    byte[] report_file = null; string FileName = ""; string Pasvand = "";
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        if (Session["RankingId"] == null)
        //            return RedirectToAction("Index", "Admin");
        //        try
        //        {
        //            Msg = servic.InsertCh_Shareholders(false, null, Chat.fldMsg, Date, Time, 1, null, Chat.fldShareholderId, "", Convert.ToInt32(Session["RankingId"]), IP, null, null, out Err);
        //            MsgTitle = "ذخیره موفق";
        //            if (Err.ErrorType)
        //            {
        //                Er = 1;
        //                MsgTitle = "خطا";
        //                Msg = Err.ErrorMsg;
        //                return Json(new
        //                {
        //                    Msg = Msg,
        //                    MsgTitle = MsgTitle,
        //                    Er = Er
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (Exception x)
        //        {
        //            if (x.InnerException != null)
        //                Msg = x.InnerException.Message;
        //            else
        //                Msg = x.Message;

        //            MsgTitle = "خطا";
        //            Er = 1;
        //        }
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //        try
        //        {
        //            if (Session["savePathChat"] != null)
        //            {
        //                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathChat"].ToString()));
        //                report_file = stream.ToArray();
        //                FileName = Session["FileNameChat"].ToString();
        //                Pasvand = Path.GetExtension(Session["savePathChat"].ToString());
        //            }
        //            //else
        //            //{
        //            //    return Json(new
        //            //    {
        //            //        Msg = "لطفا ابتدا فایل را وارد نمایید.",
        //            //        MsgTitle = "خطا",
        //            //        Err = 1,
        //            //    }, JsonRequestBehavior.AllowGet);
        //            //}
        //            if (Chat.fldMsg != null)
        //            {
        //                fldMsg = Chat.fldMsg;
        //            }
        //            Msg = servic.InsertCh_Shareholders(false, Convert.ToInt32(Session["UserId"]), fldMsg, Date, Time, Chat.fldTypeMsg, null,
        //                Chat.fldShareholderId, "", Convert.ToInt32(StateAORC), IP, report_file, Pasvand, out Err);
        //            MsgTitle = "ذخیره موفق";
        //            if (Err.ErrorType)
        //            {
        //                Er = 1;
        //                MsgTitle = "خطا";
        //                Msg = Err.ErrorMsg;
        //                if (Session["savePathChat"] != null)
        //                {
        //                    string physicalPath = System.IO.Path.Combine(Session["savePathChat"].ToString());
        //                    Session.Remove("savePathChat");
        //                    Session.Remove("FileNameChat");
        //                    System.IO.File.Delete(physicalPath);
        //                }
        //                return Json(new
        //                {
        //                    Msg = Msg,
        //                    MsgTitle = MsgTitle,
        //                    Er = Er
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //            if (Session["savePathChat"] != null)
        //            {
        //                string physicalPath = System.IO.Path.Combine(Session["savePathChat"].ToString());
        //                Session.Remove("savePathChat");
        //                Session.Remove("FileNameChat");
        //                System.IO.File.Delete(physicalPath);
        //            }
        //        }
        //        catch (Exception x)
        //        {
        //            if (x.InnerException != null)
        //                Msg = x.InnerException.Message;
        //            else
        //                Msg = x.Message;

        //            MsgTitle = "خطا";
        //            Er = 1;
        //        }
        //    }
        //    return Json(new
        //    {
        //        Msg = Msg,
        //        MsgTitle = MsgTitle,
        //        Er = Er
        //    }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult CheckPersonalGheirKhososi(int PersonalId, int OrganId, int FirstId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            //var Shareholders = servic.GetShareholdersWithFilter("fldPersonalId", PersonalId.ToString(), Session["RankingId"].ToString(),"", 0, out Err).Where(l => l.fldOrganId == OrganId).FirstOrDefault();
            var Shareholders = m.prs_tblShareholderSelect("fldCompanyPersonalId_Check", PersonalId.ToString(), ReqId.ToString(), "3", 0).Where(l => l.fldOrganId == OrganId).OrderByDescending(l => l.fldId).FirstOrDefault();
            if (Shareholders != null)
            {
                if (Shareholders.fldFirstRegisterId == FirstId)
                {
                    Msg = "فرد مورد نظر قبلا ثبت شده است.";
                    MsgTitle = "خطا";
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
        public ActionResult CheckHoghoghiKhososi(int PersonalId, int CompanyId, int FirstId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            //var Shareholders = servic.GetShareholdersWithFilter("fldPersonalId", PersonalId.ToString(),Session["RankingId"].ToString(),"", 0, out Err).Where(l => l.fldCompanyId == CompanyId).FirstOrDefault();
            var Shareholders = m.prs_tblShareholderSelect("fldCompanyPersonalId_Check", PersonalId.ToString(), ReqId.ToString(), "2", 0).OrderByDescending(l => l.fldId).Where(l => l.fldCompanyProfileId == CompanyId).FirstOrDefault();
            if (Shareholders != null)
            {
                if (Shareholders.fldFirstRegisterId == FirstId)
                {
                    Msg = "فرد مورد نظر قبلا ثبت شده است.";
                    MsgTitle = "خطا";
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
        public ActionResult NewHoghoghiKhososi(int id, int FirstId, int ReqId)
        {//باز شدن پنجره
            //if (Session["Username"] == null)
            //    return RedirectToAction("Login", "Admin");
            //else
            //{
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            var q = m.prs_GetDate().FirstOrDefault();
            var a = q.fldDateTime.AddDays(-1).ToString("yyyy-MM-dd");
            a = a.Substring(0, 10);
            var saham = m.prs_MinusSaham(FirstId, ReqId).FirstOrDefault();

            PartialView.ViewBag.fldTarikhSa = a;
            PartialView.ViewBag.fldTarikh = q.fldTarikh;
            PartialView.ViewBag.Id = id;
            PartialView.ViewBag.TedadSaham2 = saham.TedadSaham;
            PartialView.ViewBag.SumSaham2 = saham.SumSaham;
            PartialView.ViewBag.Minus2 = saham.Minus;
            PartialView.ViewBag.FirstId = FirstId;
            PartialView.ViewBag.ReqId = ReqId;
            return PartialView;
            //}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveHaghighi(prs_tblShareholderSelect Haghighi, int ShareholderId, int FirstId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                var c = m.prs_tblCompanyProfileSelect("fldFirstRegisterId_FinalSabt", FirstId.ToString(), 0).FirstOrDefault();
               
                var Tarikh = MyLib.Shamsi.Shamsi2miladiDateTime(Haghighi.fldTarikhSaham);

                var DateNow = m.prs_GetDate().FirstOrDefault().fldDate;
                if (Tarikh > DateNow)
                {
                    MsgTitle = "اخطار";
                    Msg = "تاریخ مورد نظر نامعتبر است.";
                    Er = 1;
                }
                else if (MyLib.Shamsi.Shamsi2miladiDateTime(c.fldDateSabt) > MyLib.Shamsi.Shamsi2miladiDateTime(Haghighi.fldTarikhSaham))
                {
                    return Json(new
                    {
                        Msg = "تاریخ سهامدار شدن باید بعد از تاریخ ثبت شرکت یا برابر آن باشد.",
                        MsgTitle = "خطا",
                        Err = 1
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    /*var TreeS = "";
                    //درخواست پیمانکاری
                    if (Convert.ToInt32(Session["HadafId"]) == 30 || (Convert.ToInt32(Session["HadafId"]) >= 34 && Convert.ToInt32(Session["HadafId"]) <= 44))
                    {
                        TreeS = "31,46";//ناظر لوکو
                    }
                    //درخواست ناظر
                    if (Convert.ToInt32(Session["HadafId"]) == 31 || Convert.ToInt32(Session["HadafId"]) == 46)
                    {
                        TreeS = "30,33,34,35,36,37,38,39,40,41,42,43,44,45";//پیمانکارو تامین کننده و مالک
                    }
                    //درخواست تامین کننده
                    if (Convert.ToInt32(Session["HadafId"]) == 33)
                    {
                        TreeS = "31,46";//ناظر
                    }
                    //درخواست مالک
                    if (Convert.ToInt32(Session["HadafId"]) == 45)
                    {
                        TreeS = "31,46";//ناظر 
                    }
                    if (TreeS != "")
                    {
                        var eshterak = servic.CheckSahamdarMoshtarak(Convert.ToInt32(Haghighi.fldCompanyPersonalId), TreeS, out Err);
                        if (eshterak != "")
                            return Json(new
                            {
                                Msg = "سهامدار مشترک با " + eshterak,
                                MsgTitle = "خطا",
                                Err = 1
                            }, JsonRequestBehavior.AllowGet);
                    }*/

                    if (Haghighi.fldId == 0)
                    {
                        //ذخیره
                        //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 3))
                        //{
                        if (ShareholderId != 0)
                        {
                            MsgTitle = "ذخیره موفق";
                            var q = m.prs_tblShareholderSelect("fldId", ShareholderId.ToString(), ReqId.ToString(), "", 1).Where(l => l.fldId == ShareholderId && l.fldTedadSaham == Haghighi.fldTedadSaham && l.fldAshkhasId == Haghighi.fldAshkhasId && l.fldCompanyProfileId == Haghighi.fldCompanyProfileId && l.fldOrganId == Haghighi.fldOrganId && l.fldTarikhSaham == Haghighi.fldTarikhSaham).FirstOrDefault();
                            if (q != null)
                            {
                                m.prs_tblRequest_ShareholderInsert(q.fldId, ReqId);
                            }
                            else
                            {
                                m.prs_tblShareholderInsert(Haghighi.fldAshkhasId, null, null, Haghighi.fldTedadSaham, Convert.ToInt32(Haghighi.fldTarikhSaham.Replace("/", "")), ReqId, FirstId, u.UserInputId);
                            }
                        }
                        else
                        {
                            MsgTitle = "ذخیره موفق";
                            m.prs_tblShareholderInsert(Haghighi.fldAshkhasId, null, null, Haghighi.fldTedadSaham, Convert.ToInt32(Haghighi.fldTarikhSaham.Replace("/", "")), ReqId, FirstId, u.UserInputId);
                        }
                        Msg = "ذخیره با موفقیت انجام شد.";
                        //}
                        //else
                        //{
                        //    return null;
                        //}
                    }
                    else
                    {
                        //ویرایش
                        //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 4))
                        //{
                        MsgTitle = "ویرایش موفق";
                        m.prs_tblShareholderUpdate(Haghighi.fldId, Haghighi.fldAshkhasId, null, null, Haghighi.fldTedadSaham, Convert.ToInt32(Haghighi.fldTarikhSaham.Replace("/", "")), FirstId, u.UserInputId);
                        Msg = "ویرایش با موفقیت انجام شد.";
                        //}
                        //else
                        //{
                        //    return null;
                        //}
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
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveGieyrKhososi(prs_tblShareholderSelect GieyrKhososi, int ShareholderId, int FirstId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                var c = m.prs_tblCompanyProfileSelect("fldFirstRegisterId_FinalSabt", FirstId.ToString(), 0).FirstOrDefault();
               
                var Tarikh = MyLib.Shamsi.Shamsi2miladiDateTime(GieyrKhososi.fldTarikhSaham);

                var DateNow = m.prs_GetDate().FirstOrDefault().fldDate;
                if (Tarikh > DateNow)
                {
                    MsgTitle = "اخطار";
                    Msg = "تاریخ مورد نظر نامعتبر است.";
                    Er = 1;
                }
                else if (MyLib.Shamsi.Shamsi2miladiDateTime(c.fldDateSabt)> MyLib.Shamsi.Shamsi2miladiDateTime(GieyrKhososi.fldTarikhSaham))
                {
                    return Json(new
                    {
                        Msg = "تاریخ سهامدار شدن باید بعد از تاریخ ثبت شرکت یا برابر آن باشد.",
                        MsgTitle = "خطا",
                        Err = 1
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    /*var TreeS = "";
                    //درخواست پیمانکاری
                    if (Convert.ToInt32(Session["HadafId"]) == 30 || (Convert.ToInt32(Session["HadafId"]) >= 34 && Convert.ToInt32(Session["HadafId"]) <= 44))
                    {
                        TreeS = "31,46";//ناظر لوکو
                    }
                    //درخواست ناظر
                    if (Convert.ToInt32(Session["HadafId"]) == 31 || Convert.ToInt32(Session["HadafId"]) == 46)
                    {
                        TreeS = "30,33,34,35,36,37,38,39,40,41,42,43,44,45";//پیمانکارو تامین کننده و مالک
                    }
                    //درخواست تامین کننده
                    if (Convert.ToInt32(Session["HadafId"]) == 33)
                    {
                        TreeS = "31,46";//ناظر
                    }
                    //درخواست مالک
                    if (Convert.ToInt32(Session["HadafId"]) == 45)
                    {
                        TreeS = "31,46";//ناظر 
                    }
                    if (TreeS != "")
                    {
                        var eshterak = servic.CheckSahamdarMoshtarak(Convert.ToInt32(GieyrKhososi.fldCompanyPersonalId), TreeS, out Err);
                        if (eshterak != "")
                            return Json(new
                            {
                                Msg = "سهامدار مشترک با " + eshterak,
                                MsgTitle = "خطا",
                                Err = 1
                            }, JsonRequestBehavior.AllowGet);
                    }*/
                    if (GieyrKhososi.fldId == 0)
                    {
                        //ذخیره
                        //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 3))
                        //{
                        if (ShareholderId != 0)
                        {
                            MsgTitle = "ذخیره موفق";
                            var q = m.prs_tblShareholderSelect("fldId", ShareholderId.ToString(), ReqId.ToString(), "", 1).Where(l => l.fldId == ShareholderId && l.fldTedadSaham == GieyrKhososi.fldTedadSaham && l.fldAshkhasId == GieyrKhososi.fldAshkhasId && l.fldCompanyProfileId == GieyrKhososi.fldCompanyProfileId && l.fldOrganId == GieyrKhososi.fldOrganId && l.fldTarikhSaham == GieyrKhososi.fldTarikhSaham).FirstOrDefault();
                            if (q != null)
                            {
                                m.prs_tblRequest_ShareholderInsert(q.fldId, ReqId);
                            }
                            else
                            {
                                MsgTitle = "ذخیره موفق";
                                m.prs_tblShareholderInsert(GieyrKhososi.fldAshkhasId, null, GieyrKhososi.fldOrganId, GieyrKhososi.fldTedadSaham, Convert.ToInt32(GieyrKhososi.fldTarikhSaham.Replace("/", "")), ReqId,FirstId, u.UserInputId);
                            }
                        }
                        else
                        {
                            MsgTitle = "ذخیره موفق";
                            m.prs_tblShareholderInsert(GieyrKhososi.fldAshkhasId, null, GieyrKhososi.fldOrganId, GieyrKhososi.fldTedadSaham, Convert.ToInt32(GieyrKhososi.fldTarikhSaham.Replace("/", "")), ReqId, FirstId, u.UserInputId);
                        }
                        Msg = "ذخیره با موفقیت انجام شد.";
                        //}
                        //else
                        //{
                        //    return null;
                        //}
                    }
                    else
                    {
                        //ویرایش
                        //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 4))
                        //{
                        MsgTitle = "ویرایش موفق";
                        m.prs_tblShareholderUpdate(GieyrKhososi.fldId, GieyrKhososi.fldAshkhasId, null, GieyrKhososi.fldOrganId, GieyrKhososi.fldTedadSaham, Convert.ToInt32(GieyrKhososi.fldTarikhSaham.Replace("/", "")), FirstId, u.UserInputId);
                        Msg = "ویرایش با موفقیت انجام شد.";
                        //}
                        //else
                        //{
                        //    return null;
                        //}
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
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveHoghoghiKhososi(prs_tblShareholderSelect Khososi, int ShareholderId, int FirstId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                var c = m.prs_tblCompanyProfileSelect("fldFirstRegisterId_FinalSabt", FirstId.ToString(), 0).FirstOrDefault();
               
                var Tarikh = MyLib.Shamsi.Shamsi2miladiDateTime(Khososi.fldTarikhSaham);

                var DateNow = m.prs_GetDate().FirstOrDefault().fldDate;
                if (Tarikh > DateNow)
                {
                    MsgTitle = "اخطار";
                    Msg = "تاریخ مورد نظر نامعتبر است.";
                    Er = 1;
                }
                else if (MyLib.Shamsi.Shamsi2miladiDateTime(c.fldDateSabt) > MyLib.Shamsi.Shamsi2miladiDateTime(Khososi.fldTarikhSaham))
                {
                    return Json(new
                    {
                        Msg = "تاریخ سهامدار شدن باید بعد از تاریخ ثبت شرکت یا برابر آن باشد.",
                        MsgTitle = "خطا",
                        Err = 1
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //var TreeS = "";
                    ////درخواست پیمانکاری
                    //if (Convert.ToInt32(Session["HadafId"]) == 30 || (Convert.ToInt32(Session["HadafId"]) >= 34 && Convert.ToInt32(Session["HadafId"]) <= 44))
                    //{
                    //    TreeS = "31,46";//ناظر لوکو
                    //}
                    ////درخواست ناظر
                    //if (Convert.ToInt32(Session["HadafId"]) == 31 || Convert.ToInt32(Session["HadafId"]) == 46)
                    //{
                    //    TreeS = "30,33,34,35,36,37,38,39,40,41,42,43,44,45";//پیمانکارو تامین کننده و مالک
                    //}
                    ////درخواست تامین کننده
                    //if (Convert.ToInt32(Session["HadafId"]) == 33)
                    //{
                    //    TreeS = "31,46";//ناظر
                    //}
                    ////درخواست مالک
                    //if (Convert.ToInt32(Session["HadafId"]) == 45)
                    //{
                    //    TreeS = "31,46";//ناظر 
                    //}
                    //if (TreeS != "")
                    //{
                    //    var eshterak = servic.CheckSahamdarMoshtarak(Convert.ToInt32(Khososi.fldCompanyPersonalId), TreeS, out Err);
                    //    if (eshterak != "")
                    //        return Json(new
                    //        {
                    //            Msg = "سهامدار مشترک با " + eshterak,
                    //            MsgTitle = "خطا",
                    //            Err = 1
                    //        }, JsonRequestBehavior.AllowGet);
                    //}
                    if (Khososi.fldId == 0)
                    {
                        //ذخیره
                        //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 3))
                        //{
                        if (ShareholderId != 0)
                        {
                            MsgTitle = "ذخیره موفق";
                            var q = m.prs_tblShareholderSelect("fldId", ShareholderId.ToString(), ReqId.ToString(), "", 1).Where(l => l.fldId == ShareholderId && l.fldTedadSaham == Khososi.fldTedadSaham && l.fldAshkhasId == Khososi.fldAshkhasId && l.fldCompanyProfileId == Khososi.fldCompanyProfileId && l.fldOrganId == Khososi.fldOrganId && l.fldTarikhSaham == Khososi.fldTarikhSaham).FirstOrDefault();
                            if (q != null)
                            {
                               m.prs_tblRequest_ShareholderInsert(q.fldId, ReqId);
                            }
                            else
                            {
                                MsgTitle = "ذخیره موفق";
                                m.prs_tblShareholderInsert(Khososi.fldAshkhasId, Khososi.fldCompanyProfileId, null, Khososi.fldTedadSaham, Convert.ToInt32(Khososi.fldTarikhSaham.Replace("/", "")), ReqId, FirstId, u.UserInputId);
                            }
                        }
                        else
                        {
                            MsgTitle = "ذخیره موفق";
                            m.prs_tblShareholderInsert(Khososi.fldAshkhasId, Khososi.fldCompanyProfileId, null, Khososi.fldTedadSaham, Convert.ToInt32(Khososi.fldTarikhSaham.Replace("/", "")), ReqId, FirstId, u.UserInputId);
                        }
                        Msg = "ذخیره با موفقیت انجام شد.";
                        //}
                        //else
                        //{
                        //    return null;
                        //}
                    }
                    else
                    {
                        //ویرایش
                        //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 4))
                        //{
                        MsgTitle = "ویرایش موفق";
                        m.prs_tblShareholderUpdate(Khososi.fldId, Khososi.fldAshkhasId, Khososi.fldCompanyProfileId, null, Khososi.fldTedadSaham, Convert.ToInt32(Khososi.fldTarikhSaham.Replace("/", "")), FirstId, u.UserInputId);
                       Msg = "ویرایش با موفقیت انجام شد.";
                        //}
                        //else
                        //{
                        //    return null;
                        //}
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
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                //حذف

                //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 5))
                //{

                MsgTitle = "حذف موفق";
                m.prs_tblShareholderDelete(id, ReqId);
                Msg = "حذف با موفقیت انجام شد.";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblShareholderSelect("fldId",Id.ToString(),"","",0).FirstOrDefault();
            return Json(new
            {
                fldId = q.fldId,
                fldCodeMeli = q.fldCodeMeli,
                fldCompanyId = q.fldCompanyProfileId,
                fldCompanyPersonalId = q.fldAshkhasId,
                fldName = q.fldName,
                fldOrgan_Company = q.fldOrgan_Company,
                fldOrganId = q.fldOrganId.ToString(),
                //fldStatus = q.fldStatus,
                fldTarikhSaham = q.fldTarikhSaham,
                fldTedadSaham = q.fldTedadSaham,
                fldTypeName = q.fldTypeName,
                ShenaseMeli = q.ShenaseMeli,
                ShomareSabat = q.ShomareSabat,
              //  fldDesc = q.fldDesc,
                fldNamePersonal = q.fldNamePersonal,
                fldFamilyPersonal = q.fldFamilyPersonal

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult DetailsCompanyPerson(int Id, string Type, int _Id, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            Models.prs_tblShareholderSelect q = new Models.prs_tblShareholderSelect();
            if (Type == "1")
                q = m.prs_tblShareholderSelect("fldCompanyPersonalId_Shareholders", Id.ToString(), ReqId.ToString(), Type, 0).FirstOrDefault();

            else if (Type == "2")
                q = m.prs_tblShareholderSelect("fldCompanyPersonalId_Shareholders", Id.ToString(), ReqId.ToString(), Type, 0).Where(l => l.fldCompanyProfileId == _Id).FirstOrDefault();
            else if (Type == "3")
                q = m.prs_tblShareholderSelect("fldCompanyPersonalId_Shareholders", Id.ToString(), ReqId.ToString(), Type, 0).Where(l => l.fldOrganId == _Id).FirstOrDefault();

            if (q != null)
            {
                return Json(new
                {
                    fldId = q.fldId,
                    fldCodeMeli = q.fldCodeMeli,
                    fldCompanyId = q.fldCompanyProfileId,
                    fldCompanyPersonalId = q.fldAshkhasId,
                    fldName = q.fldName,
                    fldOrgan_Company = q.fldOrgan_Company,
                    fldOrganId = q.fldOrganId.ToString(),
                    //fldStatus = q.fldStatus,
                    fldTarikhSaham = q.fldTarikhSaham,
                    fldTedadSaham = q.fldTedadSaham,
                    fldTypeName = q.fldTypeName,
                    ShenaseMeli = q.ShenaseMeli,
                    ShomareSabat = q.ShomareSabat,
                    //fldDesc = q.fldDesc,
                    fldNamePersonal = q.fldNamePersonal,
                    fldFamilyPersonal = q.fldFamilyPersonal
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    fldId = 0

                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters, int ReqId, int FirstId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            RaiSamEntities m = new RaiSamEntities();
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblShareholderSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<prs_tblShareholderSelect> data1 = null;
                foreach (var item in filterHeaders.Conditions)
                {
                    var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                    switch (item.FilterProperty.Name)
                    {
                        case "fldId":
                            searchtext = ConditionValue.Value.ToString();
                            field = "fldId_Request";
                            break;
                        case "fldName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldName_Request";
                            break;
                        case "fldCodeMeli":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldCodeMeli_Request";
                            break;
                        case "fldTypeName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldTypeName";
                            break;
                        case "fldTarikhSaham":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldTarikhSaham_Request";
                            break;

                    }
                    if (data != null)
                        data1 = m.prs_tblShareholderSelect(field, searchtext, ReqId.ToString(), "", 0).ToList();
                    else
                        data = m.prs_tblShareholderSelect(field, searchtext, ReqId.ToString(), "", 0).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {
                data = m.prs_tblShareholderSelect("fldFirstRegisterUser", FirstId.ToString(), ReqId.ToString(), "", 0).ToList();
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
            //int limit = parameters.Limit;

            //if ((parameters.Start + parameters.Limit) > data.Count)
            //{
            //    limit = data.Count - parameters.Start;
            //}

            //List<RaiTrainClientWS.OBJ_Shareholders> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(data, data.Count);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ReadCh_Shareholder(string ShareholderId, string StateAORC)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    List<prs_tblShareholderSelect> data = null;
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        if (Session["RankingId"] == null)
        //            return RedirectToAction("Index", "Admin");
        //        data = servic.GetCh_ShareholdersWithFilter("fldShareholderId", ShareholderId, Session["RankingId"].ToString(), 0, out Err).ToList();
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //        data = servic.GetCh_ShareholdersWithFilter("fldShareholderId", ShareholderId, StateAORC, 0, out Err).ToList();
        //    }
        //    return this.Store(data);
        //}
        //public ActionResult UploadChat()
        //{
        //    string Msg = "";
        //    try
        //    {
        //        if (Session["savePathChat"] != null)
        //        {
        //            string physicalPath = System.IO.Path.Combine(Session["savePathChat"].ToString());
        //            Session.Remove("savePathChat");
        //            Session.Remove("FileNameChat");
        //            System.IO.File.Delete(physicalPath);
        //        }

        //        //if (Request.Files[0].ContentType == "application/pdf")
        //        var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
        //        if (IsPDF == true)
        //        {
        //            if (Request.Files[0].ContentLength <= 10240000)
        //            {
        //                HttpPostedFileBase file = Request.Files[0];
        //                string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
        //                file.SaveAs(savePath);
        //                Session["FileNameChat"] = file.FileName;
        //                Session["savePathChat"] = savePath;
        //                object r = new
        //                {
        //                    success = true,
        //                    name = Request.Files[0].FileName
        //                };
        //                return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
        //            }
        //            else
        //            {
        //                X.Msg.Show(new MessageBoxConfig
        //                {
        //                    Buttons = MessageBox.Button.OK,
        //                    Icon = MessageBox.Icon.ERROR,
        //                    Title = "خطا",
        //                    Message = "حجم فایل انتخابی می بایست کمتر از 10 مگابایت باشد."
        //                });
        //                DirectResult result = new DirectResult();
        //                return result;
        //            }
        //        }
        //        else
        //        {
        //            X.Msg.Show(new MessageBoxConfig
        //            {
        //                Buttons = MessageBox.Button.OK,
        //                Icon = MessageBox.Icon.ERROR,
        //                Title = "خطا",
        //                Message = "فایل انتخاب شده غیر مجاز است."
        //            });
        //            DirectResult result = new DirectResult();
        //            return result;
        //        }
        //    }
        //    catch (Exception x)
        //    {
        //        if (x.InnerException != null)
        //            Msg = x.InnerException.Message;
        //        else
        //            Msg = x.Message;

        //        X.Msg.Show(new MessageBoxConfig
        //        {
        //            Buttons = MessageBox.Button.OK,
        //            Icon = MessageBox.Icon.ERROR,
        //            Title = "خطا",
        //            Message = Msg
        //        });
        //        DirectResult result = new DirectResult();
        //        return result;
        //    }

        //}

    }
}
