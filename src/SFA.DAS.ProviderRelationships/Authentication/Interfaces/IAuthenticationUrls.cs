
namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IAuthenticationUrls
    {
        string AuthorizeEndpoint { get; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }

        string ChangePasswordLink { get; }
        string ChangeEmailLink { get; }
    }
}
