using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public static class CosmosDbExtensions
    {
        internal static bool TestMode = false;

        public static IDocumentQueryWrapper<T> AsDocumentQueryWrapper<T>(this IQueryable<T> query)
        {
            if (TestMode)
                return new FakeCosmosQueryWrapper<T> {
                    Data = query.AsEnumerable()
                };

            return new DocumentQueryWrapper<T> {
                DocumentQuery = query.AsDocumentQuery()
            };
        }
    }
}
