using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class GoogleAnalyticsConfigurationFactory : IGoogleAnalyticsConfigurationFactory
    {
        private readonly IEnvironmentService _environmentService;

        public GoogleAnalyticsConfigurationFactory(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public GoogleAnalyticsConfiguration CreateConfiguration()
        {
            var configuration = new GoogleAnalyticsConfiguration();

            if (_environmentService.IsCurrent(DasEnv.PREPROD))
            {
                configuration.ContainerId = "GTM-KWQBWGJ";
                configuration.TrackingId = "UA-83918739-9";
            }
            else if (_environmentService.IsCurrent(DasEnv.PROD))
            {
                configuration.ContainerId = "GTM-KWQBWGJ";
                configuration.TrackingId = "UA‌-83918739-10";
            }

            return configuration;
        }
    }
}