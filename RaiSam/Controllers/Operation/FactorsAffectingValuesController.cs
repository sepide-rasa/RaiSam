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
    public class FactorsAffectingValuesController : Controller
    {
        //
        // GET: /FactorsAffectingValues/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new FactorsAffectingValues();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                ViewData = ViewData,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;
        }
        public ActionResult Help()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpFactorsAffectingValues()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "17", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinFactorsAffecting()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoFactorsAffecting()
        {
            if (Session["User"] == null)
                return null;

            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "17", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult New(string DegreeIds, int CharkheId, string fldTarikh)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.DegreeIds = DegreeIds;
            PartialView.ViewBag.CharkheId = CharkheId;
            PartialView.ViewBag.fldTarikh = fldTarikh;
            return PartialView;
        }
        public ActionResult New_Header(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            var CurrentDate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            PartialView.ViewBag.CurrentDate = CurrentDate;
            PartialView.ViewBag.Id = Id;
            return PartialView;
        }
        public ActionResult GetCareerBase()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblAnvaPayeSelect("","",0).Select(l => new { fldId = l.fldId, fldName = l.fldTitle });
            return this.Store(q);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCharkhe()
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
                var q = m.prs_tblCharkheSelect("","",u.UserSecondId,0).Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDegree()
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
                var q = m.prs_tblDegreeOfEducationSelect("","",0).Select(l => new { fldId = l.fldId, fldName = l.fldTitle });
                return this.Store(q);
            }
        }

        //public ActionResult GetLastDate()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    var MinDate = "";
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    param.FieldName = "LastDate";
        //    param.Value = "";
        //    param.Top = 1;
        //    var q=service.SelectFactorsAffectingValues_Header(param, user.UserKey, IP).ListHeader.FirstOrDefault();
        //    if (q != null)
        //    {
        //        MinDate = MyLib.Shamsi.Shamsi2miladiDateTime(q.fldTarikh).AddDays(1).ToString();
        //    }
        //    return Json(new
        //    {
        //        MinDate = MinDate
        //    }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(/*OBJ_FactorsAffectingValues_Header Header,*/ string DetailsArray1, int HeaderId, string EditDegreeIds, string NewDegreeIds)
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
                int AvamelUpgradeId = 0, DegreeEducationId = 0; bool checkRepeat = false;
                string jsonstr = "";
                try
                {
                    List<prs_tblFactorsAffectingValues_DetailSelect> DetailsArray = JsonConvert.DeserializeObject<List<prs_tblFactorsAffectingValues_DetailSelect>>(DetailsArray1);
                    //Header.fldInputId = user.UserInputId;
                    //Header.fldUserId = user.UserId;
                    if (EditDegreeIds == "")
                    {
                        //    //ذخیره
                        //    var output1 = service.InsertFactorsAffectingValues_Header(Header, user.UserKey, IP);
                        //    if (output1.ErrorType)
                        //    {
                        //        return Json(new
                        //        {
                        //            Er = 1,
                        //            Msg = output1.Msg,
                        //            MsgTitle = output1.MsgTitle
                        //        }, JsonRequestBehavior.AllowGet);
                        //    }
                        foreach (var item in DetailsArray)
                        {
                            AvamelUpgradeId = item.fldAvamelUpgradeId; DegreeEducationId = item.fldDegreeEducationId;//bara log


                            item.fldHeaderId = HeaderId;
                            item.fldDesc = NewDegreeIds;

                            var AvamelUpgradeName = m.prs_tblAvamelMoasser_UpgradeSelect("fldId", item.fldAvamelUpgradeId.ToString(), 0).FirstOrDefault().fldName;


                            var q = m.prs_tblFactorsAffectingValues_HeaderSelect("fldId", item.fldHeaderId.ToString(), "", 0).FirstOrDefault();


                            var DegreeEducation = m.prs_tblDegreeOfEducationSelect("fldId", item.fldDegreeEducationId.ToString(), 0).FirstOrDefault().fldTitle;
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("توضیحات", item.fldDesc);
                            parameters.Add("عامل موثر", AvamelUpgradeName);
                            parameters.Add("نام چرخه", q.fldNameCharkhe);
                            parameters.Add("تاریخ", q.fldTarikh);
                            parameters.Add("مدرک تحصیلی", DegreeEducation);
                            parameters.Add("مقدار ", item.fldValue);

                            checkRepeat = m.prs_tblFactorsAffectingValues_DetailSelect("fldAvamelUpgradeId_HeaderId", item.fldAvamelUpgradeId.ToString(), item.fldHeaderId.ToString(), 0).Where(l => l.fldDegreeEducationId == DegreeEducationId).Any();

                            if (checkRepeat == true)
                            {
                                Msg = "اطلاعات وارد شده تکراری است.";
                                MsgTitle = "خطا";
                                parameters.Add("متن خطا", "FactorsAffectingValues_Detail:save: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                                m.prs_LogInsert(u.UserInputId, "dbo.tblFactorsAffectingValues_Detail", jsonstr, false);
                            }
                            else
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                                // item.fldUserId = user.UserId;
                                m.prs_tblFactorsAffectingValues_DetailInsert(item.fldAvamelUpgradeId, item.fldHeaderId, item.fldDegreeEducationId, item.fldValue, item.fldDesc, u.UserInputId, jsonstr);
                            }


                            // item.fldUserId = user.UserId;

                        }
                    }
                    else
                    {


                        m.prs_tblFactorsAffectingValues_DetailDelete(HeaderId, "fldHeaderId", EditDegreeIds);

                        var q2 = m.prs_tblFactorsAffectingValues_DetailSelect("fldHeaderId", HeaderId.ToString(), "", 0).ToList();
                        foreach (var item in q2)
                        {
                            var AvamelUpgradeName = m.prs_tblAvamelMoasser_UpgradeSelect("fldId", item.fldAvamelUpgradeId.ToString(), 0).FirstOrDefault().fldName;


                            var q = m.prs_tblFactorsAffectingValues_HeaderSelect("fldId", item.fldHeaderId.ToString(), "", 0).FirstOrDefault();


                            var DegreeEducation = m.prs_tblDegreeOfEducationSelect("fldId", item.fldDegreeEducationId.ToString(), 0).FirstOrDefault().fldTitle;
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("توضیحات", item.fldDesc);
                            parameters.Add("عامل موثر", AvamelUpgradeName);
                            parameters.Add("نام چرخه", q.fldNameCharkhe);
                            parameters.Add("تاریخ", q.fldTarikh);
                            parameters.Add("مدرک تحصیلی", DegreeEducation);
                            parameters.Add("مقدار ", item.fldValue);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_LogDelete(u.UserInputId, "dbo.tblFactorsAffectingValues_Detail", jsonstr, true, item.fldId);
                        }

                        //param.FieldName = "fldHeaderId";
                        //param.Value = Header.fldId.ToString();
                        //param.UserInfoId = user.UserInputId;
                        //param.UserId = user.UserId;
                        //param.Top = 0;
                        //var SavedData = service.SelectFactorsAffectingValues_Detail(param, user.UserKey, IP).ListDetail;
                        //var DeleteRows = SavedData.Except(DetailsArray).ToArray();

                        //foreach (var _item in DeleteRows)
                        //{
                        //    output = service.DeleteFactorsAffectingValues_Detail("fldId",_item.fldId ,user.UserKey, IP);
                        //    if (output.ErrorType)
                        //    {
                        //        return Json(new
                        //        {
                        //            Er = 1,
                        //            Msg = output.Msg,
                        //            MsgTitle = output.MsgTitle
                        //        }, JsonRequestBehavior.AllowGet);
                        //    }
                        //}
                        foreach (var item in DetailsArray)
                        {

                            AvamelUpgradeId = item.fldAvamelUpgradeId; DegreeEducationId = item.fldDegreeEducationId;//bara log

                            item.fldHeaderId = HeaderId;
                            item.fldDesc = NewDegreeIds;
                            var AvamelUpgradeName = m.prs_tblAvamelMoasser_UpgradeSelect("fldId", item.fldAvamelUpgradeId.ToString(), 0).FirstOrDefault().fldName;


                            var q = m.prs_tblFactorsAffectingValues_HeaderSelect("fldId", item.fldHeaderId.ToString(), "", 0).FirstOrDefault();


                            var DegreeEducation = m.prs_tblDegreeOfEducationSelect("fldId", item.fldDegreeEducationId.ToString(), 0).FirstOrDefault().fldTitle;
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("توضیحات", item.fldDesc);
                            parameters.Add("عامل موثر", AvamelUpgradeName);
                            parameters.Add("نام چرخه", q.fldNameCharkhe);
                            parameters.Add("تاریخ", q.fldTarikh);
                            parameters.Add("مدرک تحصیلی", DegreeEducation);
                            parameters.Add("مقدار ", item.fldValue);

                            checkRepeat = m.prs_tblFactorsAffectingValues_DetailSelect("fldAvamelUpgradeId_HeaderId", item.fldAvamelUpgradeId.ToString(), item.fldHeaderId.ToString(), 0).Where(l => l.fldDegreeEducationId == DegreeEducationId).Any();

                            if (checkRepeat == true)
                            {
                                Msg = "اطلاعات وارد شده تکراری است.";
                                MsgTitle = "خطا";
                                parameters.Add("متن خطا", "FactorsAffectingValues_Detail:save: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                                m.prs_LogInsert(u.UserInputId, "dbo.tblFactorsAffectingValues_Detail", jsonstr, false);
                            }
                            else
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                                // item.fldUserId = user.UserId;
                                m.prs_tblFactorsAffectingValues_DetailInsert(item.fldAvamelUpgradeId, item.fldHeaderId, item.fldDegreeEducationId, item.fldValue, item.fldDesc, u.UserInputId, jsonstr);
                            }
                        }
                    }
                  
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

                    var AvamelUpgradeName = m.prs_tblAvamelMoasser_UpgradeSelect("fldId", AvamelUpgradeId.ToString(), 0).FirstOrDefault().fldName;


                    var q = m.prs_tblFactorsAffectingValues_HeaderSelect("fldId", HeaderId.ToString(), "", 0).FirstOrDefault();


                    var DegreeEducation = m.prs_tblDegreeOfEducationSelect("fldId", DegreeEducationId.ToString(), 0).FirstOrDefault().fldTitle;
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("عامل موثر", AvamelUpgradeName);
                    parameters.Add("نام چرخه", q.fldNameCharkhe);
                    parameters.Add("تاریخ", q.fldTarikh);
                    parameters.Add("مدرک تحصیلی", DegreeEducation);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogInsert(u.UserInputId, "dbo.tblFactorsAffectingValues_Detail", jsonstr, false);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveHeader(prs_tblFactorsAffectingValues_HeaderSelect Header)
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
                string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0; bool CheckRepeatedRow = false; var jsonstr = "";
                try
                {
                    //  Header.fldUserId = user.UserId;
                    if (Header.fldId == 0)
                    { //ذخیره
                        if (Header.fldDesc == null)
                            Header.fldDesc = "";

                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("توضیحات", Header.fldDesc);
                        parameters.Add("تاریخ", Header.fldTarikh);
                        parameters.Add("نام چرخه", Header.fldNameCharkhe);

                        System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));
                        CheckRepeatedRow = m.prs_tblFactorsAffectingValues_HeaderSelect("fldCharkheId_Tarikh", Header.fldCharkheId.ToString(), Header.fldTarikh, 0).Any();

                        if (CheckRepeatedRow == false)
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_tblFactorsAffectingValues_HeaderInsert(Id, Convert.ToInt32(Header.fldTarikh.Replace("/", "")), Header.fldDesc, u.UserInputId, Header.fldCharkheId, jsonstr);
                            MsgTitle = "ذخیره موفق";
                            Msg = "ذخیره با موفقیت انجام شد.";
                        }
                        else
                        {
                            Msg = "اطلاعات وارده شده تکراری است.";
                            MsgTitle = "خطا";
                            Er = 1;

                            parameters.Add("متن خطا", "FactorsAffectingValues_Header:Save: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogInsert(u.UserInputId, "dbo.tblFactorsAffectingValues_Header", jsonstr, false);
                        }
                       
                    }
                    else
                    {
                        //ویرایش
                        var A = m.prs_tblFactorsAffectingValues_HeaderSelect("fldCharkheId_Tarikh", Header.fldCharkheId.ToString(), Header.fldTarikh, 0).FirstOrDefault();
                        if (A != null && A.fldId != Header.fldId)
                            CheckRepeatedRow = true;

                        if (Header.fldDesc == null)
                            Header.fldDesc = "";

                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("توضیحات", Header.fldDesc);
                        parameters.Add("تاریخ", Header.fldTarikh);
                        parameters.Add("نام چرخه", Header.fldNameCharkhe);
                        if (CheckRepeatedRow == true)
                        {
                            Msg = "اطلاعات وارده شده تکراری است.";
                            MsgTitle = "خطا";

                            parameters.Add("متن خطا", Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblFactorsAffectingValues_Header", jsonstr, false, Header.fldId);

                        }
                        else
                        {

                            var s = m.prs_tblFactorsAffectingValues_HeaderUpdate(Header.fldId,Convert.ToInt32(Header.fldTarikh.Replace("/", "")), Header.fldDesc,Header.fldTimeStamp,Header.fldCharkheId,u.UserInputId).FirstOrDefault();


                            if (s.flag == 1)
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                            }
                            else if (s.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (s.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            var Logstatus = true;
                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "FactorsAffectingValues_Header:Edit: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblFactorsAffectingValues_Header", jsonstr, Logstatus, Header.fldId);
                        }


                    }
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
                    parameters.Add("توضیحات", Header.fldDesc);
                    parameters.Add("تاریخ", Header.fldTarikh);
                    parameters.Add("نام چرخه", Header.fldNameCharkhe);
                    parameters.Add("کد خطا", ErrorId.Value);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (Header.fldId == 0)
                    {
                        parameters.Add("متن خطا", "FactorsAffectingValues_Header:Save: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogInsert(u.UserInputId, "dbo.tblFactorsAffectingValues_Header", jsonstr, false);
                    }
                    else
                    {
                        parameters.Add("متن خطا", "FactorsAffectingValues_Header:Edit: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblFactorsAffectingValues_Header", jsonstr, false, Header.fldId);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        Change = Change
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int HeaderId, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0;
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var q = m.prs_tblFactorsAffectingValues_HeaderSelect("fldId", HeaderId.ToString(),"",0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("توضیحات", q.fldDesc);
                parameters.Add("تاریخ", q.fldTarikh);
                parameters.Add("نام چرخه", q.fldNameCharkhe);
                try
                {//حذف
                    var s = m.prs_tblFactorsAffectingValues_HeaderDelete(HeaderId, TimeStamp).FirstOrDefault();
                     if (s.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (s.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (s.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }
                        var Logstatus = true;
                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "FactorsAffectingValues_Header:Delete: " + Msg);
                            Logstatus = false;
                        }

                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblFactorsAffectingValues_Header", jsonstr, Logstatus, HeaderId);

                    

                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(u.UserInputId, "dbo.tblFactorsAffectingValues_Header", jsonstr, false, HeaderId);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        Change = Change

                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    Change = Change
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblFactorsAffectingValues_HeaderSelect("fldId", Id.ToString(),"",1).FirstOrDefault();
                //List<string> a = q.fldDegreeEducationId.Split(',').Reverse().Skip(1).Reverse().ToList();
                return Json(new
                {
                    fldId = q.fldId,
                    fldCharkheId = q.fldCharkheId,
                    fldDesc = q.fldDesc,
                    fldTarikh = q.fldTarikh,
                    fldTimeStamp = q.fldTimeStamp
                    //fldDegreeEducationId = a
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Details_Group(int Id)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFactorsAffectingValues_DetailSelect("fldId", Id.ToString(),"",1).FirstOrDefault();
            List<string> a = q.fldDesc.Split(';').Reverse().Skip(1).Reverse().ToList();
            return Json(new
            {
                fldId = q.fldId,
                fldDegreeEducationId = a
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(136, "tblFactorsAffectingValues_Header", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblFactorsAffectingValues_HeaderSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblFactorsAffectingValues_HeaderSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;
                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTarikh":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikh";
                                    break;
                                case "fldTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitle";
                                    break;
                                case "fldTitlePaye":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitlePaye";
                                    break;
                                case "fldNameCharkhe":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameCharkhe";
                                    break;
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDesc";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_tblFactorsAffectingValues_HeaderSelect(field, searchtext,"",100).ToList();
                            else
                                data = m.prs_tblFactorsAffectingValues_HeaderSelect(field, searchtext, "", 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblFactorsAffectingValues_HeaderSelect("", "", "", 100).ToList();
                    }

                    var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    //FilterConditions fc = parameters.GridFilters;

                    //-- start filtering ------------------------------------------------------------
                    if (fc != null)
                    {
                        foreach (var condition in fc.Conditions)
                        {
                            string field = condition.FilterProperty.Name;
                            var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

                            data.RemoveAll(
                                item =>
                                {
                                    object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                    return !(oValue.ToString().IndexOf(value.ToString(), StringComparison.OrdinalIgnoreCase) >= 0);
                                }
                            );
                        }
                    }
                    //-- end filtering ------------------------------------------------------------

                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblFactorsAffectingValues_HeaderSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadDetail(StoreRequestParameters parameters, int HeaderId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(136, "tblFactorsAffectingValues_Detail", "0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_SelectCheckUniqueFactors> data = null;
                    data = m.prs_SelectCheckUniqueFactors(HeaderId).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_SelectCheckUniqueFactors> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadFactorsAffecting(int HeaderId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(132,"","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var data = m.prs_SelectAllAvamelMoasser_Upgrade_Effecting(HeaderId,"").ToList();
                    return this.Store(data);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadDataFactorsAffecting(int HeaderId, string DegreeIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(132, "", "0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var data = m.prs_SelectAllAvamelMoasser_Upgrade_Effecting(HeaderId, DegreeIds).ToList();

                    int[] checkId = null;
                    var check = m.prs_tblFactorsAffectingValues_DetailSelect("fldHeaderId_Group", HeaderId.ToString(), DegreeIds,0).ToList();
                    checkId = new int[check.Count];
                    for (int i = 0; i < check.Count; i++)
                    {
                        checkId[i] = Convert.ToInt32(check[i].fldId);
                    }

                    return new JsonResult()
                    {
                        Data = new
                        {
                            checkId = checkId,
                            Data = data
                        },
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }

    }
}
