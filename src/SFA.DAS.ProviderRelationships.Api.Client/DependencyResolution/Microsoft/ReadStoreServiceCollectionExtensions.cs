using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.Microsoft
{
    internal static class ReadStoreServiceCollectionExtensions
    {
        public static IServiceCollection AddReadStore(this IServiceCollection services)
        {
            return services.AddSingleton<IDocumentClientFactory, DocumentClientFactory>()
                .AddTransient<IAccountProviderLegalEntitiesReadOnlyRepository, AccountProviderLegalEntitiesReadOnlyRepository>();
        }
    }
}