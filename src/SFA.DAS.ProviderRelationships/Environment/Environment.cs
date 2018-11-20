using System;
using System.Linq;
using System.Collections.Specialized;

namespace SFA.DAS.ProviderRelationships.Environment
{
    public class Environment : IEnvironment
    {
        public DasEnv Current { get; }
        
        // we could pass individual settings to ctor rather than appsettings
        public Environment(NameValueCollection appSettings)
        {
            var environmentName = appSettings["EnvironmentName"];

            if (!Enum.TryParse(environmentName, out DasEnv currentEnvironment))
                throw new Exception($"Unknown environment name '{environmentName}'");

            Current = currentEnvironment;
        }

        public bool IsCurrent(params DasEnv[] environment)
        {
            return environment.Contains(Current);
        }
    }
}