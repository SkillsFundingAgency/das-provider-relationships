using SFA.DAS.ProviderRelationships.Api.Authentication;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class AzureActiveDirectoryAuthenticationRegistry : Registry
    {
        public AzureActiveDirectoryAuthenticationRegistry()
        {
            For<IAuthenticationStartup>().Use<AuthenticationStartup>();
        }
    }
}
