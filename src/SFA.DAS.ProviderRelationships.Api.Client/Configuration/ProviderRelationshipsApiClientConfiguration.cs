using SFA.DAS.Http;

namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
    public class ProviderRelationshipsApiClientConfiguration : IAzureADClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string IdentifierUri { get; }
        public string Tenant { get; }
    }
}