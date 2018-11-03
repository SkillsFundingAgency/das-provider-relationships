using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class PermissionsRepository : DocumentRepository<Permission>, IPermissionsRepository
    {
        public PermissionsRepository(IDocumentClientFactory documentClientFactory)
            : base(documentClientFactory.CreateDocumentClient(), "SFA.DAS.ProviderRelationships.ReadStore.Database", "permissions")
        {
        }
    }
}