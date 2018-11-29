namespace SFA.DAS.ProviderRelationships.Authentication.Oidc
{
    public interface IAuthenticationUrls
    {
        string AuthorizeEndpoint { get; }
        string LogoutEndpoint { get; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }
    }
}
