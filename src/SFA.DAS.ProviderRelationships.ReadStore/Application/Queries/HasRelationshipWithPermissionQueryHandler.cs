using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasRelationshipWithPermissionQueryHandler : IReadStoreRequestHandler<HasRelationshipWithPermissionQuery, bool>
    {
        private readonly IAccountProviderLegalEntitiesRepository _accountProviderLegalEntitiesRepository;

        public HasRelationshipWithPermissionQueryHandler(IAccountProviderLegalEntitiesRepository accountProviderLegalEntitiesRepository)
        {
            _accountProviderLegalEntitiesRepository = accountProviderLegalEntitiesRepository;
        }
        
        public async Task<bool> Handle(HasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasRelationshipWithPermission = await _accountProviderLegalEntitiesRepository.CreateQuery()
                .AnyAsync(p => p.Ukprn == request.Ukprn && p.Deleted == null && p.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasRelationshipWithPermission;
        }
    }
}