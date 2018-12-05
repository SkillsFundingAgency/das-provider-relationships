using System.Security.Claims;

namespace SFA.DAS.ProviderRelationships.Authentication.Oidc
{
    public interface IPostAuthenticationHandler
    {
        void Handle(ClaimsIdentity claimsIdentity);
    }
}