using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(Guid id);
        Task Save(TEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> sqlApiQuery);
    }

}
