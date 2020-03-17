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
    public class FindAllProvidersQueryHandler : IRequestHandler<FindAllProvidersQuery, FindAllProvidersQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public FindAllProvidersQueryHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task<FindAllProvidersQueryResult> Handle(FindAllProvidersQuery request, CancellationToken cancellationToken)
        {
            var providers = new List<AccountProviderDto>();

            var providerIds = await _db.Value.Providers
                                .ToListAsync(cancellationToken);

            foreach (var provider in providerIds)
            {
                var providerDto = new AccountProviderDto {
                    ProviderUkprn = provider.Ukprn,
                    ProviderName = provider.Name,
                    FormattedProviderSuggestion = FormatSuggestion(provider.Name, provider.Ukprn)
                };
                providers.Add(providerDto);
            }
            return new FindAllProvidersQueryResult(providers);
        }

        private string FormatSuggestion(string providerName, long ukprn)
        {
            return $"{providerName.ToUpper()} {ukprn}";
        }
    }
}

