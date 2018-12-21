using SFA.DAS.AutoConfiguration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
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