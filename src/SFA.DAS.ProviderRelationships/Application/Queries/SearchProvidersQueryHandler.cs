using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class SearchProvidersQueryHandler : IRequestHandler<SearchProvidersQuery, SearchProvidersQueryResponse>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public SearchProvidersQueryHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task<SearchProvidersQueryResponse> Handle(SearchProvidersQuery request, CancellationToken cancellationToken)
        {
            var data = await _db.Value.Providers
                .Where(p => p.Ukprn == request.Ukprn)
                .Select(p => new
                {
                    p.Ukprn,
                    AccountProviderId = p.AccountProviders
                        .Where(ap => ap.AccountId == request.AccountId)
                        .Select(ap => (int?)ap.Id)
                        .FirstOrDefault()
                })
                .SingleOrDefaultAsync(cancellationToken);
            
            return new SearchProvidersQueryResponse(data?.Ukprn, data?.AccountProviderId);
        }
    }
}