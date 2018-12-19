using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;
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
                f => f.Mediator.Verify(m => m.Send(It.Is<DeletePermissionsCommand>(c =>
                        c.AccountProviderLegalEntityId == f.Message.AccountProviderLegalEntityId &&
                        c.Ukprn == f.Message.Ukprn &&
                        c.Deleted == f.Message.Deleted &&
                        c.MessageId == f.MessageId), 
                    It.IsAny<CancellationToken>()), Times.Once));
        }
    }

    public class DeletedPermissionsEventHandlerTestsFixture
    {
        public DeletedPermissionsEvent Message { get; set; }
        public string MessageId { get; set; }
        public Mock<IMessageHandlerContext> MessageHandlerContext { get; set; }
        public IHandleMessages<DeletedPermissionsEvent> Handler { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        
        public DeletedPermissionsEventHandlerTestsFixture()
        {
            Message = new DeletedPermissionsEvent(1, 12345678, DateTime.UtcNow);
            MessageId = Guid.NewGuid().ToString();
            MessageHandlerContext = new Mock<IMessageHandlerContext>();
            Mediator = new Mock<IMediator>();

            MessageHandlerContext.Setup(c => c.MessageId).Returns(MessageId);
            
            Handler = new DeletedPermissionsEventHandler(Mediator.Object);
        }
    }
}