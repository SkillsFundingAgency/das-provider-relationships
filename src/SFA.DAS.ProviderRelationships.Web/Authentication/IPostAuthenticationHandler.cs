namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public interface IPostAuthenticationHandler
    {
        Task<IEnumerable<Claim>> Handle(ClaimsIdentity claimsIdentity);
    }
}