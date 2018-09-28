using Owin;
using Microsoft.Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Web;
using SFA.DAS.ProviderRelationships.Web.App_Start;
using StructureMap.Pipeline;

[assembly: OwinStartup(typeof(Startup))]

namespace SFA.DAS.ProviderRelationships.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = StructuremapMvc.StructureMapDependencyScope.Container;
            var log = container.GetInstance<ILog>();
            log.Info("Starting ProviderRelations Web Application");

            var authenticationStartupArgs = new ExplicitArguments();
            authenticationStartupArgs.Set(app);
            var authenticationStartup = container.GetInstance<IAuthenticationStartup>(authenticationStartupArgs);
            authenticationStartup.Initialise();
        }
    }
}