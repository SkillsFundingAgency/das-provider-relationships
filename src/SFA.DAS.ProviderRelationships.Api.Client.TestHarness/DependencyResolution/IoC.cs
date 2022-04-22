using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.Microsoft;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution
{
    public static class IoC
    {
        public static ServiceProvider InitializeServiceProvider(ProviderRelationshipsApiConfiguration configuration)
        {
            return new ServiceCollection()
                .AddProviderRelationshipsApiClient()
                .AddTransient<GetAccountProviderLegalEntitiesWithPermissionScenario>()
                .AddTransient<HasPermissionScenario>()
                .AddTransient<HasRelationshipWithPermissionScenario>()
                .AddTransient<PingScenario>()
                .AddSingleton<ProviderRelationshipsApiConfiguration>(configuration)
                .BuildServiceProvider();
        }

        public static Container InitializeContainer(ProviderRelationshipsApiConfiguration configuration)
        {
            var container = new Container(c => c.AddRegistry<ProviderRelationshipsApiClientRegistry>());
            container.Configure(c => c.For<ProviderRelationshipsApiConfiguration>().Use(c => configuration).Singleton());

            return container;
        }
    }
}