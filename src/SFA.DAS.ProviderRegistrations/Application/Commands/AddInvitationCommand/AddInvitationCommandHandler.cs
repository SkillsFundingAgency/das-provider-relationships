using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand
{
    public class AddInvitationCommandHandler : RequestHandler<AddInvitationCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddInvitationCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(AddInvitationCommand request)
        {
            var invitation = new Invitation(
                request.Reference, 
                request.Ukprn, 
                request.UserRef, 
                request.EmployerOrganisation, 
                request.EmployerFirstName,
                request.EmployerLastName,
                request.EmployerEmail,
                request.Status,
                request.CreatedDate);
            
            _db.Value.Invitations.Add(invitation);
        }
    }
}