using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal interface IRelationshipsRepository : IDocumentRepository<Relationship>
    {
    }
}