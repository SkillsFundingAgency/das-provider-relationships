using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderUkprnsByAccountIdQueryHandler : IRequestHandler<GetAccountProviderUkprnsByAccountIdQuery, GetAccountProviderUkprnsByAccountIdQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public GetAccountProviderUkprnsByAccountIdQueryHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task<GetAccountProviderUkprnsByAccountIdQueryResult> Handle(GetAccountProviderUkprnsByAccountIdQuery request, CancellationToken cancellationToken)
        {
            var ukprns = await _db.Value.AccountProviders
                .Where(ap => ap.Account.Id == request.AccountId)
                .Select(ap => ap.Provider.Ukprn)
                .ToListAsync(cancellationToken);
            
            return new GetAccountProviderUkprnsByAccountIdQueryResult(ukprns);
        }
    }
}