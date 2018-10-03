namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IIdentityServerConfiguration
    {
        string ClientId { get; }
        string ClientSecret { get; }
        string BaseAddress { get; }
        string AuthorizeEndPoint { get; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }
        bool UseCertificate { get; }
        string Scopes { get; }
        string ChangePasswordUrl { get; }
        string ChangeEmailUrl { get; }
        string AccountActivationUrl { get; }
    }
}