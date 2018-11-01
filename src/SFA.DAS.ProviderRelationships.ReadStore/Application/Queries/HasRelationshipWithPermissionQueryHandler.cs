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
            var hasRelationshipWithPermission = await _repository.CreateQuery()
                .AnyAsync(p => p.Ukprn == request.Ukprn && p.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasRelationshipWithPermission;
        }
    }
}