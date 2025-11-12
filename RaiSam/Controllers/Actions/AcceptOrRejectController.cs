using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.IO;
using RaiSam.Models;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using Newtonsoft.Json;

namespace RaiSam.Controllers.Actions
{
    public class AcceptOrRejectController : Controller
    {
        //
        // GET: /AcceptOrReject/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        TaiidMarhaleController TaiidMarhale = new TaiidMarhaleController();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAcceptOrReject(string AcceptOrRejectInfo/*List<SrvClasses.TOL.Action.OBJ_AcceptOrReject> ListArray*/, int EghdamId, int cartableId, byte NoeGhabelCharkhesh)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "", MsgTitle = ""; var Er = 0;
            List<Models.prs_tblAcceptOrRejectSelect> ListArray = JsonConvert.DeserializeObject<List<Models.prs_tblAcceptOrRejectSelect>>(AcceptOrRejectInfo);
            //var ListArray = JsonConvert.DeserializeObject<List<SrvClasses.TOL.Action.OBJ_AcceptOrReject>>(AcceptOrRejectInfo);
            List<ActionsResult> ListResults = new List<ActionsResult>();
          
            var TypeName = ""; var CartablName = ""; var EghdamName = "";
            string ValueForFormul = "";
            try
            {
                foreach (var item in ListArray)
                {
                    if (item.fldType)
                        ValueForFormul = "a1";
                    else
                        ValueForFormul = "a0";

                    int Charkhe_FirstEghdamId = m.prs_tblCharkhe_FirstEghdamSelect("LastRecordByEnterCycleId", item.fldEnterCycleId.ToString(), 0).FirstOrDefault().fldId;

                    ActionsResult r = new ActionsResult();

                    var dd = m.prs_tblEnterToCycleSelect("fldId", item.fldEnterCycleId.ToString(), 1).FirstOrDefault();
                    r.ContractId = dd.fldcontractId;

                    var data = m.prs_tblOperationSelect("fldId", "3", 0, 0).FirstOrDefault();

                    string Eghdam = "";
                    Eghdam = m.prs_tblActionsSelect("fldId", (EghdamId).ToString(), 0).FirstOrDefault().fldName;
                    string Kartabl = "";
                    Kartabl = m.prs_tblKartablSelect("fldId", (cartableId).ToString(), 0).FirstOrDefault().fldName;

                    TypeName = "";
                    if (item.fldType)
                        TypeName = "تائید";
                    else
                        TypeName = "عدم تائید";
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نوع", TypeName);
                    parameters.Add("نام اقدام", Eghdam);
                    parameters.Add("نام کارتابل ", Kartabl);
                    // parameters.Add("کد جدول ورود و خروج", AcceptOrReject.fldInputId);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));
                    m.prs_tblAcceptOrRejectInsert(Id, item.fldExternalId, item.fldType, EghdamId, cartableId, item.fldDesc, NoeGhabelCharkhesh, user.UserInputId, IP);

                    var uu = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                    m.prs_tblSafSMSUpdate(3, uu.fldShakhsId, Charkhe_FirstEghdamId, user.UserInputId);
                   
                        //if (item.fldType)
                        r.Result = ValueForFormul;
                        ListResults.Add(r);
                 
                        int EghdamBadi = 0;
                        if (data.fldeffective == true)
                        {
                            var rul = m.prs_tblReferralRulesSelect("Check_NextAction", dd.fldCharkheId.ToString(), EghdamId.ToString(), "3", ValueForFormul, 0).FirstOrDefault();
                            if (rul != null)
                            {
                                EghdamBadi = TaiidMarhale.SelectEghdamBadiId(rul.fldId, EghdamId, dd.fldCharkheId, 3, dd.fldId, cartableId);
                            }
                            else
                            {
                                var next = m.prs_SelectNextAction_CharkheEghdam(dd.fldCharkheId, EghdamId).FirstOrDefault();
                                if (next != null)
                                    EghdamBadi = next.fldEghdamId;
                            }
                        }
                        
                        string Eghdamm = "";
                        var E = m.prs_tblActionsSelect("fldId", (EghdamBadi).ToString(), 0).FirstOrDefault();
                        if (E != null) Eghdamm = E.fldName;
                        var EnterCycle = "";
                        EnterCycle = m.prs_tblEnterToCycleSelect("fldId", item.fldEnterCycleId.ToString(), 0).FirstOrDefault().fldNameCharkhe;
                        parameters = new Dictionary<string, object>();
                        parameters.Add("نام اکشن", EghdamBadi);
                        parameters.Add("نام چرخه", EnterCycle);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        var d=m.prs_UpdateCharkhFirstTableActions("AcceptOrReject", (Id.Value).ToString(), EghdamBadi, item.fldEnterCycleId, user.UserInputId, "", jsonstr).FirstOrDefault();

                        var cc = m.prs_SelectCartablInCherkhe_Eghdam(dd.fldCharkheId, EghdamBadi).FirstOrDefault();
                        if (cc.fldIdKartabl != 100)
                        {
                            var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                            RaiSms.Service w = new RaiSms.Service();
                            var Matn = "به کارتابل شما در سامانه ماده12، پرونده ارجاع داده شده است. لطفا برای بررسی به سامانه مراجعه فرمایید.";
                            var l = m.prs_ListNameShakhInCartabl("AcceptOrReject", Convert.ToInt32(Id.Value)).ToList();
                            foreach (var itemm in l)
                            {

                                if (itemm.fldMobile != "")
                                {
                                    m.prs_tblSafSMSInsert(Matn, 1, itemm.fldShakhsId, d.fldIdChecrkheFirst, user.UserInputId);
                                    var returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, itemm.fldMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                                    if (returnCode.Length <= 3)
                                    {
                                        System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                        var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                                        m.prs_tblErrorInsert(ErrorId, "ارسال پیامک برای کارتابل: " + w.ShowError(returnCode, "FA"), DateTime.Now, Input.fldId, "");
                                    }
                                }
                            }
                        }
                }
                Msg = "ذخیره با موفقیت انجام شد.";
                MsgTitle = "ذخیره موفق";
            }
            catch (Exception x)
            {
                var ErMsg = "";
                if (x.InnerException != null)
                    ErMsg = x.InnerException.Message;
                else
                    ErMsg = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                if (cartableId != 0)
                {
                    CartablName = m.prs_tblKartablSelect("fldId", cartableId.ToString(), 0).FirstOrDefault().fldName;
                }
                if (EghdamId != 0)
                {
                    EghdamName = m.prs_tblActionsSelect("fldId", EghdamId.ToString(), 0).FirstOrDefault().fldName;
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نوع", TypeName);
                parameters.Add("نام اقدام", EghdamName);
                parameters.Add("نام کارتابل ", CartablName);
                parameters.Add("کد خطا", ErrorId.Value);
                parameters.Add("متن خطا", "AcceptOrReject:SaveAcceptOrReject: " + ErMsg);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                m.prs_LogInsert(user.UserInputId, "dbo.tblAcceptOrReject", jsonstr, false);

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
                ResultOfAction = ListResults
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
