using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DtosShared;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity
{
    public class GetAccountProviderLegalEntityQueryHandler : IRequestHandler<GetAccountProviderLegalEntityQuery, GetAccountProviderLegalEntityQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetAccountProviderLegalEntityQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetAccountProviderLegalEntityQueryResult> Handle(GetAccountProviderLegalEntityQuery request, CancellationToken cancellationToken)
        {
            var accountProvider = await _db.Value.AccountProviders
                .Where(ap => ap.AccountId == request.AccountId && ap.Id == request.AccountProviderId)
                .ProjectTo<AccountProviderBasicDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            
            var accountLegalEntity = await _db.Value.AccountLegalEntities
                .Where(ale => ale.AccountId == request.AccountId && ale.Id == request.AccountLegalEntityId)
                .ProjectTo<AccountLegalEntityBasicDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (accountProvider == null || accountLegalEntity == null)
            {
                return null;
            }
            
            var accountProviderLegalEntity = await _db.Value.AccountProviderLegalEntities
                .Where(aple => aple.AccountProviderId == request.AccountProviderId && aple.AccountLegalEntityId == request.AccountLegalEntityId)
                .ProjectTo<AccountProviderLegalEntitySummaryDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            
            var accountLegalEntitiesCount = await _db.Value.AccountLegalEntities.CountAsync(ale => ale.AccountId == request.AccountId, cancellationToken);
            
            return new GetAccountProviderLegalEntityQueryResult(accountProvider, accountLegalEntity, accountProviderLegalEntity, accountLegalEntitiesCount);
        }
    }
}