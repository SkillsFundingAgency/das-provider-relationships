using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class ReadOnlyDocumentRepository<TEntity> : IReadOnlyDocumentRepository<TEntity> where TEntity : class
    {
        protected IDocumentDbClient<TEntity> DbClient { get; }
        protected string Collection { get; }

        public ReadOnlyDocumentRepository(IDocumentDbClient<TEntity> dbClient, string collection)
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
    }
}