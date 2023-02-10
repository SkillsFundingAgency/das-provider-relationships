using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.Managers;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.Managers;

namespace SFA.DAS.ProviderRelationships.Web.AppStart;

public static class AddServiceRegistrationsExtensions
{
    public static void AddServiceRegistration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        services.AddHttpClient<IOuterApiClient, OuterApiClient>();
        services.AddScoped<ICustomClaims, PostAuthenticationHandler>();
        services.AddUnitOfWork();
        services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();//why is this not done in extn above? :sadness:
    }
}