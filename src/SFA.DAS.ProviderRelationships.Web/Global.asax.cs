﻿using System;
using System.Configuration;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Startup;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = DasClaimTypes.Id;
            AreaRegistration.RegisterAllAreas();
            BinderConfig.RegisterBinders(ModelBinders.Binders);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            MvcHandler.DisableMvcResponseHeader = true;
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
            DependencyResolver.Current.GetService<IStartup>().StartAsync().GetAwaiter().GetResult();
        }

        protected void Application_End()
        {
            DependencyResolver.Current.GetService<IStartup>().StopAsync().GetAwaiter().GetResult();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Error(ex, ex.GetAggregateMessage());
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            // Ensure Session is created on first page interaction before
            // establishing an attempt an authentication
            // https://web.archive.org/web/20170912171644/https:/katanaproject.codeplex.com/workitem/197
            HttpContext.Current.Session["RunSession"] = "1";
        }
    }
}