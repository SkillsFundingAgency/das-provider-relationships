using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission
{
    internal class HasRelationshipWithPermissionQueryHandler : IRequestHandler<HasRelationshipWithPermissionQuery, bool>
    {
        private readonly IAccountProviderLegalEntitiesReadOnlyRepository _accountProviderLegalEntitiesReadOnlyRepository;

        public HasRelationshipWithPermissionQueryHandler(IAccountProviderLegalEntitiesReadOnlyRepository providerLegalEntitiesReadOnlyRepository)
        {
            _accountProviderLegalEntitiesReadOnlyRepository = providerLegalEntitiesReadOnlyRepository;
        }
        
        public async Task<bool> Handle(HasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasRelationshipWithPermission = await _accountProviderLegalEntitiesReadOnlyRepository.CreateQuery()
                .AnyAsync(p => p.Ukprn == request.Ukprn && p.Deleted == null && p.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasRelationshipWithPermission;
        }
    }
}