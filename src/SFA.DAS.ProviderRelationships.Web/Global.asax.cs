using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Web.App_Start;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            if (exception is HttpException httpException && httpException.GetHttpCode() == (int)HttpStatusCode.NotFound)
            {
                return;
            }
            
            var container = StructuremapMvc.StructureMapDependencyScope.Container;
            var logger = container.GetInstance<ILog>();
            var telemetryClient = new TelemetryClient();

            logger.Error(exception, "Application error");
            telemetryClient.TrackException(exception);
        }
    }
}