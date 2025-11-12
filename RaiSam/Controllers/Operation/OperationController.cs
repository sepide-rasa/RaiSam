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
using System.ComponentModel;
using RaiSam.Controllers;
using Newtonsoft.Json;

namespace RaiSam.Controllers.Operation
{
    public class OperationController : Controller
    {
        //
        // GET: /Operation/
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
        public FileContentResult ShowHelpOperation()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "13", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinOperation()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoOperation()
        {
            if (Session["User"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "13", 1).FirstOrDefault();
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
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                try
                {

                
                if (Permission.haveAccess(99, "tblOperation", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblOperationSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblOperationSelect> data1 = null;
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
                                case "fldEffectiveName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldEffectiveName";
                                    break;
                                case "fldNameDataType":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameDataType";
                                    break;
                                case "fldGroupName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldGroupName";
                                    break;
                                case "fldUsableName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldUsableName";
                                    break;
                                case "fldDescAction":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDescAction";
                                    break;
                                case "fldMethodName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldMethodName";
                                    break;
                                case "fldSpecificShowName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldSpecificShowName";
                                    break;
                                case "fldActive_DeactiveName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldActive_DeactiveName";
                                    break;
                                case "fldNoeCharkheshName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNoeCharkheshName";
                                    break;
                                case "fldTypeName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTypeName";
                                    break;
                            }
                            if (data != null)
                                data1 = m.prs_tblOperationSelect(field, searchtext,0,100).ToList();
                            else
                                data = m.prs_tblOperationSelect(field, searchtext, 0, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblOperationSelect("","",0,100).ToList();
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

                    List<prs_tblOperationSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
                }
                catch (Exception x)
                {

                    throw;
                }
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetDynamicTable()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        Models.RaiSamEntities m = new RaiSamEntities();
        //        param.FieldName = "";
        //        param.Value = "";
        //        param.Top = 0;
        //        var q = m.SelectMoayenatDynamic(param, user.UserKey, IP).MoayenatDynamicList.Select(l => new { ID = l.fldId, fldName = l.fldTitle });
        //        return this.Store(q);
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRotatingAgent()
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
                var q = m.prs_tblAnavinGhabelCharkheshSelect("","",0).Select(l => new { ID = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMethodName()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Type type = (typeof(SpecializedKartablController));
                Type type2 = (typeof(GeneralKartablController));
                var q2 = type2.GetMethods().Where(m => m.GetCustomAttributes(typeof(ActionAttribute), false).Length > 0)
                    .Select(l => new
                    {
                        fldId = l.Name,
                        fldTableName = l.GetCustomAttributes(typeof(ActionAttribute), true)
                            .Cast<ActionAttribute>().SingleOrDefault().TableName,
                        fldName = l.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                            .Cast<DisplayNameAttribute>().SingleOrDefault() != null ? l.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault().DisplayName : l.Name
                    }).ToList();
                var q = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(ActionAttribute), false).Length > 0)
                    .Select(l => new
                    {
                        fldId = l.Name,
                        fldTableName = l.GetCustomAttributes(typeof(ActionAttribute), true)
                            .Cast<ActionAttribute>().SingleOrDefault().TableName,
                        fldName = l.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                        .Cast<DisplayNameAttribute>().SingleOrDefault() != null ? l.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault().DisplayName : l.Name
                    }).ToList();
                q.AddRange(q2);
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDataType()
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
                var q = m.prs_tblDataTypesSelect("","",0).Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        //public ActionResult GetMethodName()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    Type type = (typeof(SpecializedKartablController));
        //    Type type2 = (typeof(GeneralKartablController));
        //    var q2 = type2.GetMethods().Where(m => m.GetCustomAttributes(typeof(ActionAttribute), false).Length > 0)
        //        .Select(l => new
        //        {
        //            fldId = l.Name,
        //            fldTableName = l.GetCustomAttributes(typeof(ActionAttribute), true)
        //                .Cast<ActionAttribute>().SingleOrDefault().TableName,
        //            fldName = l.GetCustomAttributes(typeof(DisplayNameAttribute), true)
        //                .Cast<DisplayNameAttribute>().SingleOrDefault() != null ? l.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault().DisplayName : l.Name
        //        }).ToList();
        //    var q = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(ActionAttribute), false).Length > 0)
        //        .Select(l => new
        //        {
        //            fldId = l.Name,
        //            fldTableName = l.GetCustomAttributes(typeof(ActionAttribute), true)
        //                .Cast<ActionAttribute>().SingleOrDefault().TableName,
        //            fldName = l.GetCustomAttributes(typeof(DisplayNameAttribute), true)
        //            .Cast<DisplayNameAttribute>().SingleOrDefault() != null ? l.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault().DisplayName : l.Name
        //        }).ToList();
        //    q.AddRange(q2);
        //    return this.Store(q);
        //}
        [Throttle(Name = "UploadThrottle")]
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            try
            {

                if (Session["savePathOperation"] != null)
                {
                    System.IO.File.Delete(Session["savePathOperation"].ToString());
                    Session.Remove("FileName");
                    Session.Remove("savePathOperation");
                }
                var IsImage = FileInfoExtensions.IsImage(Request.Files[0]);
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                if (IsImage == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".tif"
                || extension == ".tiff" || extension==".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 307200)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        file.SaveAs(savePath);
                        Session["savePathOperation"] = savePath;
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
                            Message = "حجم فایل انتخابی می بایست کمتر از 300 کیلو بایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از 300 کیلو بایت باشد."
                        //});
                        //DirectResult result = new DirectResult();
                        //return result;
                    }
                }

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
                Models.RaiSamEntities m = new RaiSamEntities();
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
            if (Session["savePathOperation"] != null)
            {
                string physicalPath = System.IO.Path.Combine(Session["savePathOperation"].ToString());
                Session.Remove("savePathOperation");
                Session.Remove("FileName");
                System.IO.File.Delete(physicalPath);
                Msg = "حذف فایل با موفقیت انجام شد";
            }
            return Json(new
            {
                Msg = Msg
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowPic(string dc)
        {//برگرداندن عکس 
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            byte[] file = null;
            MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathOperation"].ToString()));
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
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblOperationSelect("fldId", Id.ToString(),0,1).FirstOrDefault();
                string pic = "";
                if (q.fldFile != null)
                {
                    pic = Convert.ToBase64String(q.fldFile);
                }
                return Json(new
                {
                    fldId = q.fldId,
                    fldName = q.fldName,
                    fldDataTypeId = q.fldDataTypeId,
                    fldGroup = q.fldGroup,
                    fldUsable = q.fldUsable,
                    fldeffective = q.fldeffective,
                    fldFileId = q.fldFileId,
                    NameFile = q.fldfileName,
                    pic = pic,
                    fldMethodName = q.fldMethodName,
                    fldActive_Deactive = Convert.ToByte(q.fldActive_Deactive).ToString(),
                    fldSpecificShow = q.fldSpecificShow,
                    fldDescAction = q.fldDescAction,
                    fldNoeCharkheshId = q.fldNoeCharkheshId,
                    fldtype = q.fldtype,
                    fldTimeStamp = q.fldTimeStamp,
                    fldIsDynamic = q.fldIsDynamic//,
                   // fldMoayenatDynamicId = q.fldMoayenatDynamicId.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getAx(int fldId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var q = m.prs_tblOperationSelect("fldId", fldId.ToString(),0,0).FirstOrDefault();
            var image = "IA==";
            if (q != null && q.fldFile != null)
            {
                image = Convert.ToBase64String(q.fldFile);
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
        public ActionResult Save(string Operation1, string NoeGhabeleCharkheIds, string NoeGhabeleCharkheName, string DeletedFile)
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
                string Msg = "", MsgTitle = ""; var Er = 0;
                byte[] file = null; string Pasvand = ""; string NameFile = ""; byte TimeStamp = 1; var Change = 0;

                prs_tblOperationSelect Operation = JsonConvert.DeserializeObject<prs_tblOperationSelect>(Operation1);
                Operation.fldDescAction = System.Uri.UnescapeDataString(Operation.fldDescAction);
                var template = new { DeleteFileOPP = false };
                bool DeleteFileOP = JsonConvert.DeserializeAnonymousType(DeletedFile, template).DeleteFileOPP;

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان", Operation.fldName);
                parameters.Add("توضیحات اکشن", Operation.fldDescAction);
                parameters.Add("نوع داده", Operation.fldNameDataType);
                parameters.Add("وضعیت اعمال", Operation.fldGroupName);
                parameters.Add("قابل نمایش در کارتابل عمومی", Operation.fldUsableName);
                parameters.Add("نوع اکشن", Operation.fldEffectiveName);
                parameters.Add("نام متد", Operation.fldMethodName);
                parameters.Add("قابل نمایش در کارتابل تخصصی", Operation.fldSpecificShowName);
                parameters.Add("نام فارسی متد", Operation.fldFA_Name);
                parameters.Add("نام جدول", Operation.fldNameTable);
                parameters.Add("وضعیت", Operation.fldActive_DeactiveName);
                parameters.Add("توضیحات", Operation.fldDesc);
                parameters.Add("نوع استفاده", Operation.fldTypeName);
                //parameters.Add("نوع معاینه", Operation.fldTitleMoayenatDynamic);
                //parameters.Add("نوع پیاده سازی", Operation.fldIsDynamicName);
                string jsonstr = "";
                try
                {
                    if (Operation.fldDesc == null)
                    {
                        Operation.fldDesc = "";
                    }
                    if (Operation.fldFA_Name == null)
                    {
                        Operation.fldFA_Name = "";
                    }
                    if (Operation.fldNameTable == null)
                    {
                        Operation.fldNameTable = "";
                    }
                    if (Operation.fldMethodName == null)
                    {
                        Operation.fldMethodName = "";
                    }
                    if (Operation.fldId == 0)
                    {
                        if (Permission.haveAccess(100, "dbo.tblOperation", null))
                        {
                            //ذخیره
                            if (Session["savePathOperation"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathOperation"].ToString()));
                                file = stream.ToArray();
                                Pasvand = Path.GetExtension(Session["savePathOperation"].ToString());
                                NameFile = Session["FileName"].ToString();
                            }
                            else
                            {
                                Pasvand = "";
                                file = null;
                                NameFile = "";
                            }

                           
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                            System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("fldId", typeof(int));

                            m.prs_tblOperationInsert(Id, Operation.fldName, Operation.fldDesc, file, Pasvand, NameFile, u.UserInputId, Operation.fldDesc, Operation.fldDataTypeId, Operation.fldGroup,
                                Operation.fldUsable, Operation.fldeffective, Operation.fldFormulId, Operation.fldMethodName, Operation.fldSpecificShow, Operation.fldFA_Name, Operation.fldNameTable,
                                Operation.fldActive_Deactive, jsonstr, Operation.fldtype, Operation.fldIsDynamic);
                            if (Session["savePathOperation"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathOperation"].ToString());
                                Session.Remove("savePathOperation");
                                Session.Remove("FileName");
                                System.IO.File.Delete(physicalPath);
                            }

                            parameters = new Dictionary<string, object>();
                            parameters.Add("نوع قابل چرخش", NoeGhabeleCharkheName);
                            parameters.Add("عنوان اقدام", Operation.fldName);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblOpertion_NoeCharkheshInsert(Convert.ToInt32(Id.Value), NoeGhabeleCharkheIds, "", u.UserInputId, jsonstr);

                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "ذخیره موفق";
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
                        if (Permission.haveAccess(101, "dbo.tblOperation", Operation.fldId.ToString()))
                        {
                            //ویرایش
                            if (Session["savePathOperation"] != null)
                            {
                                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathOperation"].ToString()));
                                Pasvand = Path.GetExtension(Session["savePathOperation"].ToString());
                                file = stream.ToArray();
                                NameFile = Session["FileName"].ToString();
                            }
                            else
                            {
                                if (DeleteFileOP == true)
                                {
                                    file = null;
                                    Pasvand = "";
                                    NameFile = "";
                                }
                                else
                                {
                                    var pic = m.prs_tblFileSelect("fldId", Operation.fldFileId.ToString(), 1).FirstOrDefault();
                                    if (pic != null && pic.fldFile != null)
                                    {
                                        file = pic.fldFile;
                                        Pasvand = pic.fldPasvand;
                                        NameFile = pic.fldFileName;
                                    }
                                }
                            }

                            var s = m.prs_tblOperationUpdate(Operation.fldId, Operation.fldName, Operation.fldDescAction, Operation.fldFileId, file, Pasvand, NameFile, Operation.fldDesc,
                                Operation.fldDataTypeId, Operation.fldGroup, Operation.fldUsable, Operation.fldeffective, Operation.fldMethodName, Operation.fldSpecificShow, Operation.fldFA_Name,
                                Operation.fldNameTable, Operation.fldActive_Deactive, Operation.fldTimeStamp, Operation.fldtype, Operation.fldIsDynamic, u.UserInputId).FirstOrDefault();
                            if (Session["savePathOperation"] != null)
                            {
                                string physicalPath = System.IO.Path.Combine(Session["savePathOperation"].ToString());
                                Session.Remove("savePathOperation");
                                Session.Remove("FileName");
                                System.IO.File.Delete(physicalPath);
                            }
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
                                parameters.Add("متن خطا", "Operation:Edit: " + Msg);
                                Logstatus = false;
                            }


                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogUpdate(u.UserInputId, "dbo.tblOperation", jsonstr, Logstatus, Operation.fldId);


                            m.prs_tblOpertion_NoeCharkheshUpdate(Operation.fldId, NoeGhabeleCharkheIds, "", u.UserInputId);

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
                    parameters.Add("کد خطا", ErrorId.Value);
                    if (Operation.fldId == 0)
                    {
                        parameters.Add("متن خطا", "Operation:Save: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogInsert(u.UserInputId, "dbo.tblOperation", jsonstr, false);
                    }
                    else
                    {
                        parameters.Add("متن خطا", "Operation:Edit: " + InnerException);
                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogUpdate(u.UserInputId, "dbo.tblOperation", jsonstr, false, Operation.fldId);
                    }

                    if (Session["savePathOperation"] != null)
                    {
                        string physicalPath = System.IO.Path.Combine(Session["savePathOperation"].ToString());
                        Session.Remove("savePathOperation");
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
                    TimeStamp = TimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, int TimeStamp)
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
                var q = m.prs_tblOperationSelect("fldId", Id.ToString(),0,0).FirstOrDefault();
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان", q.fldName);
                parameters.Add("توضیحات اکشن", q.fldDescAction);
                try
                {//حذف
                    if (Permission.haveAccess(102, "dbo.tblOperation", Id.ToString()))
                    {
                    var s = m.prs_tblOperationDelete(Id, TimeStamp).FirstOrDefault();
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
                            parameters.Add("متن خطا", "Operation:Delete: " + Msg);
                            Logstatus = false;
                        }

                        jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                        m.prs_LogDelete(u.UserInputId, "dbo.tblOperation", jsonstr, Logstatus, Id);
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

                    m.prs_LogDelete(u.UserInputId, "dbo.tblOperation", jsonstr, false, Id);
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
        public ActionResult SaveOrder(string Order)
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
                string Msg = "", MsgTitle = ""; var Er = 0; 
                try
                {

                    List<OBJ_UpdateOrder> Order1 = JsonConvert.DeserializeObject<List<OBJ_UpdateOrder>>(Order);
                    foreach (var item in Order1)
                    {
                        var s = m.prs_UpdateOrder("Operation", item.fldId, item.order);
                    }
                    MsgTitle = "ویرایش با موفقیت انجام شد.";
                    Msg = "ویرایش موفق";
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
