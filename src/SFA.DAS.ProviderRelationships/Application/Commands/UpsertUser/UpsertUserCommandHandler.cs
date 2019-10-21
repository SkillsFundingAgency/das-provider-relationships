using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpsertUser
{
    public class UpsertUserCommandHandler : AsyncRequestHandler<UpsertUserCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public UpsertUserCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(UpsertUserCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out _))
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == Guid.Parse(request.CorrelationId) && i.Status < (int) InvitationStatus.AccountStarted, cancellationToken);
                invitation?.UpdateStatus((int) InvitationStatus.AccountStarted, DateTime.Now);
            }
        }
    }
}