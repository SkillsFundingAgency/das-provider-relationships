namespace SFA.DAS.ProviderRelationships.Web.Configuration
{
    public class ProviderRelationshipsWebConfiguration : IProviderRelationshipsWebConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IdentifierUri { get; set; }
        public string Tenant { get; set; }
    }

    public interface IProviderRelationshipsWebConfiguration
    {
        string ApiBaseUrl { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string IdentifierUri { get; }
        string Tenant { get; }
    }
}