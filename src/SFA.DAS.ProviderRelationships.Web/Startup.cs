﻿using Owin;
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
            var log = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ILog>();
            log.Info("Starting ProviderRelations Web Application");

            var authenticationStartupArgs = new ExplicitArguments();
            authenticationStartupArgs.Set(app);
            var authenticationStartup = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<IAuthenticationStartup>(authenticationStartupArgs);
            authenticationStartup.Initialise();

            //todo: config repo - whats this??
        }
    }
}