using System.Web.Mvc;

namespace RaiSam.Areas.Faces
{
    public class FacesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Faces";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Faces_default",
                "Faces/{controller}/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
