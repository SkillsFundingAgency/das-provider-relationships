using Microsoft.Extensions.Options;
using SFA.DAS.PAS.Account.Api.ClientV2.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.ServiceRegistrations;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationSections(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ProviderRelationshipsConfiguration>(configuration);
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsConfiguration>>().Value);

        services.AddSingleton<IProviderRelationshipsConfiguration>(configuration.Get<ProviderRelationshipsConfiguration>());

        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsConfiguration>>().Value);

        services.Configure<PasAccountApiConfiguration>(configuration.GetSection("PasAccountApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<PasAccountApiConfiguration>>().Value);

        services.Configure<ReadStoreConfiguration>(configuration.GetSection("ReadStore"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ReadStoreConfiguration>>().Value);

        return services;
    }
}