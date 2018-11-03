using System;
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
            var ukprn = long.Parse(request.Ukprn);
            var providerExists = await _db.Value.Providers.AnyAsync(p => p.Ukprn == ukprn, cancellationToken);

            return new SearchProvidersQueryResponse(ukprn, providerExists);
        }
    }
}