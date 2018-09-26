using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Configuration.Interfaces;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            var providerRelationshipsConfig = ConfigurationHelper.GetConfiguration<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships");

            For<ProviderRelationshipsConfiguration>().Use(() => providerRelationshipsConfig).Singleton();
            For<IIdentityServerConfiguration>().Use(() => providerRelationshipsConfig.Identity).Singleton();
            //todo: change consumers of this to accept IIdentityServerConfiguration instead?
            For<IClaimIdentifierConfiguration>().Use(() => providerRelationshipsConfig.Identity.ClaimIdentifierConfiguration).Singleton();
        }
    }
}