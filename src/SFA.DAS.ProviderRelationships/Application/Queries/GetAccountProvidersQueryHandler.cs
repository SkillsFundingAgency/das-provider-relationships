using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProvidersQueryHandler : IRequestHandler<GetAccountProvidersQuery, GetAccountProvidersQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetAccountProvidersQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetAccountProvidersQueryResult> Handle(GetAccountProvidersQuery request, CancellationToken cancellationToken)
        {
            var accountProviders = await _db.Value.AccountProviders
                .Where(ap => ap.Account.Id == request.AccountId)
                .ProjectTo<AccountProviderDto>(_configurationProvider)
                .ToListAsync(cancellationToken);

            return new GetAccountProvidersQueryResult(accountProviders);
        }
    }
}
