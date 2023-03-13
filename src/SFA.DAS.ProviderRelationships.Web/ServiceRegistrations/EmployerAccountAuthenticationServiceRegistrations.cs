using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations
{
    public static class AddEmployerAccountAuthenticationExtensions
    {
        public static void AddAndConfigureEmployerAuthentication(
            this IServiceCollection services,
            IdentityServerConfiguration configuration)
        {
            services
                .AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;

                }).AddOpenIdConnect(options =>
                {
                    options.ClientId = configuration.ClientId;
                    options.ClientSecret = configuration.ClientSecret;
                    options.Authority = configuration.BaseAddress;
                    options.MetadataAddress = $"{configuration.BaseAddress}/.well-known/openid-configuration";
                    options.ResponseType = "code";
                    options.UsePkce = false;

                    options.Scope.Add("openid");
                    options.Scope.Add("profile");

                    options.ClaimActions.MapUniqueJsonKey("sub", "id");
                    options.Events.OnRemoteFailure = c =>
                    {
                        if (c.Failure.Message.Contains("Correlation failed"))
                        {
                            c.Response.Redirect("/");
                            c.HandleResponse();
                        }

                        return Task.CompletedTask;
                    };
                });

            services
                .AddOptions<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme)
                .Configure<ICustomClaims>((options, customClaims) =>
                {
                    options.Events.OnTokenValidated = async (ctx) =>
                    {
                        var claims = await customClaims.GetClaims(ctx);
                        ctx.Principal.Identities.First().AddClaims(claims);
                    };
                });

            services.AddAuthentication().AddCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/error/403");
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Cookie.Name = $"provider-relationships";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.CookieManager = new ChunkingCookieManager { ChunkSize = 3000 };
            });
        }
    }
}