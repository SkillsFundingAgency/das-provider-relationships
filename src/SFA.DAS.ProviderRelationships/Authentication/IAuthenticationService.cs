namespace SFA.DAS.ProviderRelationships.Authentication
{
    //todo: ideally interfaces should live in their own separate project
    public interface IAuthenticationService
    {
        string GetCurrentUserClaimValue(string key);
        //bool TryGetCurrentUserClaimValue(string key, out string value);//todo: return 2 values?
        bool IsUserAuthenticated();
        void SignOutUser();
        //Task UpdateClaims();
    }
}
