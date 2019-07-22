using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.Microsoft;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution
{
    public static class IoC
    {
        public static ServiceProvider InitializeServiceProvider()
        {
            return new ServiceCollection()
                .AddProviderRelationshipsApiClient()
                .AddTransient<GetAccountProviderLegalEntitiesWithPermissionScenario>()
                .AddTransient<HasPermissionScenario>()
                .AddTransient<HasRelationshipWithPermissionScenario>()
                .BuildServiceProvider();
        }

        public static Container InitializeContainer()
        {
            return new Container(c => c.AddRegistry<ProviderRelationshipsApiClientRegistry>());
        }
    }
}