using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Jobs
{
    public class EndpointJob
    {
        private readonly IContainer _container;
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;

        public EndpointJob(IContainer container, ProviderRelationshipsConfiguration providerRelationshipsConfiguration)
        {
            _container = container;
            _providerRelationshipsConfiguration = providerRelationshipsConfiguration;
        }

        [NoAutomaticTrigger]
        public async Task RunAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.Jobs")
                .UseAzureServiceBusTransport(() => _providerRelationshipsConfiguration.ServiceBusConnectionString)
                .UseLicense(_providerRelationshipsConfiguration.NServiceBusLicense)
                .UseSqlServerPersistence(() => _container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseSendOnly()
                .UseStructureMapBuilder(_container);

            var endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            _container.Configure(c => c.For<IMessageSession>().Use(endpoint));
        }
    }
}