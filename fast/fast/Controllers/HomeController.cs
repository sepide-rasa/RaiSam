using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fast.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ReportPage()
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\RptTarazMahane.frx";
            ViewBag.Path = path;
            ViewBag.RId = "Taraz";

            ViewBag.LogoId = "15";
            return View();
        }
    }
}