using Moq;
using NServiceBus;
using NServiceBus.Testing;
using SFA.DAS.NLog.Logger;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    public class EventHandlerTestsFixture<TEvent, TEventHandler>
        where TEvent : Event
        where TEventHandler : IHandleMessages<TEvent> //, new()
    {
        public IHandleMessages<TEvent> Handler { get; set; }

        public TestProviderRelationshipsDbContext Db { get; set; }
        public IMessageHandlerContext MessageHandlerContext { get; }

        public EventHandlerTestsFixture()
        {
            Db = new TestProviderRelationshipsDbContext();

            MessageHandlerContext = new TestableMessageHandlerContext();

            Handler = (TEventHandler) Activator.CreateInstance(typeof(TEventHandler), new object[] { new Lazy<IProviderRelationshipsDbContext>(() => Db), Mock.Of<ILog>() });
            //Handler = new TEventHandler(new Lazy<IProviderRelationshipsDbContext>(() => Db), Mock.Of<ILog>());
        }

        public virtual Task Handle(TEvent theEvent)
        {
            return Handler.Handle(theEvent, MessageHandlerContext);
        }
    }
}
