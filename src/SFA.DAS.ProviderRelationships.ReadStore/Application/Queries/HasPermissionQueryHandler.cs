using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasPermissionQueryHandler : IReadStoreRequestHandler<HasPermissionQuery, bool>
    {
        private readonly IAccountProviderLegalEntitiesRepository _accountProviderLegalEntitiesRepository;

        public HasPermissionQueryHandler(IAccountProviderLegalEntitiesRepository accountProviderLegalEntitiesRepository)
        {
            _accountProviderLegalEntitiesRepository = accountProviderLegalEntitiesRepository;
        }

        public async Task<bool> Handle(HasPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasPermission = await _accountProviderLegalEntitiesRepository.CreateQuery()
                .AnyAsync(p => p.Ukprn == request.Ukprn && p.Deleted == null 
                                                        && p.AccountLegalEntityId == request.EmployerAccountLegalEntityId 
                                                        && p.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasPermission;
        }
    }
}