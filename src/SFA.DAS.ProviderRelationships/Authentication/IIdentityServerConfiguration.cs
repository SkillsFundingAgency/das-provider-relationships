namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IIdentityServerConfiguration
    {
        string AccountActivationUrl { get; }
        string AuthorizeEndpoint { get; }
        string BaseAddress { get; }
        string ChangeEmailUrl { get; }
        string ChangePasswordUrl { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string LogoutEndpoint { get; }
        string Scopes { get; }
        string TokenCertificateThumbprint { get; }
        string TokenEndpoint { get; }
        bool UseCertificate { get; }
        string UserInfoEndpoint { get; }
    }
}