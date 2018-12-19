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