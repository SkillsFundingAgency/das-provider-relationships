using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Startup;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    public class EndpointStartup : IStartup
    {
        private readonly IContainer _container;
        private readonly IEnvironment _environment;
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;
        private IEndpointInstance _endpoint;

        public EndpointStartup(IContainer container, IEnvironment environment, ProviderRelationshipsConfiguration providerRelationshipsConfiguration)
        {
            _container = container;
            _environment = environment;
            _providerRelationshipsConfiguration = providerRelationshipsConfiguration;
        }

        public async Task StartAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness")
                .UseAzureServiceBusTransport(() => _container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString, _environment.IsCurrentEnvironment(DasEnv.LOCAL))
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(_providerRelationshipsConfiguration.NServiceBusLicense)
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