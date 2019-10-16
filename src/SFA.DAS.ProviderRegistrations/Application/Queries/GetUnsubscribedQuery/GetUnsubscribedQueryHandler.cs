using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Domain.Data;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery
{
    public class GetUnsubscribedQueryHandler : IRequestHandler<GetUnsubscribedQuery, bool>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public GetUnsubscribedQueryHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task<bool> Handle(GetUnsubscribedQuery request, CancellationToken cancellationToken)
        {
            return await _db.Value.Unsubscribed.AnyAsync(u => u.Ukprn == request.Ukprn && u.EmailAddress == request.EmailAddress, cancellationToken);
        }
    }
}