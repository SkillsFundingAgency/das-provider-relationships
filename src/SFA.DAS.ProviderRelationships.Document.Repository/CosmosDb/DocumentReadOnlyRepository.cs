using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentReadOnlyRepository<TEntity> : IDocumentReadOnlyRepository<TEntity> where TEntity : class
    {
        protected readonly IDocumentDbClient<TEntity> DbClient;
        protected readonly string Collection;

        public DocumentReadOnlyRepository(IDocumentDbClient<TEntity> dbClient, string collection)
        {
            DbClient = dbClient;
            Collection = collection;
        }

        public Task<TEntity> GetById(Guid id)
        {
            return DbClient.GetById(Collection, id);
        }

        public IQueryable<TEntity> CreateQuery()
        {
            return DbClient.CreateQuery(Collection);
        }

        public IQueryable<TEntity> CreateQuery(FeedOptions options)
        {
            return DbClient.CreateQuery(Collection, options);
        }

        //public async Task<IEnumerable<TEntity>> ExecuteQuery(IQueryable<TEntity> query, CancellationToken cancellationToken)
        //{
        //    var docQuery = DbClient.ConvertToDocumentQuery(query);

        //    var results = new List<TEntity>();
        //    while (docQuery.HasMoreResults)
        //    {
        //        results.AddRange(await DbClient.GetEntities(docQuery, cancellationToken));
        //    }
        //    return results;
        //}
    }
}
