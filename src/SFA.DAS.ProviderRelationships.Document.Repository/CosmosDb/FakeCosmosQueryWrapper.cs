using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

[assembly: InternalsVisibleTo("SFA.DAS.ProviderRelatonships.Document.Repository.UnitTests")]

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    internal class FakeCosmosQueryWrapper<T> : IDocumentQueryWrapper<T>
    {
        public IEnumerable<T> Data { get; set; }

        public IDocumentQuery<T> DocumentQuery { get; set; }

        public async Task<IEnumerable<T>> ExecuteNextAsync<T>(CancellationToken token = new CancellationToken())
        {
            return (IEnumerable<T>) await Task.Run(() => Data.ToList().AsEnumerable()); 
        }

        public Task<IEnumerable<T>> ExecuteAsync<T>(CancellationToken token)
        {
            return ExecuteNextAsync<T>(token);
        }

        public bool HasMoreResults => false;

        public void Dispose()
        {
        }

    }
}