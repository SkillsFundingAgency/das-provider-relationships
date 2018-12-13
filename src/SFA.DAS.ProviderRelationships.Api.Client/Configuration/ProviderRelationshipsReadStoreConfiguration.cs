using SFA.DAS.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
    //todo: main config will contain one of these
    public class ProviderRelationshipsReadStoreConfiguration : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string AuthKey { get; set; }
    }
}