using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.Web.Data
{
    public class EntityFrameworkCoreUnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly Lazy<T> _db;

        public EntityFrameworkCoreUnitOfWork(Lazy<T> db)
        {
            _db = db;
        }

        public async Task CommitAsync(Func<Task> next)
        {
            _db.Value.SaveChanges();
            await next().ConfigureAwait(false);
        }
    }
}