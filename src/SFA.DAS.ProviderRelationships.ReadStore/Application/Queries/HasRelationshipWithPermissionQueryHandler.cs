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
        private readonly IReadOnlyDocumentRepository<Permission> _repository;

        public HasRelationshipWithPermissionQueryHandler(IReadOnlyDocumentRepository<Permission> repository)
        {
            _repository = repository;
        }
        
        public async Task<bool> Handle(HasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var relationships = await _repository.CreateQuery().Where(p => p.Ukprn == request.Ukprn).AsDocumentQueryWrapper().ExecuteAsync(cancellationToken);
            var hasRelationshipWithPermission = relationships.Any(p => p.Operations != null && p.Operations.Any(o => o == request.Permission));

            return hasRelationshipWithPermission;
        }
    }
}