namespace SFA.DAS.ProviderRelationships.Authentication.Oidc
{
    public class OidcConfiguration : IOidcConfiguration
    {
        public string AccountActivationUrl { get; set; }
        public string AuthorizeEndpoint { get; set; }
        public string BaseAddress { get; set; }
        public string ChangeEmailUrl { get; set; }
        public string ChangePasswordUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string LogoutEndpoint { get; set; }
        public string Scopes { get; set; }
        public string TokenCertificateThumbprint { get; set; }
        public string TokenEndpoint { get; set; }
        public bool UseCertificate { get; set; }
        public string UserInfoEndpoint { get; set; }
    }
}