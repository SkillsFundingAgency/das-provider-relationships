using System;
using System.Configuration;
using System.Linq;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public static class ConfigurationHelper
    {
        public static DasEnv CurrentEnvironment
        {
            get
            {
                if (_currentEnvironment == null)
                {
                    var environmentName = ConfigurationManager.AppSettings["EnvironmentName"];

                    if (!Enum.TryParse(environmentName, out DasEnv currentEnvironment))
                        throw new Exception($"Unknown environment name '{environmentName}'");

                    _currentEnvironment = currentEnvironment;
                }

                return _currentEnvironment.Value;
            }
        }

        private static DasEnv? _currentEnvironment;

        public static T GetConfiguration<T>(string serviceName)
        {
            var storageConnectionString = ConfigurationManager.AppSettings["ConfigurationStorageConnectionString"];
            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(serviceName, CurrentEnvironment.ToString(), "1.0"));

            return configurationService.Get<T>();
        }

        public static bool IsCurrentEnvironment(params DasEnv[] environment)
        {
            return environment.Contains(CurrentEnvironment);
        }
    }
}