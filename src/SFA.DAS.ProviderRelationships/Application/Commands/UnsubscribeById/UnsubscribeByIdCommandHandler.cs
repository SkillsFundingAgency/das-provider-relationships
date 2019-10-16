using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UnsubscribeById
{
    public class UnsubscribeByIdCommandHandler : AsyncRequestHandler<UnsubscribeByIdCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public UnsubscribeByIdCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(UnsubscribeByIdCommand request, CancellationToken cancellationToken)
        {
            {
                var invitation = await _db.Value.Invitations.SingleOrDefaultAsync(i => i.Reference == request.CorrelationId, cancellationToken);

                if (invitation != null)
                {
                    var exists = await _db.Value.Unsubscribed.SingleOrDefaultAsync(u => u.EmailAddress == invitation.EmployerEmail && u.Ukprn == invitation.Ukprn, cancellationToken);

                    if (exists == null)
                    {
                        _db.Value.Unsubscribed.Add(new Unsubscribe(invitation.EmployerEmail, invitation.Ukprn));
                    }
                }
            }
        }
    }
}