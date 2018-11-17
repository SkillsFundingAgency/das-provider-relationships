using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderUkprnsQueryHandler : IRequestHandler<GetAccountProviderUkprnsQuery, GetAccountProviderUkprnsQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public GetAccountProviderUkprnsQueryHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task<GetAccountProviderUkprnsQueryResult> Handle(GetAccountProviderUkprnsQuery request, CancellationToken cancellationToken)
        {
            var ukprns = await _db.Value.AccountProviders
                .Where(ap => ap.Account.Id == request.AccountId)
                .Select(ap => ap.Provider.Ukprn)
                .ToListAsync(cancellationToken);
            
            return new GetAccountProviderUkprnsQueryResult(ukprns);
        }
    }
}