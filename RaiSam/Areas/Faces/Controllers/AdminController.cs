using Ext.Net;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RaiSam.Models;

namespace RaiSam.Areas.Faces.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Faces/Admin/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(int? CompanyProfileId, int? state)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            if (state != null || (Session["FristRegisterId"] != null && Session["SFToken"] != null && Request.Cookies["SFToken"] != null))
            {
                if (state == null && !Session["SFToken"].ToString().Equals(Request.Cookies["SFToken"].Value))
                {
                    return RedirectToAction("LogOn", "Account", new { area = "" });
                }
                else
                {
                    if (CompanyProfileId != null && state != null)
                    {
                        //if (state == 1)
                        //{
                            var FirstRegisterId = m.prs_tblCompanyProfileSelect("fldId",CompanyProfileId.ToString(), 1).FirstOrDefault().fldFirstRegisterUser;
                            var q = m.prs_tblFirstRegisterSelect("fldId", FirstRegisterId.ToString(), "", 1).FirstOrDefault();
                            var q2 = m.prs_tblUserSelect("fldFirstRigesterId", FirstRegisterId.ToString(), "", 1).FirstOrDefault();
                            Session["Pass"] = q.fldPassword;
                            Session["User"] = q.fldUserName;
                            Session["FristRegisterId"] = FirstRegisterId;

                            UserInfo user = (UserInfo)(Session["User"]);
                            string Key = Guid.NewGuid().ToString();

                            user.UserSecondId = q2.fldId;
                            user.Pass = q.fldPassword;
                            user.UserKey = Key.ToString();
                            user.UserName = q.fldUserName;
                            user.UserType = 2;
                            user.FirstRegisterId = FirstRegisterId;

                            ViewBag.FirstLogin = q.fldFirstLogin;

                            
                        //}
                        //else
                        //{
                        //    var FirstRegisterId = m.prs_tblRealPersonLocationDetail("fldId", CompanyProfileId.ToString(), 1).fldFirstRegisterUser;
                        //    var q = m.prs_tblFirstRegisterSelect("fldId", FirstRegisterId.ToString(),  1).FirstOrDefault();
                        //    Session["Pass"] = q.fldPassword;
                        //    Session["User"] = q.fldUserName;
                        //    Session["FristRegisterId"] = FirstRegisterId;
                        //    ViewBag.FirstLogin = q.fldFirstLogin;
                        //}
                    }
                    else
                    {
                        var q = m.prs_tblFirstRegisterSelect("fldId", Session["FristRegisterId"].ToString(), "", 1).FirstOrDefault();
                        ViewBag.FirstLogin = q.fldFirstLogin;
                    }

                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogOn", "Account", new { area = "" });
            }
        }
        public ActionResult ChangePassword()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            var userInf = (UserInfo)(Session["User"]);
            PartialView.ViewBag.userNamee = userInf.UserName.ToString();

            var user = m.prs_tblUserSelect("fldId", userInf.UserSecondId.ToString(), "", 1).FirstOrDefault();
            PartialView.ViewBag.fldFirstLogin = user.fldFirstLogin;
            return PartialView;
        }
        public ActionResult SaveChangePass(string fldPass, string fldNewPass)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            var u = (UserInfo)(Session["User"]);
            try
            {
                if (Session["FristRegisterId"] == null)
                    return RedirectToAction("LogOn", "Account", new { area = "" });

                var user = m.prs_tblUserSelect("checkPass", u.UserName, CodeDecode.ComputeSha256Hash(fldPass), 1);

                if (user.FirstOrDefault() == null)
                {
                    return Json(new
                    {
                        Er = 1,
                        MsgTitle = "خطا",
                        Msg = "رمز عبور قبلی وارد شده اشتباه است."
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    m.prs_UserFirsLoginUpdate(u.UserSecondId, false);

                    m.prs_UserPassUpdate(u.UserSecondId, CodeDecode.ComputeSha256Hash(fldNewPass), u.UserSecondId);

                    MsgTitle = "عملیات موفق";
                    Msg = "ذخیره با موفقیت انجام شد.";
                }
            }
            catch (Exception x)
            {
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;

                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();


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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("LogOn", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var u = (UserInfo)(Session["User"]);
            FormsAuthentication.SignOut();
            var date = m.prs_GetDate().FirstOrDefault().fldDateTime;

            string Key = Guid.NewGuid().ToString();
            System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
            m.prs_tblInputInfoInsert(Id, IP, "", 1, u.UserSecondId, "", Request.Browser.Browser + " Version:" + Request.Browser.Version, Key, 1, u.UserSecondId);
            RaiSam.Models.UserLoginCount.RemoveOnlineUser(u.UserSecondId.ToString());
            Session.RemoveAll();
            return RedirectToAction("LogOn", "Account", new { area = "" });
        }
        public ActionResult GetAnnoncement(byte type)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo u = (UserInfo)Session["User"];
            Models.RaiSamEntities m = new RaiSamEntities();
            var EtelaieTitleAll = "";
            var EtelaieIDAll = "";
            var FieldName = "";
            if (type == 0)
            {
                FieldName = "fldCheckTarikhShamsi_Type_All_Hadaf";
            }
            else
            {
                FieldName = "fldCheckTarikhShamsi_Type_NotSeen_Hadaf";
            }

            var Etelaie = m.prs_tblAnnouncementManagerSelect(FieldName, "1", u.UserInputId, 0, Session["HadafId"].ToString()).ToList();
            foreach (var item in Etelaie)
            {
                EtelaieTitleAll = EtelaieTitleAll + item.fldTitle + ";";
                EtelaieIDAll = EtelaieIDAll + item.fldID + ";";
            }
            return Json(new { EtelaieTitleAll = EtelaieTitleAll, EtelaieIDAll = EtelaieIDAll }, JsonRequestBehavior.AllowGet);
        }

    }
}
