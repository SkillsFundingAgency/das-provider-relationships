namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IAuthenticationUrls
    {
        string AuthorizeEndpoint { get; }
        string ChangePasswordUrl { get; }
        string ChangeEmailUrl { get; }
        string LogoutEndpoint { get; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }
    }
}
