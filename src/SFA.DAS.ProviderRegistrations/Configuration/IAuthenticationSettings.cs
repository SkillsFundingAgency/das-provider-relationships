namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public interface IAuthenticationSettings
    {
        string MetadataAddress { get; set; }
        string Wtrealm { get; set; }
    }
}
