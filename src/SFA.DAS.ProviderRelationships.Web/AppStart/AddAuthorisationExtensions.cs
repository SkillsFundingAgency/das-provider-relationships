using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ProviderRelationships.Web.AppStart;

public static class AddAuthorisationExtensions
{
    public static void AddEmployerAuthenticationServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, EmployerOwnerAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerTransactorAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerViewerAuthorizationHandler>();
    }
}