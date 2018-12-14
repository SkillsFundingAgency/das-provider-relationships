namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class AzureActiveDirectoryConfiguration : IAzureActiveDirectoryConfiguration
    {
        public string Audience { get; set; }
        public string Tenant { get; set; }
    }
}