using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.Jobs.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IRoatpService, RoatpService>();
        services.AddTransient<IProviderRelationshipsDbContextFactory, DbContextWithNewTransactionFactory>();
        
        return services;
    }
}