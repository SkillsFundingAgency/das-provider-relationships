using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SignedAgreement
{
    public class SignedAgreementCommandHandler : AsyncRequestHandler<SignedAgreementCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public SignedAgreementCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(SignedAgreementCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.CorrelationId) && Guid.TryParse(request.CorrelationId, out _))
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == Guid.Parse(request.CorrelationId) && i.Status < (int) InvitationStatus.LegalAgreementSigned, cancellationToken);
                invitation?.UpdateStatus((int) InvitationStatus.LegalAgreementSigned, DateTime.Now);
            }
        }
    }
}