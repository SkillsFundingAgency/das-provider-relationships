using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class CosmosQueryWrapper<T> : ICosmosQueryWrapper<T>
    {
        public IDocumentQuery<T> DocumentQuery { get; set; }

        public async Task<IEnumerable<TResult>> ExecuteNextAsync<TResult>(CancellationToken token = new CancellationToken())
        {
            if(DocumentQuery == null)
                throw new ArgumentNullException(nameof(DocumentQuery), "DocumentQuery must be set before calling this method");

            return await DocumentQuery.ExecuteNextAsync<TResult>(token);
        }

        public async Task<IEnumerable<TResult>> ExecuteQuery<TResult>(CancellationToken token)
        {
            if (DocumentQuery == null)
                throw new ArgumentNullException(nameof(DocumentQuery), "DocumentQuery must be set before calling this method");

            var results = new List<TResult>();
            while (DocumentQuery.HasMoreResults)
            {
                results.AddRange(await DocumentQuery.ExecuteNextAsync<TResult>(token));
            }
            return results;
        }

        public bool HasMoreResults {
            get {
                if (DocumentQuery == null)
                    throw new ArgumentNullException(nameof(DocumentQuery), "DocumentQuery must be set before calling HasMoreResults");

                return DocumentQuery.HasMoreResults;
            }
        }

        public void Dispose()
        {
            DocumentQuery?.Dispose();
        }

    }
}