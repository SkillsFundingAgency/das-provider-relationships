﻿using System.Data.Common;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Configuration.NLog;
using SFA.DAS.NServiceBus.Configuration.StructureMap;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Startup;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Jobs
{
    public class NServiceBusStartup : IStartup
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
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.Jobs")
                .UseAzureServiceBusTransport(() => _providerRelationshipsConfiguration.ServiceBusConnectionString, _environmentService.IsCurrent(DasEnv.LOCAL))
                .UseLicense(_providerRelationshipsConfiguration.NServiceBusLicense)
                .UseMessageConventions()
                .UseSqlServerPersistence(() => _container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseSendOnly()
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