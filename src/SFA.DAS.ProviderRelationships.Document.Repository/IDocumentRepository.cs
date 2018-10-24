using System;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentRepository<TEntity> : IDocumentReadOnlyRepository<TEntity> where TEntity : class
    {
        Task Update(TEntity entity);
        Task Remove(Guid id);
    }
}
