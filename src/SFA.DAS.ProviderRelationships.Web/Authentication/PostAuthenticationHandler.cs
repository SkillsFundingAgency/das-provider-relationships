using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authorisation;
using GetUserAccountsResponse = SFA.DAS.ProviderRelationships.Services.OuterApi.GetUserAccountsResponse;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class PostAuthenticationHandler : IPostAuthenticationHandler, ICustomClaims
    {
        private readonly IOuterApiClient _outerApiClient;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;

        public PostAuthenticationHandler(
            IOuterApiClient outerApiClient,
            IMediator mediator,
            IOptions<ProviderRelationshipsConfiguration> options,
            IConfiguration configuration)
        {
            _outerApiClient = outerApiClient;
            _mediator = mediator;
            _configuration = configuration;
            _providerRelationshipsConfiguration = options.Value;
        }

        public async Task<IEnumerable<Claim>> Handle(ClaimsIdentity claimsIdentity)
        {
            if (_providerRelationshipsConfiguration.UseGovUkSignIn)
            {
                var userId = claimsIdentity.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var email = claimsIdentity.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
                var request = new GetEmployerAccountRequest(userId, email);

                var apiResponse = await _outerApiClient.Get<GetUserAccountsResponse>(request);

                var accountsAsJson = JsonConvert.SerializeObject(apiResponse.UserAccounts.ToDictionary(k => k.AccountId));
                claimsIdentity.AddClaim(new Claim(EmployerClaimTypes.AssociatedAccounts, accountsAsJson, JsonClaimValueTypes.Json));
                claimsIdentity.AddClaim(new Claim(EmployerClaimTypes.UserId, apiResponse.EmployerUserId));
                claimsIdentity.AddClaim(new Claim(EmployerClaimTypes.EmailAddress, email));
                claimsIdentity.AddClaim(new Claim(EmployerClaimTypes.GivenName, apiResponse.FirstName));
                claimsIdentity.AddClaim(new Claim(EmployerClaimTypes.FamilyName, apiResponse.LastName));

                var upsertUserCommand = new CreateOrUpdateUserCommand(
                    Guid.Parse(apiResponse.EmployerUserId),
                    email,
                    apiResponse.FirstName,
                    apiResponse.LastName);

                await _mediator.Send(upsertUserCommand);
            }
            else
            {
                var @ref = Guid.Parse(claimsIdentity.FindFirst(EmployerClaimTypes.UserId).Value);
                var email = claimsIdentity.FindFirst(EmployerClaimTypes.EmailAddress).Value;
                var firstName = claimsIdentity.FindFirst(EmployerClaimTypes.GivenName).Value;
                var lastName = claimsIdentity.FindFirst(EmployerClaimTypes.FamilyName).Value;

                var command = new CreateOrUpdateUserCommand(@ref, email, firstName, lastName);

                await _mediator.Send(command);
            }

            return claimsIdentity.Claims;
        }

        public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext tokenValidatedContext)
        {
            return await Handle(tokenValidatedContext.Principal.Identities.First());
        }
    }
}