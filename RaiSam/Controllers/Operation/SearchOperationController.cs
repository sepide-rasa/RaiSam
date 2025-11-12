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

namespace RaiSam.Controllers.Operation
{
    public class SearchOperationController : Controller
    {
        //
        // GET: /SearchOperation/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(int state, int Charkhe_EghdamId, int NoeGhebelCharkheshId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            string checkId = "";
            string checkId_Status = "";
            int CharkheId = 0;

            var check = m.prs_tblOpertion_ActionSelect("fldCharkhe_EghdamId", Charkhe_EghdamId.ToString(),"","",0).ToList();
            foreach (var item in check)
            {
                CharkheId = item.fldCharkheId;
                checkId = checkId + item.fldOpertionId + ";";
                checkId_Status = checkId_Status + Convert.ToByte(item.fldfaal) + ";";
            }
            result.ViewBag.state = state;
            result.ViewBag.checkId = checkId;
            result.ViewBag.checkId_Status = checkId_Status;
            result.ViewBag.Charkhe_EghdamId = Charkhe_EghdamId;
            result.ViewBag.NoeGhebelCharkheshId = NoeGhebelCharkheshId;
            result.ViewBag.CharkheId = CharkheId;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters, int NoeGhebelCharkheshId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (Permission.haveAccess(62))
            //{
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var data = m.prs_tblOperationSelect("fldNoeGhabekCharkheshId", NoeGhebelCharkheshId.ToString(), 0,0).ToList();
                return this.Store(data);
            }
            //}
            //else
            //{
            //    return RedirectToAction("Error", "Home", new { Code = "403" });
            //}
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOpration_Action(string OprationArray1, int Charkhe_EghdamId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0; 
                 var DiagnosisLog = 0; var Id_Log = 0;
                try
                {
                    List<prs_tblOpertion_ActionSelect> OprationArray = JsonConvert.DeserializeObject<List<prs_tblOpertion_ActionSelect>>(OprationArray1);

                    var SaveData = m.prs_tblOpertion_ActionSelect("fldCharkhe_EghdamId", Charkhe_EghdamId.ToString(),"","",0).ToList();
                    string[] SaveOperationId = new string[SaveData.Count];
                    int[] SaveTableId = new int[SaveData.Count];
                    string[] NewOperationId = new string[OprationArray.Count];

                    for (int j = 0; j < SaveData.Count; j++)
                    {
                        SaveOperationId[j] = SaveData[j].fldOpertionId.ToString();
                        SaveTableId[j] = SaveData[j].fldId;
                    }
                    for (int i = 0; i < OprationArray.Count; i++)
                    {
                        NewOperationId[i] = OprationArray[i].fldOpertionId.ToString();
                    }
                    var DeleteRow = SaveOperationId.Except(NewOperationId).ToArray();
                    var InsertRow = NewOperationId.Except(SaveOperationId).ToArray();
                    var UpdatedRow = NewOperationId.Intersect(SaveOperationId).ToArray();
                    if (DeleteRow.Length != 0)
                    {
                        DiagnosisLog = 1;//حذف
                        foreach (var item in DeleteRow)
                        {
                            var CheckDel=false;
                            var Index = SaveOperationId.ToList().IndexOf(item);

                            var Ma = m.prs_tblReferralRulesSelect("fldOpertion_ActionId", SaveData[Index].fldId.ToString(), "", "", "", 0).FirstOrDefault();
                            var Ch = m.prs_tblCharkheSelect("fldDefulteOpertionId", SaveData[Index].fldId.ToString(), 0, 0).FirstOrDefault();
                            var S = m.prs_tblSpecificCartablePermissionSelect("fldOperation_ActionId", SaveData[Index].fldId.ToString(), 0).FirstOrDefault();
                            var Ch_E = m.prs_tblCharkhe_EghdamSelect("fldOpertionId_Defulte", SaveData[Index].fldId.ToString(), "", 0).FirstOrDefault();
                            if (Ma != null || Ch != null || S != null || Ch_E != null)
                                CheckDel = true;

                            

                            var NameOperation = "";
                            if (SaveData[Index].fldOpertionId != 0)
                            {
                                NameOperation = m.prs_tblOperationSelect("fldId", SaveData[Index].fldOpertionId.ToString(),0,0).FirstOrDefault().fldName;
                            }
                            string NameCharkhe = "", NameEghdam = "";
                            if (SaveData[Index].fldCharkhe_EghdamId != 0)
                            {
                                var q = m.prs_tblCharkhe_EghdamSelect("fldId", SaveData[Index].fldCharkhe_EghdamId.ToString(),"",0).FirstOrDefault();
                                NameCharkhe = q.fldNameCharkhe; NameEghdam = q.fldNameEghdam;
                            }
                            var faalName = "غیرفعال";
                            if (SaveData[Index].fldfaal)
                                faalName = "فعال";
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نام اکشن", NameOperation);
                            parameters.Add("نام چرخه", NameCharkhe);
                            parameters.Add("نام اقدام", NameEghdam);
                            parameters.Add("وضعیت", faalName);
                            parameters.Add("توضیحات", SaveData[Index].fldDesc);

                            if (CheckDel == false)
                                m.prs_tblOpertion_ActionDelete(SaveData[Index].fldId, "fldId");
                            else
                            {
                                DiagnosisLog = 0;

                                parameters.Add("متن خطا", "SearchOperation:SaveOpration_Action: " + "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.");
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                                m.prs_LogDelete(u.UserInputId, "dbo.tblOpertion_Action", jsonstr, false, SaveData[Index].fldId);
                                return Json(new
                                {
                                    Er = 1,
                                    Msg = "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.",
                                    MsgTitle = "خطا"
                                }, JsonRequestBehavior.AllowGet);
                            }
                            
                           
                        }
                    }
                    if (InsertRow.Length != 0)
                    {
                        DiagnosisLog = 2;//ذخیره
                        foreach (var item in InsertRow)
                        {
                            var Index = OprationArray.FindIndex(l => l.fldOpertionId == Convert.ToInt32(item));
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            var NameOperation = m.prs_tblOperationSelect("fldId", OprationArray[Index].fldOpertionId.ToString(), 0, 0).FirstOrDefault().fldName;
                            var Charkhe_Eghdam = m.prs_tblCharkhe_EghdamSelect("fldId", OprationArray[Index].fldCharkhe_EghdamId.ToString(), "", 0).FirstOrDefault();
                                var faalName = "غیرفعال";
                                if (OprationArray[Index].fldfaal)
                                faalName = "فعال";
                            parameters.Add("نام اکشن", NameOperation);
                            parameters.Add("نام چرخه", Charkhe_Eghdam.fldNameCharkhe);
                            parameters.Add("نام اقدام", Charkhe_Eghdam.fldNameEghdam);
                            parameters.Add("وضعیت", faalName);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblOpertion_ActionInsert(OprationArray[Index].fldOpertionId, u.UserInputId, "", OprationArray[Index].fldCharkhe_EghdamId, OprationArray[Index].fldfaal, jsonstr);
                            DiagnosisLog = 0;

                        }
                    }
                    if (UpdatedRow.Length != 0)
                    {
                        DiagnosisLog = 3;//ویرایش
                        foreach (var item in UpdatedRow)
                        {
                            var Index = OprationArray.FindIndex(l => l.fldOpertionId == Convert.ToInt32(item));
                            var index2 = Array.FindIndex(SaveOperationId, l => l == item);
                            m.prs_tblOpertion_ActionUpdate(SaveTableId[index2], OprationArray[Index].fldOpertionId, "", OprationArray[Index].fldCharkhe_EghdamId, OprationArray[Index].fldfaal, u.UserInputId);
                            DiagnosisLog = 0;
                           
                        }
                    }
                    Msg ="ذخیره با موفقیت انجام شد";
                    MsgTitle = "عملیات موفق";
                }
                catch (Exception x)
                {
                    var InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "SearchOperation:SaveOpration_Action: " + InnerException);
                 
                    var q = m.prs_tblCharkhe_EghdamSelect("fldId", Charkhe_EghdamId.ToString(),"",0).FirstOrDefault();
                    parameters.Add("نام چرخه", q.fldNameCharkhe);
                    parameters.Add("نام اقدام", q.fldNameEghdam);
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (DiagnosisLog == 1)//delete
                    {
                        m.prs_LogDelete(u.UserInputId, "dbo.tblOpertion_Action", jsonstr, false, Id_Log);
                    }
                    else if (DiagnosisLog == 2)//insert
                    {

                        m.prs_LogInsert(u.UserInputId, "dbo.tblOpertion_Action", jsonstr, false);
                    }
                    else if (DiagnosisLog == 3)//update
                    {
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblOpertion_Action", jsonstr, false, Id_Log);
                    }
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
        }

    }
}
