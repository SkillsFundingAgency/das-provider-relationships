using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class GetRelationshipWithPermissionQueryHandler : IReadStoreRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public GetRelationshipWithPermissionQueryHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task<GetRelationshipWithPermissionQueryResult> Handle(GetRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var relationships = await _relationshipsRepository.CreateQuery()
                .Where(p => p.Ukprn == request.Ukprn && p.Deleted == null && p.Operations.Contains(request.Operation))
                .Select(p => new RelationshipDto
                {
                    EmployerAccountId = p.AccountId,
                    EmployerAccountPublicHashedId = p.AccountPublicHashedId,
                    EmployerAccountName = p.AccountName,
                    EmployerAccountLegalEntityId = p.AccountLegalEntityId,
                    EmployerAccountLegalEntityPublicHashedId = p.AccountLegalEntityPublicHashedId,
                    EmployerAccountLegalEntityName = p.AccountLegalEntityName,
                    EmployerAccountProviderId = p.AccountProviderId,
                    Ukprn = p.Ukprn
                })
                .ToListAsync(cancellationToken);
            
            return new GetRelationshipWithPermissionQueryResult
            {
                Relationships = relationships
            };
        }
    }
}