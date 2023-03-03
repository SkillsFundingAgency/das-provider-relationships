using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IEmployerUrls, EmployerUrls>();
        services.AddTransient<IRoatpService, RoatpService>();
        services.AddTransient<IDasRecruitService, DasRecruitService>();
        
        return services;
    }
}