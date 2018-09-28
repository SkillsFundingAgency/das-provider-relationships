
namespace SFA.DAS.ProviderRelationships.Configuration
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
        public string ChangePasswordLink { get; set; }
        public string ChangeEmailLink { get; set; }
        public string AccountActivationUrl { get; set; }

        #region required by EAS, but not by us (or employercomitments)
        //public string LogoutEndpoint { get; set; }
        //public string RegisterLink { get; set; }
        #endregion
    }
}