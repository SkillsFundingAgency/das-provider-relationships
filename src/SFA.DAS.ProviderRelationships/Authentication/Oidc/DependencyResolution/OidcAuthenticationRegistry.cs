using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Authentication.Oidc.DependencyResolution
{
    public class OidcAuthenticationRegistry : Registry
    {
        public OidcAuthenticationRegistry()
        {
            For<ConfigurationFactory>().Use<IdentityServerConfigurationFactory>();
            For<IAuthenticationUrls>().Use<AuthenticationUrls>();
            For<IAuthenticationService>().Use<OwinAuthenticationService>();
            For<IAuthenticationStartup>().Use<AuthenticationStartup>();
            For<IIdentityServerConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().Identity).Singleton();
            For<IPostAuthenticationHandler>().Use<PostAuthenticationHandler>();
        }
    }
}
