using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NServiceBus;
using SFA.DAS.CosmosDb.Testing;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    internal abstract class ReadStoreEventHandlerTestsFixture<TEvent> where TEvent : class
    {
        internal Mock<IRelationshipsRepository> RelationshipsRepository;
        internal Mock<IMessageHandlerContext> MessageHandlerContext;

        internal List<Relationship> Relationships;
        internal FakeDocumentQuery<Relationship> DocumentQuery;

        public IHandleMessages<TEvent> Handler { get; set; }
        public TEvent Message { get; set; }

        protected ReadStoreEventHandlerTestsFixture(Func<IRelationshipsRepository, IHandleMessages<TEvent>> createEventHandler)
        {
            MessageHandlerContext = new Mock<IMessageHandlerContext>();

            RelationshipsRepository = new Mock<IRelationshipsRepository>();
            Relationships = new List<Relationship>();
            DocumentQuery = new FakeDocumentQuery<Relationship>(Relationships);
            RelationshipsRepository.Setup(r => r.CreateQuery(null)).Returns(DocumentQuery);

            Handler = createEventHandler(RelationshipsRepository.Object);
        }

        public virtual async Task Handle()
        {
            await Handler.Handle(Message, MessageHandlerContext.Object);
        }
        public ReadStoreEventHandlerTestsFixture<TEvent> SetMessageIdInContext(string messageId)
        {
            MessageHandlerContext.Setup(x => x.MessageId).Returns(messageId);
            return this;
        }
    }

}