using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasRelationshipWithPermissionQueryHandler : IRequestHandler<HasRelationshipWithPermissionQuery, bool>
    {
        private readonly IReadOnlyDocumentRepository<ProviderPermissions> _repository;

        public HasRelationshipWithPermissionQueryHandler(IReadOnlyDocumentRepository<ProviderPermissions> repository)
        {
            _repository = repository;
        }
        
        public async Task<bool> Handle(HasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var relationships = await _repository.CreateQuery().Where(p => p.Ukprn == request.Ukprn).AsDocumentQueryWrapper().ExecuteAsync(cancellationToken);
            var hasRelationshipWithPermission = relationships.Any(p => p.GrantPermissions != null && p.GrantPermissions.Any(o => o.Permission == request.Permission));

            return hasRelationshipWithPermission;
        }
    }
}