using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentReadOnlyRepository<TEntity> : IDocumentReadOnlyRepository<TEntity> where TEntity : class
    {
        protected readonly CosmosDbClient<TEntity> DbClient;
        protected readonly string Collection;

        public DocumentReadOnlyRepository(CosmosDbClient<TEntity> dbClient, string collection)
        {
            DbClient = dbClient;
            Collection = collection;
        }

        public Task<TEntity> GetById(Guid id)
        {
            return DbClient.GetById(Collection, id);
        }

        public async Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            var query = DbClient.CreateQuery(Collection, predicate);
            
            return await query.AsDocumentQuery().ExecuteNextAsync<TEntity>();
        }

        public async Task<bool> FindAny(Expression<Func<TEntity, bool>> predicate)
        {
            var query = DbClient.CreateQuery(Collection, predicate, new FeedOptions {MaxItemCount = 1});
            return (await query.AsDocumentQuery().ExecuteNextAsync()).Any();
        }
    }
}
