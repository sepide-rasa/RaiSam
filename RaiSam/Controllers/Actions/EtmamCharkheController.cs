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
    public class EtmamCharkheController : Controller
    {
        //
        // GET: /EtmamCharkhe/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        TaiidMarhaleController TaiidMarhale = new TaiidMarhaleController();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEtmamCharkhe(string Data/*List<SrvClasses.TOL.Action.OBJ_EtmamCharkhe> ListArray*/, string fldDesc, int EghdamId, int cartableId,
           int contractId, string fldShomareContract,string fldTarikhContract,string fldShomareMovafeghat,string fldTarikhMovafeghat)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }

            string Msg = "", MsgTitle = ""; var Er = 0;

            List<Models.prs_tblEtmamCharkheSelect> ListArray = JsonConvert.DeserializeObject<List<Models.prs_tblEtmamCharkheSelect>>(Data);

            List<ActionsResult> ListResults = new List<ActionsResult>();
            Models.RaiSamEntities m = new RaiSamEntities();

            var CartablName = ""; string EghdamName = "", ActionName = "";
            string ValueForFormul = "a";
            try
            {
                //var OpId = "11";
                //if (State == 2)
                //    OpId = "17";

                //param.FieldName = "fldId";
                //param.Value = OpId;
                //param.Top = 0;
                //var data = service.SelectOperation(param, user.UserKey, IP).OperationList.FirstOrDefault();
                var data = m.prs_tblOperationSelect("fldId", "15", 0, 0).FirstOrDefault();
                foreach (var item in ListArray)
                {
                    ActionsResult r = new ActionsResult();

                    var dd = m.prs_tblEnterToCycleSelect("fldId", item.fldEnterCycleId.ToString(), 1).FirstOrDefault();
                    r.ContractId = dd.fldcontractId;

                    string Eghdam = "";
                    Eghdam = m.prs_tblActionsSelect("fldId", (EghdamId).ToString(), 0).FirstOrDefault().fldName;
                    string Kartabl = "";
                    Kartabl = m.prs_tblKartablSelect("fldId", (cartableId).ToString(), 0).FirstOrDefault().fldName;
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نام اقدام", Eghdam);
                    parameters.Add("نام کارتابل", Kartabl);
                    parameters.Add("توضیحات", fldDesc);
                    // parameters.Add("کد ورود و خروج", EtmamCharkhe.fldInputId);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblEtmamCharkheInsert(Id, fldDesc, item.fldEnterCycleId, EghdamId, cartableId, user.UserInputId, jsonstr);

                    //****************** اینزرت پروژه
                    var TarikhMovafeghat = 0;
                    if (!(fldTarikhMovafeghat == "" || fldTarikhMovafeghat == null))
                        TarikhMovafeghat = Convert.ToInt32(fldTarikhMovafeghat.Replace("/", ""));

                    if (fldShomareMovafeghat == null)
                        fldShomareMovafeghat = "";
                    m.prs_InsertContractWithRegistration(contractId, Convert.ToInt32(fldTarikhContract.Replace("/", "")), fldShomareContract, TarikhMovafeghat, fldShomareMovafeghat);
               

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
                    m.prs_UpdateCharkhFirstTableActions("EtmamCharkhe", (Id.Value).ToString(), EghdamBadi, item.fldEnterCycleId, user.UserInputId, "", jsonstr);

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
                parameters.Add("نام اقدام", EghdamName);
                parameters.Add("نام کارتابل", CartablName);
                parameters.Add("کد خطا", ErrorId.Value);
                parameters.Add("متن خطا", "EtmamCharkhe:SaveEtmamCharkhe: " + ErMsg);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                m.prs_LogInsert(user.UserInputId, "dbo.tblEtmamCharkhe", jsonstr, false);

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
