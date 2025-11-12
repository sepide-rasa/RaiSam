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
using Aspose.Cells;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Net.Http;
using System.Text;
using System.Web.Services;

namespace RaiSam.Controllers.Report
{
    public class RptRizAmalkardController : Controller
    {
        //
        // GET: /RptRizAmalkard/

        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult IndexFasli(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var partial = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return partial;
        }
        public ActionResult GetSal()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            UserInfo user = (UserInfo)(Session["User"]);

            List<ClassYear> sh = new List<ClassYear>();
            var q = m.prs_GetDate().FirstOrDefault();
            int fldsal = Convert.ToInt32(q.fldTarikh.Substring(0, 4)) - 10;
            for (int i = fldsal; i < fldsal + 19; i++)
            {
                ClassYear CboSal = new ClassYear();

                CboSal.fldYear = i;
                sh.Add(CboSal);
            }
            return Json(sh, JsonRequestBehavior.AllowGet);
        }
        public FileResult GenerateXls(string AzSal, string TaSal, int state)
        {

            if (Session["User"] == null)
                return null;
            try
            {

                Models.RaiSamEntities m = new RaiSamEntities();
                UserInfo user = (UserInfo)(Session["User"]);

                FastReport.Report rep = new FastReport.Report();
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptAmalkardMahane.frx";
                if (state == 2)
                    path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptAmalkardFasli.frx";
                rep.Load(path);
                rep.SetParameterValue("azsal", Convert.ToInt32(AzSal));
                rep.SetParameterValue("tasal", Convert.ToInt32(TaSal));
                rep.SetParameterValue("ConectionStr", System.Configuration.ConfigurationManager.ConnectionStrings["RaiSamConnectionString"].ConnectionString);

                FastReport.Export.OoXML.Excel2007Export xlsExport = new FastReport.Export.OoXML.Excel2007Export();



                MemoryStream stream = new MemoryStream();
      
                rep.Prepare();


                rep.Export(xlsExport, stream);

                return File(stream.ToArray(), "xls", "Amalkard.xls");


            }
            catch (Exception x)
            {
                return null;
            }
        }
    }
}
