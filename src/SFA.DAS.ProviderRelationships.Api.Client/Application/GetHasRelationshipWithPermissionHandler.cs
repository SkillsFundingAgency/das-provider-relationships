using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.Documents.Linq;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.Application
{
    public class GetHasRelationshipWithPermissionHandler : IRequestHandler<GetHasRelationshipWithPermissionQuery, bool>
    {
        private readonly IDocumentReadOnlyRepository<ProviderPermissions> _repository;

        public GetHasRelationshipWithPermissionHandler(IDocumentReadOnlyRepository<ProviderPermissions> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(GetHasRelationshipWithPermissionQuery request,CancellationToken cancellationToken)
        {
            var query = _repository.CreateQuery().Where(x => x.Ukprn == request.Ukprn);
            var all = await _repository.ExecuteQuery(query, cancellationToken);
            return all.Any(x => x.GrantPermissions.Any(y => y.Permission == request.Permission));
        }
    }
}