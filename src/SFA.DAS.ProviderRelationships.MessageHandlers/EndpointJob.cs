using System.Data.Common;
using System.Threading;
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
using SFA.DAS.UnitOfWork.NServiceBus;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class EndpointJob
    {
        private readonly IContainer _container;

        public EndpointJob(IContainer container)
        {
            _container = container;
        }

        [NoAutomaticTrigger]
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.MessageHandlers")
                .UseAzureServiceBusTransport(() => _container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString)
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(_container.GetInstance<ProviderRelationshipsConfiguration>().NServiceBusLicense)
                .UseSqlServerPersistence(() => _container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseOutbox()
                .UseStructureMapBuilder(_container)
                .UseUnitOfWork();

            var endpoint = await Endpoint.Start(endpointConfiguration);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(3000, cancellationToken);
            }

            await endpoint.Stop();
        }
    }
}