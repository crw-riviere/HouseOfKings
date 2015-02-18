using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseOfKings.Web.Startup))]
namespace HouseOfKings.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
