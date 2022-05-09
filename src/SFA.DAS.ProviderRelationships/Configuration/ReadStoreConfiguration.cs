using SFA.DAS.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class ReadStoreConfiguration : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string AuthKey { get; set; }
    }
}