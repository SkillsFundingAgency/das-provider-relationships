using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingChangedAccountNameEvent_ThenShouldSendUpdateAccountNameCommand()
        {
            return RunAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<UpdateAccountNameCommand>(c => 
                c.AccountId == f.Message.AccountId &&
                c.Name == f.Message.CurrentName &&
                c.Created == f.Message.Created), CancellationToken.None), Times.Once));
        }
    }

    public class ChangedAccountNameEventHandlerTestsFixture
    {
        public Mock<IMediator> Mediator { get; set; }
        public ChangedAccountNameEvent Message { get; set; }
        public IHandleMessages<ChangedAccountNameEvent> Handler { get; set; }
        
        public ChangedAccountNameEventHandlerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            
            Message = new ChangedAccountNameEvent
            {
                AccountId = 1,
                CurrentName = "Foo",
                Created = DateTime.UtcNow
            };
            
            Handler = new ChangedAccountNameEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, null);
        }
    }
}