using SFA.DAS.Api.Common.Infrastructure;

namespace SFA.DAS.ProviderRelationships.Api.Authorization;

public static class AuthorizationExtensions
{
    private static readonly string[] PolicyRoles =
    {
        ApiRoles.Read,
        ApiRoles.Write,
    };

    private const string DefaultPolicyName = "default";

    public static IServiceCollection AddApiAuthorization(this IServiceCollection services, bool isDevelopment = false)
    {
        services.AddAuthorization(options =>
        {
            AddDefaultPolicy(isDevelopment, options);

            AddRolePolicies(isDevelopment, options);

            options.DefaultPolicy = options.GetPolicy(DefaultPolicyName);           
        });

        if (isDevelopment)
        {
            services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();
        }

        return services;
    }

    private static void AddDefaultPolicy(bool isDevelopment, AuthorizationOptions options)
    {
        options.AddPolicy(DefaultPolicyName, policy =>
        {
            if (isDevelopment)
            {
                policy.AllowAnonymousUser();
            }
            else
            {
                policy.RequireAuthenticatedUser();
            }
        });
    }

    private static void AddRolePolicies(bool isDevelopment, AuthorizationOptions options)
    {
        foreach (var roleName in PolicyRoles)
        {
            options.AddPolicy(roleName, policy =>
            {
                if (isDevelopment)
                {
                    policy.AllowAnonymousUser();
                }
                else
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(roleName);
                }
            });
        }
    }
}