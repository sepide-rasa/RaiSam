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
    public class PersonalInfoController : Controller
    {
        //
        // GET: /PersonalInfo/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };
            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;
        }
        public ActionResult New(int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.state = state;
            return PartialView;
        }
        public ActionResult Help()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpPersonalInfo()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "6", 1).FirstOrDefault();

            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinPersonalInfo()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoPersonalInfo()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "6", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();
            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getAx(int Id)
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
                var image = "IA==";
            
                    var q = m.prs_tblPersonalInfoSelect("fldId", Id.ToString(),"","",0,1).FirstOrDefault();
                    if (q != null && q.fldfileId != null)
                    {
                        var f = m.prs_tblFileSelect("fldId", q.fldfileId.ToString(),1).FirstOrDefault();
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
                        image = image,
                        fldName_Family = q.fldName_Family,
                        fldFatherName = q.fldFatherName,
                        fldMobile = q.fldMobile,
                        fldCodeEnhesari = q.fldCodeEnhesari,
                        fldCodeMeli = q.fldCodeMeli,
                        TypeEstekhdam = q.TypeEstekhdam,
                        fldDescType = q.fldDescType,
                        fldTitel_MaleSazemani = q.fldTitel_MaleSazemani
                    },
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }


        public ActionResult EditMahalKhedmat(int Id, int MahalSazmaniId, int StationId, string MahalSazmani, bool Movazaff, string Per)
        {//باز شدن تب
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            var nahi = m.prs_tblNahiSelect("fldCode_Personal", MahalSazmaniId.ToString(),1,u.UserSecondId).FirstOrDefault();
            var nahiId = 0;
            if (nahi != null)
                nahiId = nahi.fldCode;
            result.ViewBag.Id = Id;
            result.ViewBag.MahalSazmaniId = MahalSazmaniId;
            result.ViewBag.StationId = StationId;
            result.ViewBag.MahalSazmani = MahalSazmani;
            result.ViewBag.Movazaff = (Convert.ToInt32(Movazaff)).ToString();
            result.ViewBag.NahiId = nahiId;
            result.ViewBag.Per = Per;
            return result;
        }
        public ActionResult GetMahalSazmani()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            var q = m.prs_tblMahalSazmaniSelect("","",u.UserSecondId.ToString(),1).ToList().Select(c => new { ID = c.fldId, Name = c.fldTitle, fldNahiId = c.fldNahiId });
            return this.Store(q);
        }

        //public ActionResult GetStation(int? MahalSazmaniId)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    Models.RaiSamEntities m = new RaiSamEntities();
        //    var nahi = m.prs_tblNahiSelect("fldCode_Personal", MahalSazmaniId.ToString(),1,u.UserSecondId).FirstOrDefault();
           
        //    var q = m.prs_tblStationSelect("Tree", nahi.fldCode.ToString(), u.UserInputId,1).ToList().Select(c => new { ID = c.fldId, Name = c.fldName });
        //    return this.Store(q);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
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
                if (Permission.haveAccess(60,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    List<Models.prs_tblPersonalInfoSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<Models.prs_tblPersonalInfoSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                //case "fldName_Family":
                                //    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                //    field = "fldName_Family";
                                //    break;
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
                                case "fldCodeEnhesari":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldCodeEnhesari";
                                    break;
                                case "fldDescType":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDescType";
                                    break;
                                case "fldTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitle";
                                    break;
                                case "fldDateServer":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDateServer";
                                    break;
                                case "fldStationName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldStationName";
                                    break;
                                case "fldDescPost":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldDescPost";
                                    break;
                                case "fldIsMovazafName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldIsMovazafName";
                                    break;
                                case "fldPayeName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldPayeName";
                                    break;
                                case "fldEmploymentStatusTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldEmploymentStatusTitle";
                                    break;
                                case "fldTitel_MaleSazemani":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitel_MaleSazemani";
                                    break;
                                case "fldSh_Shenasname":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldSh_Shenasname";
                                    break;
                                case "fldTarikhTavalod":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTarikhTavalod";
                                    break;
                                case "fldNameMahalTavalod":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameMahalTavalod";
                                    break;
                                case "fldNameMahalSodoor":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameMahalSodoor";
                                    break;
                                case "TypeEstekhdam":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "TypeEstekhdam";
                                    break;
                                case "fldNameCompany":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameCompany";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblPersonalInfoSelect(field, searchtext,"","",0,200).ToList();
                            else
                                data = m.prs_tblPersonalInfoSelect(field, searchtext, "", "", 0, 200).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblPersonalInfoSelect("", "","","",0,200).ToList();
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

                    List<Models.prs_tblPersonalInfoSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetOnePersonelInfo(string CodeEnhesari)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    if (Request.IsAjaxRequest() == false)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        UserInfo user = (UserInfo)(Session["User"]);
        //        try
        //        {
        //            LocoPersonalService.MedicalPersonalService sv = new LocoPersonalService.MedicalPersonalService();
        //            var kk = sv.GetOnePersonelInfo(CodeEnhesari, user.UserSecondId, user.UserInputId);
        //            return Json(new
        //            {
        //                Msg = kk,
        //                MsgTitle = "پیام",
        //                Er = 0
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception x)
        //        {
        //            string InnerException = "";
        //            if (x.InnerException != null)
        //                InnerException = x.InnerException.Message;
        //            else
        //                InnerException = x.Message;
        //            var error = service.Error(InnerException, user.UserKey, IP);
        //            return Json(new
        //            {
        //                MsgTitle = error.MsgTitle,/*"کد انحصاری وارد شده معتبر نمی باشد."*/
        //                Msg = error.Msg,
        //                Er = 1
        //            });
        //        }
        //    }
        //}
        //public ActionResult GetAllPersonelInfo()
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    var z = 1;
        //    try
        //    {
        //        LocoPersonalService.MedicalPersonalService sv = new LocoPersonalService.MedicalPersonalService();
        //        var kk = sv.GetAllPersonelInfo(user.UserSecondId, user.UserInputId);

        //        //string SessionID = "";
        //        //string XML_Result = "";
        //        //string XML_Result2 = "";
        //        //string argument = "";
        //        //int CNT_Page = 0;
        //        //Service1Client wb = new Service1Client();
        //        //wb.Endpoint.Address = new EndpointAddress("http://172.23.30.160:9517/WebServiceGeneral/WebServiceGenerals.svc");
        //        //argument = "<Arguments><tablename>uv_current_prs_main_info_OccpationalMedicine</tablename></Arguments>";
        //        //XML_Result = argument;
        //        //wb.FirstRequestForStore_Fast("SP_call_all_TableOrView_fast", argument, ref XML_Result, "prs_medicine", "Mm@123456", ref SessionID, ref CNT_Page);
        //        //z = 30+CNT_Page;

        //        //var Path1 = Server.MapPath(@"~\Uploaded\ResultAllPersonelInfo1_" + CNT_Page + ".xml");
        //        //XmlDocument xdoc1 = new XmlDocument();
        //        //xdoc1.LoadXml(XML_Result);
        //        //xdoc1.Save(Path1);

        //        ////CNT_Page = 1;
        //        ////for (int i = 1; i <= CNT_Page; i++)
        //        ////{
        //        //int i = 1;
        //        //    z =10+i;
        //        //    wb.RequestForManualPaging_Fast(ref XML_Result2, "SP_call_all_TableOrView_fast", SessionID, "prs_medicine", "Mm@123456", i);
        //        //    var Path = Server.MapPath(@"~\Uploaded\ResultAllPersonelInfo_" + CNT_Page+"_"+i + ".xml");
        //        //    z = 20 + i;
        //        //    XmlDocument xdoc = new XmlDocument();
        //        //    xdoc.LoadXml(XML_Result2);
        //        //    xdoc.Save(Path);

        //        return Json(new
        //        {
        //            Msg = kk,
        //            MsgTitle = "پیام",
        //            Er = 0
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception x)
        //    {
        //        string InnerException = "";
        //        if (x.InnerException != null)
        //            InnerException = x.InnerException.Message;
        //        else
        //            InnerException = x.Message;
        //        var error = service.Error(InnerException, user.UserKey, IP);
        //        return Json(new
        //        {
        //            MsgTitle = error.MsgTitle,/*"کد انحصاری وارد شده معتبر نمی باشد."*/
        //            Msg = z + "**" + error.Msg,
        //            Er = 1
        //        });
        //    }
        //}
        //public ActionResult GetOneShakhsInfo(string NationalCode, string BirthDate, string Family)
        //{
        //    if (Session["User"] == null)
        //        return RedirectToAction("Logon", "Account", new { area = "" });
        //    UserInfo user = (UserInfo)(Session["User"]);
        //    try
        //    {
        //        //LocoPersonalService.MedicalPersonalService sv = new LocoPersonalService.MedicalPersonalService();
        //        //var kk = sv.GetOnePersonelInfo(CodeEnhesari, user.UserSecondId, user.UserInputId);

        //        RaiClient RaiClient = new RaiClient("https://externalapi.rai.ir");
        //        User AuthorizeUser = new User();
        //        AuthorizeUser.UserName = "TebeKar_Sitaad";
        //        AuthorizeUser.Password = "TebeKar_Sitaad.IT.13990912.583.Op5#d%56Mo09LaW7Vv";

        //        InputDto_SabtAhval InputDto = new InputDto_SabtAhval();
        //        if (BirthDate != "")
        //            Family = "";
        //        InputDto.birthDate = BirthDate.Replace(@"/", string.Empty);
        //        InputDto.nationalCode = NationalCode;
        //        InputDto.family = Family;
        //        InputDto.family = "";
        //        var r = RaiClient.GetInfoAndImage(AuthorizeUser, InputDto, new CancellationToken());
        //        if (r.Status == System.Threading.Tasks.TaskStatus.Faulted)
        //        {
        //            return Json(new
        //            {
        //                Message = "خطا در برقراری ارتباط با وب سرویس",
        //                Er = 1
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            if (r.Result.IsSuccess)
        //            {
        //                if (r.Result.Data.exceptionMessage != null)
        //                {
        //                    if (r.Result.Data.exceptionMessage == "err.record.not.found")
        //                    {
        //                        return Json(new
        //                        {
        //                            Message = "کد ملی و یا تاریخ تولد اشتباه است.",
        //                            Er = 3,
        //                            Msg = "کد ملی و یا تاریخ تولد اشتباه است.",
        //                            MsgTitle = "عملیات موفق"
        //                        });
        //                    }
        //                    else
        //                    {
        //                        var error = service.Error(r.Result.Data.exceptionMessage, user.UserKey, IP);
        //                        return Json(new
        //                        {
        //                            Message = "خطا در زمان استعلام. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید. در غیر این صورت اطلاعات را به صورت دستی وارد نمایید.",
        //                            Er = 1,
        //                            Msg = "خطا در برقراری ارتباط.",
        //                            MsgTitle = "عملیات موفق"
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    var Pic = "";
        //                    byte[] picture = null;
        //                    foreach (var item1 in r.Result.Data.payload.images)
        //                    {
        //                        if (item1.image != null)
        //                        {
        //                            Pic = Convert.ToBase64String(item1.image);
        //                        }
        //                    }

        //                    var shenasname = "";
        //                    if (r.Result.Data.payload.shenasnameNo == "0")
        //                    {
        //                        shenasname = r.Result.Data.payload.nationalCode;
        //                    }
        //                    else
        //                    {
        //                        shenasname = r.Result.Data.payload.shenasnameNo;
        //                    }

        //                    var Gender = 0;
        //                    if (r.Result.Data.payload.gender == "1")
        //                    {
        //                        Gender = 1;
        //                    }
        //                    else
        //                    {
        //                        Gender = 0;
        //                    }

        //                    int mahalSodurId = 0;
        //                    string mahalSodur = "";
        //                    int mahalTavalodId = 0;
        //                    string mahalTavalod = "";
        //                    OBJ_City p = new OBJ_City();
        //                    p.fldInputId = user.UserInputId;
        //                    p.fldName = r.Result.Data.payload.shenasnameIssuePlace;
        //                    p.fldCode = 0;
        //                    var m = service.InsertCity(p, user.UserKey, IP);
        //                    if (m.ErrorType == false)
        //                    {
        //                        mahalSodur = r.Result.Data.payload.shenasnameIssuePlace;
        //                        mahalSodurId = m.OutputId;
        //                    }

        //                    p.fldName = r.Result.Data.payload.birthplace;
        //                    m = service.InsertCity(p, user.UserKey, IP);
        //                    if (m.ErrorType == false)
        //                    {
        //                        mahalTavalod = r.Result.Data.payload.birthplace;
        //                        mahalTavalodId = m.OutputId;
        //                    }

        //                    //return Json(new
        //                    //{
        //                    //    ss = r.Result.Data,
        //                    //     fldName = r.Result.Data.payload.name,
        //                    //    fldFamily = r.Result.Data.payload.family,
        //                    //    fldFatherName = r.Result.Data.payload.fatherName,
        //                    //    shenasnameNo = shenasname,
        //                    //    Gender=Gender.ToString(),
        //                    //    mahalSodurId=mahalSodurId,
        //                    //    mahalSodur=mahalSodur,
        //                    //    mahalTavalodId=mahalTavalodId,
        //                    //    mahalTavalod=mahalTavalod,
        //                    //    Pic = Pic,
        //                    //    Message = "",
        //                    //    Er = 0
        //                    //}, JsonRequestBehavior.AllowGet);
        //                    return Json(new
        //                    {
        //                        Msg = "بارگذاری با موفقیت انجام شد",
        //                        MsgTitle = "عملیات موفق",
        //                        Er = 0
        //                    }, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }
        //        return Json(new
        //        {
        //            Msg = "بارگذاری با موفقیت انجام شد",
        //            MsgTitle = "عملیات موفق",
        //            Er = 0
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception x)
        //    {
        //        string InnerException = "";
        //        if (x.InnerException != null)
        //            InnerException = x.InnerException.Message;
        //        else
        //            InnerException = x.Message;
        //        var error = service.Error(InnerException, user.UserKey, IP);
        //        return Json(new
        //        {
        //            MsgTitle = error.MsgTitle,/*"کد انحصاری وارد شده معتبر نمی باشد."*/
        //            Msg = error.Msg,
        //            Er = 1
        //        });
        //    }

        //}

    }
}
