using System.Security.Claims;
using SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Models;

namespace SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Services;

public interface IOidcService
{
    Token GetToken(string code, string redirectUri);
    void PopulateAccountClaims(ClaimsIdentity claimsIdentity, string accessToken);
}