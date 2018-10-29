using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using Z.EntityFramework.Plus;

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
            var ukprnQuery = _db.Value.Providers
                .Where(p => p.Ukprn == request.Ukprn)
                .Select(p => (long?)p.Ukprn)
                .DeferredSingleOrDefault()
                .FutureValue();

            var accountProviderIdQuery = _db.Value.AccountProviders
                .Where(ap => ap.AccountId == request.AccountId && ap.ProviderUkprn == request.Ukprn)
                .Select(ap => (int?)ap.Id)
                .DeferredSingleOrDefault()
                .FutureValue();

            var ukprn = await ukprnQuery.ValueAsync();
            var accountProviderId = await accountProviderIdQuery.ValueAsync();
            
            return new SearchProvidersQueryResponse(ukprn, accountProviderId);
        }
    }
}