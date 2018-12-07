using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd
{
    public class GetProviderToAddQueryHandler : IRequestHandler<GetProviderToAddQuery, GetProviderToAddQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetProviderToAddQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetProviderToAddQueryResult> Handle(GetProviderToAddQuery request, CancellationToken cancellationToken)
        {
            var provider = await _db.Value.Providers
                .Where(p => p.Ukprn == request.Ukprn)
                .ProjectTo<ProviderBasicDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return provider == null ? null : new GetProviderToAddQueryResult(provider);
        }
    }
}