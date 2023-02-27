using Microsoft.Extensions.Options;
using SFA.DAS.PAS.Account.Api.ClientV2.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using ConfigurationKeys = SFA.DAS.ProviderRelationships.Configuration.ConfigurationKeys;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationSections(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProviderRelationshipsConfiguration>(configuration.GetSection(ConfigurationKeys.ProviderRelationships));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsConfiguration>>().Value);
        services.AddSingleton(configuration.GetSection(ConfigurationKeys.ProviderRelationships).Get<ProviderRelationshipsConfiguration>());

        services.Configure<PasAccountApiConfiguration>(configuration.GetSection($"{ConfigurationKeys.ProviderRelationships}:PasAccountApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<PasAccountApiConfiguration>>().Value);

        var readStoreKey = $"{ConfigurationKeys.ProviderRelationships}:ReadStore";
        services.Configure<ReadStoreConfiguration>(configuration.GetSection(readStoreKey));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ReadStoreConfiguration>>().Value);

        return services;
    }
}