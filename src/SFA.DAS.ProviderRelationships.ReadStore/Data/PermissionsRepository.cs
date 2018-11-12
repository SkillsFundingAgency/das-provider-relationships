using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.Document.Repository;
using Permission = SFA.DAS.ProviderRelationships.ReadStore.Models.Permission;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class PermissionsRepository : DocumentRepository<Permission>, IPermissionsRepository
    {
        public PermissionsRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.CollectionName, DocumentSettings.CollectionName)
        {
        }
    }
}