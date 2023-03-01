using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ProviderRelationships.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services, bool isDevelopment = false)
    {
        services.AddAuthorization(x =>
        {
            {
                x.AddPolicy("default", policy =>
                {
                    if (isDevelopment)
                    {
                        policy.AllowAnonymousUser();
                    }
                    else { policy.RequireAuthenticatedUser(); }
                });

                x.DefaultPolicy = x.GetPolicy("default");

                x.AddPolicy(ApiRoles.Read, policy =>
                {
                    if (isDevelopment)
                        policy.AllowAnonymousUser();
                    else
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(ApiRoles.Read);
                    }
                });

                x.AddPolicy(ApiRoles.Write, policy =>
                {
                    if (isDevelopment)
                        policy.AllowAnonymousUser();
                    else
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(ApiRoles.Write);
                    }
                });
            }
        });

        if (isDevelopment){
            services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();
        }

        return services;
    }
}