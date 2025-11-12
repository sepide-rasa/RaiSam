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

namespace RaiSam.Controllers.CRM
{
    public class ErjaController : Controller
    {
        //
        // GET: /Erja/

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
        public ActionResult HelpErja()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpErja()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId","25",1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinErja()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoWinErja()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "25", 1).FirstOrDefault();
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
                Models.RaiSamEntities m = new RaiSamEntities();
                PartialView.ViewBag.Tarikh = m.prs_GetDate().FirstOrDefault().fldTarikh;
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult DownloadErja(string FileIdErja)
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFileSelect("fldId", FileIdErja,1).FirstOrDefault();
            if (q != null && q.fldFile != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblErjaSelect Erja, bool DeleteFileErja)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0; byte[] File = null; string Passvand = "", FileName = "";
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (Request.IsAjaxRequest() == false)
                {
                    return null;
                }
                else
                {

                    if (Erja.fldId == 0)
                    {//ذخیره
                        if (Permission.haveAccess(72, "tblErja", null))
                        {
                            if (Session["savePathErja"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathErja"].ToString()));
                                File = stream.ToArray();
                                Passvand = Path.GetExtension(Session["savePathErja"].ToString());
                                FileName = Session["FileNameErja"].ToString();
                            }
                            else
                            {
                                Passvand = "";
                                File = null;
                                FileName = "";
                            }

                            Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                            parameters1.Add("ارجاع دهنده", Erja.fldNameErjadahande);
                            parameters1.Add("شرح ارجاع", Erja.fldSharh);
                            parameters1.Add("تاریخ ارجاع", Erja.fldTarikhErja);
                            parameters1.Add("نوع ارجاع", Erja.fldTypeErjaName);
                            string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);

                            m.prs_tblErjaInsert(Erja.fldSharh, Erja.fldErjadahandeId, MiladiDate.GetMiladiDate(Erja.fldTarikhErja + " " + Erja.fldTime), Erja.fldTypeErja, File, Passvand, FileName, user.UserInputId, jsonstr1);

                            if (Session["savePathErja"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathErja"].ToString());
                                Session.Remove("savePathErja");
                                Session.Remove("FileNameErja");
                                System.IO.File.Delete(physicalPath);
                            }
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
                        //ویرایش
                        if (Permission.haveAccess(73, "tblErja", Erja.fldId.ToString()))
                        {
                            if (Session["savePathErja"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathErja"].ToString()));
                                File = stream.ToArray();
                                Passvand = Path.GetExtension(Session["savePathErja"].ToString());
                                FileName = Session["FileNameErja"].ToString();
                            }
                            else
                            {
                                if (DeleteFileErja == true)
                                {
                                    File = null;
                                    Passvand = "";
                                    FileName = "";
                                }
                                else
                                {

                                    var pic = m.prs_tblFileSelect("fldId", Erja.fldFileId.ToString(), 1).FirstOrDefault();
                                    if (pic != null && pic.fldFile != null)
                                    {
                                        File = pic.fldFile;
                                        Passvand = pic.fldPasvand;
                                        FileName = pic.fldFileName;
                                    }
                                }
                            }
                            var q = m.prs_tblErjaUpdate(Erja.fldId, Erja.fldSharh, Erja.fldErjadahandeId, MiladiDate.GetMiladiDate(Erja.fldTarikhErja + " " + Erja.fldTime), Erja.fldTypeErja, Erja.fldFileId, File, Passvand, FileName, Erja.fldTimeStamp, user.UserInputId).FirstOrDefault();

                            if (Session["savePathErja"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathErja"].ToString());
                                Session.Remove("savePathErja");
                                Session.Remove("FileNameErja");
                                System.IO.File.Delete(physicalPath);
                            }


                            if (q.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                            }
                            else if (q.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("ارجاع دهنده", Erja.fldNameErjadahande);
                            parameters.Add("شرح ارجاع", Erja.fldSharh);
                            parameters.Add("تاریخ ارجاع", Erja.fldTarikhErja);
                            parameters.Add("نوع ارجاع", Erja.fldTypeErjaName);


                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Erja:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblErja", jsonstr, false, Erja.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblErja", jsonstr, true, Erja.fldId);
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
                if (Session["savePathErja"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathErja"].ToString());
                    Session.Remove("savePathErja");
                    Session.Remove("FileNameErja");
                    System.IO.File.Delete(physicalPath);
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ارجاع دهنده", Erja.fldNameErjadahande);
                parameters.Add("شرح ارجاع", Erja.fldSharh);
                parameters.Add("تاریخ ارجاع", Erja.fldTarikhErja);
                parameters.Add("نوع ارجاع", Erja.fldTypeErjaName);
                parameters.Add("متن خطا", "Erja:Save: " + InnerException);
                parameters.Add("کد خطا", ErrorId.Value);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (Erja.fldId == 0)
                {
                    m.prs_LogInsert(user.UserInputId, "crm.tblErja", jsonstr, false);
                }
                else
                {

                    m.prs_LogUpdate(user.UserInputId, "crm.tblErja", jsonstr, false, Erja.fldId);
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
        public ActionResult Delete(int id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var Erja = m.prs_tblErjaSelect("fldId", id.ToString(),0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("ارجاع دهنده", Erja.fldNameErjadahande);
                parameters.Add("شرح ارجاع", Erja.fldSharh);
                parameters.Add("تاریخ ارجاع", Erja.fldTarikhErja);
                parameters.Add("نوع ارجاع", Erja.fldTypeErjaName);
                try
                {//حذف

                    if (Permission.haveAccess(74, "tblErja", id.ToString()))
                    {
                        var Na = m.prs_tblNatijeErjaSelect("fldErjaId", id.ToString(), 0).FirstOrDefault();
                        if (Na != null)
                        {
                            Msg = "آیتم مورد نظر در جای دیگر استفاده شده است لذا مجاز به حذف نمی باشید.";
                            MsgTitle = "خطا";
                            Er = 1;

                            parameters.Add("متن خطا", "Erja:Delete: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "crm.tblErja", jsonstr, false, id);

                        }
                        else
                        {
                            var q1 = m.prs_tblErjaDelete(id, TimeStamp).FirstOrDefault();


                            if (q1.flag == 1)
                            {
                                Msg = "حذف با موفقیت انجام شد";
                                MsgTitle = "حذف موفق";
                            }
                            else if (q1.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q1.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }

                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Erja:Delete: " + Msg);
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogDelete(user.UserInputId, "crm.tblErja", jsonstr, false, id);
                            }

                            else
                            {
                                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogDelete(user.UserInputId, "crm.tblErja", jsonstr, true, id);
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

                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Erja:Delete: " + InnerException);

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogDelete(user.UserInputId, "crm.tblErja", jsonstr, false, id);
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblErjaSelect("fldId", Id.ToString(),0).FirstOrDefault();
                string TypeErja = "0";
                if (q.fldTypeErja)
                    TypeErja = "1";
                return Json(new
                {
                    fldId = q.fldId,
                    fldErjadahandeId = q.fldErjadahandeId,
                    fldTimeStamp = q.fldTimeStamp,
                    fldFileId = q.fldFileId,
                    fldNameErjadahande = q.fldNameErjadahande,
                    fldSharh = q.fldSharh,
                    fldTarikhErja = q.fldTarikhErja,
                    fldTypeErja = TypeErja,
                    fldTypeErjaName = q.fldTypeErjaName,
                    fldFileName = q.fldFileName + q.fldPasvand,
                    fldSaatErja = q.fldTime.Replace(":", ""),
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
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                if (Permission.haveAccess(71,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<prs_tblErjaSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblErjaSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldNameErjadahande":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameErjadahande";
                                    break;
                                case "fldTarikhErja":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikhErja";
                                    break;
                                case "fldTypeErjaName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTypeErjaName";
                                    break;
                                case "fldSharh":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldSharh";
                                    break;
                                case "fldCodeEnhesari":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldCodeEnhesari";
                                    break;
                                case "fldCodeMeli":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldCodeMeli";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblErjaSelect(field, searchtext,0).ToList();
                            else
                                data = m.prs_tblErjaSelect(field, searchtext, 0).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblErjaSelect("", "", 0).ToList();
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
                                    return !oValue.ToString().Contains(value.ToString());
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

                    List<prs_tblErjaSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePathErja"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathErja"].ToString());
                Session.Remove("savePathErja");
                Session.Remove("FileNameErja");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            string Msg = "";
            try
            {
                if (Session["savePathErja"] != null)
                {
                    System.IO.File.Delete(Session["savePathErja"].ToString());
                    Session.Remove("savePathErja");
                    Session.Remove("FileNameErja");
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var IsWord = FileInfoExtensions.IsWord(Request.Files[0]);
                var IsPDF = FileInfoExtensions.IsPDF(Request.Files[0]);
                var IsExcel = FileInfoExtensions.IsExcel(Request.Files[0]);
                var IsPowerPoint = FileInfoExtensions.IsPowerPoint(Request.Files[0]);

                if (IsWord == true || IsPDF == true || IsExcel == true || IsPowerPoint == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff"*/)
                {
                    if (Request.Files[0].ContentLength <= 31457280)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathErja"] = savePath;
                        Session["FileNameErja"] = Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
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
                            Message = "حجم فایل انتخابی می بایست کمتر از 10 مگابایت باشد.",
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
                Models.RaiSamEntities m = new RaiSamEntities();
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
        }
        public ActionResult NatijeErja(int ErjaId, string NameErjadahande)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            PartialView.ViewBag.Tarikh = m.prs_GetDate().FirstOrDefault().fldTarikh;
            PartialView.ViewBag.ErjaId = ErjaId;
            PartialView.ViewBag.NameErjadahande = NameErjadahande;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadNatijeErja(StoreRequestParameters parameters, int ErjaId)
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
                if (Permission.haveAccess(75,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<Models.prs_tblNatijeErjaSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblNatijeErjaSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTarikhAnjam":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikhAnjam";
                                    break;
                                case "fldNatijeName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNatijeName";
                                    break;
                                case "fldSharh":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldSharh";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_tblNatijeErjaSelect(field, searchtext,0).Where(l => l.fldErjaId == ErjaId).ToList();
                            else
                                data = m.prs_tblNatijeErjaSelect(field, searchtext, 0).Where(l => l.fldErjaId == ErjaId).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblNatijeErjaSelect("fldErjaId", ErjaId.ToString(),0).ToList();
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
                                    return !oValue.ToString().Contains(value.ToString());
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

                    List<prs_tblNatijeErjaSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult SaveNatijeErja(prs_tblNatijeErjaSelect NatijeErja)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (Request.IsAjaxRequest() == false)
                {
                    return null;
                }
                else
                {
                    if (NatijeErja.fldId == 0)
                    {//ذخیره
                        if (Permission.haveAccess(76, "tblNatijeErja", null))
                        {
                            Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                            parameters1.Add("نتیجه", NatijeErja.fldNatijeName);
                            parameters1.Add("شرح نتیجه", NatijeErja.fldSharh);
                            parameters1.Add("تاریخ انجام", NatijeErja.fldTarikhAnjam);
                            parameters1.Add("ارجاع دهنده", NatijeErja.fldNameErjaDahande);
                            string jsonstr1 = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);


                            m.prs_tblNatijeErjaInsert(NatijeErja.fldErjaId, NatijeErja.fldSharh, MiladiDate.GetMiladiDate(NatijeErja.fldTarikhAnjam + " " + NatijeErja.fldTime), NatijeErja.fldNatije, user.UserInputId, jsonstr1);


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
                        //ویرایش
                        if (Permission.haveAccess(77, "tblNatijeErja", NatijeErja.fldId.ToString()))
                        {
                            var q = m.prs_tblNatijeErjaUpdate(NatijeErja.fldId, NatijeErja.fldErjaId, NatijeErja.fldSharh, MiladiDate.GetMiladiDate(NatijeErja.fldTarikhAnjam + " " + NatijeErja.fldTime), NatijeErja.fldNatije, user.UserInputId, NatijeErja.fldTimeStamp).FirstOrDefault();



                            if (q.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                            }
                            else if (q.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نتیجه", NatijeErja.fldNatijeName);
                            parameters.Add("شرح نتیجه", NatijeErja.fldSharh);
                            parameters.Add("تاریخ انجام", NatijeErja.fldTarikhAnjam);
                            parameters.Add("ارجاع دهنده", NatijeErja.fldNameErjaDahande);


                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Erja:SaveNatijeErja: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblNatijeErja", jsonstr, false, NatijeErja.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "crm.tblNatijeErja", jsonstr, true, NatijeErja.fldId);
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

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نتیجه", NatijeErja.fldNatijeName);
                parameters.Add("شرح نتیجه", NatijeErja.fldSharh);
                parameters.Add("تاریخ انجام", NatijeErja.fldTarikhAnjam);
                parameters.Add("ارجاع دهنده", NatijeErja.fldNameErjaDahande);
                parameters.Add("متن خطا", "Erja:SaveNatijeErja: " + InnerException);
                parameters.Add("کد خطا", ErrorId.Value);
                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                if (NatijeErja.fldId == 0)
                {
                    m.prs_LogInsert(user.UserInputId, "crm.tblNatijeErja", jsonstr, false);
                }
                else
                {
                    m.prs_LogUpdate(user.UserInputId, "crm.tblNatijeErja", jsonstr, false, NatijeErja.fldId);
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
        public ActionResult DeleteNatijeErja(int id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "";
                var NatijeErja = m.prs_tblNatijeErjaSelect("fldId", id.ToString(),0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نتیجه", NatijeErja.fldNatijeName);
                parameters.Add("شرح نتیجه", NatijeErja.fldSharh);
                parameters.Add("تاریخ انجام", NatijeErja.fldTarikhAnjam);
                parameters.Add("ارجاع دهنده", NatijeErja.fldNameErjaDahande);
                try
                {//حذف

                    if (Permission.haveAccess(78, "tblNatijeErja", id.ToString()))
                    {
                        var q = m.prs_tblNatijeErjaDelete(id, TimeStamp).FirstOrDefault();

                        if (q.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (q.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (q.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }

                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "Erja:DeleteNatijeErja: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "crm.tblNatijeErja", jsonstr, false, id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "crm.tblNatijeErja", jsonstr, true, id);
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

                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Erja:DeleteNatijeErja: " + InnerException);

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "crm.tblNatijeErja", jsonstr, false, id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsNatijeErja(int Id)
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
                var q = m.prs_tblNatijeErjaSelect("fldId", Id.ToString(),0).FirstOrDefault();
                string Natije = "0";
                if (q.fldNatije)
                    Natije = "1";
                return Json(new
                {
                    fldId = q.fldId,
                    fldErjaId = q.fldErjaId,
                    fldTimeStamp = q.fldTimeStamp,
                    fldSharh = q.fldSharh,
                    fldTarikhAnjam = q.fldTarikhAnjam,
                    Natije = Natije,
                    fldTime = q.fldTime.Replace(":", ""),
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult DownloadShowVersion(int FileIdMostanad)
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblFileSelect("fldId", FileIdMostanad.ToString(),1).FirstOrDefault();
            if (q != null && q.fldFile != null)
            {
                MemoryStream st = new MemoryStream(q.fldFile);
                return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
            }
            return null;
        }
    }
}
