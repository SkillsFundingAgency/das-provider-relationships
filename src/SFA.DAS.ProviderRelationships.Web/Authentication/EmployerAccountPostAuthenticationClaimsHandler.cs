using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRelationships.Web.Authentication;

public class EmployerAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    private readonly ProviderRelationshipsConfiguration _employerFinanceConfiguration;
    private readonly IUserAccountService _userAccountService;

    public EmployerAccountPostAuthenticationClaimsHandler(IUserAccountService userAccountService,
        IOptions<ProviderRelationshipsConfiguration> providerRelationshipsConfiguration)
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

            claims.Add(new Claim(EmployerClaims.IdamsUserEmailClaimTypeIdentifier, email));
        }
        else
        {
            userId = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(EmployerClaims.IdamsUserIdClaimTypeIdentifier))
                .Value;

            email = tokenValidatedContext.Principal.Claims
                .First(c => c.Type.Equals(EmployerClaims.IdamsUserEmailClaimTypeIdentifier)).Value;

            claims.AddRange(tokenValidatedContext.Principal.Claims);
            claims.Add(new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, userId));
        }

        var result = await _userAccountService.GetUserAccounts(userId, email);

        // TODO: This needs removing and was only added back into this area to facilitate the completion of the NET6 upgrade work.
        // If provider-relationships is going to keep a local cache of users then it needs a better way to keep it in sync
        var unitOfWorkScope = tokenValidatedContext.HttpContext.RequestServices?.GetService<IUnitOfWorkScope>();
        await SaveUser(unitOfWorkScope, result, email);

        if (result.IsSuspended)
        {
            claims.Add(new Claim(ClaimTypes.AuthorizationDecision, "Suspended"));
        }

        var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
        var associatedAccountsClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, accountsAsJson,
            JsonClaimValueTypes.Json);

        claims.Add(associatedAccountsClaim);

        if (!_employerFinanceConfiguration.UseGovUkSignIn)
        {
            return claims;
        }

        claims.Add(new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, result.EmployerUserId));
        claims.Add(new Claim(EmployerClaims.IdamsUserDisplayNameClaimTypeIdentifier,
            $"{result.FirstName} {result.LastName}"));

        return claims;
    }

    private static async Task SaveUser(IUnitOfWorkScope unitOfWorkScope, EmployerUserAccounts account, string email)
    {
        // Just for unit testing purposes ...
        if (unitOfWorkScope == null)
        {
            return;
        }

        var command = new CreateOrUpdateUserCommand(
            Guid.Parse(account.EmployerUserId),
            email,
            account.FirstName,
            account.LastName
        );

        await unitOfWorkScope.RunAsync(c =>
        {
            var mediator = c.GetService<IMediator>();
            return mediator.Send(command);
        });
    }
}