using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public interface IDocumentQueryWrapper<T> : IDisposable
    {
        bool HasMoreResults { get; }
        Task<IEnumerable<T>> ExecuteAsync(CancellationToken token);
        Task<IEnumerable<T>> ExecuteNextAsync(CancellationToken token);
    }
}