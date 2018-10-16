using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    public class StartupEndpoint : IStartupTask
    {
        private readonly IContainer _container;
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;
        private IEndpointInstance _endpoint;

        public StartupEndpoint(IContainer container, ProviderRelationshipsConfiguration providerRelationshipsConfiguration)
        {
            _container = container;
            _providerRelationshipsConfiguration = providerRelationshipsConfiguration;
        }

        public async Task StartAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness")
                .UseAzureServiceBusTransport(() => _container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString)
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(_providerRelationshipsConfiguration.NServiceBusLicense)
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseStructureMapBuilder(_container);
                
            _endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
                
            _container.Configure(c => c.For<IMessageSession>().Use(_endpoint));
        }

        public Task StopAsync()
        {
            return _endpoint.Stop();
        }
    }
}