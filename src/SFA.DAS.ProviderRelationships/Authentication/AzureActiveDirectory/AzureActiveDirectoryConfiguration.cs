namespace SFA.DAS.ProviderRelationships.Authentication.AzureActiveDirectory
{
    public class AzureActiveDirectoryConfiguration : IAzureActiveDirectoryConfiguration
    {
        public string Audience { get; set; }
        public string Tenant { get; set; }
    }
}