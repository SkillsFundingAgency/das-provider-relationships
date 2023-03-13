using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorisation;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class AddAuthorisationExtensions
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomClaims, PostAuthenticationHandler>();
        services.AddTransient<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorisationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerOwnerAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerViewerAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerAllRolesAuthorizationHandler>();
        services.AddTransient<IUserAccountService, UserAccountService>();
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.HasEmployerOwnerAccount, policy =>
                {
                    policy.RequireClaim(EmployerClaimTypes.AssociatedAccounts);
                    policy.Requirements.Add(new EmployerOwnerRoleRequirement());
                    policy.RequireAuthenticatedUser();
                });

            options.AddPolicy(PolicyNames.HasEmployerViewAccount, policy =>
                {
                    policy.RequireClaim(EmployerClaimTypes.AssociatedAccounts);
                    policy.Requirements.Add(new EmployerViewerRoleRequirement());
                    policy.RequireAuthenticatedUser();
                });

            options.AddPolicy(PolicyNames.HasEmployerOwnerOrViewerAccount, policy =>
            {
                policy.RequireClaim(EmployerClaimTypes.AssociatedAccounts);
                policy.Requirements.Add(new EmployerAccountAllRolesRequirement());
                policy.RequireAuthenticatedUser();
            });
        });
    }
}