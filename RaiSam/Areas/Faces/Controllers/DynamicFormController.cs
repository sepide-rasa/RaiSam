using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using RaiSam.Models;
using Aspose.Pdf.DOM;

namespace RaiSam.Areas.Faces.Controllers
{
    public class DynamicFormController : Controller
    {
        //
        // GET: /Faces/DynamicForm/
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index(int State, string EnterSicleIds)//1=modiriati    2=omumiProj   3=ekhtesasiProj
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
           

            Models.RaiSamEntities m = new Models.RaiSamEntities();

            string TitleRatingDynamicIds = "";
            string Msg = "";
            int ReqId = 0;
            int FirstId = 0;
            var IsInClient = 0;

            //string RatingIds = "";
            try
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();

                if (EnterSicleIds != null)
                {
                    var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
                    var k = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
                    FirstId = k.fldFirstRegisterId;
                    ReqId = k.fldRequestId;
                }
                else
                {
                    FirstId = Convert.ToInt32(Session["FristRegisterId"]);
                    ReqId = Convert.ToInt32(Session["RankingId"]);

                }
                var re = m.prs_tblRequestRankingSelect("fldId", ReqId.ToString(), 0).FirstOrDefault();
                var q = m.prs_tblItemsDynamicRatingSelect("fldHadafId", re.fldHadafId.ToString(), 0).Where(l => l.fldGeneralRatingId == State).ToList();
                if (re.fldKartablId == 100) IsInClient = 1;


                //if (k.fldStatusId == 2 || k.fldStatusId == 4 || k.fldStatusId == 5)
                //{
                //    SentToAdmin = 1;
                //}
                //else
                //{
                //    int app = 20;
                //    if (State == 2) app = 21;
                //    var z = servic.SelectBeforeDashboard(Convert.ToInt32(Session["RankingId"]), app, IP, Convert.ToInt32(Session["FristRegisterId"]), out Err);
                //    if (z == false)
                //        SentToAdmin = 0;
                //    else
                //        SentToAdmin = 1;
                //}


                foreach (var item in q)
                {
                    var s = m.prs_ExistsTitleRating_CharkheEghdam(item.fldDynamicRatingId, ReqId).FirstOrDefault();
                    if (s.fldFlag == true)
                        TitleRatingDynamicIds = TitleRatingDynamicIds + item.fldDynamicRatingId + ";";
                    //RatingIds = RatingIds + item.fldId + ";";
                }
                PartialView.ViewBag.TitleRatingDynamicIds = TitleRatingDynamicIds;
                PartialView.ViewBag.State = State;

                PartialView.ViewBag.FirstId = FirstId;
                PartialView.ViewBag.ReqId = ReqId;
                PartialView.ViewBag.IsInClient = IsInClient;

