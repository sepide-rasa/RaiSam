using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using Newtonsoft.Json;
using RaiSam.Models;

namespace RaiSam.Controllers.Actions
{
    public class TaiidMarhaleController : Controller
    {
        //
        // GET: /TaiidMarhale/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadSelectedRequest(string EnterSicleIds)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Session["User"] == null)
                    return RedirectToAction("Logon", "Account", new { area = "" });
                UserInfo user = (UserInfo)(Session["User"]);

                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblRegistrationFirstContractSelect> ListLoco = new List<Models.prs_tblRegistrationFirstContractSelect>();

                var per = EnterSicleIds.Split(';');
             
                foreach (var item in per)
                {
                    var dd = m.prs_tblEnterToCycleSelect("fldId", item,1).FirstOrDefault();
                    if (dd != null)
                    {
                        Models.prs_tblRegistrationFirstContractSelect PerLoco = new Models.prs_tblRegistrationFirstContractSelect();
                        var data = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(),  0).FirstOrDefault();
                        if (data != null)
                        {
                            PerLoco.fldId = dd.fldId;
                            PerLoco.fldTitle = data.fldTitle;
                            PerLoco.fldHadafName = data.fldHadafName;
                            PerLoco.fldRequestId = data.fldRequestId;
                            ListLoco.Add(PerLoco);
                        }
                    }
                }

