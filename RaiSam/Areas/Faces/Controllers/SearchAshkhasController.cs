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
    public class SearchAshkhasController : Controller
    {
        //
        // GET: /Faces/SearchAshkhas/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult SearchAll(int State, string CompanyId, string rowIdx, string FirstId, string ReqId)
        {//باز شدن تب جدید
            //if (Session["Username"] == null)
            //    return RedirectToAction("Account", "logon");
            //if (Permossions.haveAccess(Convert.ToInt32(Session["UserId"]), 17))
            //{
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            RaiSamEntities m = new RaiSamEntities();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            var q = m.prs_GetDate().FirstOrDefault();
            var a = q.fldDateTime.AddDays(-1).ToString("yyyy-MM-dd");
            a = a.Substring(0, 10);
            PartialView.ViewBag.fldDateB = a;
            PartialView.ViewBag.fldTarikh = q.fldTarikh;
            PartialView.ViewBag.State = State;
            PartialView.ViewBag.CompanyId = CompanyId;
            PartialView.ViewBag.rowIdx = rowIdx;
            PartialView.ViewBag.ReqId = ReqId;
            PartialView.ViewBag.FirstId = FirstId;
            return PartialView;
            //}
            //else
            //{
            //    return null;
            //}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string CodeMeli, string ShenasnameNo, string BDate)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            RaiSamEntities m = new RaiSamEntities();
            var fldId = 0;
            string fldFamily = "";
            string fldName = "";
            string fldCodeMeli = "";
            string fldMobile = "";
            int haveMeliCode = 0;
            var q = m.prs_tblAshkhasSelect("CodeMeli_ShSh",CodeMeli,"",  ShenasnameNo,BDate, 0).FirstOrDefault();
            if (q != null)
            {
                fldId = q.fldId;
                fldFamily = q.fldFamily;
                fldName = q.fldName;
                fldCodeMeli = q.fldCodeMeli;
                fldMobile = q.fldMobile;
            }
            else
            {
                var q2 = m.prs_tblAshkhasSelect("fldCodeMeli", CodeMeli, "", "", "", 0).FirstOrDefault();
                haveMeliCode = 1;
            }
            return Json(new
            {
                fldId = fldId,
                fldFamily = fldFamily,
                fldName = fldName,
                fldCodeMeli = fldCodeMeli,
                haveMeliCode = haveMeliCode,
                fldMobile = fldMobile
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SabtAshkhasNew(string CodeMeli, string BDate, int State)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.CodeMeli = CodeMeli;
            result.ViewBag.BDate = BDate;
            result.ViewBag.State = State;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAshkhas(Models.prs_tblAshkhasSelect Human)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            string Msg = "", MsgTitle = ""; var Er = 0;
            int? AshkhasId = null;

            try
            {
                int? birthDay = null;
                if (Human.fldTarikhTavalod != null)
                {
                    birthDay = Convert.ToInt32(Human.fldTarikhTavalod.Replace("/", ""));
                }
                if (birthDay > 13671230)
                    Human.fldSh_Shenasname = Human.fldCodeMeli;

               var ss= m.prs_InsertAshkhas(Human.fldName, Human.fldFamily, Human.fldFatherName, Human.fldCodeMeli, birthDay,
                                Human.fldSh_Shenasname, Human.fldJensiyat).FirstOrDefault();

              
                if (ss.ErrorCode == 0)
                {
                    AshkhasId = ss.fldId;
                    Msg = "ذخیره با موفقیت انجام شد.";
                    MsgTitle = "دخیره موفق";
                }
                else if (ss.ErrorCode == -1)
                {
                    Msg = "شخص با این کد ملی قبلا در سیستم، استعلام و ثبت شده است.";
                    MsgTitle = "خطا";
                    Er = 1;
                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "خطایی با شماره: " + ss.ErrorCode + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.";
                    Er = 1;
                }
                  
            }
            catch (Exception x)
            {
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
              

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
                AshkhasId = AshkhasId
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
