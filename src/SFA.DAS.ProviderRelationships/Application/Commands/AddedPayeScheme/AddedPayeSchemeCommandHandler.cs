using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.AddedPayeScheme
{
    public class AddedPayeSchemeCommandHandler : AsyncRequestHandler<AddedPayeSchemeCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddedPayeSchemeCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(AddedPayeSchemeCommand request, CancellationToken cancellationToken)
        {
            if (request.CorrelationId != Guid.Empty)
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == request.CorrelationId && i.Status < (int) InvitationStatus.PayeSchemeAdded, cancellationToken);
                invitation?.UpdateStatus((int) InvitationStatus.PayeSchemeAdded, DateTime.Now);
            }
        }
    }
}