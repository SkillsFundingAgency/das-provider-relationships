using SFA.DAS.Encoding;

namespace SFA.DAS.ProviderRelationships.Api.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IEncodingService, EncodingService>();
 
        return services;
    }
}