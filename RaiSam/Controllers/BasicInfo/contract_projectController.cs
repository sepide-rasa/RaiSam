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
    public class contract_projectController : Controller
    {
        //
        // GET: /contract_project/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            ViewData.Model = new RaiSam.Models.Contract_Project();
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo,
                ViewData = this.ViewData
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
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
        public ActionResult NewIndex(int id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTypeContract()
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
                var q = m.prs_tblContractTypeSelect("", "", 0).Select(l => new { fldId = l.fldId, fldName = l.fldTitle });
                return this.Store(q);
            }
        }
        public ActionResult NewHeader(int id)
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
        public ActionResult ReadVagons(string fldHeaderId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<RaiSam.Models.prs_SelectContract_Project_Detail> data = null;
            data = m.prs_SelectContract_Project_Detail("fldHeaderId", fldHeaderId, 0).ToList();
            return this.Store(data);
        }
        public ActionResult UploadFileContract_Project()
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
        public FileContentResult ShowHelpContract_Project()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "34", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinContract_Project()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoContract_Project()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "34", 1).FirstOrDefault();
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
                if (Permission.haveAccess(208, "", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblContract_ProjectSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblContract_ProjectSelect> data1 = null;
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
                                case "fldNameCompany":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameCompany";
                                    break;
                                case "fldShomareContract":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldShomareContract";
                                    break;
                                case "fldTarikhContract":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikhContract";
                                    break;
                                case "fldShomareMovafeghat":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldShomareMovafeghat";
                                    break;
                                case "fldTarikhMovafeghat":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikhMovafeghat";
                                    break;
                                case "fldShomareVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldShomareVagon";
                                    break;
                                case "fldVaznKhali":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldVaznKhali";
                                    break;
                                case "fldTypeVagon":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTypeVagon";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblContract_ProjectSelect(field, searchtext,"",0, 100).ToList();
                            else
                                data = m.prs_tblContract_ProjectSelect(field, searchtext, "", 0, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblContract_ProjectSelect("", "", "", 0, 100).ToList();
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

                    List<prs_tblContract_ProjectSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        private List<Models.prs_SelectAllContract_Project> ProcessXlsProject(string fileName)
        {
            List<Models.prs_SelectAllContract_Project> contr = new List<Models.prs_SelectAllContract_Project>();
            try
            {
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(fileName);
                for (int i = 1; i < wb.Worksheets[0].Cells.MaxDataRow + 1; i++)
                {
                    prs_SelectAllContract_Project person = new prs_SelectAllContract_Project();
                    int count = 0;
                    for (int j = wb.Worksheets[0].Cells.MinColumn; j <= wb.Worksheets[0].Cells.MaxDataColumn ; j++)
                    {
                        var str = wb.Worksheets[0].Cells[i, j].StringValue.Split(',').Join("");
                        switch (count)
                        {
                            case 0:
                                person.fldTitle = str;
                                break;
                            case 1:
                                person.fldSamaneMarjaId = str;
                                break;
                            case 2:
                                person.fldNameCompany = str;
                                break;
                            case 3:
                                person.fldShomareContract = str;
                                break;
                            case 4:
                                person.fldTarikhContract = str;
                                break;
                            case 5:
                                person.fldShomareMovafeghat = str;
                                break;
                            case 6:
                                person.fldTarikhMovafeghat = str;
                                break;
                            case 7:
                                person.fldShomareVagon = str;
                                break;
                            case 8:
                                person.fldVaznKhali = str;
                                break;
                            case 9:
                                person.fldTypeVagon = str;
                                break;
                            case 11:
                                person.fldCodeEghtasadi = str;
                                break;
                            case 15:
                                person.fldShenaseMeli = str;
                                break;
                        }
                        count++;
                    }
                    contr.Add(person);
                }
                return contr;
            }
            catch (Exception x)
            {
                return new List<prs_SelectAllContract_Project>();
            }
        }
        public ActionResult UploadExcelFileProject()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            try
            {
                UserInfo user = (UserInfo)(Session["User"]);
                if (Session["savePathExcelProject"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathExcelProject"].ToString());
                    Session.Remove("savePathExcelProject");
                    System.IO.File.Delete(physicalPath);
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var FileNamee = Path.GetFileNameWithoutExtension(Request.Files[0].FileName) + user.UserName + extension;
                if (extension == ".xls" || extension == ".xlsx")
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string savePath = Server.MapPath(@"~\Uploaded\" + FileNamee);
                    file.SaveAs(savePath);
                    Session["savePathExcelProject"] = savePath;
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
                if (Session["savePathExcelProject"] != null)
                {
                    System.IO.File.Delete(Session["savePathExcelProject"].ToString());
                    Session.Remove("savePathExcelProject");
                }
                return null;
            }
        }
        public ActionResult SaveProjectExcel(string KarbariVagon, string TypeSamane)
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
                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "دخیره موفق";
                            var shKhata = "";
                    var ListProject = ProcessXlsProject(Session["savePathExcelProject"].ToString());
                    foreach (var item in ListProject)
                    {
                        if (item.fldTarikhContract.Substring(0, 1) != "0")
                            item.fldTarikhContract = "13" + item.fldTarikhContract;
                        else
                            item.fldTarikhContract = "14" + item.fldTarikhContract;

                        if (item.fldTarikhMovafeghat.Substring(0, 1) != "0")
                            item.fldTarikhMovafeghat = "13" + item.fldTarikhMovafeghat;
                        else
                            item.fldTarikhMovafeghat = "14" + item.fldTarikhMovafeghat;

                       var s= m.prs_tblContract_Project_ExcelInsert(item.fldTitle, Convert.ToInt32(item.fldTarikhContract), item.fldShomareContract,  Convert.ToInt32(item.fldTarikhMovafeghat), item.fldShomareMovafeghat,
                            item.fldTypeVagon, item.fldShomareVagon, item.fldVaznKhali, Convert.ToByte(KarbariVagon), item.fldSamaneMarjaId, Convert.ToByte(TypeSamane),item.fldNameCompany,
                             item.fldShenaseMeli, item.fldCodeEghtasadi, user.UserInputId).FirstOrDefault();
                       if (s.ErrorCode != 0)
                       {
                           shKhata = shKhata + item.fldShomareContract + "،";
                           //Msg =s.ErrorMessage;
                           //MsgTitle = "خطا";
                       }
                    }

                    if (shKhata != "")
                    {
                        Msg = "خطا در شماره قراردادهای: " + shKhata;
                        MsgTitle = "خطا";
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
               // var q = m.prs_tblContract_Project_DetailSelect("fldId", Id.ToString(), 0).FirstOrDefault();
                var q2 = m.prs_tblContract_ProjectSelect("fldId", Id.ToString(), "", 0, 0).FirstOrDefault();
                return Json(new
                {
                    fldId = q2.fldId,
                    fldNameCompany = q2.fldNameCompany,
                    fldMalekId=q2.fldMalekId,
                    fldTitle = q2.fldTitle,
                    fldShomareContract=q2.fldShomareContract,
                    fldTarikhContract=q2.fldTarikhContract,
                    fldTarikhMovafeghat=q2.fldTarikhMovafeghat,
                    fldShomareMovafeghat=q2.fldShomareMovafeghat,
                   // fldShomareVagon=q.fldShomareVagon,
                  //  fldVagonId=q.fldVagonId,
                    fldSamaneMarjaId = q2.fldSamaneMarjaId,
                   // fldTypeVagon=q.fldTypeVagon,
                   fldTypeContract=q2.fldTypeContract.ToString(),
                   fldHoghoghiId=q2.fldHoghoghiId,
                   fldSaghfSarmayeGozari=q2.fldSaghfSarmayeGozari,
                    fldTimeStamp = q2.fldTimeStamp
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,int TimeStamp)
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
                title = m.prs_tblContract_ProjectSelect("fldId", id.ToString(), "", 0, 0).FirstOrDefault().fldTitle;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان", title);
                try
                {
                    if (Permission.haveAccess(211, "tblContract_Project", id.ToString()))
                    {
                       var q= m.prs_tblContract_ProjectDelete(id, TimeStamp).FirstOrDefault();
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
                             parameters.Add("متن خطا", "Contract_Project:Delete: " + Msg);
                             jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                             m.prs_LogDelete(user.UserInputId, "dbo.tblContract_Project", jsonstr, false, id);
                         }

                         else
                         {
                             jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                             m.prs_LogDelete(user.UserInputId, "dbo.tblContract_Project", jsonstr, true, id);
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Contract_Project:Delete: " + ErMsg);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblContract_Project", jsonstr, false, id);
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
        public ActionResult DeleteVagon(int id)
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
                title = m.prs_tblContract_Project_DetailSelect("fldId", id.ToString(), 0).FirstOrDefault().fldShomareVagon;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("شماره واگن", title);
                try
                {
                    if (Permission.haveAccess(211, "tblContract_Project", id.ToString()))
                    {
                        var q = m.prs_tblContract_Project_DetailDelete(id);
                       
                            Msg = "حذف با موفقیت انجام شد";
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
                    parameters.Add("متن خطا", "Contract_Project:Delete: " + ErMsg);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblContract_Project", jsonstr, false, id);
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
        public ActionResult Save(prs_tblContract_ProjectSelect malek)
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
                    var TarikhMovafeghat=0;
                    if (!(malek.fldTarikhMovafeghat == "" || malek.fldTarikhMovafeghat == null))
                        TarikhMovafeghat = Convert.ToInt32(malek.fldTarikhMovafeghat.Replace("/", ""));

                    if (malek.fldShomareMovafeghat == null)
                        malek.fldShomareMovafeghat = "";

                    if (malek.fldMalekId == 0)
                        malek.fldMalekId = null;
                    if (malek.fldHoghoghiId == 0)
                        malek.fldHoghoghiId = null;

                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    parameters.Add("عنوان", malek.fldTitle);
                 //   parameters.Add("شماره واگن", malek.fldShomareVagon);
                    parameters.Add("شماره قرارداد", malek.fldShomareContract);
                    parameters.Add("تاریخ قرارداد", malek.fldTarikhContract);
                    parameters.Add("شماره موافقتنامه", malek.fldShomareMovafeghat);
                    parameters.Add("تاریخ موافقتنامه", malek.fldTarikhMovafeghat);
                    if (malek.fldId == 0)
                    {
                        if (Permission.haveAccess(209, "tblContract_Project", null))
                        {

                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            var z = m.prs_tblContract_ProjectInsert(malek.fldTitle, Convert.ToInt32(malek.fldTarikhContract.Replace("/", "")), malek.fldShomareContract, TarikhMovafeghat, malek.fldShomareMovafeghat, malek.fldMalekId, malek.fldTypeContract,malek.fldHoghoghiId,malek.fldSaghfSarmayeGozari, user.UserInputId, jsonstr).FirstOrDefault();
                          //  m.prs_UpdateAcceptContract(z.fldId,true);
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
                        if (Permission.haveAccess(210, "tblContract_Project", malek.fldId.ToString()))
                        {
                            var q = m.prs_tblContract_ProjectUpdate(malek.fldId, malek.fldTitle, Convert.ToInt32(malek.fldTarikhContract.Replace("/", "")), malek.fldShomareContract, TarikhMovafeghat, malek.fldShomareMovafeghat, malek.fldMalekId, malek.fldHoghoghiId,malek.fldSaghfSarmayeGozari, malek.fldTypeContract,  user.UserInputId, malek.fldTimeStamp).FirstOrDefault();

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

                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "Contract_Project:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblContract_Project", jsonstr, false, malek.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblContract_Project", jsonstr, true, malek.fldId);
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
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("عنوان", malek.fldTitle);
                   // parameters.Add("شماره واگن", malek.fldShomareVagon);
                    parameters.Add("شماره قرارداد", malek.fldShomareContract);
                    parameters.Add("تاریخ قرارداد", malek.fldTarikhContract);
                    parameters.Add("شماره موافقتنامه", malek.fldShomareMovafeghat);
                    parameters.Add("تاریخ موافقتنامه", malek.fldTarikhMovafeghat);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "Contract_Project:Save: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (malek.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "dbo.tblContract_Project", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblContract_Project", jsonstr, false, malek.fldId);
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
        public ActionResult SaveVagons(int headerId, string VagonId)
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
                   
                        //if (Permission.haveAccess(209, "tblContract_Project", null))
                        //{
                    m.prs_tblContract_Project_DetailDelete(headerId);
                    var kk = VagonId.Split(',');
                    foreach (var item in kk)
                    {
                        if (item != "")
                            m.prs_tblContract_Project_DetailInsert(headerId, Convert.ToInt32(item));
                    }
                            

                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "دخیره موفق";
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
        public ActionResult SaveDizels(int headerId, string DizelId)
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

                    //if (Permission.haveAccess(209, "tblContract_Project", null))
                    //{
                    m.prs_tblContract_DetailDizelDelete(headerId);
                    var kk = DizelId.Split(',');
                    foreach (var item in kk)
                    {
                        if (item != "")
                            m.prs_tblContract_DetailDizelInsert(headerId, Convert.ToInt32(item),user.UserInputId);
                    }


                    Msg = "ذخیره با موفقیت انجام شد.";
                    MsgTitle = "دخیره موفق";
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
        public ActionResult SaveStations(int headerId, string StationId)
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

                    //if (Permission.haveAccess(209, "tblContract_Project", null))
                    //{
                    m.prs_tblContract_StationDelete(headerId);
                    var kk = StationId.Split(',');
                    foreach (var item in kk)
                    {
                        if (item != "")
                            m.prs_tblContract_StationInsert(headerId, Convert.ToInt32(item));
                    }


                    Msg = "ذخیره با موفقیت انجام شد.";
                    MsgTitle = "دخیره موفق";
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
    }
}
