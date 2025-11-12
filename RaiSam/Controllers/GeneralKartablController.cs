using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using System.IO;
using Aspose.Cells;
using RaiSam.Models;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using System.ComponentModel;
using System.IO.Compression;
using RaiSam.Controllers.Actions;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading;
using System.Xml.Serialization;
using Aspose.Pdf.DOM;

namespace RaiSam.Controllers
{
    public class GeneralKartablController : Controller
    {
        //
        // GET: /GeneralKartabl/
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
        [ActionAttribute("tblTaeedMarhale")]
        [DisplayName("تأیید مرحله")]
        public ActionResult TaiidMarhale(string EnterSicleIds, int EghdamId, string NameCharkhe, string cartableTitle, bool IsGroup, int cartableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();

            
                result.ViewBag.fldNoeGhabelCharkhesh = 1;//درخواست
           

            if (IsGroup == false)
            {
               
                var dd = m.prs_tblEnterToCycleSelect("fldId",EnterSicleIds,1).FirstOrDefault();
                result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
                if (dd.fldAnavinGhabelCharkheshId == 1)
                {
                    result.ViewBag.contractId = dd.fldcontractId.ToString();
                    result.ViewBag.TitleContract = dd.fldTitleContract;
                    result.ViewBag.FullName = dd.fldFullName;
                }
            }

            var e = m.prs_tblActionsSelect("fldId", EghdamId.ToString(),0).FirstOrDefault();

            result.ViewBag.EghdamTitle = e.fldName;
            result.ViewBag.EnterSicleIds = EnterSicleIds;
            result.ViewBag.EghdamId = EghdamId;
            result.ViewBag.NameCharkhe = NameCharkhe;
            result.ViewBag.cartableTitle = cartableTitle;
            result.ViewBag.IsGroup = IsGroup;
            result.ViewBag.cartableId = cartableId;
            return result;
        }
        [InfoActionAttribute("tblTaeedMarhale")]
        public ActionResult TaiidMarhale_Info(string CharkheFirstId, string TableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
         
                UserInfo user = (UserInfo)(Session["User"]);
              
               var dd = m.prs_tblTaeedMarhaleSelect("fldId", TableId,"",0).FirstOrDefault();
           
            var result = new Ext.Net.MVC.PartialViewResult();
            //result.ViewBag.Name = dd.fldName;
            //result.ViewBag.PersonalLocoId = dd.fldpersonalid.ToString();
            //result.ViewBag.AshkhasIdLocoId = dd.fldAshkhasId.ToString();
            result.ViewBag.CharkheFirstId = CharkheFirstId;
            result.ViewBag.Desc = dd.fldDesc;
            result.ViewBag.NameAction = dd.fldNameEghdam;
            result.ViewBag.TitleContract = dd.fldTitleContract;
            return result;
        }
        [ActionAttribute("tblEtmamCharkhe")]
        [DisplayName("اتمام چرخه")]
        public ActionResult EtmamCharkhe(string EnterSicleIds, int EghdamId, string NameCharkhe, string cartableTitle, bool IsGroup, int cartableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();


            result.ViewBag.fldNoeGhabelCharkhesh = 1;//درخواست


            if (IsGroup == false)
            {

                var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
                result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
                if (dd.fldAnavinGhabelCharkheshId == 1)
                {
                    result.ViewBag.contractId = dd.fldcontractId.ToString();
                    result.ViewBag.TitleContract = dd.fldTitleContract;
                    result.ViewBag.FullName = dd.fldFullName;
                }
            }

            var e = m.prs_tblActionsSelect("fldId", EghdamId.ToString(), 0).FirstOrDefault();

            result.ViewBag.EghdamTitle = e.fldName;
            result.ViewBag.EnterSicleIds = EnterSicleIds;
            result.ViewBag.EghdamId = EghdamId;
            result.ViewBag.NameCharkhe = NameCharkhe;
            result.ViewBag.cartableTitle = cartableTitle;
            result.ViewBag.IsGroup = IsGroup;
            result.ViewBag.cartableId = cartableId;
            return result;
        }
        [InfoActionAttribute("tblEtmamCharkhe")]
        public ActionResult EtmamCharkhe_Info(string CharkheFirstId, string TableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            UserInfo user = (UserInfo)(Session["User"]);

            var dd = m.prs_tblEtmamCharkheSelect("fldId", TableId, "", 0).FirstOrDefault();

            var result = new Ext.Net.MVC.PartialViewResult();
            //result.ViewBag.Name = dd.fldName;
            //result.ViewBag.PersonalLocoId = dd.fldpersonalid.ToString();
            //result.ViewBag.AshkhasIdLocoId = dd.fldAshkhasId.ToString();
            result.ViewBag.CharkheFirstId = CharkheFirstId;
            result.ViewBag.Desc = dd.fldDesc;
            result.ViewBag.NameAction = dd.fldNameEghdam;
            return result;
        }
        [ActionAttribute("tblBazgashBeGhabl")]
        [DisplayName("بازگشت به قبل")]
        public ActionResult BazgashtBeGhabl(string EnterSicleIds, int EghdamId/*, int State*/, string NameCharkhe, string cartableTitle, bool IsGroup, int cartableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);

            ViewData.Model = new BazgashtBeGhabl();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ViewData = ViewData
            };

       
                result.ViewBag.fldNoeGhabelCharkhesh = 1;//درخواست

