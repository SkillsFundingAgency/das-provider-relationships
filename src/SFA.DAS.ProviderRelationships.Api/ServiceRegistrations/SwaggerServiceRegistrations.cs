using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.ProviderRelationships.Api.Filters;

namespace SFA.DAS.ProviderRelationships.Api.ServiceRegistrations;

public static class SwaggerServiceRegistrations
{
    public static IServiceCollection AddDasSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var securityScheme = new OpenApiSecurityScheme() {
                Description = "Access Token. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            };

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    Array.Empty<string>()
                }
            };

            options.AddSecurityDefinition("bearerAuth", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
            options.OperationFilter<SwaggerVersionHeaderFilter>();
            options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Provider Relationships API" });
        });

        return services;
    }
}