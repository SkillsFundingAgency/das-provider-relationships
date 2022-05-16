using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.HasPermission
{
    public class HasPermissionQueryHandler : IRequestHandler<HasPermissionQuery, bool>
    {
        private readonly IAccountProviderLegalEntitiesRepository _accountProviderLegalEntitiesRepository;

        public HasPermissionQueryHandler(IAccountProviderLegalEntitiesRepository providerLegalEntitiesRepository)
        {
            _accountProviderLegalEntitiesRepository = providerLegalEntitiesRepository;
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
