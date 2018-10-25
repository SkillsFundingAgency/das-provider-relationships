using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentDbClient<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(string collection, Guid id);
        IQueryable<TEntity> CreateQuery(string collection);
        IQueryable<TEntity> CreateQuery(string collection, FeedOptions feedOptions);
        IDocumentQuery<TEntity> ConvertToDocumentQuery(IQueryable<TEntity> query);
        Task<IEnumerable<TEntity>> GetEntities(IDocumentQuery<TEntity> docQuery, CancellationToken cancellationToken);
    }

}
