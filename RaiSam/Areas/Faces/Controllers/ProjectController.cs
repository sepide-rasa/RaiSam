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
using FastReport;

namespace RaiSam.Areas.Faces.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Faces/Project/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            //var SentToAdmin = 0;
            //var k = m.prs_tblRequestRankingSelect("fldId", Session["RankingId"].ToString(), 0).FirstOrDefault();
            //if (k.fldStatusId == 2 || k.fldStatusId == 5)
            //    SentToAdmin = 1;
            //PartialView.ViewBag.SentToAdmin = SentToAdmin;
            return PartialView;
        }

        public ActionResult MatnHtmlProject()
        {
            if (Session["FristRegisterId"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Session["HadafId"] == null)
                return RedirectToAction("Index", "Home");
            var MatnHtmlProject = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                PageHtml = m.prs_tblHtmlViewerSelect("Hadaf-NameTable", "36", Convert.ToInt32(Session["HadafId"]), 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlProject = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlProject = "";
            }

            return Json(new { MatnHtmlProject = MatnHtmlProject }, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DownloadV()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["FristRegisterId"] == null)
                return null;
            string savePath = Server.MapPath(@"~\Uploaded\Help\Project.mp4");

            MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
            return File(st.ToArray(), MimeType.Get(".mp4"), "Help_" + "Project" + ".mp4");
        }
        public ActionResult New(string Id, string EnterSicleIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            //Ext.Net.MVC.PartialViewResult result = new Ext.Net.MVC.PartialViewResult();
            ViewData.Model = new Models.ProjectDetails();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ViewData = ViewData
            };
            Models.RaiSamEntities m = new RaiSamEntities();
            //  var q = servic.GetDashboardRankingWithFilter("fldTreeId",Session["TreeId"].ToString(),1,out Err).FirstOrDefault();
            //var Starttarikh = MyLib.Shamsi.Miladi2ShamsiString(q.fldDate);
            //var StartDate = q.fldDate.ToString("yyyy-MM-dd");
            //StartDate = StartDate.Substring(0, 10);
            //PartialView.ViewBag.StartDate = StartDate;
            //PartialView.ViewBag.Starttarikh = Starttarikh;
            var IsInClient = 0;
            int req = 0;
        

            if (EnterSicleIds != null)
            {
                var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
                var kk = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
                result.ViewBag.FirstId = kk.fldFirstRegisterId;
                req = kk.fldRequestId;
                result.ViewBag.Id = kk.fldId;

            }
            else
            {
                result.ViewBag.FirstId = Convert.ToInt32(Session["FristRegisterId"]);
                req = Convert.ToInt32(Session["RankingId"]);

                

                if (Id == "00")
                {
                    var c = m.prs_tblRegistrationFirstContractSelect("fldRequestId", Session["RankingId"].ToString(), 0).FirstOrDefault();
                    Id = c.fldId.ToString();
                }
                result.ViewBag.Id = Id;
            }

            var q = m.prs_tblRequestRankingSelect("fldId", req.ToString(), 0).FirstOrDefault();
            if (q.fldKartablId == 100) IsInClient = 1;
            result.ViewBag.CompanyTitle = q.fldFullName;
            result.ViewBag.ReqId = req;
            result.ViewBag.IsInClient = IsInClient;
            result.ViewBag.HadafId = q.fldHadafId;

            return result;
        }
        public ActionResult WinUploadFiles(string Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTypeVagon()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblTypeVagonSelect("","", 0).Select(c => new { Id = c.fldId, Name = c.fldTitleVagon });
           return Json(q, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Print(string Id, string Status)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Session["HadafId"] == null)
                return RedirectToAction("Index", "Admin");
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            PartialView.ViewBag.Status = Status;
            return PartialView;
        }
        



        public ActionResult ShowInfoProject(string Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            ViewData.Model = new Models.ProjectDetails();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ViewData = ViewData
            };
            result.ViewBag.Id = Id;
            return result;
        }

        //public ActionResult ShowConversationProject(string ProjectId, string StateAORC, string StatusRequest, string UserNameAdmin, string PassAdmin)
        //{
        //    int Permission = 0;
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //    }

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
        //    Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //    PartialView.ViewBag.ProjectId = ProjectId;
        //    PartialView.ViewBag.StateAORC = StateAORC;
        //    PartialView.ViewBag.StatusRequest = StatusRequest;
        //    PartialView.ViewBag.UserNameAdmin = UserNameAdmin;
        //    PartialView.ViewBag.PassAdmin = PassAdmin;
        //    return PartialView;
        //}

        //public ActionResult ReadCh_Project(string ProjectId)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    if (Session["Username"] == null && Session["FristRegisterId"] == null)
        //        return null;
        //    List<RaiTrainClientWS.OBJ_Ch_Project> data = null;

        //    data = servic.GetCh_ProjectWithFilter("fldProjectId", ProjectId, 0, out Err).ToList();
        //    return this.Store(data);
        //}
       

        //public ActionResult ShowMsgProject(string Message, string StateAORC, string SenderId, int Id, string UserNameAdmin)
        //{
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        if (SenderId != "0")
        //        {
        //            servic.UpdateStatusMsgProject("Project", Id, true, out Err);
        //        }
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //        if (SenderId == "0")
        //        {
        //            servic.UpdateStatusMsgProject("Project", Id, true, out Err);
        //        }
        //    }
        //    Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //    PartialView.ViewBag.Message = Message;
        //    PartialView.ViewBag.StateAORC = StateAORC;
        //    PartialView.ViewBag.UserNameAdmin = UserNameAdmin;
        //    PartialView.ViewBag.SenderId = SenderId;
        //    return PartialView;
        //}

        //public ActionResult NewCh_Project(string ProjectId, string StateAORC, string StatusRequest, string UserNameAdmin, string PassAdmin)
        //{
        //    int Permission = 0;
        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //    }
        //    else
        //    {
        //        if (Session["Username"] == null)
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //    }
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
        //    Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
        //    PartialView.ViewBag.UserNameAdmin = UserNameAdmin;
        //    PartialView.ViewBag.StateAORC = StateAORC;
        //    PartialView.ViewBag.ProjectId = ProjectId;
        //    return PartialView;
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SaveNewCh_Project(OBJ_Ch_Project ChatC, string StateAORC)
        //{
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return Content("شما مجاز به انجام این عملیات نمی باشید.");
        //    }
        //    string Msg = "";
        //    string MsgTitle = ""; byte Er = 0; string fldMsg = "";
        //    var Date = servic.Getdate().fldTarikh;
        //    var Time = servic.Getdate().fldTime;

        //    if (StateAORC == "0")
        //    {
        //        if (Session["FristRegisterId"] == null)
        //            return RedirectToAction("Logon", "Account", new { area = "" });
        //        if (Session["RankingId"] == null)
        //            return RedirectToAction("Index", "Home");
        //        try
        //        {
        //            Msg = servic.InsertCh_Project(false, null, ChatC.fldMsg, Date, Time, 1, null, ChatC.fldProjectId, "", Convert.ToInt32(Session["FristRegisterId"]), IP, out Err);
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
        //            return RedirectToAction("Login", "Admin", new { area = "faces" });
        //        try
        //        {
        //            if (ChatC.fldMsg != null)
        //            {
        //                fldMsg = ChatC.fldMsg;
        //            }
        //            var FirstRegisterId = servic.GetRequestRankingDetail(Convert.ToInt32(StateAORC), true, IP, out Err).fldFirstRegisterId;
        //            Msg = servic.InsertCh_Project(false, Convert.ToInt32(Session["UserId"]), fldMsg, Date, Time, 1, null, ChatC.fldProjectId, "", FirstRegisterId, IP, out Err);
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
        //    return Json(new
        //    {
        //        Msg = Msg,
        //        MsgTitle = MsgTitle,
        //        Er = Er
        //    }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult MatnHtmlNewProject(int ReqId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var MatnHtmlNewProject = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            prs_tblHtmlViewerSelect PageHtml = null;
            try
            {
                var s = m.prs_tblRequestRankingSelect("fldId", ReqId.ToString(), 0).FirstOrDefault();
                PageHtml = m.prs_tblHtmlViewerSelect("Hadaf-NameTable", "37", s.fldHadafId, 1).FirstOrDefault();
                if (PageHtml != null)
                {
                    MatnHtmlNewProject = PageHtml.fldMatnHtml;
                }
            }
            catch (Exception x)
            {
                MatnHtmlNewProject = "";
            }

            return Json(new { MatnHtmlNewProject = MatnHtmlNewProject }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblRegistrationFirstContractSelect re, string Namayande, string Hoghughi/*, string kk*/)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "", MsgTitle = ""; var Er = 0;
       
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            

            byte[] file = null; string Pasvand = ""; string NameFile = ""; int? HeaderId = 0;

            List<prs_tblRegistrationFirstContract_HaghighiSelect> NamayandeVal = JsonConvert.DeserializeObject<List<prs_tblRegistrationFirstContract_HaghighiSelect>>(Namayande);
            List<prs_tblRegistrationFirstContract_HoghoghiSelect> HoghughiVal = JsonConvert.DeserializeObject<List<prs_tblRegistrationFirstContract_HoghoghiSelect>>(Hoghughi);
            try
            {
             
                if (re.fldId == 0)
                {

                    var s=m.prs_tblRegistrationFirstContractInsert(Convert.ToInt32(Session["RankingId"]), re.fldTitle, re.fldTedad, re.fldTypeVagonId, user.UserInputId).FirstOrDefault();
                    HeaderId = s.fldId;
                    foreach (var item in NamayandeVal)
                    {
                        m.prs_tblRegistrationFirstContract_HaghighiInsert(item.fldAshkhasId, item.fldMobile, HeaderId, user.UserInputId);
                    }
                    foreach (var item in HoghughiVal)
                    {
                        m.prs_tblRegistrationFirstContract_HoghoghiInsert(item.fldAshkhasHoghoghiId, HeaderId, Convert.ToInt32(item.fldTarikhSanad.Replace("/", "")), item.fldTedadVagon, item.fldArzeshRiyali, item.fldArzeshDolari, file, Pasvand, NameFile, file, Pasvand, NameFile, user.UserInputId);
                    }
                    Msg = "ذخیره با موفقیت انجام شد.";
                    MsgTitle = "دخیره موفق";
                    
                }
                else
                {

                    var q = m.prs_tblRegistrationFirstContractUpdate(re.fldId, Convert.ToInt32(Session["RankingId"]), re.fldTitle, re.fldTedad, re.fldTypeVagonId, user.UserInputId);
                    HeaderId = re.fldId;
                    foreach (var item in NamayandeVal)
                    {
                        if (item.fldId == 0)
                            m.prs_tblRegistrationFirstContract_HaghighiInsert(item.fldAshkhasId, item.fldMobile, HeaderId, user.UserInputId);
                        else
                            m.prs_tblRegistrationFirstContract_HaghighiUpdate(item.fldId, item.fldAshkhasId, item.fldMobile, HeaderId, user.UserInputId);
                    }
                    foreach (var item in HoghughiVal)
                    {
                        if (item.fldId == 0)
                            m.prs_tblRegistrationFirstContract_HoghoghiInsert(item.fldAshkhasHoghoghiId, HeaderId, Convert.ToInt32(item.fldTarikhSanad.Replace("/", "")), item.fldTedadVagon, item.fldArzeshRiyali, item.fldArzeshDolari, file, Pasvand, NameFile, file, Pasvand, NameFile, user.UserInputId);
                        else
                            m.prs_tblRegistrationFirstContract_HoghoghiUpdate(item.fldId, item.fldAshkhasHoghoghiId, HeaderId, Convert.ToInt32(item.fldTarikhSanad.Replace("/", "")), item.fldTedadVagon, item.fldArzeshRiyali, item.fldArzeshDolari, 0, 0, file, Pasvand, NameFile, file, Pasvand, NameFile, user.UserInputId);
                    }    
                            Msg = "ویرایش با موفقیت انجام شد";
                            MsgTitle = "ویرایش موفق";
                        
                    
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
                HeaderId = HeaderId
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
           
            string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
                try
                {

                    m.prs_tblRegistrationFirstContractDelete(id, u.UserSecondId); ;
                        
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                       
                
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
        public ActionResult Details(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            // var q = m.prs_tblRegistrationFirstContract_DetailSelect("fldId", Id.ToString(), 0).FirstOrDefault();
            var q2 = m.prs_tblRegistrationFirstContractSelect("fldId", Id.ToString(), 0).FirstOrDefault();
            return Json(new
            {
                fldId = q2.fldId,
                fldTypeVagonId = q2.fldTypeVagonId.ToString(),
                fldTypeVagonName=q2.fldTypeVagonName,
                fldTitle = q2.fldTitle,
                fldTedad = q2.fldTedad
            }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
          
            Models.RaiSamEntities m = new RaiSamEntities();

            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblRegistrationFirstContractSelect> data = null;
            if (filterHeaders.Conditions.Count > 0)
            {
                string field = "";
                string searchtext = "";
                List<prs_tblRegistrationFirstContractSelect> data1 = null;
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
                        case "fldTypeVagonName":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldTypeVagonName";
                            break;
                        case "fldTedad":
                            searchtext = "%" + ConditionValue.Value.ToString() + "%";
                            field = "fldTedad";
                            break;
                        

                    }
                    if (data != null)
                        data1 = m.prs_tblRegistrationFirstContractSelect(field, searchtext,  0).Where(l => l.fldRequestId == Convert.ToInt32(Session["RankingId"])).ToList();
                    else
                        data = m.prs_tblRegistrationFirstContractSelect(field, searchtext,  0).Where(l => l.fldRequestId == Convert.ToInt32(Session["RankingId"])).ToList();
                }
                if (data != null && data1 != null)
                    data.Intersect(data1);
            }
            else
            {

                data = m.prs_tblRegistrationFirstContractSelect("fldRequestId", (Session["RankingId"]).ToString(), 0).ToList();
            }

            //var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

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
            //int limit = parameters.Limit;

            //if ((parameters.Start + parameters.Limit) > data.Count)
            //{
            //    limit = data.Count - parameters.Start;
            //}

            //List<RaiTrainClientWS.OBJ_Contract> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
            //-- end paging ------------------------------------------------------------

            return this.Store(data, data.Count);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadNamayande(StoreRequestParameters parameters, int ProjectId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
           
            Models.RaiSamEntities m = new RaiSamEntities();

            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblRegistrationFirstContract_HaghighiSelect> data = null;


            data = m.prs_tblRegistrationFirstContract_HaghighiSelect("fldHeaderId", ProjectId.ToString(), 0).ToList();
            

            return this.Store(data, data.Count);
        }
        public ActionResult ReadHoghughi(StoreRequestParameters parameters, int ProjectId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
       
            Models.RaiSamEntities m = new RaiSamEntities();

            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblRegistrationFirstContract_HoghoghiSelect> data = null;


            data = m.prs_tblRegistrationFirstContract_HoghoghiSelect("fldHeaderId", ProjectId.ToString(), 0).ToList();


            return this.Store(data, data.Count);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNamayande(int id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
         
            string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                m.prs_tblRegistrationFirstContract_HaghighiDelete("fldId", id); 

                Msg = "حذف با موفقیت انجام شد";
                MsgTitle = "حذف موفق";


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
        public ActionResult DeleteHoghughi(int id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
        
            string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                m.prs_tblRegistrationFirstContract_HoghoghiDelete("fldId", id); ;

                Msg = "حذف با موفقیت انجام شد";
                MsgTitle = "حذف موفق";


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
        public ActionResult UploadFilePro()
        {
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

                //if (Request.Files[0].ContentType == "application/pdf")
                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                if (IsPDF == true)
                {
                    if (Request.Files[0].ContentLength <= 5242880)
                    {
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
                            Message = "حجم فایل انتخابی می بایست کمتر از 5 مگابایت باشد."
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
        public ActionResult UploadFileDaftarche()
        {
            string Msg = "";
            try
            {
                if (Session["savePathD"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathD"].ToString());
                    Session.Remove("savePathD");
                    Session.Remove("FileNameD");
                    System.IO.File.Delete(physicalPath);
                }

                //if (Request.Files[0].ContentType == "application/pdf")
                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[1]);
                if (IsPDF == true)
                {
                    if (Request.Files[1].ContentLength <= 5242880)
                    {
                        HttpPostedFileBase file = Request.Files[1];
                        string savePath = Server.MapPath(@"~\Uploaded\" + "D" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileNameD"] = file.FileName;
                        Session["savePathD"] = savePath;
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
                            Message = "حجم فایل انتخابی می بایست کمتر از 5 مگابایت باشد."
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
        public ActionResult DetailsFile(string Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            int? fldSanadFileId = 0; int? fldDaftarcheFileId = 0; string Msg = ""; string MsgTitle = ""; byte Er = 0;
            var q = m.prs_tblRegistrationFirstContract_HoghoghiSelect("fldId", Id, 1).FirstOrDefault();

            if (q.fldSanadFileId != null)
            {
                fldSanadFileId = q.fldSanadFileId;
                fldDaftarcheFileId=q.fldDaftarcheFileId;
            }
            return Json(new
            {
                Er = Er,
                fldSanadFileId = fldSanadFileId,
                fldDaftarcheFileId = fldDaftarcheFileId
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult DownloadFilePro(string FileId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            if (Session["User"] == null)
                return null;

            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblUploadFileCompanySelect("fldId", FileId.ToString(), 1).FirstOrDefault();

            if (q != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPassvand), "DownloadFile" + q.fldPassvand);
            }
            return null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFilePro(int SanadFileId, int DaftarcheFileId, int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            byte[] file = null; string e = ""; int IdFile = 0; string FileName = ""; byte Er = 0; string Msg = ""; string MsgTitle = "";

            byte[] fileD = null; string eD = ""; int IdFileD = 0; string FileNameD = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePath"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                    file = stream.ToArray();
                    e = Path.GetExtension(Session["savePath"].ToString());
                    FileName = Session["FileName"].ToString();
                }
                else
                {
                    return Json(new
                    {
                        Er = 1,
                        Msg = "لطفا فایل سند را انتخاب کنید.",
                        MsgTitle = "خطا"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (Session["savePathD"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathD"].ToString()));
                    fileD = stream.ToArray();
                    eD = Path.GetExtension(Session["savePathD"].ToString());
                    FileNameD = Session["FileNameD"].ToString();
                }
                else
                {
                    return Json(new
                    {
                        Er = 1,
                        Msg = "لطفا فایل سند را انتخاب کنید.",
                        MsgTitle = "خطا"
                    }, JsonRequestBehavior.AllowGet);
                }

                var kk = m.prs_tblRegistrationFirstContract_HoghoghiUpdateFile(Id, SanadFileId, file, e, FileName, DaftarcheFileId, fileD, eD, FileNameD,u.UserInputId).FirstOrDefault();
                Msg = "ذخیره با موفقیت انجام شد.";
                MsgTitle = "ذخیره موفق";


                if (Session["savePath"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                    Session.Remove("savePath");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                if (Session["savePathD"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathD"].ToString());
                    Session.Remove("savePathD");
                    Session.Remove("FileNameD");
                    System.IO.File.Delete(physicalPath);
                }

                return Json(new
                {
                    Er = Er,
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    IdFileSanad = kk.fldSanadFileId,
                    IdFileDaftarche = kk.fldDaftarcheFileId
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;
                return Json(new
                {
                    Er = 1,
                    Msg = Msg,
                    MsgTitle = "خطا"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintProj(int Id, int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
             PartialView.ViewBag.Id = Id;
             PartialView.ViewBag.state = state;

            return PartialView;
        }
        public ActionResult GenerateRptProj(int Id, int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var uu = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();

                Report rep = new Report();
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\Contract.frx";
               
                rep.Load(path);
                rep.SetParameterValue("FieldName", "fldHeaderId");
                rep.SetParameterValue("Value",  Id.ToString());
                rep.SetParameterValue("h", 0);
                rep.SetParameterValue("FieldName-hoghoghi", "fldHeaderId");
                rep.SetParameterValue("Value-hoghoghi", Id.ToString());
                rep.SetParameterValue("h-hoghoghi", 0);
                rep.SetParameterValue("IdContract", Id);
                rep.SetParameterValue("UserName", uu.fldNamePersonal);
                rep.SetParameterValue("conectionstr", System.Configuration.ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString);

                if (rep.Report.Prepare())
                {
                    // Set PDF export props
                    FastReport.Export.Pdf.PDFExport pdfExport = new FastReport.Export.Pdf.PDFExport();
                    pdfExport.ShowProgress = false;
                    pdfExport.Compressed = true;
                    pdfExport.AllowPrint = true;
                    pdfExport.EmbeddingFonts = true;


                    MemoryStream strm = new MemoryStream();
                    rep.Report.Export(pdfExport, strm);
                    rep.Dispose();
                    pdfExport.Dispose();
                    strm.Position = 0;

                    // return stream in browser
                    return File(strm.ToArray(), "application/pdf");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception x)
            {
                return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }
    }
}
