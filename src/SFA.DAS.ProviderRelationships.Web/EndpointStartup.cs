using System.Data.Common;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Startup;
using SFA.DAS.UnitOfWork.NServiceBus;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web
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
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.Web")
                .UseAzureServiceBusTransport(() => _container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString, _environment.IsCurrentEnvironment(DasEnv.LOCAL))
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(_providerRelationshipsConfiguration.NServiceBusLicense)
                .UseSqlServerPersistence(() => _container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseOutbox()
                .UseStructureMapBuilder(_container)
                .UseUnitOfWork();

            _endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            _container.Configure(c => c.For<IMessageSession>().Use(_endpoint));
        }

        public Task StopAsync()
        {
            return _endpoint.Stop();
        }
    }
}