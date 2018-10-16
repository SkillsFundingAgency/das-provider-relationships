using System.Data.Common;
using System.Web.Mvc;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web
{
    public static class NServiceBusConfig
    {
        private static IEndpointInstance _endpoint;

        public static void Start()
        {
            var container = DependencyResolver.Current.GetService<IContainer>();

            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.Web")
                .UseAzureServiceBusTransport(() => container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString)
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(container.GetInstance<ProviderRelationshipsConfiguration>().NServiceBusLicense)
                .UseSqlServerPersistence(() => container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseOutbox()
                .UseStructureMapBuilder(container)
                .UseUnitOfWork();

            _endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            container.Configure(c => c.For<IMessageSession>().Use(_endpoint));
        }

        public static void Stop()
        {
            _endpoint?.Stop().GetAwaiter().GetResult();
        }
    }
}