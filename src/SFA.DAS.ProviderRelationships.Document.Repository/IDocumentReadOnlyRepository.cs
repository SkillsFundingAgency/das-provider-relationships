using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentReadOnlyRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate);
        Task<bool> FindAny(Expression<Func<TEntity, bool>> predicate);
    }

}
