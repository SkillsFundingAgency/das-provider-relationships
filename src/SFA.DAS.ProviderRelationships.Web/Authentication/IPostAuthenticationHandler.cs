namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public interface IPostAuthenticationHandler
    {
        Task Handle(ClaimsIdentity claimsIdentity);
    }
}