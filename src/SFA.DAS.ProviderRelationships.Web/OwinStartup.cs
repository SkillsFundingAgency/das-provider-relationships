using Owin;
using Microsoft.Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Web;
using SFA.DAS.ProviderRelationships.Web.App_Start;
using StructureMap.Pipeline;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace SFA.DAS.ProviderRelationships.Web
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = StructuremapMvc.StructureMapDependencyScope.Container;
            var logger = container.GetInstance<ILog>();
            
            logger.Info("Starting Provider Relationships web application");

            var authenticationStartupArgs = new ExplicitArguments();
            
            authenticationStartupArgs.Set(app);
            
            var authenticationStartup = container.GetInstance<IAuthenticationStartup>(authenticationStartupArgs);
            
            authenticationStartup.Initialise();
        }
    }
}