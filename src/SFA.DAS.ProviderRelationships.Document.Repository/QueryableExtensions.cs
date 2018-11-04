using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public static class QueryableExtensions
    {
        public static async Task<bool> AnyAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            var items = await source
                .Take(1)
                .Select(p => 1)
                .AsDocumentQuery()
                .ExecuteNextAsync<int>(cancellationToken)
                .ConfigureAwait(false);

            return items.Any();
        }
        
        public static Task<bool> AnyAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Where(predicate).AnyAsync(cancellationToken);
        }
        
        public static async Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            var items = await source
                .Take(2)
                .AsDocumentQuery()
                .ExecuteNextAsync<T>(cancellationToken)
                .ConfigureAwait(false);

            return items.SingleOrDefault();
        }
        
        public static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Where(predicate).SingleOrDefaultAsync(cancellationToken);
        }
        
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            var query = source.AsDocumentQuery();
            var results = new List<T>();

            while (query.HasMoreResults)
            {
                var items = await query
                    .ExecuteNextAsync<T>(cancellationToken)
                    .ConfigureAwait(false);
                
                results.AddRange(items);
            }
            
            return results;
        }
    }
}