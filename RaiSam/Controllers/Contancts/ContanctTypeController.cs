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

namespace RaiSam.Controllers.Contancts
{
    public class ContanctTypeController : Controller
    {
        //
        // GET: /ContanctType/

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
        public ActionResult FormulSaz(/*int PayeId, int HeaderId, int FormulId,*/ int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            //ViewData.Model = new Models.Parameters();
            //PartialView.ViewData = ViewData;
            //PartialView.ViewBag.PayeId = PayeId.ToString();
            //PartialView.ViewBag.HeaderId = HeaderId.ToString();
            //PartialView.ViewBag.FormulId = FormulId.ToString();
            PartialView.ViewBag.fldId = fldId.ToString();
            return PartialView;
        }
        public FileContentResult ShowHelpContactType()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "24",1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(),1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinContactType()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoContactType()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "24", 1).FirstOrDefault();
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
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
        public ActionResult Upload()
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
                        Session["savePath"] = savePath;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowPicUpload(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                byte[] file = null;
                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                file = stream.ToArray();
                var image = Convert.ToBase64String(file);
                return Json(new { image = image });
            }
        }
        public ActionResult DeleteSessionFile()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "";
            if (Session["savePath"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                Session.Remove("savePath");
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
        public ActionResult Save(Models.prs_tblContanctTypeSelect ContanctType, bool DeleteFile)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; byte[] Icon = null; string pasavand = ""; string fileName = ""; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    if (ContanctType.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(105, "tblContanctType", null))
                        {
                            if (Session["savePath"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                                pasavand = Path.GetExtension(Session["savePath"].ToString());
                                Icon = stream.ToArray();
                                fileName = Session["FileName"].ToString();
                            }
                            else
                            {
                                return Json(new
                                {
                                    Msg = "فایل مورد نظر را انتخاب نمایید.",
                                    MsgTitle = "خطا",
                                    Er = 1
                                }, JsonRequestBehavior.AllowGet);
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نوع تماس", ContanctType.fldType);
                            parameters.Add("نام فایل", fileName);
                            parameters.Add("پسوند", pasavand);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblContanctTypeInsert(ContanctType.fldType, ContanctType.fldFormul, ContanctType.fldFormulId, Icon, pasavand, fileName, user.UserInputId, jsonstr);
                            if (Session["savePath"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                                Session.Remove("savePath");
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
                        if (Permission.haveAccess(106, "tblContanctType", ContanctType.fldId.ToString()))
                        {
                            if (Session["savePath"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePath"].ToString()));
                                pasavand = Path.GetExtension(Session["savePath"].ToString());
                                Icon = stream.ToArray();
                                fileName = Session["FileName"].ToString();
                            }
                            else
                            {
                                if (DeleteFile == true)
                                {
                                    Icon = null;
                                    pasavand = "";
                                    fileName = "";
                                }
                                else
                                {
                                    var Ac = m.prs_tblFileSelect("fldId", ContanctType.fldIconId.ToString(), 0).FirstOrDefault();

                                    if (Ac != null)
                                    {
                                        Icon = Ac.fldFile;
                                        pasavand = Ac.fldPasvand;
                                        fileName = Ac.fldFileName;
                                    }
                                }
                            }
                            var q1 = m.prs_tblContanctTypeUpdate(ContanctType.fldId, ContanctType.fldType, ContanctType.fldIconId, Icon, pasavand, fileName, user.UserInputId, ContanctType.fldTimeStamp).FirstOrDefault();
                            if (Session["savePath"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                                Session.Remove("savePath");
                                Session.Remove("FileName");
                                System.IO.File.Delete(physicalPath);
                            }

                            if (q1.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
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
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("نوع تماس", ContanctType.fldType);
                            parameters.Add("نام فایل", fileName);
                            parameters.Add("پسوند", pasavand);

                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "ContanctType:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "Cnt.tblContanctType", jsonstr, false, ContanctType.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "Cnt.tblContanctType", jsonstr, true, ContanctType.fldId);
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

                    if (Session["savePath"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePath"].ToString());
                        Session.Remove("savePath");
                        Session.Remove("FileName");
                        System.IO.File.Delete(physicalPath);
                    }
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نوع تماس", ContanctType.fldType);
                    parameters.Add("نام فایل", fileName);
                    parameters.Add("پسوند", pasavand);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "ContanctType:Save: " + InnerException);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (ContanctType.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "Cnt.tblContanctType", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "Cnt.tblContanctType", jsonstr, false, ContanctType.fldId);
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
                string Msg = "", MsgTitle = ""; byte Er = 0; var Change = 0; var checkDel = false;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "", title = "";
                title = m.prs_tblContanctTypeSelect("fldId", id.ToString(), 0).FirstOrDefault().fldType;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نوع تماس", title);
                try
                {//حذف

                    //service.DeleteOpertion_Action(id, user.UserKey, IP);
                    if (Permission.haveAccess(107, "tblContanctType", id.ToString()))
                    {
                        var Na = m.prs_tblContactSelect("CheckTypeContactId", id.ToString(), user.UserSecondId, 0).FirstOrDefault();
                        if (Na != null)
                            checkDel = true;

                        if (checkDel)
                        {
                            parameters.Add("متن خطا", "ContanctType:Delete: " + "ایتم انتخاب شده قبلا در سیستم استفاده شده است.");
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            m.prs_LogInsert(user.UserInputId, "Cnt.tblContanctType", jsonstr, false);
                            return Json(new
                            {
                                Msg = "آیتم انتخاب شده قبلا در سیستم استفاده شده است.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                        var q = m.prs_tblContanctTypeDelete(id, TimeStamp).FirstOrDefault();

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
                            parameters.Add("متن خطا", "ContanctType:Delete: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "Cnt.tblContanctType", jsonstr, false, id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "Cnt.tblContanctType", jsonstr, true, id);
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
                    parameters.Add("متن خطا", "ContanctType:Delete: " + InnerException);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "Cnt.tblContanctType", jsonstr, false, id);
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
                var q = m.prs_tblContanctTypeSelect("fldId", Id.ToString(),1).FirstOrDefault();

                //param.FieldName = "fldId";
                //param.Value = q.fldIconId.ToString();
                //var Iconn = "";
                //var file = service.SelectFile(param, user.UserKey, IP).FileList.FirstOrDefault();
                //if (file != null)
                //{
                //    Iconn = Convert.ToBase64String(file.fldFile);
                //}
                return Json(new
                {
                    fldId = q.fldId,
                    fldType = q.fldType,
                    fldIconId = q.fldIconId,
                    fldIcon = Convert.ToBase64String(q.fldFile),
                    NameFile = q.fldFileName,
                    fldTimeStamp = q.fldTimeStamp
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
                if (Permission.haveAccess(104,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblContanctTypeSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblContanctTypeSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldType":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldType";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_tblContanctTypeSelect(field, searchtext,100).ToList();
                            else
                                data = m.prs_tblContanctTypeSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblContanctTypeSelect("","",100).ToList();
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

                    List<prs_tblContanctTypeSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult ReadContactType(StoreRequestParameters parameters, int ContactTypeId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(104,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    List<prs_tblContanctTypeSelect> data = null;

                    data = m.prs_tblContanctTypeSelect("fldId", ContactTypeId.ToString(),0).ToList();

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

                    List<prs_tblContanctTypeSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
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
        public ActionResult DetailsFormul(int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string FarsiFormula = "";
                string Formul = "";
                UserInfo user = (UserInfo)(Session["User"]);

                /* param.FieldName = "fldId";
                 param.Value = fldId.ToString();
                 param.Top = 1;
                 param.UserInfoId = user.UserInputId;
                 param.UserId = user.UserId;
                 var q = service.SelectRules(param, user.UserKey, IP).listRules.FirstOrDefault();
                 var s = q.fldFormul.Split(';');
                 for (int i = 0; i < s.Length - 1; i++)
                 {
                     //if (s[i] == "if(" || s[i] == "then" || s[i] == "else")
                     //{
                     //    FarsiFormula = FarsiFormula + s[i] + ";";
                     //    Formul = Formul + s[i];
                     //}
                     //else
                     //{
                     param.FieldName = "fldEnglishNameFormul";
                     param.Value = s[i];
                     param.Top = 1;
                     param.UserInfoId = user.UserInputId;
                     param.UserId = user.UserId;

                     var f = service.SelectAvamelMoasser_Upgrade(param, user.UserKey, IP).AvamelList.FirstOrDefault();
                     if (f != null)
                     {
                         FarsiFormula = FarsiFormula + f.fldName + ";";
                         Formul = Formul + f.fldName;
                     }
                     else
                     {
                         param.FieldName = "fldNameEn";
                         param.Value = s[i];
                         param.Top = 1;
                         param.UserInfoId = user.UserInputId;
                         param.UserId = user.UserId;
                         var f1 = service.SelectDegreeEducation(param, user.UserKey, IP).DegreeEducationList.FirstOrDefault();
                         if (f1 != null)
                         {
                             FarsiFormula = FarsiFormula + f1.fldTitle + ";";
                             Formul = Formul + f1.fldTitle;
                         }
                         else
                         {
                             FarsiFormula = FarsiFormula + s[i] + ";";
                             Formul = Formul + s[i];
                         }
                     }
                     //}
                 }
                 FarsiFormula = FarsiFormula.Replace("if(", "اگر(").Replace("then", "آنگاه ").Replace("else", "درغیراین صورت ");
                 Formul = Formul.Replace("if(", "اگر(").Replace("then", "آنگاه ").Replace("else", "درغیراین صورت ");
                 return Json(new
                 {
                     Formul = Formul,
                     FarsiFormula = FarsiFormula,
                     EnglishFormula = q.fldFormul
                 }, JsonRequestBehavior.AllowGet);*/
                return Json(new
                {
                    Formul = "",
                    FarsiFormula = "",
                    EnglishFormula = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalcFormul(string Formul)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string result = "";
                string temp = "";
                UserInfo user = (UserInfo)(Session["User"]);

                Formul = Formul.Replace("÷", "/");
                int Count1 = Formul.Count(k => k == ')');
                int Count2 = Formul.Count(k => k == '(');
                int Count3 = Formul.Count(k => k == ']');
                int Count4 = Formul.Count(k => k == '[');
                if (Count1 != Count2 || Count3 != Count4)
                {
                    return Json(new { Er = 1, MsgTitle = "خطا", Msg = "تعداد پرانتزهای باز شده با تعداد پرانتزهای بسته شده یکی نیست." }, JsonRequestBehavior.AllowGet);
                }

                var s = Formul.Split(';');
                for (int i = 0; i < s.Length - 1; i++)
                {
                    if (s[i] == "then" || s[i] == "else")
                    {
                        s[i] = ",";
                    }
                    //else
                    //{
                    //    param.FieldName = "fldEnglishNameFormul";
                    //    param.Value = s[i];
                    //    param.Top = 1;
                    //    param.UserInfoId = user.UserInputId;
                    //    param.UserId = user.UserId;
                    //    var f = service.SelectAvamelMoasser_Upgrade(param, user.UserKey, IP).AvamelList.FirstOrDefault();
                    //    if (f != null)
                    //    {
                    //        s[i] = f.fldId.ToString();
                    //    }

                    //}
                    temp = temp + s[i];
                }
                var sss = Summary(temp).Replace('[', '(').Replace(']', ')');
                result = Calculate(sss).ToString();
                return Json(new { Er = 0, FResult = Convert.ToInt64(result.Split('.')[0]) }, JsonRequestBehavior.AllowGet);
            }
        }

        private static string Summary(string temp)
        {
            string _prefix = "";
            string _postfix = "";
            string str;
            string _else = str = temp;
            string _then = str;
            string _if = str;
            if (!temp.Trim().Contains("if"))
                return temp;
            spliteStatment(temp, out _prefix, out _if, out _then, out _else, out _postfix);
            if (Condition(Summary(_if)))
                return Summary(_prefix + _then + _postfix);
            else
                return Summary(_prefix + _else + _postfix);
        }

        private static bool Condition(string _if)
        {
            if (_if.Contains("!="))
            {
                string[] strArray = _if.Split(new string[1]
        {
          "!="
        }, StringSplitOptions.None);
                double? nullable1 = Calculate(strArray[0]);
                double? nullable2 = Calculate(strArray[1]);
                return (nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) != 0;
            }
            else if (_if.Contains("="))
            {
                if (CountOf(_if, '=') > 1)
                    throw new Exception("Error in =");
                string[] strArray = _if.Split(new string[1]
        {
          "="
        }, StringSplitOptions.None);
                double? nullable1 = Calculate(strArray[0]);
                double? nullable2 = Calculate(strArray[1]);
                return (nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue == nullable2.HasValue ? 1 : 0)) != 0;
            }
            else if (_if.Contains("<"))
            {
                if (CountOf(_if, '<') > 2)
                    throw new Exception("Error in <");
                string[] strArray = _if.Split(new string[1]
        {
          "<"
        }, StringSplitOptions.None);
                if (CountOf(_if, '<') == 1)
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    return (nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0;
                }
                else
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    int num;
                    if ((nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0)
                    {
                        nullable1 = Calculate(strArray[1]);
                        nullable2 = Calculate(strArray[2]);
                        num = (nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 ? 1 : 0;
                    }
                    else
                        num = 1;
                    return num == 0;
                }
            }
            else
            {
                if (!_if.Contains(">"))
                    return true;
                if (CountOf(_if, '>') > 2)
                    throw new Exception("Error in >");
                string[] strArray = _if.Split(new string[1]
        {
          ">"
        }, StringSplitOptions.None);
                if (CountOf(_if, '>') == 1)
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    return (nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0;
                }
                else
                {
                    double? nullable1 = Calculate(strArray[0]);
                    double? nullable2 = Calculate(strArray[1]);
                    int num;
                    if ((nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) != 0)
                    {
                        nullable1 = Calculate(strArray[1]);
                        nullable2 = Calculate(strArray[2]);
                        num = (nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 ? 1 : 0;
                    }
                    else
                        num = 1;
                    return num == 0;
                }
            }
        }

        private static int CountOf(string STR, char CHAR)
        {
            int num1 = 0;
            foreach (int num2 in STR)
            {
                if (num2 == (int)CHAR)
                    ++num1;
            }
            return num1;
        }

        public static double? Calculate(string Formoul)
        {
            Models.FM F = new Models.FM();
            int num = 12;
            double result;
            if (F.Neu(Formoul, out result) || num != 21)
                return new double?(result);
            return new double?();
        }

        private static void spliteStatment(string temp, out string _prefix, out string _if, out string _then, out string _else, out string _postfix)
        {
            bool flag = false;
            do
            {
                if (temp.Length > 0 && (flag = Removeable(temp)))
                    temp = temp.Substring(1, temp.Length - 2);
            }
            while (flag);
            _prefix = "";
            if (!temp.StartsWith("if("))
            {
                for (int index = 2; index < temp.Length; ++index)
                {
                    if ((int)temp[index - 2] == 105 && (int)temp[index - 1] == 102 && (int)temp[index] == 40)
                    {
                        _prefix = temp.Substring(0, index - 2);
                        if (((object)temp.Substring(index - 2)).ToString() != "")
                        {
                            temp = temp.Substring(index - 2);
                            break;
                        }
                    }
                }
            }
            do
            {
                if (temp.Length > 0 && (flag = Removeable(temp)))
                    temp = temp.Substring(1, temp.Length - 2);
            }
            while (flag);
            int num = 0;
            int index1;
            for (index1 = 2; index1 < temp.Length; ++index1)
            {
                if ((int)temp[index1] == 40 || (int)temp[index1] == 91)
                    ++num;
                if ((int)temp[index1] == 41 || (int)temp[index1] == 93)
                    --num;
                if ((int)temp[index1] == 44 && num == 0)
                    break;
            }
            _if = temp.Substring(temp.IndexOf("if(") + 3, index1 - 4);
            int index2;
            for (index2 = index1 + 1; index2 < temp.Length; index2++)
            {
                if ((int)temp[index2] == 40 || (int)temp[index2] == 91)
                    num++;
                if ((int)temp[index2] == 41 || (int)temp[index2] == 93)
                    num--;
                if ((int)temp[index2] == 44 && num == 0)
                    break;
            }
            _then = temp.Substring(temp.IndexOf(_if) + _if.Length + 2, index2 - (temp.IndexOf(_if) + _if.Length + 2));
            _postfix = "";
            if (index2 == temp.Length)
            {
                _else = "";
            }
            else
            {
                int index3;
                for (index3 = index2 + 1; index3 < temp.Length; ++index3)
                {
                    if ((int)temp[index3] == 40 || (int)temp[index3] == 91)
                        ++num;
                    else if (((int)temp[index3] == 41 || (int)temp[index3] == 93) && num != 0)
                        --num;
                    else if (((int)temp[index3] == 41 || (int)temp[index3] == 93) && num == 0)
                        break;
                }
                _else = temp.Substring(temp.IndexOf("if(" + _if + "," + _then) + _if.Length + _then.Length + 7, index3 - (temp.IndexOf("if(" + _if + "," + _then) + _if.Length + _then.Length + 7));
                _postfix = temp.Substring(index3);
            }
        }

        private static bool Removeable(string temp)
        {
            if (temp.Length < 1 || (int)temp[0] != 40 && (int)temp[0] != 91 || (int)temp[temp.Length - 1] != 41 && (int)temp[temp.Length - 1] != 93)
                return false;
            int num = 0;
            for (int index = 0; index < temp.Length; ++index)
            {
                if ((int)temp[index] == 40 || (int)temp[index] == 91)
                    ++num;
                if ((int)temp[index] == 41 || (int)temp[index] == 93)
                {
                    --num;
                    if (num == 0 && index < temp.Length - 1)
                        return false;
                    if (num == 0 && index == temp.Length - 1)
                        return true;
                }
            }
            return true;
        }

        /*public ActionResult SaveFormul(MedicalExaminations.SrvClasses.TOL.Operation.OBJ_Rules Rules)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            string Msg = "", MsgTitle = ""; var Er = 0;
            UserInfo user = (UserInfo)(Session["User"]);
            try
            {
                Rules.fldInputId = user.UserInputId;
                Rules.fldUserId = user.UserId;
                var output = service.UpdateFormulRules("Rule", Rules.fldFormul, Rules.fldId, user.UserKey, IP);
                if (output.ErrorType)
                {
                    return Json(new
                    {
                        Er = 1,
                        Msg = output.Msg,
                        MsgTitle = output.MsgTitle
                    }, JsonRequestBehavior.AllowGet);
                }
                Msg = output.Msg;
                MsgTitle = output.MsgTitle;
            }
            catch (Exception x)
            {
                var ErMsg = "";
                if (x.InnerException != null)
                    ErMsg = x.InnerException.Message;
                else
                    ErMsg = x.Message;
                var Msg3 = service.Error(ErMsg, user.UserKey, IP);
                return Json(new
                {
                    MsgTitle = Msg3.MsgTitle,
                    Msg = Msg3.Msg,
                    Er = 1
                });
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }*/
    }
}
