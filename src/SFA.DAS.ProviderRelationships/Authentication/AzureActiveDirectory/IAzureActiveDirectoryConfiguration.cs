namespace SFA.DAS.ProviderRelationships.Authentication.AzureActiveDirectory
{
    public interface IAzureActiveDirectoryConfiguration
    {
        string Audience { get; }
        string Tenant { get; }
    }
}