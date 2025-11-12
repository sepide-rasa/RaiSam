using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.IO;
using Hogaf;
using Hogaf.ExtNet.UX;
using Hogaf.ExtNet;
using System.Web.Configuration;
using RaiSam.Models;

namespace RaiSam.Areas.Faces.Controllers
{
    public class RequestRankingController : Controller
    {
        //
        // GET: /Faces/RequestRanking/
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            var n = m.prs_tblFirstRegisterSelect("fldId", Session["FristRegisterId"].ToString(), "", 0).FirstOrDefault();
            var personType = 0;
          /*  if (n.fldPersonalType == true)
                personType = 1;*/
            PartialView.ViewBag.personType = personType;
            //var haveMomayezi = servic1.GetMomayezi_HeaderWithFilter("FirstRegisterId", Session["FristRegisterId"].ToString(), 0, out Err1).Where(l => l.fldSeen == false).Any();
            //PartialView.ViewBag.haveMomayezi = haveMomayezi;
            return PartialView;
        }
        public FileContentResult DownloadV(string state)
        {
            if (Session["FristRegisterId"] == null)
                return null;
            string savePath = Server.MapPath(@"~\Uploaded\Help\RequestRanking.mp4");

            MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
            return File(st.ToArray(), MimeType.Get(".mp4"), "Help_" + "RequestRanking" + ".mp4");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<prs_tblRequestRankingSelect> data = null;
            data = m.prs_tblRequestRankingSelect("fldFirstRegisterId", Session["FristRegisterId"].ToString(), 0).ToList();
            return this.Store(data);
        }

        public ActionResult ShowHideDesktopIcons(string ModuleIds, string Id, string HadafId)
        {
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            //var IsSend = servic.GetDashboardWithFilter("fldRequestId", Id,"","","","", 1, out Err).FirstOrDefault();

            //if (IsSend == null || Session["Username"] != null)
            //{
            Models.RaiSamEntities m = new RaiSamEntities();
            var desktop = this.GetCmp<Desktop>("Desktop1");
            var ModuleIdsSplit = ModuleIds.Split(';').Reverse().Skip(1).Reverse().ToArray();
           // desktop.SetShortcutHidden("_CommentSent", true);
            //  desktop.SetShortcutHidden("_KhodEzhari", true);

            desktop.SetShortcutHidden("_Ticket", true);

            var Req = m.prs_tblRequestRankingSelect("fldId", Id, 0).FirstOrDefault();
            //if (Req.fldType == 2)
            //{
            //    desktop.SetShortcutHidden(ModuleIdsSplit[25], false);
            //    desktop.SetShortcutHidden(ModuleIdsSplit[27], false);
            //}
            //else if (Req.fldType == 3)
            //{
            //    desktop.SetShortcutHidden(ModuleIdsSplit[18], false);
            //    desktop.SetShortcutHidden(ModuleIdsSplit[27], false);
            //}
            //else if (Req.fldType == 5)
            //{
            //    desktop.SetShortcutHidden(ModuleIdsSplit[29], false);
            //    desktop.SetShortcutHidden(ModuleIdsSplit[27], false);
            //}
            //else
            //{
                for (int i = 0; i < ModuleIdsSplit.Count(); i++)
                {
                   /* if (ModuleIdsSplit[i] == "_RotbeEtebari")
                    {
                        if (Models.Permission.haveAccessClient(Convert.ToInt32(TreeId), 31, Convert.ToBoolean(Req.fldPersonType)) && i != 25 && i != 18 && i != 29)//درخواست ناظر
                            desktop.SetShortcutHidden(ModuleIdsSplit[i], false);
                    }
                    else if (i == 5)
                    {
                        desktop.SetShortcutHidden(ModuleIdsSplit[i], false);//ticket
                    }
                    else
                    {*/


                    if (m.prs_SelectExistsOpertionOnAction(ModuleIdsSplit[i], Convert.ToInt32(Id)).FirstOrDefault().fldFalg == true)
                            if (Models.Permission.haveAccessClient(Convert.ToInt32(HadafId), ModuleIdsSplit[i]))
                            desktop.SetShortcutHidden(ModuleIdsSplit[i], false);// !Models.Permission.haveAccessClient(Convert.ToInt32(TreeId), i + 1));
                    
                   /* }*/
                }

            //}
            desktop.SetShortcutHidden("_PrintCompanyProfile", true);
            desktop.SetShortcutHidden("DemandConsortium", true);
            desktop.SetShortcutHidden("ListofRequest", true);
           // desktop.SetShortcutHidden("_Ticket", true);

            Session["RankingId"] = Id;
            Session["HadafId"] = HadafId;
            return this.Direct();
            


        }
        public ActionResult CheckHideStartIcons(string ModuleIds, string HadafId, string Id)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var EtelaieTitleAll = "";
            var EtelaieIDAll = "";

