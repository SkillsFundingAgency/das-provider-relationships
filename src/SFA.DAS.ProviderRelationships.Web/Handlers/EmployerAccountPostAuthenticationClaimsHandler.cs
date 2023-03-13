﻿using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Handlers;

public class EmployerAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    private readonly ProviderRelationshipsConfiguration _employerFinanceConfiguration;
    private readonly IUserAccountService _userAccountService;

    public EmployerAccountPostAuthenticationClaimsHandler(IUserAccountService userAccountService, IOptions<ProviderRelationshipsConfiguration> providerRelationshipsConfiguration)
    {
        _userAccountService = userAccountService;
        _employerFinanceConfiguration = providerRelationshipsConfiguration.Value;
    }

    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
    {
        var claims = new List<Claim>();
        
        
        string userId;
        var email = string.Empty;
            
        if (_employerFinanceConfiguration.UseGovUkSignIn)
        {
            userId = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Value;
            email = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(ClaimTypes.Email))
                .Value;
            claims.Add(new Claim(EmployerClaimTypes.IdamsUserEmailClaimTypeIdentifier, email));
        }
        else
        {
            userId = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(EmployerClaimTypes.IdamsUserIdClaimTypeIdentifier))
                .Value;
        }
                
        var result = await _userAccountService.GetUserAccounts(userId, email);

        var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
        var associatedAccountsClaim = new Claim(EmployerClaimTypes.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);
        claims.Add(associatedAccountsClaim);
        if (!_employerFinanceConfiguration.UseGovUkSignIn)
        {
            return claims;
        }
                
        claims.Add(new Claim(EmployerClaimTypes.IdamsUserIdClaimTypeIdentifier, result.EmployerUserId));
        claims.Add(new Claim(EmployerClaimTypes.IdamsUserDisplayNameClaimTypeIdentifier, result.FirstName + " " + result.LastName));
            
        return claims;
            
    }
}