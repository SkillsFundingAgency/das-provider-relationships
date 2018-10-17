using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = DasClaimTypes.Id;
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DependencyResolver.Current.GetService<IStartup>().StartAsync().GetAwaiter().GetResult();
        }

        protected void Application_End()
        {
            DependencyResolver.Current.GetService<IStartup>().StopAsync().GetAwaiter().GetResult();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var logger = DependencyResolver.Current.GetService<ILog>();
            var telemetryClient = new TelemetryClient();

            logger.Error(exception, "Application error");
            telemetryClient.TrackException(exception);
        }
    }
}