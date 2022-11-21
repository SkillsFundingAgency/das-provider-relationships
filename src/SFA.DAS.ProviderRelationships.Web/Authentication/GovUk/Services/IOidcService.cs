using System.Security.Claims;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Models;

namespace SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Services
{
    public interface IOidcService
    {
        Token GetToken(string code, string redirectUri);
        Task PopulateAccountClaims(ClaimsIdentity claimsIdentity, string accessToken);
    }
}