            if (IsGroup == false)
            {
                var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds,1).FirstOrDefault();
                result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
                if (dd.fldAnavinGhabelCharkheshId == 1)
                {
                    result.ViewBag.contractId = dd.fldcontractId.ToString();
                    result.ViewBag.TitleContract = dd.fldTitleContract;
                    result.ViewBag.FullName = dd.fldFullName;
                }
                
            }


            var e = m.prs_tblActionsSelect("fldId", EghdamId.ToString(),0).FirstOrDefault();

            result.ViewBag.EghdamTitle = e.fldName;
            result.ViewBag.EnterSicleIds = EnterSicleIds;
            var kk = EnterSicleIds.Split(';');
            result.ViewBag.OneEnterSicleId = kk[0];
            result.ViewBag.EghdamId = EghdamId;
            result.ViewBag.State = 1;/*بازگشت به اقدامت قبلی:1          بازگشت به اقدام قبل:2*/
            result.ViewBag.NameCharkhe = NameCharkhe;
            result.ViewBag.cartableTitle = cartableTitle;
            result.ViewBag.IsGroup = IsGroup;
            result.ViewBag.cartableId = cartableId;
            return result;
        }
        [InfoActionAttribute("tblBazgashBeGhabl")]
        public ActionResult BazgashtBeGhabl_Info(string CharkheFirstId, string TableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            
                UserInfo user = (UserInfo)(Session["User"]);
                /*param.FieldName = "fldCharkheFirstId";
                param.Value = CharkheFirstId;*/
              
                var dd = m.prs_tblBazgashBeGhablSelect("fldId", TableId,"",0).FirstOrDefault();
           
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.TitleContract = dd.fldTitleContract;
            result.ViewBag.contractId = dd.fldcontractId.ToString();
            result.ViewBag.CharkheFirstId = CharkheFirstId;
            result.ViewBag.Desc = dd.fldDesc;
            result.ViewBag.NameAction = dd.fldNameAction;
            return result;
        }
        [ActionAttribute("tblBazgashBeGhabl")]
        [DisplayName("بازگشت به مرحله قبل")]
        public ActionResult BazgashtBeMarhaleGhabl(string EnterSicleIds, int EghdamId/*, int State*/, string NameCharkhe, string cartableTitle, bool IsGroup, int cartableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);

            ViewData.Model = new BazgashtBeGhabl();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ViewData = ViewData
            };


            result.ViewBag.fldNoeGhabelCharkhesh = 1;//درخواست

            if (IsGroup == false)
            {
                var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
                result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
                if (dd.fldAnavinGhabelCharkheshId == 1)
                {
                    result.ViewBag.contractId = dd.fldcontractId.ToString();
                    result.ViewBag.TitleContract = dd.fldTitleContract;
                    result.ViewBag.FullName = dd.fldFullName;
                }

            }


            var e = m.prs_tblActionsSelect("fldId", EghdamId.ToString(), 0).FirstOrDefault();

            result.ViewBag.EghdamTitle = e.fldName;
            result.ViewBag.EnterSicleIds = EnterSicleIds;
            var kk = EnterSicleIds.Split(';');
            result.ViewBag.OneEnterSicleId = kk[0];
            result.ViewBag.EghdamId = EghdamId;
            result.ViewBag.State = 2;/*بازگشت به اقدامت قبلی:1          بازگشت به اقدام قبل:2*/
            result.ViewBag.NameCharkhe = NameCharkhe;
            result.ViewBag.cartableTitle = cartableTitle;
            result.ViewBag.IsGroup = IsGroup;
            result.ViewBag.cartableId = cartableId;
            return result;
        }
        [InfoActionAttribute("tblBazgashBeGhabl")]
        public ActionResult BazgashtBeMarhaleGhabl_Info(string CharkheFirstId, string TableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();


            UserInfo user = (UserInfo)(Session["User"]);
            /*param.FieldName = "fldCharkheFirstId";
            param.Value = CharkheFirstId;*/

            var dd = m.prs_tblBazgashBeGhablSelect("fldId", TableId, "", 0).FirstOrDefault();

            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.TitleContract = dd.fldTitleContract;
            result.ViewBag.contractId = dd.fldcontractId.ToString();
            result.ViewBag.CharkheFirstId = CharkheFirstId;
            result.ViewBag.Desc = dd.fldDesc;
            result.ViewBag.NameAction = dd.fldNameAction;
            return result;
        }
        [ActionAttribute("tblAcceptOrReject")]
        [DisplayName("تأیید/عدم تأیید")]
        public ActionResult AcceptOrReject(string EnterSicleIds, int EghdamId, string NameCharkhe, string cartableTitle, bool IsGroup, int cartableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);

                result.ViewBag.fldNoeGhabelCharkhesh = 1;//درخواست
           

            if (IsGroup == false)
            {
                var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds,1).FirstOrDefault();
                result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
                  if (dd.fldAnavinGhabelCharkheshId == 1)
                  {
                      result.ViewBag.contractId = dd.fldcontractId.ToString();
                      result.ViewBag.TitleContract = dd.fldTitleContract;
                      result.ViewBag.FullName = dd.fldFullName;
                }
            }

             var e = m.prs_tblActionsSelect("fldId", EghdamId.ToString(),0).FirstOrDefault();

            result.ViewBag.EghdamTitle = e.fldName;
            result.ViewBag.EnterSicleIds = EnterSicleIds;
            result.ViewBag.EghdamId = EghdamId;
            result.ViewBag.NameCharkhe = NameCharkhe;
            result.ViewBag.cartableTitle = cartableTitle;
            result.ViewBag.IsGroup = IsGroup;
            result.ViewBag.cartableId = cartableId;
            return result;
        }
        [InfoActionAttribute("tblAcceptOrReject")]
        public ActionResult AcceptOrReject_Info(string CharkheFirstId, string TableId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
           
                UserInfo user = (UserInfo)(Session["User"]);
                /*param.FieldName = "fldCharkheFirstId";
                param.Value = CharkheFirstId;*/
             
                var dd = m.prs_tblAcceptOrRejectSelect("fldId", TableId,"",0).FirstOrDefault();


                var result = new Ext.Net.MVC.PartialViewResult();
                result.ViewBag.TitleContract = dd.fldTitleContract;
                result.ViewBag.contractId = dd.fldcontractId.ToString();
            result.ViewBag.CharkheFirstId = CharkheFirstId;
            result.ViewBag.Desc = dd.fldDesc;
            result.ViewBag.TypeName = dd.fldTypeName;
            result.ViewBag.NameAction = dd.fldNameEghdam;
            return result;
        }
        [ActionAttribute("tblScheduleContract_Info")]
        [DisplayName("مشاهده زمانبندی")]
        public ActionResult ShowScheduleContract(string EnterSicleIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
          
                    var HaveTaeed = false;

                var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
                result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
                if (dd.fldAnavinGhabelCharkheshId == 1)
                {
                    result.ViewBag.contractId = dd.fldcontractId.ToString();
                    result.ViewBag.TitleContract = dd.fldTitleContract;
                    result.ViewBag.FullName = dd.fldFullName;

                    var k = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
                        HaveTaeed = m.prs_tblTaeedItemsSelect("fldHadafId", k.fldHadafId.ToString(), 0).Where(l => l.fldApp_ClientId == 11).Any();
                        var s = m.prs_tblScheduleContract_HeaderSelect("fldFirstContractId", dd.fldcontractId.ToString(), 0).FirstOrDefault();

                        result.ViewBag.ScheduleHeader = s.fldId;
                }
                result.ViewBag.HaveTaeed = HaveTaeed;
                return result;
         
        }
       
        [ActionAttribute("tblShareholder")]
        [DisplayName("مشاهده سهامداران")]
        public ActionResult ShowShareholders(string EnterSicleIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();


            var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
            result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
            if (dd.fldAnavinGhabelCharkheshId == 1)
            {
                result.ViewBag.contractId = dd.fldcontractId.ToString();
                result.ViewBag.TitleContract = dd.fldTitleContract;
                result.ViewBag.FullName = dd.fldFullName;
            }
            var k = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
            result.ViewBag.FirstRegisterId = k.fldFirstRegisterId;

            result.ViewBag.EnterSicleIds = EnterSicleIds;
            return result;

        }
        [ActionAttribute("tblRegistrationFirstContract")]
        [DisplayName("مشاهده پروژه")]
        public ActionResult ShowProject(string EnterSicleIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            ViewData.Model = new Models.ProjectDetails();
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ViewData = ViewData
            };
            Models.RaiSamEntities m = new RaiSamEntities();

            var HaveTaeed = false;

            var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
            result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
            if (dd.fldAnavinGhabelCharkheshId == 1)
            {
                result.ViewBag.contractId = dd.fldcontractId.ToString();
                result.ViewBag.TitleContract = dd.fldTitleContract;
                result.ViewBag.FullName = dd.fldFullName;

                var k = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
                HaveTaeed = m.prs_tblTaeedItemsSelect("fldHadafId", k.fldHadafId.ToString(), 0).Where(l => l.fldApp_ClientId == 11).Any();
                   
            }
            result.ViewBag.EnterSicleIds = EnterSicleIds;
            result.ViewBag.HaveTaeed = HaveTaeed;
            return result;

        }
        [ActionAttribute("tblCompanyProfile")]
        [DisplayName("مشاهده اطلاعات شرکت")]
        public ActionResult ShowCompanyProfile(string EnterSicleIds)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();

            var HaveTaeed = false;

            var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
            result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
            if (dd.fldAnavinGhabelCharkheshId == 1)
            {
                result.ViewBag.contractId = dd.fldcontractId.ToString();
                result.ViewBag.TitleContract = dd.fldTitleContract;
                result.ViewBag.FullName = dd.fldFullName;

                var kk = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
                HaveTaeed = m.prs_tblTaeedItemsSelect("fldHadafId", kk.fldHadafId.ToString(), 0).Where(l => l.fldApp_ClientId == 11).Any();
                   
            }

            var k = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
            var r = m.prs_tblRequestRankingSelect("fldId", k.fldRequestId.ToString(), 0).FirstOrDefault();
            result.ViewBag.CompanyProfileId = r.fldComanyId;
            result.ViewBag.HaveTaeed = HaveTaeed;
            return result;

        }
        [ActionAttribute("tblItemsDynamicRating")]
        [DisplayName("مشاهده مدارک داینامیک")]
        public ActionResult ShowDynamicForm(string EnterSicleIds, int GeneralRatingId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            UserInfo user = (UserInfo)(Session["User"]);
            var result = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();


            var dd = m.prs_tblEnterToCycleSelect("fldId", EnterSicleIds, 1).FirstOrDefault();
            result.ViewBag.fldNoeGhabelCharkhesh = dd.fldAnavinGhabelCharkheshId;
            if (dd.fldAnavinGhabelCharkheshId == 1)
            {
                result.ViewBag.contractId = dd.fldcontractId.ToString();
                result.ViewBag.TitleContract = dd.fldTitleContract;
                result.ViewBag.FullName = dd.fldFullName;
            }

            var State = GeneralRatingId;
            string TitleRatingDynamicIds = "";
            var k = m.prs_tblRegistrationFirstContractSelect("fldId", dd.fldcontractId.ToString(), 0).FirstOrDefault();
            var r = m.prs_tblRequestRankingSelect("fldId", k.fldRequestId.ToString(), 0).FirstOrDefault();
            var q = m.prs_tblItemsDynamicRatingSelect("fldHadafId", r.fldHadafId.ToString(), 0).Where(l => l.fldGeneralRatingId == State).ToList();
            if (q.Count != 0)
            {
                foreach (var item in q)
                {
                    TitleRatingDynamicIds = TitleRatingDynamicIds + item.fldDynamicRatingId + ";";
                    //RatingIds = RatingIds + item.fldId + ";";
                }
                result.ViewBag.TitleRatingDynamicIds = TitleRatingDynamicIds;
            }
            result.ViewBag.State = State;
            result.ViewBag.RequestId = k.fldRequestId;
            result.ViewBag.EnterSicleIds = EnterSicleIds;

            return result;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadEghdamatGhabli(StoreRequestParameters parameters, int OneEnterSicleId)
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
                UserInfo user = (UserInfo)(Session["User"]);
                List<Models.prs_SelectRequest_Action> data = null;
                data = m.prs_SelectRequest_Action(OneEnterSicleId).ToList();

                return this.Store(data, data.Count);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCycleLoc(StoreRequestParameters parameters, int EghdamId, int CartableId, int CompanyId)
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
                SqlConnection con = new SqlConnection();
                if (Permission.haveAccess(164, "", "0"))
                {
                    try
                    {
                        string Condition = "";

                        
                        //if (PostId != 0)
                        //{
                        //    Condition = Condition + " and fldOrganId in (select d.fldOrganPostId from  tblShoghleAmaliyati_Detail as d where d.fldHeaderId=" + PostId + ")";
                        //}

                        string commandText = "SELECT top(500) * FROM " +
                        " (SELECT tblEnterToCycle.fldId AS fldIdVorood, tblEnterToCycle.fldExternalId as fldContractId, tblEnterToCycle.fldCharkheId,  " +
                        " dbo.fn_OprtionIdByCharkhe('Charkhe',fldcharkheid,tblEnterToCycle.fldId,0)as DefaultOperationId,       " +
                        " dbo.fn_OprtionIdByCharkhe('Charkhe_Eghdam',fldcharkheid,tblEnterToCycle.fldId, "+  EghdamId +"     )as DefaultOperation_CharkheEghdam,    " +
                        " tblCharkhe.fldName AS fldNameCharkhe,  tblCharkhe_FirstEghdam.fldActionId, fldTarikhVorood, fldCount,    " +
                        " tblActions.fldName AS fldNameAction,h.fldTitle as fldTitleHadaf,fldRequestId,[fldTypeVagon]+'('+isnull(cast(fldMehvar as varchar(1)),'0')+N'محور)' as fldTypeVagonName " +
                        " ,cp.fldTitle as fldTitleContract,cp.fldTedad as fldTedad " +
                        " ,ah.fldName as fldFullName,format(fldTarikhDarkhast,'####/##/##')fldTarikhDarkhast,h.fldId as fldHadafId " +
                        " FROM tblEnterToCycle       " +
                        " INNER JOIN tblCharkhe ON tblEnterToCycle.fldCharkheId = tblCharkhe.fldId     " +
                        " INNER JOIN tblCharkhe_FirstEghdam ON tblEnterToCycle.fldId = tblCharkhe_FirstEghdam.fldEnterCycleId 	  " +
                        " INNER JOIN  tblActions ON tblCharkhe_FirstEghdam.fldActionId = tblActions.fldId      " +
                        " inner join tblRegistrationFirstContract cp  on fldExternalId=cp.fldid    " +
                        " inner join tblTypeVagon t on t.fldid=fldTypeVagonId "+
                        " LEFT JOIN dbo.tblRequestRanking r ON cp.fldRequestId = r.fldId     " +
                        " left join tblFirstRegister_HadafAllowed f1 on f1.fldid=r.fldHadafIdAllowed " +
                        " left join tblHadafSabtNam h on h.fldid=fldHadafId " +
                        " left join tblFirstRegister fi on fi.fldid=r.fldFirstRegisterId " +
                        " left join tblCompanyProfile co on co.fldFirstRegisterUser=fi.fldid " +
                        " left join tblAshkhasHoghooghi ah on ah.fldid=co.fldAshkhasHoghoghiId " +
                        " cross apply ( " +
                                    " 	select count(*)fldCount " +
                                    " 	from tblCharkhe_FirstEghdam f  " +
                                    " 	where f.fldEnterCycleId=tblEnterToCycle.fldId and fldActionId= " +   EghdamId   +  
                                " )TedadAction   " +
                       " outer apply( " +
                                    " select 1 as IsArchive  " +
                                    " FROM tblEtmamCharkhe  " +
                                    " inner JOIN     tblCharkhe_FirstEghdam ON tblEtmamCharkhe.fldCharkheFirstId = tblCharkhe_FirstEghdam.fldId   " +
                                    " inner join     tblEnterToCycle ON tblCharkhe_FirstEghdam.fldEnterCycleId = tblEnterToCycle.fldId       " +
                                    " where tblEnterToCycle.fldExternalId=cp.fldid and fldAnavinGhabelCharkheshId=1    " +
                                " ) tblEnter       " +
                     " cross apply ( " +
                                " select dbo.Fn_AssembelyMiladiToShamsi(t2.StartTime)+' '+ CAST(CAST(t2.StartTime AS time(0))AS VARCHAR(8)) AS fldTarikhVorood       " +
                                " from ( " +
                                " 	    select top(1) CONVERT(datetime,SWITCHOFFSET(CONVERT(datetimeoffset,c.[StartTime]),DATENAME(TzOffset, SYSDATETIMEOFFSET()))) StartTime       " +
                                " 		from tblCharkhe_FirstEghdam c  " +
                                " 		where c.fldEnterCycleId=tblEnterToCycle.fldId and fldActionId= "+  EghdamId  +
                                " 		order by c.fldId desc " +
                                " 	)t2 " +
                                " ) tblChare_First       " +
                     " cross apply  ( " +
                                    " select top(1)  c.fldActionId  " +
                                    " from tblCharkhe_FirstEghdam c  " +
                                    " where c.fldEnterCycleId=tblEnterToCycle.fldId   " +
                                    " order by c.fldId desc " +
                                " ) tblChare_First_last    " +
                        " where fldFinal_Sabt=1 and  fldAnavinGhabelCharkheshId=1 and  tblEnterToCycle.fldCharkheId in(select fldCharkheId from tblEkhtesasActions_Cartable inner join tblCharkhe_Eghdam on     " +
                        " tblCharkhe_Eghdam.fldId=tblEkhtesasActions_Cartable.fldCharkhe_EghdamId  where fldEghdamId= "+   EghdamId  +"     and fldCartableId=  "+    CartableId  +"   )      " +
                 Condition + "   and tblCharkhe_FirstEghdam.fldActionId= " + EghdamId + " and tblChare_First_last.fldActionId =" + EghdamId + "     )t			  " +
                        " group by  fldIdVorood,  fldContractId, fldCharkheId,  DefaultOperationId, DefaultOperation_CharkheEghdam,   " +
                        " fldNameCharkhe, fldActionId, fldTarikhVorood, fldCount, fldNameAction,fldTitleHadaf,fldRequestId,fldTypeVagonName " +
                       " ,fldTitleContract,fldTedad,fldFullName,fldTarikhDarkhast, fldHadafId " +
                        " ORDER BY fldFullName  ";

                        List<Models.ContractCycle> Inf = new List<Models.ContractCycle>();
                        string ConnectionString = ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString;
                        con = new SqlConnection(ConnectionString);
                        con.Open();
                        SqlCommand com = new SqlCommand(commandText, con);
                        com.CommandTimeout = 200000;
                        var dr = com.ExecuteReader();
                        while (dr.Read())
                        {
                            Inf.Add(new Models.ContractCycle
                            {
                                fldIdVorood = Convert.ToInt32(dr["fldIdVorood"]),
                                fldContractId = Convert.ToInt32(dr["fldContractId"]),
                                fldCharkheId = Convert.ToInt32(dr["fldCharkheId"]),
                                DefaultOperationId = Convert.ToInt32(dr["DefaultOperationId"]),
                                DefaultOperation_CharkheEghdam = Convert.ToInt32(dr["DefaultOperation_CharkheEghdam"]),
                                fldNameCharkhe = dr["fldNameCharkhe"].ToString(),
                                fldActionId = Convert.ToInt32(dr["fldActionId"]),
                                fldTarikhVorood = dr["fldTarikhVorood"].ToString(),
                                fldCount = Convert.ToInt32(dr["fldCount"]),
                                fldNameAction = dr["fldNameAction"].ToString(),
                                fldTitleHadaf = dr["fldTitleHadaf"].ToString(),
                                fldRequestId = Convert.ToInt32(dr["fldRequestId"]),
                                fldTypeVagonName = dr["fldTypeVagonName"].ToString(),
                                fldTitleContract = dr["fldTitleContract"].ToString(),
                                fldTedad = Convert.ToInt32(dr["fldTedad"]),
                                fldFullName = dr["fldFullName"].ToString(),
                                fldTarikhDarkhast = dr["fldTarikhDarkhast"].ToString(),
                                fldHadafId = Convert.ToInt32(dr["fldHadafId"])
                            });
                        }
                        con.Close();
                        return new JsonResult()
                        {
                            Data = new
                            {
                                Er = 0,
                                ListLoc = Inf
                            },
                            MaxJsonLength = Int32.MaxValue,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    catch (Exception x)
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                        var ErMsg = "";
                        if (x.InnerException != null)
                            ErMsg = x.InnerException.Message;
                        else
                            ErMsg = x.Message;
                        System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                        var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                        m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                        return Json(new
                        {
                            MsgTitle = "خطا",
                            Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                            Er = 1
                        });
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GotoLastOperation(int EntertoCycleId, int? Operation_ActionId)
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
                try
                {
                    var tableName = "";
                    string Id = "0";
                    byte State = 1;
                    int? OperationId = null;
                    if (Operation_ActionId != null)
                    {
                        var q = m.prs_tblOpertion_ActionSelect("fldId", Operation_ActionId.ToString(),"","",1).FirstOrDefault();

                        tableName = q.fldNameTable;
                        Id = EntertoCycleId + ";" + q.fldActionId;
                        OperationId = q.fldOpertionId;
                        State = 2;
                    }
                    else
                    {
                        var q = m.prs_SelectLastOpration_EnterCycleId(EntertoCycleId).FirstOrDefault();
                        tableName = q.fldNameTable;
                        Id = q.fldCharkheFirstId.ToString();
                    }
                    switch (tableName)
                    {
                        case "tblAcceptOrReject":
                            Server.TransferRequest("/GeneralKartabl/WinAcceptOrRejec?Id=" + Id + "&State=" + State);
                            break;
                        case "tblBazgashBeGhabl":
                            Server.TransferRequest("/GeneralKartabl/WinBazgashtBeGhabl?Id=" + Id + "&State=" + State);
                            break;
                        case "tblTaeedMarhale":
                            Server.TransferRequest("/GeneralKartabl/WinTaeedMarhale?Id=" + Id + "&State=" + State);
                            break;
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return null;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetShareOperation(int CartableId, int ActionId, string CharkheIds)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);
                List<int> OPId = new List<int>();
                var q = m.prs_SelectEshterakOpration(ActionId, CharkheIds, user.UserSecondId).ToList();
                foreach (var item in q)
                {
                    OPId.Add(item.fldOpertionId);
                }
                return Json(new
                {
                    OPId = OPId
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetKartabl()
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
                string Msg = ""; string MsgTitle = ""; byte Er = 0;
                int[] KartablId = new int[1];
                string[] KartablName = new string[1];
                string[] KartablIcon = new string[1];

                try
                {
                    var q = m.prs_tblKartablSelect("Permission", user.UserSecondId.ToString(),0).ToList();

                    KartablId = new int[q.Count()];
                    KartablName = new string[q.Count()];
                    KartablIcon = new string[q.Count()];
                    var Icon = "";
                    for (int j = 0; j < q.Count(); j++)
                    {
                        KartablId[j] = q[j].fldId;
                        KartablName[j] = q[j].fldName;
                        if (q[j].fldFile != null)
                        {
                            Icon = Convert.ToBase64String(q[j].fldFile);
                        }
                        KartablIcon[j] = Icon;
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    KartablId = KartablId,
                    KartablName = KartablName,
                    KartablIcon = KartablIcon,
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEghdam(int KartablId)
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
                UserInfo user = (UserInfo)(Session["User"]);
                string Msg = ""; string MsgTitle = ""; byte Er = 0;
                int[] EghdamId = new int[1];
                string[] EghdamName = new string[1];
                string[] EghdamIcon = new string[1];

                try
                {
                    var q = m.prs_SelectDistictAction_Cartable(KartablId, user.UserSecondId,0).ToList();

                    EghdamId = new int[q.Count()];
                    EghdamName = new string[q.Count()];
                    EghdamIcon = new string[q.Count()];

                    for (int j = 0; j < q.Count(); j++)
                    {
                        var Icon = "";
                        EghdamId[j] = q[j].fldActionsId;
                        if (q[j].fldCount == 0)
                        {
                            EghdamName[j] = q[j].fldName_Actions;
                        }
                        else
                        {
                            EghdamName[j] = q[j].fldName_Actions + " (" + q[j].fldCount + ")";
                        }
                        if (q[j].fldIconAction != null)
                        {
                            Icon = Convert.ToBase64String(q[j].fldIconAction);
                        }
                        EghdamIcon[j] = Icon;
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    EghdamId = EghdamId,
                    EghdamName = EghdamName,
                    EghdamIcon = EghdamIcon,
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAction(int IdEghdam, int CartableId)
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
                UserInfo user = (UserInfo)(Session["User"]);
                string Msg = ""; string MsgTitle = ""; byte Er = 0;
                int[] OperationId = new int[1];
                string[] OperationName = new string[1];
                string[] OperationMethodName = new string[1];
                string[] OperationIcon = new string[1];
                string[] OperationDesc = new string[1];
                bool[] OperationEffect = new bool[1];
                bool[] OperationGroup = new bool[1];

                try
                {
                    var q = m.prs_SelectDistictOperation_Action(IdEghdam, user.UserSecondId,0).ToList();

                    OperationId = new int[q.Count()];
                    OperationName = new string[q.Count()];
                    OperationMethodName = new string[q.Count()];
                    OperationIcon = new string[q.Count()];
                    OperationDesc = new string[q.Count()];
                    OperationEffect = new bool[q.Count()];
                    OperationGroup = new bool[q.Count()];

                    for (int j = 0; j < q.Count(); j++)
                    {
                        var Icon = "";
                        OperationId[j] = q[j].fldOpertionId;
                        OperationName[j] = q[j].NameOperation;
                        if (q[j].fldIsDynamic == true)
                            OperationMethodName[j] = "MoayenatDynamic";
                        else
                            OperationMethodName[j] = q[j].fldMethodName;
                        OperationEffect[j] = Convert.ToBoolean(q[j].fldEffective);
                        OperationGroup[j] = q[j].fldGroup;
                        OperationDesc[j] = q[j].fldDescAction;
                        if (q[j].fldFileOperation != null)
                        {
                            Icon = Convert.ToBase64String(q[j].fldFileOperation);
                        }
                        OperationIcon[j] = Icon;
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    OperationId = OperationId,
                    OperationName = OperationName,
                    OperationMethodName = OperationMethodName,
                    OperationEffect = OperationEffect,
                    OperationIcon = OperationIcon,
                    OperationDesc = OperationDesc,
                    OperationGroup = OperationGroup,
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReloadCountTree(int CartablId, string EghdamIds)
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
                UserInfo user = (UserInfo)(Session["User"]);
                var NewActionName = "";
                try
                {
                    var EghdamArray = EghdamIds.Split(';').Reverse().Skip(1).Reverse();
                    foreach (var item in EghdamArray)
                    {
                        var q = m.prs_SelectDistictAction_Cartable(CartablId, user.UserSecondId,0).Where(l => l.fldActionsId == Convert.ToInt32(item)).FirstOrDefault();
                        if (q.fldCount != 0)
                        {
                            NewActionName = NewActionName + q.fldName_Actions + " (" + q.fldCount + ")" + ";";
                        }
                        else
                        {
                            NewActionName = NewActionName + q.fldName_Actions + ";";
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
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    NewActionName = NewActionName,
                    Er = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult WinBazgashtBeGhabl(string Id, byte State)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            PartialView.ViewBag.State = State;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadBazgashtBeGhablL(StoreRequestParameters parameters, string Id, byte State)
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
                UserInfo user = (UserInfo)(Session["User"]);
                List<Models.prs_tblBazgashBeGhablSelect> data = null;
                var FieldName = "";
                var val = "";
                var val2 = "";
                if (State == 1)
                {
                    FieldName = "fldCharkheFirstId";
                    val = Id;
                }
                else
                {
                    FieldName = "fldEnterCycleId";
                    val = Id.Split(';')[0];
                    val2 = Id.Split(';')[1];
                }

                data = m.prs_tblBazgashBeGhablSelect(FieldName, val, val2, 0).ToList();
                //-- start paging ------------------------------------------------------------
                int limit = parameters.Limit;

                if ((parameters.Start + parameters.Limit) > data.Count)
                {
                    limit = data.Count - parameters.Start;
                }

                List<Models.prs_tblBazgashBeGhablSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
        }
        public ActionResult WinTaeedMarhale(string Id, byte State)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.Id = Id;
            PartialView.ViewBag.State = State;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadTaeedMarhale(StoreRequestParameters parameters, string Id, byte State)
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
                UserInfo user = (UserInfo)(Session["User"]);
                List<Models.prs_tblTaeedMarhaleSelect> data = null;
                var FieldName = "";
                var val = "";
                var val2 = "";
                if (State == 1)
                {
                    FieldName = "fldCharkheFirstId";
                    val = Id;
                }
                else
                {
                    FieldName = "fldEnterCycleId";
                    val = Id.Split(';')[0];
                    val2 = Id.Split(';')[1];
                }
                data = m.prs_tblTaeedMarhaleSelect(FieldName, val, val2,0).ToList();
                //-- start paging ------------------------------------------------------------
                int limit = parameters.Limit;

                if ((parameters.Start + parameters.Limit) > data.Count)
                {
                    limit = data.Count - parameters.Start;
                }

                List<Models.prs_tblTaeedMarhaleSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
        }
        public ActionResult ReloadScheduleContract(string Proj)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
            Models.RaiSamEntities m = new RaiSamEntities();

            List<prs_SelectScheduleTitleByFirstContract> data = null;
            data = m.prs_SelectScheduleTitleByFirstContract(Convert.ToInt32(Proj)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadShareholder(StoreRequestParameters parameters, int contractId)
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
                UserInfo user = (UserInfo)(Session["User"]);
                var k = m.prs_tblRegistrationFirstContractSelect("fldId", contractId.ToString(), 0).FirstOrDefault();
                List<Models.prs_tblShareholderSelect> data = null;
                data = m.prs_tblShareholderSelect("fldFirstRegisterId", k.fldFirstRegisterId.ToString(), "", "", 0).ToList();

                return this.Store(data, data.Count);
            }
        }
        public ActionResult GetInfoGridDynamicForm(string fldTitleDynamicId, string RequestId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            string Msg = ""; string MsgTitle = ""; byte Er = 0;
            string NamesKhasiyat_Fa = "";
            string NamesKhasiyat_En = "";
            string JenseKhasiyat = "";
            string NoeKhasiyatha = "";
            string ItemPropertiesId = "";
            string TitleDynamic = "";
            string ItemsDynamicRatingId = "";
            string IsInClient = "1";

            Models.RaiSamEntities m = new Models.RaiSamEntities();
            var r = m.prs_tblRequestRankingSelect("fldId", RequestId, 0).FirstOrDefault();
            try
            {
                var q = m.prs_tblItemDynamicPropertiesSelect("fldTitleDynamicId", fldTitleDynamicId, r.fldHadafId.ToString(), 0).ToList();

                if (q.Count != 0)
                {
                    TitleDynamic = q[0].fldTitleRatingFA;
                    ItemsDynamicRatingId = q[0].fldItemsDynamicRatingId.ToString();
                    //  IsInClient = q[0].fldIsInClient.ToString();

                    foreach (var item in q)
                    {
                        NamesKhasiyat_Fa = NamesKhasiyat_Fa + item.fldNameKhasiyat_Fa + ";";
                        NamesKhasiyat_En = NamesKhasiyat_En + item.fldNameKhasiyat_En + item.fldItemsDynamicRatingId + ";";
                        ItemPropertiesId = ItemPropertiesId + item.fldId + ";";
                        NoeKhasiyatha = NoeKhasiyatha + item.fldStringType + ";";
                        JenseKhasiyat = JenseKhasiyat + item.fldJenseKhasiyat + ";";
                    }
                }
                else
                {
                    Er = 1;
                    MsgTitle = "خطا";
                    Msg = "مدارک و مستنداتی برای بارگذاری ثبت نشده است.";
                    return Json(new
                    {
                        Msg = Msg,
                        MsgTitle = MsgTitle,
                        Er = Er
                    }, JsonRequestBehavior.AllowGet);
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
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Er = Er,
                NamesKhasiyat_Fa = NamesKhasiyat_Fa,
                NamesKhasiyat_En = NamesKhasiyat_En,
                JenseKhasiyat = JenseKhasiyat,
                ItemPropertiesId = ItemPropertiesId,
                NoeKhasiyatha = NoeKhasiyatha,
                TitleDynamic = TitleDynamic,
                ItemsDynamicRatingId = ItemsDynamicRatingId,
                IsInClient = IsInClient
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadDynamicForm(string fldItemsDynamicRatingId, string RequestId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new Models.RaiSamEntities();

            IDictionary<string, object> aa = new Dictionary<string, object>();
            /*List<object> aa = new List<object>();*/

            List<IDictionary<string, object>> qq = new List<IDictionary<string, object>>();

            // var q = servic.GetItemDynamicPropertiesWithFilter("fldTitleDynamicId", idddd, Session["TreeId"].ToString(), 0, out Err).ToList();

            var data = m.prs_tblItemDynamic_ClientSelect("fldItemDynamicId", fldItemsDynamicRatingId, RequestId, 0).ToList();

            if (data.Count != 0)
            {
                foreach (var item in data)
                {
                    aa = new Dictionary<string, object>();
                    var items = m.prs_tblItemProperties_ClientSelect("fldDynamicClientId", item.fldId.ToString(), 0).ToList();
                    var Idname = "fldId" + fldItemsDynamicRatingId;
                    var Statusname = "fldStatus" + fldItemsDynamicRatingId;
                    var IsInClientname = "fldIsInClient" + fldItemsDynamicRatingId;

                    if (items.Count != 0)
                    {
                        aa.Add(Idname, items[0].fldDynamicClientId);
                        aa.Add(Statusname, item.fldStatus);
                        //aa.Add(IsInClientname, item.fldIsInClient);
                        foreach (var h in items)
                        {
                            var name = h.fldNameKhasiyat_En + fldItemsDynamicRatingId;
                            //var x = h.fldMeghdar.Replace(Convert.ToChar((byte)0x01), ' ');
                            aa.Add(name, h.fldMeghdar);
                        }
                        qq.Add(aa);
                    }
                }
            }
            return this.Store(qq);
        }
        public ActionResult TaeedWin(int App_ClientId, int TableId, int ContractId, string CodeFileId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.App_ClientId = App_ClientId;
            PartialView.ViewBag.TableId = TableId;
            PartialView.ViewBag.ContractId = ContractId;
            PartialView.ViewBag.CodeFileId = CodeFileId;
            return PartialView;
        }
        public ActionResult HistoryTaeedWin(int App_ClientId, int TableId, int ContractId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });

            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.App_ClientId = App_ClientId;
            PartialView.ViewBag.TableId = TableId;
            PartialView.ViewBag.ContractId = ContractId;
            return PartialView;
        }
        public ActionResult ShowMsgTaeed(string Message, int fldId, int App_Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
           // m.prs_UpdateSeenTaeedat(fldId, true);
            PartialView.ViewBag.Message = Message;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadHistoryTaeed(StoreRequestParameters parameters, int App_ClientId, int TableId, int ContractId)
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
                UserInfo user = (UserInfo)(Session["User"]);
                List<Models.prs_tblTaeedatSelect> data = null;

                data = m.prs_tblTaeedatSelect("App_Contract", App_ClientId.ToString(), ContractId.ToString(), TableId.ToString(), 0).ToList();
                //-- start paging ------------------------------------------------------------
                int limit = parameters.Limit;

                if ((parameters.Start + parameters.Limit) > data.Count)
                {
                    limit = data.Count - parameters.Start;
                }

                List<Models.prs_tblTaeedatSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTaeedat(prs_tblTaeedatSelect Taeed, string CodeFileId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            string Msg = "ذخیره با موفقیت انجام شد.", MsgTitle = "ذخیره موفق"; var Er = 0;

            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                  

                m.prs_tblTaeedatInsert(Taeed.fldType, Taeed.fldApp_ClientId, Taeed.fldFirstContractId, Taeed.fldTableId, u.UserInputId, Taeed.fldDesc);


                var uu = m.prs_tblUserSelect("fldId", u.UserSecondId.ToString(), "", 0).FirstOrDefault();
                var s = m.prs_tblSign_StampDigitalSelect("fldAshkhasId", uu.fldShakhsId.ToString(), 0).FirstOrDefault();
                //امضای مدرک
                if (s != null)
                {
                    if (Taeed.fldType == 1 && (Taeed.fldApp_ClientId == 5 || Taeed.fldApp_ClientId == 6 || Taeed.fldApp_ClientId == 13))
                    {
                        var Emzaha = m.prs_tblItemDynamic_Client_SignSelect("fldItemDynamic_ClientId", Taeed.fldTableId.ToString(), 0).Where(l => l.fldSign_StampDigitalId == s.fldId).Any();
                        if (Emzaha == false)
                            m.prs_tblItemDynamic_Client_SignInsert(s.fldId,Taeed.fldTableId,  u.UserInputId);
                    }
                    else if (Taeed.fldType == 0 && (Taeed.fldApp_ClientId == 5 || Taeed.fldApp_ClientId == 6 || Taeed.fldApp_ClientId == 13))
                    {
                        m.prs_tblItemDynamic_Client_SignDelete(s.fldId,Taeed.fldTableId);
                    }
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
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadNamayande(StoreRequestParameters parameters, int ProjectId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblRegistrationFirstContract_HaghighiSelect> data = null;


            data = m.prs_tblRegistrationFirstContract_HaghighiSelect("fldHeaderId", ProjectId.ToString(), 0).ToList();


            return this.Store(data, data.Count);
        }
        public ActionResult ReadHoghughi(StoreRequestParameters parameters, int ProjectId)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("شما مجاز به انجام این عملیات نمی باشید.");
            }
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);

            List<prs_tblRegistrationFirstContract_HoghoghiSelect> data = null;


            data = m.prs_tblRegistrationFirstContract_HoghoghiSelect("fldHeaderId", ProjectId.ToString(), 0).ToList();


            return this.Store(data, data.Count);
        }
        public ActionResult TimeLine(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();

            UserInfo user = (UserInfo)(Session["User"]);

            var dd = m.prs_SelectTimeLine(Id).ToList();

            if (dd.Count != 0)
            {
                var result = new Ext.Net.MVC.PartialViewResult();
                result.ViewBag.Id = Id;
                Session["EnterCycleId_TimeLine"] = Id;
                result.ViewBag.EnterCycleId = Id;
                return result;
            }

            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "خطا",
                    Message = "تاکنون هیچ عملیاتی روی این پرونده انجام نگرفته است."
                });
                DirectResult result = new DirectResult();
                return result;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TimeLineDetails(int id)
        {//نمایش اطلاعات جهت رویت کاربر
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                List<Models.prs_SelectTimeLine> q = new List<Models.prs_SelectTimeLine>();
                Models.RaiSamEntities h = new RaiSamEntities();
               
                    q = h.prs_SelectTimeLine(id).ToList();
               
                var NameEghdam = "";
                var waiting = "";
                var NameCharkhe = "";
                var tarikh = "";
                var UserName = "";
                var FirstId = "";
                var NameOpertion = "";
                var FileIdOpertion = "";
                var NameKartable = "";
                var PicCharkhe = "";
                var MethodName = "";
                var TableId = "";
                var HaveInfo = "";

                string[] IconOpertion = new string[q.Count];
                int i = 0;
                foreach (var item in q)
                {
                    var Day = item.fldDay.ToString() + " روز ";
                    var minute = item.fldMinute.ToString().PadLeft('0', 2);
                    var hour = item.fldHour.ToString().PadLeft('0', 2);
                    if (item.fldDay == 0)
                    {
                        waiting = waiting + hour + ":" + minute + ',';
                    }
                    else
                    {
                        waiting = waiting + Day + hour + ":" + minute + ',';
                    }

                    NameOpertion = NameOpertion + item.fldNameOperation + ',';
                    NameCharkhe = NameCharkhe + item.fldNameCharkhe + ',';
                    NameEghdam = NameEghdam + item.fldNameAction + ',';
                    NameKartable = NameKartable + item.fldNameKartable + ',';
                    tarikh = tarikh + item.tarikh + "_" + item.ZAMAN + ',';
                    UserName = UserName + item.fldNameKarbar + ',';
                    FirstId = FirstId + item.fldCharkheFirstId + ',';
                    FileIdOpertion = FileIdOpertion + item.fldFileIdOpertion + ',';
                    MethodName = MethodName + item.fldMethodName + ',';
                    TableId = TableId + item.Id + ',';

                    var infoMethod = item.fldMethodName + "_Info";
                    var hh = "1";
                    Type type = (typeof(SpecializedKartablController));
                    Type type2 = (typeof(GeneralKartablController));
                    var q2 = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(InfoActionAttribute), false).Length > 0 && m.Name == infoMethod).FirstOrDefault();
                    var q3 = type2.GetMethods().Where(m => m.GetCustomAttributes(typeof(InfoActionAttribute), false).Length > 0 && m.Name == infoMethod).FirstOrDefault();
                    if (q2 == null && q3 == null)
                        hh = "0";

                    HaveInfo = HaveInfo + hh + ',';

                    if (item.fileCharkhe != null)
                    {
                        PicCharkhe = Convert.ToBase64String(item.fileCharkhe);
                    }
                    else
                    {
                        PicCharkhe = Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(@"~/Content/icon/actoin/no-image75.jpg")));
                    }
                    if (item.fldFileopertion != null)
                    {
                        IconOpertion[i] = Convert.ToBase64String(item.fldFileopertion);
                    }
                    else
                    {
                        IconOpertion[i] = Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath(@"~/Content/icon/actoin/no-image.jpg")));
                    }
                    i++;
                }
                return Json(new
                {
                    NameEghdam = NameEghdam,
                    waiting = waiting,
                    NameCharkhe = NameCharkhe,
                    tarikh = tarikh,
                    UserName = UserName,
                    FirstId = FirstId,
                    FileIdOpertion = FileIdOpertion,
                    NameOpertion = NameOpertion,
                    NameKartable = NameKartable,
                    MethodName = MethodName,
                    TableId = TableId,
                    PicCharkhe = PicCharkhe,
                    IconOpertion = IconOpertion,
                    HaveInfo = HaveInfo
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInfoAction(string MethodName)
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account");
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                var infoMethod = MethodName + "_Info";

                //Type type = (typeof(SpecializedKartablController));
                Type type2 = (typeof(GeneralKartablController));
                //var q = type.GetMethods().Where(m => m.GetCustomAttributes(typeof(InfoActionAttribute), false).Length > 0 && m.Name == infoMethod).FirstOrDefault();
                var q2 = type2.GetMethods().Where(m => m.GetCustomAttributes(typeof(InfoActionAttribute), false).Length > 0 && m.Name == infoMethod).FirstOrDefault();

                var Url = "";
               /* if (q != null)
                    Url = "/SpecializedKartabl/" + infoMethod;
                else*/ if (q2 != null)
                    Url = "/GeneralKartabl/" + infoMethod;

                return Json(new
                {
                    Url = Url
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintRptGeneralKartabl(int EghdamId, int CartableId, int CompanyId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.EghdamId = EghdamId;
            PartialView.ViewBag.CartableId = CartableId;
            PartialView.ViewBag.CompanyId = CompanyId;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GeneratePDFRptGeneralKartabl(int EghdamId, int CartableId, int CompanyId)
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
                try
                {

                    DataSet.DataSet1 dt = new DataSet.DataSet1();
                    DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter pic = new DataSet.DataSet1TableAdapters.prs_tblSettingSelectTableAdapter();
                    DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter GetDate = new DataSet.DataSet1TableAdapters.prs_GetDateTableAdapter();
                    DataSet.DataSet1.RptGeneralKartablDataTable RptGeneralKartabl = new DataSet.DataSet1.RptGeneralKartablDataTable();
                    string NameReport = "گزارش کارتابل ";


                        string Condition = "";


                        //if (PostId != 0)
                        //{
                        //    Condition = Condition + " and fldOrganId in (select d.fldOrganPostId from  tblShoghleAmaliyati_Detail as d where d.fldHeaderId=" + PostId + ")";
                        //}

                        string commandText = "SELECT top(500) * FROM " +
                        " (SELECT tblEnterToCycle.fldId AS fldIdVorood, tblEnterToCycle.fldExternalId as fldContractId, tblEnterToCycle.fldCharkheId,  " +
                        " dbo.fn_OprtionIdByCharkhe('Charkhe',fldcharkheid,tblEnterToCycle.fldId,0)as DefaultOperationId,       " +
                        " dbo.fn_OprtionIdByCharkhe('Charkhe_Eghdam',fldcharkheid,tblEnterToCycle.fldId, " + EghdamId + "     )as DefaultOperation_CharkheEghdam,    " +
                        " tblCharkhe.fldName AS fldNameCharkhe,  tblCharkhe_FirstEghdam.fldActionId, fldTarikhVorood, fldCount,    " +
                        " tblActions.fldName AS fldNameAction,h.fldTitle as fldTitleHadaf,fldRequestId,[fldTypeVagon]+'('+isnull(cast(fldMehvar as varchar(1)),'0')+N'محور)' as fldTypeVagonName " +
                        " ,cp.fldTitle as fldTitleContract,cp.fldTedad as fldTedad " +
                        " ,ah.fldName as fldFullName,format(fldTarikhDarkhast,'####/##/##')fldTarikhDarkhast,h.fldId as fldHadafId " +
                        " FROM tblEnterToCycle       " +
                        " INNER JOIN tblCharkhe ON tblEnterToCycle.fldCharkheId = tblCharkhe.fldId     " +
                        " INNER JOIN tblCharkhe_FirstEghdam ON tblEnterToCycle.fldId = tblCharkhe_FirstEghdam.fldEnterCycleId 	  " +
                        " INNER JOIN  tblActions ON tblCharkhe_FirstEghdam.fldActionId = tblActions.fldId      " +
                        " inner join tblRegistrationFirstContract cp  on fldExternalId=cp.fldid    " +
                        " inner join tblTypeVagon t on t.fldid=fldTypeVagonId " +
                        " LEFT JOIN dbo.tblRequestRanking r ON cp.fldRequestId = r.fldId     " +
                        " left join tblFirstRegister_HadafAllowed f1 on f1.fldid=r.fldHadafIdAllowed " +
                        " left join tblHadafSabtNam h on h.fldid=fldHadafId " +
                        " left join tblFirstRegister fi on fi.fldid=r.fldFirstRegisterId " +
                        " left join tblCompanyProfile co on co.fldFirstRegisterUser=fi.fldid " +
                        " left join tblAshkhasHoghooghi ah on ah.fldid=co.fldAshkhasHoghoghiId " +
                        " cross apply ( " +
                                    " 	select count(*)fldCount " +
                                    " 	from tblCharkhe_FirstEghdam f  " +
                                    " 	where f.fldEnterCycleId=tblEnterToCycle.fldId and fldActionId= " + EghdamId +
                                " )TedadAction   " +
                       " outer apply( " +
                                    " select 1 as IsArchive  " +
                                    " FROM tblEtmamCharkhe  " +
                                    " inner JOIN     tblCharkhe_FirstEghdam ON tblEtmamCharkhe.fldCharkheFirstId = tblCharkhe_FirstEghdam.fldId   " +
                                    " inner join     tblEnterToCycle ON tblCharkhe_FirstEghdam.fldEnterCycleId = tblEnterToCycle.fldId       " +
                                    " where tblEnterToCycle.fldExternalId=cp.fldid and fldAnavinGhabelCharkheshId=1    " +
                                " ) tblEnter       " +
                     " cross apply ( " +
                                " select dbo.Fn_AssembelyMiladiToShamsi(t2.StartTime)+' '+ CAST(CAST(t2.StartTime AS time(0))AS VARCHAR(8)) AS fldTarikhVorood       " +
                                " from ( " +
                                " 	    select top(1) CONVERT(datetime,SWITCHOFFSET(CONVERT(datetimeoffset,c.[StartTime]),DATENAME(TzOffset, SYSDATETIMEOFFSET()))) StartTime       " +
                                " 		from tblCharkhe_FirstEghdam c  " +
                                " 		where c.fldEnterCycleId=tblEnterToCycle.fldId and fldActionId= " + EghdamId +
                                " 		order by c.fldId desc " +
                                " 	)t2 " +
                                " ) tblChare_First       " +
                     " cross apply  ( " +
                                    " select top(1)  c.fldActionId  " +
                                    " from tblCharkhe_FirstEghdam c  " +
                                    " where c.fldEnterCycleId=tblEnterToCycle.fldId   " +
                                    " order by c.fldId desc " +
                                " ) tblChare_First_last    " +
                        " where fldFinal_Sabt=1 and  fldAnavinGhabelCharkheshId=1 and  tblEnterToCycle.fldCharkheId in(select fldCharkheId from tblEkhtesasActions_Cartable inner join tblCharkhe_Eghdam on     " +
                        " tblCharkhe_Eghdam.fldId=tblEkhtesasActions_Cartable.fldCharkhe_EghdamId  where fldEghdamId= " + EghdamId + "     and fldCartableId=  " + CartableId + "   )      " +
                 Condition + "   and tblCharkhe_FirstEghdam.fldActionId= " + EghdamId + " and tblChare_First_last.fldActionId =" + EghdamId + "     )t			  " +
                        " group by  fldIdVorood,  fldContractId, fldCharkheId,  DefaultOperationId, DefaultOperation_CharkheEghdam,   " +
                        " fldNameCharkhe, fldActionId, fldTarikhVorood, fldCount, fldNameAction,fldTitleHadaf,fldRequestId,fldTypeVagonName " +
                       " ,fldTitleContract,fldTedad,fldFullName,fldTarikhDarkhast, fldHadafId " +
                        " ORDER BY fldFullName  ";

                        List<Models.ContractCycle> Inf = new List<Models.ContractCycle>();
                        string ConnectionString = ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString;
                        SqlConnection con = new SqlConnection(ConnectionString);
                        con.Open();
                        SqlCommand com = new SqlCommand(commandText, con);
                        com.CommandTimeout = 200000; 
                        var dr = com.ExecuteReader();
                        int i = 0;
                        while (dr.Read())
                        {
                            dt.RptGeneralKartabl.Rows.Add();
                                dt.RptGeneralKartabl[i].fldIdVorood = Convert.ToInt32(dr["fldIdVorood"]);
                                dt.RptGeneralKartabl[i].fldContractId = Convert.ToInt32(dr["fldContractId"]);
                                dt.RptGeneralKartabl[i].fldCharkheId = Convert.ToInt32(dr["fldCharkheId"]);
                                dt.RptGeneralKartabl[i].DefaultOperationId = Convert.ToInt32(dr["DefaultOperationId"]);
                                dt.RptGeneralKartabl[i].DefaultOperation_CharkheEghdam = Convert.ToInt32(dr["DefaultOperation_CharkheEghdam"]);
                                dt.RptGeneralKartabl[i].fldNameCharkhe = dr["fldNameCharkhe"].ToString();
                                dt.RptGeneralKartabl[i].fldActionId = Convert.ToInt32(dr["fldActionId"]);
                                dt.RptGeneralKartabl[i].fldTarikhVorood = dr["fldTarikhVorood"].ToString();
                                dt.RptGeneralKartabl[i].fldCount = Convert.ToInt32(dr["fldCount"]);
                                dt.RptGeneralKartabl[i].fldNameAction = dr["fldNameAction"].ToString();
                                dt.RptGeneralKartabl[i].fldTitleHadaf = dr["fldTitleHadaf"].ToString();
                                dt.RptGeneralKartabl[i].fldRequestId = Convert.ToInt32(dr["fldRequestId"]);
                                dt.RptGeneralKartabl[i].fldTypeVagonName = dr["fldTypeVagonName"].ToString();
                                dt.RptGeneralKartabl[i].fldTitleContract = dr["fldTitleContract"].ToString();
                                dt.RptGeneralKartabl[i].fldTedad = Convert.ToInt32(dr["fldTedad"]);
                                dt.RptGeneralKartabl[i].fldFullName = dr["fldFullName"].ToString();
                                dt.RptGeneralKartabl[i].fldTarikhDarkhast = dr["fldTarikhDarkhast"].ToString();
                                dt.RptGeneralKartabl[i].fldHadafId = Convert.ToInt32(dr["fldHadafId"]);
                           i = i + 1;
                        }
                        con.Close();

                        Models.RaiSamEntities m = new RaiSamEntities();
                        var q = m.prs_tblUserSelect("fldId", user.UserSecondId.ToString(), "", 0).FirstOrDefault();


                    dt.EnforceConstraints = false;
                    pic.Fill(dt.prs_tblSettingSelect, "fldId", "1", 0);
                    dt.prs_tblSettingSelect[0].fldTitle = CodeDecode.stringDecode(dt.prs_tblSettingSelect[0].fldTitle);
                    GetDate.Fill(dt.prs_GetDate);
                    var dd = m.prs_GetDate().FirstOrDefault();

                    FastReport.Report Report = new FastReport.Report();
                        Report.Load(AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptKartabl.frx");
                        Report.RegisterData(dt, "raiSamDataSet");
                    Report.SetParameterValue("UserName", q.fldNamePersonal);
                    Report.SetParameterValue("ReportName", NameReport);
                    Report.SetParameterValue("Name", "");
                    Report.SetParameterValue("Saat", dd.fldTime);
                    Report.SetParameterValue("Tarikh", dd.fldTarikh);
                    FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport();
                    pdf.EmbeddingFonts = true;
                    MemoryStream stream = new MemoryStream();
                    Report.Prepare();
                    Report.Export(pdf, stream);



                    return File(stream.ToArray(), "application/pdf");
                }
                catch (Exception x)
                {
                    return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateExcelRptGeneralKartabl(int EghdamId, int CartableId, int CompanyId)
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
                try
                {

                    string[] alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA" };
                    int index = 0;

                    DataSet.DataSet1 dt = new DataSet.DataSet1();
                    DataSet.DataSet1.RptGeneralKartablDataTable RptGeneralKartabl = new DataSet.DataSet1.RptGeneralKartablDataTable();


                    MemoryStream stream = new MemoryStream();



                    string Condition = "";


                    //if (PostId != 0)
                    //{
                    //    Condition = Condition + " and fldOrganId in (select d.fldOrganPostId from  tblShoghleAmaliyati_Detail as d where d.fldHeaderId=" + PostId + ")";
                    //}

                    string commandText = "SELECT top(500) * FROM " +
                    " (SELECT tblEnterToCycle.fldId AS fldIdVorood, tblEnterToCycle.fldExternalId as fldContractId, tblEnterToCycle.fldCharkheId,  " +
                    " dbo.fn_OprtionIdByCharkhe('Charkhe',fldcharkheid,tblEnterToCycle.fldId,0)as DefaultOperationId,       " +
                    " dbo.fn_OprtionIdByCharkhe('Charkhe_Eghdam',fldcharkheid,tblEnterToCycle.fldId, " + EghdamId + "     )as DefaultOperation_CharkheEghdam,    " +
                    " tblCharkhe.fldName AS fldNameCharkhe,  tblCharkhe_FirstEghdam.fldActionId, fldTarikhVorood, fldCount,    " +
                    " tblActions.fldName AS fldNameAction,h.fldTitle as fldTitleHadaf,fldRequestId,[fldTypeVagon]+'('+isnull(cast(fldMehvar as varchar(1)),'0')+N'محور)' as fldTypeVagonName " +
                    " ,cp.fldTitle as fldTitleContract,cp.fldTedad as fldTedad " +
                    " ,ah.fldName as fldFullName,format(fldTarikhDarkhast,'####/##/##')fldTarikhDarkhast,h.fldId as fldHadafId " +
                    " FROM tblEnterToCycle       " +
                    " INNER JOIN tblCharkhe ON tblEnterToCycle.fldCharkheId = tblCharkhe.fldId     " +
                    " INNER JOIN tblCharkhe_FirstEghdam ON tblEnterToCycle.fldId = tblCharkhe_FirstEghdam.fldEnterCycleId 	  " +
                    " INNER JOIN  tblActions ON tblCharkhe_FirstEghdam.fldActionId = tblActions.fldId      " +
                    " inner join tblRegistrationFirstContract cp  on fldExternalId=cp.fldid    " +
                    " inner join tblTypeVagon t on t.fldid=fldTypeVagonId " +
                    " LEFT JOIN dbo.tblRequestRanking r ON cp.fldRequestId = r.fldId     " +
                    " left join tblFirstRegister_HadafAllowed f1 on f1.fldid=r.fldHadafIdAllowed " +
                    " left join tblHadafSabtNam h on h.fldid=fldHadafId " +
                    " left join tblFirstRegister fi on fi.fldid=r.fldFirstRegisterId " +
                    " left join tblCompanyProfile co on co.fldFirstRegisterUser=fi.fldid " +
                    " left join tblAshkhasHoghooghi ah on ah.fldid=co.fldAshkhasHoghoghiId " +
                    " cross apply ( " +
                                " 	select count(*)fldCount " +
                                " 	from tblCharkhe_FirstEghdam f  " +
                                " 	where f.fldEnterCycleId=tblEnterToCycle.fldId and fldActionId= " + EghdamId +
                            " )TedadAction   " +
                   " outer apply( " +
                                " select 1 as IsArchive  " +
                                " FROM tblEtmamCharkhe  " +
                                " inner JOIN     tblCharkhe_FirstEghdam ON tblEtmamCharkhe.fldCharkheFirstId = tblCharkhe_FirstEghdam.fldId   " +
                                " inner join     tblEnterToCycle ON tblCharkhe_FirstEghdam.fldEnterCycleId = tblEnterToCycle.fldId       " +
                                " where tblEnterToCycle.fldExternalId=cp.fldid and fldAnavinGhabelCharkheshId=1    " +
                            " ) tblEnter       " +
                 " cross apply ( " +
                            " select dbo.Fn_AssembelyMiladiToShamsi(t2.StartTime)+' '+ CAST(CAST(t2.StartTime AS time(0))AS VARCHAR(8)) AS fldTarikhVorood       " +
                            " from ( " +
                            " 	    select top(1) CONVERT(datetime,SWITCHOFFSET(CONVERT(datetimeoffset,c.[StartTime]),DATENAME(TzOffset, SYSDATETIMEOFFSET()))) StartTime       " +
                            " 		from tblCharkhe_FirstEghdam c  " +
                            " 		where c.fldEnterCycleId=tblEnterToCycle.fldId and fldActionId= " + EghdamId +
                            " 		order by c.fldId desc " +
                            " 	)t2 " +
                            " ) tblChare_First       " +
                 " cross apply  ( " +
                                " select top(1)  c.fldActionId  " +
                                " from tblCharkhe_FirstEghdam c  " +
                                " where c.fldEnterCycleId=tblEnterToCycle.fldId   " +
                                " order by c.fldId desc " +
                            " ) tblChare_First_last    " +
                    " where fldFinal_Sabt=1 and  fldAnavinGhabelCharkheshId=1 and  tblEnterToCycle.fldCharkheId in(select fldCharkheId from tblEkhtesasActions_Cartable inner join tblCharkhe_Eghdam on     " +
                    " tblCharkhe_Eghdam.fldId=tblEkhtesasActions_Cartable.fldCharkhe_EghdamId  where fldEghdamId= " + EghdamId + "     and fldCartableId=  " + CartableId + "   )      " +
             Condition + "   and tblCharkhe_FirstEghdam.fldActionId= " + EghdamId + " and tblChare_First_last.fldActionId =" + EghdamId + "     )t			  " +
                    " group by  fldIdVorood,  fldContractId, fldCharkheId,  DefaultOperationId, DefaultOperation_CharkheEghdam,   " +
                    " fldNameCharkhe, fldActionId, fldTarikhVorood, fldCount, fldNameAction,fldTitleHadaf,fldRequestId,fldTypeVagonName " +
                   " ,fldTitleContract,fldTedad,fldFullName,fldTarikhDarkhast, fldHadafId " +
                    " ORDER BY fldFullName  ";

                    List<Models.ContractCycle> Inf = new List<Models.ContractCycle>();
                    string ConnectionString = ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(ConnectionString);
                    con.Open();
                        SqlCommand com = new SqlCommand(commandText, con);
                        com.CommandTimeout = 200000;
                        var dr = com.ExecuteReader();
                        dt.RptGeneralKartabl.Load(dr);
                        con.Close();

                        dt.EnforceConstraints = false;
                        var q = dt.RptGeneralKartabl.ToList().OrderBy(l => l.fldFullName);

                        string Checked = "fldFullName;fldNameCharkhe;fldTarikhVorood;fldTitleContract";
                        var ListExcel = Checked.Split(';');

                        Workbook wb = new Workbook();
                        Worksheet sheet = wb.Worksheets[0];
                        var Check = "";
                        var fldFullName = ""; var fldNameCharkhe = ""; var fldTarikhVorood = ""; var fldTitleContract = ""; 
                        foreach (var item in ListExcel)
                        {

                            switch (item)
                            {
                                case "fldFullName":
                                    Check = "نام شرکت";
                                    Cell cell = sheet.Cells[alpha[index] + "1"];
                                    cell.PutValue(Check);
                                    int ii = 0;
                                    foreach (var _item in q)
                                    {
                                        fldFullName = _item.fldFullName;
                                        Cell Cell = sheet.Cells[alpha[index] + (ii + 2)];
                                        Cell.PutValue(fldFullName);
                                        ii++;
                                    }
                                    index++;
                                    break;

                                case "fldNameCharkhe":
                                    Check = "عنوان چرخه";
                                    Cell cell1 = sheet.Cells[alpha[index] + "1"];
                                    cell1.PutValue(Check);
                                    int i1 = 0;
                                    foreach (var _item in q)
                                    {
                                        fldNameCharkhe = _item.fldNameCharkhe;
                                        Cell Cell = sheet.Cells[alpha[index] + (i1 + 2)];
                                        Cell.PutValue(fldNameCharkhe);
                                        i1++;
                                    }
                                    index++;
                                    break;
                                case "fldTarikhVorood":
                                    Check = "زمان آخرین ورود";
                                    Cell cell2 = sheet.Cells[alpha[index] + "1"];
                                    cell2.PutValue(Check);
                                    int i2 = 0;
                                    foreach (var _item in q)
                                    {
                                        fldTarikhVorood = _item.fldTarikhVorood;
                                        Cell Cell = sheet.Cells[alpha[index] + (i2 + 2)];
                                        Cell.PutValue(fldTarikhVorood);
                                        i2++;
                                    }
                                    index++;
                                    break;
                                case "fldTitleContract":
                                    Check = "عنوان";
                                    Cell cell3 = sheet.Cells[alpha[index] + "1"];
                                    cell3.PutValue(Check);
                                    int i3 = 0;
                                    foreach (var _item in q)
                                    {
                                        fldTitleContract = _item.fldTitleContract;
                                        Cell Cell = sheet.Cells[alpha[index] + (i3 + 2)];
                                        Cell.PutValue(fldTitleContract);
                                        i3++;
                                    }
                                    index++;
                                    break;
                                
                            }
                        }
                        wb.Save(stream, SaveFormat.Excel97To2003);
                 


                    return File(stream.ToArray(), "application/vnd.ms-excel", "گزارش کارتابل.xls");
                }
                catch (Exception x)
                {

                    return Json(x.Message.ToString(), JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
