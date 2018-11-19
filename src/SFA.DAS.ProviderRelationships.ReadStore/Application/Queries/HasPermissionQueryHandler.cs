using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasPermissionQueryHandler : IApiRequestHandler<HasPermissionQuery, bool>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public HasPermissionQueryHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task<bool> Handle(HasPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasPermission = await _relationshipsRepository.CreateQuery()
                .AnyAsync(p => p.AccountProvider.Ukprn == request.Ukprn && p.Deleted == null
                                                        && p.AccountLegalEntity.AccountLegalEntityId == request.EmployerAccountLegalEntityId
                                                        && p.Permissions.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasPermission;
        }
    }
}