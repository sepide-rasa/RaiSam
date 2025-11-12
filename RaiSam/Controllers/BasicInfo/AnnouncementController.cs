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

namespace RaiSam.Controllers.BasicInfo
{
    public class AnnouncementController : Controller
    {
        //
        // GET: /Announcement/
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

        public ActionResult New(int Id, int state)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            PartialView.ViewBag.Tarikh = m.prs_GetDate().FirstOrDefault().fldTarikh;
            var TypeNamayesh = "";
            if (state == 0)
            {
                TypeNamayesh = "1";
                var h = m.prs_tblAnnouncement_HadafSelect("fldAnnounceId", Id.ToString(), 0).Any();
                if (h)
                    TypeNamayesh = "0";
            }
            else
                TypeNamayesh = (state - 1).ToString();

            PartialView.ViewBag.Id = Id;
            PartialView.ViewBag.TypeNamayesh = TypeNamayesh;
            return PartialView;
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public ActionResult LoadAllData(string AnnouncementId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
           
                var data = m.prs_tblHadafSabtNamSelect("", "", 0).ToList();
                var check = m.prs_tblAnnouncement_HadafSelect("fldAnnounceId", AnnouncementId, 0).ToList();
                int[] checkId = new int[check.Count];

                for (int i = 0; i < check.Count; i++)
                {
                    checkId[i] = check[i].fldHadafId;
                }
                return Json(new
                {
                    data = data,
                    checkId = checkId
                }, JsonRequestBehavior.AllowGet);
     
        }
        public FileContentResult ShowHelpAnnouncement()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "7", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinAnnouncement()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoAnnouncement()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "7", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult Download(int fldIDAnnouncement)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblAnnouncementManagerSelect("fldId_Details", fldIDAnnouncement.ToString(), u.UserInputId,1,"").FirstOrDefault();
            if (q != null && q.fldAttachment != null)
            {
                MemoryStream st = new MemoryStream(q.fldAttachment);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int fldAttachmentId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; byte Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();

            string jsonstr = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var Attach = m.prs_tblAnnouncementManagerAttachmentSelect("fldId", fldAttachmentId.ToString(),0).FirstOrDefault();
            if (Attach != null)
            {
                var q = m.prs_tblAnnouncementManagerSelect("fldId", Attach.fldAnnouncementID.ToString(),u.UserInputId,0,"").FirstOrDefault();
                var SeenMovazafName = "نیست";
                if (q.fldSeenMovazaf)
                    SeenMovazafName = "هست";

                parameters.Add("عنوان", q.fldTitle);
                parameters.Add("متن", q.fldMatn);
                parameters.Add("وضعیت", q.fldStatusName);
                parameters.Add("مشاهده توسط مأمورین موظف", SeenMovazafName);
                parameters.Add("توضیحات", q.fldDesc);
                parameters.Add("تاریخ", q.fldTarikhShamsi);
            }

            try
            {//حذف

                 m.prs_tblAnnouncementManagerDelete("Attachment", fldAttachmentId);
              
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(u.UserInputId, "dbo.tblAnnouncementManagerAttachment", jsonstr, true, fldAttachmentId);

                    Msg = "حذف با موفقیت انجام شد";
                    MsgTitle = "حذف موفق";
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
                parameters.Add("متن خطا", "Announcement:DeleteFile: " + InnerException);
                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                m.prs_LogDelete(u.UserInputId, "dbo.tblAnnouncementManagerAttachment", jsonstr, false, fldAttachmentId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; byte Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();

            string jsonstr = "";
            var q = m.prs_tblAnnouncementManagerSelect("fldId", Id.ToString(),u.UserInputId,0,"").FirstOrDefault();
            var SeenMovazafName = "نیست";
            if (q.fldSeenMovazaf)
                SeenMovazafName = "هست";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("عنوان", q.fldTitle);
            parameters.Add("متن", q.fldMatn);
            parameters.Add("وضعیت", q.fldStatusName);
            parameters.Add("مشاهده توسط مأمورین موظف", SeenMovazafName);
            parameters.Add("توضیحات", q.fldDesc);
            parameters.Add("تاریخ", q.fldTarikhShamsi);

            try
            {//حذف
                if (Permission.haveAccess(64, "dbo.tblAnnouncementManager", Id.ToString()))
                {
                    m.prs_tblAnnouncement_HadafDelete(Id);
                    m.prs_tblAnnouncementManagerDelete("AnnouncementManager", Id);

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogDelete(u.UserInputId, "dbo.tblAnnouncementManager", jsonstr, true, Id);

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
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                parameters.Add("کد خطا", ErrorId.Value);
                parameters.Add("متن خطا", "Announcement:Delete: " + InnerException);
                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                m.prs_LogDelete(u.UserInputId, "dbo.tblAnnouncementManager", jsonstr, false, Id);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
         
            var q = m.prs_tblAnnouncementManagerSelect("fldId", Id.ToString(),u.UserInputId,0,"").FirstOrDefault();
            var TypeNamayesh = "1";
            var h = m.prs_tblAnnouncement_HadafSelect("fldAnnounceId", Id.ToString(), 0).Any();
            if(h)
                TypeNamayesh = "0";
            return Json(new
            {
                fldId = q.fldID,
                fldTitle = q.fldTitle,
                fldMatn = q.fldMatn,
                fldTarikhShamsi = q.fldTarikhShamsi,
                fldStatus = Convert.ToByte(q.fldStatus).ToString(),
                fldStatusName = q.fldStatusName,
                fldSeenMovazaf = q.fldSeenMovazaf,
                TypeNamayesh = TypeNamayesh
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetTreeNahi(int Id)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    string path = "";
        //    string treeId = "";
        //    string IsSetadi = "0";

        //    param.FieldName = "fldAnn_ManagerId";
        //    param.Value = Id.ToString();
        //    param.Top = 0;

        //    var q = service.SelectAnnouncement_Tree(param, user.UserKey, IP).Announcement_TreeList.ToList();
        //    foreach (var item in q)
        //    {
        //        param.FieldName = "fldId";
        //        param.Value = item.fldTreeId.ToString();
        //        param.UserInfoId = user.UserInputId;
        //        param.UserId = user.UserSecondId;
        //        param.Top = 1;
        //        var tree = service.SelectTreeStructure(param, user.UserKey, IP).TreeStructurelist.FirstOrDefault();
        //        if (tree.fldNahiId != null)/*در سطح اداره کل*/
        //        {
        //            path = path + "/1/" + tree.fldId + ";";
        //            treeId = treeId + tree.fldId + ";";
        //        }
        //        else if (tree.fldStationId != null)/*در سطح ایستگاه*/
        //        {
        //            path = path + "/1/" + tree.fldPId + "/" + tree.fldId + ";";
        //            treeId = treeId + tree.fldId + ";";
        //        }
        //        else
        //        {
        //            IsSetadi = "1";
        //        }
        //    }
        //    return Json(new
        //    {
        //        IsSetadi = IsSetadi,
        //        path = path,
        //        treeId = treeId
        //    }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string Announcement1, string StationIdArray1, string HadafIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "", MsgTitle = ""; var Er = 0;
            byte[] file = null; var extension = "";
            Models.prs_tblAnnouncementManagerSelect Announcement = JsonConvert.DeserializeObject<Models.prs_tblAnnouncementManagerSelect>(Announcement1);
            Announcement.fldMatn = System.Uri.UnescapeDataString(Announcement.fldMatn);
            try
            {
                List<int> StationIdArray = JsonConvert.DeserializeObject<List<int>>(StationIdArray1);

                if (Announcement.fldDesc == null)
                    Announcement.fldDesc = "";
                if (Announcement.fldID == 0)
                {
                    if (Permission.haveAccess(62, "dbo.tblAnnouncementManager", null))
                    {
                        //ذخیره
                        if (Session["savePathAnnouncement"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathAnnouncement"].ToString()));
                            Announcement.fldAttachment = stream.ToArray();
                            Announcement.fldPasvand = Path.GetExtension(Session["savePathAnnouncement"].ToString()).ToLower();
                        }
                        System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldIDAnnouncementManager", typeof(int));

                        m.prs_tblAnnouncementManagerInsert(Id, Announcement.fldTitle, Announcement.fldMatn, Announcement.fldStatus, Announcement.fldAttachment, Announcement.fldDesc
                            , Convert.ToInt32(Announcement.fldTarikhShamsi.Replace("/", "")), Announcement.fldPasvand, Announcement.fldSeenMovazaf, u.UserInputId, IP);

                        Msg = "ذخیره با موفقیت انجام شد.";
                        MsgTitle = "دخیره موفق";

                        if (HadafIds != "")
                        {
                            var HadafIdsSplit = HadafIds.Split(',').Reverse().Skip(1).Reverse().ToList();
                            for (int i = 0; i < HadafIdsSplit.Count(); i++)
                                m.prs_tblAnnouncement_HadafInsert(Convert.ToInt32(Id.Value), Convert.ToInt32(HadafIdsSplit[i]), u.UserInputId);
                        }
                        else
                        {
                            foreach (var item in StationIdArray)
                            {
                                m.prs_tblAnnouncement_TreeInsert(Convert.ToInt32(Id.Value), "", item, u.UserInputId);

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
                else
                {
                    if (Permission.haveAccess(63, "dbo.tblAnnouncementManager", Announcement.fldID.ToString()))
                    {
                        //ویرایش
                        if (Session["savePathAnnouncement"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathAnnouncement"].ToString()));
                            Announcement.fldAttachment = stream.ToArray();
                            Announcement.fldPasvand = Path.GetExtension(Session["savePathAnnouncement"].ToString()).ToLower();
                        }
                        m.prs_tblAnnouncementManagerUpdate(Announcement.fldID, Announcement.fldTitle, Announcement.fldMatn, Announcement.fldAttachment, Announcement.fldStatus, Announcement.fldDesc
                            , Convert.ToInt32(Announcement.fldTarikhShamsi.Replace("/", "")), Announcement.fldPasvand, Announcement.fldSeenMovazaf, u.UserInputId);

                        var StatusName = "غیرفعال";
                        if (Announcement.fldStatus)
                            StatusName = "فعال";
                        var SeenMovazafName = "نیست";
                        if (Announcement.fldSeenMovazaf)
                            SeenMovazafName = "هست";
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("عنوان", Announcement.fldTitle);
                        parameters.Add("متن", Announcement.fldMatn);
                        parameters.Add("وضعیت", StatusName);
                        parameters.Add("مشاهده توسط مأمورین موظف", SeenMovazafName);
                        parameters.Add("توضیحات", Announcement.fldDesc);
                        parameters.Add("تاریخ", Announcement.fldTarikhShamsi);
                        string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                        m.prs_LogUpdate(u.UserInputId, "dbo.tblAnnouncementManager", jsonstr, true, Announcement.fldID);

                        Msg = "ویرایش با موفقیت انجام شد.";
                        MsgTitle = "ویرایش موفق";

                        m.prs_tblAnnouncementManagerDelete("DeleteTree", Announcement.fldID);

                        if (HadafIds != "")
                        {
                            m.prs_tblAnnouncement_HadafDelete(Announcement.fldID);
                            var HadafIdsSplit = HadafIds.Split(',').Reverse().Skip(1).Reverse().ToList();
                            for (int i = 0; i < HadafIdsSplit.Count(); i++)
                                m.prs_tblAnnouncement_HadafInsert(Announcement.fldID, Convert.ToInt32(HadafIdsSplit[i]), u.UserInputId);
                        }
                        foreach (var item in StationIdArray)
                        {
                            m.prs_tblAnnouncement_TreeInsert(Announcement.fldID, "", item, u.UserInputId);

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
                if (Session["savePathAnnouncement"] != null)
                {
                    System.IO.File.Delete(Session["savePathAnnouncement"].ToString());
                    Session.Remove("savePathAnnouncement");
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

                var StatusName = "غیرفعال";
                if (Announcement.fldStatus)
                    StatusName = "فعال";
                var SeenMovazafName = "نیست";
                if (Announcement.fldSeenMovazaf)
                    SeenMovazafName = "هست";
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان", Announcement.fldTitle);
                parameters.Add("متن", Announcement.fldMatn);
                parameters.Add("وضعیت", StatusName);
                parameters.Add("مشاهده توسط مأمورین موظف", SeenMovazafName);
                parameters.Add("توضیحات", Announcement.fldDesc);
                parameters.Add("تاریخ", Announcement.fldTarikhShamsi);
                parameters.Add("کد خطا", ErrorId.Value);
                parameters.Add("متن خطا", "Announcement:Save: " + InnerException);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (Announcement.fldID == 0)
                {
                    m.prs_LogInsert(u.UserInputId, "dbo.tblAnnouncementManager", jsonstr, false);
                }
                else
                {
                    m.prs_LogUpdate(u.UserInputId, "dbo.tblAnnouncementManager", jsonstr, false, Announcement.fldID);
                }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Permission.haveAccess(61,"","0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<prs_tblAnnouncementManagerSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<prs_tblAnnouncementManagerSelect> data1 = null;
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
                            case "fldMatn":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldMatn";
                                break;
                            case "fldTarikhShamsi":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTarikhShamsi";
                                break;
                            case "fldStatusName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldStatusName";
                                break;
                        }
                        if (data != null)
                            data1 = m.prs_tblAnnouncementManagerSelect(field, searchtext,u.UserInputId,100,"")/*.Where(l => l.fldType == State)*/.ToList();
                        else
                            data = m.prs_tblAnnouncementManagerSelect(field, searchtext, u.UserInputId, 100, "")/*.Where(l => l.fldType == State)*/.ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblAnnouncementManagerSelect("","",u.UserInputId,100,"")/*.Where(l => l.fldType == State)*/.ToList();
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

                List<prs_tblAnnouncementManagerSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { Code = "403" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NodeLoadTreeStructure(string nod, int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            NodeCollection nodes = new Ext.Net.NodeCollection();
            Models.RaiSamEntities m = new RaiSamEntities();
            List<prs_tblUser_StationSelect> child = new List<prs_tblUser_StationSelect>();

            //param.FieldName = "fldId";
            //param.Value = user.UserId.ToString();
            //param.UserInfoId = user.UserInputId;
            //param.UserId = user.UserSecondId;
            //param.Value2 = "";
            //param.Top = 1;
            //var User = service.SelectUser(param, user.UserKey, IP).UserList.FirstOrDefault();
            if (nod == "1")
            {
                child = m.prs_tblUser_StationSelect("fldNahi","",u.UserSecondId,0).ToList();
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
                child = m.prs_tblUser_StationSelect("fldStation", nod.Split(';')[1],u.UserSecondId,0).ToList();
                foreach (var ch in child)
                {
                    Node childNode = new Node();
                    childNode.Checked = false;
                    childNode.Text = ch.fldName;
                    childNode.Leaf = true;
                    childNode.NodeID = "S;" + ch.fldId.ToString();
                    childNode.Icon = Ext.Net.Icon.Building;

                    if (Id != 0)
                    {
                        var q = m.prs_tblAnnouncement_TreeSelect("fldCheck", Id.ToString(), ch.fldId,1).FirstOrDefault();
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
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "";
            try
            {
                if (Session["savePathAnnouncement"] != null)
                {
                    System.IO.File.Delete(Session["savePathAnnouncement"].ToString());
                    Session.Remove("savePathAnnouncement");
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var IsWord = FileInfoExtensions.IsWord(Request.Files[0]);
                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                var IsExcel = FileInfoExtensions.IsExcel(Request.Files[0]);

                if (IsImage == true || IsWord == true || IsPDF == true || IsExcel == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff"*/)
                {
                    if (Request.Files[0].ContentLength <= 1024000)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathAnnouncement"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "فایل با موفقیت آپلود شد.",
                            IsValid = true
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "حجم فایل انتخابی می بایست کمتر از یک مگابایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از یک مگابایت باشد."
                        //});
                        //DirectResult result = new DirectResult();
                        //return result;
                    }
                }
                else
                {
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName,
                        Message = "فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
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
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
        }

    }
}
