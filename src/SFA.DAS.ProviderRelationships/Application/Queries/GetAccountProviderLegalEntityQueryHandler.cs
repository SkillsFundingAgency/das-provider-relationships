using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;
using Z.EntityFramework.Plus;

namespace SFA.DAS.ProviderRelationships.Application.Queries
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
            var accountProviderQuery = _db.Value.AccountProviders
                .Where(ap => ap.AccountId == request.AccountId && ap.Id == request.AccountProviderId)
                .ProjectTo<AccountProviderBasicDto>(_configurationProvider)
                .DeferredSingleOrDefault()
                .FutureValue();
            
            var accountLegalEntityQuery = _db.Value.AccountLegalEntities
                .Where(ale => ale.AccountId == request.AccountId && ale.Id == request.AccountLegalEntityId)
                .ProjectTo<AccountLegalEntityBasicDto>(_configurationProvider)
                .DeferredSingleOrDefault()
                .FutureValue();
            
            var accountProviderLegalEntityQuery = _db.Value.AccountProviderLegalEntities
                .Where(aple => aple.AccountProviderId == request.AccountProviderId && aple.AccountLegalEntityId == request.AccountLegalEntityId)
                .ProjectTo<AccountProviderLegalEntityDto>(_configurationProvider)
                .DeferredSingleOrDefault()
                .FutureValue();
            
            var accountProvider = await accountProviderQuery.ValueAsync();
            var accountLegalEntity = await accountLegalEntityQuery.ValueAsync();
            var accountProviderLegalEntity = await accountProviderLegalEntityQuery.ValueAsync();
            
            if (accountProvider == null || accountLegalEntity == null)
            {
                return null;
            }
            
            return new GetAccountProviderLegalEntityQueryResult(accountProvider, accountLegalEntity, accountProviderLegalEntity);
        }
    }
}