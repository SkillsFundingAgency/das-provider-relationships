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
        public bool HasMoreResults => false;

        public Task<IEnumerable<T>> ExecuteAsync(CancellationToken token)
        {
            return ExecuteNextAsync(token);
        }

        public Task<IEnumerable<T>> ExecuteNextAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() => Data.ToList().AsEnumerable(), cancellationToken); 
        }

        public void Dispose()
        {
        }
    }
}