using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ProviderRelationships.Web.AppStart;

public static class AddAuthenticationCookieExtensions
{
    public static void AddAuthenticationCookie(this IServiceCollection services)
    {
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