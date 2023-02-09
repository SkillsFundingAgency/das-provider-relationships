using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorisation;

namespace SFA.DAS.ProviderRelationships.Web.AppStart;

public static class AddAuthorisationExtensions
{
    public static void AddEmployerAuthorisationServices(
        this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerOwnerAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerViewerAuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                PolicyNames
                    .HasEmployerOwnerAccount
                , policy =>
                {
                    policy.RequireClaim(EmployerClaimTypes.AssociatedAccounts);
                    policy.Requirements.Add(new EmployerOwnerRoleRequirement());
                    policy.RequireAuthenticatedUser();
                });
            options.AddPolicy(
                PolicyNames.HasEmployerViewAccount, policy =>
                {
                    policy.RequireClaim(EmployerClaimTypes.AssociatedAccounts);
                    policy.Requirements.Add(new EmployerViewerRoleRequirement());
                    policy.RequireAuthenticatedUser();
                });
        });
    }
}