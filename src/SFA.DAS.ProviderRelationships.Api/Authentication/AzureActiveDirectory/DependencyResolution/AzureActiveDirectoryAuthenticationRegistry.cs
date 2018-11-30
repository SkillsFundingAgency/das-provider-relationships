using SFA.DAS.ProviderRelationships.Authentication;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Authentication.AzureActiveDirectory.DependencyResolution
{
    public class AzureActiveDirectoryAuthenticationRegistry : Registry
    {
        public AzureActiveDirectoryAuthenticationRegistry()
        {
            For<IAuthenticationStartup>().Use<AuthenticationStartup>();
        }
    }
}
