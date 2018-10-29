using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using Z.EntityFramework.Plus;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    public abstract class EventHandlerTestsFixture<T> where T : Event
    {
        public DateTime Now { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IHandleMessages<T> Handler { get; set; }
        public T Message { get; set; }

        protected EventHandlerTestsFixture(Func<Lazy<ProviderRelationshipsDbContext>, IHandleMessages<T>> createEventHandler)
        {
            Now = DateTime.UtcNow;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            Db = new ProviderRelationshipsDbContext(dbContextOptionsBuilder.Options);
            
            BatchDeleteManager.InMemoryDbContextFactory = () => Db;
            
            var lazyDb = new Lazy<ProviderRelationshipsDbContext>(() => Db);
            
            Handler = createEventHandler(lazyDb);
        }

        public virtual async Task Handle()
        {
            await Handler.Handle(Message, null);
            await Db.SaveChangesAsync();
        }
    }
}