                if (TitleRatingDynamicIds != "")
                {
                    return PartialView;
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "مدارک و مستنداتی برای بارگذاری ثبت نشده است."
                    });
                    X.Mask.Hide();
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = Msg
                });
                X.Mask.Hide();
                DirectResult result = new DirectResult();
                return result;
            }
        }
        public FileContentResult DownloadV(string state)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            string Name = "Modiriyati";
            string savePath = Server.MapPath(@"~\Uploaded\Help\Modiriyati.mp4");
            if (state == "2")
            {
                savePath = Server.MapPath(@"~\Uploaded\Help\Mohandesi.mp4");
                Name = "Mohandesi";
            }
            MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
            return File(st.ToArray(), MimeType.Get(".mp4"), "Help_" + Name + ".mp4");
        }
        public ActionResult GetInfoGrid(string fldTitleDynamicId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            string NamesKhasiyat_Fa = "";
            string NamesKhasiyat_En = "";
            string JenseKhasiyat = "";
            string NoeKhasiyatha = "";
            string ItemPropertiesId = "";
            string TitleDynamic = "";
            string ItemsDynamicRatingId = "";
            int IsInClient = 1;

            Models.RaiSamEntities m = new Models.RaiSamEntities();
            try
            {
                var s = m.prs_tblRequestRankingSelect("fldId", ReqId.ToString(), 0).FirstOrDefault();
                if (s.fldKartablId != 100) IsInClient = 0;

                var q = m.prs_tblItemDynamicPropertiesSelect("fldTitleDynamicId", fldTitleDynamicId, s.fldHadafId.ToString(),0).ToList();
                
                if (q.Count != 0)
                {
                    TitleDynamic = q[0].fldTitleRatingFA;
                    ItemsDynamicRatingId = q[0].fldItemsDynamicRatingId.ToString();
                    //  IsInClient = q[0].fldIsInClient.ToString();

                    foreach (var item in q)
                    {
                        NamesKhasiyat_Fa = NamesKhasiyat_Fa + item.fldNameKhasiyat_Fa + ";";
                        NamesKhasiyat_En = NamesKhasiyat_En + item.fldNameKhasiyat_En + item.fldItemsDynamicRatingId + ";";
                        ItemPropertiesId = ItemPropertiesId + item.fldId + ";";
                        NoeKhasiyatha = NoeKhasiyatha + item.fldStringType + ";";
                        JenseKhasiyat = JenseKhasiyat + item.fldJenseKhasiyat + ";";
                    }
                }
                else
                {
                    Er = 1;
                    MsgTitle = "خطا";
                    Msg = "مدارک و مستنداتی برای بارگذاری ثبت نشده است.";
                    return Json(new
                    {
                        Msg = Msg,
                        MsgTitle = MsgTitle,
                        Er = Er
                    }, JsonRequestBehavior.AllowGet);
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
                Er = Er,
                NamesKhasiyat_Fa = NamesKhasiyat_Fa,
                NamesKhasiyat_En = NamesKhasiyat_En,
                JenseKhasiyat = JenseKhasiyat,
                ItemPropertiesId = ItemPropertiesId,
                NoeKhasiyatha = NoeKhasiyatha,
                TitleDynamic = TitleDynamic,
                ItemsDynamicRatingId = ItemsDynamicRatingId,
                IsInClient = IsInClient
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read(string fldItemsDynamicRatingId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new Models.RaiSamEntities();
            IDictionary<string, object> aa = new Dictionary<string, object>();
            /*List<object> aa = new List<object>();*/

            List<IDictionary<string, object>> qq = new List<IDictionary<string, object>>();

            // var q = servic.GetItemDynamicPropertiesWithFilter("fldTitleDynamicId", idddd, Session["TreeId"].ToString(), 0, out Err).ToList();

            var data = m.prs_tblItemDynamic_ClientSelect("fldItemDynamicId", fldItemsDynamicRatingId, ReqId.ToString(), 0).ToList();

            if (data.Count != 0)
            {
                foreach (var item in data)
                {
                    aa = new Dictionary<string, object>();
                    var items = m.prs_tblItemProperties_ClientSelect("fldDynamicClientId", item.fldId.ToString(), 0).ToList();
                    var Idname = "fldId" + fldItemsDynamicRatingId;
                    var Statusname = "fldStatus" + fldItemsDynamicRatingId;
                    var IsInClientname = "fldIsInClient" + fldItemsDynamicRatingId;

                    if (items.Count != 0)
                    {
                        aa.Add(Idname, items[0].fldDynamicClientId);
                        aa.Add(Statusname, item.fldStatus);
                        //aa.Add(IsInClientname, item.fldIsInClient);
                        foreach (var h in items)
                        {
                            var name = h.fldNameKhasiyat_En + fldItemsDynamicRatingId;
                            //var x = h.fldMeghdar.Replace(Convert.ToChar((byte)0x01), ' ');
                            aa.Add(name, h.fldMeghdar);
                        }
                        qq.Add(aa);
                    }
                }
            }
            return this.Store(qq);
        }
        public ActionResult New(string NoeKhasiyatha, string fldIdDynamic_Client, string TitleDynamic, string fldItemsDynamicRatingIdd, string types, string TitleItems, string ItemPropertiesId, int FirstId, int ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.TitleDynamic = TitleDynamic;
            result.ViewBag.types = types;
            result.ViewBag.TitleItems = TitleItems;
            result.ViewBag.ItemPropertiesIds = ItemPropertiesId;
            result.ViewBag.fldItemsDynamicRatingIdd = fldItemsDynamicRatingIdd;
            result.ViewBag.NoeKhasiyatha = NoeKhasiyatha;
            result.ViewBag.fldIdDynamic_Client = fldIdDynamic_Client;

            var IdPropNaghshe = 0;
            Models.RaiSamEntities m = new Models.RaiSamEntities();
            var q1 = m.prs_tblItemsDynamicRatingSelect("fldId", fldItemsDynamicRatingIdd, 0).FirstOrDefault();
            var q2 = m.prs_tblItemDynamicPropertiesSelect("fldTitleDynamicId_JenseKhasiyat", q1.fldDynamicRatingId.ToString(), "6", 0).FirstOrDefault();
            if (q2 != null)
                IdPropNaghshe = q2.fldId;
            result.ViewBag.IdPropNaghshe = IdPropNaghshe;

            result.ViewBag.FirstId = FirstId;
            result.ViewBag.ReqId = ReqId;

            var q3 = m.prs_tblTitleRatingDynamicSelect("fldId", q1.fldDynamicRatingId.ToString(), 0).FirstOrDefault();
            result.ViewBag.NoeFile = q3.fldFormatName;
            result.ViewBag.NoeFilePass = q3.fldPassvand;
            return result;
        }

        public ActionResult ShowConversationDynamicForm(string Status, string Id, string StateAORC, string gridId, string StatusRequest, string UserNameAdmin, string PassAdmin, string TypeGrid)
        {
            int Permission = 0;
            if (StateAORC == "0")
            {
                if (Session["FristRegisterId"] == null)
                    return RedirectToAction("Logon", "Account", new { area = "" });
            }
            else
            {
                if (Session["Username"] == null)
                    return RedirectToAction("Logon", "Account", new { area = "" });
            }
            //if (Status != "")
            //{
            //    if (StatusRequest == "Completed")
            //    {
            //        if (!servic1.Permission(228, UserNameAdmin, PassAdmin, out Err1))
            //        {
            //            Permission = 1;
            //        }
            //    }
            //    if (Permission == 1)
            //    {
            //        X.Msg.Show(new MessageBoxConfig
            //        {
            //            Buttons = MessageBox.Button.OK,
            //            Icon = MessageBox.Icon.ERROR,
            //            Title = "خطا",
            //            Message = "شما مجاز به دسترسی نمی باشید."
            //        });
            //        DirectResult result = new DirectResult();
            //        return result;
            //    }
            //}

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.fldItemClientId = Id;
            PartialView.ViewBag.gridId = gridId;
            PartialView.ViewBag.StateAORC = StateAORC;
            PartialView.ViewBag.StatusRequest = StatusRequest;
            PartialView.ViewBag.UserNameAdmin = UserNameAdmin;
            PartialView.ViewBag.PassAdmin = PassAdmin;
            PartialView.ViewBag.Status = Status;
            PartialView.ViewBag.TypeGrid = TypeGrid;
            return PartialView;
        }

        public ActionResult ShowMsgDynamic(string Message, string SenderId, string TypeMsgName, string TypeMsg, bool IsGrade, int Id, int FileId, string StateAORC)
        {
            //if (StateAORC == "0")
            //{
            //    if (Session["FristRegisterId"] == null)
            //        return RedirectToAction("Logon", "Account", new { area = "" });
            //    if (SenderId != "0")
            //    {
            //        servic.UpdateStatusMsgItemDynamic_Client("ItemDynamic_Client", Id, true, out Err);
            //    }
            //}
            //else
            //{
            //    if (Session["Username"] == null)
            //        return RedirectToAction("Login", "Admin", new { area = "faces" });
            //    if (SenderId == "0")
            //    {
            //        servic.UpdateStatusMsgItemDynamic_Client("ItemDynamic_Client", Id, true, out Err);
            //    }
            //}
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Message = Message;
            PartialView.ViewBag.TypeMsgName = TypeMsgName;
            PartialView.ViewBag.IsGrade = IsGrade;
            PartialView.ViewBag.StateAORC = StateAORC;
            PartialView.ViewBag.SenderId = SenderId;
            PartialView.ViewBag.TypeMsg = TypeMsg;
            PartialView.ViewBag.FileId = FileId;
            return PartialView;
        }

        //public ActionResult GetStatusChatDynamic(string RequestId, string fldItemClientId, string TypeGrid)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    string UserName = ""; string Pass = "";
        //    if (Session["Username"] != null || Session["Password"] != null)
        //    {
        //        UserName = Session["Username"].ToString();
        //        Pass = Session["Password"].ToString();
        //    }

        //var q = servic1.GetStatusChat("ItemDynamic_Client", Convert.ToInt32(fldItemClientId), Convert.ToInt32(RequestId), Convert.ToInt32(TypeGrid), UserName, Pass, out Err1).ToList().Select(c => new { fldStatusId = c.Id, fldStatusName = c.fldName });
        //if (Err1.ErrorType)
        //{
        //    return null;
        //}
        //    return this.Store(q);
        //}
        public ActionResult NewCh_Dynamic(string Status, string ItemClientId, string StateAORC, string gridId, string StatusRequest, string UserNameAdmin, string PassAdmin, string TypeGrid)
        {
            int Permission = 0;
            if (StateAORC == "0")
            {
                if (Session["FristRegisterId"] == null)
                    return RedirectToAction("Logon", "Account", new { area = "" });
            }
            else
            {
                if (Session["Username"] == null)
                    return RedirectToAction("Logon", "Account", new { area = "" });
            }
            //if (Status != "")
            //{
            //    if (StatusRequest == "Completed")
            //    {
            //        if (!servic1.Permission(228, UserNameAdmin, PassAdmin, out Err1))
            //        {
            //            Permission = 1;
            //        }
            //    }
            //    if (Permission == 1)
            //    {
            //        X.Msg.Show(new MessageBoxConfig
            //        {
            //            Buttons = MessageBox.Button.OK,
            //            Icon = MessageBox.Icon.ERROR,
            //            Title = "خطا",
            //            Message = "شما مجاز به دسترسی نمی باشید."
            //        });
            //        DirectResult result = new DirectResult();
            //        return result;
            //    }
            //}

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.ItemClientId = ItemClientId;
            PartialView.ViewBag.StateAORC = StateAORC;
            PartialView.ViewBag.gridId = gridId;
            PartialView.ViewBag.TypeGrid = TypeGrid;
            return PartialView;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SaveNewCh_DynamicForm(OBJ_Ch_ItemDynamic_Client ChatDynamic, string StateAORC)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    string Msg = "";
        //    string MsgTitle = ""; byte Er = 0; string fldMsg = "";
        //    var Date = servic.Getdate().fldTarikh;
        //    var Time = servic.Getdate().fldTime;
        //    byte[] report_file = null; string FileName = ""; string Pasvand = "";
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        if (Session["HadafId"] == null)
        //            return RedirectToAction("Index", "Admin");
        //        try
        //        {
        //            Msg = servic.InsertCh_ItemDynamic_Client(false, null, ChatDynamic.fldMsg, Date, Time, 1, null, ChatDynamic.fldItemClientId, "", Convert.ToInt32(Session["RankingId"]), IP, null, null, out Err);
        //            MsgTitle = "ذخیره موفق";
        //            if (Err.ErrorType)
        //            {
        //                Er = 1;
        //                MsgTitle = "خطا";
        //                Msg = Err.ErrorMsg;
        //                return Json(new
        //                {
        //                    Msg = Msg,
        //                    MsgTitle = MsgTitle,
        //                    Er = Er
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (Exception x)
        //        {
        //            if (x.InnerException != null)
        //                Msg = x.InnerException.Message;
        //            else
        //                Msg = x.Message;

        //            MsgTitle = "خطا";
        //            Er = 1;
        //        }
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        try
        //        {
        //            if (ChatDynamic.fldTypeMsg == 4 || ChatDynamic.fldTypeMsg == 5)
        //            {
        //                var dd = servic.GetItemDynamic_ClientDetail(ChatDynamic.fldItemClientId, IP, out Err);
        //                if (dd.fldidTitleRatingDynamic == 14 || dd.fldidTitleRatingDynamic == 17)//آرشیو و R&D
        //                {
        //                    var chekMomayez = servic1.CheckRequestFormomayezi(Convert.ToInt32(StateAORC), Session["Username"].ToString(), (Session["Password"].ToString()), IP, out Err1);
        //                    if (chekMomayez == false)
        //                        return Json(new
        //                        {
        //                            Msg = "لطفا ابتدا افراد ممیز را انتخاب و فایل گزارش آن ها را آپلود نمایید.",
        //                            MsgTitle = "خطا",
        //                            Er = 1,
        //                        }, JsonRequestBehavior.AllowGet);
        //                }
        //            }

        //            if (Session["savePathChat"] != null)
        //            {
        //                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathChat"].ToString()));
        //                report_file = stream.ToArray();
        //                

        //                Pasvand = Path.GetExtension(Session["savePathChat"].ToString());
        //            }
        //            //else
        //            //{
        //            //    return Json(new
        //            //    {
        //            //        Msg = "لطفا ابتدا فایل را وارد نمایید.",
        //            //        MsgTitle = "خطا",
        //            //        Err = 1,
        //            //    }, JsonRequestBehavior.AllowGet);
        //            //}
        //            if (ChatDynamic.fldMsg != null)
        //            {
        //                fldMsg = ChatDynamic.fldMsg;
        //            }
        //            Msg = servic.InsertCh_ItemDynamic_Client(false, Convert.ToInt32(Session["UserId"]), fldMsg, Date, Time,
        //                ChatDynamic.fldTypeMsg, null, ChatDynamic.fldItemClientId, "", Convert.ToInt32(StateAORC), IP, report_file, Pasvand, out Err);
        //            MsgTitle = "ذخیره موفق";
        //            if (Err.ErrorType)
        //            {
        //                Er = 1;
        //                MsgTitle = "خطا";
        //                Msg = Err.ErrorMsg;
        //                if (Session["savePathChat"] != null)
        //                {
        //                    string physicalPath = System.IO.Path.Combine(Session["savePathChat"].ToString());
        //                    Session.Remove("savePathChat");
        //                    Session.Remove("FileNameChat");
        //                    System.IO.File.Delete(physicalPath);
        //                }
        //                return Json(new
        //                {
        //                    Msg = Msg,
        //                    MsgTitle = MsgTitle,
        //                    Er = Er
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //            if (Session["savePathChat"] != null)
        //            {
        //                string physicalPath = System.IO.Path.Combine(Session["savePathChat"].ToString());
        //                Session.Remove("savePathChat");
        //                Session.Remove("FileNameChat");
        //                System.IO.File.Delete(physicalPath);
        //            }
        //        }
        //        catch (Exception x)
        //        {
        //            if (x.InnerException != null)
        //                Msg = x.InnerException.Message;
        //            else
        //                Msg = x.Message;

        //            MsgTitle = "خطا";
        //            Er = 1;
        //        }
        //    }
        //    return Json(new
        //    {
        //        Msg = Msg,
        //        MsgTitle = MsgTitle,
        //        Er = Er
        //    }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(string Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = ""; string MsgTitle = ""; byte Er = 0; int FileId = 0; string CodeFileId = "";
            if (Session["User"] == null)
                return null;

            string[] value = new string[1];
            string[] itemPropertyId = new string[1];
            //string[] itemPropertyName = new string[1];
            var MokhtasatNaghshe = "";
            var HaveEmzaha = false;
            Models.RaiSamEntities m = new Models.RaiSamEntities();
            try
            {

                 HaveEmzaha = m.prs_tblItemDynamic_Client_SignSelect("fldItemDynamic_ClientId", Id, 0).Any();

                var ss = m.prs_tblItemDynamic_ClientSelect("fldId", Id, "", 0).FirstOrDefault();
                FileId = ss.fldFileId;
                CodeFileId = ss.fldCodeFileId;
                var q = m.prs_tblItemProperties_ClientSelect("fldDynamicClientId", Id, 0).Where(l => l.fldJenseKhasiyat != 6).ToList();

                if (q.Count != 0)
                {
                    value = new string[q.Count];
                    itemPropertyId = new string[q.Count];
                    //itemPropertyName = new string[q.Count];
                    for (int i = 0; i < q.Count; i++)
                    {
                        value[i] = q[i].fldMeghdar;
                        itemPropertyId[i] = q[i].fldItemPropertisId.ToString();
                        //itemPropertyName[i] = q[i].fldNameKhasiyat_Fa;

                    }
                }
                var q2 = m.prs_tblItemProperties_ClientSelect("fldDynamicClientId", Id, 0).Where(l => l.fldJenseKhasiyat == 6).FirstOrDefault();
                if (q2 != null)
                    MokhtasatNaghshe = q2.fldMeghdar;
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
                FileId = FileId,
                CodeFileId = CodeFileId,
                value = value,
                itemPropertyId = itemPropertyId,
                MokhtasatNaghshe = MokhtasatNaghshe,
                //itemPropertyName=itemPropertyName,
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er,
                HaveEmzaha = HaveEmzaha
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload(string NoeFilePass)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            try
            {
                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }

                var passva= Path.GetExtension(Request.Files[0].FileName);
                var sss=NoeFilePass.IndexOf(passva.Split('.')[1]);
                if(sss>=0)
               // if (Request.Files[0].ContentType == MimeType.Get(".pdf"))
               // var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                //if (IsPDF == true)
                {
                    if (Request.Files[0].ContentLength <= 10240000)
                    {
                        /*if (Session["savePath"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                            Session.Remove("savePath");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }*/

                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileName"] = file.FileName;
                        Session["savePath"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "خطا",
                            Message = "حجم فایل انتخابی می بایست کمتر از 10 مگابایت باشد."
                        });
                        DirectResult result = new DirectResult();
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده غیر مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = Msg
                });
                DirectResult result = new DirectResult();
                return result;
            }

        }
        public ActionResult UploadChat()
        {
            if (Session["Username"] == null && Session["FristRegisterId"] == null)
                return null;
            string Msg = "";
            try
            {
                if (Session["savePathChat"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathChat"].ToString());
                    Session.Remove("savePathChat");
                    Session.Remove("FileNameChat");
                    System.IO.File.Delete(physicalPath);
                }

                //if (Request.Files[0].ContentType == "application/pdf")
                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                if (IsPDF == true)
                {
                    if (Request.Files[0].ContentLength <= 10240000)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameChat"] = file.FileName;
                        Session["savePathChat"] = savePath;
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "خطا",
                            Message = "حجم فایل انتخابی می بایست کمتر از 10 مگابایت باشد."
                        });
                        DirectResult result = new DirectResult();
                        return result;
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "خطا",
                        Message = "فایل انتخاب شده غیر مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = Msg
                });
                DirectResult result = new DirectResult();
                return result;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecord(string Id, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
           
            Models.RaiSamEntities m = new Models.RaiSamEntities();

            try
            {
                var q = m.prs_tblRequest_ItemDynamic_ClientSelect("fldItemDynamicClientId", Id, "", 0).Where(l => l.fldRequestId != ReqId).GroupBy(l => l.fldRequestId).ToList();

                if (q.Count > 0)
                {
                    Er = 1;
                    MsgTitle = "خطا";
                    Msg = "آیتم مورد نظر در درخواست دیگری استفاده شده است و شما قادر به حذف آن نمی باشید.";
                    return Json(new
                    {
                        Msg = Msg,
                        MsgTitle = MsgTitle,
                        Er = Er
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    MsgTitle = "حذف موفق";
                    Msg = "حذف با موفقیت انجام شد.";
                    m.prs_tblItemDynamic_ClientDelete(Convert.ToInt32(Id), ReqId);

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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChecked(int itemDynamicRatingID, int ReqId)
        {
            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new Models.RaiSamEntities();

            List<int> CheckedId = new List<int>();
            try
            {

                var q2 = m.prs_tblRequest_ItemDynamic_ClientSelect("fldRequestId", itemDynamicRatingID.ToString(), ReqId.ToString(), 0).ToList();

                foreach (var item in q2)
                {
                    CheckedId.Add(item.fldItemDynamicClientId);
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
                CheckedId = CheckedId,
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckRepeateSelection(string fldItemDynamicClientId,int ReqId)
        {
            string Msg = ""; string MsgTitle = ""; byte Er = 0; string Repeat = "";
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new Models.RaiSamEntities();

            try
            {
                var q = m.prs_tblRequest_ItemDynamic_ClientSelect("fldItemDynamicClientId", fldItemDynamicClientId, "", 0).Where(l => l.fldRequestId != ReqId).ToList();

                if (q.Count > 0)
                {
                    Repeat = "1";
                }
                else
                {
                    Repeat = "0";
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
                Repeat = Repeat,
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string data1, int ItemsDynamicRatingId, string Id, int FileId, int FirstId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new Models.RaiSamEntities();

            List<Models.prs_tblItemProperties_ClientSelect> ItemValue = JsonConvert.DeserializeObject<List<prs_tblItemProperties_ClientSelect>>(data1);
            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            string CodeFileId = "";
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                byte[] file = null; string Passvand = ""; string FileName = "";

                if (Id == "0")
                {
                    if (Session["savePath"] != null)
                    {
                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                        file = stream.ToArray();
                        FileName = Path.GetFileName(Session["savePath"].ToString());
                        Passvand = Path.GetExtension(Session["savePath"].ToString());
                    }
                    else
                    {
                        return Json(new
                        {
                            Er = 1,
                            Msg = "لطفا فایل را انتخاب کنید.",
                            MsgTitle = "خطا"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    //System.Data.Entity.Core.Objects.ObjectParameter fldId = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));
                    //System.Data.Entity.Core.Objects.ObjectParameter fldFileId = new System.Data.Entity.Core.Objects.ObjectParameter("fldFileId", typeof(int));

                    var z = m.prs_tblItemDynamic_ClientInsert(file, Passvand, FileName, ItemsDynamicRatingId, FirstId, u.FirstUserInputId).FirstOrDefault();
                    if (z.ErrorCode!=0)
                    {
                        if (Session["savePath"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                            Session.Remove("savePath");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }

                        Er = 1;
                        MsgTitle = "خطا";
                        Msg = z.ErrorMsg;
                        return Json(new
                        {
                            Msg = Msg,
                            MsgTitle = MsgTitle,
                            Er = Er
                        }, JsonRequestBehavior.AllowGet);
                    }
                    var DynamicClientId = z.fldID;
                    FileId = Convert.ToInt32(z.fldFileId);

                    Msg = "ذخیره ب موفقیت انجام شد.";
                    m.prs_tblRequest_ItemDynamic_ClientInsert(ReqId, DynamicClientId, u.UserInputId);

                    foreach (var item in ItemValue)
                    {
                        MsgTitle = "ذخیره موفق";
                        var Meghdar = "";
                        if (item.fldMeghdar != null)
                        {
                            Meghdar = item.fldMeghdar;
                        }

                        Msg = "ذخیره با موفقیت انجام شد.";
                        m.prs_tblItemProperties_ClientInsert(DynamicClientId, item.fldItemPropertisId, Meghdar, FirstId, u.FirstUserInputId);

                    }
                }
                else
                {
                    if (Session["savePath"] != null)
                    {
                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                        file = stream.ToArray();
                    }
                    foreach (var item in ItemValue)
                    {
                        MsgTitle = "ویرایش موفق";
                        var Meghdar = "";
                        if (item.fldMeghdar != null)
                        {
                            Meghdar = item.fldMeghdar;
                        }
                        var z = m.prs_tblItemProperties_ClientSelect("fldDynamicClientId", Id, 0).Where(l => l.fldItemPropertisId == item.fldItemPropertisId).Any();
                        if (z == false)
                        {
                            Msg = "ذخیره با موفقیت انجام شد.";
                            m.prs_tblItemProperties_ClientInsert(Convert.ToInt32(Id), item.fldItemPropertisId, Meghdar, FirstId, u.FirstUserInputId);
                        }
                        else
                        {
                            Msg = "ویرایش با موفقیت انجام شد.";
                            var zz = m.prs_UpdateItemDynamicClient(Convert.ToInt32(Id), FileId, file, Passvand, FileName, ItemsDynamicRatingId, u.UserInputId, FirstId, item.fldItemPropertisId, Meghdar).FirstOrDefault();
                            if (zz.ErrorCode!=0)
                            {
                                if (Session["savePath"] != null)
                                {
                                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                                    Session.Remove("savePath");
                                    Session.Remove("FileName");
                                    System.IO.File.Delete(physicalPath);
                                }

                                Er = 1;
                                MsgTitle = "خطا";
                                Msg = zz.ErrorMsg;
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

                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }

                var cc = m.prs_tblUploadFileCompanySelect("fldId", FileId.ToString(), 1).FirstOrDefault();
                CodeFileId = cc.fldCodeFileId;
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
                FileId = FileId,
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er,
                CodeFileId = CodeFileId
            }, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult Download(string FileId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new Models.RaiSamEntities();
            try
            {
                var q = m.prs_tblUploadFileCompanySelect("CodeId", FileId.ToString(), 1).FirstOrDefault();
                string passvand = q.fldPassvand;
                if (q != null && q.fldFile != null)
                {
                    MemoryStream st = new MemoryStream(q.fldFile);
                    return File(st.ToArray(), MimeType.Get(passvand), "DownloadFile" + passvand);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
        public FileContentResult DownloadSigned(string FileId,string ItemDynamic_ClientId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new Models.RaiSamEntities();
            try
            {
                var Emzaha = m.prs_tblItemDynamic_Client_SignSelect("fldItemDynamic_ClientId", ItemDynamic_ClientId, 0).ToList();
                //var sample = Server.MapPath("~/Content/sample.pdf");

                var Dessample = Server.MapPath("~/Uploaded/Addsignature_out" + u.UserSecondId + ".pdf");

                var q = m.prs_tblUploadFileCompanySelect("CodeId", FileId, 1).FirstOrDefault();
                MemoryStream sample = new MemoryStream(q.fldFile);


                    // Open document
                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(sample);

                    // Set coordinates
                    var s1 = pdfDocument.PageInfo.Height;
                    var s2 = pdfDocument.PageInfo.Width;
                    var s3 = pdfDocument.PageInfo.Margin;
                    int pageH = 900;
                    int lowerLeftX = 100;

                    foreach (var item in Emzaha)
                {

                    var s = m.prs_tblSign_StampDigitalSelect("fldId", item.fldSign_StampDigitalId.ToString(), 0).FirstOrDefault();
                    var signa = Server.MapPath("~/Uploaded/signature" + item.fldId + ".png");



                    var sf = m.prs_tblFileSelect("fldId", s.fldFileSignId.ToString(), 0).FirstOrDefault();
                    if (s.fldFileSignId == null)
                    {
                        sf = m.prs_tblFileSelect("fldId", s.fldFileStampId.ToString(), 0).FirstOrDefault();
                    }
                    System.IO.File.WriteAllBytes(signa, sf.fldFile);

                    // Load image into stream
                    FileStream imageStream = new FileStream(signa, FileMode.Open);

                    System.Drawing.Image img = System.Drawing.Image.FromStream(imageStream);
                    int height = img.Height;
                    int width = img.Width;

                    int lowerLeftY = height;
                    int upperRightX = 100 + width;
                    int upperRightY = 0;

                    if (s.fldFileSignId == null)
                    {
                        lowerLeftX = 100;
                        lowerLeftY = 300 ;
                        upperRightX = 100 + width;
                        upperRightY = 300- height;
                    }
                    

                    // Get the page where image needs to be added
                    for (int i = 1; i <= pdfDocument.Pages.Count; i++)
                    {
                        Aspose.Pdf.Page page = pdfDocument.Pages[i];
                        // Add image to Images collection of Page Resources
                        page.Resources.Images.Add(imageStream);
                        // Using GSave operator: this operator saves current graphics state
                        page.Contents.Add(new Aspose.Pdf.Operator.GSave());
                        // Create Rectangle and Matrix objects
                        Aspose.Pdf.Rectangle rectangle = new Aspose.Pdf.Rectangle(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
                        Matrix matrix = new Matrix(new double[] { rectangle.URX - rectangle.LLX, 0, 0, rectangle.URY - rectangle.LLY, rectangle.LLX, rectangle.LLY });
                        // Using ConcatenateMatrix (concatenate matrix) operator: defines how image must be placed
                        page.Contents.Add(new Aspose.Pdf.Operator.ConcatenateMatrix(matrix));
                        Aspose.Pdf.XImage ximage = page.Resources.Images[page.Resources.Images.Count];
                        // Using Do operator: this operator draws image
                        page.Contents.Add(new Aspose.Pdf.Operator.Do(ximage.Name));
                        // Using GRestore operator: this operator restores graphics state
                        page.Contents.Add(new Aspose.Pdf.Operator.GRestore());
                    }
                    if (s.fldFileSignId != null)
                        lowerLeftX = lowerLeftX + 300;

                    imageStream.Close();
                    System.IO.File.Delete(signa);
                }
                // dataDir = dataDir + "AddImage_out.pdf";
                // Save updated document
                MemoryStream DesStream = new MemoryStream();
                pdfDocument.Save(DesStream);


                return File(DesStream.ToArray(), MimeType.Get(q.fldPassvand), "DownloadFile" + q.fldPassvand);
                
            }
            catch (Exception)
            {
                return null;
            }
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRequest(string Ids, string dynamicId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new Models.RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);

            List<string> DynamicClientIds = new List<string>();
            dynamicId = dynamicId.Substring(4);
            try
            {
                if (Ids != "")
                {
                    DynamicClientIds = Ids.Split(';').Reverse().Skip(1).Reverse().ToList();

                    var q2 = m.prs_tblRequest_ItemDynamic_ClientSelect("fldRequestId", dynamicId, ReqId.ToString(), 0).ToList();

                    if (q2.Count != 0)
                    {
                        for (int k = 0; k < q2.Count; k++)
                        {
                            MsgTitle = "حذف موفق";
                            Msg = "حذف با موفقیت انجام شد.";
                            m.prs_tblRequest_ItemDynamic_ClientDelete(q2[k].fldId);

                        }
                    }
                        var s = m.prs_tblSign_StampDigitalSelect("fldFirstRegisterId", user.FirstRegisterId.ToString(), 0).FirstOrDefault();
                    for (var i = 0; i < DynamicClientIds.Count; i++)
                    {
                        MsgTitle = "ذخیره موفق";
                        Msg = "ذخیره با موفقیت انجام شد.";
                        m.prs_tblRequest_ItemDynamic_ClientInsert(ReqId, Convert.ToInt32(DynamicClientIds[i]), u.UserInputId);

                        var Emzaha = m.prs_tblItemDynamic_Client_SignSelect("fldItemDynamic_ClientId", DynamicClientIds[i].ToString(), 0).Where(l => l.fldSign_StampDigitalId == s.fldId).Any();
                        if (Emzaha == false)
                            m.prs_tblItemDynamic_Client_SignInsert(s.fldId,Convert.ToInt32(DynamicClientIds[i]),  u.UserInputId);

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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DelRequest(string dynamicId, int ReqId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = ""; string MsgTitle = ""; byte Er = 0; byte havedata = 0;
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new Models.RaiSamEntities();

            dynamicId = dynamicId.Substring(4);

            try
            {
                var q2 = m.prs_tblRequest_ItemDynamic_ClientSelect("fldRequestId", dynamicId, ReqId.ToString(), 0).ToList();

                if (q2.Count != 0)
                {
                    havedata = 1;
                    for (int k = 0; k < q2.Count; k++)
                    {
                        MsgTitle = "حذف موفق";
                        Msg = "حذف با موفقیت انجام شد.";
                        m.prs_tblRequest_ItemDynamic_ClientDelete(q2[k].fldId);

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
                havedata = havedata,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ReadCh_Dynamic(string Id, string StateAORC)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    Models.RaiSamEntities m = new Models.RaiSamEntities();
        //    List<Models.prs_tblItemDynamic_ClientSelect> data = null;
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        if (Session["HadafId"] == null)
        //            return RedirectToAction("Index", "Admin");
        //        data = servic.GetCh_ItemDynamic_ClientWithFilter("fldItemClientId", Id, Session["RankingId"].ToString(), 0, out Err).ToList();
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //        data = servic.GetCh_ItemDynamic_ClientWithFilter("fldItemClientId", Id, StateAORC, 0, out Err).ToList();
        //    }
        //    return this.Store(data);
        //}
        public ActionResult MatnHtmlDynamicForm(string NameTable, int ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var MatnHtmlDynamicForm = "";
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                var s = m.prs_tblRequestRankingSelect("fldId", ReqId.ToString(), 0).FirstOrDefault();
                PageHtml = m.prs_tblHtmlViewerSelect("Hadaf-NameTable", NameTable, s.fldHadafId, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlDynamicForm = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlDynamicForm = "";
            }

            return Json(new { MatnHtmlDynamicForm = MatnHtmlDynamicForm }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetState()
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    if (Session["FristRegisterId"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    var q = servic.GetStateWithFilter("", "", 0, out Err).ToList().Select(c => new { fldStateId = c.fldId, fldStateName = c.fldTitle });
        //    return this.Store(q);
        //}
        //public ActionResult GetCity(string ID)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    if (Session["FristRegisterId"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    var q = servic.GetCityWithFilter("fldStateId", ID, 0, out Err).ToList().Select(c => new { fldCityId = c.fldId, fldCityName = c.fldTitle });
        //    return this.Store(q);
        //}
        public ActionResult ShowInfoDynamicForm(string NoeKhasiyatha, string fldIdDynamic_Client, string TitleDynamic, string fldItemsDynamicRatingIdd, string types, string TitleItems, string ItemPropertiesId, string EnterSicleIds, string contractId, string State)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.TitleDynamic = TitleDynamic;
            result.ViewBag.types = types;
            result.ViewBag.TitleItems = TitleItems;
            result.ViewBag.ItemPropertiesIds = ItemPropertiesId;
            result.ViewBag.fldItemsDynamicRatingIdd = fldItemsDynamicRatingIdd;
            result.ViewBag.NoeKhasiyatha = NoeKhasiyatha;
            result.ViewBag.fldIdDynamic_Client = fldIdDynamic_Client;

            var IdPropNaghshe = 0;
            Models.RaiSamEntities m = new Models.RaiSamEntities();
            var q1 = m.prs_tblItemsDynamicRatingSelect("fldId", fldItemsDynamicRatingIdd, 0).FirstOrDefault();
            var q2 = m.prs_tblItemDynamicPropertiesSelect("fldTitleDynamicId_JenseKhasiyat", q1.fldDynamicRatingId.ToString(), "6", 0).FirstOrDefault();
            if (q2 != null)
                IdPropNaghshe = q2.fldId;
            result.ViewBag.IdPropNaghshe = IdPropNaghshe;

            var HaveTaeed = false;
            if (contractId != "")
            {
                var k = m.prs_tblRegistrationFirstContractSelect("fldId", contractId, 0).FirstOrDefault();
                HaveTaeed = m.prs_tblTaeedItemsSelect("fldHadafId", k.fldHadafId.ToString(), 0).Where(l => l.fldApp_ClientId == 10).Any();
                result.ViewBag.contractId = contractId;
            }
            result.ViewBag.HaveTaeed = HaveTaeed;
            result.ViewBag.State = State;

            return result;
        }
    }
}
