using System.Security.Claims;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public interface IPostAuthenticationHandler
    {
        void Handle(ClaimsIdentity claimsIdentity);
    }
}