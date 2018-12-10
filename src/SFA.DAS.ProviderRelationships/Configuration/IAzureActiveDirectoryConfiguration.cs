namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IAzureActiveDirectoryConfiguration
    {
        string Audience { get; }
        string Tenant { get; }
    }
}