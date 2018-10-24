using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate)
        {
            var results = _list.Where(predicate.Compile());
            return Task.Run(() => results);
        }

        public Task<bool> FindAny(Expression<Func<T, bool>> predicate)
        {
            var result = _list.Any(predicate.Compile());
            return Task.Run(() => result);

        }

        public Task<T> GetById(Guid id)
        {
            return Task.Run(() => _singleItem);
        }
    }
}
