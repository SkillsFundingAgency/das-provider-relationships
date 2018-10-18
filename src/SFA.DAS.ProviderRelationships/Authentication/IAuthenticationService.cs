namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IAuthenticationService
    {
        bool IsUserAuthenticated();
        void SignOutUser();
        bool TryGetCurrentUserClaimValue(string key, out string value);
    }
}
