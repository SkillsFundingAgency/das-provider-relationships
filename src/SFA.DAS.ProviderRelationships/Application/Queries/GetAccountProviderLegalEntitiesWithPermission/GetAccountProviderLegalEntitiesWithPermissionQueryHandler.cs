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

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission
{
    public class GetAccountProviderLegalEntitiesWithPermissionQueryHandler : IRequestHandler<GetAccountProviderLegalEntitiesWithPermissionQuery, GetAccountProviderLegalEntitiesWithPermissionQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetAccountProviderLegalEntitiesWithPermissionQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }
        
        public async Task<GetAccountProviderLegalEntitiesWithPermissionQueryResult> Handle(GetAccountProviderLegalEntitiesWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var relationshipsQuery = _db.Value.AccountProviderLegalEntities
                .IgnoreQueryFilters() //removing the queryfilter on AccountLegalEntity here prevents the query being generated with inneficient subqueries
                .Where(aple =>
                    aple.Permissions.Any(p => request.Operations.Contains(p.Operation)));

            if (request.Ukprn.HasValue)
            {
                relationshipsQuery = relationshipsQuery.Where(aple =>
                    aple.AccountProvider.ProviderUkprn == request.Ukprn);
            }

            if (!string.IsNullOrEmpty(request.AccountHashedId))
            {
                relationshipsQuery = relationshipsQuery.Where(aple =>
                   aple.AccountProvider.Account.HashedId == request.AccountHashedId);
            }

            if(!string.IsNullOrEmpty(request.AccountLegalEntityPublicHashedId))
            {
                relationshipsQuery = relationshipsQuery.Where(aple =>
                   aple.AccountLegalEntity.PublicHashedId == request.AccountLegalEntityPublicHashedId);
            }


            var relationships = await relationshipsQuery
                .Where(aple => aple.AccountLegalEntity.Deleted != null)
                .ProjectTo<AccountProviderLegalEntityDto>(_configurationProvider)
                .ToListAsync(cancellationToken);

            return new GetAccountProviderLegalEntitiesWithPermissionQueryResult(relationships);
        }
    }
}