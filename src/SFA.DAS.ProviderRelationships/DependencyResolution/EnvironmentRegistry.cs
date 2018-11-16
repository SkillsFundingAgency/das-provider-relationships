using System.Collections.Specialized;
using System.Configuration;
using SFA.DAS.ProviderRelationships.Environment;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class EnvironmentRegistry : Registry
    {
        public EnvironmentRegistry()
        {
            For<IEnvironment>().Use<Environment.Environment>().Ctor<NameValueCollection>().Is(ConfigurationManager.AppSettings).Singleton();
        }
    }
}