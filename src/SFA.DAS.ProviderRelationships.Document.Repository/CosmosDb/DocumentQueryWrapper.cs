using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class DocumentQueryWrapper<T> : IDocumentQueryWrapper<T>
    {
        public IDocumentQuery<T> DocumentQuery { get; set; }

        public bool HasMoreResults
        {
            get
            {
                if (DocumentQuery == null)
                    throw new ArgumentNullException(nameof(DocumentQuery), "DocumentQuery must be set before calling HasMoreResults");

                return DocumentQuery.HasMoreResults;
            }
        }

        public async Task<IEnumerable<T>> ExecuteAsync(CancellationToken token)
        {
            if (DocumentQuery == null)
                throw new ArgumentNullException(nameof(DocumentQuery), "DocumentQuery must be set before calling this method");

            var results = new List<T>();
            
            while (DocumentQuery.HasMoreResults)
            {
                results.AddRange(await DocumentQuery.ExecuteNextAsync<T>(token));
            }
            
            return results;
        }

        public async Task<IEnumerable<T>> ExecuteNextAsync(CancellationToken token = new CancellationToken())
        {
            if(DocumentQuery == null)
                throw new ArgumentNullException(nameof(DocumentQuery), "DocumentQuery must be set before calling this method");

            return await DocumentQuery.ExecuteNextAsync<T>(token);
        }

        public void Dispose()
        {
            DocumentQuery?.Dispose();
        }
    }
}