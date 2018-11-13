using System;
using System.Linq;
using System.Collections.Specialized;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Environment
{
    public class Environment : IEnvironment
    {
        private readonly NameValueCollection _appSettings;
        private DasEnv? _current;
        
        // we could pass individual settings to ctor rather than appsettings
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