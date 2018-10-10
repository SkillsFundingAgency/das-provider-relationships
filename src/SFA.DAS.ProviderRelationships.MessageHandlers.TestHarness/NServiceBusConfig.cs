using System.Data.Common;
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

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    public class NServiceBusConfig
    {
        private readonly IContainer _container;

        public IEndpointInstance Endpoint { get; set; }

        public NServiceBusConfig(IContainer container)
        {
            _container = container;
        }

        public void Start()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.EAS.Web")
                .UseAzureServiceBusTransport(() =>
                    _container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString)
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(_container.GetInstance<ProviderRelationshipsConfiguration>().NServiceBusLicense)
                .UseSqlServerPersistence(() => _container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                //.UseOutbox()
                .UseStructureMapBuilder(_container);
                //.UseUnitOfWork();

            Endpoint = global::NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            //container.Configure(c =>
            //{
            //    c.For<IMessageSession>().Use(_endpoint);
            //});
        }

        public void Stop()
        {
            Endpoint?.Stop().GetAwaiter().GetResult();
        }
    }
}
    