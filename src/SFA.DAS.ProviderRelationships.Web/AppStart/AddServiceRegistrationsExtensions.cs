using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authentication;

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
        //todo services.AddTransient<IEmployerAccountService, EmployerAccountService>();
        services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();
    }
}