namespace SFA.DAS.ProviderRelationships.Authentication
{
    //todo: ideally interfaces should live in their own separate project
    public interface IAuthenticationService
    {
        bool IsUserAuthenticated();
        void SignOutUser();
    }
}
