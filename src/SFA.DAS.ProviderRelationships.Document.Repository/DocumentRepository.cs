using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentRepository<TEntity> : IDocumentRepository<TEntity> where TEntity : class
    {
        private readonly CosmosDbClient<TEntity> _dbClient;
        private readonly string _collection;

        public DocumentRepository(CosmosDbClient<TEntity> dbClient, string collection)
        {
            _dbClient = dbClient;
            _collection = collection;
        }

        public Task<TEntity> GetById(Guid id)
        {
            return _dbClient.GetById(_collection, id);
        }

        public Task Save(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> sqlApiQuery)
        {
            return _dbClient.Search(_collection, sqlApiQuery);
        }
    }
}
