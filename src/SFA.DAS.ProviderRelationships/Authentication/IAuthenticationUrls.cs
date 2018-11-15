namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IAuthenticationUrls
    {
        string AuthorizeEndpoint { get; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }
    }
}
