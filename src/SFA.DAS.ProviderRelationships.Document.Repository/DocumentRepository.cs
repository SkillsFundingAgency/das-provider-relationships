using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentRepository<TEntity> : CosmosDbClient<TEntity>, IDocumentRepository<TEntity> where TEntity : class
    {
        public DocumentRepository(IDocumentClientFactory dbClientFactory, IDocumentConfiguration configuration)
            : base (dbClientFactory, configuration)
        {
        }
        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task Save(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
