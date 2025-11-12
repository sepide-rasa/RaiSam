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

namespace RaiSam.Controllers.Contancts
{
    public class ContactGroupTreeController : Controller
    {
        //
        // GET: /ContactGroupTree/

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
        public ActionResult GetContactGroup()
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
                var q = m.prs_tblContactGroupSelect("","",0).Select(l => new { ID = l.fldId, fldName = l.fldNameGroup });
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
        public FileContentResult ShowHelpContactGroupTree()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "23", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinContactGroupTree()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoContactGroupTree()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "23", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadContactGroupTree(string nod, string GroupId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(112,"","0"))
                {
                    int IdGroup = 0;
                    if (GroupId != "")
                        IdGroup = Convert.ToInt32(GroupId);
                    NodeCollection nodes = new Ext.Net.NodeCollection();
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var q = m.prs_tblTreeGroupSelect("fldPId", nod, IdGroup, 0).ToList();
                    foreach (var item in q)
                    {
                        Node asyncNode = new Node();
                        asyncNode.NodeID = item.fldId.ToString();
                        asyncNode.Text = item.fldTitle;
                        //asyncNode.AttributesObject = new
                        //{
                        //    fldId = item.fldId,
                        //    fldTitle = item.fldTitle
                        //};
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
        public ActionResult MoveNodes(int childId, int parentId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; int? PId = null;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    if (parentId == 0)
                        PId = null;
                    else
                        PId = parentId;
                    if (Permission.haveAccess(114, "tblTreeGroup", childId.ToString()))
                    {
                    m.prs_tblTreeGroupUpdate("PId", "", childId, 0, user.UserInputId,PId);

                    Msg = "ویرایش با موفقیت انجام شد.";
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
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "ContactGroupTree:MoveNodes: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogUpdate(user.UserInputId, "Cnt.tblContactGroup", jsonstr, false, childId);
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
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblTreeGroupSelect("fldId",Id.ToString(),0,1).FirstOrDefault();
            return Json(new
            {
                fldId = q.fldId,
                fldPId = q.fldPId,
                fldGroupId = q.fldGroupId.ToString(),
                fldTitle = q.fldTitle
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; int Er = 0; var checkDel = false;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var q = m.prs_tblTreeGroupSelect("fldId", id.ToString(), 0, 1).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("گروه تماس", q.fldNameGroup);
                parameters.Add("عنوان", q.fldTitle);
                try
                {
                    if (Permission.haveAccess(115, "tblTreeGroup", id.ToString()))
                    {
                        var Na = m.prs_tblContact_TreeGroupSelect("fldTreeGroupId", id.ToString(), 0, 0).FirstOrDefault();
                        if (Na != null)
                            checkDel = true;

                        if (checkDel)
                        {
                            parameters.Add("متن خطا", "ContactGroupTree:Delete: " + "ایتم انتخاب شده قبلا در سیستم استفاده شده است.");
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_LogInsert(user.UserInputId, "Cnt.tblTreeGroup", jsonstr, false);
                            return Json(new
                            {
                                Msg = "آیتم انتخاب شده قبلا در سیستم استفاده شده است.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                        m.prs_tblTreeGroupDelete(id);

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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "ContactGroupTree:Delete: " + ErMsg);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "Cnt.tblTreeGroup", jsonstr, false, id);
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
        public ActionResult Save(prs_tblTreeGroupSelect TreeGroup)
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
                var Change = 0; int? fldTimeStamp;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    if (TreeGroup.fldPId == 0)
                        TreeGroup.fldPId = null;
                    if (TreeGroup.fldId == 0)
                    {
                        if (Permission.haveAccess(114, "tblTreeGroup", null))
                        {
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("گروه تماس", TreeGroup.fldNameGroup);
                            parameters.Add("عنوان", TreeGroup.fldTitle);
                            //parameters.Add("عنوان گره پدر", TreeGroup.NameTree);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblTreeGroupInsert(TreeGroup.fldTitle, TreeGroup.fldGroupId, TreeGroup.fldPId, user.UserInputId, jsonstr);


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
                        if (Permission.haveAccess(115, "tblTreeGroup", TreeGroup.fldId.ToString()))
                        {
                            m.prs_tblTreeGroupUpdate("Id", TreeGroup.fldTitle, TreeGroup.fldId, TreeGroup.fldGroupId, user.UserInputId, TreeGroup.fldPId);

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
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("گروه تماس", TreeGroup.fldNameGroup);
                    parameters.Add("عنوان", TreeGroup.fldTitle);
                    //parameters.Add("عنوان گره پدر", TreeGroup.NameTree);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "ContactGroupTree:Save: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    if (TreeGroup.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "Cnt.tblTreeGroup", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "Cnt.tblTreeGroup", jsonstr, false, TreeGroup.fldId);
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
    }
}
