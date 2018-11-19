using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasRelationshipWithPermissionQueryHandler : IApiRequestHandler<HasRelationshipWithPermissionQuery, bool>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public HasRelationshipWithPermissionQueryHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task<bool> Handle(HasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasRelationshipWithPermission = await _relationshipsRepository.CreateQuery()
                .AnyAsync(p => p.Provider.Ukprn == request.Ukprn && p.Deleted == null && p.AccountProvider.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasRelationshipWithPermission;
        }
    }
}