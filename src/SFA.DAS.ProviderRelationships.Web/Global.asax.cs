using System;
using System.Data.Common;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights;
using NServiceBus;
using SFA.DAS.NLog.Logger;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Web.App_Start;
using SFA.DAS.UnitOfWork.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class MvcApplication : HttpApplication
    {
        private IEndpointInstance _endpoint;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            StartServiceBusEndpoint();
        }

        protected void Application_End()
        {
            StopServiceBusEndpoint();
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

        private void StartServiceBusEndpoint()
        {
            var container = StructuremapMvc.StructureMapDependencyScope.Container;

            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.Web")
                .UseAzureServiceBusTransport(() => container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString)
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(container.GetInstance<ProviderRelationshipsConfiguration>().NServiceBusLicense.HtmlDecode())
                .UseSqlServerPersistence(() => container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseOutbox()
                .UseStructureMapBuilder(container)
                .UseUnitOfWork();

            _endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            container.Configure(c =>
            {
                c.For<IMessageSession>().Use(_endpoint);
            });
        }

        private void StopServiceBusEndpoint()
        {
            _endpoint?.Stop().GetAwaiter().GetResult();
        }
    }
}