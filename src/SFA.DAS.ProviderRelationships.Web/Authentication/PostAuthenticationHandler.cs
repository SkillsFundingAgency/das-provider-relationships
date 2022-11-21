using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authentication.GovUk;
using SFA.DAS.UnitOfWork.DependencyResolution.StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class PostAuthenticationHandler : IPostAuthenticationHandler
    {
        private readonly IUnitOfWorkScope _unitOfWorkScope;
        private readonly IOuterApiClient _outerApiClient;
        private readonly ProviderRelationshipsConfiguration _config;

        public PostAuthenticationHandler(IUnitOfWorkScope unitOfWorkScope, 
            IOuterApiClient outerApiClient,
            ProviderRelationshipsConfiguration config)
        {
            _unitOfWorkScope = unitOfWorkScope;
            _outerApiClient = outerApiClient;
            _config = config;
        }

        public async Task Handle(ClaimsIdentity claimsIdentity)
        {
            if (_config.UseGovUkSignIn.HasValue && _config.UseGovUkSignIn.Value)
            {
                var userId = claimsIdentity.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                var email = claimsIdentity.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
                var request = new GetEmployerAccountRequest(userId, email);
                
                var apiResponse = await _outerApiClient.Get<GetUserAccountsResponse>(request);
                
                var accountsAsJson = JsonConvert.SerializeObject(apiResponse.UserAccounts.ToDictionary(k => k.AccountId));
                claimsIdentity.AddClaim(new Claim(DasClaimsTypesExtended.Accounts, accountsAsJson, JsonClaimValueTypes.Json));
                claimsIdentity.AddClaim(new Claim(DasClaimTypes.Id, apiResponse.EmployerUserId));
                claimsIdentity.AddClaim(new Claim(DasClaimTypes.Email, email));
                claimsIdentity.AddClaim(new Claim(DasClaimsTypesExtended.FirstName, apiResponse.FirstName));
                claimsIdentity.AddClaim(new Claim(DasClaimsTypesExtended.LastName, apiResponse.LastName));

                var upsertUserCommand = new CreateOrUpdateUserCommand(
                    Guid.Parse(apiResponse.EmployerUserId), 
                    email, 
                    apiResponse.FirstName, 
                    apiResponse.LastName);
                await _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(upsertUserCommand));
            }
            else
            {
                var @ref = Guid.Parse(claimsIdentity.FindFirst(DasClaimTypes.Id).Value);
                var email = claimsIdentity.FindFirst(DasClaimTypes.Email).Value;
                var firstName = claimsIdentity.FindFirst(DasClaimTypes.GivenName).Value;
                var lastName = claimsIdentity.FindFirst(DasClaimTypes.FamilyName).Value;
                var upsertUserCommand = new CreateOrUpdateUserCommand(@ref, email, firstName, lastName);
            
                await _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(upsertUserCommand));
            }
        }
    }
}