using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Authentication.Oidc.DependencyResolution
{
    public class OidcAuthenticationRegistry : Registry
    {
        public OidcAuthenticationRegistry()
        {
            For<ConfigurationFactory>().Use<OidcConfigurationFactory>();
            For<IAuthenticationUrls>().Use<AuthenticationUrls>();
            For<IAuthenticationService>().Use<OwinAuthenticationService>();
            For<IAuthenticationStartup>().Use<AuthenticationStartup>();
            For<IOidcConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().Oidc).Singleton();
            For<IPostAuthenticationHandler>().Use<PostAuthenticationHandler>();
        }
    }
}
