using System;
using System.Linq;
using System.Collections.Specialized;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Environment
{
    //todo: should we pass individual setting to ctors here or appsettings?
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
}