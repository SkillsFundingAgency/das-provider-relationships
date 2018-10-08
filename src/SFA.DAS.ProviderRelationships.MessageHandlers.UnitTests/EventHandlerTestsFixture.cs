using NServiceBus;
using NServiceBus.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    public class EventHandlerTestsFixture
    {
        public TestProviderRelationshipsDbContext Db { get; set; }
        public IMessageHandlerContext MessageHandlerContext { get; }

        public EventHandlerTestsFixture()
        {
            Db = new TestProviderRelationshipsDbContext();

            MessageHandlerContext = new TestableMessageHandlerContext();
        }
    }
}
