namespace SFA.DAS.ProviderRelationships.Authentication
{
    public class IdentityServerConfiguration : IIdentityServerConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseAddress { get; set; }
        public string AuthorizeEndPoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }
        public bool UseCertificate { get; set; }
        public string Scopes { get; set; }
        public string ChangePasswordUrl { get; set; }
        public string ChangeEmailUrl { get; set; }
        public string AccountActivationUrl { get; set; }
        public string TokenCertificateThumbprint { get; set; }

        #region required by EAS, but not by us (or employercomitments)
        //public string LogoutEndpoint { get; set; }
        //public string RegisterLink { get; set; }
        #endregion
    }
}