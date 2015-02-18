using HouseOfKings.Web.App_Start;
using HouseOfKings.Web.DAL.Repository;
using HouseOfKings.Web.Hubs;
using HouseOfKings.Web.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseOfKings.Web.Startup))]

namespace HouseOfKings.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.CreatePerOwinContext<RuleRepository>(RuleRepository.Create);

            var kernel = new StandardKernel();
            var resolver = new NinjectSignalRDependencyResolver(kernel);

            kernel.Bind<GameService>()
                .ToSelf()
                .InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>))
                .ToMethod(context => resolver.Resolve<IConnectionManager>().GetHubContext<GameHub>().Clients).WhenInjectedInto<GameService>();

            var config = new HubConfiguration();
            config.Resolver = resolver;
            app.MapSignalR(config);
        }
    }
}