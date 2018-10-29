using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public interface IDocumentQueryWrapper<T> : IDisposable
    {
        Task<IEnumerable<TResult>> ExecuteNextAsync<TResult>(CancellationToken token);
        Task<IEnumerable<TResult>> ExecuteAsync<TResult>(CancellationToken token);
        bool HasMoreResults { get; }
    }
}