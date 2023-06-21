using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.ServiceRegistrations;

public static class ReadStoreServiceRegistrations
{
    public static IServiceCollection AddReadStoreServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentClientFactory, DocumentClientFactory>();

        services.AddSingleton(provider =>
        {
            var clientFactory = provider.GetService<IDocumentClientFactory>();
            return clientFactory.CreateDocumentClient();
        });

        services.AddTransient<IAccountProviderLegalEntitiesRepository, AccountProviderLegalEntitiesRepository>();

        return services;
    }
}