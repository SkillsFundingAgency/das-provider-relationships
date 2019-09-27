using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand
{
    public class AddInvitationCommandHandler : AsyncRequestHandler<AddInvitationCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddInvitationCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(AddInvitationCommand request, CancellationToken cancellationToken)
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
                request.CreatedDate,
                request.CreatedDate);
            
            _db.Value.Invitations.Add(invitation);
            await _db.Value.SaveChangesAsync(cancellationToken);
        }
    }
}