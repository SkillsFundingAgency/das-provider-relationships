using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class ConfigurationHelper
    {
        //todo: unit tests
        public static Environment CurrentEnvironment
        {
            get
            {
                if (!Enum.TryParse(CurrentEnvironmentName, true, out Environment environment))
                    throw new Exception($"Unknown current environment '{CurrentEnvironmentName}'");
                return environment;
            }
        }

        private static string _currentEnvironmentName;
        
        private static string CurrentEnvironmentName
        {
            get
            {
                if (_currentEnvironmentName != null)
                    return _currentEnvironmentName;

                var environmentName = System.Environment.GetEnvironmentVariable("DASENV");

                if (string.IsNullOrEmpty(environmentName))
                    environmentName = CloudConfigurationManager.GetSetting("EnvironmentName");

                return _currentEnvironmentName = environmentName?.ToUpperInvariant();
            }
        }

        public static T GetConfiguration<T>(string serviceName)
        {
            var configurationService = CreateConfigurationService(serviceName);
            return configurationService.Get<T>();
        }

        public static Task<T> GetConfigurationAsync<T>(string serviceName)
        {
            var configurationService = CreateConfigurationService(serviceName);
            return configurationService.GetAsync<T>();
        }

        public static bool IsEnvironment(params Environment[] environment)
        {
            return environment.Contains(CurrentEnvironment);
        }

        private static ConfigurationService CreateConfigurationService(string serviceName)
        {
            var storageConnectionString = CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString");
            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(serviceName, CurrentEnvironmentName, "1.0"));

            return configurationService;
        }
    }
}