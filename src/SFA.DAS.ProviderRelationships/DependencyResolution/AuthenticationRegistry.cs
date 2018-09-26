using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Authentication;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class AuthenticationRegistry : Registry
    {
        public AuthenticationRegistry()
        {
            //var providerRelationshipsConfig = ConfigurationHelper.GetConfiguration<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships");

            //For<IIdentityServerConfiguration>().Use(() => providerRelationshipsConfig.Identity).Singleton();
            ////todo: change consumers of this to accept IIdentityServerConfiguration instead?
            //For<IClaimIdentifierConfiguration>().Use(() => providerRelationshipsConfig.Identity.ClaimIdentifierConfiguration).Singleton();

            For<ConfigurationFactory>().Use<IdentityServerConfigurationFactory>();
        }
    }
}
