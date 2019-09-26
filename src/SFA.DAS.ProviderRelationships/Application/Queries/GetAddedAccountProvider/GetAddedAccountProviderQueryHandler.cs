using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider.Dtos;
using SFA.DAS.ProviderRelationships.Domain.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider
{
    public class GetAddedAccountProviderQueryHandler : IRequestHandler<GetAddedAccountProviderQuery, GetAddedAccountProviderQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetAddedAccountProviderQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetAddedAccountProviderQueryResult> Handle(GetAddedAccountProviderQuery request, CancellationToken cancellationToken)
        {
            var accountProvider = await _db.Value.AccountProviders
                .Where(ap => ap.Id == request.AccountProviderId && ap.Account.Id == request.AccountId)
                .ProjectTo<AccountProviderDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (accountProvider == null)
            {
                return null;
            }
            
            return new GetAddedAccountProviderQueryResult(accountProvider);
        }
    }
}