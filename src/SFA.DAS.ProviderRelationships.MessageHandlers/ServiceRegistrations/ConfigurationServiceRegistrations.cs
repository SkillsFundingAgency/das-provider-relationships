using Microsoft.Extensions.Options;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationSections(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProviderRelationshipsConfiguration>(configuration.GetSection(ConfigurationKeys.ProviderRelationships));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsConfiguration>>().Value);
        services.AddSingleton(configuration.GetSection(ConfigurationKeys.ProviderRelationships).Get<ProviderRelationshipsConfiguration>());

        //var roatpApiClientKey = $"{ConfigurationKeys.ProviderRelationships}:RoatpApiClientSettings";
        //services.Configure<RoatpApiConfiguration>(configuration.GetSection(roatpApiClientKey));
        //services.AddSingleton(configuration.GetSection(roatpApiClientKey).Get<RoatpApiConfiguration>());

        //var readStoreKey = $"{ConfigurationKeys.ProviderRelationships}:ReadStore";
        //services.Configure<ReadStoreConfiguration>(configuration.GetSection(readStoreKey));
        //services.AddSingleton(cfg => cfg.GetService<IOptions<ReadStoreConfiguration>>().Value);

        return services;
    }
}