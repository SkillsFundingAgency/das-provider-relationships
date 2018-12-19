
namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
    //todo: (add a note to the pr description for this..) there are now 2 locations in table storage containing ReadStoreConfiguration, 1 in the api client config (used by the read-only client) and 1 in its own table for the write clients (currently message handlers and jobs)
    // the api client will use a read-only key - plug one in and check it
    public class ProviderRelationshipsApiClientConfiguration
    {
        public AzureAdClientConfiguration AzureAdClient { get; set; }
        public ReadStoreConfiguration ReadStore { get; set; }
    }
}