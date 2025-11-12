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

namespace RaiSam.Areas.Faces.Controllers
{
    public class FinalCheckController : Controller
    {
        //
        // GET: /Faces/FinalCheck/
        List<Models.FinalCheckGrid> groupsGrid = new List<Models.FinalCheckGrid>();
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(/*string Title, int ContractId*/)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var currentDate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            Ext.Net.MVC.PartialViewResult result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.currentDate = currentDate;
            /*result.ViewBag.Title = Title;
            result.ViewBag.ContractId = ContractId;*/
            var IsInClient = 0;
            var Req = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
            if (Req.fldKartablId == 100) IsInClient = 1;
            result.ViewBag.IsInClient = IsInClient;
            return result;
        }
        public ActionResult IndexCheck(/*string Title, int ContractId*/)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var currentDate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            Ext.Net.MVC.PartialViewResult result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.currentDate = currentDate;
            /*result.ViewBag.Title = Title;
            result.ViewBag.ContractId = ContractId;*/
            var IsInClient = 0;
            var Req = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
            if (Req.fldKartablId == 100) IsInClient = 1;
            result.ViewBag.IsInClient = IsInClient;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCharkhe()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
        //    var q = m.prs_tblCharkheSelect("","", 0, 0).Select(c => new { Id = c.fldId, Name = c.fldName });

            var req = m.prs_tblRequestRankingSelect("fldId", (Session["RankingId"]).ToString(), 0).FirstOrDefault();
            var q = m.prs_SelectNextAction_CharkheEghdam(req.fldCharkheId, req.fldActionId).Select(c => new { Id = c.fldEghdamId, Name = c.fldNameKartabl });
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetProject()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblRegistrationFirstContractSelect("fldRequestId", (Session["RankingId"]).ToString(), 0).Select(c => new { Id = c.fldId, Name = c.fldTitle });
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveEnterCycles(int ContractId,int CharkheId)
        {
            if (Session["FristRegisterId"] == null)
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
               string CherkheName = "", Name = "", CodeMeli = "";
               var IsInClient = 0;
                try
                {
                    var Req = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
                    if (Req.fldKartablId != 100)
                    {
                        return Json(new
                        {
                            MsgTitle = "خطا",
                            Msg = "امکان ارسال پرونده موردنظر وجود ندارد.",
                            Er = 1,
                            IsInClient = 0
                        });
                    }

                    var NameCharkhe = "";
                    var q = m.prs_tblRegistrationFirstContractSelect("fldId", ContractId.ToString(), 0).FirstOrDefault();
                    var req = m.prs_tblRequestRankingSelect("fldId", q.fldRequestId.ToString(), 0).FirstOrDefault();
                var z = m.prs_tblRegistrationFirstContract_HoghoghiSelect("fldHeaderId", ContractId.ToString(), 0).ToList();
                int teadadvagon = 0;
                foreach (var item in z)
                    teadadvagon = teadadvagon + item.fldTedadVagon;
                
                if (q.ExistsNullFile == 0)
                {
                    Msg = "تمامی فایل های مربوط به این پروژه بارگزاری نشده است.";
                    MsgTitle = "خطا";
                    Er = 1;
                }
                else if (teadadvagon<q.fldTedad)
                {
                    Msg = "مالک تمامی واگن های مربوط به این پروژه مشخص نشده است.";
                    MsgTitle = "خطا";
                    Er = 1;
                }
                else if (teadadvagon > q.fldTedad)
                {
                    Msg = "تعداد واگن های درخواست شده با مجموع واگن های مالکان مطابقیت ندارد.";
                    MsgTitle = "خطا";
                    Er = 1;
                }
                else
                {
                    //NameCharkhe = m.prs_tblCharkheSelect("fldId", CharkheId.ToString(), 0, 0).FirstOrDefault().fldName;
                    //Dictionary<string, object> parameters = new Dictionary<string, object>();
                    //parameters.Add("عنوان", q.fldTitle);
                    //parameters.Add("نام چرخه", NameCharkhe);
                    //string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    //m.prs_InsertEnterToCycle_CharkheFirstEghdam(ContractId, CharkheId, null, "", user.UserInputId, jsonstr);

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("توضیحات", "کنترل نهایی وارسال");
                    parameters.Add("نام چرخه", req.fldCharkheName);
                    parameters.Add("عنوان", q.fldTitle);
                    parameters.Add("نام اقدام", req.fldActionName);
                    parameters.Add("نام کارتابل ", req.fldNameKartabl);
                    // parameters.Add("کد جدول ورود و خروج", EtmamCharkhe.fldInputId);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);


                    System.Data.Entity.Core.Objects.ObjectParameter fldId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblTaeedMarhaleInsert(fldId, req.fldActionId, req.fldKartablId, "کنترل نهایی وارسال", user.UserInputId, jsonstr);

                    int EghdamBadi = 0;
                    var next = m.prs_SelectNextAction_CharkheEghdam(req.fldCharkheId, req.fldActionId).FirstOrDefault();
                    if (next != null)
                        EghdamBadi = next.fldEghdamId;

                    parameters = new Dictionary<string, object>();
                    parameters.Add("نام اکشن", EghdamBadi);
                    //parameters.Add("نام چرخه", req.EnterCycle);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    var d=m.prs_UpdateCharkhFirstTableActions("TaeedMarhale", (fldId.Value).ToString(), EghdamBadi, req.fldEnterCycleId, user.UserInputId, "", jsonstr).FirstOrDefault();

                    var l = m.prs_ListNameShakhInCartabl("TaeedMarhale", Convert.ToInt32(fldId.Value)).ToList();

                    var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                    RaiSms.Service w = new RaiSms.Service();
                    var Matn = "به کارتابل شما در سامانه ماده12، پرونده ارجاع داده شده است. لطفا برای بررسی به سامانه مراجعه فرمایید.";
                    var ll = m.prs_ListNameShakhInCartabl("TaeedMarhale", Convert.ToInt32(fldId.Value)).ToList();

                    foreach (var itemm in ll)
                    {
                        m.prs_tblSafSMSInsert(Matn, 1, itemm.fldShakhsId, d.fldIdChecrkheFirst, user.UserInputId);

                        if (itemm.fldMobile != "")
                        {
                            var returnCode = w.SendSms(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, itemm.fldMobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");

                            if (returnCode.Length <= 3)
                            {
                                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                                m.prs_tblErrorInsert(ErrorId, "ارسال پیامک برای کارتابل: "+w.ShowError(returnCode, "FA"), DateTime.Now, Input.fldId, "");
                            }
                        }
                    }
                    Msg = "ورود به چرخه با موفقیت انجام شد.";
                    MsgTitle = "ورود موفق";

                   
                    Req = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
                    if (Req.fldKartablId == 100) IsInClient = 1;
                }
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
                    parameters.Add("نام و نام خانوادگی", Name);
                    parameters.Add("کدملی", CodeMeli);
                    parameters.Add("نام چرخه", CherkheName);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);



                    var Req = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
                    if (Req.fldKartablId == 100) IsInClient = 1;

                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        IsInClient = IsInClient
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    IsInClient = IsInClient
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReloadGrid2()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });


