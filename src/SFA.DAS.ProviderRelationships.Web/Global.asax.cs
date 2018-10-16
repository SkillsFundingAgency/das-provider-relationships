using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var startupTasks = DependencyResolver.Current.GetServices<IStartupTask>();
            
            StartupTasks.StartAsync(startupTasks).GetAwaiter().GetResult();
            AntiForgeryConfig.UniqueClaimTypeIdentifier = DasClaimTypes.Id;
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            StartupTasks.StopAsync().GetAwaiter().GetResult();
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