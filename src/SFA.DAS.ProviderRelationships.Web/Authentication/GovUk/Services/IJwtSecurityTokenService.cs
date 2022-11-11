using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Services
{
    public interface IJwtSecurityTokenService
    {
        string CreateToken(ClaimsIdentity claimsIdentity,
            SigningCredentials signingCredentials);
    }
}