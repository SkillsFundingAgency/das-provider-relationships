using System.Collections.Specialized;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ProviderRelationships.Environment
{
    //todo: should we pass individual setting to ctors here or appsettings?
    public class EnvironmentConfiguration : IEnvironmentConfiguration
    {
        private readonly NameValueCollection _appSettings;
        private readonly IEnvironment _environment;
        
        //public EnvironmentConfiguration(Func<NameValueCollection> getAppSettings) // do we need this to pick up latest?
        public EnvironmentConfiguration(NameValueCollection appSettings, IEnvironment environment)
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