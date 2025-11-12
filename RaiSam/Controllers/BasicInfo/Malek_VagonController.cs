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
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace RaiSam.Controllers.BasicInfo
{
    public class Malek_VagonController : Controller
    {
        //
        // GET: /Malek_Vagon/
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
        public ActionResult UploadFileMalekan()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var partial = new Ext.Net.MVC.PartialViewResult();
            var curdate = m.prs_GetDate().FirstOrDefault().fldTarikh;
            partial.ViewBag.sal = curdate.Substring(0, 4);
            return partial;
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpMalek_Vagon()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "30", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinMalek_Vagon()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoMalek_Vagon()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "30", 1).FirstOrDefault();
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
                if (Permission.haveAccess(173, "", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblMalek_VagonInfo_MalekSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblMalek_VagonInfo_MalekSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTypeSamaneName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTypeSamaneName";
                                    break;
                                case "fldNameCompany":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameCompany";
                                    break;
                                case "fldCodeEghtasadi":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldCodeEghtasadi";
                                    break;
                                case "fldShenaseMeli":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldShenaseMeli";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblMalek_VagonInfo_MalekSelect(field, searchtext, 100).ToList();
                            else
                                data = m.prs_tblMalek_VagonInfo_MalekSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblMalek_VagonInfo_MalekSelect("", "", 100).ToList();
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

                    List<prs_tblMalek_VagonInfo_MalekSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult UploadExcelFileMalek()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                if (Session["savePathExcelMalek"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathExcelMalek"].ToString());
                    Session.Remove("savePathExcelMalek");
                    System.IO.File.Delete(physicalPath);
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var FileNamee = Path.GetFileNameWithoutExtension(Request.Files[0].FileName) + user.UserName + extension;
                if (extension == ".xls" || extension == ".xlsx")
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string savePath = Server.MapPath(@"~\Uploaded\" + FileNamee);
                    file.SaveAs(savePath);
                    Session["savePathExcelMalek"] = savePath;
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
                        Message = "فایل انتخاب شده معتبر نمی باشد."
                    });
                    DirectResult result = new DirectResult();
                    return result;
                }
            }
            catch (Exception x)
            {
                if (Session["savePathExcelMalek"] != null)
                {
                    System.IO.File.Delete(Session["savePathExcelMalek"].ToString());
                    Session.Remove("savePathExcelMalek");
                }
                return null;
            }
        }
        public ActionResult SaveMalekExcel(string TypeM)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);
            string Msg = "", MsgTitle = "";
            byte Er = 0;
            try
            {

                DataTable dt = ProcessXlsMalek(Session["savePathExcelMalek"].ToString(), TypeM);
                string[] FeedBack = ProcessBulkCopyMalek(dt).Split(';');
                Msg = FeedBack[1];
                MsgTitle = FeedBack[0];
                if (FeedBack[0] == "خطا")
                {
                    Er = 1;
                }
                if (Session["savePathExcelMalek"] != null)
                {
                    System.IO.File.Delete(Session["savePathExcelMalek"].ToString());
                    Session.Remove("savePathExcelMalek");
                }
            }
            catch (Exception x)
            {
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                string InnerException = x.Message;
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, user.UserInputId, "");
                if (Session["savePathExcelMalek"] != null)
                {
                    System.IO.File.Delete(Session["savePathExcelMalek"].ToString());
                    Session.Remove("savePathExcelMalek");
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
                Er = Er
            }, JsonRequestBehavior.AllowGet);
        }
        private DataTable ProcessXlsMalek(string fileName,string typem)
        {
            DataTable dt = new DataTable();
            UserInfo user = (UserInfo)(Session["User"]);

            dt.Columns.Add("fldId");
            dt.Columns.Add("fldSamaneMarjaId").DefaultValue = "";
            dt.Columns.Add("fldTypeSamane").DefaultValue = Convert.ToByte(typem);
            dt.Columns.Add("fldNameCompany");
            dt.Columns.Add("fldCodeEghtasadi");
            dt.Columns.Add("fldShenaseMeli");
            dt.Columns.Add("fldInputId").DefaultValue = user.UserInputId;
            dt.Columns.Add("fldDate").DefaultValue = DateTime.Now;

            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(fileName);
            for (int i = 1; i < wb.Worksheets[0].Cells.MaxDataRow + 1; i++)
            {
                dt.Rows.Add();
                int count = 3;
                for (int j = wb.Worksheets[0].Cells.MinColumn; j < wb.Worksheets[0].Cells.MaxDataColumn + 1; j++)
                {
                    var str = wb.Worksheets[0].Cells[i, j].StringValue;
                    dt.Rows[dt.Rows.Count - 1][count] = str;
                    count++;
                }
            }
            return dt;
        }
        private String ProcessBulkCopyMalek(DataTable dt)
        {
            string Feedback = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (var copy = new SqlBulkCopy(connString, SqlBulkCopyOptions.CheckConstraints))
                {
                    conn.Open();
                    copy.DestinationTableName = "tblMalek_VagonInfo";
                    copy.ColumnMappings.Add("fldId", "fldId");
                    copy.ColumnMappings.Add("fldSamaneMarjaId", "fldSamaneMarjaId");
                    copy.ColumnMappings.Add("fldTypeSamane", "fldTypeSamane");
                    copy.ColumnMappings.Add("fldNameCompany", "fldNameCompany");
                    copy.ColumnMappings.Add("fldCodeEghtasadi", "fldCodeEghtasadi");
                    copy.ColumnMappings.Add("fldShenaseMeli", "fldShenaseMeli");
                    copy.ColumnMappings.Add("fldInputId", "fldInputId");
                    copy.ColumnMappings.Add("fldDate", "fldDate");

                    copy.BatchSize = dt.Rows.Count;
                    try
                    {
                        copy.WriteToServer(dt);
                        Feedback = "ذخیره موفق;ذخیره فایل با موفقیت انجام شد.";
                    }
                    catch (Exception ex)
                    {
                        Feedback = "خطا" + ";" + ex.Message;
                    }
                }
            }
            return Feedback;
        }
        public ActionResult ReloadMalek_Vagon()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string MsgTitle = "عملیات موفق", Msg = "بارگذاری با موفقیت انجام شد"; int Er = 0; 
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();

                string Errmsg="";
                var mList = m.prs_tblMalek_VagonInfo_MalekSelect("", "", 0).ToList();
                try
                {
                    //if (Permission.haveAccess(188, "tblHadafGroupName", id.ToString()))
                    //{
                    foreach (var item in mList)
                    {
                        if (item.fldShenaseMeli != "")
                        {
                            var q = RaiSam.Areas.Faces.Controllers.CompanyProfileController.GetInquiryByNationalCode(item.fldShenaseMeli);
                            if (q != null){
                                if (q.Err != 1)
                                    m.prs_tblMalek_VagonInfoUpdate(item.fldId, item.fldSamaneMarjaId, item.fldTypeSamane, q.Name, item.fldCodeEghtasadi, item.fldShenaseMeli, Convert.ToInt32(q.EstablishmentDate.Replace("/", "")), Convert.ToInt32(q.RegisterDate.Replace("/", "")), q.RegisterNumber, q.PostCode, q.Address, item.fldAshkhasHoghoghiId, user.UserInputId);
                                else
                                    Errmsg = Errmsg + item.fldShenaseMeli + ",";
                            }
                            else
                                Errmsg=Errmsg+item.fldShenaseMeli+",";
                        }
                        else if (item.fldNameCompany != "")
                        {
                            var q = RaiSam.Areas.Faces.Controllers.CompanyProfileController.GetInquiryByName(item.fldNameCompany);
                            if (q != null)
                            {
                                if (q.Err != 1)
                                    m.prs_tblMalek_VagonInfoUpdate(item.fldId, item.fldSamaneMarjaId, item.fldTypeSamane, item.fldNameCompany, item.fldCodeEghtasadi, q.NationalCode, Convert.ToInt32(q.EstablishmentDate.Replace("/", "")), Convert.ToInt32(q.RegisterDate.Replace("/", "")), q.RegisterNumber, q.PostCode, q.Address, item.fldAshkhasHoghoghiId, user.UserInputId);
                                else
                                    Errmsg = Errmsg + item.fldNameCompany + ",";
                            }
                            else
                                Errmsg = Errmsg + item.fldNameCompany + ",";
                        }
                    }
                    if (Errmsg != "")
                    {
                        MsgTitle = "خطا";
                        Msg = "شرکت هایی که قابل واکشی نبود: " + Errmsg;
                    }
                    //}
                    //else
                    //{
                    //    return Json(new
                    //    {
                    //        Msg = "شما مجاز به دسترسی نمی باشید.",
                    //        MsgTitle = "خطا",
                    //        Er = 1
                    //    }, JsonRequestBehavior.AllowGet);
                    //}

                }
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                   
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
                var q = m.prs_tblMalek_VagonInfo_MalekSelect("fldId", Id.ToString(), 0).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldNameCompany = q.fldNameCompany,
                    fldCodeEghtasadi = q.fldCodeEghtasadi,
                    fldSamaneMarjaId=q.fldSamaneMarjaId,
                    fldShenaseMeli=q.fldShenaseMeli,
                    fldTypeSamane=q.fldTypeSamane.ToString(),
                    fldAshkhasHoghoghiId=q.fldAshkhasHoghoghiId,
                    fldAddress=q.fldAddress,
                    fldCodePosti=q.fldCodePosti,
                    fldDateSabt=q.fldDateSabt,
                    fldShomareSabt=q.fldShomareSabt,
                    fldTarikhTasis=q.fldTarikhTasis
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "", title = "";
                title = m.prs_tblMalek_VagonInfo_MalekSelect("fldId", id.ToString(), 0).FirstOrDefault().fldNameCompany;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("نام شرکت", title);
                try
                {
                    if (Permission.haveAccess(176, "tblMalek_VagonInfo", id.ToString()))
                    {
                         m.prs_tblMalek_VagonInfoDelete(id);

                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblMalek_VagonInfo", jsonstr, true, id);

                            Msg = "حذف با موفقیت انجام شد.";
                            MsgTitle = "حذف موفق";
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Malek_VagonInfo:Delete: " + ErMsg);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblMalek_VagonInfo", jsonstr, false, id);
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
                    Change = Change,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblMalek_VagonInfo_MalekSelect malek)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0;
                var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    parameters.Add("نام مالک", malek.fldNameCompany);
                    parameters.Add("کد اقتصادی", malek.fldCodeEghtasadi);
                    parameters.Add("شناسه ملی", malek.fldShenaseMeli);
                    parameters.Add("کد سامانه مرجع", malek.fldSamaneMarjaId);
                    if (malek.fldId == 0)
                    {
                        if (Permission.haveAccess(174, "tblMalek_VagonInfo", null))
                        {

                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblMalek_VagonInfoInsert(malek.fldSamaneMarjaId, malek.fldTypeSamane, malek.fldNameCompany, malek.fldCodeEghtasadi, malek.fldShenaseMeli,
                               Convert.ToInt32(malek.fldTarikhTasis.Replace("/", "")), Convert.ToInt32(malek.fldDateSabt.Replace("/", "")), malek.fldShomareSabt, malek.fldCodePosti, malek.fldAddress, user.UserInputId, jsonstr);

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
                        if (Permission.haveAccess(175, "tblMalek_VagonInfo", malek.fldId.ToString()))
                        {
                             m.prs_tblMalek_VagonInfoUpdate(malek.fldId, malek.fldSamaneMarjaId, malek.fldTypeSamane, malek.fldNameCompany, malek.fldCodeEghtasadi, malek.fldShenaseMeli,
                                 Convert.ToInt32(malek.fldTarikhTasis.Replace("/", "")), Convert.ToInt32(malek.fldDateSabt.Replace("/", "")), malek.fldShomareSabt, malek.fldCodePosti, malek.fldAddress,malek.fldAshkhasHoghoghiId, user.UserInputId);

                             Msg = "ویرایش با موفقیت انجام شد.";
                             MsgTitle = "ویرایش موفق";

                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblMalek_VagonInfo", jsonstr, true, malek.fldId);
                            
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("نام مالک", malek.fldNameCompany);
                    parameters.Add("کد اقتصادی", malek.fldCodeEghtasadi);
                    parameters.Add("شناسه ملی", malek.fldShenaseMeli);
                    parameters.Add("کد سامانه مرجع", malek.fldSamaneMarjaId);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Malek_Vagon:Save: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (malek.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "dbo.tblMalek_VagonInfo", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblMalek_VagonInfo", jsonstr, false, malek.fldId);
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
        public ActionResult LoadFromService(int state, string data,int Id)
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
                        m.prs_UpdateMalekInfoSabt(Id, q.Name, data);
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
                        m.prs_UpdateMalekInfoSabt(Id, data,q.NationalCode);
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
