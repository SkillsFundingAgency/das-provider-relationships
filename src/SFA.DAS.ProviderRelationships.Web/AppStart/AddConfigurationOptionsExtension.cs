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
        services.Configure<ProviderRelationshipsConfiguration>(configuration.GetSection(nameof(ProviderRelationshipsConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsConfiguration>>().Value);
        services.Configure<OuterApiConfiguration>(configuration.GetSection(nameof(OuterApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<OuterApiConfiguration>>().Value);
        services.Configure<IdentityServerConfiguration>(configuration.GetSection("Identity"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<IdentityServerConfiguration>>().Value);

    }
}