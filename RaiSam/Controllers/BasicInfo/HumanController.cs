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
using System.Text.RegularExpressions;
using System.Globalization;
using GroupDocs.Signature;
using GroupDocs.Signature.Domain;
using GroupDocs.Signature.Options;
using Aspose.Pdf;
using Aspose.Pdf.DOM;

namespace RaiSam.Controllers.BasicInfo
{
    public class HumanController : Controller
    {
        //
        // GET: /Human/
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
        public ActionResult DetermineLocs(string containerId)
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
        public ActionResult Sign(int Id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            int? SignFileId = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblSign_StampDigitalSelect("fldAshkhasId", Id.ToString(), 0).FirstOrDefault();
            if (q != null)
                SignFileId = q.fldFileSignId;
            PartialView.ViewBag.SignFileId = SignFileId;
            return PartialView;
        }
        public FileContentResult Download(int fldFileId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();

                var q = m.prs_tblFileSelect("fldId", fldFileId.ToString(), 1).FirstOrDefault();
                if (q != null && q.fldFile != null)
                {
                    MemoryStream st = new MemoryStream(q.fldFile);
                    return File(st.ToArray(), MimeType.Get(q.fldPasvand), "DownloadFile" + q.fldPasvand);
                }
                return null;
            }
        }
        public ActionResult DeleteSessionFileEmza()
        {
            string Msg = "";

            if (Session["savePathEmza"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathEmza"].ToString());
                Session.Remove("savePathEmza");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UploadEmza()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = ""; string MsgTitle = ""; int Er = 0;
            try
            {
                if (Session["savePathEmza"] != null)
                {
                    System.IO.File.Delete(Session["savePathEmza"].ToString());
                    Session.Remove("FileName");
                    Session.Remove("savePathEmza");
                }

                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (IsImage == true)
                {
                    if (Request.Files[0].ContentLength <= 10485760)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathEmza"] = savePath;
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
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                MsgTitle = "خطا";
                Er = 1;
                return Json(new
                {
                    MsgTitle = MsgTitle,
                    Msg = Msg,
                    Er = 1
                });
            }
        }
        public ActionResult SaveEmza(int ShakhsId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; var Er = 0; byte[] Image = null; string Pasvand = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            try
            {


                //ذخیره
                if (Session["savePathEmza"] != null)
                {
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathEmza"].ToString()));
                    Image = stream.ToArray();
                    Pasvand = Path.GetExtension(Session["savePathEmza"].ToString());
                }
                else
                {
                    return Json(new
                    {
                        Msg = "لطفا فایل امضا را بارگذاری نمایید.",
                        MsgTitle = "خطا",
                        Err = 1
                    }, JsonRequestBehavior.AllowGet);
                }

                MsgTitle = "ذخیره موفق";
                Msg ="ذخیره با موفقیت انجام شد." ;
                m.prs_tblSign_StampDigitalInsert(null,null,null,Session["FileName"].ToString(),Pasvand,Image, ShakhsId,  null, user.UserInputId);


               
                if (Session["savePathEmza"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathEmza"].ToString());
                    Session.Remove("savePathEmza");
                    System.IO.File.Delete(physicalPath);
                }
            }
            catch (Exception x)
            {
                if (x.InnerException != null)
                    Msg = x.InnerException.Message;
                else
                    Msg = x.Message;

                if (Session["savePathEmza"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathEmza"].ToString());
                    Session.Remove("savePathEmza");
                    System.IO.File.Delete(physicalPath);
                }

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
        public ActionResult Help()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpHuman()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "5", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinHuman()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoHuman()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "5", 1).FirstOrDefault();
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
            if (Permission.haveAccess(17, "", "0"))
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                UserInfo user = (UserInfo)(Session["User"]);
                List<Models.prs_tblAshkhasSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {
                    string field = "";
                    string searchtext = "";
                    List<Models.prs_tblAshkhasSelect> data1 = null;
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
                            case "fldFamily":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldFamily";
                                break;
                            case "fldFatherName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldFatherName";
                                break;
                            case "fldCodeMeli":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldCodeMeli";
                                break;
                            case "fldEmail":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldEmail";
                                break;
                            case "fldMobile":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldMobile";
                                break;
                        }
                        if (data != null)
                            data1 = m.prs_tblAshkhasSelect(field, searchtext, "", "", "", 100).ToList();
                        else
                            data = m.prs_tblAshkhasSelect(field, searchtext, "", "", "", 100).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);
                }
                else
                {
                    data = m.prs_tblAshkhasSelect("", "", "", "", "", 100).ToList();
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

                List<Models.prs_tblAshkhasSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { Code = "403" });
            }
        }
        //public ActionResult GetNahi()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    param.FieldName = "fldUserId";
        //    param.Value = "";
        //    param.UserId = user.UserId;
        //    param.Top = 0;
        //    var q = service.SelectNahi(param, user.UserKey, IP).NahiList.Select(c => new { fldId = c.fldCode, fldName = c.fldNameNahi });
        //    return this.Store(q);
        //}

        public ActionResult New(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.Id = Id;
            return result;
        }
        public ActionResult NewCity(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.Id = Id;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCity(Models.prs_tblCitySelect City)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            string Msg = "", MsgTitle = ""; var Er = 0; bool CheckRepeatedRow = false;
            try
            {
                //ذخیره
                if (Permission.haveAccess(54, "tblCity", null))
                {
                    CheckRepeatedRow = m.prs_tblCitySelect("fldName", City.fldName, 0).Any();
                    if (CheckRepeatedRow)
                    {
                        return Json(new
                        {
                            Msg = "شهر وارد شده تکراری است.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        m.prs_tblCityInsert(City.fldCode,City.fldName, user.UserInputId, IP);

                        Msg = "ذخیره با موفقیت انجام شد.";
                        MsgTitle = "دخیره موفق";
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
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePathHuman"] != null)
                {
                    System.IO.File.Delete(Session["savePathHuman"].ToString());
                    Session.Remove("FileName");
                    Session.Remove("savePathHuman");
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
                        Session["savePathHuman"] = savePath;
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
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
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
            if (Session["savePathHuman"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathHuman"].ToString());
                Session.Remove("savePathHuman");
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
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathHuman"].ToString()));
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
            UserInfo user = (UserInfo)(Session["User"]);

            var q = m.prs_tblAshkhasSelect("fldId", Id.ToString(), "", "", "", 1).FirstOrDefault();
            byte[] file = null; string FileName = "";
            if (q.fldFileId != null)
            {
                var f = m.prs_tblFileSelect("fldId", q.fldFileId.ToString(), 1).FirstOrDefault();
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
                fldFamily = q.fldFamily,
                fldCodeMeli = q.fldCodeMeli,
                fldFatherName = q.fldFatherName,
                fldMobile = q.fldMobile,
                fldEmail = q.fldEmail,
                fldFileId = q.fldFileId,
                NameFile = FileName,
                fldTarikhTavalod = q.fldTarikhTavalod,
                // fldEstelam = q.fldEstelam,
                pic = pic,
                fldSh_Shenasname = q.fldSh_Shenasname,
                fldMahaleSodoor = q.fldMahaleSodoor,
                fldMahalTavalod = q.fldMahalTavalod,
                fldCodeMahalSodoor = q.fldCodeMahalSodoor,
                fldCodeMahalTavalod = q.fldCodeMahalTavalod,
                fldJensiyat = Convert.ToByte(q.fldJensiyat).ToString(),
                fldDesc = q.fldDesc,
                fldTimeStamp = q.fldTimeStamp
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Estelam(string BirthDate, string NationalCode)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);

        //    //RaiClient RaiClient = new RaiClient("http://172.23.30.211:5001/");
        //    RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir");
        //    User AuthorizeUser = new User();
        //    AuthorizeUser.UserName = "TebeKar_Sitaad";
        //    AuthorizeUser.Password = "TebeKar_Sitaad.IT.13990912.583.Op5#d%56Mo09LaW7Vv";

        //    InputDto_SabtAhval InputDto = new InputDto_SabtAhval();
        //    InputDto.birthDate = BirthDate.Replace(@"/", string.Empty);
        //    InputDto.nationalCode = NationalCode;
        //    InputDto.family = "";
        //    var r = RaiClient.GetInfoAndImage(AuthorizeUser, InputDto, new CancellationToken());
        //    if (r.Status == System.Threading.Tasks.TaskStatus.Faulted)
        //    {
        //        return Json(new
        //        {
        //            Message = "خطا در برقراری ارتباط با وب سرویس",
        //            Er = 1
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        if (r.Result.IsSuccess)
        //        {
        //            if (r.Result.Data.exceptionMessage != null)
        //            {
        //                if (r.Result.Data.exceptionMessage == "err.record.not.found")
        //                {
        //                    return Json(new
        //                    {
        //                        Message = "کد ملی و یا تاریخ تولد اشتباه است.",
        //                        Er = 3
        //                    });
        //                }
        //                else
        //                {
        //                    var error = service.Error(r.Result.Data.exceptionMessage, user.UserKey, IP);
        //                    return Json(new
        //                    {
        //                        Message = "خطا در زمان استعلام. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید. در غیر این صورت اطلاعات را به صورت دستی وارد نمایید.",
        //                        Er = 1
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                var Pic = "";
        //                byte[] picture = null;
        //                foreach (var item1 in r.Result.Data.payload.images)
        //                {
        //                    if (item1.image != null)
        //                    {
        //                        Pic = Convert.ToBase64String(item1.image);
        //                    }
        //                }

        //                var shenasname = "";
        //                if (r.Result.Data.payload.shenasnameNo == "0")
        //                {
        //                    shenasname = r.Result.Data.payload.nationalCode;
        //                }
        //                else
        //                {
        //                    shenasname = r.Result.Data.payload.shenasnameNo;
        //                }

        //                var Gender = 0;
        //                if (r.Result.Data.payload.gender == "1")
        //                {
        //                    Gender = 1;
        //                }
        //                else
        //                {
        //                    Gender = 0;
        //                }

        //                int mahalSodurId = 0;
        //                string mahalSodur = "";
        //                int mahalTavalodId = 0;
        //                string mahalTavalod = "";
        //                OBJ_City p = new OBJ_City();
        //                p.fldInputId = user.UserInputId;
        //                p.fldName = r.Result.Data.payload.shenasnameIssuePlace;
        //                p.fldCode = 0;
        //                var m = service.InsertCity(p, user.UserKey, IP);
        //                if (m.ErrorType == false)
        //                {
        //                    mahalSodur = r.Result.Data.payload.shenasnameIssuePlace;
        //                    mahalSodurId = m.OutputId;
        //                }

        //                p.fldName = r.Result.Data.payload.birthplace;
        //                m = service.InsertCity(p, user.UserKey, IP);
        //                if (m.ErrorType == false)
        //                {
        //                    mahalTavalod = r.Result.Data.payload.birthplace;
        //                    mahalTavalodId = m.OutputId;
        //                }

        //                return Json(new
        //                {
        //                    ss = r.Result.Data,
        //                    fldName = r.Result.Data.payload.name,
        //                    fldFamily = r.Result.Data.payload.family,
        //                    fldFatherName = r.Result.Data.payload.fatherName,
        //                    shenasnameNo = shenasname,
        //                    Gender = Gender.ToString(),
        //                    mahalSodurId = mahalSodurId,
        //                    mahalSodur = mahalSodur,
        //                    mahalTavalodId = mahalTavalodId,
        //                    mahalTavalod = mahalTavalod,
        //                    Pic = Pic,
        //                    Message = "",
        //                    Er = 0
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                Message = "خطایی با شماره " + r.Result.StatusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید. در غیر اینصورت اطلاعات را به صورت دستی وارد نمایید.",
        //                Er = 1
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

        //public ActionResult UpdateBySabt(int Id)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    string BirthDate, NationalCode;
        //    param.FieldName = "fldId";
        //    param.Value = Id.ToString();
        //    var per = service.SelectAshkhas(param, user.UserKey, IP).AshkhasList.FirstOrDefault();
        //    BirthDate = per.fldTarikhTavalod;
        //    NationalCode = per.fldCodeMeli;
        //    //RaiClient RaiClient = new RaiClient("http://172.23.30.211:5001/");
        //    RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir");
        //    User AuthorizeUser = new User();
        //    AuthorizeUser.UserName = "TebeKar_Sitaad";
        //    AuthorizeUser.Password = "TebeKar_Sitaad.IT.13990912.583.Op5#d%56Mo09LaW7Vv";

        //    InputDto_SabtAhval InputDto = new InputDto_SabtAhval();
        //    InputDto.birthDate = BirthDate.Replace(@"/", string.Empty);
        //    InputDto.nationalCode = NationalCode;
        //    InputDto.family = "";
        //    var r = RaiClient.GetInfoAndImage(AuthorizeUser, InputDto, new CancellationToken());
        //    if (r.Status == System.Threading.Tasks.TaskStatus.Faulted)
        //    {
        //        return Json(new
        //        {
        //            Msg = "خطا در برقراری ارتباط با وب سرویس",
        //            Er = 1,
        //            MsgTitle = "خطا",
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        if (r.Result.IsSuccess)
        //        {
        //            if (r.Result.Data.exceptionMessage != null)
        //            {
        //                if (r.Result.Data.exceptionMessage == "err.record.not.found")
        //                {
        //                    return Json(new
        //                    {
        //                        Msg = "کد ملی و یا تاریخ تولد اشتباه است.",
        //                        Er = 1,
        //                        MsgTitle = "خطا",
        //                    });
        //                }
        //                else
        //                {
        //                    var error = service.Error(r.Result.Data.exceptionMessage, user.UserKey, IP);
        //                    return Json(new
        //                    {
        //                        Msg = "خطا در زمان استعلام. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید. در غیر این صورت اطلاعات را به صورت دستی وارد نمایید.",
        //                        Er = 1,
        //                        MsgTitle = "خطا",
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                byte[] picture = null;
        //                foreach (var item1 in r.Result.Data.payload.images)
        //                {
        //                    if (item1.image != null)
        //                    {
        //                        picture = item1.image;
        //                    }
        //                }

        //                var shenasname = "";
        //                if (r.Result.Data.payload.shenasnameNo == "0")
        //                {
        //                    shenasname = r.Result.Data.payload.nationalCode;
        //                }
        //                else
        //                {
        //                    shenasname = r.Result.Data.payload.shenasnameNo;
        //                }

        //                var Gender = false;
        //                if (r.Result.Data.payload.gender == "1")
        //                {
        //                    Gender = true;
        //                }
        //                else
        //                {
        //                    Gender = false;
        //                }

        //                int mahalSodurId = 0;
        //                string mahalSodur = "";
        //                int mahalTavalodId = 0;
        //                string mahalTavalod = "";
        //                OBJ_City p = new OBJ_City();
        //                p.fldInputId = user.UserInputId;
        //                p.fldName = r.Result.Data.payload.shenasnameIssuePlace;
        //                p.fldCode = 0;
        //                var m = service.InsertCity(p, user.UserKey, IP);
        //                if (m.ErrorType == false)
        //                {
        //                    mahalSodur = r.Result.Data.payload.shenasnameIssuePlace;
        //                    mahalSodurId = m.OutputId;
        //                }

        //                p.fldName = r.Result.Data.payload.birthplace;
        //                m = service.InsertCity(p, user.UserKey, IP);
        //                if (m.ErrorType == false)
        //                {
        //                    mahalTavalod = r.Result.Data.payload.birthplace;
        //                    mahalTavalodId = m.OutputId;
        //                }
        //                OBJ_Ashkhas Ashkhas = new OBJ_Ashkhas();
        //                Ashkhas.fldCodeMeli = NationalCode;
        //                Ashkhas.fldName = r.Result.Data.payload.name;
        //                Ashkhas.fldFamily = r.Result.Data.payload.family;
        //                Ashkhas.fldFatherName = r.Result.Data.payload.fatherName;
        //                Ashkhas.fldSh_Shenasname = shenasname;
        //                Ashkhas.fldJensiyat = Gender;
        //                Ashkhas.fldFile = picture;
        //                Ashkhas.fldPasvand = ".png";
        //                Ashkhas.fldFileName = "Personpic";

        //                var s = service.UpdateAshkhasInfoFromSabt(Ashkhas, user.UserKey, IP);
        //            }
        //        }
        //    }
        //    return Json(new
        //    {
        //        Er = 0,
        //        Msg = "واکشی اطلاعات با موفقیت انجام شد.",
        //        MsgTitle = "عملیات موفق",
        //    }, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getAx(int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_tblAshkhasSelect("fldId", fldId.ToString(), "", "", "", 1).FirstOrDefault();
            var image = "IA==";
            if (q != null && q.fldFileId != null)
            {
                var f = m.prs_tblFileSelect("fldId", q.fldFileId.ToString(), 1).FirstOrDefault();
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
        public static byte[] Base64ToImage(string Base64String)
        {
            try
            {
                MemoryStream stream;

                stream = new MemoryStream(Convert.FromBase64String(Base64String));
                return stream.ToArray();

            }
            catch (Exception x)
            {
                return null;
            }
        }
        bool invalid = false;
        public bool checkEmail(string Email)
        {

            if (String.IsNullOrEmpty(Email))
                invalid = false;

            else
            {
                Email = Regex.Replace(Email, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);

                invalid = Regex.IsMatch(Email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            }
            return invalid;
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblAshkhasSelect Human, bool DeleteFileHu, string PersonPic)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            string Msg = "", MsgTitle = ""; var Er = 0; var Change = 0;
            byte[] file = null; string Pasvand = ""; string NameFile = ""; bool CheckRepeatedRow = false;
            //string MahalSodor = ""; string MahalTavalod = "";


            file = Base64ToImage(PersonPic);
            Pasvand = ".png";
            NameFile = "Personpic";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            string jsonstr = "";

            try
            {
                //if (Human.fldCodeMahalSodoor != null)
                //{
                //    MahalSodor = m.prs_tblCitySelect("fldId", Human.fldCodeMahalSodoor.ToString(),0).FirstOrDefault().fldName;
                //}
                //if (Human.fldCodeMahalTavalod != null)
                //{
                //    MahalTavalod = m.prs_tblCitySelect("fldId", Human.fldCodeMahalTavalod.ToString(),0).FirstOrDefault().fldName;
                //}

                if (Human.fldDesc == null)
                    Human.fldDesc = "";

                int? birthDay = null;
                if (Human.fldTarikhTavalod != null)
                {
                    birthDay = Convert.ToInt32(Human.fldTarikhTavalod.Replace("/", ""));
                }

                if (birthDay > 13671230)
                    Human.fldSh_Shenasname = Human.fldCodeMeli;

                if (Human.fldEmail != "" && Human.fldEmail != null)
                {
                    var checkE = true;
                    checkE = checkEmail(Human.fldEmail);
                    if (checkE == false)
                    {
                        parameters.Add("متن خطا", "Human:Save: " + "ایمیل وارد شده نامعتبر است.");
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                        m.prs_LogInsert(u.UserInputId, "dbo.tblAshkhas", jsonstr, false);
                        return Json(new
                        {
                            Msg = "ایمیل وارد شده نامعتبر است.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (Human.fldId == 0)
                {
                    if (Permission.haveAccess(18, "dbo.tblAshkhas", ""))
                    {
                        //ذخیره
                        if (Session["savePathHuman"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathHuman"].ToString()));
                            file = stream.ToArray();
                            Pasvand = Path.GetExtension(Session["savePathHuman"].ToString());
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

                        parameters.Add("نام", Human.fldName);
                        parameters.Add("نام خانوادگی", Human.fldFamily);
                        parameters.Add("نام پدر", Human.fldFatherName);
                        parameters.Add("کدملی", Human.fldCodeMeli);
                        parameters.Add("ایمیل", Human.fldEmail);
                        parameters.Add("موبایل", Human.fldMobile);
                        parameters.Add("شماره شناسنامه", Human.fldSh_Shenasname);
                        parameters.Add("محل تولد", Human.fldMahalTavalod);
                        parameters.Add("محل صدور", Human.fldMahaleSodoor);
                        parameters.Add("جنسیت", Human.fldJensiyatName);
                        parameters.Add("تاریخ تولد", Human.fldTarikhTavalod);
                        parameters.Add("پسوند تصویر شخص", Pasvand);
                        parameters.Add("نام فایل", NameFile);
                        parameters.Add("توضیحات", Human.fldDesc);


                        CheckRepeatedRow = m.prs_tblAshkhasSelect("fldCodeMeli", Human.fldCodeMeli, "", "", "", 0).Any();
                        if (CheckRepeatedRow)
                        {
                            parameters.Add("متن خطا", "Human:Save: " + "کدملی وارد شده قبلا در سیستم ثبت شده است.");
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_LogInsert(u.UserInputId, "dbo.tblAshkhas", jsonstr, false);
                            return Json(new
                            {
                                Msg = "کدملی وارد شده قبلا در سیستم ثبت شده است.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblAshkhasInsert(Human.fldName, Human.fldFamily, Human.fldFatherName, Human.fldCodeMeli, Human.fldEmail, Human.fldMobile, file, Pasvand, NameFile, "",
                                Human.fldSh_Shenasname, Human.fldCodeMahalTavalod, Human.fldCodeMahalSodoor, Human.fldJensiyat, birthDay, u.UserInputId, jsonstr);
                        }
                        if (Session["savePathHuman"] != null)
                        {
                            string physicalPath = System.IO.Path.Combine(Session["savePathHuman"].ToString());
                            Session.Remove("savePathHuman");
                            Session.Remove("FileName");
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


                    if (Permission.haveAccess(18, "dbo.tblAshkhas", Human.fldId.ToString()))
                    {

                        if (Session["savePathHuman"] != null)
                        {
                            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathHuman"].ToString()));
                            Pasvand = Path.GetExtension(Session["savePathHuman"].ToString());
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

                                var pic = m.prs_tblFileSelect("fldId", Human.fldFileId.ToString(), 1).FirstOrDefault();
                                if (pic != null && pic.fldFile != null)
                                {
                                    file = pic.fldFile;
                                    Pasvand = pic.fldPasvand;
                                    NameFile = pic.fldFileName;
                                }

                            }
                        }


                        parameters.Add("نام", Human.fldName);
                        parameters.Add("نام خانوادگی", Human.fldFamily);
                        parameters.Add("نام پدر", Human.fldFatherName);
                        parameters.Add("کدملی", Human.fldCodeMeli);
                        parameters.Add("ایمیل", Human.fldEmail);
                        parameters.Add("موبایل", Human.fldMobile);
                        parameters.Add("شماره شناسنامه", Human.fldSh_Shenasname);
                        parameters.Add("محل تولد", Human.fldMahalTavalod);
                        parameters.Add("محل صدور", Human.fldMahaleSodoor);
                        parameters.Add("جنسیت", Human.fldJensiyatName);
                        parameters.Add("تاریخ تولد", Human.fldTarikhTavalod);
                        parameters.Add("پسوند تصویر شخص", Pasvand);
                        parameters.Add("نام فایل", NameFile);
                        parameters.Add("توضیحات", Human.fldDesc);


                        var UserG = m.prs_tblAshkhasSelect("fldCodeMeli", Human.fldCodeMeli, "", "", "", 0).FirstOrDefault();
                        if (UserG != null && UserG.fldId != Human.fldId)
                            CheckRepeatedRow = true;

                        if (CheckRepeatedRow)
                        {
                            parameters.Add("متن خطا", "Human:Save: " + "کدملی وارد شده قبلا در سیستم ثبت شده است.");
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_LogUpdate(u.UserInputId, "dbo.tblAshkhas", jsonstr, false, Human.fldId);
                            return Json(new
                            {
                                Msg = "کدملی وارد شده قبلا در سیستم ثبت شده است.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var s = m.prs_tblAshkhasUpdate(Human.fldId, Human.fldName, Human.fldFamily, Human.fldFatherName, Human.fldCodeMeli, Human.fldEmail, Human.fldMobile,
                                Human.fldFileId, file, Pasvand, NameFile, "", Human.fldSh_Shenasname, Human.fldCodeMahalTavalod, Human.fldCodeMahalSodoor, Human.fldJensiyat, birthDay, u.UserInputId, Human.fldTimeStamp).FirstOrDefault();

                            if (Session["savePathHuman"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathHuman"].ToString());
                                Session.Remove("savePathHuman");
                                Session.Remove("FileName");
                                System.IO.File.Delete(physicalPath);
                            }

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
                                parameters.Add("متن خطا", "Human:Save: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(user.UserInputId, "dbo.tblAshkhas", jsonstr, Logstatus, Human.fldId);
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
                var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                if (Session["savePathHuman"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathHuman"].ToString());
                    Session.Remove("savePathHuman");
                    Session.Remove("FileName");
                    System.IO.File.Delete(physicalPath);
                }
                Dictionary<string, object> parameters1 = new Dictionary<string, object>();
                parameters1.Add("نام", Human.fldName);
                parameters1.Add("نام خانوادگی", Human.fldFamily);
                parameters1.Add("نام پدر", Human.fldFatherName);
                parameters1.Add("کدملی", Human.fldCodeMeli);
                parameters1.Add("ایمیل", Human.fldEmail);
                parameters1.Add("موبایل", Human.fldMobile);
                parameters1.Add("شماره شناسنامه", Human.fldSh_Shenasname);
                parameters1.Add("محل تولد", Human.fldMahalTavalod);
                parameters1.Add("محل صدور", Human.fldMahaleSodoor);
                parameters1.Add("جنسیت", Human.fldJensiyatName);
                parameters1.Add("تاریخ تولد", Human.fldTarikhTavalod);
                parameters1.Add("پسوند تصویر شخص", Pasvand);
                parameters1.Add("نام فایل", NameFile);
                parameters1.Add("توضیحات", Human.fldDesc);
                if (Human.fldId == 0)
                {
                    parameters.Add("متن خطا", "Human:Save: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogInsert(user.UserInputId, "dbo.tblAshkhas", jsonstr, false);
                }
                else
                {
                    parameters.Add("متن خطا", "Human:Edit: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters1, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogUpdate(user.UserInputId, "dbo.tblAshkhas", jsonstr, false, Human.fldId);
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
            string jsonstr = "", Name = "", CodeMeli = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var ashkhas = m.prs_tblAshkhasSelect("fldId", Id.ToString(), "", "", "", 0).FirstOrDefault();
            Name = ashkhas.fldName + " " + ashkhas.fldFamily;
            CodeMeli = ashkhas.fldCodeMeli;
            parameters.Add("نام و نام خانوادگی", Name);
            parameters.Add("کد ملی", CodeMeli);
            try
            {//حذف

                if (Permission.haveAccess(20, "tblUserGroup", Id.ToString()))
                {
                    var user = m.prs_tblUserSelect("fldShakhsId", Id.ToString(), "", 0).FirstOrDefault();
                    var personal = m.prs_tblPersonalInfoSelect("fldshakhsId", Id.ToString(), "", "", 0, 0).FirstOrDefault();
                    var con = m.prs_tblContactSelect("CheckAshkhasId", Id.ToString(), 0, 0).FirstOrDefault();
                    var Erja = m.prs_tblErjaSelect("fldErjadahandeId", Id.ToString(), 0).FirstOrDefault();
                    if (user != null || personal != null || con != null || Erja!=null)
                        checkDel = true;

                    if (checkDel)
                    {
                        parameters.Add("متن خطا", "Human:Delete: " + "شخص انتخاب شده قبلا در سیستم استفاده شده است.");
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                        m.prs_LogInsert(u.UserInputId, "dbo.tblAshkhas", jsonstr, false);
                        return Json(new
                        {
                            Msg = "شخص انتخاب شده قبلا در سیستم استفاده شده است.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }

                var s = m.prs_tblAshkhasDelete(Id, TimeStamp).FirstOrDefault();
                
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
                        parameters.Add("متن خطا", "Human:Delete: " + Msg);
                        Logstatus = false;
                    }

                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    m.prs_LogDelete(u.UserInputId,"dbo.tblAshkhas",jsonstr,Logstatus,Id);
                    
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
                parameters.Add("متن خطا", "Human:Delete: " + InnerException);
                jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);


                m.prs_LogDelete(u.UserInputId, "dbo.tblAshkhas", jsonstr, false, Id);
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
        public ActionResult SignTEST(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; var Er = 0; byte[] Image = null; string Pasvand = "";
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            try
            {

                
                var sample = Server.MapPath("~/Content/sample.pdf");
                var signa = Server.MapPath("~/Content/signature.png");
                var Dessample = Server.MapPath("~/Content/AddImage_out.pdf");



                // Open document
                Document pdfDocument = new Document(sample);

                // Set coordinates
                int pageH = 900;
                    // Load image into stream
                    FileStream imageStream = new FileStream(signa, FileMode.Open);

                    System.Drawing.Image img = System.Drawing.Image.FromStream(imageStream);
                    int height = img.Height;
                    int width = img.Width;


                int lowerLeftX = 100;
                int lowerLeftY = pageH - height-100;
                int upperRightX = 100+width;
                int upperRightY = pageH-100;

                // Get the page where image needs to be added
                for (int i=1;i<=pdfDocument.Pages.Count;i++)
                {
                    Page page = pdfDocument.Pages[i];
                    // Add image to Images collection of Page Resources
                    page.Resources.Images.Add(imageStream);
                    // Using GSave operator: this operator saves current graphics state
                    page.Contents.Add(new Aspose.Pdf.Operator.GSave());
                    // Create Rectangle and Matrix objects
                    Aspose.Pdf.Rectangle rectangle = new Aspose.Pdf.Rectangle(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
                    Matrix matrix = new Matrix(new double[] { rectangle.URX - rectangle.LLX, 0, 0, rectangle.URY - rectangle.LLY, rectangle.LLX, rectangle.LLY });
                    // Using ConcatenateMatrix (concatenate matrix) operator: defines how image must be placed
                    page.Contents.Add(new Aspose.Pdf.Operator.ConcatenateMatrix(matrix));
                    XImage ximage = page.Resources.Images[page.Resources.Images.Count];
                    // Using Do operator: this operator draws image
                    page.Contents.Add(new Aspose.Pdf.Operator.Do(ximage.Name));
                    // Using GRestore operator: this operator restores graphics state
                    page.Contents.Add(new Aspose.Pdf.Operator.GRestore());
                }
                    // dataDir = dataDir + "AddImage_out.pdf";
                    // Save updated document
                    pdfDocument.Save(Dessample); 
                

                //Page page = pdfDocument.Pages.Add();
                //page.SetPageSize(PageSize.A4.Height, PageSize.A4.Width);
                //page = pdfDocument.Pages.Add();
                //Aspose.Pdf.Facades.PdfFileMend mender = new Aspose.Pdf.Facades.PdfFileMend(pdfDocument);
                //mender.AddImage(signa, 1, 0, 0, (float)page.CropBox.Width, (float)page.CropBox.Height);
                //pdfDocument.Save(Dessample);
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
        //public ActionResult UpdateFilePersonal()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    param.FieldName = "fldUpdateFile";
        //    param.Value = "";
        //    param.Value2 = "";
        //    param.Top = 0;
        //    var per = service.SelectAshkhas(param, user.UserKey, IP).AshkhasList.ToList();

        //    RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir");
        //    User AuthorizeUser = new User();
        //    AuthorizeUser.UserName = "TebeKar_Sitaad";
        //    AuthorizeUser.Password = "TebeKar_Sitaad.IT.13990912.583.Op5#d%56Mo09LaW7Vv";

        //    InputDto_SabtAhval InputDto = new InputDto_SabtAhval();

        //    foreach (var item in per)
        //    {
        //        OBJ_PersonalInfo newpic = new OBJ_PersonalInfo();

        //        InputDto.birthDate = item.fldTarikhTavalod.Replace(@"/", string.Empty);
        //        InputDto.nationalCode = item.fldCodeMeli;
        //        InputDto.family = "";
        //        var r = RaiClient.GetInfoAndImage(AuthorizeUser, InputDto, new CancellationToken());

        //        Dictionary<string, object> parameters = new Dictionary<string, object>();

        //        parameters.Add("نام", item.fldName);
        //        parameters.Add("نام خانوادگی", item.fldFamily);
        //        parameters.Add("نام پدر", item.fldFatherName);
        //        parameters.Add("کدملی", item.fldCodeMeli);

        //        byte[] picture = null;
        //        foreach (var item1 in r.Result.Data.payload.images)
        //        {
        //            if (item1.image != null)
        //            {
        //                picture = item1.image;
        //            }
        //        }
        //        newpic.File = picture;
        //        newpic.fldShakhsId = item.fldId;
        //        newpic.Pasvand = ".png";
        //        newpic.FileName = "Personpic";

        //        var m = service.EditFilePersonal(newpic, user.UserKey, IP);
        //    }
        //    return Json(new
        //    {
        //        Er = 0,
        //        Msg = "تمام",
        //        MsgTitle = "موفق",
        //    }, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UpdateFileOnePersonal(int Id)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    param.FieldName = "fldId";
        //    param.Value = Id.ToString();
        //    param.Value2 = "";
        //    param.Top = 0;
        //    var per = service.SelectAshkhas(param, user.UserKey, IP).AshkhasList.FirstOrDefault();
        //    OBJ_Ashkhas sh = new OBJ_Ashkhas();
        //    RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir");
        //    User AuthorizeUser = new User();
        //    AuthorizeUser.UserName = "TebeKar_Sitaad";
        //    AuthorizeUser.Password = "TebeKar_Sitaad.IT.13990912.583.Op5#d%56Mo09LaW7Vv";

        //    InputDto_SabtAhval InputDto = new InputDto_SabtAhval();

        //    InputDto.birthDate = per.fldTarikhTavalod.Replace(@"/", string.Empty);
        //    InputDto.nationalCode = per.fldCodeMeli;
        //    var r = RaiClient.GetInfoAndImage(AuthorizeUser, InputDto, new CancellationToken());

        //    byte[] picture = null;
        //    foreach (var item1 in r.Result.Data.payload.images)
        //    {
        //        if (item1.image != null)
        //        {
        //            picture = item1.image;
        //        }

        //        sh.fldFile = picture;
        //        sh.fldId = per.fldId;
        //        sh.fldPasvand = ".png";
        //        sh.fldFileName = "Personpic";
        //        sh.FieldName = "SabtAhval";

        //        var m = service.EditFileAshkhas(sh, user.UserKey, IP);
        //    }
        //    return Json(new
        //    {
        //        Er = 0,
        //        Msg = "آپلود تصویر با موفقیت انجام شد.",
        //        MsgTitle = "عملیات موفق",
        //    }, JsonRequestBehavior.AllowGet);
        //}

    }
}
