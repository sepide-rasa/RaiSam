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
    public class RulesController : Controller
    {
        //
        // GET: /Rules/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
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
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpRules()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "18", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinRules()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoRules()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "18", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
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
                if (Permission.haveAccess(140,"","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblRulesSelect> data = null;
                    data = m.prs_tblRulesSelect("","",0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblRulesSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }

        public ActionResult New(int HeaderId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            var CurrentDate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            PartialView.ViewBag.CurrentDate = CurrentDate;
            PartialView.ViewBag.HeaderId = HeaderId;
            return PartialView;
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
                var q = m.prs_tblCharkheSelect("","",u.UserSecondId,0).ToList().Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
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
                string Msg = "", MsgTitle = ""; byte Er = 0;  var Change = 0;
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var q = m.prs_tblRulesSelect("fldId", HeaderId.ToString(),0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نام چرخه", q.fldNameCharkhe);
                parameters.Add("توضیحات", q.fldDesc);
                parameters.Add("تاریخ اجرا", q.fldTarikhEjra);
                try
                {//حذف
                    if (Permission.haveAccess(143, "dbo.tblRules", HeaderId.ToString()))
                    {
                        var s = m.prs_tblRulesDelete(HeaderId, TimeStamp).FirstOrDefault();
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
                            parameters.Add("متن خطا", "Rules:Delete: " + Msg);
                            Logstatus = false;
                        }

                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblRules", jsonstr, Logstatus, HeaderId);
                    }
                    else
                    {
                        return Json(new
                        {
                            Msg = "شما مجاز به دسترسی نمی باشید.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }

                   

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


                    m.prs_LogDelete(u.UserInputId, "dbo.tblRules", jsonstr, false, HeaderId);
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
                var q = m.prs_tblRulesSelect("fldId", Id.ToString(),1).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldTarikhEjra = q.fldTarikhEjra,
                    fldCharkheId = q.fldCharkheId,
                    fldDesc = q.fldDesc,
                    fldTimeStamp = q.fldTimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult ReadDetail(StoreRequestParameters parameters, int HeaderId)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    if (Permission.haveAccess(110))
        //    {
        //        UserInfo user = (UserInfo)(Session["User"]);
        //        List<OBJ_RulesDetail> data = null;
        //        param.FieldName = "fldHeaderId";
        //        param.Value = HeaderId.ToString();
        //        param.UserInfoId = user.UserInputId;
        //        param.UserId = user.UserId;
        //        param.Top = 0;
        //        data = service.SelectRulesDetail(param, user.UserKey, IP).listRulesDetail;
        //        //-- start paging ------------------------------------------------------------
        //        int limit = parameters.Limit;

        //        if ((parameters.Start + parameters.Limit) > data.Count)
        //        {
        //            limit = data.Count - parameters.Start;
        //        }

        //        List<OBJ_RulesDetail> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
        //        //-- end paging ------------------------------------------------------------

        //        return this.Store(rangeData, data.Count);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error", "Home", new { Code = "403" });
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblRulesSelect RulesHeader)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var jsonstr = "";
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0; bool CheckRepeatedRow = false; bool CheckTarikh = false; 
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نام چرخه", RulesHeader.fldNameCharkhe);
                    parameters.Add("توضیحات", RulesHeader.fldDesc);
                    parameters.Add("تاریخ اجرا", RulesHeader.fldTarikhEjra);

                    if (RulesHeader.fldId == 0)
                    {
                        if (Permission.haveAccess(141, "dbo.tblRules", null))
                        {

                            RulesHeader.fldFormulId = 0;
                            RulesHeader.fldFormul = "";
                            if (RulesHeader.fldDesc == null)
                                RulesHeader.fldDesc = "";

                            CheckRepeatedRow = m.prs_tblRulesSelect("fldTarikhEjra", RulesHeader.fldTarikhEjra, 0).Where(l => l.fldCharkheId == RulesHeader.fldCharkheId).Any();
                            var q2 = m.prs_tblRulesSelect("LastfldTarikhEjra", RulesHeader.fldCharkheId.ToString(), 0).FirstOrDefault();
                            if (q2 != null && MyLib.Shamsi.Shamsi2miladiDateTime(RulesHeader.fldTarikhEjra) < MyLib.Shamsi.Shamsi2miladiDateTime(q2.fldTarikhEjra))
                                CheckTarikh = true;

                            if (CheckRepeatedRow)
                            {
                                Msg = "تاریخ اجرا وارد شده تکراری است";
                                MsgTitle = "خطا";
                                Er = 1;

                                parameters.Add("متن خطا", "Rules:Save: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogInsert(u.UserInputId, "dbo.tblRules", jsonstr, false);
                            }
                            else if (CheckTarikh)
                            {
                                Msg = "تاریخ نمیتواند قبل از آخرین تاریخ اجرا باشد";
                                MsgTitle = "خطا";
                                Er = 1;

                                parameters.Add("متن خطا", "Rules:Save: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogInsert(u.UserInputId, "dbo.tblRules", jsonstr, false);
                            }

                            else
                            {
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                                m.prs_tblRulesInsert(RulesHeader.fldFormul, RulesHeader.fldFormulId, RulesHeader.fldDesc, u.UserInputId, Convert.ToInt32(RulesHeader.fldTarikhEjra.Replace("/", "")), RulesHeader.fldCharkheId, jsonstr);

                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (Permission.haveAccess(142, "dbo.tblRules", RulesHeader.fldId.ToString()))
                        {
                        var R = m.prs_tblRulesSelect("fldTarikhEjra", RulesHeader.fldTarikhEjra, 0).Where(l => l.fldCharkheId == RulesHeader.fldCharkheId).FirstOrDefault();
                        if (R != null && R.fldId != RulesHeader.fldId)
                            CheckRepeatedRow = true;
                        var q2 = m.prs_tblRulesSelect("LastfldTarikhEjra", RulesHeader.fldCharkheId.ToString(), 0).FirstOrDefault();
                        if (q2 != null && q2.fldId != RulesHeader.fldId && MyLib.Shamsi.Shamsi2miladiDateTime(RulesHeader.fldTarikhEjra) < MyLib.Shamsi.Shamsi2miladiDateTime(q2.fldTarikhEjra))
                            CheckTarikh = true;

                        if (CheckRepeatedRow)
                        {
                            Msg = "تاریخ اجرا وارد شده تکراری است";
                            MsgTitle = "خطا";
                            Er = 1;

                            parameters.Add("متن خطا", "Rules:Edit: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblRules", jsonstr, false, RulesHeader.fldId);
                        }
                        else if (CheckTarikh)
                        {
                            Msg = "تاریخ نمیتواند قبل از آخرین تاریخ اجرا باشد";
                            MsgTitle = "خطا";
                            Er = 1;

                            parameters.Add("متن خطا", "Rules:Edit: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblRules", jsonstr, false, RulesHeader.fldId);
                        }
                        else
                        {
                            if (RulesHeader.fldDesc == null)
                                RulesHeader.fldDesc = "";
                            var s = m.prs_tblRulesUpdate(RulesHeader.fldId, RulesHeader.fldDesc, Convert.ToInt32(RulesHeader.fldTarikhEjra.Replace("/", "")), RulesHeader.fldCharkheId, u.UserInputId, RulesHeader.fldTimeStamp).FirstOrDefault();
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
                                parameters.Add("متن خطا", "Rules:Edit: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblRules", jsonstr, Logstatus, RulesHeader.fldId);
                        }

                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
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
                    parameters.Add("نام چرخه", RulesHeader.fldNameCharkhe);
                    parameters.Add("توضیحات", RulesHeader.fldDesc);
                    parameters.Add("تاریخ اجرا", RulesHeader.fldTarikhEjra);
                    parameters.Add("کد خطا", ErrorId.Value);
                    if (RulesHeader.fldId == 0)
                    {
                        parameters.Add("متن خطا", "Rules:Save: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogInsert(u.UserInputId, "dbo.tblRules", jsonstr, false);
                    }
                    else
                    {
                        parameters.Add("متن خطا", "Rules:Edit: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblRules", jsonstr, false, RulesHeader.fldId);
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
                    Er = Er,
                    Change = Change
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FormulSaz(/*int PayeId, int HeaderId, int FormulId,*/ int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            ViewData.Model = new Models.Parameters();
            PartialView.ViewData = ViewData;
            //PartialView.ViewBag.PayeId = PayeId.ToString();
            //PartialView.ViewBag.HeaderId = HeaderId.ToString();
            //PartialView.ViewBag.FormulId = FormulId.ToString();
            PartialView.ViewBag.fldId = fldId.ToString();
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsFormul(int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string FarsiFormula = "";
                string Formul = "";
                Models.RaiSamEntities m = new RaiSamEntities();

                var q = m.prs_tblRulesSelect("fldId", fldId.ToString(),1).FirstOrDefault();
                var s = q.fldFormul.Split(';');
                for (int i = 0; i < s.Length - 1; i++)
                {
                    //if (s[i] == "if(" || s[i] == "then" || s[i] == "else")
                    //{
                    //    FarsiFormula = FarsiFormula + s[i] + ";";
                    //    Formul = Formul + s[i];
                    //}
                    //else
                    //{

                    var f = m.prs_tblAvamelMoasser_UpgradeSelect("fldEnglishNameFormul", s[i],1).FirstOrDefault();
                    if (f != null)
                    {
                        FarsiFormula = FarsiFormula + f.fldName + ";";
                        Formul = Formul + f.fldName;
                    }
                    else
                    {
                        var f1 = m.prs_tblDegreeOfEducationSelect("fldNameEn", s[i],1).FirstOrDefault();
                        if (f1 != null)
                        {
                            FarsiFormula = FarsiFormula + f1.fldTitle + ";";
                            Formul = Formul + f1.fldTitle;
                        }
                        else
                        {
                            FarsiFormula = FarsiFormula + s[i] + ";";
                            Formul = Formul + s[i];
                        }
                    }
                    //}
                }
                FarsiFormula = FarsiFormula.Replace("if(", "اگر(").Replace("then", "آنگاه ").Replace("else", "درغیراین صورت ");
                Formul = Formul.Replace("if(", "اگر(").Replace("then", "آنگاه ").Replace("else", "درغیراین صورت ");
                return Json(new
                {
                    Formul = Formul,
                    FarsiFormula = FarsiFormula,
                    EnglishFormula = q.fldFormul
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalcFormul(string Formul)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string result = "";
                string temp = "";
                Models.RaiSamEntities m = new RaiSamEntities();

                Formul = Formul.Replace("÷", "/");
                int Count1 = Formul.Count(k => k == ')');
                int Count2 = Formul.Count(k => k == '(');
                int Count3 = Formul.Count(k => k == ']');
                int Count4 = Formul.Count(k => k == '[');
                if (Count1 != Count2 || Count3 != Count4)
                {
                    return Json(new { Er = 1, MsgTitle = "خطا", Msg = "تعداد پرانتزهای باز شده با تعداد پرانتزهای بسته شده یکی نیست." }, JsonRequestBehavior.AllowGet);
                }

                var s = Formul.Split(';');
                for (int i = 0; i < s.Length - 1; i++)
                {
                    if (s[i] == "then" || s[i] == "else")
                    {
                        s[i] = ",";
                    }
                    else
                    {
                        var f = m.prs_tblAvamelMoasser_UpgradeSelect("fldEnglishNameFormul", s[i],1).FirstOrDefault();
                        if (f != null)
                        {
                            s[i] = f.fldId.ToString();
                        }
                        else
                        {
                            var dd = m.prs_tblDegreeOfEducationSelect("fldNameEn", s[i],1).FirstOrDefault();
                            if (dd != null)
                            {
                                s[i] = dd.fldId.ToString();
                            }
                        }
                    }
                    temp = temp + s[i];
                }
                var sss = Summary(temp).Replace('[', '(').Replace(']', ')');
                result = Calculate(sss).ToString();
                return Json(new { Er = 0, FResult = Convert.ToInt64(result.Split('.')[0]) }, JsonRequestBehavior.AllowGet);
            }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFormul(prs_tblRulesSelect Rules)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0;
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    m.prs_UpdateFormulRules("Rule", Rules.fldFormul, Rules.fldId);
             
                    Msg = "ذخیره فرمول با موفقیت انجام شد";
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
        public ActionResult ReadFactorsAffacting()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(136, "tblAvamelMoasser_Upgrade", "0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblAvamelMoasser_UpgradeSelect> data = null;
                    data = m.prs_tblAvamelMoasser_UpgradeSelect("","",0).ToList();
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
        public ActionResult ReadDegreeEducation()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                //if (Permission.haveAccess(40))
                //{
                Models.RaiSamEntities m = new RaiSamEntities();
                List<prs_tblDegreeOfEducationSelect> data = null;
                data = m.prs_tblDegreeOfEducationSelect("","",0).ToList();
                return this.Store(data);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }

    }
}
