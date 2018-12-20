using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            For<EmployerFeaturesConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<EmployerFeaturesConfiguration>("SFA.DAS.ProviderRelationships.EmployerFeatures")).Singleton();
            For<IAzureActiveDirectoryConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().AzureActiveDirectory).Singleton();
            For<IEmployerUrlsConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>()).Singleton();
            For<GoogleAnalyticsConfiguration>().Use(c => c.GetInstance<IGoogleAnalyticsConfigurationFactory>().CreateConfiguration()).Singleton();
            For<IGoogleAnalyticsConfigurationFactory>().Use<GoogleAnalyticsConfigurationFactory>();
            For<IOidcConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().Oidc).Singleton();
            For<ProviderRelationshipsConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships")).Singleton();
            For<SlackConfiguration>().Use(new SlackConfiguration { DasMetricsApiUrl = "https://hooks.slack.com/services/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" });
        }
    }
}