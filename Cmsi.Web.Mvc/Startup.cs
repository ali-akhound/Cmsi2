using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AVA.Web.Mvc.Startup))]
namespace AVA.Web.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
