using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.UnitOfWork.DependencyResolution.StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class PostAuthenticationHandler : IPostAuthenticationHandler
    {
        private readonly IUnitOfWorkScope _unitOfWorkScope;
        private readonly IOuterApiClient _outerApiClient;
        private readonly ProviderRelationshipsConfiguration _webConfig;

        public PostAuthenticationHandler(
            IUnitOfWorkScope unitOfWorkScope,
            IOuterApiClient outerApiClient,
            IOptions<ProviderRelationshipsConfiguration> options)
        {
            _unitOfWorkScope = unitOfWorkScope;
            _outerApiClient = outerApiClient;
            _webConfig = options.Value;
        }

        public async Task Handle(ClaimsIdentity claimsIdentity)
        {
            if (_webConfig.UseGovUkSignIn)
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
                await _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(upsertUserCommand));
            }
            else
            {
                var @ref = Guid.Parse(claimsIdentity.FindFirst(EmployerClaimTypes.UserId).Value);
                var email = claimsIdentity.FindFirst(EmployerClaimTypes.EmailAddress).Value;
                var firstName = claimsIdentity.FindFirst(EmployerClaimTypes.GivenName).Value;
                var lastName = claimsIdentity.FindFirst(EmployerClaimTypes.FamilyName).Value;
                var command = new CreateOrUpdateUserCommand(@ref, email, firstName, lastName);
            
                await _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(command));   
            }
        }
    }
}