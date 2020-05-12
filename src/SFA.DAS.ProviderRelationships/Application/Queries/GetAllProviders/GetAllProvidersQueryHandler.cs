using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders
{
    public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, GetAllProvidersQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public GetAllProvidersQueryHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task<GetAllProvidersQueryResult> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
        {
            return new GetAllProvidersQueryResult(
                await _db.Value.Providers
                    .Select(p => new ProviderDto { Ukprn = p.Ukprn, Name = p.Name })
                    .ToListAsync(cancellationToken));
        }
    }
}

