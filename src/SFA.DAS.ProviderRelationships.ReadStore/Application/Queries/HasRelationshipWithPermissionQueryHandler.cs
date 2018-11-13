using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasRelationshipWithPermissionQueryHandler : IApiRequestHandler<HasRelationshipWithPermissionQuery, bool>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public HasRelationshipWithPermissionQueryHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }
        
        public async Task<bool> Handle(HasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasRelationshipWithPermission = await _permissionsRepository.CreateQuery()
                .AnyAsync(p => p.Ukprn == request.Ukprn && p.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasRelationshipWithPermission;
        }
    }
}