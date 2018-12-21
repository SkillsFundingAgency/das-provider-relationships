using SFA.DAS.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
    //todo: use this config in the main provider relationships assembly, as its version is identical, an it already has a reference to the api.client assembly
    public class ReadStoreConfiguration : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string AuthKey { get; set; }
    }
}