using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentRepository<TEntity> : DocumentReadOnlyRepository<TEntity>, IDocumentRepository<TEntity> where TEntity : class
    {
        public DocumentRepository(CosmosDbClient<TEntity> dbClient, string collection) : base(dbClient, collection)
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
