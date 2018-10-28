using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using SFA.DAS.ProviderRelationships.Document.Repository.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{

    public static class CosmosDbExtensions
    {
        public static bool TestMode = false;

        public static ICosmosQueryWrapper<T> AsDocumentQueryWrapper<T>(this IQueryable<T> query)
        {
            Registry registry = TestMode ? (Registry) new FakeDocumentRegistry() : new DocumentRegistry();
            var container = new Container(registry);
            var instance = container.GetInstance<ICosmosQueryWrapper<T>>();
            instance.DocumentQuery = query.AsDocumentQuery();
            return instance;
        }

        //public static async Task<IEnumerable<TEntity>> GetEntities<TEntity>(this ICosmosQueryWrapper<TEntity> docQuery, CancellationToken cancellationToken)
        //{
        //    return await docQuery.ExecuteNextAsync<TEntity>(cancellationToken);
        //}

    }
}
