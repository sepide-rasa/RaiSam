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
    public class SpecificCartablePermission_UserGroupController : Controller
    {
        //
        // GET: /SpecificCartablePermission_UserGroup/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            Models.RaiSamEntities m = new RaiSamEntities();
            var userGroupId = m.prs_tblUserGroupSelect("ByUserId", "", user.UserSecondId,0).FirstOrDefault().fldID;
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            result.ViewBag.userGroupId = userGroupId;
            return result;
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpSpecificCartablePer()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            var Help = m.prs_tblHelpSelect("fldId", "27", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinSpecificCartablePer()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoSpecificCartablePer()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "27", 1).FirstOrDefault();

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
                var q = m.prs_tblUserGroupSelect("ByUserId", "", user.UserSecondId, 0).Select(n => new { ID = n.fldID, Name = n.fldTitle });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadCartabl(string nod, byte Typee, int UserGroupId, byte Level, int CartableIdd)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(160,"","0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    var fldShowGeneral = false;
                    var fldShowSpecific = false;
                    var fldStatus = false;
                    var fldEffective = false;
                    var url = "/Content/icon/mini/کارتابل.png";

                    var FieldName = "";

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
                            FieldName = "GetOperation_CharkheEghdamId";
                            url = "/Content/icon/mini/اکشن.png";
                            break;
                    }
                    var q = m.prs_SpecificCartablePermissionTreeSelect(FieldName, nod, UserGroupId, user.UserSecondId, CartableIdd).ToList();
                    foreach (var item in q)
                    {
                        Node asyncNode = new Node();
                        if (Typee == 3)
                        {
                            asyncNode.Leaf = true;

                            var OP_AC = m.prs_tblOpertion_ActionSelect("fldId", item.NodeId.ToString(),"","",1).FirstOrDefault();

                            

                            var op = m.prs_tblOperationSelect("fldId", OP_AC.fldOpertionId.ToString(),0,0).FirstOrDefault();
                            fldShowGeneral = op.fldUsable;
                            fldShowSpecific = Convert.ToBoolean(op.fldSpecificShow);
                            fldStatus = op.fldGroup;
                            fldEffective = op.fldeffective;
                        }
                        asyncNode.NodeID = item.NodeId.ToString() + item.NodeType + CartableIdd;
                        asyncNode.Text = item.NodeTitle;
                        asyncNode.IconFile = url;
                        asyncNode.Checked = item.Accessed;
                        if (Level == 0)
                        {
                            asyncNode.Expanded = true;
                        }
                        else
                        {
                            asyncNode.Expanded = false;
                        }
                        asyncNode.AttributesObject = new
                        {
                            fldNodeId = item.NodeId,
                            fldNodeName = item.NodeTitle,
                            fldNodeType = item.NodeType.ToString(),
                            fldShowGeneral = fldShowGeneral,
                            fldShowSpecific = fldShowSpecific,
                            fldStatus = fldStatus,
                            fldEffective = fldEffective,
                            CartablId = item.CartablId
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string permi1, int UserGroupID)
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
                    if (Permission.haveAccess(161, "tblUserGroup", null))
                    {
                    List<prs_tblSpecificCartablePermission_UserGroupSelect> permi = JsonConvert.DeserializeObject<List<prs_tblSpecificCartablePermission_UserGroupSelect>>(permi1);
                    
                    var q = m.prs_tblSpecificCartablePermission_UserGroupSelect("fldUserGroupId", UserGroupID.ToString(), user.UserSecondId,0).ToList();
                    if (q.Count() != 0)
                    {
                        m.prs_tblSpecificCartablePermission_UserGroupDelete(UserGroupID,user.UserInputId);
                       
                    }

                    if (permi != null)
                    {
                        foreach (var item in permi)
                        {
                            m.prs_tblSpecificCartablePermission_UserGroupInsert(UserGroupID,item.fldOperation_ActionId,user.UserInputId,"");
                           
                        }
                    }
                    else if (q.Count == 0 && permi == null)
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
        public JsonResult ReloadDrp()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblUserGroupSelect("ByUserId", "", user.UserSecondId,0).FirstOrDefault();

                return Json(new
                {
                    fldUserGroupId = q.fldID.ToString(),

                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
