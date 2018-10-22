using System.Security.Claims;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public interface IPostAuthenticationHandler
    {
        void Handle(ClaimsIdentity claimsIdentity);
    }
}