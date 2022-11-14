using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerUsers.WebClientComponents;
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
                await _outerApiClient.Get<GetUserAccountsResponse>(request);
                //todo: claimsIdentity.AddClaim("");
            }
            else
            {
                var @ref = Guid.Parse(claimsIdentity.FindFirst(DasClaimTypes.Id).Value);
                var email = claimsIdentity.FindFirst(DasClaimTypes.Email).Value;
                var firstName = claimsIdentity.FindFirst(DasClaimTypes.GivenName).Value;
                var lastName = claimsIdentity.FindFirst(DasClaimTypes.FamilyName).Value;
                var command = new CreateOrUpdateUserCommand(@ref, email, firstName, lastName);
            
                _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(command)).GetAwaiter().GetResult();
            }
        }
    }
}