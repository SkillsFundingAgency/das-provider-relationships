using System;
using System.Security.Claims;
using MediatR;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.Authentication
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
            var command = new AddOrUpdateUserCommand
            {
                Ref = Guid.Parse(claimsIdentity.FindFirst(DasClaimTypes.Id).Value),
                Email = claimsIdentity.FindFirst(DasClaimTypes.Email).Value,
                FirstName = claimsIdentity.FindFirst(DasClaimTypes.GivenName).Value,
                LastName = claimsIdentity.FindFirst(DasClaimTypes.FamilyName).Value
            };
            
            _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(command)).GetAwaiter().GetResult();
        }
    }
}