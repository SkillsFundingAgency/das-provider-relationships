using SFA.DAS.Http;

namespace SFA.DAS.ProviderRelationships.Api.Client.Configuration
{
    public class AzureAdClientConfiguration : IAzureADClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IdentifierUri { get; set; }
        public string Tenant { get; set; }
    }
}