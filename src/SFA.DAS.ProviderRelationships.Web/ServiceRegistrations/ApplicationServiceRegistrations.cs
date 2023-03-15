using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.Managers;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.Managers;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

        services.AddUnitOfWork();
        services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();//why is this not done in extn above? :sadness:

        services.AddTransient<IEmployerUrls, EmployerUrls>();
        services.AddTransient<IRoatpService, RoatpService>();
        services.AddTransient<IDasRecruitService, DasRecruitService>();
        services.AddTransient<IEncodingService, EncodingService>();

        return services;
    }
}