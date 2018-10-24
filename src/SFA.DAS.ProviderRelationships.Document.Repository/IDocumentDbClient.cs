using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentDbClient<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(string collection, Guid id);
        IQueryable<TEntity> CreateQuery(string collection, Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> CreateQuery(string collection, Expression<Func<TEntity, bool>> predicate, FeedOptions feedOptions);

    }

}
