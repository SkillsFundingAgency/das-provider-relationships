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
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity
{
    public class GetAccountProviderLegalEntityQueryHandler : IRequestHandler<GetAccountProviderLegalEntityQuery, GetAccountProviderLegalEntityQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IDasRecruitService _dasRecruitService;

        public GetAccountProviderLegalEntityQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IDasRecruitService dasRecruitService, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
            _dasRecruitService = dasRecruitService;
        }

        public async Task<GetAccountProviderLegalEntityQueryResult> Handle(GetAccountProviderLegalEntityQuery request, CancellationToken cancellationToken)
        {
            var accountProvider = await _db.Value.AccountProviders
                .Where(ap => ap.AccountId == request.AccountId && ap.Id == request.AccountProviderId)
                .ProjectTo<AccountProviderDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var accountLegalEntity = await _db.Value.AccountLegalEntities
                .Where(ale => ale.AccountId == request.AccountId && ale.Id == request.AccountLegalEntityId)
                .ProjectTo<AccountLegalEntityDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (accountProvider == null || accountLegalEntity == null)
            {
                return null;
            }

            var accountProviderLegalEntity = await _db.Value.AccountProviderLegalEntities
                .Where(aple => aple.AccountProviderId == request.AccountProviderId && aple.AccountLegalEntityId == request.AccountLegalEntityId)
                .ProjectTo<AccountProviderLegalEntityDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var accountLegalEntitiesCount = await _db.Value.AccountLegalEntities.CountAsync(ale => ale.AccountId == request.AccountId, cancellationToken);

            //var providerOrgBlockStatus = await _dasRecruitService.GetProviderBlockedStatusAsync(accountProvider.ProviderUkprn, cancellationToken);
            //var isProviderBlockedFromRecruit = providerOrgBlockStatus != null && providerOrgBlockStatus.Status.Equals(BlockedOrganisationStatusConstants.Blocked);

            return new GetAccountProviderLegalEntityQueryResult(accountProvider, accountLegalEntity, accountProviderLegalEntity, accountLegalEntitiesCount, false);
        }
    }
}