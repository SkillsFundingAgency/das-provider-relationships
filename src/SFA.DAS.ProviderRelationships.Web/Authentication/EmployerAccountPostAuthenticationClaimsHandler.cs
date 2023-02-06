using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Authorisation;

namespace SFA.DAS.ProviderRelationships.Web.Authentication;

public class EmployerAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    private readonly IEmployerAccountService _accountsSvc;
    private readonly IConfiguration _configuration;
    private readonly ProviderRelationshipsConfiguration _webConfig;

    public EmployerAccountPostAuthenticationClaimsHandler(IEmployerAccountService accountsSvc, IConfiguration configuration, IOptions<ProviderRelationshipsConfiguration> configOptions)
    {
        _accountsSvc = accountsSvc;
        _configuration = configuration;
        _webConfig = configOptions.Value;
    }
    
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext ctx)
    {
        if (_configuration["StubAuth"] != null && _configuration["StubAuth"]
                .Equals("true", StringComparison.CurrentCultureIgnoreCase))
        {
            throw new AuthenticationException("Add missing rule in Startup.cs to use stub auth: services.AddEmployerStubAuthentication();");
        }
        
        string userId;
        var email = string.Empty;
        if (_webConfig.UseGovSignIn)
        {
            userId = ctx.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Value;
            email = ctx.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.Email))
                .Value;
        }
        else
        {
            userId = ctx.Principal.Claims
                .First(c => c.Type.Equals(EmployerClaimTypes.UserId))
                .Value;
        }
            
        var result = await _accountsSvc.GetUserAccounts(userId, email);

        var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
        var associatedAccountsClaim = new Claim(EmployerClaimTypes.AssociatedAccounts, accountsAsJson, JsonClaimValueTypes.Json);
        return new List<Claim> {associatedAccountsClaim};
    }
}