using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class FakeReadOnlyRepository<T> : IDocumentReadOnlyRepository<T> where T : class
    {
        private readonly List<T> _list;
        private readonly T _singleItem;

        public FakeReadOnlyRepository(List<T> list, T singleItem)
        {
            _list = list;
            _singleItem = singleItem;
        }

        public IQueryable<T> CreateQuery()
        {
            return _list.AsQueryable();
        }

        public IQueryable<T> CreateQuery(FeedOptions options)
        {
            return _list.AsQueryable();
        }

        public Task<T> GetById(Guid id)
        {
            return Task.Run(() => _singleItem);
        }

        public Task<IEnumerable<T>> ExecuteQuery(IQueryable<T> query, CancellationToken cancellationToken)
        {
            return Task.Run(()=>query.AsEnumerable(), cancellationToken);
        }
    }
}
