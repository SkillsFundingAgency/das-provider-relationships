using System.Linq;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository
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
