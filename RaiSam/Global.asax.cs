using RaiSam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RaiSam
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Models.RaiSamEntities m = new Models.RaiSamEntities();
            m.prs_tblSafDownloadUpdate("All", 0, 4);
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                Models.RaiSamEntities m = new Models.RaiSamEntities();

                UserInfo user = (UserInfo)(Session["User"]);
                m.prs_tblSafDownloadUpdate("fldInputId", Convert.ToInt64(user.UserInputId), 4);
            }
        }
    }
}