            Models.RaiSamEntities m = new RaiSamEntities();

            var req = m.prs_tblRequestRankingSelect("fldId", (Session["RankingId"]).ToString(), 0).FirstOrDefault();


            var RequestID = Session["RankingId"].ToString();
            var FisrtRegister = Session["FristRegisterId"].ToString();
            var HadafId = req.fldHadafId.ToString();
            var CharkheId = req.fldCharkheId;
            var ActionId = req.fldActionId;
            var Functions = "";


            Functions = Functions + "MadarkeOmumi;" + "MadarkeOmumiP;" + "MadarkeEkhtesasiP;";
            

           
            return Json(new
            {
                RequestID = RequestID,
                FisrtRegister = FisrtRegister,
                CharkheId=CharkheId,
                HadafId = HadafId,
                ActionId=ActionId,
                Functions = Functions
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MadarkeOmumi(string RequestID, string CharkheId,string ActionId, string HadafId, int end)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Models.RaiSamEntities m = new RaiSamEntities();
            var d = m.prs_HaveItemDynamic(Convert.ToInt32(RequestID), Convert.ToInt32(HadafId), 1, Convert.ToInt32(CharkheId), Convert.ToInt32(ActionId)).FirstOrDefault();
            Models.FinalCheckGrid b = new Models.FinalCheckGrid();
            b.FnTitle = "MadarkeOmumi";
            if (d.Resulte == false)
            {
                b.status = 0;
                b.name = "ثبت مدارک عمومی شرکت ضروری است.";
            }
            else if (d.Resulte == true)
            {
                b.status = 1;
                b.name = "تمامی مدارک عمومی شرکت ثبت شده است.";
            }
            else
            {
                b.status = 2;
                b.name = "موردی برای ثبت در مدارک عمومی شرکت در این مرحله وجود ندارد.";
            }
            if (b != null)
                groupsGrid.Add(b);
            return Json(new
            {
                GridData = groupsGrid.ToList(),
                end = end
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MadarkeOmumiP(string RequestID, string CharkheId, string ActionId, string HadafId, int end)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Models.RaiSamEntities m = new RaiSamEntities();
            var d = m.prs_HaveItemDynamic(Convert.ToInt32(RequestID), Convert.ToInt32(HadafId), 2, Convert.ToInt32(CharkheId), Convert.ToInt32(ActionId)).FirstOrDefault();
            Models.FinalCheckGrid b = new Models.FinalCheckGrid();
            b.FnTitle = "MadarkeOmumi";
            if (d.Resulte == false)
            {
                b.status = 0;
                b.name = "ثبت مدارک عمومی پروژه ضروری است.";
            }
            else if (d.Resulte == true)
            {
                b.status = 1;
                b.name = "تمامی مدارک عمومی پروژه ثبت شده است.";
            }
            else
            {
                b.status = 2;
                b.name = "موردی برای ثبت در مدارک اختصاصی شرکت در این مرحله وجود ندارد.";
            }
            if (b != null)
                groupsGrid.Add(b);
            return Json(new
            {
                GridData = groupsGrid.ToList(),
                end = end
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MadarkeEkhtesasiP(string RequestID, string CharkheId, string ActionId, string HadafId, int end)
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Models.RaiSamEntities m = new RaiSamEntities();
            var d = m.prs_HaveItemDynamic(Convert.ToInt32(RequestID), Convert.ToInt32(HadafId), 3, Convert.ToInt32(CharkheId), Convert.ToInt32(ActionId)).FirstOrDefault();
            Models.FinalCheckGrid b = new Models.FinalCheckGrid();
            b.FnTitle = "MadarkeOmumi";
            if (d.Resulte == false)
            {
                b.status = 2;
                b.name = "مدارک اختصاصی پروژه ثبت نشده است.";
            }
            else if (d.Resulte == true)
            {
                b.status = 1;
                b.name = "تمامی مدارک اختصاصی پروژه ثبت شده است.";
            }
            else
            {
                b.status = 2;
                b.name = "موردی برای ثبت در مدارک اختصاصی پروژه در این مرحله وجود ندارد.";
            }
            if (b != null)
                groupsGrid.Add(b);
            return Json(new
            {
                GridData = groupsGrid.ToList(),
                end = end
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
