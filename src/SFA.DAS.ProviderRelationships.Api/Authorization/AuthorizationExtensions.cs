namespace SFA.DAS.ProviderRelationships.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services, bool isDevelopment = false)
    {
        services.AddAuthorization(options =>
        {
            AddDefaultPolicy(isDevelopment, options);

            options.DefaultPolicy = options.GetPolicy("default");

            AddReadPolicy(isDevelopment, options);

            AddWritePolicy(isDevelopment, options);

        });

        if (isDevelopment)
        {
            services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();
        }

        return services;
    }

    private static void AddDefaultPolicy(bool isDevelopment, AuthorizationOptions options)
    {
        options.AddPolicy("default", policy =>
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

    private static void AddWritePolicy(bool isDevelopment, AuthorizationOptions options)
    {
        options.AddPolicy(ApiRoles.Write, policy =>
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

    private static void AddReadPolicy(bool isDevelopment, AuthorizationOptions options)
    {
        options.AddPolicy(ApiRoles.Read, policy =>
        {
            if (isDevelopment)
                policy.AllowAnonymousUser();
            else
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(ApiRoles.Read);
            }
        });
    }
}