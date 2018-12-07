using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.DependencyResolution
{
    public class OidcAuthenticationRegistry : Registry
    {
        public OidcAuthenticationRegistry()
        {
            For<ConfigurationFactory>().Use<OidcConfigurationFactory>();
            For<IAuthenticationUrls>().Use<AuthenticationUrls>();
            For<IAuthenticationService>().Use<OwinAuthenticationService>();
            For<IAuthenticationStartup>().Use<AuthenticationStartup>();
        }
    }
}
