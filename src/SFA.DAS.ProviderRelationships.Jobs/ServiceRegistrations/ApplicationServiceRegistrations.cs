using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.Jobs.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IProviderRelationshipsDbContextFactory, DbContextWithNewTransactionFactory>();
        services.AddTransient<IRoatpApiHttpClientFactory, RoatpApiHttpClientFactory>();
        services.AddTransient<IRoatpService, RoatpService>();

        services.AddTransient<IDocumentClientFactory, DocumentClientFactory>();
        services.AddSingleton<IDocumentClient>(provider=>
        {
            var factory = provider.GetService<IDocumentClientFactory>();
            return factory.CreateDocumentClient();
        });

        return services;
    }
}