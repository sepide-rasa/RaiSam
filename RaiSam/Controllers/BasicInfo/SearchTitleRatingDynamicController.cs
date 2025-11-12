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

namespace RaiSam.Controllers.BasicInfo
{
    public class SearchTitleRatingDynamicController : Controller
    {
        //
        // GET: /SearchTitleRatingDynamic/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index(int State)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            PartialView.ViewBag.State = State;
            return PartialView;
        }

        public ActionResult Read()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            List<Models.prs_tblTitleRatingDynamicSelect> info = null;

            info = m.prs_tblTitleRatingDynamicSelect("", "", 0).ToList();
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddTitleRatingDynamic(string DynamicRatingId, string ItemDynamicRatingID)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            List<Models.prs_tblItemsDynamicRatingSelect> groups = new List<Models.prs_tblItemsDynamicRatingSelect>();
            Models.prs_tblItemsDynamicRatingSelect V = new Models.prs_tblItemsDynamicRatingSelect();
            var IdDynamicRating = DynamicRatingId.Split(';');
            var IdItemDynamic = ItemDynamicRatingID.Split(';');
            bool Elzami = false; bool IsCopy = false; int Id = 0;


            Models.RaiSamEntities m = new RaiSamEntities();

            for (int i = 0; i < IdDynamicRating.Length - 1; i++)
            {
                var T = m.prs_tblTitleRatingDynamicSelect("fldId", IdDynamicRating[i], 0).FirstOrDefault();
                for (int j = 0; j < IdItemDynamic.Length - 1; j++)
                {
                    var ItemDynamic = m.prs_tblItemsDynamicRatingSelect("fldId", IdItemDynamic[j].ToString(), 0).Where(k => k.fldDynamicRatingId == Convert.ToInt32(IdDynamicRating[i])).FirstOrDefault();
                    if (ItemDynamic != null)
                    {
                        Elzami = ItemDynamic.fldElzami;
                        IsCopy = ItemDynamic.fldIsCopy;
                        Id = ItemDynamic.fldId;
                        break;
                    }

                }

                V = new Models.prs_tblItemsDynamicRatingSelect();
                V.fldElzami = Elzami;
                V.fldIsCopy = IsCopy;
                V.fldDynamicRatingId = T.fldId;
                V.fldTitleRatingDynamic = T.fldTitleFA;
                V.fldId = Id;
                Id = 0;
                Elzami = false;
                IsCopy = false;
                groups.Add(V);
            }
            return Json(groups.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
