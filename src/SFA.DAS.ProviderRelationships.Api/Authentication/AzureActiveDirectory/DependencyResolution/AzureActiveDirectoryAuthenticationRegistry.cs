using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Authentication.AzureActiveDirectory;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Authentication.AzureActiveDirectory.DependencyResolution
{
    public class AzureActiveDirectoryAuthenticationRegistry : Registry
    {
        public AzureActiveDirectoryAuthenticationRegistry()
        {
            For<IAuthenticationStartup>().Use<AuthenticationStartup>();
            For<IAzureActiveDirectoryConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().AzureActiveDirectory).Singleton();
        }
    }
}
