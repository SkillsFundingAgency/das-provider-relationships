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
        private readonly IPermissionsRepository _permissionsRepository;

        public GetRelationshipWithPermissionQueryHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task<GetRelationshipWithPermissionQueryResult> Handle(GetRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var relationships = await _permissionsRepository.CreateQuery()
                .Where(p => p.Ukprn == request.Ukprn && p.Operations.Contains(request.Operation))
                .Select(p => new RelationshipDto
                {
                    EmployerAccountId = p.EmployerAccountId,
                    EmployerAccountPublicHashedId = p.EmployerAccountPublicHashedId,
                    EmployerAccountName = p.EmployerAccountName,
                    EmployerAccountLegalEntityId = p.EmployerAccountLegalEntityId,
                    EmployerAccountLegalEntityPublicHashedId = p.EmployerAccountLegalEntityPublicHashedId,
                    EmployerAccountLegalEntityName = p.EmployerAccountLegalEntityName,
                    EmployerAccountProviderId = p.EmployerAccountProviderId,
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