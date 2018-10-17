using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentDbClient<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(string collection, Guid id);
        //Task Save(string collection, TEntity entity);
        //Task Delete(string collection, Guid id);
        Task<IEnumerable<TEntity>> Search(string collection, Expression<Func<TEntity, bool>> sqlApiQuery);
        Task<IEnumerable<TEntity>> Search(string collection, Expression<Func<TEntity, bool>> sqlApiQuery, FeedOptions feedOptions);

    }

}
