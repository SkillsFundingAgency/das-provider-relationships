using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity
{
    public class GetUpdatedAccountProviderLegalEntityQueryHandler : IRequestHandler<GetUpdatedAccountProviderLegalEntityQuery, GetUpdatedAccountProviderLegalEntityQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetUpdatedAccountProviderLegalEntityQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetUpdatedAccountProviderLegalEntityQueryResult> Handle(GetUpdatedAccountProviderLegalEntityQuery request, CancellationToken cancellationToken)
        {
            var accountProviderLegalEntity = await _db.Value.AccountProviderLegalEntities
                .Where(aple => aple.AccountProviderId == request.AccountProviderId && aple.AccountLegalEntityId == request.AccountLegalEntityId && aple.AccountProvider.AccountId == request.AccountId)
                .ProjectTo<AccountProviderLegalEntityDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (accountProviderLegalEntity == null)
            {
                return null;
            }
            
            var accountLegalEntitiesCount = await _db.Value.AccountLegalEntities.CountAsync(ale => ale.AccountId == request.AccountId, cancellationToken);
            
            return new GetUpdatedAccountProviderLegalEntityQueryResult(accountProviderLegalEntity, accountLegalEntitiesCount);
        }
    }
}