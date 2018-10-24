using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.Application
{
    public class GetHasRelationshipWithPermissionHandler : IRequestHandler<GetHasRelationshipWithPermissionQuery, bool>
    {
        private readonly IDocumentReadOnlyRepository<ProviderRelationship> _repository;

        public GetHasRelationshipWithPermissionHandler(IDocumentReadOnlyRepository<ProviderRelationship> repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(GetHasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
        {
            return _repository.FindAny(x => x.Ukprn == request.Ukprn && x.GrantPermissions.Any(y => y.Permission == request.Permission));
        }
    }
}