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

namespace RaiSam.Controllers.User
{
    public class LimitationController : Controller
    {
        //
        // GET: /Limitation/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadLimitationTime(StoreRequestParameters parameters, int UserId)
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
                if (Permission.haveAccess(65,"","0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    List<Models.prs_tblLimitationTimeSelect> data = null;
                    data = m.prs_tblLimitationTimeSelect("fldUserLimId", UserId.ToString(),0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblLimitationTimeSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadLimitationIP(StoreRequestParameters parameters, int UserId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(65,"","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    UserInfo user = (UserInfo)(Session["User"]);
                    List<Models.prs_tblLimitationIPSelect> data = null;
                    data = m.prs_tblLimitationIPSelect("fldUserLimId", UserId.ToString(),0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblLimitationIPSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadLimitationMac(StoreRequestParameters parameters, int UserId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(65,"","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    UserInfo user = (UserInfo)(Session["User"]);
                    List<Models.prs_tblLimitationMacAddressSelect> data = null;
                    data = m.prs_tblLimitationMacAddressSelect("fldUserLimId", UserId.ToString(),0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblLimitationMacAddressSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }

        public ActionResult New(string UserName, int UserId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new Limitation();
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewData = ViewData;
            result.ViewBag.UserName = UserName;
            result.ViewBag.UserId = UserId.ToString();
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsTime(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblLimitationTimeSelect("fldId", Id.ToString(),1).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldRoozHafte = q.fldRoozHafte,
                    fldDesc = q.fldDesc,
                    fldTaSaat = q.fldTaSaat,
                    fldAzSaat = q.fldAzSaat,
                    fldTimestamp = q.fldTimestamp,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsIP(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblLimitationIPSelect("fldId", Id.ToString(),1).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldIPValid = q.fldIPValid,
                    fldDesc = q.fldDesc,
                    fldTimestamp = q.fldTimestamp,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsMac(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblLimitationMacAddressSelect("fldId", Id.ToString(),1).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldMacValid = q.fldMacValid,
                    fldDesc = q.fldDesc
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTime(Models.prs_tblLimitationTimeSelect LimitationTime)
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
                
                string Msg = "", MsgTitle = ""; var Er = 0;  var Change = 0;
                var Name_Family = "";
                var u = m.prs_tblUserSelect("fldId", LimitationTime.fldUserLimId.ToString(),"",0).FirstOrDefault();
                Name_Family = u.fldName + " " + u.fldFamily;
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("روز", LimitationTime.fldRoozHafte);
                    parameters.Add("از ساعت", LimitationTime.fldAzSaat);
                    parameters.Add("تا ساعت", LimitationTime.fldTaSaat);
                    parameters.Add("نام و نام خانوادگی", Name_Family);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);



                    int? AzSaat = null;
                    if (LimitationTime.fldAzSaat != null)
                    {
                        AzSaat = Convert.ToInt32(LimitationTime.fldAzSaat.Replace(":", ""));
                    }
                    int? TaSaat = null;
                    if (LimitationTime.fldTaSaat != null)
                    {
                        TaSaat = Convert.ToInt32(LimitationTime.fldTaSaat.Replace(":", ""));
                    }
                    if (LimitationTime.fldDesc == null)
                        LimitationTime.fldDesc = "";
                    if (LimitationTime.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(66, "tblLimitationTime", null))
                        {
                            m.prs_tblLimitationTimeInsert(LimitationTime.fldUserLimId, LimitationTime.fldRoozHafte, AzSaat, TaSaat, LimitationTime.fldDesc, user.UserInputId, jsonstr);

                            Msg = "ذخیره با موفقیت انجام شد";
                            MsgTitle = "دخیره موفق";
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
                        //ویرایش
                        if (Permission.haveAccess(67, "tblLimitationTime", LimitationTime.fldId.ToString()))
                        {
                            var q = m.prs_tblLimitationTimeUpdate(LimitationTime.fldId, LimitationTime.fldUserLimId, LimitationTime.fldRoozHafte, AzSaat, TaSaat, LimitationTime.fldDesc,user.UserInputId, LimitationTime.fldTimestamp).FirstOrDefault();


                            if (q.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                            }
                            else if (q.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                            parameters1.Add("روز", LimitationTime.fldRoozHafte);
                            parameters1.Add("از ساعت", LimitationTime.fldAzSaat);
                            parameters1.Add("تا ساعت", LimitationTime.fldTaSaat);
                            parameters1.Add("نام و نام خانوادگی", Name_Family);


                            if (Er == 1)
                            {
                                parameters1.Add("متن خطا", "Limitation:SaveTime: " + Msg);
                                string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblLimitationTime", jsonstr1, false, LimitationTime.fldId);
                            }

                            else
                            {
                                string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblLimitationTime", jsonstr1, true, LimitationTime.fldId);
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                    parameters1.Add("روز", LimitationTime.fldRoozHafte);
                    parameters1.Add("از ساعت", LimitationTime.fldAzSaat);
                    parameters1.Add("تا ساعت", LimitationTime.fldTaSaat);
                    parameters1.Add("نام و نام خانوادگی", Name_Family);
                    parameters1.Add("کد خطا", ErrorId.Value);
                    parameters1.Add("متن خطا", "Limitation:SaveTime: " + ErMsg);
                    string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                    if (LimitationTime.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "dbo.tblLimitationTime", jsonstr1, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblLimitationTime", jsonstr1, false, LimitationTime.fldId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveIP(Models.prs_tblLimitationIPSelect LimitationIP)
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
                string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0; ;
                var Name_Family = "";
                var u = m.prs_tblUserSelect("fldId", LimitationIP.fldUserLimId.ToString(), "", 0).FirstOrDefault();
                Name_Family = u.fldName + " " + u.fldFamily;
                try
                {

                    if (LimitationIP.fldDesc == null)
                        LimitationIP.fldDesc = "";
                    if (LimitationIP.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(66, "tblLimitationIP", null))
                        {
                            var CheckRepeatedRow = m.prs_CheckIPUnique(LimitationIP.fldUserLimId, LimitationIP.fldIPValid, 0).FirstOrDefault();//خروجی 1 باشه میتونه ذخیره کنه 
                            if (CheckRepeatedRow.fldCheck == 0)
                            {
                                Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                                parameters1.Add("IP", LimitationIP.fldIPValid);
                                parameters1.Add("نام و نام خانوادگی", Name_Family);
                                parameters1.Add("متن خطا", "Limitation:SaveIP: " + "اطلاعات وارد شده تکراری است.");
                                string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogInsert(user.UserInputId, "dbo.tblLimitationIP", jsonstr1, false);
                                return Json(new
                                {
                                    Msg = "اطلاعات وارد شده تکراری است.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                Dictionary<string, object> parameters = new Dictionary<string, object>();
                                parameters.Add("IP", LimitationIP.fldIPValid);
                                parameters.Add("نام و نام خانوادگی", Name_Family);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);


                                m.prs_tblLimitationIPInsert(LimitationIP.fldUserLimId, LimitationIP.fldIPValid, LimitationIP.fldDesc, user.UserInputId, jsonstr);

                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "دخیره موفق";
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
                        //ویرایش
                        if (Permission.haveAccess(67, "tblLimitationIP", LimitationIP.fldId.ToString()))
                        {
                            var CheckRepeatedRow = m.prs_CheckIPUnique(LimitationIP.fldUserLimId, LimitationIP.fldIPValid, LimitationIP.fldId).FirstOrDefault();//خروجی 1 باشه میتونه ذخیره کنه 
                            if (CheckRepeatedRow.fldCheck == 0)
                            {
                                Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                                parameters1.Add("IP", LimitationIP.fldIPValid);
                                parameters1.Add("نام و نام خانوادگی", Name_Family);
                                parameters1.Add("متن خطا", "Limitation:SaveIP: " + "اطلاعات وارد شده تکراری است.");
                                string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblLimitationIP", jsonstr1, false, LimitationIP.fldId);
                                return Json(new
                                {
                                    Msg = "اطلاعات وارد شده تکراری است.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var q = m.prs_tblLimitationIPUpdate(LimitationIP.fldId, LimitationIP.fldUserLimId, LimitationIP.fldIPValid, LimitationIP.fldTimestamp, user.UserInputId, LimitationIP.fldDesc).FirstOrDefault();

                                if (q.flag == 1)
                                {
                                    Msg = "ویرایش با موفقیت انجام شد";
                                    MsgTitle = "ویرایش موفق";
                                }
                                else if (q.flag == 0)
                                {
                                    Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                    MsgTitle = "هشدار";
                                    Er = 1;
                                    Change = 1;
                                }
                                else if (q.flag == 2)
                                {
                                    Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                    MsgTitle = "خطا";
                                    Er = 1;
                                    Change = 2;
                                }
                                Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                                parameters1.Add("IP", LimitationIP.fldIPValid);
                                parameters1.Add("نام و نام خانوادگی", Name_Family);


                                if (Er == 1)
                                {
                                    parameters1.Add("متن خطا", "Limitation:SaveIP: " + Msg);
                                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                                    m.prs_LogUpdate(user.UserInputId, "dbo.tblLimitationIP", jsonstr, false, LimitationIP.fldId);
                                }

                                else
                                {
                                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                                    m.prs_LogUpdate(user.UserInputId, "dbo.tblLimitationIP", jsonstr, true, LimitationIP.fldId);
                                }
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                    parameters1.Add("IP", LimitationIP.fldIPValid);
                    parameters1.Add("نام و نام خانوادگی", Name_Family);
                    parameters1.Add("کد خطا", ErrorId.Value);
                    parameters1.Add("متن خطا", "Limitation:SaveIP: " + ErMsg);
                    string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                    if (LimitationIP.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "dbo.tblLimitationIP", jsonstr1, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblLimitationIP", jsonstr1, false, LimitationIP.fldId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveMac(Models.prs_tblLimitationMacAddressSelect LimitationMac)
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
                string Msg = "", MsgTitle = ""; var Er = 0; bool CheckRepeatedRow = false;
                try
                {
                    if (LimitationMac.fldDesc == null)
                        LimitationMac.fldDesc = "";
                    if (LimitationMac.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(66, "tblLimitationMacAddress", null))
                        {
                            CheckRepeatedRow = m.prs_tblLimitationMacAddressSelect("fldMacValid", LimitationMac.fldMacValid, 0).Where(l => l.fldUserLimId == LimitationMac.fldUserLimId).Any();
                            if (CheckRepeatedRow)
                            {
                                return Json(new
                                {
                                    Msg = "اطلاعات وارد شده تکراری است.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                m.prs_tblLimitationMacAddressInsert(LimitationMac.fldUserLimId, LimitationMac.fldMacValid, LimitationMac.fldDesc, user.UserInputId);
                                Msg = "ذخیره با موفقیت انجام شد";
                                MsgTitle = "دخیره موفق";
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
                        //ویرایش
                        if (Permission.haveAccess(67, "tblLimitationMacAddress", LimitationMac.fldId.ToString()))
                        {
                            var query = m.prs_tblLimitationMacAddressSelect("fldMacValid", LimitationMac.fldMacValid, 0).Where(l => l.fldUserLimId == LimitationMac.fldUserLimId).FirstOrDefault();
                            if (query != null && query.fldId != LimitationMac.fldId)
                            {
                                return Json(new
                                {
                                    Msg = "اطلاعات وارد شده تکراری است.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                m.prs_tblLimitationMacAddressUpdate(LimitationMac.fldId, LimitationMac.fldUserLimId, LimitationMac.fldMacValid, LimitationMac.fldDesc, user.UserInputId);
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
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
        public ActionResult DeleteTime(int Id, int TimeStamp)
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                string jsonstr = "";
                var q = m.prs_tblLimitationTimeSelect("fldId", Id.ToString(),0).FirstOrDefault();

                var Name_Family = "";
                var u = m.prs_tblUserSelect("fldId", q.fldUserLimId.ToString(),"",0).FirstOrDefault();
                Name_Family = u.fldName + " " + u.fldFamily;

                try
                {//حذف
                    if (Permission.haveAccess(68, "tblLimitationTime", Id.ToString()))
                    {
                        var q1 = m.prs_tblLimitationTimeDelete(Id, TimeStamp).FirstOrDefault();


                        if (q1.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (q1.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (q1.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("روز", q.fldRoozHafte);
                        parameters.Add("از ساعت", q.fldAzSaat);
                        parameters.Add("تا ساعت", q.fldTaSaat);
                        parameters.Add("نام و نام خانوادگی", Name_Family);
                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "Limitation:DeleteTime: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblLimitationTime", jsonstr, false, Id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblLimitationTime", jsonstr, true, Id);
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
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("روز", q.fldRoozHafte);
                    parameters.Add("از ساعت", q.fldAzSaat);
                    parameters.Add("تا ساعت", q.fldTaSaat);
                    parameters.Add("نام و نام خانوادگی", Name_Family);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Limitation:DeleteTime: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblLimitationTime", jsonstr, false, Id);
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
        public ActionResult DeleteIP(int Id, int TimeStamp)
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblLimitationIPSelect("fldId", Id.ToString(),0).FirstOrDefault();
                string jsonstr = "";
                var Name_Family = "";
                var u = m.prs_tblUserSelect("fldId", q.fldUserLimId.ToString(),"",0).FirstOrDefault();
                Name_Family = u.fldName + " " + u.fldFamily;
                try
                {//حذف
                    if (Permission.haveAccess(68, "tblLimitationIP", Id.ToString()))
                    {
                        var q1 = m.prs_tblLimitationIPDelete(Id, TimeStamp).FirstOrDefault();

                        if (q1.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (q1.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (q1.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("IP", q.fldIPValid);
                        parameters.Add("نام و نام خانوادگی", Name_Family);
                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "Limitation:DeleteIP: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblLimitationIP", jsonstr, false, Id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblLimitationIP", jsonstr, true, Id);
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
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("IP", q.fldIPValid);
                    parameters.Add("نام و نام خانوادگی", Name_Family);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Limitation:DeleteIP: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblLimitationIP", jsonstr, false, Id);
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
        public ActionResult DeleteMac(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; ;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {//حذف
                    if (Permission.haveAccess(68, "tblLimitationMacAddress", Id.ToString()))
                    {
                        m.prs_tblLimitationMacAddressDelete(Id, user.UserInputId);
                        Msg = "حذف با موفقیت انجام شد";
                        MsgTitle = "حذف موفق";
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
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
