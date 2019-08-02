using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.Microsoft
{
    internal static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            return services.AddSingleton(p => p.GetRequiredService<ProviderRelationshipsApiClientConfiguration>().AzureActiveDirectoryClient)
                .AddSingleton(p => p.GetRequiredService<IAutoConfigurationService>().Get<ProviderRelationshipsApiClientConfiguration>(ConfigurationKeys.ApiClientBase))
                .AddSingleton(p => p.GetRequiredService<ProviderRelationshipsApiClientConfiguration>().ReadStore);
        }
    }
}