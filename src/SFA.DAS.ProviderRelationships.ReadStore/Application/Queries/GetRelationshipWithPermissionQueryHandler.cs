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
                .Where(p => p.Provider.Ukprn == request.Ukprn && p.Deleted == null && p.AccountProvider.Operations.Contains(request.Operation))
                .Select(p => new RelationshipDto {
                    EmployerAccountId = p.Account.Id,
                    EmployerAccountPublicHashedId = p.Account.AccountPublicHashedId,
                    EmployerAccountName = p.Account.AccountName,
                    EmployerAccountLegalEntityId = p.AccountLegalEntity.Id,
                    EmployerAccountLegalEntityPublicHashedId = p.AccountLegalEntity.PublicHashedId,
                    EmployerAccountLegalEntityName = p.AccountLegalEntity.Name,
                    EmployerAccountProviderId = p.AccountProvider.Id,
                    Ukprn = p.Provider.Ukprn
                })
                .ToListAsync(cancellationToken);

            return new GetRelationshipWithPermissionQueryResult {
                Relationships = relationships
            };
        }
    }
}