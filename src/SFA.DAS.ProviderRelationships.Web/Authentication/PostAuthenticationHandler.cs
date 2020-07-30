using System;
using System.Security.Claims;
using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.UnitOfWork.DependencyResolution.StructureMap;
using SFA.DAS.ProviderRelationships.Web.App_Start;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class PostAuthenticationHandler : IPostAuthenticationHandler
    {
        private readonly IUnitOfWorkScope _unitOfWorkScope;
        private readonly ILog _log;

        public PostAuthenticationHandler(IUnitOfWorkScope unitOfWorkScope, ILog log)
        {
            _unitOfWorkScope = unitOfWorkScope;
            _log = log;
        }

        public void Handle(ClaimsIdentity claimsIdentity)
        {
            var @ref = Guid.Parse(claimsIdentity.FindFirst(DasClaimTypes.Id).Value);
            var email = claimsIdentity.FindFirst(DasClaimTypes.Email).Value;
            var firstName = claimsIdentity.FindFirst(DasClaimTypes.GivenName).Value;
            var lastName = claimsIdentity.FindFirst(DasClaimTypes.FamilyName).Value;
            var command = new CreateOrUpdateUserCommand(@ref, email, firstName, lastName);

            // TO DO : remove this log later (GDPR)
            _log.Info($"PostAuthenticationHandler : ClaimsIdentity :: email : {email} firstName : {firstName} lastName: {lastName} ref : {@ref}");
            
            _unitOfWorkScope.RunAsync(c => c.GetInstance<IMediator>().Send(command)).GetAwaiter().GetResult();
        }
    }
}