using Newtonsoft.Json;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationSections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ProviderRelationshipsConfiguration>(configuration.GetSection(ConfigurationKeys.ProviderRelationships));

        services.AddSingleton(configuration.Get<ProviderRelationshipsConfiguration>());

        services.Configure<ReadStoreConfiguration>(configuration.GetSection("ReadStore"));
        services.AddSingleton(configuration.GetSection("ReadStore").Get<ReadStoreConfiguration>());

        var encodingConfigJson = configuration.GetSection(ConfigurationKeys.EncodingConfig).Value;
        var encodingConfig = JsonConvert.DeserializeObject<EncodingConfig>(encodingConfigJson);
        services.AddSingleton(encodingConfig);

        return services;
    }
}