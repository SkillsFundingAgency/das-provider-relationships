using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.RemoveAccountLegalEntity;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class RemovedLegalEntityEventHandlerTests : FluentTest<RemovedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingRemoveLegalEntityEvent_ThenShouldSendRemoveAccountLegalEntityCommand()
        {
            return RunAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<RemoveAccountLegalEntityCommand>(c =>
                c.AccountId == f.Message.AccountId &&
                c.AccountLegalEntityId == f.Message.AccountLegalEntityId &&
                c.Removed == f.Message.Created), CancellationToken.None), Times.Once));
        }
    }

    public class RemovedLegalEntityEventHandlerTestsFixture
    {
        public Mock<IMediator> Mediator { get; set; }
        public RemovedLegalEntityEvent Message { get; set; }
        public IHandleMessages<RemovedLegalEntityEvent> Handler { get; set; }
        
        public RemovedLegalEntityEventHandlerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            
            Message = new RemovedLegalEntityEvent
            {
                AccountId = 1,
                AccountLegalEntityId = 2,
                Created = DateTime.UtcNow
            };
            
            Handler = new RemovedLegalEntityEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, null);
        }
    }
}