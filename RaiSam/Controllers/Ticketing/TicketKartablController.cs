using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace RaiSam.Controllers.Ticketing
{
    public class TicketKartablController : Controller
    {
        //
        // GET: /TicketKartabl/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            //result.ViewBag.TreeId = u.TreeId.ToString();
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            var q = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(),"",1).FirstOrDefault();
            result.ViewBag.setadi = q.fldSetadi;
            return result;
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpTicketKartabl()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "10", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinTicketKartabl()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoTicketKartabl()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "10", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        public ActionResult New(int Ashkhas_FirstId, string Type, int CategoryId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var ValidFormat = "";
            var Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
            var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
            foreach (var item in MojazNew)
            {
                ValidFormat = ValidFormat + item + ";";
            }
            PartialView.ViewBag.Ashkhas_FirstId = Ashkhas_FirstId.ToString();
            PartialView.ViewBag.Type = Type;
            PartialView.ViewBag.CategoryId = CategoryId.ToString();
            PartialView.ViewBag.ValidFormat = ValidFormat;
            return PartialView;
        }
        public ActionResult NewChatWin(string Type, int CategoryId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var ValidFormat = "";
            var Mojaz = m.prs_tblTicketFormatFileSelect("", "", 0).Select(l => l.fldPassvand.ToLower()).ToList();
            var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
            foreach (var item in MojazNew)
            {
                ValidFormat = ValidFormat + item + ";";
            }
            PartialView.ViewBag.Type = Type;
            PartialView.ViewBag.CategoryId = CategoryId.ToString();
            PartialView.ViewBag.ValidFormat = ValidFormat;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCategory(string TypeMsg)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                var q = m.prs_tblTicketCategorySelect("fldType_Permission", TypeMsg, u.UserSecondId.ToString(),0).ToList().Select(n => new { ID = n.fldId, Name = n.fldTitle });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadChat(int Id, int CategoryId, int countTicket)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo userSession = (UserInfo)(Session["User"]);

                var type = "";
                var matn = "";
                var att = "";
                var time = "";
                var UserIdd = "";
                var seen = "";
                int AshkhasId = 0;
                var ChatId = "";
                var Forwarded = "";

                var q = m.prs_tblTicketSelect("fldAshkhasId", Id.ToString(), CategoryId.ToString(), countTicket).ToList();
                var cc=m.prs_tblTicketCategorySelect("fldId", CategoryId.ToString(),"",0).FirstOrDefault();
                if(cc.fldType==false)
                    q = m.prs_tblTicketSelect("fldFirstRegisterId", Id.ToString(), CategoryId.ToString(), countTicket).ToList();

                var user = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(),"", 1).FirstOrDefault();
                AshkhasId = Convert.ToInt32(user.fldShakhsId);

                var q1 = m.prs_tblTicketSelect("fldAshkhasId_NotSeen", AshkhasId.ToString(),"",0).ToList();

                var q2 = m.prs_tblTicketSelect("Permmision", AshkhasId.ToString(),"",0).ToList();

                var ReplyPermission = m.prs_CheckTicketPermission("Answer", u.UserSecondId, CategoryId).FirstOrDefault().fldPermission;

                foreach (var item in q)
                {
                    if (item.fldUserId != null)
                    {
                        UserIdd = UserIdd + item.fldUserId + "|";
                    }
                    else
                    {
                        UserIdd = UserIdd + "0|";
                    }


                    int? haveAtt = 0;
                    if (item.fldFileId != null)
                        haveAtt = item.fldFileId;
                    if (item.fldSeen)
                        seen = seen + "1" + "|";
                    else
                        seen = seen + "0" + "|";
                    var t = 2;
                    if (item.fldUserId != null)
                        t = 1;
                    type = type + t + "|";

                    var matnHtml = item.fldHTML;
                    if (item.fldSourceReplyId != null)
                    {
                        
                        var tt = m.prs_tblTicketSelect("fldId", item.fldSourceReplyId.ToString(),"",0).FirstOrDefault();
                        matnHtml = "<div style='background-color: #bbd2f0;cursor: pointer;' onclick='FnShowTiketHtml(" + item.fldSourceReplyId.ToString() + ")' title='" + tt.Matn + "' >پاسخ به: " + item.fldMatnReply + "</div>" + matnHtml;
                    }
                    matn = matn + matnHtml + "|";
                    att = att + haveAtt + "|";
                    time = time + item.fldTarikh + "|";
                    ChatId = ChatId + item.fldId + "|";
                    Forwarded = Forwarded + item.fldSourceForwardId + "|";
                }
                return Json(new
                {
                    type = type,
                    matn = matn,
                    att = att,
                    time = time,
                    UserIdd = UserIdd,
                    seen = seen,
                    ReplyPermission = ReplyPermission,
                    CountUser = q1.Count,
                    CountAdmin = q2.Count,
                    Count = countTicket,
                    ChatId = ChatId,
                    Forwarded = Forwarded
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult DownloadAttach(int Id)
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                var f = m.prs_tblFileSelect("fldId", Id.ToString(),1).FirstOrDefault();
                if (f != null)
                {
                    MemoryStream st = new MemoryStream(f.fldFile);
                    //return File(st.ToArray(), MimeType.Get(System.IO.Path.GetExtension(f.fldPasvand)), "File." + f.fldPasvand);
                    return File((byte[])f.fldFile, MimeType.Get(f.fldPasvand), "DownloadFile." + f.fldPasvand);
                }
                return null;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters, string TypeMsg, string Category)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(50, "", "0"))
                {
                    var fieldName = "ComiteUserIdGroup";
                    if (TypeMsg == "0")
                        fieldName = "FirstRegisterIdGroup";

                    var user = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(),"",1).FirstOrDefault();

                    List<Models.prs_tblTicketSelect> data = null;
                  
                    data = m.prs_tblTicketSelect(fieldName, Category,"",0).ToList();

                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<Models.prs_tblTicketSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }
        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }
        [HttpPost]
        public ActionResult Upload()
        {
            UserInfo userSession = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (Session["savePathTicket"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathTicket"].ToString());
                    Session.Remove("savePathTicket");
                    Session.Remove("Pasvand");
                    Session.Remove("FileNameTicket");
                    System.IO.File.Delete(physicalPath);
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var Allowed = false;
                var Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
                var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
                if (MojazNew.Contains(extension.Substring(1)))
                {
                    switch (extension.ToLower())
                    {
                        case ".pdf":
                            Allowed = FileInfoExtensions.IsPDF(Request.Files[0]);
                            break;
                        case ".xls":
                        case ".xlsx":
                            Allowed = FileInfoExtensions.IsExcel(Request.Files[0]);
                            break;
                        case ".doc":
                        case ".docx":
                            Allowed = FileInfoExtensions.IsWord(Request.Files[0]);
                            break;
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                        case ".tiff":
                        case ".tif":
                        case ".gif":
                        case ".bmp":
                            Allowed = FileInfoExtensions.IsImage(Request.Files[0]);
                            break;
                        case ".mp4":
                            Allowed = FileInfoExtensions.IsMP4(Request.Files[0]);
                            break;
                        default:
                            Allowed = FileInfoExtensions.blockList(Request.Files[0]);
                            break;
                    }
                }
                if (Allowed == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".pdf"
                    || extension == ".tif" || extension == ".tiff" || extension == ".ico" || extension == ".bmp"
                    || extension == ".doc" || extension == ".docx" || extension == ".txt" || extension == ".xls"
                    || extension == ".xlsx" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 10485760)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        string e = Path.GetExtension(savePath);
                        file.SaveAs(savePath);
                        Session["savePathTicket"] = savePath;
                        Session["Pasvand"] = file.FileName.Split('.').Last();
                        Session["FileNameTicket"] = file.FileName.Split('.').First();
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
                            Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد."
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
                        Message = "فرمت فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فرمت فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
                }
            }
            catch (Exception x)
            {
                if (Session["savePathTicket"] != null)
                {
                    System.IO.File.Delete(Session["savePathTicket"].ToString());
                    Session.Remove("savePathTicket");
                    Session.Remove("Pasvand");
                    Session.Remove("FileNameTicket");
                }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string Ticket1, string HaveFile1, int? SourceId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var OutId = 0;
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo userSession = (UserInfo)(Session["User"]);
                string Msg = "", MsgTitle = ""; var Er = 0; 

                var s = m.prs_GetDate().FirstOrDefault();
                var d = s.fldTarikh + "-" + s.fldTime.Substring(0, 5);
                try
                {
                    string Pasvand = "";
                    byte[] report_file = null; string FileName = "";
                    Models.prs_tblTicketSelect Ticket = JsonConvert.DeserializeObject<Models.prs_tblTicketSelect>(Ticket1);

                    var template = new { HaveFile = false };
                    bool HaveFile = JsonConvert.DeserializeAnonymousType(HaveFile1, template).HaveFile;

                    Ticket.fldHTML = System.Uri.UnescapeDataString(Ticket.fldHTML);

                    if (HaveFile == true && Session["savePathTicket"] != null)
                    {
                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathTicket"].ToString()));
                        report_file = stream.ToArray();
                        Pasvand = Session["Pasvand"].ToString();
                        FileName = Session["FileNameTicket"].ToString();
                    }
                    if (Ticket.fldHTML == null)
                        Ticket.fldHTML = "";
                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("FileId", typeof(int));

                    Ticket.fldFirstRegisterId = null;
                    var cc = m.prs_tblTicketCategorySelect("fldId", Ticket.fldTicketCategoryId.ToString(), "", 0).FirstOrDefault();
                    if (cc.fldType == false)
                    {
                        var user = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(), "", 1).FirstOrDefault();
                        Ticket.fldFirstRegisterId = Ticket.fldAshkhasId;
                        Ticket.fldAshkhasId = Convert.ToInt32(user.fldShakhsId);
                    }

                    m.prs_tblTicketInsert(Id, Ticket.fldFirstRegisterId, Ticket.fldHTML, false, report_file, Pasvand, u.UserSecondId, "", Ticket.fldTicketCategoryId, Ticket.fldAshkhasId, FileName,
                        u.UserInputId, null, null, SourceId);

                   OutId= Convert.ToInt32(Id.Value);

                    if (Session["savePathTicket"] != null)
                    {
                        System.IO.File.Delete(Session["savePathTicket"].ToString());
                        Session.Remove("savePathTicket");
                        Session.Remove("Pasvand");
                        Session.Remove("FileNameTicket");
                    }
                }
                catch (Exception x)
                {
                    var InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    if (Session["savePathTicket"] != null)
                    {
                        System.IO.File.Delete(Session["savePathTicket"].ToString());
                        Session.Remove("savePathTicket");
                        Session.Remove("Pasvand");
                        Session.Remove("FileNameTicket");
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
                    Er = Er,
                    time = d,
                    FileId = OutId,
                    Iduser = u.UserSecondId
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowPic(int id, byte State)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            var image = "";

            if (id != 0)
            {
                int ShakhsId = id;
                if (State == 2)//ورودی آیدی کاربر است
                {
                    var user = m.prs_tblUserSelect("fldId", id.ToString(),"",1).FirstOrDefault();
                    ShakhsId = Convert.ToInt32(user.fldShakhsId);
                }
                var shakhs = m.prs_tblAshkhasSelect("fldId", ShakhsId.ToString(), "", "", "", 1).FirstOrDefault();

               

                if (shakhs.fldFileId == null)
                {
                    var Image = Server.MapPath("~/Content/user.png");
                    if (State == 2)
                        Image = Server.MapPath("~/Content/user1.png"); 

                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                    image = Convert.ToBase64String(stream.ToArray());
                }
                else
                {
                    var FilePerson = m.prs_tblFileSelect("fldId", shakhs.fldFileId.ToString(), 1).FirstOrDefault();
                    if (FilePerson.fldFile == null)
                    {
                        var Image = Server.MapPath("~/Content/user.png");
                        if (State == 2)
                            Image = Server.MapPath("~/Content/user1.png");

                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                        image = Convert.ToBase64String(stream.ToArray());
                    }
                    else
                    {
                        image = Convert.ToBase64String(FilePerson.fldFile);
                        if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
                        {
                            var Image = Server.MapPath("~/Content/user.png");
                            if (State == 2)
                                Image = Server.MapPath("~/Content/user1.png");

                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                            image = Convert.ToBase64String(stream.ToArray());
                        }
                    }
                }
            }
            else
            {
                var q = m.prs_tblSettingSelect("","",1).FirstOrDefault();
                if (q != null)
                {
                    if (q.fldFile != null)
                    {
                        image = Convert.ToBase64String(q.fldFile);
                    }
                }
            }


            return new JsonResult()
            {
                Data = new
                {
                    image = image
                },
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSeenAdmin(int Id, int CategoryId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
             
                m.prs_tblTicketUpdateBySetadUserId(Id, CategoryId, "Admin");
                //}
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void ReloadFromAdmin()
        {
            SignalrHub h = new SignalrHub();
            h.Send("");//صدا زدن Clients.All.broadcastMessage
        }
        [HttpPost]
        public ActionResult GetCountPM()
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                List<Models.prs_tblTicketSelect> q = new List<Models.prs_tblTicketSelect>();
                List<Models.prs_tblTicketSelect> q2 = new List<Models.prs_tblTicketSelect>();

                if (Session["User"] != null)
                {
                    var UserInfo = (UserInfo)(Session["User"]);
                    var user = m.prs_tblUserSelect("fldId", UserInfo.UserSecondId.ToString(),"",1).FirstOrDefault();

                    q = m.prs_tblTicketSelect("fldAshkhasId_NotSeen", user.fldShakhsId.ToString(),"",0).ToList();

                    q2 = m.prs_tblTicketSelect("Permision_New", user.fldShakhsId.ToString(), "", 0).ToList();
                }
                else
                {
                    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);

                    q = m.prs_tblTicketSelect("fldAshkhasId_NotSeen", MovazafInfo.AshkhasId.ToString(),"",0).ToList();

                    q2 = m.prs_tblTicketSelect("Permmision", MovazafInfo.AshkhasId.ToString(),"",0).ToList();
                }
                return Json(new
                {
                    CountUser = q.Count,
                    CountAdmin = q2.Count
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CheckHavePm()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                int UserLoginCount = RaiSam.Models.UserLoginCount.userObj.Count();
                return Json(new
                {
                    UserOnline = UserLoginCount
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult OpenForwardWin(int chatId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.chatId = chatId;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadCategory(int chatId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo userSession = (UserInfo)(Session["User"]);
                
                var Ticket = m.prs_tblTicketSelect("fldId", chatId.ToString(),"",0).FirstOrDefault();

                var data = m.prs_tblTicketCategorySelect("See", userSession.UserSecondId.ToString(),"",0).Where(l => l.fldId != Ticket.fldTicketCategoryId).ToList();

                return Json(new
                {
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadMatnHtml(int chatId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo userSession = (UserInfo)(Session["User"]);
               
                var data = m.prs_tblTicketSelect("fldId", chatId.ToString(),"",0).FirstOrDefault();

                return Json(new
                {
                    fldHTML = data.fldHTML
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveForward(string Data)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0;
                var s = m.prs_GetDate().FirstOrDefault();
                var d = s.fldTarikh + "-" + s.fldTime.Substring(0, 5);
                string Pasvand = "";
                var FileId = 0;
                var template = new { category = new List<Models.prs_tblTicketCategorySelect>(), Matn = "", chatId = "" };
                var Matn = System.Uri.UnescapeDataString(JsonConvert.DeserializeAnonymousType(Data, template).Matn);
                var category = JsonConvert.DeserializeAnonymousType(Data, template).category;
                var chatId = JsonConvert.DeserializeAnonymousType(Data, template).chatId;
                try
                {
                    UserInfo userSession = (UserInfo)(Session["User"]);
                    
                    var Ticket = m.prs_tblTicketSelect("fldId", chatId.ToString(),"",0).FirstOrDefault();

                    if (Matn == null)
                        Matn = "";


                    MsgTitle = "عملیات موفق";
                    Msg = "پیام با موفقیت انتقال داده شد.";

                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("FileId", typeof(int));
               
                    foreach (var item in category)
                    {
                        m.prs_tblTicketInsert(Id,Ticket.fldFirstRegisterId, Matn, false, null, Pasvand, Ticket.fldUserId, "", item.fldId, Ticket.fldAshkhasId, "",
                        u.UserInputId, Ticket.fldId, userSession.UserSecondId, null);
                        FileId =Convert.ToInt32(Id.Value);
                       
                    }

                }
                catch (Exception x)
                {
                    var InnerException = "";
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
                        Err = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Err = Er,
                    time = d,
                    FileId = FileId
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ForwardHistory(int chatId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.chatId = chatId;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadForwardHistory(int chatId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo userSession = (UserInfo)(Session["User"]);
                var data = m.prs_TicketForwardHistory(chatId).ToList();

                return Json(new
                {
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult OpenReplyWin(int chatId)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                var ValidFormat = "";

                var Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
                var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
                foreach (var item in MojazNew)
                {
                    ValidFormat = ValidFormat + item + ";";
                }
                PartialView.ViewBag.ValidFormat = ValidFormat;
                PartialView.ViewBag.chatId = chatId;
                return PartialView;
            }
        }
        [HttpPost]
        public ActionResult UploadReply()
        {
            UserInfo userSession = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePathReply"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathReply"].ToString());
                    Session.Remove("savePathReply");
                    Session.Remove("FileNameReply");
                    Session.Remove("PasvandReply");
                    System.IO.File.Delete(physicalPath);
                }
                HttpPostedFileBase file = Request.Files[0];
                string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                string e = Path.GetExtension(savePath);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var Allowed = false;
               
                var Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
                var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
                if (MojazNew.Contains(extension.Substring(1)))
                {
                    switch (extension.ToLower())
                    {
                        case ".pdf":
                            Allowed = FileInfoExtensions.IsPDF(Request.Files[0]);
                            break;
                        case ".xls":
                        case ".xlsx":
                            Allowed = FileInfoExtensions.IsExcel(Request.Files[0]);
                            break;
                        case ".doc":
                        case ".docx":
                            Allowed = FileInfoExtensions.IsWord(Request.Files[0]);
                            break;
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                        case ".tiff":
                        case ".tif":
                        case ".gif":
                        case ".bmp":
                            Allowed = FileInfoExtensions.IsImage(Request.Files[0]);
                            break;
                        case ".mp4":
                            Allowed = FileInfoExtensions.IsMP4(Request.Files[0]);
                            break;
                        default:
                            Allowed = FileInfoExtensions.blockList(Request.Files[0]);
                            break;
                    }
                }
                if (Allowed == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".pdf"
                    || extension == ".tif" || extension == ".tiff" || extension == ".ico" || extension == ".bmp"
                    || extension == ".doc" || extension == ".docx" || extension == ".txt" || extension == ".xls"
                    || extension == ".xlsx" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 10485760)
                    {
                        file.SaveAs(savePath);
                        Session["FileNameReply"] = file.FileName;
                        Session["savePathReply"] = savePath;
                        Session["PasvandReply"] = file.FileName.Split('.').Last();
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
                            Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد."
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
                        Message = "فرمت فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فرمت فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
                }
            }
            catch (Exception x)
            {
                if (Session["savePathReply"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathReply"].ToString());
                    Session.Remove("savePathReply");
                    Session.Remove("FileNameReply");
                    Session.Remove("PasvandReply");
                    System.IO.File.Delete(physicalPath);
                }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReply(string Data/*string MatnHtml, bool? HaveFile, int SourceId*/)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "پاسخ با موفقیت ارسال شد.", MsgTitle = ""; var Er = 0;

                var template = new { MatnHtml = "", HaveFile = false, SourceId = "" };
                var MatnHtml = System.Uri.UnescapeDataString(JsonConvert.DeserializeAnonymousType(Data, template).MatnHtml);
                var HaveFile = JsonConvert.DeserializeAnonymousType(Data, template).HaveFile;
                var SourceId = JsonConvert.DeserializeAnonymousType(Data, template).SourceId;

                var s = m.prs_GetDate().FirstOrDefault();
                var d = s.fldTarikh + "-" + s.fldTime.Substring(0, 5);
                string Pasvand = ""; string FileName = "";
                var FileId = 0;
                UserInfo userSession = (UserInfo)(Session["User"]);
                try
                {
                    byte[] report_file = null;
                    if (HaveFile == true && Session["savePathReply"] != null)
                    {
                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathReply"].ToString()));
                        report_file = stream.ToArray();
                        Pasvand = Session["PasvandReply"].ToString();
                        FileName = Session["FileNameReply"].ToString();
                    }
                    if (MatnHtml == null)
                        MatnHtml = "";


                    var Ticket = m.prs_tblTicketSelect("fldId", SourceId.ToString(),"",0).FirstOrDefault();

                    //var haveSmsPanel = service.GetSMSSettingWithFilter("", "", 1, Session["Username"].ToString(), (Session["Password"].ToString()), out Err).FirstOrDefault();
                    //RaiSms.Service w = new RaiSms.Service();
                    //var Matn = "به تیکت شما در سامانه نظام فنی، پاسخ داده شد.";
                    //var r = servic.GetRequestRankingWithFilter("fldId", Ticket.fldRequestId.ToString(), false, 0, out Err).FirstOrDefault();

                    //if (Ticket.fldRequestId != null)
                    //{
                    //    var q = servic.GetFirstRegisterWithFilter("fldId", r.fldFirstRegisterId.ToString(), "", 1, out Err).FirstOrDefault();
                    //    var k = servic.GetTicketWithFilter("fldRequestId", Ticket.fldRequestId.ToString(), Ticket.fldTicketCategoryId.ToString(), 0, Session["Username"].ToString(), (Session["Password"].ToString()), out Err).OrderByDescending(l => l.fldId).FirstOrDefault();

                    //}
                      System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("FileId", typeof(int));

                      m.prs_tblTicketInsert(Id, Ticket.fldFirstRegisterId, MatnHtml, false, report_file, Pasvand, userSession.UserSecondId, "", Ticket.fldTicketCategoryId, Ticket.fldAshkhasId, FileName,
                        u.UserInputId, null, null, Convert.ToInt32(SourceId));
                        FileId =Convert.ToInt32(Id.Value);


                    if (Session["savePathReply"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePathReply"].ToString());
                        Session.Remove("savePathReply");
                        Session.Remove("FileNameReply");
                        Session.Remove("PasvandReply");
                        System.IO.File.Delete(physicalPath);
                    }
                }
                catch (Exception x)
                {
                    var InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    if (Session["savePathReply"] != null)
                    {
                        System.IO.File.Delete(Session["savePathReply"].ToString());
                        Session.Remove("savePathReply");
                        Session.Remove("PasvandReply");
                        Session.Remove("FileNameReply");
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
                    Err = Er,
                    time = d,
                    FileId = FileId
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
