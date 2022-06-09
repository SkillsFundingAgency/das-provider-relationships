namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IRegistrationApiConfiguration
    {
        string BaseUrl { get; set; }
        string IdentifierUri { get; set; }
    }
}
