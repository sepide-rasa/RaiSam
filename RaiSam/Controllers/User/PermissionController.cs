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
    public class PermissionController : Controller
    {
        //
        // GET: /Permission/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var q = m.prs_tblUserGroupSelect("ByUserId", "", user.UserSecondId,0).ToList();
            if (q.Count == 0)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "برای تعیین دسترسی ابتدا باید گروه کاربری تعریف شود."
                });
                DirectResult result1 = new DirectResult();
                return result1;
            }
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
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                return PartialView;
            }
        }
        public FileContentResult ShowHelpPermission()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var Help = m.prs_tblHelpSelect("fldId","3",1).FirstOrDefault();
            
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinPermission()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoPermission()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var Help = m.prs_tblHelpSelect("fldId", "3", 1).FirstOrDefault();
            
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult HelpUserforPermission()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                return PartialView;
            }
        }
        public FileContentResult ShowHelpUserforPermission()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var Help = m.prs_tblHelpSelect("fldId","4",1).FirstOrDefault();
           
            
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinUserforPermission()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoUserforPermission()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "4", 1).FirstOrDefault();


            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
           
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserGroup()
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
                
                var q = m.prs_tblUserGroupSelect("ByUserId", "", user.UserSecondId,0).Select(n => new { ID = n.fldID, Name = n.fldTitle });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReloadDrp()
        {
            if (Session["User"] == null)
                return null;
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                
                var q = m.prs_tblUserGroupSelect("ByUserId", "", user.UserSecondId, 0).FirstOrDefault();

                return Json(new
                {
                    fldUserGroupId = q.fldID.ToString(),

                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePermission(string permi1, int UserGroupID, string UnChecked, string UnChecked_Charkhe)
        {
            string Msg = "ذخیره با موفقیت انجام شد.",
            MsgTitle = "ذخیره موفق";
            byte Er = 0; 
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                try
                {
                    List<Models.OBJ_Permission> permi = JsonConvert.DeserializeObject<List<Models.OBJ_Permission>>(permi1);
                    
                    var q = m.prs_tblPermissionSelect("fldUserGroupID", UserGroupID.ToString(),0,0).ToList();
                    var q1 = m.prs_tblPermissionCharkheSelect("fldUserGroupID", UserGroupID.ToString(), 0, 0).ToList();

                    if (q.Count == 0 && q1.Count == 0 && permi == null)
                    {
                        return Json(new
                        {
                            Msg = "لطفا دسترسی های مورد نظر را انتخاب نمایید.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (q.Count() != 0)
                        {
                            m.prs_tblPermissionDelete(UserGroupID, user.UserInputId);
                            
                            var outDeletetime = m.prs_tblTimeLimit_UserGroupDelete(UserGroupID, user.UserInputId);

                            Msg = "حذف با موفقیت انجام شد.";
                            MsgTitle = "حذف موفق";
                            //حذف این دسترسی از بقیه زیرمجموعه ها
                            if (UnChecked != "")
                                m.prs_DeleteChildUserGroupPermission(UnChecked, UserGroupID, user.UserInputId);

                            Msg = "حذف با موفقیت انجام شد.";
                            MsgTitle = "حذف موفق";
                        }

                        if (q1.Count != 0)
                        {
                            m.prs_tblPermissionCharkheDelete(UserGroupID, user.UserInputId);

                            Msg = "حذف با موفقیت انجام شد.";
                            MsgTitle = "حذف موفق";
                            //حذف این دسترسی از بقیه زیرمجموعه ها
                            if (UnChecked_Charkhe != "")
                                m.prs_DeleteChildUserGroupPermissionCharkhe(UnChecked_Charkhe, UserGroupID, user.UserInputId);

                            Msg = "حذف با موفقیت انجام شد.";
                            MsgTitle = "حذف موفق";
                        }

                        if (permi != null)
                        {
                            foreach (var item in permi)
                            {

                                if (item.fldApplicationPartID.Contains("Charkhe"))
                                {
                                    item.fldApplicationPartID = item.fldApplicationPartID.Substring(7);
                                   m.prs_tblPermissionCharkheInsert( Convert.ToInt32(item.fldApplicationPartID),UserGroupID,"",user.UserInputId);
                                }
                                else
                                {
                                    m.prs_tblPermissionInsert(item.fldUserGroupID, Convert.ToInt32(item.fldApplicationPartID), user.UserInputId, "");
                                    if (item.fldTimeLimit != 0)
                                    {

                                        m.prs_tblTimeLimit_UserGroupInsert(Convert.ToInt32(item.fldApplicationPartID), item.fldTimeLimit, UserGroupID, user.UserInputId);

                                    }
                                }
                                  
                                
                                Msg = "ذخیره با موفقیت انجام شد.";
                                MsgTitle = "ذخیره موفق";
                            }
                        }
                    }
                    //else if (q.Count == 0 && permi == null)
                    //{
                    //    return Json(new
                    //    {
                    //        Msg = "لطفا دسترسی های مورد نظر را انتخاب نمایید.",
                    //        MsgTitle = "خطا",
                    //        Er = 1
                    //    }, JsonRequestBehavior.AllowGet);
                    //}

                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();

                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
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
                if (Permission.haveAccess(10,"",null))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    if (node == "165")
                    {
                        var child = m.prs_ApplictionPart_Charkhe(user.UserInputId).ToList();
                        foreach (var ch in child)
                        {
                            Node childNode = new Node();
                            childNode.Text = ch.fldName;
                            childNode.NodeID = "Charkhe" + ch.fldId.ToString();
                            childNode.Icon = Ext.Net.Icon.Building;
                            childNode.Leaf = true;
                            var f = m.prs_tblPermissionCharkheSelect("fldCharkheId", ch.fldId.ToString(), Convert.ToInt32(GrohKarbari),0).FirstOrDefault();
                            if (f == null)
                            {
                                childNode.Checked = false;
                            }
                            else
                            {
                                childNode.Checked = true;
                            }
                            childNode.AttributesObject = new
                            {
                                fldID = "Charkhe" + ch.fldId.ToString(),
                                fldTitle = ch.fldName,
                                fldTimeLimit = false,
                                fldTime = "",
                                fldIsLimit = false,
                                fldMinute = 0
                            };
                            nodes.Add(childNode);
                        }
                    }
                    else
                    {
                        var child = m.prs_tblApplicationPartSelect("fldPId", node, user.UserSecondId, Convert.ToInt32(GrohKarbari), 0).ToList();
                        foreach (var ch in child)
                        {
                            string Time = "";
                            bool IsLimit = false;
                            short Minute = 0;
                            Node childNode = new Node();
                            childNode.Text = ch.fldTitle;
                            childNode.NodeID = ch.fldID.ToString();
                            childNode.Icon = Ext.Net.Icon.Building;

                            var f = m.prs_tblPermissionSelect("fldApplicationPartID", ch.fldID.ToString(), Convert.ToInt32(GrohKarbari), 0).FirstOrDefault();
                            if (f == null)
                            {
                                childNode.Checked = false;
                            }
                            else
                            {
                                childNode.Checked = true;
                            }

                            if (ch.fldTimeLimit == true)//اگر جزو فیلدهایی بود که محدودیت براش تعریف شده بود
                            {

                                var TimeLimit = m.prs_tblTimeLimit_UserGroupSelect("Check", ch.fldID.ToString(), GrohKarbari, 1).FirstOrDefault();
                                if (TimeLimit == null)
                                {
                                    Time = "نامحدود";
                                    childNode.Icon = Ext.Net.Icon.ClockStop2;
                                }
                                else
                                {
                                    childNode.Icon = Ext.Net.Icon.ClockAdd;
                                    IsLimit = true;
                                    Minute = TimeLimit.fldTimeLimit;
                                    if (TimeLimit.fldTimeLimit >= 1440)
                                    {
                                        if (TimeLimit.fldTimeLimit % 1440 == 0)
                                            Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز";
                                        else
                                        {
                                            if (TimeLimit.fldTimeLimit % 1440 >= 60)
                                                if ((TimeLimit.fldTimeLimit % 1440) % 60 == 0)
                                                {
                                                    Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز و " + (TimeLimit.fldTimeLimit % 1440) / 60 + " ساعت";
                                                }
                                                else
                                                {
                                                    Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز و " + (TimeLimit.fldTimeLimit % 1440) / 60 + " ساعت و " +
                                                        (TimeLimit.fldTimeLimit % 1440) % 60 + " دقیقه";
                                                }
                                            else
                                                Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز و " + TimeLimit.fldTimeLimit % 1440 + " دقیقه";
                                        }
                                    }
                                    else if (TimeLimit.fldTimeLimit >= 60)
                                    {
                                        if (TimeLimit.fldTimeLimit % 60 == 0)
                                            Time = (TimeLimit.fldTimeLimit / 60).ToString() + " ساعت";
                                        else
                                            Time = (TimeLimit.fldTimeLimit / 60).ToString() + " ساعت و " + TimeLimit.fldTimeLimit % 60 + " دقیقه";
                                    }
                                    else
                                    {
                                        Time = TimeLimit.fldTimeLimit.ToString() + " دقیقه";
                                    }
                                }
                            }
                            childNode.AttributesObject = new
                            {
                                fldID = ch.fldID.ToString(),
                                fldTitle = ch.fldTitle,
                                fldTimeLimit = ch.fldTimeLimit,
                                fldTime = Time,
                                fldIsLimit = IsLimit,
                                fldMinute = Minute
                            };
                            nodes.Add(childNode);
                        }
                    }
                    return this.Direct(nodes);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }

        public ActionResult PermissionUser(int id, string containerId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            result.ViewBag.UserId = id;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadGroupTreePermissionforUser(string nod)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(14,"","0"))
                {
                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    Models.RaiSamEntities m = new RaiSamEntities();
                    UserInfo user = (UserInfo)(Session["User"]);
                   
                    var child = m.prs_tblApplicationPartSelect("fldPID_User", nod, user.UserSecondId, user.UserSecondId,0).ToList();

                    foreach (var ch in child)
                    {
                        Node childNode = new Node();
                        childNode.Text = ch.fldTitle;
                        childNode.NodeID = ch.fldID.ToString();
                        childNode.Icon = Ext.Net.Icon.UserMagnify;
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

        public ActionResult ShowUsers(int ApplicationPartId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.ApplicationPartId = ApplicationPartId;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadUsers(StoreRequestParameters parameters, int ApplicationPartId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(6,"","0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<Models.prs_GetName_applicationpart> data = null;
                    data = m.prs_GetName_applicationpart(ApplicationPartId).ToList();
                    return this.Store(data);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult UserforPermission(string containerId)
        {//باز شدن پنجره
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
        public ActionResult SavePermissionUser(string TimeLimit_User1, string AppIds, string CharkheIds, int UserId)
        {
            string Msg = "ذخیره با موفقیت انجام شد.",
            MsgTitle = "ذخیره موفق";
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
                
                try
                {
                    List<Models.prs_tblTimeLimit_UserSelect> TimeLimit_User = JsonConvert.DeserializeObject<List<prs_tblTimeLimit_UserSelect>>(TimeLimit_User1);
                    if (Permission.haveAccess(3,"","0"))
                    {
                       
                        var q = m.prs_tblUser_PermissionSelect("fldUserSelectId", UserId.ToString(), user.UserSecondId,0).ToList();
                        var q1 = m.prs_tblUser_PermissionCharkheSelect("fldUserSelectId", UserId.ToString(),0).ToList();

                        if (q.Count == 0 && q1.Count == 0 && AppIds == "" && CharkheIds == "")
                        {
                            return Json(new
                            {
                                Msg = "لطفا دسترسی های مورد نظر را انتخاب نمایید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (q.Count() != 0)
                            {
                                m.prs_tblUser_PermissionDelete(UserId, user.UserInputId);
                                m.prs_tblTimeLimit_UserDelete(UserId, user.UserInputId);
                            }

                            if (q1.Count() != 0)
                            {
                                 m.prs_tblUser_PermissionCharkheDelete(UserId, user.UserInputId);
                                
                            }

                            if (AppIds != "")
                            {
                                
                                m.prs_tblUser_PermissionInsert(UserId,AppIds, user.UserInputId,"");
                                
                                foreach (var item in TimeLimit_User)
                                {
                                  
                                    m.prs_tblTimeLimit_UserInsert(item.fldAppId,item.fldTimeLimit, user.UserSecondId, user.UserInputId);
                                    
                                }
                            }
                            if (CharkheIds != "")
                            {
                                m.prs_tblUser_PermissionCharkheInsert(UserId, CharkheIds, user.UserInputId,"");
                                
                            }
                            
                        }
                        //else if (q.Count == 0 && AppIds == "")
                        //{
                        //    return Json(new
                        //    {
                        //        Msg = "لطفا دسترسی های مورد نظر را انتخاب نمایید.",
                        //        MsgTitle = "خطا",
                        //        Er = 1
                        //    }, JsonRequestBehavior.AllowGet);
                        //}
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
        public ActionResult NodeLoadPermissionUser(string node, int UserId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(12, "", "0"))
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    if (node == "165")
                    {
                        var child = m.prs_ApplictionPart_Charkhe(user.UserInputId).ToList();
                        foreach (var ch in child)
                        {
                            Node childNode = new Node();
                            childNode.Text = ch.fldName;
                            childNode.NodeID = "Charkhe" + ch.fldId.ToString();
                            childNode.Icon = Ext.Net.Icon.Building;
                            childNode.Leaf = true;
                            var f = m.prs_User_PermissionCharkheIdSelect(user.UserInputId).Where(l => l.fldCharkheId == ch.fldId).FirstOrDefault();
                            if (f == null)
                            {
                                childNode.Checked = false;
                            }
                            else
                            {
                                childNode.Checked = true;
                            }
                            childNode.AttributesObject = new
                            {
                                fldID = "Charkhe" + ch.fldId.ToString(),
                                fldTitle = ch.fldName,
                                fldTimeLimit = false,
                                fldTime = "",
                                fldIsLimit = false,
                                fldMinute = 0
                            };
                            nodes.Add(childNode);
                        }
                    }

                    else
                    {
                        var child = m.prs_tblApplicationPartSelect("fldPID_User", node, user.UserSecondId, UserId, 0).ToList();

                        foreach (var ch in child)
                        {
                            string Time = "";
                            bool IsLimit = false;
                            short Minute = 0;
                            Node childNode = new Node();
                            childNode.Text = ch.fldTitle;
                            childNode.NodeID = ch.fldID.ToString();
                            childNode.Icon = Ext.Net.Icon.Building;

                            var f = m.prs_User_PermissionAppIdSelect("PermissionUser", "", UserId, user.UserSecondId, "", "0").Where(l => l.fldAppId == ch.fldID).FirstOrDefault();
                            if (f == null)
                            {
                                childNode.Checked = false;
                            }
                            else
                            {
                                childNode.Checked = true;
                            }

                            if (ch.fldTimeLimit == true)//اگر جزو فیلدهایی بود که محدودیت براش تعریف شده بود
                            {

                                var TimeLimit = m.prs_tblTimeLimit_UserSelect("Check", ch.fldID.ToString(), UserId.ToString(), 1).FirstOrDefault();
                                if (TimeLimit == null)
                                {
                                    Time = "نامحدود";
                                    childNode.Icon = Ext.Net.Icon.ClockStop2;
                                }
                                else
                                {
                                    childNode.Icon = Ext.Net.Icon.ClockAdd;
                                    IsLimit = true;
                                    Minute = TimeLimit.fldTimeLimit;
                                    if (TimeLimit.fldTimeLimit >= 1440)
                                    {
                                        if (TimeLimit.fldTimeLimit % 1440 == 0)
                                            Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز";
                                        else
                                        {
                                            if (TimeLimit.fldTimeLimit % 1440 >= 60)
                                                if ((TimeLimit.fldTimeLimit % 1440) % 60 == 0)
                                                {
                                                    Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز و " + (TimeLimit.fldTimeLimit % 1440) / 60 + " ساعت";
                                                }
                                                else
                                                {
                                                    Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز و " + (TimeLimit.fldTimeLimit % 1440) / 60 + " ساعت و " +
                                                        (TimeLimit.fldTimeLimit % 1440) % 60 + " دقیقه";
                                                }
                                            else
                                                Time = (TimeLimit.fldTimeLimit / 1440).ToString() + " روز و " + TimeLimit.fldTimeLimit % 1440 + " دقیقه";
                                        }
                                    }
                                    else if (TimeLimit.fldTimeLimit >= 60)
                                    {
                                        if (TimeLimit.fldTimeLimit % 60 == 0)
                                            Time = (TimeLimit.fldTimeLimit / 60).ToString() + " ساعت";
                                        else
                                            Time = (TimeLimit.fldTimeLimit / 60).ToString() + " ساعت و " + TimeLimit.fldTimeLimit % 60 + " دقیقه";
                                    }
                                    else
                                    {
                                        Time = TimeLimit.fldTimeLimit.ToString() + " دقیقه";
                                    }
                                }
                            }
                            childNode.AttributesObject = new
                            {
                                fldID = ch.fldID.ToString(),
                                fldTitle = ch.fldTitle,
                                fldTimeLimit = ch.fldTimeLimit,
                                fldTime = Time,
                                fldIsLimit = IsLimit,
                                fldMinute = Minute
                            };
                            nodes.Add(childNode);
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
    }
}
