using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IReadOnlyDocumentRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(Guid id);
        IQueryable<TEntity> CreateQuery();
        IQueryable<TEntity> CreateQuery(FeedOptions options);
        //Task<IEnumerable<TEntity>> ExecuteQuery(IQueryable<TEntity> query, CancellationToken cancellationToken);
    }
}
