using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client.Application
{
    public class ProviderRelationshipService  : IProviderRelationshipService
    {
        private readonly IDocumentReadOnlyRepository<ProviderPermissions> _repository;

        public ProviderRelationshipService(IDocumentReadOnlyRepository<ProviderPermissions> repository)
        {
            _repository = repository;
        }

        public async Task<bool> HasRelationshipWithPermission(long ukprn, PermissionEnumDto permission, CancellationToken cancellationToken)
        {
            var query = _repository.CreateQuery().Where(x => x.Ukprn == ukprn);
            var all = await query.AsDocumentQueryWrapper().ExecuteAsync<ProviderPermissions>(cancellationToken);
            return all.Any(x => x.GrantPermissions != null && x.GrantPermissions.Any(y => y.Permission == permission));
        }

        Task<IEnumerable<ProviderPermissions>> IProviderRelationshipService.ListRelationshipsWithPermission(long ukPrn, PermissionEnumDto permission, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}