using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class GetRelationshipWithPermissionQueryHandler : IApiRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public GetRelationshipWithPermissionQueryHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task<GetRelationshipWithPermissionQueryResult> Handle(GetRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var relationships = await _relationshipsRepository.CreateQuery()
                .Where(p => p.AccountProvider.Ukprn == request.Ukprn && p.Deleted == null && p.Permissions.Operations.Contains(request.Operation))
                .Select(p => new RelationshipDto {
                    EmployerAccountId = p.AccountProvider.AccountId,
                    EmployerAccountPublicHashedId = p.AccountProvider.AccountPublicHashedId,
                    EmployerAccountName = p.AccountProvider.AccountName,
                    EmployerAccountLegalEntityId = p.AccountProviderLegalEntity.AccountLegalEntityId,
                    EmployerAccountLegalEntityPublicHashedId = p.AccountProviderLegalEntity.AccountLegalEntityPublicHashedId,
                    EmployerAccountLegalEntityName = p.AccountProviderLegalEntity.AccountLegalEntityName,
                    EmployerAccountProviderId = p.AccountProvider.AccountProviderId,
                    Ukprn = p.AccountProvider.Ukprn
                })
                .ToListAsync(cancellationToken);

            return new GetRelationshipWithPermissionQueryResult {
                Relationships = relationships
            };
        }
    }
}