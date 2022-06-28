namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IRegistrationApiConfiguration
    {
        string ApiBaseUrl { get; set; }
        string IdentifierUri { get; set; }
    }
}
