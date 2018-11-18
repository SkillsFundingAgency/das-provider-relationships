using SFA.DAS.CosmosDb;

namespace SFA.DAS.ProviderRelationships.ReadStore.Configuration
{
    public class ProviderRelationshipsReadStoreConfiguration : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string AuthKey { get; set; }
        public string DatabaseName { get; set; }
    }
}