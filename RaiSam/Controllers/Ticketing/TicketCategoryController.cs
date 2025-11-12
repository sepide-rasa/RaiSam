using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;

namespace RaiSam.Controllers.Ticketing
{
    public class TicketCategoryController : Controller
    {
        //
        // GET: /TicketCategory/
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
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpTicketCategory()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect( "fldId","8",1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinTicketCategory()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoTicketCategory()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "8", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult New(int Id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblTicketCategorySelect TicketCategory)
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0; bool CheckRepeatedRow = false;

                UserInfo user = (UserInfo)(Session["User"]);
                try
                {
                    if (TicketCategory.fldDesc == null)
                        TicketCategory.fldDesc = "";

                    if (TicketCategory.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(47, "tblTicketCategory", null))
                    {
                        CheckRepeatedRow = m.prs_tblTicketCategorySelect("fldTitle_Type", TicketCategory.fldTitle, TicketCategory.fldType.ToString(), 0).Any();
                        if (CheckRepeatedRow)
                        {
                            return Json(new
                            {
                                Msg = "عنوان وارد شده تکراری است.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("عنوان", TicketCategory.fldTitle);
                            parameters.Add("توضیحات", TicketCategory.fldDesc);
                            parameters.Add("فرستنده", TicketCategory.fldTypeName);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblTicketCategoryInsert(TicketCategory.fldTitle, TicketCategory.fldType, TicketCategory.fldDesc, user.UserInputId, jsonstr);

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
                        if (Permission.haveAccess(48, "tblTicketCategory", TicketCategory.fldId.ToString()))
                        {  //ویرایش
                            var query =  m.prs_tblTicketCategorySelect("fldTitle_Type", TicketCategory.fldTitle, TicketCategory.fldType.ToString(), 0).FirstOrDefault();
                            if (query != null && query.fldId != TicketCategory.fldId)
                            {
                                return Json(new
                                {
                                    Msg = "عنوان وارد شده تکراری است.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                            var q = m.prs_tblTicketCategoryUpdate(TicketCategory.fldId, TicketCategory.fldTitle,TicketCategory.fldType,  TicketCategory.fldDesc, user.UserInputId, TicketCategory.fldTimeStamp).FirstOrDefault();


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
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                             parameters.Add("عنوان", TicketCategory.fldTitle);
                                parameters.Add("توضیحات", TicketCategory.fldDesc);
                                parameters.Add("فرستنده", TicketCategory.fldTypeName);
                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "TicketCategory:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblTicketCategory", jsonstr, false, TicketCategory.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblTicketCategory", jsonstr, true, TicketCategory.fldId);
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
                    var InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("عنوان", TicketCategory.fldTitle);
                    parameters.Add("توضیحات", TicketCategory.fldDesc);
                    parameters.Add("فرستنده", TicketCategory.fldTypeName);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "TicketCategory:Save: " + InnerException);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (TicketCategory.fldId == 0)
                    {

                        m.prs_LogInsert(user.UserInputId, "dbo.tblTicketCategory", jsonstr, false);
                    }
                    else
                    {

                        m.prs_LogUpdate(user.UserInputId, "dbo.tblTicketCategory", jsonstr, false, TicketCategory.fldId);
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
        public ActionResult Delete(int Id, int TimeStamp)
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
                Models.RaiSamEntities mo = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);


                string jsonstr = "";
                var q = mo.prs_tblTicketCategorySelect("fldId", Id.ToString(),"",1).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان", q.fldTitle);
                parameters.Add("توضیحات", q.fldDesc);
                parameters.Add("فرستنده", q.fldTypeName);
                try
                {//حذف
                     if (Permission.haveAccess(49, "tblTicketCategory", Id.ToString()))
                    {
                        var Ticket = mo.prs_tblTicketSelect("fldTicketCategoryId", Id.ToString(), "", 0).FirstOrDefault();
                        var Ticket_P = mo.prs_tblTicketPermissionSelect("fldCategoryId", Id.ToString(), "", 0).FirstOrDefault();
                        if (Ticket != null || Ticket_P != null)
                        {
                            Msg = "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.";
                            MsgTitle = "خطا";
                            Er = 1;
                        }
                        else { 
                    var m = mo.prs_tblTicketCategoryDelete(Id, TimeStamp).FirstOrDefault();

                        if (m.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (m.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (m.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }
                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "TicketCategory:delete: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            mo.prs_LogDelete(user.UserInputId, "dbo.tblTicketCategory", jsonstr, false, Id);
                        }
                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            mo.prs_LogDelete(user.UserInputId, "dbo.tblTicketCategory", jsonstr, true, Id);
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
                catch (Exception x)
                {
                    var InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = mo.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    mo.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");

                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "TicketCategory:Delete: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    mo.prs_LogDelete(user.UserInputId, "dbo.tblTicketCategory", jsonstr, false, Id);
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

                var q = m.prs_tblTicketCategorySelect("fldId", Id.ToString(),"",1).FirstOrDefault();
                var t = "0";
                if (q.fldType)
                    t = "1";
                return Json(new
                {
                    fldId = q.fldId,
                    fldTitle = q.fldTitle,
                    fldType = t,
                    fldTypeName = q.fldTypeName,
                    fldDesc = q.fldDesc,
                    fldTimeStamp = q.fldTimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();

                if (Permission.haveAccess(46, "tblTicketCategory", null))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    List<Models.prs_tblTicketCategorySelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<Models.prs_tblTicketCategorySelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitle";
                                    break;
                                case "fldTypeName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTypeName";
                                    break;
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDesc";
                                    break;
                            }
                          
                            if (data != null)
                                data1 = m.prs_tblTicketCategorySelect(field, searchtext,"",100).ToList();
                            else
                                data = m.prs_tblTicketCategorySelect(field, searchtext, "", 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblTicketCategorySelect("", "", "", 100).ToList();
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

                    List<Models.prs_tblTicketCategorySelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult SaveOrder(string Order)
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
                try
                {
                    List<OBJ_UpdateOrder> Order1 = JsonConvert.DeserializeObject<List<OBJ_UpdateOrder>>(Order);
                    foreach (var item in Order1)
                    {

                        m.prs_UpdateOrder("TicketCategory", item.fldId, item.order);

                    }
                    MsgTitle = "ویرایش با موفقیت انجام شد.";
                    Msg = "ویرایش موفق";
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
