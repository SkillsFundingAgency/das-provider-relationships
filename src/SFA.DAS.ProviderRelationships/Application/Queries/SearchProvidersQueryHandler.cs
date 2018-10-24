using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Validation;
using SFA.DAS.Validation;

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
            var isValid = long.TryParse(request.Ukprn, out var ukprn) && await _db.Value.Providers.AnyAsync(p => p.Ukprn == ukprn, cancellationToken);

            if (!isValid)
            {
                throw new ValidationException().AddError(request, r => r.Ukprn, ErrorMessages.InvalidUkprn);
            }

            return new SearchProvidersQueryResponse
            {
                Ukprn = ukprn
            };
        }
    }
}