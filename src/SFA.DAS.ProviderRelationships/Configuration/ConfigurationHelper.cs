using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IEnvironment //(Service?)
    {
        DasEnv Current { get; }
        bool IsCurrent(params DasEnv[] environment);
    }

    // should we pass individual setting to ctors here or appsettings?
    
    public class Environment : IEnvironment
    {
        private readonly NameValueCollection _appSettings;
        private DasEnv? _current;
        
        //public Environment(Func<NameValueCollection> getAppSettings) // do we need this to pick up latest?
        public Environment(NameValueCollection appSettings)
        {
            //todo: calc current here (to avoid possible multiple settings of _current)?
            _appSettings = appSettings;
        }
        
        public DasEnv Current
        {
            get
            {
                if (_current == null)
                {
                    var environmentName = _appSettings["EnvironmentName"];

                    if (!Enum.TryParse(environmentName, out DasEnv currentEnvironment))
                        throw new Exception($"Unknown environment name '{environmentName}'");

                    _current = currentEnvironment;
                }

                return _current.Value;
            }
        }

        public bool IsCurrent(params DasEnv[] environment)
        {
            return environment.Contains(Current);
        }
    }

    public interface IEnvironmentConfiguration
    {
        T Get<T>(string serviceName);
    }

    //ConfigurationFactory?
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
            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);
            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(serviceName, _environment.Current.ToString(), "1.0"));

            return configurationService.Get<T>();
        }
    }
    
//    public static class ConfigurationHelper
//    {
//        public static DasEnv CurrentEnvironment
//        {
//            get
//            {
//                if (_currentEnvironment == null)
//                {
//                    var environmentName = ConfigurationManager.AppSettings["EnvironmentName"];
//
//                    if (!Enum.TryParse(environmentName, out DasEnv currentEnvironment))
//                        throw new Exception($"Unknown environment name '{environmentName}'");
//
//                    _currentEnvironment = currentEnvironment;
//                }
//
//                return _currentEnvironment.Value;
//            }
//        }
//
//        private static DasEnv? _currentEnvironment;
//
//        public static T GetConfiguration<T>(string serviceName)
//        {
//            var storageConnectionString = ConfigurationManager.AppSettings["ConfigurationStorageConnectionString"];
//            var configurationRepository = new AzureTableStorageConfigurationRepository(storageConnectionString);
//            var configurationService = new ConfigurationService(configurationRepository, new ConfigurationOptions(serviceName, CurrentEnvironment.ToString(), "1.0"));
//
//            return configurationService.Get<T>();
//        }
//
//        public static bool IsCurrentEnvironment(params DasEnv[] environment)
//        {
//            return environment.Contains(CurrentEnvironment);
//        }
//    }
}