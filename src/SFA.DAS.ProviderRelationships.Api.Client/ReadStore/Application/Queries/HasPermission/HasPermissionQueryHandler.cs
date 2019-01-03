using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission
{
    internal class HasPermissionQueryHandler : IRequestHandler<HasPermissionQuery, bool>
    {
        private readonly IAccountProviderLegalEntitiesReadOnlyRepository _accountProviderLegalEntitiesReadOnlyRepository;

        public HasPermissionQueryHandler(IAccountProviderLegalEntitiesReadOnlyRepository providerLegalEntitiesReadOnlyRepository)
        {
            _accountProviderLegalEntitiesReadOnlyRepository = providerLegalEntitiesReadOnlyRepository;
        }

        public async Task<bool> Handle(HasPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasPermission = await _accountProviderLegalEntitiesReadOnlyRepository.CreateQuery()
                .AnyAsync(p => p.Ukprn == request.Ukprn && p.Deleted == null 
                                                        && p.AccountLegalEntityId == request.EmployerAccountLegalEntityId 
                                                        && p.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasPermission;
        }
    }
}