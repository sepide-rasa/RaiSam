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

namespace RaiSam.Controllers.BasicInfo
{
    public class ScheduleTitle_HadafController : Controller
    {
        //
        // GET: /ScheduleTitle_Hadaf/
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (servic.Permission(44, Session["Username"].ToString(), Session["Password"].ToString(), out Err))
            //{
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;
        }


        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                return PartialView;
            }
        }
        public ActionResult GetHadafSabtNam()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblHadafSabtNamSelect("", "", 0).Select(c => new { HadafSabtNamID2 = c.fldId, HadafSabtNamName2 = c.fldTitle });
            return this.Store(q);
        }
        public ActionResult LoadAllData(string HadafId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var data = m.prs_tblScheduleTitleSelect("", "", 0).ToList();
            var check = m.prs_tblScheduleTitle_HadafSelect("fldHadafId", HadafId, 0).ToList();
                int[] checkId = new int[check.Count];

                for (int i = 0; i < check.Count; i++)
                {
                    checkId[i] = check[i].fldHadafId;
                }
                return Json(new
                {
                    data = data,
                    checkId2 = checkId
                }, JsonRequestBehavior.AllowGet);
         
        }

        public ActionResult GetChecked(string HadafId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var check = m.prs_tblScheduleTitle_HadafSelect("fldHadafId", HadafId, 0).ToList();
            int[] checksId2 = new int[check.Count];

            for (int i = 0; i < check.Count; i++)
            {
                checksId2[i] = check[i].fldScheduleTitleId;
            }
            return Json(checksId2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(string ScheduleTitleIds, string HadafId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; int Er = 0; string ExistId = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Permission.haveAccess(239, "tblScheduleTitle_Hadaf", null))
                {
                    /*  var Exist = servic.GetStakeholderSecretariatWithFilter("fldGroup_DabieKhane", SecretariatIds, StackholderGroupsId, 0, Session["UserName"].ToString(), Session["Password"].ToString(), out Err).ToList();
                      if (Exist.Count() == 0)
                      {*/
                    var ScheduleTitleIdSplit = ScheduleTitleIds.Split(',').Reverse().Skip(1).Reverse().ToList();
                    var SaveData = m.prs_tblScheduleTitle_HadafSelect("fldHadafId", HadafId, 0).ToList();
                    if (SaveData.Count == 0)
                    {
                        for (int i = 0; i < ScheduleTitleIdSplit.Count(); i++)
                        {
                             m.prs_tblScheduleTitle_HadafInsert(Convert.ToInt32(ScheduleTitleIdSplit[i]), Convert.ToInt32(HadafId),u.UserInputId ,"");
                            MsgTitle = "ذخیره موفق";
                            Msg = "ذخیره با موفقیت انجام شد.";
                        }
                    }
                    else
                    {
                        string[] SaveDataId = new string[SaveData.Count];
                        for (int i = 0; i < SaveData.Count; i++)
                        {
                            SaveDataId[i] = SaveData[i].fldScheduleTitleId.ToString();
                        }
                        var DeleteRow = SaveDataId.Except(ScheduleTitleIdSplit).ToArray();
                        var InsertRow = ScheduleTitleIdSplit.Except(SaveDataId).ToArray();

                        if (DeleteRow.Length != 0)
                        {
                            string ScheduleTitleIdds = "";
                            for (int i = 0; i < DeleteRow.Length; i++)
                            {
                                ScheduleTitleIdds = ScheduleTitleIdds + DeleteRow[i] + ",";
                            }
                            m.prs_tblScheduleTitle_HadafDelete(Convert.ToInt32(HadafId), "ScheduleTitleId", ScheduleTitleIdds);
                            MsgTitle = "ویرایش موفق";
                            Msg = "ویرایش با موفقیت انجام شد.";
                        }
                        if (InsertRow.Length != 0)
                        {
                            for (int i = 0; i < InsertRow.Count(); i++)
                            {
                                 m.prs_tblScheduleTitle_HadafInsert(Convert.ToInt32(InsertRow[i]), Convert.ToInt32(HadafId), u.UserInputId,"");
                                MsgTitle = "ذخیره موفق";
                                Msg = "ذخیره با موفقیت انجام شد.";
                            }
                        }
                    }
                    /* }
                     else
                     {
                         foreach (var item in Exist)
                         {
                             ExistId = ExistId + item.fldDabirKhaneId + "،";
                         }
                         return Json(new
                         {
                             Err = 1,
                             ExistId2 = ExistId,
                             Msg = "آیتم(های)" + ExistId + " برای گروه(های) دیگری انتخاب شده اند. لطفاً دوباره سعی نمایید.",
                             MsgTitle = "خطا"
                         }, JsonRequestBehavior.AllowGet);
                     }*/
                 
                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "شما مجاز به ذخیره اطلاعات نمی باشد.";
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
                Err = Er,
                Msg = Msg,
                ExistId2 = ExistId,
                MsgTitle = MsgTitle
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
