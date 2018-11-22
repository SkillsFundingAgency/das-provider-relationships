using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    public class HealthCheckEventHandlerTests : FluentTest<HealthCheckEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingHealthCheckEvent_ThenShouldSendCreateAccountLegalEntityCommandReceiveProviderRelationshipsHealthCheckEventCommand()
        {
            return RunAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<ReceiveProviderRelationshipsHealthCheckEventCommand>(c => c.Id == f.Message.Id), CancellationToken.None), Times.Once));
        }
    }

    public class HealthCheckEventHandlerTestsFixture
    {
        public Mock<IMediator> Mediator { get; set; }
        public HealthCheckEvent Message { get; set; }
        public IHandleMessages<HealthCheckEvent> Handler { get; set; }
        
        public HealthCheckEventHandlerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Message = new HealthCheckEvent(1, DateTime.UtcNow);
            Handler = new HealthCheckEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, null);
        }
    }
}