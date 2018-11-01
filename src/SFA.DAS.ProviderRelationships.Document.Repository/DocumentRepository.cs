using System;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentRepository<TEntity> : ReadOnlyDocumentRepository<TEntity>, IDocumentRepository<TEntity> where TEntity : class
    {
        public DocumentRepository(IDocumentDbClient<TEntity> dbClient, string collection) : base(dbClient, collection)
        {
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
