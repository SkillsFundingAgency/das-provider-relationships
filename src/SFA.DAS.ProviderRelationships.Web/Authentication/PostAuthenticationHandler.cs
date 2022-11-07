using System;
using System.Security.Claims;
using MediatR;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.UnitOfWork.DependencyResolution.StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class PostAuthenticationHandler : IPostAuthenticationHandler
    {
        private readonly IUnitOfWorkScope _unitOfWorkScope;

        public PostAuthenticationHandler(IUnitOfWorkScope unitOfWorkScope)
        {
            _unitOfWorkScope = unitOfWorkScope;
        }

        public void Handle(ClaimsIdentity claimsIdentity)
        {
            var @ref = Guid.Parse(claimsIdentity.FindFirst(DasClaimTypes.Id).Value);//todo: change to pass as string
            var email = claimsIdentity.FindFirst(DasClaimTypes.Email).Value;
            var firstName = claimsIdentity.FindFirst(DasClaimTypes.GivenName).Value;
            var lastName = claimsIdentity.FindFirst(DasClaimTypes.FamilyName).Value;
            var command = new CreateOrUpdateUserCommand(@ref, email, firstName, lastName);
            
            _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(command)).GetAwaiter().GetResult();
        }
    }
}