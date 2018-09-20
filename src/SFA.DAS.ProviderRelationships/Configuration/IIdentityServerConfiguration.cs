
namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IIdentityServerConfiguration
    {
        string ClientId { get; }
        string ClientSecret { get; }
        string BaseAddress { get; }
        string AuthorizeEndPoint { get; }
        //string LogoutEndpoint { get; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }
        bool UseCertificate { get; }
        string Scopes { get; }
        ClaimIdentifierConfiguration ClaimIdentifierConfiguration { get; }
        //string RegisterLink { get; }
        string AccountActivationUrl { get; }
    }
}
