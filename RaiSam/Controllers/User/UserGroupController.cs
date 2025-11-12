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
    public class UserGroupController : Controller
    {
        //
        // GET: /UserGroup/

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
        public ActionResult CopyPermi(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var k = m.prs_tblPermissionSelect("fldUserGroupID", id.ToString(),0,0).Any();
            if (k == true)
            {
                return Json(new
                {
                    Msg = "برای گروه کاربری موردنظر قبلا دسترسی ثبت شده است.",
                    MsgTitle = "خطا",
                    Err = 1
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.KhaliId = id;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserType()
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
                
                var q = m.prs_UserTypeSelect(user.UserSecondId).ToList().Select(c => new { Id = c.Id, fldName = c.Name });
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
        public FileContentResult ShowHelpUserGroup()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var Help = m.prs_tblHelpSelect("fldId","1",1).FirstOrDefault();
            
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinUserGroup()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoUserGroup()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "1", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCopyPermi(string UserGroups, int Khali)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                
                string Msg = "", MsgTitle = ""; var Er = 0;
                try
                {
                    if (Permission.haveAccess(25,"","0"))
                    {
                        var GroupHa = UserGroups.Split(';');
                        for (int i = 0; i < GroupHa.Length - 1; i++)
                        {

                            var q = m.prs_CopyPermission(Convert.ToInt32(GroupHa[i]), Khali, user.UserSecondId, user.UserInputId);

                        }
                        MsgTitle = "ذخیره موفق";
                        Msg = "ذخیره با موفقیت انجام شد.";
                    }
                    else
                    {
                        return Json(new
                        {
                            Msg = "شما مجاز به دسترسی نمی باشید.",
                            MsgTitle = "خطا",
                            Err = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception x)
                {
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;

                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Err = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Err = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblUserGroupSelect UserGroup)
        {
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
                Models.RaiSamEntities m = new RaiSamEntities();
               
                try
                {
                    
                    if (UserGroup.fldDesc == null)
                        UserGroup.fldDesc = "";
                    if (UserGroup.fldID == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(7, "tblUserGroup", null))
                    {
                            CheckRepeatedRow = m.prs_tblUserGroupSelect("fldTitle", UserGroup.fldTitle,0, 0).Any();
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
                                parameters.Add("عنوان", UserGroup.fldTitle);
                                parameters.Add("توضیحات", UserGroup.fldDesc);
                                parameters.Add("انواع گروه کاربری", UserGroup.fldUserType_Name);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_tblUserGroupInsert(UserGroup.fldTitle, user.UserInputId, UserGroup.fldDesc, UserGroup.fldUserType, user.UserSecondId, jsonstr);

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
                        if (Permission.haveAccess(7, "tblUserGroup", UserGroup.fldID.ToString()))
                        {
                            var query = m.prs_tblUserGroupSelect("fldTitle", UserGroup.fldTitle, 0, 0).FirstOrDefault();
                            if (query != null && query.fldID != UserGroup.fldID)
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
                            var q = m.prs_tblUserGroupUpdate(UserGroup.fldID, UserGroup.fldTitle, user.UserInputId, UserGroup.fldDesc, UserGroup.fldUserType, user.UserSecondId, UserGroup.fldTimeStamp).FirstOrDefault();


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
                            parameters.Add("عنوان", UserGroup.fldTitle);
                            parameters.Add("توضیحات", UserGroup.fldDesc);
                            parameters.Add("نوع گروه کاربری", UserGroup.fldUserType_Name);
                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "UserGroup:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblUserGroup", jsonstr, false, UserGroup.fldID);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblUserGroup", jsonstr, true, UserGroup.fldID);
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
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;

                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("عنوان", UserGroup.fldTitle);
                    parameters.Add("توضیحات", UserGroup.fldDesc);
                    parameters.Add("نوع گروه کاربری", UserGroup.fldUserType_Name);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "UserGroup:Save: " + InnerException);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (UserGroup.fldID == 0)
                    {
                        
                        m.prs_LogInsert(user.UserInputId, "dbo.tblUserGroup", jsonstr,false);
                    }
                    else
                    {
                       
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblUserGroup", jsonstr, false, UserGroup.fldID);
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
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities mo = new RaiSamEntities();


                string jsonstr = "";

                var q = mo.prs_tblUserGroupSelect("fldId", id.ToString(), 0, 0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان", q.fldTitle);
                parameters.Add("توضیحات", q.fldDesc);
                parameters.Add("نوع گروه کاربری", q.fldUserType_Name);
                try
                {//حذف
                    if (Permission.haveAccess(9, "tblUserGroup", id.ToString()))
                    {
                        var p = mo.prs_tblPermissionSelect("fldUserGroupID", id.ToString(), 0, 0).ToList();
                        if (p.Count == 0)
                        {
                            var q1 = mo.prs_tblUser_GroupSelect("fldUserGroupID", id.ToString(), 0).ToList();
                            if (q1.Count == 0)
                            {
                                var m = mo.prs_tblUserGroupDelete(id, TimeStamp).FirstOrDefault();

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
                                    parameters.Add("متن خطا", Msg);
                                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                    mo.prs_LogDelete(user.UserInputId, "dbo.tblUserGroup", jsonstr, false, id);
                                }

                                else
                                {
                                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                    mo.prs_LogDelete(user.UserInputId, "dbo.tblUserGroup", jsonstr, true, id);
                                }
                            }
                            else
                            {
                                return Json(new
                                {
                                    Msg = "برای گروه کاربری مورد نظر کاربر ثبت شده است و امکان حذف وجود ندارد.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "برای گروه کاربری مورد نظر دسترسی ثبت شده است و امکان حذف وجود ندارد.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
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
                    var Input = mo.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    mo.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");

                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "UserGroup:Delete: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);


                    mo.prs_LogDelete(user.UserInputId, "dbo.tblUserGroup", jsonstr, false, id);
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
                
                var q = m.prs_tblUserGroupSelect("fldId", Id.ToString(), user.UserSecondId,1).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldID,
                    fldTitle = q.fldTitle,
                    fldUserType = q.fldUserType.ToString(),
                    fldDesc = q.fldDesc,
                    fldUserID = q.fldInputID,
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
                if (Permission.haveAccess(6, "tblUserGroup", null))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<Models.prs_tblUserGroupSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<Models.prs_tblUserGroupSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldID":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId_ByUserId";
                                    break;
                                case "fldTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitle_ByUserId";
                                    break;
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDesc_ByUserId";
                                    break;
                                case "Name_Family":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameFamily_ByUserId";
                                    break;
                                case "NameEdarekol":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "EdarekolName_ByUserId";
                                    break;
                            }
                            
                            if (data != null)
                                data1 = m.prs_tblUserGroupSelect(field, searchtext, user.UserSecondId,100).ToList();
                            else
                                data = m.prs_tblUserGroupSelect(field, searchtext, user.UserSecondId, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                       
                        data = m.prs_tblUserGroupSelect("ByUserId", "", user.UserSecondId,100).ToList();
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
                                    return !oValue.ToString().Contains(value.ToString());
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

                    List<Models.prs_tblUserGroupSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadUserGps(StoreRequestParameters parameters, int UserGp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(6, "tblUserGroup", null))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<Models.prs_tblUserGroupSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<Models.prs_tblUserGroupSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId_ByUserId";
                                    break;
                                case "fldTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitle_ByUserId";
                                    break;
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDesc_ByUserId";
                                    break;

                            }
                           
                            if (data != null)
                                data1 = m.prs_tblUserGroupSelect(field, searchtext, user.UserSecondId,0).Where(l => l.fldID != UserGp).ToList();
                            else
                                data = m.prs_tblUserGroupSelect(field, searchtext, user.UserSecondId, 0).Where(l => l.fldID != UserGp).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                       
                        data = m.prs_tblUserGroupSelect("Copy_Pr", UserGp.ToString(), user.UserSecondId,0).ToList();
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
                                    return !oValue.ToString().Contains(value.ToString());
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

                    List<Models.prs_tblUserGroupSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }

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
                        
                        m.prs_UpdateOrder("UserGroup",item.fldId,item.order);
                        
                    }
                    Msg = "ویرایش با موفقیت انجام شد.";
                    MsgTitle = "ویرایش موفق";
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
