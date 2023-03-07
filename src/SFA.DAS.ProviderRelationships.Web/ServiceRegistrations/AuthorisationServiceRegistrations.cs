using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorisation;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class AddAuthorisationExtensions
{
    public static void AddEmployerAuthorisationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomClaims, PostAuthenticationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerOwnerAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerViewerAuthorizationHandler>();
        services.AddTransient<IUserAccountService, UserAccountService>();
        services.AddTransient<IEmployerAccountAuthorizationHandler, EmployerAccountAuthorizationHandler>();
        
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

            // TODO fix authorization policies
            options.AddPolicy(EmployerUserRole.Any, policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        });
    }
}