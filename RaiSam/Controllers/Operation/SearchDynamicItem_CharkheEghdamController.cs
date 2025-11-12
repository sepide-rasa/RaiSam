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
    public class SearchDynamicItem_CharkheEghdamController : Controller
    {
        //
        // GET: /SearchDynamicItem_CharkheEghdam/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(int Charkhe_EghdamId, int CharkheId, string GeneralRatingId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            string checkId = "";

            var check = m.prs_tblTitleRatingDynamic_CharkheEghdamSelect("fldCherkhe_EghdamId", Charkhe_EghdamId.ToString(), 0).ToList();
            foreach (var item in check)
            {
                checkId = checkId + item.fldTitleRatingDynamicId + ";";
            }
            result.ViewBag.checkId = checkId;
            result.ViewBag.Charkhe_EghdamId = Charkhe_EghdamId;
            result.ViewBag.GeneralRatingId = GeneralRatingId;
            result.ViewBag.CharkheId = CharkheId;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters,string GeneralRatingId, int CharkheId)
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
                var k = m.prs_tblCharkheSelect("fldId", CharkheId.ToString(), u.UserSecondId, 0).FirstOrDefault();
                int hadaf=0;
                if(CharkheId==4)
                    hadaf=1;
                else if(CharkheId==2)
                    hadaf=2;
                else if(CharkheId==3)
                    hadaf=3;
                else if(CharkheId==5)
                    hadaf=4;
                var data = m.prs_tblItemsDynamicRatingSelect("GeneralRatingId_Charkhe", GeneralRatingId, 0).Where(l => l.fldHadafId == hadaf).ToList();
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
        public ActionResult SaveTitleRatingDynamic_CharkheEghdam(string OprationArray1, int Charkhe_EghdamId)
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
                    List<prs_tblTitleRatingDynamic_CharkheEghdamSelect> OprationArray = JsonConvert.DeserializeObject<List<prs_tblTitleRatingDynamic_CharkheEghdamSelect>>(OprationArray1);

                    var SaveData = m.prs_tblTitleRatingDynamic_CharkheEghdamSelect("fldCherkhe_EghdamId", Charkhe_EghdamId.ToString(), 0).ToList();
                    string[] SaveOperationId = new string[SaveData.Count];
                    int[] SaveTableId = new int[SaveData.Count];
                    string[] NewOperationId = new string[OprationArray.Count];

                    for (int j = 0; j < SaveData.Count; j++)
                    {
                        SaveOperationId[j] = SaveData[j].fldTitleRatingDynamicId.ToString();
                        SaveTableId[j] = SaveData[j].fldId;
                    }
                    for (int i = 0; i < OprationArray.Count; i++)
                    {
                        NewOperationId[i] = OprationArray[i].fldTitleRatingDynamicId.ToString();
                    }
                    var DeleteRow = SaveOperationId.Except(NewOperationId).ToArray();
                    var InsertRow = NewOperationId.Except(SaveOperationId).ToArray();
                    var UpdatedRow = NewOperationId.Intersect(SaveOperationId).ToArray();
                    if (DeleteRow.Length != 0)
                    {
                        DiagnosisLog = 1;//حذف
                        foreach (var item in DeleteRow)
                        {
                            var Index = SaveOperationId.ToList().IndexOf(item);




                            var NameOperation = "";
                            if (SaveData[Index].fldTitleRatingDynamicId != 0)
                            {
                                NameOperation = m.prs_tblTitleRatingDynamicSelect("fldId", SaveData[Index].fldTitleRatingDynamicId.ToString(), 0).FirstOrDefault().fldTitleFA;
                            }
                            string NameCharkhe = "", NameEghdam = "";
                            if (SaveData[Index].fldCherkhe_EghdamId != 0)
                            {
                                var q = m.prs_tblCharkhe_EghdamSelect("fldId", SaveData[Index].fldCherkhe_EghdamId.ToString(), "", 0).FirstOrDefault();
                                NameCharkhe = q.fldNameCharkhe; NameEghdam = q.fldNameEghdam;
                            }
                       
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نام مدرک", NameOperation);
                            parameters.Add("نام چرخه", NameCharkhe);
                            parameters.Add("نام اقدام", NameEghdam);

                                m.prs_tblTitleRatingDynamic_CharkheEghdamDelete(SaveData[Index].fldId);
                            


                        }
                    }
                    if (InsertRow.Length != 0)
                    {
                        DiagnosisLog = 2;//ذخیره
                        foreach (var item in InsertRow)
                        {
                            var Index = OprationArray.FindIndex(l => l.fldTitleRatingDynamicId == Convert.ToInt32(item));
                            //Dictionary<string, object> parameters = new Dictionary<string, object>();
                            //var NameOperation = m.prs_tblTitleRatingDynamicSelect("fldId", OprationArray[Index].fldTitleRatingDynamicId.ToString(), 0).FirstOrDefault().fldTitleFA;
                            //var Charkhe_Eghdam = m.prs_tblCharkhe_EghdamSelect("fldId", OprationArray[Index].fldCherkhe_EghdamId.ToString(), "", 0).FirstOrDefault();
                         
                            //parameters.Add("نام مدرک", NameOperation);
                            //parameters.Add("نام چرخه", Charkhe_Eghdam.fldNameCharkhe);
                            //parameters.Add("نام اقدام", Charkhe_Eghdam.fldNameEghdam);
                            //string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblTitleRatingDynamic_CharkheEghdamInsert(OprationArray[Index].fldCherkhe_EghdamId,OprationArray[Index].fldTitleRatingDynamicId, u.UserInputId);
                            DiagnosisLog = 0;

                        }
                    }
                    if (UpdatedRow.Length != 0)
                    {
                        DiagnosisLog = 3;//ویرایش
                        foreach (var item in UpdatedRow)
                        {
                            var Index = OprationArray.FindIndex(l => l.fldTitleRatingDynamicId == Convert.ToInt32(item));
                            var index2 = Array.FindIndex(SaveOperationId, l => l == item);
                            m.prs_tblTitleRatingDynamic_CharkheEghdamUpdate(SaveTableId[index2], OprationArray[Index].fldCherkhe_EghdamId, OprationArray[Index].fldTitleRatingDynamicId, u.UserInputId);
                            DiagnosisLog = 0;

                        }
                    }
                    Msg = "ذخیره با موفقیت انجام شد";
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
                    parameters.Add("متن خطا", "SearchOperation:SaveTitleRatingDynamic_CharkheEghdam: " + InnerException);

                    var q = m.prs_tblCharkhe_EghdamSelect("fldId", Charkhe_EghdamId.ToString(), "", 0).FirstOrDefault();
                    parameters.Add("نام چرخه", q.fldNameCharkhe);
                    parameters.Add("نام اقدام", q.fldNameEghdam);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (DiagnosisLog == 1)//delete
                    {
                        m.prs_LogDelete(u.UserInputId, "dbo.tblTitleRatingDynamic_CharkheEghdam", jsonstr, false, Id_Log);
                    }
                    else if (DiagnosisLog == 2)//insert
                    {

                        m.prs_LogInsert(u.UserInputId, "dbo.tblTitleRatingDynamic_CharkheEghdam", jsonstr, false);
                    }
                    else if (DiagnosisLog == 3)//update
                    {
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblTitleRatingDynamic_CharkheEghdam", jsonstr, false, Id_Log);
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
