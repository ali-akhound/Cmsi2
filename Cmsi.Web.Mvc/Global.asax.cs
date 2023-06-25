using AVA.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AVA.Web.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string DefaultImageUrl = "~/assets/img/profile-green.png";
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            IViewEngine razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //var context = new ApplicationDbContext();
            //context.Database.SqlQuery("");
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Cookies["Lang"] == null)
            {
                HttpCookie languageCookie = new HttpCookie("Lang");
                AVA.Web.Resources.Resource.Culture = new CultureInfo("fa-IR");
                languageCookie.Value = "fa";
                languageCookie.Expires = DateTime.Now.AddDays(10);
                Response.SetCookie(languageCookie);
            }
            else
            {
                if (Request.Cookies["Lang"].Value == "en")
                {
                    AVA.Web.Resources.Resource.Culture = new CultureInfo("en-US");
                }
                else if (Request.Cookies["Lang"].Value == "fa")
                {
                    AVA.Web.Resources.Resource.Culture = new CultureInfo("fa-IR");
                }
            }
        }
    }
}