                return new JsonResult()
                {
                    Data = new
                    {
                        ListLoco = ListLoco
                    },
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTaiidMarhale(string Data/*List<OBJ_TaeedMarhale> ListArray*/, string fldDesc, int EghdamId, int cartableId/*,int Charkhe_FirstEghdamId*/)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0; 
                List<ActionsResult> ListResults = new List<ActionsResult>();

                List<Models.prs_tblTaeedMarhaleSelect> ListArray = JsonConvert.DeserializeObject<List<Models.prs_tblTaeedMarhaleSelect>>(Data);

                var CartablName = ""; var EghdamName = "";
                string ValueForFormul = "a";
                try
                {
                    foreach (var item in ListArray)
                    {
                        int Charkhe_FirstEghdamId = m.prs_tblCharkhe_FirstEghdamSelect("LastRecordByEnterCycleId", item.fldEnterCycleId.ToString(), 0).FirstOrDefault().fldId;
                        ActionsResult r = new ActionsResult();

                        var dd = m.prs_tblEnterToCycleSelect("fldId", item.fldEnterCycleId.ToString(),1).FirstOrDefault();
                        r.ContractId = dd.fldcontractId;

                     
                        var data = m.prs_tblOperationSelect("fldId", "1",0,0).FirstOrDefault();

                     

                        if (cartableId != 0)
                        {
                            CartablName = m.prs_tblKartablSelect("fldId", cartableId.ToString(),0).FirstOrDefault().fldName;
                        }
                        if (EghdamId != 0)
                        {
                            EghdamName = m.prs_tblActionsSelect("fldId", EghdamId.ToString(),0).FirstOrDefault().fldName;
                        }

                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("توضیحات", item.fldDesc);
                        parameters.Add("نام چرخه", dd.fldNameCharkhe);
                        parameters.Add("عنوان", dd.fldTitleContract);
                        parameters.Add("نام اقدام", EghdamName);
                        parameters.Add("نام کارتابل ", CartablName);
                        // parameters.Add("کد جدول ورود و خروج", EtmamCharkhe.fldInputId);
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    

                        System.Data.Entity.Core.Objects.ObjectParameter fldId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                         m.prs_tblTaeedMarhaleInsert(fldId, EghdamId, cartableId, item.fldDesc, user.UserInputId, jsonstr);

                         var uu = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();
                        m.prs_tblSafSMSUpdate(3,uu.fldShakhsId,Charkhe_FirstEghdamId,user.UserInputId);
                      
                            r.Result = ValueForFormul;
                            ListResults.Add(r);

                            int EghdamBadi = 0;
                            if (data.fldeffective == true)
                            {
                                var rul = m.prs_tblReferralRulesSelect("Check_NextAction", dd.fldCharkheId.ToString(), EghdamId.ToString(), "1", ValueForFormul,0).FirstOrDefault();
                                if (rul != null)
                                    EghdamBadi = SelectEghdamBadiId(rul.fldId, EghdamId, dd.fldCharkheId, 1, dd.fldId, cartableId);
                                else
                                {
                                    var next = m.prs_SelectNextAction_CharkheEghdam( dd.fldCharkheId,EghdamId).FirstOrDefault();
                                    if (next != null)
                                        EghdamBadi = next.fldEghdamId;
                                }
                            }
                   string Eghdam = "";
               var E = m.prs_tblActionsSelect("fldId", (EghdamBadi).ToString(), 0).FirstOrDefault();
               if (E != null) Eghdam = E.fldName;
                var EnterCycle = "";
                EnterCycle = m.prs_tblEnterToCycleSelect("fldId", item.fldEnterCycleId.ToString(), 0).FirstOrDefault().fldNameCharkhe;     
                  parameters = new Dictionary<string, object>();
                parameters.Add("نام اکشن", EghdamBadi);
                parameters.Add("نام چرخه", EnterCycle);
                 jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                 var d=m.prs_UpdateCharkhFirstTableActions("TaeedMarhale", (fldId.Value).ToString(), EghdamBadi, item.fldEnterCycleId, user.UserInputId, "", jsonstr).FirstOrDefault();

                        var cc=m.prs_SelectCartablInCherkhe_Eghdam(dd.fldCharkheId,EghdamBadi).FirstOrDefault();
                        if (cc.fldIdKartabl != 100)
                        {
                            var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                            RaiSms.Service w = new RaiSms.Service();
                            var Matn = "به کارتابل شما در سامانه ماده12، پرونده ارجاع داده شده است. لطفا برای بررسی به سامانه مراجعه فرمایید.";
                            var l = m.prs_ListNameShakhInCartabl("TaeedMarhale", Convert.ToInt32(fldId.Value)).ToList();
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

                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    if (cartableId != 0)
                    {
                        CartablName = m.prs_tblKartablSelect("fldId", cartableId.ToString(),0).FirstOrDefault().fldName;
                    }
                    if (EghdamId != 0)
                    {
                        EghdamName = m.prs_tblActionsSelect("fldId", EghdamId.ToString(),0).FirstOrDefault().fldName;
                    }

                    parameters.Add("نام اقدام", EghdamName);
                    parameters.Add("نام کارتابل ", CartablName);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "TaiidMarhale:SaveTaiidMarhale: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogInsert(user.UserInputId, "dbo.tblTaeedMarhale", jsonstr, false);
                   
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

        public int SelectEghdamBadiId(int ReferralRulesId, int EghdamId, int CharkheId, int OprationId, int EnterCycleId, int CartableId)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            int? EghdamBadi = 0;
         
            var rul = m.prs_tblReferralRulesSelect("fldId", ReferralRulesId.ToString(),"","","",0).FirstOrDefault();
            if (rul.fldOrder == 1)
            {
                if (rul.fldNextActionId != null)
                    EghdamBadi = rul.fldNextActionId;
            }
            else if (rul.fldOrder == 2)
            {
                if (rul.fldFormulId != null)
                    EghdamBadi = EghdamBadiWithFormulNevis(Convert.ToInt32(rul.fldFormulId), EghdamId, CharkheId, OprationId, EnterCycleId, CartableId);
            }
            else if (rul.fldOrder == 3)
            {
                if (rul.fldFormul != null)
                    EghdamBadi = EghdamBadiWithFormulSaz(rul.fldFormul, EghdamId, CharkheId, OprationId);
            }

            return Convert.ToInt32(EghdamBadi);
        }
        public int? EghdamBadiWithFormulNevis(int FormulId, int EghdamId, int CharkheId, int OperationId, int EnterCycleId, int CartableId)
        {
            int EghdamBadiId = 0;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
          
            var q = m.prs_tblComputationFormulaSelect("fldId", FormulId.ToString(),0).FirstOrDefault();

            string Formula = q.fldFormul;
            string[] ReferenceSplit = q.fldLibrary.Split(';').Reverse().Skip(1).Reverse().ToArray();

            ICodeCompiler loCompiler = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters loParameters = new CompilerParameters();

            for (int i = 0; i < ReferenceSplit.Length; i++)
            {
                if (ReferenceSplit[i] == "System.dll" || ReferenceSplit[i] == "System.Data.dll" || ReferenceSplit[i] == "System.Xml.dll" || ReferenceSplit[i] == "System.Core.dll")
                {
                    loParameters.ReferencedAssemblies.Add(ReferenceSplit[i]);
                }
                else
                {
                    var Path = Server.MapPath(@"~\Reference\" + ReferenceSplit[i]);
                    loParameters.ReferencedAssemblies.Add(Path);
                }
            }

            loParameters.GenerateInMemory = true;
            CompilerResults loCompiled = loCompiler.CompileAssemblyFromSource(loParameters, Formula);

            Assembly loAssembly = loCompiled.CompiledAssembly;
            object loObject = loAssembly.CreateInstance("MyNamespace.MyClass");

            

            object[] loCodeParms = new object[8];
            loCodeParms[0] = EnterCycleId;
            loCodeParms[1] = CharkheId;
            loCodeParms[2] = EghdamId;
            loCodeParms[3] = CartableId;
            loCodeParms[4] = user.UserSecondId;
            loCodeParms[5] = user.UserInputId;
            loCodeParms[6] = user.UserKey;
            loCodeParms[7] = IP;

            object loResult = loObject.GetType().InvokeMember("DynamicCode", BindingFlags.InvokeMethod, null, loObject, loCodeParms);

            EghdamBadiId = (int)loResult;

            return EghdamBadiId;
        }
        public int EghdamBadiWithFormulSaz(string Formul, int EghdamId, int CharkheId, int OprationId)
        {
            int EghdamBadiId = 0;
            string result = "";
            string temp = "";
            UserInfo user = (UserInfo)(Session["User"]);

            Formul = Formul.Replace("÷", "/");
            int Count1 = Formul.Count(k => k == ')');
            int Count2 = Formul.Count(k => k == '(');
            int Count3 = Formul.Count(k => k == ']');
            int Count4 = Formul.Count(k => k == '[');
            //if (Count1 != Count2 || Count3 != Count4)
            //{
            //    return Json(new { Er = 1, MsgTitle = "خطا", Msg = "تعداد پرانتزهای باز شده با تعداد پرانتزهای بسته شده یکی نیست." }, JsonRequestBehavior.AllowGet);
            //}

            var s = Formul.Split(';');
            for (int i = 0; i < s.Length - 1; i++)
            {
                if (s[i] == "then" || s[i] == "else")
                {
                    s[i] = ",";
                }
                temp = temp + s[i];
            }
            var sss = Summary(temp).Replace('[', '(').Replace(']', ')');
            //result = Calculate(sss).ToString();
            EghdamBadiId = Convert.ToInt32(Calculate(sss));
            return EghdamBadiId;
        }
        private static string Summary(string temp)
        {
            string _prefix = "";
            string _postfix = "";
            string str;
            string _else = str = temp;
            string _then = str;
            string _if = str;
            if (!temp.Trim().Contains("if"))
                return temp;
            spliteStatment(temp, out _prefix, out _if, out _then, out _else, out _postfix);
            if (Condition(Summary(_if)))
                return Summary(_prefix + _then + _postfix);
            else
                return Summary(_prefix + _else + _postfix);
        }

        private static bool Condition(string _if)
        {
            if (_if.Contains("!="))
            {
                string[] strArray = _if.Split(new string[1]
        {
          "!="
        }, StringSplitOptions.None);
                double? nullable1 = Calculate(strArray[0]);
                double? nullable2 = Calculate(strArray[1]);
                return (nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) != 0;
            }
            else if (_if.Contains("="))
            {
                if (CountOf(_if, '=') > 1)
                    throw new Exception("Error in =");
                string[] strArray = _if.Split(new string[1]
        {
          "="
        }, StringSplitOptions.None);
                double? nullable1 = Calculate(strArray[0]);
                double? nullable2 = Calculate(strArray[1]);
                return (nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue == nullable2.HasValue ? 1 : 0)) != 0;
            }
            else if (_if.Contains("<"))
            {
                if (CountOf(_if, '<') > 2)
                    throw new Exception("Error in <");
                string[] strArray = _if.Split(new string[1]
        {
          "<"
        }, StringSplitOptions.None);
                if (CountOf(_if, '<') == 1)
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    return (nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0;
                }
                else
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    int num;
                    if ((nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0)
                    {
                        nullable1 = Calculate(strArray[1]);
                        nullable2 = Calculate(strArray[2]);
                        num = (nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 ? 1 : 0;
                    }
                    else
                        num = 1;
                    return num == 0;
                }
            }
            else
            {
                if (!_if.Contains(">"))
                    return true;
                if (CountOf(_if, '>') > 2)
                    throw new Exception("Error in >");
                string[] strArray = _if.Split(new string[1]
        {
          ">"
        }, StringSplitOptions.None);
                if (CountOf(_if, '>') == 1)
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    return (nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0;
                }
                else
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    int num;
                    if ((nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0)
                    {
                        nullable1 = Calculate(strArray[1]);
                        nullable2 = Calculate(strArray[2]);
                        num = (nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 ? 1 : 0;
                    }
                    else
                        num = 1;
                    return num == 0;
                }
            }
        }

        private static int CountOf(string STR, char CHAR)
        {
            int num1 = 0;
            foreach (int num2 in STR)
            {
                if (num2 == (int)CHAR)
                    ++num1;
            }
            return num1;
        }

        public static double? Calculate(string Formoul)
        {
            Models.FM F = new Models.FM();
            int num = 12;
            double result;
            if (F.Neu(Formoul, out result) || num != 21)
                return new double?(result);
            return new double?();
        }

        private static void spliteStatment(string temp, out string _prefix, out string _if, out string _then, out string _else, out string _postfix)
        {
            bool flag = false;
            do
            {
                if (temp.Length > 0 && (flag = Removeable(temp)))
                    temp = temp.Substring(1, temp.Length - 2);
            }
            while (flag);
            _prefix = "";
            if (!temp.StartsWith("if("))
            {
                for (int index = 2; index < temp.Length; ++index)
                {
                    if ((int)temp[index - 2] == 105 && (int)temp[index - 1] == 102 && (int)temp[index] == 40)
                    {
                        _prefix = temp.Substring(0, index - 2);
                        if (((object)temp.Substring(index - 2)).ToString() != "")
                        {
                            temp = temp.Substring(index - 2);
                            break;
                        }
                    }
                }
            }
            do
            {
                if (temp.Length > 0 && (flag = Removeable(temp)))
                    temp = temp.Substring(1, temp.Length - 2);
            }
            while (flag);
            int num = 0;
            int index1;
            for (index1 = 2; index1 < temp.Length; ++index1)
            {
                if ((int)temp[index1] == 40 || (int)temp[index1] == 91)
                    ++num;
                if ((int)temp[index1] == 41 || (int)temp[index1] == 93)
                    --num;
                if ((int)temp[index1] == 44 && num == 0)
                    break;
            }
            _if = temp.Substring(temp.IndexOf("if(") + 3, index1 - 4);
            int index2;
            for (index2 = index1 + 1; index2 < temp.Length; index2++)
            {
                if ((int)temp[index2] == 40 || (int)temp[index2] == 91)
                    num++;
                if ((int)temp[index2] == 41 || (int)temp[index2] == 93)
                    num--;
                if ((int)temp[index2] == 44 && num == 0)
                    break;
            }
            _then = temp.Substring(temp.IndexOf(_if) + _if.Length + 2, index2 - (temp.IndexOf(_if) + _if.Length + 2));
            _postfix = "";
            if (index2 == temp.Length)
            {
                _else = "";
            }
            else
            {
                int index3;
                for (index3 = index2 + 1; index3 < temp.Length; ++index3)
                {
                    if ((int)temp[index3] == 40 || (int)temp[index3] == 91)
                        ++num;
                    else if (((int)temp[index3] == 41 || (int)temp[index3] == 93) && num != 0)
                        --num;
                    else if (((int)temp[index3] == 41 || (int)temp[index3] == 93) && num == 0)
                        break;
                }
                _else = temp.Substring(temp.IndexOf("if(" + _if + "," + _then) + _if.Length + _then.Length + 7, index3 - (temp.IndexOf("if(" + _if + "," + _then) + _if.Length + _then.Length + 7));
                _postfix = temp.Substring(index3);
            }
        }

        private static bool Removeable(string temp)
        {
            if (temp.Length < 1 || (int)temp[0] != 40 && (int)temp[0] != 91 || (int)temp[temp.Length - 1] != 41 && (int)temp[temp.Length - 1] != 93)
                return false;
            int num = 0;
            for (int index = 0; index < temp.Length; ++index)
            {
                if ((int)temp[index] == 40 || (int)temp[index] == 91)
                    ++num;
                if ((int)temp[index] == 41 || (int)temp[index] == 93)
                {
                    --num;
                    if (num == 0 && index < temp.Length - 1)
                        return false;
                    if (num == 0 && index == temp.Length - 1)
                        return true;
                }
            }
            return true;
        }
    }
}
