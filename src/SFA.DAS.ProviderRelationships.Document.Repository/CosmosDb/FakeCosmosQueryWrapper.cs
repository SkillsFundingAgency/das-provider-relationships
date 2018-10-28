using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class FakeCosmosQueryWrapper<T> : ICosmosQueryWrapper<T>
    {
        private readonly IEnumerable<T> _data;

        public FakeCosmosQueryWrapper(IEnumerable<T> data)
        {
            _data = data;
        }

        public IDocumentQuery<T> DocumentQuery { get; set; }

        public async Task<IEnumerable<T>> ExecuteNextAsync<T>(CancellationToken token = new CancellationToken())
        {
            return (IEnumerable<T>) await Task.Run(() => _data.ToList().AsEnumerable()); 
            //return await DocumentQuery.ExecuteNextAsync<TResult>(token);
        }

        public Task<IEnumerable<T>> ExecuteQuery<T>(CancellationToken token)
        {
            return ExecuteNextAsync<T>(token);
        }

        public bool HasMoreResults => true;

        public void Dispose()
        {
        }

    }
}