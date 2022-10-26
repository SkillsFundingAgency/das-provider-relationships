using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Services;

public class JwtSecurityTokenService : IJwtSecurityTokenService
{
    private readonly string _clientId;
    private readonly string _audience;

    public JwtSecurityTokenService(GovUkOidcConfiguration configuration)
    {
        _clientId = configuration.ClientId;
        _audience = $"{configuration.BaseUrl}token";
    }
    public string CreateToken(ClaimsIdentity claimsIdentity,
        SigningCredentials signingCredentials)
    {
        var handler = new JwtSecurityTokenHandler();
        var value = handler.CreateJwtSecurityToken(_clientId, _audience, claimsIdentity, DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(5), DateTime.UtcNow, signingCredentials);

        return value.RawData;
    }

       
    public bool CanReadToken(string securityToken)
    {
        var handler = new JwtSecurityTokenHandler();
            
        return handler.CanReadToken(securityToken);
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
        out SecurityToken validatedToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var claimsPrincipal = handler.ValidateToken(securityToken, validationParameters, out validatedToken);
            
        return new ClaimsPrincipal(claimsPrincipal);
    }

    public bool CanValidateToken { get; }
    public int MaximumTokenSizeInBytes { get; set; }
}