﻿using WebApi.StructureMap;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Api;
using SFA.DAS.ProviderRelationships.Api.Authentication;
using StructureMap;
using StructureMap.Pipeline;

//[assembly: OwinStartup(typeof(Startup))]

namespace SFA.DAS.ProviderRelationships.Api
{
    //public class Startup_OLD
    //{
    //    public void Configuration(IAppBuilder app)
    //    {
    //        var container = GlobalConfiguration.Configuration.DependencyResolver.GetService<IContainer>();
    //        var logger = container.GetInstance<ILog>();
            
    //        logger.Info("Starting Provider Relationships api");

    //        var authenticationStartupArgs = new ExplicitArguments();
            
    //        authenticationStartupArgs.Set(app);
  
    //        var authenticationStartup = container.GetInstance<IAuthenticationStartup>(authenticationStartupArgs);
            
    //        authenticationStartup.Initialize();
    //    }
    //}
}