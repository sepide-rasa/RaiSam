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
using System.Threading;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Xml;
using System.Data.SqlClient;
using System.Data;

namespace RaiSam.Controllers.BasicInfo
{
    public class VagonController : Controller
    {
        //
        // GET: /Vagon/
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
        public ActionResult New(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
      
        public ActionResult Malek(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTypeVagon()
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
                var q =m.prs_tblTypeVagonSelect("","",0).ToList().Select(c => new { fldId = c.fldId, fldName = c.fldTitleVagon });
                return this.Store(q);
            }
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpVagon()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "31", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinVagon()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoVagon()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "31", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
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
                var q = m.prs_tblVagonSelect("fldId", Id.ToString(), "", "", 0).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldTypeVagonId = q.fldTypeVagonId.ToString(),
                    fldTypeVagon=q.fldTypeVagon,
                    fldKarbariVagon=q.fldKarbariVagon.ToString(),
                    fldKarbariVagonName=q.fldKarbariVagonName,
                    fldMalek_InfoId=q.fldMalek_InfoId,
                    fldNameCompany=q.fldNameCompany,
                    fldShomareVagon=q.fldShomareVagon,
                    fldToolVagon=q.fldToolVagon,
                    fldVaznKhali=q.fldVaznKhali,
                    fldTimeStamp = q.fldTimeStamp,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "", title = "";
                title = m.prs_tblVagonSelect("fldId", id.ToString(), "", "", 0).FirstOrDefault().fldShomareVagon;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ثبت واگن", title);
                try
                {
                    if (Permission.haveAccess(180, "tblVagon", id.ToString()))
                    {
                        var q = m.prs_tblVagonDelete(id, TimeStamp).FirstOrDefault();

                        if (q.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
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
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }

                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "Vagon:Delete: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblVagon", jsonstr, false, id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblVagon", jsonstr, true, id);
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Vagon:Delete: " + ErMsg);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblVagon", jsonstr, false, id);
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
                    Change = Change,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblVagonSelect Group)
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
                var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    parameters.Add("شماره واگن", Group.fldShomareVagon);
                    parameters.Add("کاربری واگن", Group.fldKarbariVagonName);
                    parameters.Add("طول واگن", Group.fldToolVagon);
                    parameters.Add("مالک", Group.fldNameCompany);
                    parameters.Add("نوع واگن", Group.fldTypeVagon);
                    parameters.Add("وزن", Group.fldVaznKhali);
                    if (Group.fldId == 0)
                    {
                        if (Permission.haveAccess(178, "tblVagon", null))
                        {
                            
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblVagonInsert(Group.fldTypeVagonId, Group.fldShomareVagon, Group.fldVaznKhali, Group.fldToolVagon, Group.fldKarbariVagon,Group.fldExt_ShomareVagon, Group.fldZarfiyatBargiri, user.UserInputId, jsonstr);

                            Msg = "ذخیره با موفقیت انجام شد.";
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
                        if (Permission.haveAccess(179, "tblVagon", Group.fldId.ToString()))
                        {
                            var q = m.prs_tblVagonUpdate(Group.fldId, Group.fldTypeVagonId, Group.fldShomareVagon, Group.fldVaznKhali, Group.fldToolVagon, Group.fldKarbariVagon, user.UserInputId, Group.fldTimeStamp).FirstOrDefault();

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

                          

                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Vagon:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblVagon", jsonstr, false, Group.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblVagon", jsonstr, true, Group.fldId);
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

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("شماره واگن", Group.fldShomareVagon);
                    parameters.Add("کاربری واگن", Group.fldKarbariVagonName);
                    parameters.Add("طول واگن", Group.fldToolVagon);
                    parameters.Add("مالک", Group.fldNameCompany);
                    parameters.Add("نوع واگن", Group.fldTypeVagon);
                    parameters.Add("وزن", Group.fldVaznKhali);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Vagon:Save: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (Group.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "dbo.tblVagon", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblVagon", jsonstr, false, Group.fldId);
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
                if (Permission.haveAccess(177, "", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblVagonSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblVagonSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTypeVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTypeVagon";
                                    break;
                                case "fldNameCompany":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameCompany";
                                    break;
                                case "fldShomareVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldShomareVagon";
                                    break;
                                case "fldKarbariVagonName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldKarbariVagonName";
                                    break;
                                case "fldToolVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldToolVagon";
                                    break;
                                case "fldVaznKhali":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldVaznKhali";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblVagonSelect(field, searchtext, "", "", 100).ToList();
                            else
                                data = m.prs_tblVagonSelect(field, searchtext, "", "", 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblVagonSelect("", "", "", "", 100).ToList();
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

                    List<prs_tblVagonSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult GetTypeMalek()
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
                var q = m.prs_tblTypeMalekiyatSelect("", "", 0).ToList().Select(c => new { fldId = c.fldId, fldName = c.fldTitle });
                return this.Store(q);
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ReadMaleks(StoreRequestParameters parameters, string vagonId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                //if (Permission.haveAccess(177, "", "0"))
                //{
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblVagon_MalekSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblVagon_MalekSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblVagon_MalekSelect(field, searchtext, 100).ToList();
                            else
                                data = m.prs_tblVagon_MalekSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblVagon_MalekSelect("fldVagonId", vagonId, 100).ToList();
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

                    List<prs_tblVagon_MalekSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsMalekVagon(int Id)
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
                var q = m.prs_tblVagon_MalekSelect("fldId", Id.ToString(),  0).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldTypeMalekiyatId = q.fldTypeMalekiyatId.ToString(),
                    fldMalekInfoId = q.fldMalekInfoId,
                    fldAzTarikh = q.fldAzTarikh,
                    fldTatarikh = q.fldTatarikh,
                    fldVagonId = q.fldVagonId,
                    fldTitleMalekiyat=q.fldTitleMalekiyat,
                    fldNameMalek=q.fldNameMalek
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMalekVagon(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();

                try
                {
                    //if (Permission.haveAccess(180, "tblVagon", id.ToString()))
                    //{
                     m.prs_tblVagon_MalekDelete(id);

                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        
                       
                    //}
                    //else
                    //{
                    //    return Json(new
                    //    {
                    //        Msg = "شما مجاز به دسترسی نمی باشید.",
                    //        MsgTitle = "خطا",
                    //        Er = 1
                    //    }, JsonRequestBehavior.AllowGet);
                    //}

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
        public ActionResult SaveMalekVagon(prs_tblVagon_MalekSelect malek)
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
                var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    int? fldAzTarikh = null; int? fldTatarikh = null;

                    if (malek.fldTatarikh != null)
                        fldTatarikh = Convert.ToInt32(malek.fldTatarikh.Replace("/", ""));
                    if (malek.fldAzTarikh != null)
                        fldAzTarikh = Convert.ToInt32(malek.fldAzTarikh.Replace("/", ""));

                    if (malek.fldId == 0)
                    {
                        if (Permission.haveAccess(178, "tblVagon", null))
                        {

                            m.prs_tblVagon_MalekInsert(malek.fldVagonId, malek.fldMalekInfoId, malek.fldTypeMalekiyatId, fldAzTarikh, fldTatarikh, user.UserInputId);

                            Msg = "ذخیره با موفقیت انجام شد.";
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
                        if (Permission.haveAccess(179, "tblVagon", malek.fldId.ToString()))
                        {
                            m.prs_tblVagon_MalekUpdate(malek.fldId, malek.fldVagonId, malek.fldMalekInfoId, malek.fldTypeMalekiyatId, fldAzTarikh, fldTatarikh, user.UserInputId);

                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                            
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
        string GetToken()
        {
            string requestUri = "https://externalapi.rai.ir/api/v1/Users/TokenBody";
            var bodyContent = new
            {
                username = "Made12_PCS",
                password = "Made12_PCS.IT.14020708.583.PO2!qaS32a1Yu6h#v9bg$ed330",
                grant_type = "password"
            };
            var myJson = JsonConvert.SerializeObject(bodyContent);


            HttpClient client = new HttpClient();
            var result = new StringContent(myJson.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(requestUri, result).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            var token = json.access_token;
            return token;
        }
        public ActionResult InsertSajamData(string FromDate, string ToDate)
        {
            UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var Er = 0; var Message = "عملیات با موفقیت انجام شد.";
                MoveTranDetails d = null;
            try
            {
                var EndIns = 0;
                string Az=FromDate;
                while (EndIns == 0)
                {
                    string Ta = MyLib.Shamsi.ShamsiAddDay(Az, 30);
                    if (MyLib.Shamsi.Shamsi2miladiDateTime(Ta) >= MyLib.Shamsi.Shamsi2miladiDateTime(ToDate))
                    {
                        Ta = ToDate;
                        EndIns = 1;
                    }
                    var MoveTran = SejamService.GetMoveTranDetailsInApp(Az, Ta, null, null);

                    List<MoveTranDetails> a = new List<MoveTranDetails>();
                    if (MoveTran.Err == 0)
                    {
                        foreach (var item in MoveTran.data)
                        {

                            if (/*item.PelakNo != 0 && */item.SalonNo != 0)
                                m.prs_InsertFromWebServiceSajam(item.TranNo, item.MoveDate, item.MoveTime, item.PelakNo, item.SalonNo, item.SourceStation, item.TargetStation);

                        }
                    }
                    else
                        {
                            Er = MoveTran.Err;
                            Message = MoveTran.Msg;
                        }

                    Az = MyLib.Shamsi.ShamsiAddDay(Ta, 1);
                }
                return Json(new
                {
                    Er = Er,
                    Message = Message
                }, JsonRequestBehavior.AllowGet);
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
                    Er = 1,
                    Message = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید."
                }, JsonRequestBehavior.AllowGet);

            }
                
     


        }


         public void CallWebService()
        {


    
            //Reading input values from console
            string a = "railway";
            string b = "321987";
            //string a = "s";
            //string b = "e";
            //Calling InvokeService method
            InvokeService(a, b);
        }
         public void InvokeService(string a, string b)
        {
            //Calling CreateSOAPWebRequest method
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
             <soap:Body>
                <wbsLogin xmlns=""http://tempuri.org/"">
                  <username>" + a + @"</username>
                  <password>" + b + @"</password>
                </wbsLogin>
              </soap:Body>
            </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Uploaded\rajaOut.txt", ServiceResult );
                    Console.WriteLine(ServiceResult);
                    Console.ReadLine();
                }
            }
        }
         public HttpWebRequest CreateSOAPWebRequest()
         {
             //Making Web Request
             //HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://madeh12.rai.ir/SejamService.asmx");
             HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://webservices.raja.ir/online2Services.asmx");
             //SOAPAction
             //Req.Headers.Add(@"SOAPAction:http://tempuri.org/TestService");
             Req.Headers.Add(@"SOAPAction:http://tempuri.org/wbsLogin");
             //Content_type
             Req.ContentType = "text/xml;charset=\"utf-8\"";
             Req.Accept = "text/xml";
             //HTTP method
             Req.Method = "POST";
             //return HttpWebRequest
             return Req;
         }
      
      
    }
}
