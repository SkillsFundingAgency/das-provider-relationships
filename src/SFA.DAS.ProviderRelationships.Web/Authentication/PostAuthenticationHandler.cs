using System;
using System.Security.Claims;
using MediatR;
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
            var @ref = Guid.Parse(claimsIdentity.FindFirst(EmployerClaimTypes.UserId).Value);
            var email = claimsIdentity.FindFirst(EmployerClaimTypes.EmailAddress).Value;
            var firstName = claimsIdentity.FindFirst(EmployerClaimTypes.GivenName).Value;
            var lastName = claimsIdentity.FindFirst(EmployerClaimTypes.FamilyName).Value;
            var command = new CreateOrUpdateUserCommand(@ref, email, firstName, lastName);
            
            _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(command)).GetAwaiter().GetResult();
        }
    }
}