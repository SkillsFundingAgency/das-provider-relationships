using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Configuration.NLog;
using SFA.DAS.NServiceBus.Configuration.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    public class NServiceBusStartup
    {
        private readonly IContainer _container;
        private readonly IEnvironmentService _environmentService;
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;
        private IEndpointInstance _endpoint;

        public NServiceBusStartup(IContainer container, IEnvironmentService environmentService, ProviderRelationshipsConfiguration providerRelationshipsConfiguration)
        {
            _container = container;
            _environmentService = environmentService;
            _providerRelationshipsConfiguration = providerRelationshipsConfiguration;
        }

        public async Task StartAsync()
        {
            var endpoint = "SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness";
            var endpointConfiguration = new EndpointConfiguration(endpoint)
                .UseAzureServiceBusTransport(() => _container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString, _environmentService.IsCurrent(DasEnv.LOCAL))
                .UseErrorQueue($"{endpoint}-error")
                .UseInstallers()
                .UseLicense(_providerRelationshipsConfiguration.NServiceBusLicense)
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseStructureMapBuilder(_container)
                .UseSendOnly();
                
            _endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
                
            _container.Configure(c => c.For<IMessageSession>().Use(_endpoint));
        }

        public Task StopAsync()
        {
            return _endpoint.Stop();
        }
    }
}