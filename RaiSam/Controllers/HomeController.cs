using RaiSam.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.Net.NetworkInformation;
using System.Web.Security;
using System.Threading.Tasks;

namespace RaiSam.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class HomeController : Controller
    {
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index()
        {

            if (Session["User"] != null && Session["SFToken"] != null && Request.Cookies["SFToken"] != null)
            {
            UserInfo u = (UserInfo)Session["User"];
                    Models.RaiSamEntities m = new RaiSamEntities();
                if (!Session["SFToken"].ToString().Equals(Request.Cookies["SFToken"].Value))
                {
                    return RedirectToAction("Logon", "Account", new { area = "" });
                }
                else
                {
             

                    ViewBag.time = DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0');
                    


                    var EtelaieTitleAll = "";
                    var EtelaieIDAll = "";
                   
                    var Etelaie = m.prs_tblAnnouncementManagerSelect("fldCheckTarikhShamsi_Type_NotSeen", "1", u.UserInputId,0,"").ToList();
                    foreach (var item in Etelaie)
                    {
                        EtelaieTitleAll = EtelaieTitleAll + item.fldTitle + ";";
                        EtelaieIDAll = EtelaieIDAll + item.fldID + ";";
                    }
                    string Msg = "";
                    if (u.UserId == u.UserSecondId)
                    {
                        
                        var user = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(),"",1).FirstOrDefault();
                        ViewBag.FirstLogin = user.fldFirstLogin;
                        ViewBag.Msg = Msg;

                        
                        var Version_S = m.prs_tblVersion_SeenSelect("LastStatus_UserId", u.UserSecondId.ToString(),0).FirstOrDefault();
                        if (Version_S != null)
                            ViewBag.SeenVersion = Version_S.fldSeen;
                        else
                            ViewBag.SeenVersion = false;
                    }
                    else
                    {
                       
                        var user1 = m.prs_tblUserSelect("fldId", u.UserId.ToString(),"",1).FirstOrDefault();

                       
                        var user2 = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(), "", 1).FirstOrDefault();

                        Msg = "کاربر " + user1.fldNamePersonal + " در نقش " + user2.fldNamePersonal + " وارد سامانه شده است.";
                        ViewBag.Msg = Msg;
                    }


                    ViewBag.EtelaieTitleAll = EtelaieTitleAll;
                    ViewBag.EtelaieIDAll = EtelaieIDAll;

                    return View();
                }
            }
            else
            {
                return RedirectToAction("Logon", "Account", new { area = "" });
            }
        }
        public async Task<ActionResult> CheckForSmsPanel()
        {
            UserInfo u = (UserInfo)Session["User"];
            Models.RaiSamEntities m = new RaiSamEntities();

            SignalrHub h = new SignalrHub();
            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();
                    RaiSam.RaiSms.Service w = new RaiSam.RaiSms.Service();
                    var smsPanel = w.GetuserInfo(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, "RailWay");
                    h.UpdateSmsPanelInfo(smsPanel[1],0, u.UserInputId);
                    // smsPanelInfo = smsPanel[1];
                }
                catch (Exception)
                {

                    h.UpdateSmsPanelInfo("قطع ارتباط",1, u.UserInputId);
                    //throw;
                }
            });
            return null;
        }
        public ActionResult MatnHtml()
        {
            UserInfo user = new UserInfo();
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var matn = "";
            var PageHtml = m.prs_tblPageHtmlSelect("fldId", "2", 1).FirstOrDefault();
            if (PageHtml != null)
                matn = PageHtml.fldMohtavaHtml;
            return Json(new { MatnHTML = matn }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAnnoncement(byte type)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo u = (UserInfo)Session["User"];
            Models.RaiSamEntities m = new RaiSamEntities();
            var EtelaieTitleAll = "";
            var EtelaieIDAll = "";
            var FieldName = "";
            if (type == 0)
            {
                FieldName = "fldCheckTarikhShamsi_Type_All";
            }
            else
            {
                FieldName = "fldCheckTarikhShamsi_Type_NotSeen";
            }
            
            var Etelaie = m.prs_tblAnnouncementManagerSelect(FieldName, "1", u.UserInputId,0,"").ToList();
            foreach (var item in Etelaie)
            {
                EtelaieTitleAll = EtelaieTitleAll + item.fldTitle + ";";
                EtelaieIDAll = EtelaieIDAll + item.fldID + ";";
            }
            return Json(new { EtelaieTitleAll = EtelaieTitleAll, EtelaieIDAll = EtelaieIDAll }, JsonRequestBehavior.AllowGet);
        }
        public FileContentResult DownloadFile(string fldFileId)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
           
            var q = m.prs_tblFileSelect("fldId", fldFileId,1).FirstOrDefault();
            return File((byte[])q.fldFile, MimeType.Get(q.fldPasvand), q.fldFileName + "." + q.fldPasvand);
        }
        public ActionResult GotoNExtEtelaiie(string ID)
        {//باز شدن پنجره
            //if (Session["User"] == null)
            //    return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.ID = ID;
            return PartialView;
        }
        public ActionResult DetailsInfoPage(int Id)
        {//نمایش اطلاعات جهت رویت کاربر
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)Session["User"];
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                
                var q = m.prs_tblAnnouncementManagerSelect("fldId_Details", Id.ToString(), user.UserInputId,1,"").FirstOrDefault();
                return new JsonResult()
                {
                    Data = new
                    {
                        fldAttachmentId = q.fldAttachmentId,
                        fldTarikh = q.fldTarikhShamsi,
                        fldMatn = q.fldMatn,
                        fldTitle = q.fldTitle,
                        fldAttachment = q.fldAttachment
                    },
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
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
        }
        public ActionResult GetDate()
        {
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_GetDate().FirstOrDefault();
            var time = q.fldDateTime;
            return Json(new
            {
                datetime = time.Hour.ToString().PadLeft(2, '0') + ":" +
                            time.Minute.ToString().PadLeft(2, '0') + ":" +
                            time.Second.ToString().PadLeft(2, '0')
            }, JsonRequestBehavior.AllowGet);
        }
        public FileContentResult DownloadChrome()
        {
            if (Session["User"] == null)
                return null;
            string savePath = Server.MapPath(@"~\Uploaded\Google.Chrome.zip");
            MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
            return File(st.ToArray(), MimeType.Get(".zip"), "Google.Chrome.zip");
        }

        public ActionResult PrintHelp()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public ActionResult PrintHelpMostanadFani()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public ActionResult Setting()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public ActionResult SafDl()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public ActionResult ReadSafDL(StoreRequestParameters parameters)
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
                List<Models.prs_tblSafDownloadSelect> data = null;


                data = m.prs_tblSafDownloadSelect("", "", 0).OrderByDescending(l=>l.fldId).ToList();

                return this.Store(data, data.Count);
            }
        }
        public ActionResult EndsafDl(long Id)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            m.prs_tblSafDownloadUpdate("fldId", Id, 4);
            return Json(new { err = 0 });
        }
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Session["savePathSetting"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathSetting"].ToString());
                Session.Remove("savePathSetting");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
            }
            HttpPostedFileBase file = Request.Files[0];
            var e = System.IO.Path.GetExtension(Request.Files[0].FileName);
            if (e.ToLower() == ".jpg" || e.ToLower() == ".png" || e.ToLower() == ".gif")
            {
                if (Request.ContentLength <= 102400)
                {
                    string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                    file.SaveAs(savePath);
                    Session["FileName"] = file.FileName;
                    Session["savePathSetting"] = savePath;
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
                        Message = "حجم فایل بیشتر از حد مجاز است."
                    });
                    DirectResult result = new DirectResult();
                    result.IsUpload = true;
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
                    Message = "فایل غیرمجاز"
                });
                DirectResult result = new DirectResult();
                result.IsUpload = true;
                return result;
            }
        }

        public ActionResult DetailSetting(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities model = new RaiSamEntities();
            
            var u = (UserInfo)Session["User"];
            var q = model.prs_tblSettingSelect("fldId", Id.ToString(),1).FirstOrDefault();
            string pic = "";
            if (q.fldFile != null)
            {
                pic = Convert.ToBase64String(q.fldFile);
            }
            if (q != null)
            {
                return Json(new
                {
                    fldId = q.fldId,
                    fldTitle = CodeDecode.stringDecode(q.fldTitle),
                    fldFile = pic,
                    fldDesc = q.flddesc,
                    fldCompanyFromService = q.fldCompanyFromService
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    fldId = 0,
                    fldTitle = "",
                    fldFile = Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("~/Content/icon/Blank.jpg"))),
                    fldDesc = "",
                    fldCompanyFromService=false,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowPicUpload(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathSetting"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }

        public ActionResult SaveSetting(Models.prs_tblSettingSelect Setting, byte IsDel)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; var Er = 0;
            Models.RaiSamEntities model = new RaiSamEntities();
            var u = (UserInfo)Session["User"];
            try
            {
                
                byte[] file = null;
                if (Session["savePathSetting"] != null && IsDel == 0)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathSetting"].ToString()));
                    file = stream.ToArray();
                }
                else
                {
                    
                    var q = model.prs_tblSettingSelect("fldId", Setting.fldId.ToString(),1).FirstOrDefault();
                    if (q != null)
                    {
                        file = q.fldFile;
                    }
                    else
                    {
                        file = new MemoryStream(System.IO.File.ReadAllBytes(Server.MapPath("~/Content/icon/Blank.jpg"))).ToArray();
                    }
                }

                if (Models.Permission.haveAccess(22, "tblSetting", Setting.fldId.ToString()))
                {
                    model.prs_tblSettingUpdate(Setting.fldId, CodeDecode.stringcode(Setting.fldTitle), file, "", u.UserInputId, Setting.fldCompanyFromService);


                    MsgTitle = "ویرایش موفق";
                    Msg = "ویرایش با موفقیت انجام شد";
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
                   
                if (Session["savePathSetting"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathSetting"].ToString());
                    System.IO.File.Delete(physicalPath);
                    Session.Remove("savePathSetting");
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

                var Input = model.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();


                model.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
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
        public ActionResult SaveobservedAnnoncemnet(int AnnoncemnetId, bool Check)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            var user = (UserInfo)Session["User"];
            Models.RaiSamEntities model = new RaiSamEntities();
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Logon", "Account", new { area = "" });
                

                
                model.prs_tblAnnouncementSeenInsert(AnnoncemnetId, user.UserId, Check, user.UserInputId);
                
                MsgTitle = "دخیره موفق";
                Msg = "ذخیره با موفقیت انجام شد";
            }
            catch (Exception x)
            {
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;

                var Input = model.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();


                model.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
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
        public FileContentResult ShowPic()
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return null;
            UserInfo userSession = (UserInfo)(Session["User"]);
            Models.RaiSamEntities model = new RaiSamEntities();
            
            var user = model.prs_tblUserSelect("fldId", userSession.UserId.ToString(),"",1).FirstOrDefault();
           
            var shakhs = model.prs_tblAshkhasSelect("fldId", user.fldShakhsId.ToString(),"","","",1).FirstOrDefault();
            if (shakhs != null && shakhs.fldFileId != null)
            {
                
                var f = model.prs_tblFileSelect("fldId", shakhs.fldFileId.ToString(),1).FirstOrDefault();
                var image = Convert.ToBase64String(f.fldFile);
                if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
                {
                    var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                    return File((byte[])file, "jpg");
                }
                return File((byte[])f.fldFile, "jpg");
            }

            else
            {
                var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                return File((byte[])file, "jpg");
            }
            return null;
        }
        public FileContentResult ShowPicUserSecond()
        {
            if (Session["User"] == null)
                return null;
            UserInfo userSession = (UserInfo)(Session["User"]);
            Models.RaiSamEntities model = new RaiSamEntities();
            
            var user = model.prs_tblUserSelect("fldId", userSession.UserSecondId.ToString(),"",1).FirstOrDefault();
            
            var shakhs = model.prs_tblAshkhasSelect("fldId", user.fldShakhsId.ToString(),"","","",1).FirstOrDefault();
            if (shakhs != null && shakhs.fldFileId != null)
            {
                
                var f = model.prs_tblFileSelect("fldId", shakhs.fldFileId.ToString(),1).FirstOrDefault();
                var image = Convert.ToBase64String(f.fldFile);
                if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
                {
                    var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                    return File((byte[])file, "jpg");
                }
                return File((byte[])f.fldFile, "jpg");
            }

            else
            {
                var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                return File((byte[])file, "jpg");
            }
            return null;
        }
        public ActionResult OnlineUsers()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            if (Models.Permission.haveAccess(23,"","0"))
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                return PartialView;
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "شما مجاز به مشاهده لیست کاربران آنلاین نمی باشید."
                });
                DirectResult result = new DirectResult();
                return result;
                //return null;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReloadOnlineUser()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Permission.haveAccess(23,"","0"))
            {
                var data = UserLoginCount.userObj;
                data = data.OrderByDescending(l => l.AkharinOnMiladi).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { Code = "403" });
            }
        }
        public ActionResult HistoryLogin(string IdUser)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.IdUser = IdUser;
            return PartialView;
        }
        public ActionResult ReadHistoryLogin(StoreRequestParameters parameters, string IdUser)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            if (Permission.haveAccess(24,"","0"))
            {
                var userInf = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblInputInfoSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<Models.prs_tblInputInfoSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            //case "fldId":
                            //    searchtext = ConditionValue.Value.ToString();
                            //    field = "fldId";
                            //    break;
                            case "fldDateTime":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldDateTime";
                                break;
                            case "fldTime":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTime";
                                break;
                            case "fldIP":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldIP";
                                break;
                            case "fldLoginTypeName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldLoginTypeName";
                                break;

                        }
                        
                        if (data != null)
                        {
                            data1 = m.prs_tblInputInfoSelect(field, searchtext,"",false,100).Where(l => l.fldUserID == Convert.ToInt32(IdUser)).ToList();
                        }
                        else
                        {
                            data = m.prs_tblInputInfoSelect(field, searchtext, "", false, 100).Where(l => l.fldUserID == Convert.ToInt32(IdUser)).ToList();
                        }
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    
                    data = m.prs_tblInputInfoSelect("fldUserId_log", IdUser, "", false, 100).ToList();
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

                List<Models.prs_tblInputInfoSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { Code = "403" });
            }
        }
        public ActionResult Error(string Code)
        {
            ViewBag.code = Code;
            string text = "";
            switch (Code)
            {
                default:
                    text = "خطای ناشناخته";
                    break;
                case "400":
                    text = "درخواست نامناسب";
                    break;
                case "401":
                    text = "درخواست غیرمجاز";
                    break;
                case "403":
                    text = "دسترسی غیرمجاز";
                    break;
                case "404":
                    text = "صفحه درخواستی پیدا نشد.";
                    break;
                case "408":
                    text = "مهلت زمانی درخواست به پایان رسیده است.";
                    break;
                case "500":
                    text = "خطای سرور";
                    break;
                case "502":
                    text = "ایجاد مشکل برای سرور";
                    break;
                case "503":
                    text = "سرور در حال حاضر در دسترس نیست.";
                    break;
                case "504":
                    text = "عدم امکان دسترسی موقت به سرور";
                    break;
            }
            ViewBag.text = text;
            return View();
        }
        public ActionResult MohtavaAbout()
        {
            UserInfo user = new UserInfo();
            Models.RaiSamEntities m = new RaiSamEntities();
            
            var PageHtml = m.prs_tblPageHtmlSelect("fldId", "1",1).FirstOrDefault();
            return Json(new { MohtavaAbout = PageHtml.fldMohtavaHtml }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertToLog(string TransactiontTypeName, int TransactionGroupId, bool? Status)
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var Er = 0; var Msg = ""; var MsgTitle = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] != null)
            {
                //return RedirectToAction("Logon", "Account", new { area = "" });
                UserInfo user = (UserInfo)(Session["User"]);
                try
                {
                   
                    m.prs_tblSubTransactionInsert_LogForm(user.UserInputId, TransactiontTypeName, Status, TransactionGroupId);

                     Msg = "ذخیره با موفقیت انجام شد.";
                    MsgTitle = "ذخیره موفق";

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
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult InsertToLogReport(string TransactiontTypeName, int TransactionGroupId, bool? Status)
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var Er = 0; var Msg = ""; var MsgTitle = "";
            if (Session["User"] == null)
            {
                return RedirectToAction("Logon", "Account", new { area = "" });
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("کد گروه تراکنش", TransactionGroupId);
                    parameters.Add("نام نوع تراکنش", TransactiontTypeName);
                    parameters.Add("وضعیت ", Status);
                    parameters.Add("کد جدول ورود و خروج", user.UserInputId);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_tblSubTransactionInsert_Report(user.UserInputId, TransactiontTypeName, Status, TransactionGroupId, jsonstr);

                     Msg = "ذخیره با موفقیت انجام شد."; ;
                     MsgTitle = "ذخیره موفق";
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
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ChangePassword()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            var userInf = (UserInfo)(Session["User"]);
            PartialView.ViewBag.userNamee = userInf.UserName.ToString();
            
            var user = m.prs_tblUserSelect("fldId", userInf.UserSecondId.ToString(),"",1).FirstOrDefault();
            PartialView.ViewBag.fldFirstLogin = user.fldFirstLogin;
            if (user.fldNoeKarbar == true)
            {
                var q = m.prs_tblFirstRegisterSelect("fldId", user.fldFirstRigesterId.ToString(), "", 1).FirstOrDefault();
                PartialView.ViewBag.fldFirstLogin = q.fldFirstLogin;
            }
            return PartialView;
        }
        public ActionResult SaveChangePass(string fldPass, string fldNewPass)
        {
            string Msg = "", MsgTitle = ""; var Er = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            var u = (UserInfo)(Session["User"]);
            try
            {
                if (Session["User"] == null)
                    return RedirectToAction("Logon", "Account", new { area = "" });

                var user = m.prs_tblUserSelect("checkPass", u.UserName, CodeDecode.ComputeSha256Hash(fldPass), 1);

                if (user.FirstOrDefault() == null)
                {
                    return Json(new
                    {
                        Er = 1,
                        MsgTitle = "خطا",
                        Msg = "رمز عبور قبلی وارد شده اشتباه است."
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    m.prs_UserFirsLoginUpdate(u.UserSecondId, false);

                    m.prs_UserPassUpdate(u.UserSecondId, CodeDecode.ComputeSha256Hash(fldNewPass), u.UserSecondId);

                    MsgTitle = "عملیات موفق";
                    Msg = "ذخیره با موفقیت انجام شد.";
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

                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();


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
        public ActionResult ExistUserSecond()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] != null)
            {
                UserInfo userSession = (UserInfo)(Session["User"]);
                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                if (userSession.UserId != userSession.UserSecondId)
                {
                    
                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblInputInfoInsert(Id, IP, macAddr, 0, userSession.UserId, "", Request.Browser.Browser + " Version:" + Request.Browser.Version, userSession.UserKey, 1, userSession.UserSecondId);

                    UserLoginCount.RemoveOnlineUser(userSession.UserId.ToString());


                    userSession.UserInputId = userSession.FirstUserInputId;
                    userSession.UserKey = userSession.UserKeyFirst;
                    userSession.UserId = userSession.UserId;
                    userSession.UserSecondId = userSession.UserId;

                    
                    var h = m.prs_tblInputInfoSelect("fldUserId", userSession.UserId.ToString(),"",true,1).FirstOrDefault();

                   
                    var userLogin = m.prs_tblUserSelect("fldId", userSession.UserId.ToString(),"",1).FirstOrDefault();
                    if (UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).Count() > 0)
                        UserLoginCount.userObj.Remove(UserLoginCount.userObj.Where(item => item.sessionId == System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString()).FirstOrDefault());

                    if (userSession.FirstRegisterId != 0)
                    {
                        userSession.FirstRegisterId = 0;
                        Session.Remove("FristRegisterId");
                    }
                  
                        UserLoginCount.AddOnlineUser(userSession.UserId.ToString(), userLogin.fldNamePersonal, IP, userLogin.fldUserName, h.fldTarikh + " " + h.fldTime /*+ " " +*/ , System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString(), "", "");
                    return RedirectToAction("Index", "Home");
                }

            }
            return null;
        }
        public ActionResult Logout()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Session["User"] != null)
            {
                UserInfo userSession = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                if (userSession.UserId != userSession.UserSecondId)
                {
                    
                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    m.prs_tblInputInfoInsert(Id, IP, macAddr, 0, userSession.UserId, "", Request.Browser.Browser + " Version:" + Request.Browser.Version, userSession.UserKey, 1, userSession.UserSecondId);

                    UserLoginCount.RemoveOnlineUser(userSession.UserId.ToString());


                    userSession.UserInputId = userSession.FirstUserInputId;
                    userSession.UserKey = userSession.UserKeyFirst;
                    userSession.UserId = userSession.UserId;
                    userSession.UserSecondId = userSession.UserId;

                }
                
                System.Data.Entity.Core.Objects.ObjectParameter InputId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                m.prs_tblInputInfoInsert(InputId, IP, macAddr, 0, userSession.UserId, "", Request.Browser.Browser + " Version:" + Request.Browser.Version, userSession.UserKey, 1, userSession.UserId);
                UserLoginCount.RemoveOnlineUser(userSession.UserId.ToString());

                //
               
                m.prs_tblSubTransactionInsert_LogForm(userSession.UserInputId, "خروج کاربر",true,7);

                m.prs_tblSafDownloadUpdate("fldInputId", Convert.ToInt64(userSession.UserInputId), 4);
                //
            }

            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["SFToken"] != null)
            {
                Response.Cookies["SFToken"].Value = string.Empty;
                Response.Cookies["SFToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Logon", "Account", new { area = "" });
        }
        public ActionResult About()
        {
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
