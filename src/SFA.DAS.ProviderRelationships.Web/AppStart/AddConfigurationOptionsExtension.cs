using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ProviderRelationshipsConfiguration>(configuration.GetSection(nameof(ApimDeveloperWeb)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApimDeveloperWeb>>().Value);
        services.Configure<ApimDeveloperApi>(configuration.GetSection(nameof(ApimDeveloperApi)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApimDeveloperApi>>().Value);
        services.Configure<IdentityServerConfiguration>(configuration.GetSection("Identity"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<IdentityServerConfiguration>>().Value);

    }
}