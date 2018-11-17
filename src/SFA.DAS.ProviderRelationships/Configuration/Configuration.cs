using System.Collections.Specialized;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRelationships.Environment;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class Configuration : IConfiguration
    {
        private readonly NameValueCollection _appSettings;
        private readonly IEnvironment _environment;
        
        // we could pass individual settings to ctor rather than appsettings
        public Configuration(NameValueCollection appSettings, IEnvironment environment)
        {
            _appSettings = appSettings;
            _environment = environment;
        }

        public T Get<T>(string serviceName)
        {
            var storageConnectionString = _appSettings["ConfigurationStorageConnectionString"];
            // new is glue!
            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(serviceName, _environment.Current.ToString(), "1.0"));

            return configurationService.Get<T>();
        }
    }
}