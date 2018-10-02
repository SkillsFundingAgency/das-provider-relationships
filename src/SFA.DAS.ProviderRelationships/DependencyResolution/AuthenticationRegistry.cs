using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Authentication;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class AuthenticationRegistry : Registry
    {
        public AuthenticationRegistry()
        {
            For<ConfigurationFactory>().Use<IdentityServerConfigurationFactory>();
        }
    }
}
