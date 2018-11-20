using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class AuthenticationRegistry : Registry
    {
        public AuthenticationRegistry()
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