            Models.RaiSamEntities m = new RaiSamEntities();
            var Req = m.prs_tblRequestRankingSelect("fldId", Id, 0).FirstOrDefault();
            string MenuItems = "";
            try
            {
                var ModuleIdsSplit = ModuleIds.Split(';').Reverse().Skip(1).Reverse().ToArray();
                for (int i = 0; i < ModuleIdsSplit.Count(); i++)
                {
                    if (m.prs_SelectExistsOpertionOnAction(ModuleIdsSplit[i], Convert.ToInt32(Id)).FirstOrDefault().fldFalg == true)
                    {
                        if (Models.Permission.haveAccessClient(Convert.ToInt32(HadafId), ModuleIdsSplit[i]))
                            MenuItems = MenuItems + ModuleIdsSplit[i] + "|1;";
                    }
                    else
                        MenuItems = MenuItems + ModuleIdsSplit[i] + "|0;";
                }


                if (Session["HadafId"] != null)
                {
                    var Etelaie = m.prs_tblAnnouncementManagerSelect("fldCheckTarikhShamsi_Type_NotSeen_Hadaf", "1", u.UserInputId, 0, HadafId.ToString()).ToList();
                    foreach (var item in Etelaie)
                    {
                        EtelaieTitleAll = EtelaieTitleAll + item.fldTitle + ";";
                        EtelaieIDAll = EtelaieIDAll + item.fldID + ";";
                    }
                }
               
            }
            catch (Exception x)
            {
                MenuItems = "";
            }

