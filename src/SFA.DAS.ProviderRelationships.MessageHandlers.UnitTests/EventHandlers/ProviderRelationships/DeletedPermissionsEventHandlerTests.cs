using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    public class DeletedPermissionsEventHandlerTests : FluentTest<DeletedPermissionsEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEvent_ThenShouldSendDeletePermissionsCommand()
        {
            return RunAsync(f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.ReadStoreMediator.Verify(m => m.Send(It.Is<DeletePermissionsCommand>(c =>
                        c.AccountProviderLegalEntityId == f.Message.AccountProviderLegalEntityId &&
                        c.Ukprn == f.Message.Ukprn &&
                        c.Deleted == f.Message.Deleted &&
                        c.MessageId == f.MessageId), 
                    It.IsAny<CancellationToken>()), Times.Once));
        }

        [Test]
        public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldSendAuditCommand()
        {
            return RunAsync(f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object), f => f.Mediator.Verify(m => m.Send(It.Is<DeletedPermissionsEventAuditCommand>(c =>
                c.Deleted == f.Deleted &&
                c.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                c.Ukprn == f.Ukprn
            ), CancellationToken.None), Times.Once));
        }
    }

    public class DeletedPermissionsEventHandlerTestsFixture
    {
        public DeletedPermissionsEvent Message { get; set; }
        public string MessageId { get; set; }
        public Mock<IMessageHandlerContext> MessageHandlerContext { get; set; }
        public IHandleMessages<DeletedPermissionsEvent> Handler { get; set; }
        public Mock<IReadStoreMediator> ReadStoreMediator { get; set; }
        public Mock<IMediator> Mediator { get; set; }

        public long Ukprn { get; set; }
        public long AccountProviderLegalEntityId { get; set; }
        public DateTime Deleted { get; set; }
        
        public DeletedPermissionsEventHandlerTestsFixture()
        {
            Ukprn = 1122277833;
            AccountProviderLegalEntityId = 112;
            Deleted = DateTime.UtcNow;
            Message = new DeletedPermissionsEvent(AccountProviderLegalEntityId, Ukprn, Deleted);
            MessageId = Guid.NewGuid().ToString();
            MessageHandlerContext = new Mock<IMessageHandlerContext>();
            ReadStoreMediator = new Mock<IReadStoreMediator>();
            Mediator = new Mock<IMediator>();

            MessageHandlerContext.Setup(c => c.MessageId).Returns(MessageId);
            
            Handler = new DeletedPermissionsEventHandler(ReadStoreMediator.Object, Mediator.Object);
        }
    }
}