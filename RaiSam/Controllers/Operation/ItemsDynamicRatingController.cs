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
    public class ItemsDynamicRatingController : Controller
    {
        //
        // GET: /ItemsDynamicRating/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //if (servic.Permission(44, Session["Username"].ToString(), Session["Password"].ToString(), out Err))
            //{
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;
            //}
            //else
            //{
            //    return null;
            //}
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpItemsDynamicRating()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "48", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinItemsDynamicRating()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoItemsDynamicRating()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "48", 1).FirstOrDefault();
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
                PartialView.ViewBag.id = id;//->HadafId
                return PartialView;
            }
        }
        public ActionResult GetHadaf()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblHadafSabtNamSelect("", "", 0).ToList().Select(n => new { ID = n.fldId, Name = n.fldTitle });
            return this.Store(q);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string ItemDynamic1, int HadafId)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                List<prs_tblItemsDynamicRatingSelect> ItemDynamic = JsonConvert.DeserializeObject<List<prs_tblItemsDynamicRatingSelect>>(ItemDynamic1);

                foreach (var item in ItemDynamic)
                {
                    if (item.fldId == 0)
                    {
                        if (Permission.haveAccess(231, "", "0"))
                        {
                            Dictionary<string, object> parameters = new Dictionary<string, object>();

                            parameters.Add("عنوان هدف", item.fldTitleHadaf);
                            parameters.Add("عنوان آیتم", item.fldTitleRatingDynamic);
                            parameters.Add("کپی", item.fldIsCopy);
                            parameters.Add("الزامی", item.fldElzami);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "دخیره موفق";

                            m.prs_tblItemsDynamicRatingInsert(item.fldHadafId, item.fldDynamicRatingId, item.fldIsCopy, item.fldElzami, u.UserInputId, jsonstr);
                        
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به ذخیره اطلاعات نمی باشد.",
                                MsgTitle = "خطا",
                                Err = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (Permission.haveAccess(232, "", "0"))
                        {
                            MsgTitle = "ویرایش موفق";
                            Msg = "ویرایش با موفقیت انجام شد.";

                            m.prs_tblItemsDynamicRatingUpdate(item.fldId, item.fldHadafId, item.fldDynamicRatingId, item.fldIsCopy, item.fldElzami, u.UserInputId);
                         
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به ویرایش اطلاعات نمی باشد.",
                                MsgTitle = "خطا",
                                Err = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
                Er = 1;
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItemsDynamicRating(int id, int State)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            //State=1 ->id=کد جدول ,State=2 ->id=کد درخت
            try
            {
                //حذف
                Models.RaiSamEntities m = new RaiSamEntities();

                if (Permission.haveAccess(233, "", "0"))
                {
                    Msg = "حذف با موفقیت انجام شد";
                    MsgTitle = "حذف موفق";
                    if (State == 1)
                    {
                         m.prs_tblItemsDynamicRatingDelete(id, "fldId");
                    }
                    else if (State == 2)
                    {
                         m.prs_tblItemsDynamicRatingDelete(id, "fldHadafId");
                    }
                }
                else
                {
                    MsgTitle = "خطا";
                    Msg = "شما مجاز به حذف اطلاعات نمی باشد.";
                    Er = 1;
                }
             
            }
            catch (Exception x)
            {
                MsgTitle = "خطا";
                Msg = "آیتم موردنظر دارای مقدار می باشد و قابل حذف نیست.";
                Er = 1;
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
        public ActionResult Details(int HadafId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblItemsDynamicRatingSelect("fldHadafId", HadafId.ToString(), 100).ToList();
            bool IsCopy = false; string CopyName = ""; int Id = 0; string fldTitleDynamic = ""; bool Elzami = false; string ElzamiName = "";
            string fldDynamicRatingId = ""; string HadafID = ""; string Desc = "";
            var ItemDynamicRatingID = "";
            if (q.Count != 0)
            {
                foreach (var item in q)
                {
                    IsCopy = item.fldIsCopy;
                    CopyName = item.fldCopyName;
                    Id = item.fldId;
                    fldTitleDynamic = fldTitleDynamic + item.fldTitleRatingDynamic + ";";
                    Elzami = item.fldElzami;
                    ElzamiName = item.fldElzamiName;
                    fldDynamicRatingId = fldDynamicRatingId + item.fldDynamicRatingId + ";";
                    HadafID = item.fldHadafId.ToString();
                    //Desc = item.fldDesc;
                    ItemDynamicRatingID = ItemDynamicRatingID + item.fldId + ";";
                }
            }
            return Json(new
            {
                IsCopy = IsCopy,
                CopyName=CopyName,
                fldId = Id,
                fldTitleDynamic = fldTitleDynamic,
                Elzami = Elzami,
                ElzamiName=ElzamiName,
                fldDynamicRatingId = fldDynamicRatingId,
                fldHadafId = HadafID.ToString(),
                ItemDynamicRatingID = ItemDynamicRatingID,
                fldDesc = Desc
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReloadDynamicRating(int HadafId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            List<Models.prs_tblItemsDynamicRatingSelect> data = null;
            if (Permission.haveAccess(230, "", "0"))
                data = m.prs_tblItemsDynamicRatingSelect("fldHadafId", HadafId.ToString(), 0).ToList();
            return Json(data.Select(c => new { fldId = c.fldId, fldDynamicRatingId = c.fldDynamicRatingId, fldIsCopy = c.fldIsCopy, fldElzami = c.fldElzami, fldTitleRatingDynamic = c.fldTitleRatingDynamic }), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Permission.haveAccess(230, "", "0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                List<Models.prs_NameItemDynamicRatingForHadafId> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<Models.prs_NameItemDynamicRatingForHadafId> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldNameItem":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "NameItem_fldNameItem";
                                break;
                            case "fldNameHadaf":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "NameItem_fldNameHadaf";
                                break;

                        }
                        if (data != null)
                            data1 = m.prs_NameItemDynamicRatingForHadafId(field, searchtext).ToList();
                        else
                            data = m.prs_NameItemDynamicRatingForHadafId(field, searchtext).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_NameItemDynamicRatingForHadafId("NameItem", "").ToList();
                }

                var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                //FilterConditions fc = parameters.GridFilters;

                //-- start filtering ------------------------------------------------------------
                //if (fc != null)
                //{
                //    foreach (var condition in fc.Conditions)
                //    {
                //        string field = condition.FilterProperty.Name;
                //        var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

                //        data.RemoveAll(
                //            item =>
                //            {
                //                object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                //                return !oValue.ToString().Contains(value.ToString());
                //            }
                //        );
                //    }
                //}
                //-- end filtering ------------------------------------------------------------
                //-- start paging ------------------------------------------------------------
                int limit = parameters.Limit;

                if ((parameters.Start + parameters.Limit) > data.Count)
                {
                    limit = data.Count - parameters.Start;
                }

                List<Models.prs_NameItemDynamicRatingForHadafId> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
                return null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsItemsDynamicRating()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblHadafSabtNamSelect("", "", 1).FirstOrDefault();
            return Json(new
            {
                fldHadafId = q.fldId.ToString(),

            }, JsonRequestBehavior.AllowGet);
        }

    }
}
