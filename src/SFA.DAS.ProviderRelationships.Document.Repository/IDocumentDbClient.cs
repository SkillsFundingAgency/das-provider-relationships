using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentDbClient<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(string collection, Guid id);
        IOrderedQueryable<TEntity> CreateQuery(string collection);
        IOrderedQueryable<TEntity> CreateQuery(string collection, FeedOptions feedOptions);
    }

}
