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
using FastReport;

namespace RaiSam.Areas.Faces.Controllers
{
    public class ScheduleContractController : Controller
    {
        //
        // GET: /Faces/ScheduleContract/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        public ActionResult Index(string EnterSicleIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            //var SentToAdmin = 0;
            //var k = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
            //if (k.fldStatusId == 2 || k.fldStatusId == 5)
            //    SentToAdmin = 1;
            //PartialView.ViewBag.SentToAdmin = SentToAdmin;
            var IsInClient = 0;
            int req = 0;
           
            
            if (EnterSicleIds!=null)
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
            PartialView.ViewBag.IsInClient = IsInClient;
            PartialView.ViewBag.ReqId = req;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetProject(int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblRegistrationFirstContractSelect("fldRequestId", ReqId.ToString(), 0).Select(c => new { Id = c.fldId, Name = c.fldTitle });
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReloadDetail(string Proj)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            List<prs_SelectScheduleTitleByFirstContract> data = null;
            data = m.prs_SelectScheduleTitleByFirstContract(Convert.ToInt32(Proj)).ToList();
            return Json(data.Select(c => new { fldScheduleContractId = c.fldScheduleContractId, fldScheduleTitleId = c.fldScheduleTitleId, fldTitleSchedule = c.fldTitleSchedule, fldAzTarikh = c.fldAzTarikh, fldTaTarikh = c.fldTaTarikh }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save(List<prs_tblScheduleContractSelect> ItemDetail,int FirstContractId)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            RaiSamEntities m = new RaiSamEntities();
            try
            {
                var h = m.prs_tblScheduleContract_HeaderInsert(FirstContractId, u.UserInputId).FirstOrDefault();
                foreach (var item in ItemDetail)
                {
                    int? azTarikh = null;
                    if (item.fldAzTarikh != null)
                        azTarikh=Convert.ToInt32(item.fldAzTarikh.Replace("/", ""));
                    int? taTarikh = null;
                    if (item.fldTaTarikh != null)
                        taTarikh = Convert.ToInt32(item.fldTaTarikh.Replace("/", ""));

                    if (item.fldId == 0)
                    {
                            MsgTitle = "ذخیره موفق";
                            Msg = "ذخیره با موفقیت انجام شد.";

                            m.prs_tblScheduleContractInsert(h.fldId, item.fldScheduleTitleId, azTarikh, taTarikh, u.UserInputId);
}
                    else
                    {
                            MsgTitle = "ویرایش موفق";
                            Msg = "ویرایش با موفقیت انجام شد.";
                            m.prs_tblScheduleContractUpdate(item.fldId, h.fldId, item.fldScheduleTitleId, azTarikh, taTarikh, u.UserInputId);
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
        public ActionResult PrintSchedule(int Id, int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            PartialView.ViewBag.state = state;

            return PartialView;
        }
        public ActionResult GenerateRptSchedule(int Id, int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var uu = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();

                Report rep = new Report();
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\zamanbandigrafic.frx";
                if (state == 2)
                    path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\zamnbandy.frx";
                rep.Load(path);
                rep.SetParameterValue("Idcontract", Id);
                rep.SetParameterValue("UserName", uu.fldNamePersonal);
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
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }
        public FileResult CreateExcel(int Id)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var uu = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();

            Report rep = new Report();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\zamanbandigrafic.frx";
            rep.Load(path);
            rep.SetParameterValue("Idcontract", Id);
            rep.SetParameterValue("UserName", uu.fldNamePersonal);
            rep.SetParameterValue("ConectionStr", System.Configuration.ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString);

            if (rep.Report.Prepare())
            {
                // Set PDF export props
                FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();
                xlsExport.ShowProgress = false;


                MemoryStream strm = new MemoryStream();
                rep.Report.Export(xlsExport, strm);
                rep.Dispose();
                xlsExport.Dispose();
                strm.Position = 0;

                // return stream in browser
                return File(strm.ToArray(), "xls", "Schedule.xls");
            }
            else
            {
                return null;
            }
        }
    }
}
