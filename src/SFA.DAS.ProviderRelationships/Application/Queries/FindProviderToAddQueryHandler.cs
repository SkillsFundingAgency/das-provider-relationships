using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class FindProviderToAddQueryHandler : IRequestHandler<FindProviderToAddQuery, FindProviderToAddQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public FindProviderToAddQueryHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task<FindProviderToAddQueryResult> Handle(FindProviderToAddQuery request, CancellationToken cancellationToken)
        {
            var data = await _db.Value.Providers
                .Where(p => p.Ukprn == request.Ukprn)
                .Select(p => new
                {
                    p.Ukprn,
                    AccountProviderId = p.AccountProviders
                        .Where(ap => ap.AccountId == request.AccountId)
                        .Select(ap => (long?)ap.Id)
                        .FirstOrDefault()
                })
                .SingleOrDefaultAsync(cancellationToken);
            
            return new FindProviderToAddQueryResult(data?.Ukprn, data?.AccountProviderId);
        }
    }
}