            return Json(new { MenuItems = MenuItems, ReqType = Req.fldType, EtelaieTitleAll = EtelaieTitleAll, EtelaieIDAll = EtelaieIDAll }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult New()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var currentDate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            Ext.Net.MVC.PartialViewResult result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.currentDate = currentDate;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRequestType()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_SelectRequestType(Convert.ToInt32(Session["FristRegisterId"])).Select(c => new { Id = c.Id, Name = c.Title });
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetStakeholdersTreeAllowed(int? ID)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            //if (ID == null)
            //    ID = 0;
            //var FieldName = "FirstRegisterId_Dashbord";
            //if (ID == 1 || ID == 0)
               var FieldName = "fldFirstRegisterId";
               var q = m.prs_tblFirstRegister_HadafAllowedSelect(FieldName, Session["FristRegisterId"].ToString(), 0).Select(c => new { HadafAllowedId = c.fldId, HadafAllowedName = c.fldNmeHadaf });
            //.Where(l => l.fldTreeId == 1 || l.fldTreeId == 2 || l.fldTreeId == 3 || l.fldTreeId == 4 || l.fldTreeId == 6 || l.fldTreeId == 11)
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetStakeholdersTreeAllowed()
        //{
        //    if (Session["FristRegisterId"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    var q = servic1.GetStakeholderAllowedWithFilter("fldFirstRegisterId", Session["FristRegisterId"].ToString(), 0, out Err1).Select(c => new { StakeholdersTreeAllowedId = c.fldId, StakeholdersTreeAllowedName = c.TreeName });
        //    //.Where(l => l.fldTreeId == 1 || l.fldTreeId == 2 || l.fldTreeId == 3 || l.fldTreeId == 4 || l.fldTreeId == 6 || l.fldTreeId == 11)
        //    return Json(q, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsStakeholderAllowed(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            var k = true;
            var l = m.prs_tblFirstRegister_HadafAllowedSelect("fldFirstRegisterId", Session["FristRegisterId"].ToString(),  0).Where(h => h.fldId == Id).FirstOrDefault();
            //if (l.fldTreeId == 1 || l.fldTreeId == 2 || l.fldTreeId == 3 || l.fldTreeId == 4 || l.fldTreeId == 6 || l.fldTreeId == 11)
            //    k = false;
            return Json(new
            {
                NotSave = k
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(byte RequestType, int HadafId, string RequestDate)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = "";
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                /*if (c.Permission(22, Session["Username"].ToString(), Session["Password"].ToString(), out Err))
                {*/
                var t = m.prs_tblFirstRegister_HadafAllowedSelect("fldId", HadafId.ToString(),  0).FirstOrDefault();
                int sTree2 = 0;
                //if (t.fldTreeId == 31)
                //    sTree2 = 46;
                //else if (t.fldTreeId == 46)
                //    sTree2 = 31;

                //if (sTree2 != 0)
                //{
                //    var r = m.GetRequestRankingWithFilter("MonghaziNashode", (Session["FristRegisterId"]).ToString(), true, "", 0, out Err1).Where(l => l.fldStakeholderTreeId == sTree2).Any();
                //    if (r == true)
                //        return Json(new
                //        {
                //            Msg = "شرکت های ناظر فقط در یک تخصص احراز صلاحیت می شوند و مجاز نیستند توامان در هر 2 تخصص احراز صلاحیت شده و گواهینامه دریافت نمایند.",
                //            MsgTitle = "خطا"
                //        }, JsonRequestBehavior.AllowGet);
                //}

                var K = m.prs_tblRequestRankingInsert(Convert.ToInt32(RequestDate.Replace("/", "")), Convert.ToInt32(Session["FristRegisterId"]), HadafId, RequestType, u.UserInputId).FirstOrDefault();
               var Contract= m.prs_tblRegistrationFirstContractInsert(K.fldId, "", 0, 1, u.UserInputId).FirstOrDefault();

               var charkheNAme = "";
               var charkheId = 0;
               if (t.fldHadafId == 2) {
                   charkheNAme = "سرمایه گذاری واگن باری";
                   charkheId = 2;
               }
               if (t.fldHadafId == 3)
               {
                   charkheNAme = "سرمایه گذاری واگن مسافری";
                   charkheId = 3;
               }
               if (t.fldHadafId == 1)
               {
                   charkheNAme = "سرمایه گذاری لکوموتیو";
                   charkheId = 4;
               }
               if (t.fldHadafId == 4)
               {
                   charkheNAme = "سرمایه گذاری خط و زیر بنایی";
                   charkheId = 5;
               }

               Dictionary<string, object> parameters = new Dictionary<string, object>();
               parameters.Add("درخواست", K.fldId);
               parameters.Add("نام چرخه", charkheNAme);

               string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
               m.prs_InsertEnterToCycle_CharkheFirstEghdam(Contract.fldId, charkheId, null, "", u.UserInputId, jsonstr);
                Msg ="ذخیره با موفقیت انجام شد.";
                MsgTitle = "ذخیره موفق";
               
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
        public ActionResult MatnHtmlRequestRanking()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (Session["TreeId"] == null)
            //    return RedirectToAction("Index", "Home");
            Models.RaiSamEntities m = new RaiSamEntities();
            var MatnHtml = "";
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                PageHtml = m.prs_tblHtmlViewerSelect("Hadaf-NameTable", "1", null, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtml = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtml = "";
            }

            return Json(new { MatnHtml = MatnHtml }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowRelativeStakeholders()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadRelativeStakeholders()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var f = m.prs_tblFirstRegisterSelect("fldId", Session["FristRegisterId"].ToString(),"", 0).FirstOrDefault();
            //List<RaiTrainAdminWS.OBJ_StakeholderGrouping> data = null;
            //data = servic.GetStakeholderGroupingWithFilter("fldGroupNameId_Name", GroupNameId, "", 0, Session["Username"].ToString(), Session["Password"].ToString(), out Err).ToList();
            //if (PersonType=="1")
            //    data = servic.GetStakeholderGroupingWithFilter("fldGroupNameId_Name", GroupNameId, "", 0, Session["Username"].ToString(), Session["Password"].ToString(), out Err).Where(l => l.fldStakeholderTreeId == 28 || l.fldStakeholderTreeId == 12 || l.fldStakeholderTreeId == 32 || l.fldStakeholderTreeId == 47).ToList();

            List<prs_HadafListForFirstRegister> data = null;
            data = m.prs_HadafListForFirstRegister(f.fldSelectedHadaf).ToList();
            //if (f.fldPersonalType == true)
            //    data = servic1.GetStackholderTreeListForFirstRegister(f.fldAllowedTrees, out Err1).Where(l => l.fldId == 28 || l.fldId == 12 || l.fldId == 32 || l.fldId == 47).ToList();


            return this.Store(data);
        }
        public ActionResult GetChecked()
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFirstRegister_SelectedHadafSelect("fldFirstRegisterId", Session["FristRegisterId"].ToString(), 0).ToList();

            //if (Err1.ErrorType)
            //{
            //    X.Msg.Show(new MessageBoxConfig
            //    {
            //        Buttons = MessageBox.Button.OK,
            //        Icon = MessageBox.Icon.ERROR,
            //        Title = "خطا",
            //        Message = Err1.ErrorMsg
            //    });
            //    DirectResult result = new DirectResult();
            //    return result;
            //}
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSelectedTrees(string StakeholdersTreeIds)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
            try
            {

                var StakeholdersTreeIdSplit = StakeholdersTreeIds.Split(';').Reverse().Skip(1).Reverse().ToList();
                var MalekLoko = 0;
                var PLoko = 0;
                foreach (var item in StakeholdersTreeIdSplit)
                {
                    var ii = Convert.ToInt32(item);
                    if (ii == 45)
                        MalekLoko = 1;
                    if (ii == 30 || (ii >= 34 && ii <= 44))
                        PLoko = 1;
                }

                if (MalekLoko == 1 && PLoko == 1)
                    return Json(new
                    {
                        Msg = "مالک لکوموتیو نمی تواند به عنوان پیمانکار درخواست ثبت نماید.",
                        MsgTitle = "خطا",
                        Er = 1
                    }, JsonRequestBehavior.AllowGet);

                var SaveData = m.prs_tblFirstRegister_SelectedHadafSelect("fldFirstRegisterId", Session["FristRegisterId"].ToString(), 0).ToList();
                //if (Err1.ErrorType)
                //{
                //    return Json(new
                //    {
                //        Msg = Err1.ErrorMsg,
                //        MsgTitle = "خطا",
                //        Er = 1
                //    }, JsonRequestBehavior.AllowGet);
                //}

                string[] SaveDataId = new string[SaveData.Count];
                for (int i = 0; i < SaveData.Count; i++)
                {
                    SaveDataId[i] = SaveData[i].fldHadafId.ToString();
                }
                var DeleteRow = SaveDataId.Except(StakeholdersTreeIdSplit).ToArray();
                var InsertRow = StakeholdersTreeIdSplit.Except(SaveDataId).ToArray();

                if (DeleteRow.Length != 0)
                {
                    for (int i = 0; i < DeleteRow.Count(); i++)
                    {
                         m.prs_tblFirstRegister_SelectedHadafDelete("FirstRegisterId", Convert.ToInt32(Session["FristRegisterId"]), DeleteRow[i]);
                         Msg ="حذف با موفقیت انجام شد.";
                        MsgTitle = "عملیات موفق";
                       
                    }

                }
                if (InsertRow.Length != 0)
                {
                    for (int i = 0; i < InsertRow.Count(); i++)
                    {
                        m.prs_tblFirstRegister_SelectedHadafInsert(Convert.ToInt32(Session["FristRegisterId"]), Convert.ToInt32(InsertRow[i]),user.UserInputId);
                        Msg = "ذخیره با موفقیت انجام شد";
                        MsgTitle = "عملیات موفق";
                       
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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
