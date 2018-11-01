using SFA.DAS.ProviderRelationships.Document.Repository;

namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
    public class ProviderPermissionsConfiguration
    {
        public string Uri { get; set; }
        public string SecurityKey { get; set; }
        public string DatabaseName { get; set; }

        public string ProviderRelationshipCollection { get; set; }

        public IDocumentConfiguration CreateDoumentConfiguraton()
        {
            return new CosmosDbConfiguration(Uri, SecurityKey, DatabaseName);
        }

    }
}