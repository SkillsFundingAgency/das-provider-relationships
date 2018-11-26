using StructureMap;
using SFA.DAS.ProviderRelationships.Api.Authentication;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class AuthenticationRegistry : Registry
    {
        public AuthenticationRegistry()
        {
            For<IAuthenticationStartup>().Use<AuthenticationStartup>();
        }
    }
}
