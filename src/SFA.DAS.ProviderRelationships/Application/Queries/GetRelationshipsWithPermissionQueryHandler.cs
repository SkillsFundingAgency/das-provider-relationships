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

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetRelationshipsWithPermissionQueryHandler : IRequestHandler<GetRelationshipsWithPermissionQuery, GetRelationshipsWithPermissionQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetRelationshipsWithPermissionQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }
        
        public async Task<GetRelationshipsWithPermissionQueryResult> Handle(GetRelationshipsWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var relationships = await _db.Value.AccountProviderLegalEntities
                .Where(aple => aple.AccountProvider.ProviderUkprn == request.Ukprn
                               && aple.Permissions.Any(p => p.Operation == request.Operation))
                //.OrderBy(?)
                .Include(aple => aple.AccountProvider)
                    .ThenInclude(ap => ap.Account)
                .Include(aple => aple.AccountLegalEntity)
                .ProjectTo<RelationshipDto>(_configurationProvider)
                .ToListAsync(cancellationToken);

            return new GetRelationshipsWithPermissionQueryResult(relationships);
        }
    }
}