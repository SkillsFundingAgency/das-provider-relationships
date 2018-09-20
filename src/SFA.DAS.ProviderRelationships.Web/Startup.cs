using Owin;
using Microsoft.Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web;
using SFA.DAS.ProviderRelationships.Web.App_Start;

[assembly: OwinStartup(typeof(Startup))]

namespace SFA.DAS.ProviderRelationships.Web
{
    public class Startup
    {
        //empcom has non-static logger, which should we go with?
        //private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            var log = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ILog>();
            log.Info("DI Log");

            //Logger.Info("Starting ProviderRelations Web Application");
            log.Info("Starting ProviderRelations Web Application");

            var config = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ProviderRelationshipsConfiguration>();

            new AuthenticationStartup().Initialise(app, config.Identity, log);
        }
    }
}