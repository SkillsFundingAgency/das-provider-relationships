using System.Collections.Specialized;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class Configuration : IConfiguration
    {
        private readonly NameValueCollection _appSettings;
        private readonly IEnvironmentService _environment;
        
        // we could pass individual settings to ctor rather than appsettings
        public Configuration(NameValueCollection appSettings, IEnvironmentService environment)
        {
            _appSettings = appSettings;
            _environment = environment;
        }

        public T Get<T>(string serviceName)
        {
            var storageConnectionString = _appSettings["ConfigurationStorageConnectionString"];
            // new is glue!
            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(serviceName, _environment.GetVariable("EnvironmentName").ToString(), "1.0"));

            return configurationService.Get<T>();
        }
    }
}