using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders
{
    public class FindAllProvidersQueryHandler : IRequestHandler<FindAllProvidersQuery, FindAllProvidersQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;
        
        public FindAllProvidersQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;      
        }

        public async Task<FindAllProvidersQueryResult> Handle(FindAllProvidersQuery request, CancellationToken cancellationToken)
        {
            var providers = new List<ProviderDto>();

            var providerIdsTask = await _db.Value.Providers
                                .ToListAsync(cancellationToken);

            foreach (var provider in providerIdsTask)
            {
                providers.Add(await GetProvider(provider.Ukprn, cancellationToken));
            }

            return new FindAllProvidersQueryResult(providers);
        }

        private async Task<ProviderDto> GetProvider(long ukprn, CancellationToken cancellationToken)
        {
            return await _db.Value.Providers
                 .ProjectTo<ProviderDto>(_configurationProvider, new { ukprn })
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}

