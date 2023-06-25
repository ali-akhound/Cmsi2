using System.Web.Mvc;

namespace AVA.Web.Mvc.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "PublicAdmin", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AVA.Web.Mvc.Controllers.Admin" }
            );
        }
    }
}