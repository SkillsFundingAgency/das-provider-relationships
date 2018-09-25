namespace SFA.DAS.ProviderRelationships.Authentication.Interfaces
{
    //todo: ideally interfaces should live in their own separate project
    public interface IAuthenticationService
    {
        string TryGetCurrentUserClaimValue(string key);
        bool IsUserAuthenticated();
        void SignOutUser();
    }
}
