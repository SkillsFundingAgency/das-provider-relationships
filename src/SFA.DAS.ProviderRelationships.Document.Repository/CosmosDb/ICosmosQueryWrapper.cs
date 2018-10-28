using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public interface ICosmosQueryWrapper<T> : IDisposable
    {
        IDocumentQuery<T> DocumentQuery { get; set; }
        Task<IEnumerable<TResult>> ExecuteNextAsync<TResult>(CancellationToken token);
        Task<IEnumerable<TResult>> ExecuteQuery<TResult>(CancellationToken token);
        bool HasMoreResults { get; }
    }
}