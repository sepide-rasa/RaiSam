using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System.IO;
using System.Threading;

namespace RaiSam.Controllers.BasicInfo
{
    public class AshkhasHoghooghiController : Controller
    {
        //
        // GET: /AshkhasHoghooghi/
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

        public ActionResult Help()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpAshkhasHoghooghi()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "19", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinAshkhasHoghooghi()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoAshkhasHoghooghi()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "19", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Permission.haveAccess(56,"","0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                UserInfo user = (UserInfo)(Session["User"]);
                List<prs_tblAshkhasHoghooghiSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<prs_tblAshkhasHoghooghiSelect> data1 = null;
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
                            case "fldNationalCode":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNationalCode";
                                break;
                            case "fldTarikhTasis":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTarikhTasis";
                                break;

                        }
                        if (data != null)
                            data1 = m.prs_tblAshkhasHoghooghiSelect(field, searchtext,100).ToList();
                        else
                            data = m.prs_tblAshkhasHoghooghiSelect(field, searchtext, 100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblAshkhasHoghooghiSelect("","",100).ToList();
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

                List<prs_tblAshkhasHoghooghiSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { Code = "403" });
            }
        }

        public ActionResult New(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.Id = Id;
            return result;
        }
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePathAshkhasHoghooghi"] != null)
                {
                    System.IO.File.Delete(Session["savePathAshkhasHoghooghi"].ToString());
                    Session.Remove("FileName");
                    Session.Remove("savePathAshkhasHoghooghi");
                }
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (IsImage == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 1024000)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathAshkhasHoghooghi"] = savePath;
                        Session["FileName"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
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

        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathAshkhasHoghooghi"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathAshkhasHoghooghi"].ToString());
                Session.Remove("savePathAshkhasHoghooghi");
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
        public ActionResult ShowPic(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathAshkhasHoghooghi"].ToString()));
            file = stream.ToArray();
            var image = Convert.ToBase64String(file);
            return Json(new { image = image });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblAshkhasHoghooghiSelect("fldId", Id.ToString(),1).FirstOrDefault();
            byte[] file = null; string FileName = "";
            if (q.fldFileId != null)
            {
                var f = m.prs_tblUploadFileCompanySelect("fldId", q.fldFileId.ToString(), 1).FirstOrDefault();
                file = f.fldFile;
                FileName = f.fldFileName;
            }

            string pic = "";
            if (file != null)
            {
                pic = Convert.ToBase64String(file);
            }
            return Json(new
            {
                fldId = q.fldId,
                fldName = q.fldName,
                fldNationalCode = q.fldNationalCode,
                fldTarikhTasis = q.fldTarikhTasis,
                fldFileId = q.fldFileId,
                NameFile = FileName,
                pic = pic,
                fldTimeStamp = q.fldTimeStamp,
                fldAddress=q.fldAddress,
                fldCodeEghtesadi=q.fldCodeEghtesadi,
                fldCodePosti=q.fldCodePosti,
                fldDateSabt=q.fldDateSabt,
                fldShomareSabt=q.fldShomareSabt
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAx(int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_tblAshkhasHoghooghiSelect("fldId", fldId.ToString(),1).FirstOrDefault();
            var image = "IA==";
            if (q != null && q.fldFileId != null)
            {
                var f = m.prs_tblUploadFileCompanySelect("fldId", q.fldFileId.ToString(), 1).FirstOrDefault();
                image = Convert.ToBase64String(f.fldFile);
            }
            if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
            {
                var file = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\Content\icon\no-image.jpg");
                image = Convert.ToBase64String(file);
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
        public ActionResult Save(prs_tblAshkhasHoghooghiSelect AshkhasHoghooghi, bool DeleteFileHu)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0;
            byte[] file = null; string Pasvand = ""; string NameFile = "";


            Dictionary<string, object> parameters = new Dictionary<string, object>();

            string jsonstr = "";

            try
            {


                if (AshkhasHoghooghi.fldId == 0)
                {
                    if (Permission.haveAccess(57, "tblAshkhasHoghooghi", null))
                    {
                        //ذخیره
                        if (Session["savePathAshkhasHoghooghi"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathAshkhasHoghooghi"].ToString()));
                            file = stream.ToArray();
                            Pasvand = Path.GetExtension(Session["savePathAshkhasHoghooghi"].ToString());
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            //var Image = Server.MapPath("~/Content/icon/Blank.jpg");
                            //MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                            //file = stream.ToArray();
                            Pasvand = "";
                            file = null;
                            NameFile = "";
                        }

                        parameters.Add("نام", AshkhasHoghooghi.fldName);
                        parameters.Add("شناسه ملی", AshkhasHoghooghi.fldNationalCode);
                        parameters.Add("تاریخ تاسیس", AshkhasHoghooghi.fldTarikhTasis);
                        parameters.Add("پسوند تصویر لوگو", Pasvand);
                        parameters.Add("نام فایل", NameFile);

                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        var q=m.prs_tblAshkhasHoghooghiInsert(AshkhasHoghooghi.fldName, AshkhasHoghooghi.fldNationalCode, file, Pasvand, NameFile, Convert.ToInt32(AshkhasHoghooghi.fldTarikhTasis.Replace("/", "")),
                            Convert.ToInt32(AshkhasHoghooghi.fldDateSabt.Replace("/", "")),AshkhasHoghooghi.fldShomareSabt,AshkhasHoghooghi.fldCodePosti,AshkhasHoghooghi.fldAddress,AshkhasHoghooghi.fldCodeEghtesadi,u.UserInputId, jsonstr).FirstOrDefault();
                        if (q.ErrorCode != 0)
                        {
                            Msg = q.ErrorMessage;
                            MsgTitle = "خطا";
                            Er = 1;
                        }
                        else
                        {
                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "دخیره موفق";
                        }

                        if (Session["savePathAshkhasHoghooghi"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathAshkhasHoghooghi"].ToString());
                            Session.Remove("savePathAshkhasHoghooghi");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
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

                    if (Permission.haveAccess(58, "tblAshkhasHoghooghi", AshkhasHoghooghi.fldId.ToString()))
                    {
                        //ویرایش
                        if (Session["savePathAshkhasHoghooghi"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathAshkhasHoghooghi"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePathAshkhasHoghooghi"].ToString());
                            file = stream.ToArray();
                            NameFile = Session["FileName"].ToString();
                        }
                        else
                        {
                            if (DeleteFileHu == true)
                            {
                                file = null;
                                Pasvand = "";
                                NameFile = "";
                            }
                            else
                            {
                                var pic = m.prs_tblUploadFileCompanySelect("fldId", AshkhasHoghooghi.fldFileId.ToString(), 1).FirstOrDefault();
                                if (pic != null && pic.fldFile != null)
                                {
                                    file = pic.fldFile;
                                    Pasvand = pic.fldPassvand;
                                    NameFile = pic.fldFileName;
                                }
                            }
                        }


                        parameters.Add("نام", AshkhasHoghooghi.fldName);
                        parameters.Add("شناسه ملی", AshkhasHoghooghi.fldNationalCode);
                        parameters.Add("تاریخ تاسیس", AshkhasHoghooghi.fldTarikhTasis);
                        parameters.Add("پسوند تصویر لوگو", Pasvand);
                        parameters.Add("نام فایل", NameFile);

                        var s = m.prs_tblAshkhasHoghooghiUpdate(AshkhasHoghooghi.fldId, AshkhasHoghooghi.fldName, AshkhasHoghooghi.fldNationalCode, AshkhasHoghooghi.fldFileId, Convert.ToInt32(AshkhasHoghooghi.fldTarikhTasis.Replace("/", "")), file, Pasvand, NameFile
                            , Convert.ToInt32(AshkhasHoghooghi.fldDateSabt.Replace("/", "")), AshkhasHoghooghi.fldShomareSabt, AshkhasHoghooghi.fldCodePosti, AshkhasHoghooghi.fldAddress, AshkhasHoghooghi.fldCodeEghtesadi, u.UserInputId, AshkhasHoghooghi.fldTimeStamp).FirstOrDefault();

                      

                        if (Session["savePathAshkhasHoghooghi"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathAshkhasHoghooghi"].ToString());
                            Session.Remove("savePathAshkhasHoghooghi");
                            Session.Remove("FileName");
                            System.IO.File.Delete(physicalPath);
                        }

                        if (s.ErrorCode != 0)
                        {
                            Msg = s.ErrorMessage;
                            MsgTitle = "خطا";
                            Er = 1;
                        }
                        else
                        {
                            if (s.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
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
                                parameters.Add("متن خطا", "AshkhasHoghooghi:Save: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblAshkhasHoghooghi", jsonstr, Logstatus, AshkhasHoghooghi.fldId);

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
                if (Session["savePathAshkhasHoghooghi"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathAshkhasHoghooghi"].ToString());
                    Session.Remove("savePathAshkhasHoghooghi");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                parameters1.Add("نام", AshkhasHoghooghi.fldName);
                parameters1.Add("شناسه ملی", AshkhasHoghooghi.fldNationalCode);
                parameters1.Add("تاریخ تاسیس", AshkhasHoghooghi.fldTarikhTasis);
                parameters1.Add("پسوند تصویر لوگو", Pasvand);
                parameters1.Add("نام فایل", NameFile);

                parameters1.Add("متن خطا", "AshkhasHoghooghi:Save: " + InnerException);
                parameters1.Add("کد خطا", ErrorId.Value);
                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                if (AshkhasHoghooghi.fldId == 0)
                {
                    m.prs_LogInsert(u.UserInputId, "dbo.tblAshkhasHoghooghi", jsonstr, false);
                }
                else
                {
                    m.prs_LogUpdate(u.UserInputId, "dbo.tblAshkhasHoghooghi", jsonstr, false, AshkhasHoghooghi.fldId);
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
                Er = Er,
                Change = Change
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; byte Er = 0; int Change = 0; var checkDel = false;
            Models.RaiSamEntities m = new RaiSamEntities();

            string jsonstr = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var ashkhas = m.prs_tblAshkhasHoghooghiSelect("fldId", Id.ToString(),0).FirstOrDefault();
            parameters.Add("نام", ashkhas.fldName);
            parameters.Add("شناسه ملی", ashkhas.fldNationalCode);
            try
            {//حذف

                if (Permission.haveAccess(59, "tblAshkhasHoghooghi", Id.ToString()))
                {
                    //var con = m.prs_tblContactSelect("CheckAshkhasHoghooghiId", Id.ToString(), 0, 0).FirstOrDefault();
                    //if (con != null)
                    //    checkDel = true;

                    //if (checkDel)
                    //{
                    //    parameters.Add("متن خطا", "AshkhasHoghooghi:Delete: " + "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.");
                    //    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    //    m.prs_LogInsert(u.UserInputId, "dbo.tblAshkhasHoghooghi", jsonstr, false);
                    //    return Json(new
                    //    {
                    //        Msg = "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.",
                    //        MsgTitle = "خطا",
                    //        Er = 1
                    //    }, JsonRequestBehavior.AllowGet);
                    //}
                    var s = m.prs_tblAshkhasHoghooghiDelete(Id, TimeStamp).FirstOrDefault();
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
                        parameters.Add("متن خطا", "AshkhasHoghooghi:Delete: " + Msg);
                        Logstatus = false;
                    }

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogDelete(u.UserInputId, "dbo.tblAshkhas", jsonstr, Logstatus, Id);


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
                parameters.Add("کد خطا", ErrorId);
                parameters.Add("متن خطا", "AshkhasHoghooghi:Delete: " + InnerException);
                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                m.prs_LogDelete(u.UserInputId, "dbo.tblAshkhasHoghooghi", jsonstr, false, Id);
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
                Er = Er,
                Change = Change
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadFromService(int state, string data)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            try
            {
                int? k = null;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                if (state == 1)
                {
                    var q = RaiSam.Areas.Faces.Controllers.CompanyProfileController.GetInquiryByNationalCode(data);
                    if (q != null)
                    {
                        return Json(new
                        {
                            fldDateSabt = q.RegisterDate,
                            fldNationalCode = q.NationalCode,
                            fldFullName = q.Name,
                            fldSh_Sabt = q.RegisterNumber,
                            fldDateTasis = q.EstablishmentDate,
                            fldAddress = q.Address,
                            fldCodePosti = q.PostCode,
                            Err = q.Err
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new
                        {
                            fldNationalCode = k,
                            Err = 0
                        }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var q = RaiSam.Areas.Faces.Controllers.CompanyProfileController.GetInquiryByName(data);
                    if (q != null)
                    {
                        return Json(new
                        {
                            fldDateSabt = q.RegisterDate,
                            fldNationalCode = q.NationalCode,
                            fldFullName = q.Name,
                            fldSh_Sabt = q.RegisterNumber,
                            fldDateTasis = q.EstablishmentDate,
                            fldAddress = q.Address,
                            fldCodePosti = q.PostCode,
                            Err = q.Err
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new
                        {
                            fldNationalCode = k,
                            Err = 0
                        }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    msgErr = "عدم برقراری ارتباط با سرور",
                    Err = 1
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
