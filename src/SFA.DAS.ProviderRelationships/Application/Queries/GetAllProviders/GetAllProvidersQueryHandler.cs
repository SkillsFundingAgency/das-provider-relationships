using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Types.Dtos;

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
            var providersDto = new List<AccountProviderDto>();

            var providers = await _db.Value.Providers
                                .ToListAsync(cancellationToken);

            foreach (var provider in providers)
            {
                var providerDto = new AccountProviderDto {
                    ProviderUkprn = provider.Ukprn,
                    ProviderName = provider.Name
                };
                providersDto.Add(providerDto);
            }
            return new GetAllProvidersQueryResult(providersDto);
        }
    }
}

