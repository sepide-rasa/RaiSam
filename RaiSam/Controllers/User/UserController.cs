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
using System.Net.NetworkInformation;

namespace RaiSam.Controllers.User
{
    public class UserController : Controller
    {
        //
        // GET: /User/

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

        public ActionResult ShowUserGroup(int UserId, string Name)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new RaiSam.Models.UserGroupPermission();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                ViewData = this.ViewData
            };
            result.ViewBag.UserId = UserId;
            result.ViewBag.Name = Name;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoad(string node, string GrohKarbari)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(10, "", "0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();

                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    var child = m.prs_tblApplicationPartSelect("fldPId", node, user.UserSecondId, Convert.ToInt32(GrohKarbari), 0).ToList();

                    foreach (var ch in child)
                    {
                        Node childNode = new Node();
                        childNode.Text = ch.fldTitle;
                        childNode.NodeID = ch.fldID.ToString();
                        childNode.Icon = Ext.Net.Icon.Building;
                        childNode.Expanded = true;

                        var f = m.prs_tblPermissionSelect("fldApplicationPartID", ch.fldID.ToString(), Convert.ToInt32(GrohKarbari), 0).ToList();
                        if (f.Count == 0)
                        {
                            childNode.Checked = false;
                        }
                        else
                        {
                            childNode.Checked = true;
                        }
                        //if (f.Count() != 0)
                        //{
                        //    foreach (var _item in f)
                        //    {
                        //        if (_item.fldApplicationPartID == ch.fldID)
                        //        {
                        //            childNode.Checked = true;
                        //        }
                        //    }
                        //}
                        nodes.Add(childNode);
                    }
                    return this.Store(nodes);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult ShowPermission(int UserGroupId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.UserGroupId = UserGroupId;
            return result;
        }

        public ActionResult ShowPermission2(int UserGroupId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.UserGroupId = UserGroupId;
            return result;
        }

        public ActionResult ResetPassWindow()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisableUser(string UserName)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                byte Er = 0;
                try
                {

                    var user = m.prs_tblUserSelect("fldUserName", UserName, "", 0).FirstOrDefault();
                    if (user != null)
                    {

                        m.prs_UpdateTypeUser(user.fldId, false);
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


                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, null, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /* public ActionResult UpdatePassPersonal(string CodeEn)
         {
             if (Session["User"] == null)
                 return RedirectToAction("Logon", "Account", new { area = "" });
             string Msg = "", MsgTitle = ""; byte Er = 0;
             UserInfo user = (UserInfo)(Session["User"]);
             param.FieldName = "AllPersonal_CodeEnhesari";
             param.Value = CodeEn;
             param.UserInfoId = user.UserInputId;
             param.UserId = user.UserId;
             param.Top = 0;
             try
             {
                 var q = service.SelectPersonalInfo(param, user.UserKey, IP).PersonalInfoList.FirstOrDefault();
                 if (q == null)
                 {
                     MsgTitle = "خطا";
                     Msg = "کد انحصاری وارد شده معتبر نمی باشد.";
                     Er = 1;
                 }
                 else
                 {
                     m.prs_UpdatePassPersonal(q.fldCodeMeli, q.fldId);
                     MsgTitle = "عملیات موفق";
                     Msg = "رمز عبور پرسنل به کد ملی تغییر یافت.";
                 }
             }
             catch (Exception x)
             {
                 string InnerException = "";
                 if (x.InnerException != null)
                     InnerException = x.InnerException.Message;
                 else
                     InnerException = x.Message;
                 var error = service.Error(InnerException, user.UserKey, IP);
                 return Json(new
                 {
                     MsgTitle = error.MsgTitle,
                     Msg = error.Msg,
                     Er = 1
                 });
             }
             return Json(new
             {
                 Msg = Msg,
                 MsgTitle = MsgTitle,
                 Er = Er
             }, JsonRequestBehavior.AllowGet);
         }*/

        public ActionResult TreeUser(byte State)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.State = State;
            return result;
        }

        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpUser()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "2", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinUser()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoUser()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "2", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveResetPass(int fldId)
        {
            string Msg = "", MsgTitle = ""; byte Er = 0;
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

                try
                {
                    var q = m.prs_tblUserSelect("fldId", fldId.ToString(), "", 0).FirstOrDefault();
                    var pass = CodeDecode.ComputeSha256Hash(q.fldUserName);
                    if (q.fldFirstRigesterId == null)
                    {
                        m.prs_UserPassUpdate(q.fldId, pass, user.UserSecondId);
                        m.prs_UserFirsLoginUpdate(q.fldId, true);
                    }
                    else
                    {
                        m.prs_UpdatePasswordFirstRegister(pass, q.fldFirstRigesterId);
                        m.prs_UserFirsLoginUpdate(q.fldId, true);
                    }

                    Msg = "بازنشانی رمز عبور با موفقیت انجام شد (پیش فرض نام کاربری).";
                    MsgTitle = "عملیات موفق";
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
        public ActionResult Read(StoreRequestParameters parameters, int NoeKarbar)//NoeKarbar=1 داخلی 0 وخارجی 
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(3, "", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<Models.prs_SelectUserByUserId> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<Models.prs_SelectUserByUserId> data1 = null;
                        if (NoeKarbar == 0)
                        {
                            foreach (var item in filterHeaders.Conditions)
                            {
                                var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                                switch (item.FilterProperty.Name)
                                {
                                    case "fldId":
                                        searchtext = ConditionValue.Value.ToString();
                                        field = "fldId";
                                        break;
                                    case "fldFamily":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFamily";
                                        break;
                                    case "fldName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldName";
                                        break;
                                    case "fldCodeMeli":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeMeli";
                                        break;
                                    case "fldFatherName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldFatherName";
                                        break;
                                    case "fldCodeEnhesari":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeEnhesari";
                                        break;
                                    case "fldActive_DeactiveName":
                                        searchtext = ConditionValue.Value.ToString() + "%";
                                        field = "fldActive_DeactiveName";
                                        break;
                                    case "fldUserName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldUserName";
                                        break;
                                    case "fldUserType":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldUserType";
                                        break;
                                    case "fldLimitationType":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldLimitationType";
                                        break;
                                    case "fldLimitationUser":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldLimitationUser";
                                        break;
                                }

                                if (data != null)
                                    data1 = m.prs_SelectUserByUserId(field, searchtext, user.UserSecondId, 100).ToList();
                                else
                                    data = m.prs_SelectUserByUserId(field, searchtext, user.UserSecondId, 100).ToList();
                            }
                        }
                        else
                        {
                            foreach (var item in filterHeaders.Conditions)
                            {
                                var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                                switch (item.FilterProperty.Name)
                                {
                                    case "fldId":
                                        searchtext = ConditionValue.Value.ToString();
                                        field = "First_fldId";
                                        break;
                                    case "fldFamily":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldFamily";
                                        break;
                                    case "fldName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldName";
                                        break;
                                    case "fldCodeMeli":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldCodeMeli";
                                        break;
                                    case "fldFatherName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldFatherName";
                                        break;
                                    case "fldActive_DeactiveName":
                                        searchtext = ConditionValue.Value.ToString() + "%";
                                        field = "First_fldActive_DeactiveName";
                                        break;
                                    case "fldUserName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldUserName";
                                        break;
                                    case "fldUserType":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldUserType";
                                        break;
                                    case "fldLimitationType":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldLimitationType";
                                        break;
                                    case "fldLimitationUser":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "First_fldLimitationUser";
                                        break;
                                }

                                if (data != null)
                                    data1 = m.prs_SelectUserByUserId(field, searchtext, user.UserSecondId, 100).ToList();
                                else
                                    data = m.prs_SelectUserByUserId(field, searchtext, user.UserSecondId, 100).ToList();
                            }
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        if (NoeKarbar == 0)
                            data = m.prs_SelectUserByUserId("", "", user.UserSecondId, 100).ToList();
                        else
                            data = m.prs_SelectUserByUserId("First", "", 0, 100).ToList();
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

                    List<Models.prs_SelectUserByUserId> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadUserGroupWithUserId(StoreRequestParameters parameters, int UserId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(3, "", "0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();

                    List<prs_TreeUserGroupSelect> data = null;
                    data = m.prs_TreeUserGroupSelect(UserId).ToList();
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
        public ActionResult ReadUserGroupWithGrantWithUserId(StoreRequestParameters parameters, int UserId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(3, "", "0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();

                    List<prs_TreeUserGroupWithGrantSelect> data = null;
                    data = m.prs_TreeUserGroupWithGrantSelect(UserId).ToList();
                    return this.Store(data);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }

        public ActionResult New(int Id, int UserId)
        {//باز شدن تب
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.Id = Id;
            result.ViewBag.OldUserId = UserId;
            result.ViewBag.NewUserId = user.UserSecondId;
            var OwnerUser = UserId;
            if (Id == 0)
                OwnerUser = user.UserSecondId;
            //result.ViewBag.TreeIdUser = user.TreeId;
            result.ViewBag.OwnerUser = OwnerUser;
            return result;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserType(int OwnerUser)
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
                var q = m.prs_UserTypeSelect(OwnerUser).ToList().Select(c => new { Id = c.Id, fldName = c.Name });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserOwner()
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

                var q = m.prs_SelectUserByUserId("OwnerUser", "", user.UserSecondId, 0).ToList().Select(c => new { Id = c.fldId, fldName = c.fldNamePersonal });
                return this.Store(q);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadGroup(string nod, int UserId, byte UserType, int OwnerUser)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(3, "", "0"))
                {
                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();

                    if (nod == "0")
                    {

                        var q = m.prs_GetUserGroupTree(UserId, OwnerUser, UserType).ToList();
                        foreach (var item in q)
                        {
                            Node asyncNode = new Node();
                            //asyncNode.Text = item.fldTitle;
                            asyncNode.NodeID = item.fldId.ToString();
                            asyncNode.Leaf = true;
                            asyncNode.Icon = Ext.Net.Icon.Group;
                            asyncNode.Qtip = "<div>کاربر ایجاد کننده: " + item.Name_Family + "</div></br>" + "<div>اداره کل: " + item.NameEdarekol + "</div>";
                            //asyncNode.Leaf = true;
                            //asyncNode.Checked = false;
                            asyncNode.AttributesObject = new { fldTitle = item.fldTitle, fldGrant = item.fldGrant, fldWithGrant = item.fldWithGrant };
                            /*var f = servic.GetUser_GroupWithFilter("fldUserSelectId", UserId.ToString(), 0, Session["Username"].ToString(), Session["Password"].ToString(), out Err).ToList();
                            if (f.Count() != 0)
                            {
                                foreach (var _item in f)
                                {
                                    if (_item.fldUserGroupId == item.fldId && _item.fldUserSelectId == UserId)
                                    {
                                        asyncNode.Checked = true;
                                    }
                                }
                            }*/
                            nodes.Add(asyncNode);
                        }
                    }
                    return this.Store(nodes);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadGroupTreeUser(string nod)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(6, "", "0"))
                {
                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();

                    var q = m.prs_tblUserSelect("fldUserId", nod, user.UserSecondId.ToString(), 0).ToList();
                    var tooltip = "";
                    foreach (var item in q)
                    {

                        var person = m.prs_tblPersonalInfoSelect("AllPersonal_ShakhsId", item.fldShakhsId.ToString(), "", "", 0, 0).FirstOrDefault();

                        if (person == null)
                        {

                            var Shakhs = m.prs_tblAshkhasSelect("fldId", item.fldShakhsId.ToString(), "", "", "", 0).FirstOrDefault();

                            var Shakhspic = m.prs_tblFileSelect("fldId", Shakhs.fldFileId.ToString(), 0).FirstOrDefault();
                            var image = "IA==";
                            if (Shakhs != null && Shakhs.fldFileId != null && Shakhspic.fldFile != null)
                            {
                                image = Convert.ToBase64String(Shakhspic.fldFile);
                            }
                            tooltip = "<div style='float:right;margin-top:5px;margin-left:20px;display: inline-block;'><img style='width:95px;height:120px;' src='data:image/jpeg;base64,"
                            + image + "'/></div><div style='display: inline-block';><p><strong>نام و نام خانوادگی: " +
                            Shakhs.fldName + ' ' + Shakhs.fldFamily + "</strong></p><p><strong>نام پدر: " + Shakhs.fldFatherName +
                            "</strong></p>" + "<p><strong>کد انحصاری: " + "" + "</strong></p><p><strong>کد ملی: " + Shakhs.fldCodeMeli +
                            "</strong></p><p><strong>شماره موبایل: " + Shakhs.fldMobile + "</strong></p></div>";
                        }
                        else
                        {

                            var Perpic = m.prs_tblFileSelect("fldId", person.fldfileId.ToString(), 0).FirstOrDefault();

                            var image = "IA==";
                            if (person != null && person.fldfileId != null && Perpic.fldFile != null)
                            {
                                image = Convert.ToBase64String(Perpic.fldFile);
                            }
                            if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
                            {
                                var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                                image = Convert.ToBase64String(file);
                            }
                            tooltip = "<div style='float:right;margin-top:25px;margin-left:20px;display: inline-block;'><img style='width:120px;height:150px;' src='data:image/jpeg;base64,"
                          + image + "'/></div><div style='display: inline-block';><p><strong>نام و نام خانوادگی: " +
                          person.fldName_Family + "</strong></p><p><strong>نام پدر: " + person.fldFatherName +
                          "</strong></p>" + "<p><strong>کد انحصاری: " + person.fldCodeEnhesari + "</strong></p><p><strong>کد ملی: " + person.fldCodeMeli +
                          "</strong></p><p><strong>شماره موبایل: " + person.fldMobile + "</strong></p><p><strong>پایه: " + person.fldPayeName +
                          "</strong></p><p><strong>شرح پست: " + person.fldDescPost +
                          "</strong></p><p><strong>محل خدمت: " + person.fldTitel_MaleSazemani + "(" + person.fldStationName + ")" +
                          "</strong></p></div>";
                        }

                        Node asyncNode = new Node();
                        asyncNode.Text = item.fldNamePersonal;
                        asyncNode.NodeID = item.fldId.ToString();
                        asyncNode.Icon = Ext.Net.Icon.Group;
                        asyncNode.Qtip = tooltip;

                        var q1 = m.prs_tblUserSelect("fldUserId", item.fldId.ToString(), "", 0).ToList();
                        if (q1.Count == 0)
                        {
                            asyncNode.Leaf = true;
                        }
                        nodes.Add(asyncNode);
                    }
                    return this.Store(nodes);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadTreeStructure(string nod, int EditUser, int OwnerUser)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                NodeCollection nodes = new Ext.Net.NodeCollection();
                List<prs_tblUser_StationSelect> child = new List<prs_tblUser_StationSelect>();
                UserInfo user1 = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                //param.FieldName = "fldId";
                //param.Value = user1.UserSecondId.ToString();
                //param.UserInfoId = user1.UserInputId;
                //param.UserId = user1.UserSecondId;
                //param.Top = 0;
                //var user = service.SelectUser(param, user1.UserKey, IP).UserList.FirstOrDefault();
                //if (user.fldTreeId == 1)
                //{
                //    param.FieldName = "fldPId";
                //    param.Value = nod;
                //    param.UserInfoId = user1.UserInputId;
                //    param.UserId = user1.UserSecondId;
                //    param.Top = 0;
                //    var child = service.SelectTreeStructure(param, user1.UserKey, IP).TreeStructurelist.ToList();
                //    foreach (var ch in child)
                //    {
                //        Node childNode = new Node();
                //        childNode.Text = ch.fldTitle;
                //        childNode.NodeID = ch.fldId.ToString();
                //        childNode.Icon = Ext.Net.Icon.Building;
                //        nodes.Add(childNode);
                //    }
                //}
                if (nod == "1")
                {

                    child = m.prs_tblUser_StationSelect("fldNahi", "", OwnerUser, 0).ToList();
                    foreach (var ch in child)
                    {
                        Node childNode = new Node();
                        childNode.Text = ch.fldName;
                        childNode.Checked = false;
                        childNode.Leaf = false;
                        childNode.NodeID = "N;" + ch.fldId.ToString();
                        childNode.Icon = Ext.Net.Icon.Building;
                        nodes.Add(childNode);
                    }
                }
                else
                {

                    child = m.prs_tblUser_StationSelect("fldStation", nod.Split(';')[1], user1.UserSecondId, 0).ToList();
                    foreach (var ch in child)
                    {
                        Node childNode = new Node();
                        childNode.Checked = false;
                        childNode.Text = ch.fldName;
                        childNode.Leaf = true;
                        childNode.NodeID = "S;" + ch.fldId.ToString();
                        childNode.Icon = Ext.Net.Icon.Building;
                        if (EditUser != 0)
                        {

                            var q = m.prs_tblUser_StationSelect("Permission", ch.fldId.ToString(), EditUser, 1).FirstOrDefault();
                            if (q != null)
                            {
                                childNode.Checked = true;
                            }
                        }
                        nodes.Add(childNode);
                    }
                }
                return this.Store(nodes);
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

                var q = m.prs_tblUserSelect("fldId", Id.ToString(), "", 0).FirstOrDefault();

                var usergroup = m.prs_tblUser_GroupSelect("fldUserSelectId", Id.ToString(), 0).ToList();
                List<int> Checked = new List<int>();
                foreach (var item in usergroup)
                {
                    Checked.Add(item.fldID);
                }
                string Active_Deactive = "0"; string Type = "0";
                if (q.fldActive_Deactive == true)
                    Active_Deactive = "1";
                if (q.fldType == true)
                    Type = "1";
                //param.FieldName = "TreeId";
                //param.Value = q.fldTreeId.ToString();
                //param.UserId = user.UserSecondId;
                //param.UserInfoId = user.UserInputId;
                //param.Top = 0;
                //var title = service.GetTreeStructureTitle(param, user.UserKey, IP).TreeStructurelist.FirstOrDefault().fldTitle;
                return Json(new
                {
                    fldId = q.fldId,
                    fldShakhsId = q.fldShakhsId,
                    fldUserName = q.fldUserName,
                    fldPassword = q.fldPassword,
                    fldFamilyName = q.fldNamePersonal,
                    fldActive_Deactive = Active_Deactive,
                    fldType = Type,
                    fldDesc = q.fldDesc,
                    Checked = Checked,
                    //fldTreeId = q.fldTreeId,
                    UserType = q.fldUserType.ToString(),
                    //fldTitleTreeStructure = title
                    fldUserId = q.fldUserId.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getToolsData(int Id)
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

                var q = m.prs_tblUserSelect("fldId", Id.ToString(), "", 0).FirstOrDefault();

                if (q.fldFirstRigesterId == null)
                {
                    var person = m.prs_tblPersonalInfoSelect("AllPersonal_ShakhsId", q.fldShakhsId.ToString(), "", "", 0, 0).FirstOrDefault();


                    if (person != null)
                    {

                        var Perpic = m.prs_tblFileSelect("fldId", person.fldfileId.ToString(), 1).FirstOrDefault();

                        var image = "IA==";
                        if (person != null && person.fldfileId != null && Perpic.fldFile != null)
                        {
                            image = Convert.ToBase64String(Perpic.fldFile);
                        }
                        if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
                        {
                            var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                            image = Convert.ToBase64String(file);
                        }
                        return new JsonResult()
                        {
                            Data = new
                            {
                                Type = q.fldUserType,
                                fldShakhsId = q.fldShakhsId,
                                image = image,
                                fldName = person.fldName_Family,
                                fldFatherName = person.fldFatherName,
                                fldCodeEnhesari = person.fldCodeEnhesari,
                                fldCodeMeli = person.fldCodeMeli,
                                fldMobile = person.fldMobile,
                                fldPayeName = person.fldPayeName,
                                fldDescPost = person.fldDescPost,
                                fldTitle = person.fldTitel_MaleSazemani,
                                fldStationName = person.fldStationName
                            },
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {

                        var shakhs = m.prs_tblAshkhasSelect("fldId", q.fldShakhsId.ToString(), "", "", "", 0).FirstOrDefault();
                        var image = "IA==";
                        if (shakhs != null && shakhs.fldFileId != null)
                        {

                            var f = m.prs_tblFileSelect("fldId", shakhs.fldFileId.ToString(), 0).FirstOrDefault();
                            image = Convert.ToBase64String(f.fldFile);
                        }
                        return new JsonResult()
                        {
                            Data = new
                            {
                                Type = q.fldUserType,
                                fldShakhsId = q.fldShakhsId,
                                image = image,
                                fldName = shakhs.fldName + ' ' + shakhs.fldFamily,
                                fldFatherName = shakhs.fldFatherName,
                                fldCodeEnhesari = "",
                                fldCodeMeli = shakhs.fldCodeMeli,
                                fldMobile = shakhs.fldMobile,
                                fldPayeName = "",
                                fldDescPost = "",
                                fldTitle = "",
                                fldStationName = ""
                            },
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }
                else
                {
                    var q1 = m.prs_tblFirstRegisterSelect("fldId", q.fldFirstRigesterId.ToString(), "", 0).FirstOrDefault();
                    var co = m.prs_tblCompanyProfileSelect("fldFirstRegisterId", q.fldFirstRigesterId.ToString(), 1).FirstOrDefault();
                    string FullName = "", NationalCode = "", Sh_Sabt="";
                    if (co != null)
                    {
                        FullName=co.fldFullName;
                        NationalCode=co.fldNationalCode;
                        Sh_Sabt = co.fldSh_Sabt;
                        
                    }
                    var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                    return new JsonResult()
                    {
                        Data = new
                        {
                            image= Convert.ToBase64String(file),
                            fldName = q1.fldName + ' ' + q1.fldFamily,
                            fldFatherName = FullName,
                            fldCodeEnhesari = "",
                            fldCodeMeli = NationalCode,
                            Sh_Sabt = Sh_Sabt
                        },
                        MaxJsonLength = Int32.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
        }

        //public ActionResult GetTreeUser(int Id)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    param.FieldName = "fldId";
        //    param.Value = Id.ToString();
        //    param.UserId = user.UserSecondId;
        //    param.UserInfoId = user.UserInputId;
        //    param.Top = 0;
        //    var q = service.SelectUser(param, user.UserKey, IP).UserList.FirstOrDefault();
        //    param.FieldName = "fldId";
        //    param.Value = q.fldTreeId.ToString();
        //    param.UserId = user.UserSecondId;
        //    param.UserInfoId = user.UserInputId;
        //    param.Top = 0;
        //    var tree = service.SelectTreeStructure(param, user.UserKey, IP).TreeStructurelist.FirstOrDefault();
        //    string path = "";
        //    if (tree.fldNahiId != null)/*در سطح اداره کل*/
        //    {
        //        path = "/1/" + tree.fldId;
        //    }
        //    else if (tree.fldStationId != null)/*در سطح ایستگاه*/
        //    {
        //        path = "/1/" + tree.fldPId + "/" + tree.fldId;
        //    }
        //    return Json(new
        //    {
        //        path = path
        //    }, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetNodeTitle(int id)
        //{//نمایش اطلاعات جهت رویت کاربر
        //    if (Session["User"] == null)
        //        return null;
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    param.FieldName = "TreeId";
        //    param.Value = id.ToString();
        //    param.UserId = user.UserSecondId;
        //    param.UserInfoId = user.UserInputId;
        //    param.Top = 0;
        //    var title = service.GetTreeStructureTitle(param, user.UserKey, IP).TreeStructurelist.FirstOrDefault().fldTitle;
        //    return Json(new
        //    {
        //        fldTitle = title
        //    }, JsonRequestBehavior.AllowGet);

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblUserSelect user, string UserGroup1/*, bool Change*/)
        {
            string Msg = "", MsgTitle = ""; byte Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user1 = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();

                try
                {
                    List<prs_tblUser_GroupSelect> UserGroup = JsonConvert.DeserializeObject<List<prs_tblUser_GroupSelect>>(UserGroup1);
                    if (user.fldDesc == null)
                        user.fldDesc = "";

                    if (user.fldId == 0)
                    { //ذخیره
                        if (Permission.haveAccess(4, "tblUser", null))
                        {
                            var q1 = m.prs_tblUserSelect("CheckShakhsId", user.fldShakhsId.ToString(), "", 0).Any();
                            var q2 = m.prs_tblUserSelect("fldUserName", user.fldUserName, "", 0).Any();
                            if (q1 == true || q2 == true)
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
                                System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                                Dictionary<string, object> parameters = new Dictionary<string, object>();
                                parameters.Add("نام کاربری", user.fldUserName);
                                parameters.Add("وضعیت دسترسی", user.fldActive_DeactiveName);
                                parameters.Add("توضیحات", user.fldDesc);
                                parameters.Add("نوع کاربر ", user.fldUserType_Name);
                                parameters.Add("وضعیت فعالیت", user.fldTypeName);
                                parameters.Add("کد ایستگاه", user.fldStationId);
                                parameters.Add("نام و نام خانوادگی ", user.fldName);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_tblUserInsert(Id, user.fldUserName, CodeDecode.ComputeSha256Hash(user.fldUserName), user.fldActive_Deactive, user.fldShakhsId, user1.UserInputId, user.fldDesc, user.fldUserType, user.fldStationId, user1.UserSecondId, user.fldType, user1.UserSecondId, user.fldSetadi,user.fldFirstRigesterId,user.fldNoeKarbar, jsonstr);

                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "دخیره موفق";
                                foreach (var item in UserGroup)
                                {
                                    m.prs_tblUser_GroupInsert(item.fldUserGroupID, Convert.ToInt32(Id.Value), item.fldGrant, item.fldWithGrant, user1.UserInputId, "");

                                }
                            }
                            
                        }
                        else
                        {
                            return Json(new
                            {
                                Er = Er,
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا"
                            }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    { //ویرایش
                        if (Permission.haveAccess(5, "tblUser", user.fldId.ToString()))
                        {
                            var q1 = m.prs_tblUserSelect("CheckShakhsId", user.fldShakhsId.ToString(), "", 0).FirstOrDefault();
                            var q2 = m.prs_tblUserSelect("fldUserName", user.fldUserName, "", 0).FirstOrDefault();
                            if (q1 != null && q1.fldId != user.fldId || q2 != null && q2.fldId != user.fldId)
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
                                var userid = user.fldUserId;
                                user.fldUserIdlog = user1.UserSecondId;
                                user.fldInputID = user1.UserInputId;
                                m.prs_tblUserUpdate(user.fldId, user.fldUserName, user.fldActive_Deactive, user.fldShakhsId, user1.UserInputId, user.fldDesc, user.fldUserType, user1.UserSecondId, user.fldType, user.fldStationId, user.fldSetadi, user1.UserSecondId);
                                /*var m = service.UpdateUserId(user, user1.UserKey, IP);*/

                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                                //* }*/
                                m.prs_tblUser_GroupDelete(user.fldId, user1.UserInputId);

                                foreach (var item in UserGroup)
                                {
                                    m.prs_tblUser_GroupInsert(item.fldUserGroupID, user.fldId, item.fldGrant, item.fldWithGrant, user1.UserInputId, "");


                                }
                            }
                            
                        }
                        else
                        {
                            return Json(new
                            {
                                Er = Er,
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا"
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user1.UserKey, IP, false, 0).FirstOrDefault();


                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نام کاربری", user.fldUserName);
                    parameters.Add("وضعیت دسترسی", user.fldActive_DeactiveName);
                    parameters.Add("توضیحات", user.fldDesc);
                    parameters.Add("نوع کاربر ", user.fldUserType_Name);
                    parameters.Add("وضعیت فعالیت", user.fldTypeName);
                    parameters.Add("کد ایستگاه", user.fldStationId);
                    parameters.Add("کد کاربر", user.fldUserId);
                    parameters.Add("نام و نام خانوادگی ", user.fldName);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "User:Save: " + InnerException);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (user.fldId == 0)
                    {
                        m.prs_LogInsert(user1.UserInputId, "dbo.tblUser", jsonstr, false);
                    }
                    else
                    {
                        
                        m.prs_LogUpdate(user1.UserInputId, "dbo.tblUser", jsonstr, false, user.fldId);
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                    });
                }
                return Json(new
                {
                    Er = Er,
                    Msg = Msg,
                    MsgTitle = MsgTitle
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassPersonal(string CodeEn)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    if (Session["User"] == null)
                        return RedirectToAction("Logon", "Account", new { area = "" });
                    
                    
                    var person = m.prs_tblPersonalInfoSelect("AllPersonal_CodeEnhesari", CodeEn,"","",0,0).FirstOrDefault();

                    if (person == null)
                    {
                        return Json(new
                        {
                            Er = 1,
                            MsgTitle = "خطا",
                            Msg = "کدانحصاری موردنظر در سیستم موجود نمی باشد."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        
                        m.prs_UpdatePassPersonal(person.fldCodeMeli, person.fldId, ((UserInfo)Session["User"]).UserInputId);
                        
                        MsgTitle = "عملیات موفق";
                        Msg = "بازنشانی رمزعبور با موفقت انجام شد.(پیشفرض کدملی)";
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", ((UserInfo)Session["User"]).UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
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
        public ActionResult SetUserSecond(int UserSecondId, string NameUserSecond)
        {
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            string Key = Guid.NewGuid().ToString();
            var macaddres= (from nic in NetworkInterface.GetAllNetworkInterfaces()  /*where nic.OperationalStatus == OperationalStatus.Up*/   select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            
            System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
            m.prs_tblInputInfoInsert(Id, IP, macaddres, 1, user.UserId, "", Request.Browser.Browser + " Version:" + Request.Browser.Version, Key, 1, UserSecondId);

            
            user.UserInputId =Convert.ToInt32(Id.Value);
            user.UserSecondId = UserSecondId;
            user.UserKey = Key;

            
            var h = m.prs_tblInputInfoSelect("fldId", user.UserInputId.ToString(),"",true,1).FirstOrDefault();
            

            var userLogin = m.prs_tblUserSelect("fldId", user.UserId.ToString(),"",1).FirstOrDefault();
            if (UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).Count() > 0)
                UserLoginCount.userObj.Remove(UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).FirstOrDefault());


            UserLoginCount.AddOnlineUser(user.UserId.ToString(), h.Name_Family, IP, userLogin.fldUserName, h.fldTarikh + " " + h.fldTime /*+ " " +*/ , System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString(), "", "");

            var usersec = m.prs_tblUserSelect("fldId", UserSecondId.ToString(), "", 1).FirstOrDefault();
            if (usersec.fldFirstRigesterId != null)
            {
                user.FirstRegisterId = Convert.ToInt32(usersec.fldFirstRigesterId);
                Session["FristRegisterId"] = usersec.fldFirstRigesterId;

                return RedirectToAction("Index", "Admin", new { area = "Faces" });
            }
            else
            {
               
                return RedirectToAction("Index", "Home");
            }



        }
        public ActionResult SpecificKartablPer(int UserId)
        {//باز شدن تب
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.UserId = UserId;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePermission(/*List<OBJ_SpecificCartablePermission> permi*/string AppIds, int UserAccessId)
        {
            string Msg = "",
            MsgTitle = "";
            byte Er = 0;
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
                prs_tblSpecificCartablePermissionSelect userPer = new prs_tblSpecificCartablePermissionSelect();
                try
                {
                    if (Permission.haveAccess(163, "tblSpecificCartablePermission", null))
                    {
                        var q = m.prs_tblSpecificCartablePermissionSelect("fldUserAccessId", UserAccessId.ToString(), 0).ToList();
                        if (q.Count() != 0)
                        {
                            m.prs_tblSpecificCartablePermissionDelete(UserAccessId, user.UserInputId);

                        }
                        if (AppIds != ""/*permi != null*/)
                        {

                            m.prs_tblSpecificCartablePermissionInsert(UserAccessId, AppIds, user.UserInputId, "");

                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "ذخیره موفق";
                        }
                        else if (q.Count == 0 && AppIds == ""/*&& permi == null*/)
                        {
                            return Json(new
                            {
                                Msg = "لطفا دسترسی های مورد نظر را انتخاب نمایید.",
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadCartabl(string nod, byte Typee, int UserAccessId, int CartableIdd)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(162,"","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var fldShowGeneral = false;
                    var fldShowSpecific = false;
                    var fldStatus = false;
                    var fldEffective = false;
                    var url = "/Content/icon/mini/کارتابل.png";
                    var FieldName="";
                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    UserInfo user = (UserInfo)(Session["User"]);
                    switch (Typee)
                    {
                        case 0:
                           FieldName = "GetAllCartable";
                            break;
                        case 1:
                            FieldName = "GetAction_CartableId";
                            url = "/Content/icon/mini/اقدام.png";
                            break;
                        case 2:
                            FieldName = "GetCharkhe_EghdamID";
                            url = "/Content/icon/mini/چرخه.png";
                            break;
                        case 3:
                            FieldName = "GetOperation_CharkheEghdamId_User";
                            url = "/Content/icon/mini/اکشن.png";
                            break;
                    }
                    var q = m.prs_SpecificCartablePermissionTreeSelect(FieldName, nod, UserAccessId, user.UserSecondId, CartableIdd).ToList().ToList();
                    foreach (var item in q)
                    {
                        Node asyncNode = new Node();
                        if (Typee == 3)
                        {
                            asyncNode.Leaf = true;

                            var OP_AC = m.prs_tblOpertion_ActionSelect("fldId",item.NodeId.ToString(),"","",1).FirstOrDefault();

                            var op = m.prs_tblOperationSelect("fldId", OP_AC.fldOpertionId.ToString(), user.UserSecondId,1).FirstOrDefault();
                            fldShowGeneral = op.fldUsable;
                            fldShowSpecific = Convert.ToBoolean(op.fldSpecificShow);
                            fldStatus = op.fldGroup;
                            fldEffective = op.fldeffective;
                        }
                        asyncNode.NodeID = item.NodeId.ToString() + item.NodeType + CartableIdd;
                        asyncNode.Text = item.NodeTitle;
                        asyncNode.IconFile = url;
                        asyncNode.Checked = item.Accessed;
                        asyncNode.Expanded = true;
                        asyncNode.AttributesObject = new
                        {
                            fldNodeId = item.NodeId,
                            fldNodeName = item.NodeTitle,
                            CartablId = item.CartablId,
                            fldNodeType = item.NodeType.ToString(),
                            fldShowGeneral = fldShowGeneral,
                            fldShowSpecific = fldShowSpecific,
                            fldStatus = fldStatus,
                            fldEffective = fldEffective
                        };
                        nodes.Add(asyncNode);
                    }
                    return this.Store(nodes);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
    }
}
