using SFA.DAS.ProviderRelationships.Document.Repository;

namespace SFA.DAS.ProviderRelationships.ReadStore.Configuration
{
    public class ProviderRelationshipsReadStoreConfiguration : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string SecurityKey { get; set; }
    }
}