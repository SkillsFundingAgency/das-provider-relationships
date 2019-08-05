using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.Extensibility;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Api.DependencyResolution;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Startup;
using WebApi.StructureMap;

namespace SFA.DAS.ProviderRelationships.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            IoC.Initialize(GlobalConfiguration.Configuration);
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
            GlobalConfiguration.Configuration.DependencyResolver.GetService<IStartup>().StartAsync().GetAwaiter().GetResult();
        }

        protected void Application_End()
        {
            GlobalConfiguration.Configuration.DependencyResolver.GetService<IStartup>().StopAsync().GetAwaiter().GetResult();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var logger = GlobalConfiguration.Configuration.DependencyResolver.GetService<ILog>();

            logger.Error(ex, ex.GetAggregateMessage());
        }
    }
}