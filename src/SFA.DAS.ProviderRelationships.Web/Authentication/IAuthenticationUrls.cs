namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public interface IAuthenticationUrls
    {
        string AuthorizeEndpoint { get; }
        string LogoutEndpoint { get; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }
    }
}
