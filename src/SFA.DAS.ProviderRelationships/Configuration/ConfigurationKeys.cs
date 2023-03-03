namespace SFA.DAS.ProviderRelationships.Configuration
{
    public static class ConfigurationKeys
    {
        public const string ProviderRelationships = "SFA.DAS.ProviderRelationshipsV2";
        public static string ProviderRelationshipsReadStore => $"{ProviderRelationships}:ReadStore";
        public const string EmployerFeatures = "SFA.DAS.ProviderRelationships.EmployerFeatures";
        public const string EncodingConfig = "SFA.DAS.Encoding";
        public const string AzureActiveDirectoryApiConfiguration = "AzureADApiAuthentication";
    }
}