namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public class AuthenticationSettings : IAuthenticationSettings
    {
        public string MetadataAddress { get; set; }
        public string Wtrealm { get; set; }
    }
}