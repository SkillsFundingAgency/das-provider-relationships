using SFA.DAS.AutoConfiguration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
    //todo: (add a note to the pr description for this..) there are now 2 locations in table storage containing ReadStoreConfiguration, 1 in the api client config (used by the read-only client) and 1 in its own table for the write clients (currently message handlers and jobs)
    // the api client will use a read-only key - plug one in and check it
    //todo: this is going to have to go in as a V2 (with the original left in for the first release) so that there is no outage
    public class ProviderRelationshipsApiClientConfiguration
    {
        public AzureAdClientConfiguration AzureAdClient { get; set; }
        public ReadStoreConfiguration ReadStore { get; set; }
        
        public static ProviderRelationshipsApiClientConfiguration Get(IContext context)
        {
            const string providerRelationshipsApiClientConfigurationRowKey = "SFA.DAS.ProviderRelationships.Api.Client_V2";

            return _instance ?? (_instance =
                       context.GetInstance<ITableStorageConfigurationService>()
                           .Get<ProviderRelationshipsApiClientConfiguration>(
                               providerRelationshipsApiClientConfigurationRowKey));
        }

        private static ProviderRelationshipsApiClientConfiguration _instance;
    }
}