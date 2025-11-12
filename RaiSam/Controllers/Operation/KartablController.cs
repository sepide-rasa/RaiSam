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
    public class KartablController : Controller
    {
        //
        // GET: /Kartabl/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new KartablAction();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                ViewData = ViewData,
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
        public FileContentResult ShowHelpKartabl()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "15", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinKartabl()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoKartabl()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "15", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }

        public ActionResult New(int CartableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            ViewData.Model = new Models.KartablAction();
            result.ViewData = ViewData;
            result.ViewBag.CartableId = CartableId.ToString();
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadDetail(StoreRequestParameters parameters, int CartablId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(120, "tblKartabl", "0"))
                {
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblEkhtesasActions_CartableSelect> data = null;
                
                    data = m.prs_tblEkhtesasActions_CartableSelect("fldCartableId", CartablId.ToString(),"","",0).ToList();
                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblEkhtesasActions_CartableSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        //public ActionResult ReadCharkhe(int EghdamId)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    param.FieldName = "fldEghdamId";
        //    param.Value = EghdamId.ToString();
        //    param.Top = 0;
        //    var q = service.SelectCharkhe_Eghdam(param, user.UserKey, IP).Charkhe_EghdamList;
        //    return this.Store(q);
        //}
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            try
            {
                if (Session["savePathKartabl"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathKartabl"].ToString());
                    Session.Remove("savePathKartabl");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (IsImage == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 307200)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        file.SaveAs(savePath);
                        Session["FileName"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                        Session["savePathKartabl"] = savePath;
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
                            Message = "حجم فایل انتخاب شده غیرمجاز است.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخاب شده غیرمجاز است."
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
        public ActionResult ShowPicUpload(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathKartabl"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }
        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathKartabl"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathKartabl"].ToString());
                Session.Remove("savePathKartabl");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblKartablSelect Kartabl, string Actions1, bool DeleteFileKK)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; byte[] file = null; string Pasvand = ""; string NameFile = ""; byte TimeStamp = 1;
                int IdHeader = 0; var Change = 0;
                string jsonstr = ""; bool checkRepeat = false;
                Models.RaiSamEntities m = new RaiSamEntities();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                try
                {
                    List<prs_tblEkhtesasActions_CartableSelect> Actions = JsonConvert.DeserializeObject<List<prs_tblEkhtesasActions_CartableSelect>>(Actions1);
                    if (Kartabl.fldDesc == null)
                        Kartabl.fldDesc = "";
                   if (Kartabl.fldId == 0)
                    {
                        //ذخیره
                        if (Session["savePathKartabl"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathKartabl"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePathKartabl"].ToString());
                            file = stream.ToArray();
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            Pasvand = "";
                            file = null;
                            NameFile = "";
                        }

                        parameters.Add("عنوان کارتابل", Kartabl.fldName);
                        parameters.Add("پسوند", Pasvand);
                        parameters.Add("نام فایل", NameFile);
                        parameters.Add("توضیحات", Kartabl.fldDesc);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));

                        var s = m.prs_tblKartablInsert(Id, Kartabl.fldName, Kartabl.fldDesc, u.UserInputId, file, Pasvand, NameFile, jsonstr);
                        if (Session["savePathKartabl"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathKartabl"].ToString());
                            Session.Remove("savePathKartabl");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }
                        IdHeader = Convert.ToInt32(Id.Value);
                       

                            foreach (var item in Actions)
                            {
                                checkRepeat = m.prs_tblEkhtesasActions_CartableSelect("fldCharkhe_EghdamId", item.fldCharkhe_EghdamId.ToString(), "", "", 0).Any();
                                if (item.fldDesc == null)
                                    item.fldDesc = "";
                                parameters = new Dictionary<string, object>();
                                parameters.Add("نام کارتابل", Kartabl.fldName);
                                parameters.Add("ترتیب", item.fldOrder);
                                parameters.Add("توضیحات", item.fldDesc);
                                if (checkRepeat == false)
                                {
                                    Msg = "ذخیره با موفقیت انجام شد.";
                                    MsgTitle = "ذخیره موفق";
                                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                    m.prs_tblEkhtesasActions_CartableInsert(IdHeader, item.fldOrder, item.fldDesc, u.UserInputId, item.fldCharkhe_EghdamId, jsonstr);
                                }
                                else
                                {
                                    Msg = "اقدام انتخاب شده تنها به یک کارتابل میتواند اختصاص داده شود";
                                    MsgTitle = "خطا";
                                    Er = 1;

                                    parameters.Add("متن خطا", "EkhtesasActions_Cartable:Save: " + Msg);
                                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                    m.prs_LogInsert(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr, false);
                                }
                                
                            
                        }
                    }
                    else
                    {
                        IdHeader = Kartabl.fldId;
                        //ویرایش
                        if (Session["savePathKartabl"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathKartabl"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePathKartabl"].ToString());
                            file = stream.ToArray();
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            if (DeleteFileKK == true)
                            {
                                file = null;
                                Pasvand = "";
                                NameFile = "";
                            }
                            else
                            {
                                var Ac = m.prs_tblFileSelect("fldId", Kartabl.fldFileId.ToString(),0).FirstOrDefault();

                                if (Ac != null)
                                {
                                    file = Ac.fldFile;
                                    Pasvand = Ac.fldPasvand;
                                    NameFile = Ac.fldFileName;
                                }
                            }
                        }

                        var s = m.prs_tblKartablUpdate(Kartabl.fldId, Kartabl.fldName, Kartabl.fldFileId, Kartabl.fldDesc, u.UserInputId, file, Pasvand, NameFile,Kartabl.fldTimeStamp).FirstOrDefault();
                        if (Session["savePathKartabl"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathKartabl"].ToString());
                            Session.Remove("savePathKartabl");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }
                        parameters = new Dictionary<string, object>();
                        parameters.Add("عنوان کارتابل", Kartabl.fldName);
                        parameters.Add("پسوند", Pasvand);
                        parameters.Add("نام فایل", NameFile);
                        parameters.Add("توضیحات", Kartabl.fldDesc);
                        if (s.flag == 1)
                        {
                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "ذخیره موفق";
                        }
                        else if (s.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (s.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }
                        var Logstatus = true;
                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "Kartabl:Edit: " + Msg);
                            Logstatus = false;
                        }
                         
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblKartabl", jsonstr, Logstatus, Kartabl.fldId);

                            m.prs_tblEkhtesasActions_CartableDelete(Kartabl.fldId,"fldCartableId");
                            
                                var q = m.prs_tblEkhtesasActions_CartableSelect("fldCartableId", Kartabl.fldId.ToString(),"","",0).ToList();
                                foreach (var item in q)
                                {
                                    Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                                    parameters1.Add("عنوان کارتابل", item.fldName_Kartabl);
                                    parameters1.Add("ترتیب", item.fldOrder);
                                    parameters1.Add("توضیحات", item.fldDesc);
                                    string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);

                                    m.prs_LogDelete(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr1, true, item.fldId);
                                }
                            
                            foreach (var item in Actions)
                            {
                                checkRepeat = m.prs_tblEkhtesasActions_CartableSelect("fldCharkhe_EghdamId", item.fldCharkhe_EghdamId.ToString(), "", "", 0).Any();
                                if (item.fldDesc == null)
                                    item.fldDesc = "";
                                parameters = new Dictionary<string, object>();
                                parameters.Add("نام کارتابل", Kartabl.fldName);
                                parameters.Add("ترتیب", item.fldOrder);
                                parameters.Add("توضیحات", item.fldDesc);
                                if (checkRepeat == false)
                                {
                                    Msg = "ذخیره با موفقیت انجام شد.";
                                    MsgTitle = "ذخیره موفق";
                                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                    m.prs_tblEkhtesasActions_CartableInsert(Kartabl.fldId, item.fldOrder, item.fldDesc, u.UserInputId, item.fldCharkhe_EghdamId, jsonstr);
                                }
                                else
                                {
                                    Msg = "اقدام انتخاب شده تنها به یک کارتابل میتواند اختصاص داده شود";
                                    MsgTitle = "خطا";
                                    Er = 1;

                                    parameters.Add("متن خطا", "EkhtesasActions_Cartable:Save: " + Msg);
                                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                    m.prs_LogInsert(u.UserInputId, "dbo.tblEkhtesasActions_Cartable", jsonstr, false);
                                }


                            
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    if (Session["savePathKartabl"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePathKartabl"].ToString());
                        Session.Remove("savePathKartabl");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
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
                    IdHeader = IdHeader,
                    TimeStamp = TimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int fldTimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; byte outTimeStamp = 1; var Change = 0;
                Models.RaiSamEntities m = new RaiSamEntities();

                string jsonstr = "";
                var q = m.prs_tblKartablSelect("fldId", id.ToString(),0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان کارتابل", q.fldName);
                parameters.Add("توضیحات", q.fldDesc);
                try
                {//حذف
                      if (Permission.haveAccess(123, "dbo.tblActions", id.ToString()))
                    {
                    var s = m.prs_tblKartablDelete(id, fldTimeStamp).FirstOrDefault();
                    if (s.flag == 1)
                    {
                        Msg = "حذف با موفقیت انجام شد";
                        MsgTitle = "حذف موفق";
                    }
                    else if (s.flag == 0)
                    {
                        Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                        MsgTitle = "هشدار";
                        Er = 1;
                        Change = 1;
                    }
                    else if (s.flag == 2)
                    {
                        Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                        MsgTitle = "خطا";
                        Er = 1;
                        Change = 2;
                    }
                    var Logstatus = true;
                    if (Er == 1)
                    {
                        parameters.Add("متن خطا", "Kartabl:Delete: " + Msg);
                        Logstatus = false;
                    }

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogDelete(u.UserInputId, "dbo.tblKartabl", jsonstr, Logstatus, id);
                    
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
                    parameters.Add("متن خطا", InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(u.UserInputId, "dbo.tblKartabl", jsonstr, false, id);
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
                    outTimeStamp = outTimeStamp
                }, JsonRequestBehavior.AllowGet);
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
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblKartablSelect("fldId", Id.ToString(),1).FirstOrDefault();

                var Iconn = "";
                var file = m.prs_tblFileSelect("fldId", q.fldFileId.ToString(),0).FirstOrDefault();
                if (file != null)
                {
                    Iconn = Convert.ToBase64String(file.fldFile);
                }
                return Json(new
                {
                    fldId = q.fldId,
                    fldName = q.fldName,
                    fldDesc = q.fldDesc,
                    fldFileId = q.fldFileId,
                    fldFileName = q.fldFileName,
                    fldTimeStamp = q.fldTimeStamp,
                    fldIcon = Iconn
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(120, "tblKartabl","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblKartablSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblKartablSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;
                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldName";
                                    break;
                                case "fldDesc":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDesc";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_tblKartablSelect(field, searchtext,100).ToList();
                            else
                                data = m.prs_tblKartablSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblKartablSelect("","",100).ToList();
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

                    List<prs_tblKartablSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadAction(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                //if (Permission.haveAccess(82))
                //{
                Models.RaiSamEntities m = new RaiSamEntities();
                List<prs_tblCharkhe_EghdamSelect> data = null;

                data = m.prs_tblCharkhe_EghdamSelect("","","",100).ToList();
                return this.Store(data);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadEkhtesas(int CartableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(120, "tblKartabl", "0"))
                {
                Models.RaiSamEntities m = new RaiSamEntities();

                    List<prs_tblEkhtesasActions_CartableSelect> data = null;
                    data = m.prs_tblEkhtesasActions_CartableSelect("fldCartableId", CartableId.ToString(),"","",0).ToList();
                    return this.Store(data);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }

    }
}
