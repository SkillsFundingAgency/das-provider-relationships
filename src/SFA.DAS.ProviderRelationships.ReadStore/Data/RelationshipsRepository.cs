using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class RelationshipsRepository : DocumentRepository<Relationship>, IRelationshipsRepository
    {
        public RelationshipsRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.CollectionName, DocumentSettings.CollectionName)
        {
        }
    }
}