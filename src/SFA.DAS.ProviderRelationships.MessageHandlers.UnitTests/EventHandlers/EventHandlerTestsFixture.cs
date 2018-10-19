using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    public class EventHandlerTestsFixture<TEvent> where TEvent : Event, new()
    {
        public DateTime Now { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public TEvent Message { get; set; }
        public IHandleMessages<TEvent> Handler { get; set; }

        public EventHandlerTestsFixture(Func<Lazy<ProviderRelationshipsDbContext>, IHandleMessages<TEvent>> createEventHandler)
        {
            Now = DateTime.UtcNow;
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            //Message = new TEvent();
            
            var lazyDbContext = new Lazy<ProviderRelationshipsDbContext>(() => Db);
            Handler = createEventHandler(lazyDbContext);
        }

        public virtual async Task Handle()
        {
            await Handler.Handle(Message, null);
            await Db.SaveChangesAsync();
        }
    }
}