using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaiSam.Controllers.Operation
{
    public class MostanadatFaniController : Controller
    {
        //
        // GET: /MostanadatFani/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PrintMostanadPdf()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        [HttpPost]
        public FileContentResult DownloadMostanadCHM()
        {
            if (Session["User"] == null)
                return null;
            string savePath = Server.MapPath(@"~\Help\Mostanadat\CHM\ChmDocument.chm");
            //FileInfo fileinf = new FileInfo(savePath);
            //FileInfoExtensions.Unblock(fileinf);
            MemoryStream st = new MemoryStream(System.IO.File.ReadAllBytes(savePath));
            //Process runcommd = new Process();

            //runcommd.StartInfo.FileName = "streams";
            //runcommd.StartInfo.Arguments =@" -d \"+savePath;

            //runcommd.StartInfo.UseShellExecute = false;
            //runcommd.StartInfo.CreateNoWindow = false;

            //runcommd.StartInfo.RedirectStandardError = true;
            //runcommd.StartInfo.RedirectStandardOutput = true;
            //runcommd.StartInfo.RedirectStandardInput = true;

            //// now run it
            //runcommd.Start();

            //// be sure that we end
            //runcommd.StandardInput.Flush();
            //runcommd.StandardInput.Close();
            return File(st.ToArray(), MimeType.Get(".chm"), "ChmDocument.chm");

        }
    }
}
