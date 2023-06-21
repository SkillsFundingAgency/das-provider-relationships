using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider
{
    public class GetAccountProviderQueryHandler : IRequestHandler<GetAccountProviderQuery, GetAccountProviderQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetAccountProviderQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetAccountProviderQueryResult> Handle(GetAccountProviderQuery request, CancellationToken cancellationToken)
        {
            var accountProvider = await _db.Value.AccountProviders
                .Where(ap => ap.Id == request.AccountProviderId && ap.Account.Id == request.AccountId)
                .ProjectTo<AccountProviderDto>(_configurationProvider, new { accountProviderId = request.AccountProviderId })
                .SingleOrDefaultAsync(cancellationToken);
            
            if (accountProvider == null)
            {
                return null;
            }
            
            return new GetAccountProviderQueryResult(accountProvider);
        }
    }
}