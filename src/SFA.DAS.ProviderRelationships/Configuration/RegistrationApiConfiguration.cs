namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class RegistrationApiConfiguration : IRegistrationApiConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}
