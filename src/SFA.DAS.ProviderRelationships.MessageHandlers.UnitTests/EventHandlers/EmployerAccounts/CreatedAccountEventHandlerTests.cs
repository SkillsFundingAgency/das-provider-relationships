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
    public class CreatedAccountEventHandlerTests : FluentTest<CreatedAccountEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldSendCreateAccountLegalEntityCommand()
        {
            return RunAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<CreateAccountCommand>(c => 
                c.AccountId == f.Message.AccountId &&
                c.PublicHashedId == f.Message.PublicHashedId &&
                c.Name == f.Message.Name &&
                c.Created == f.Message.Created), CancellationToken.None), Times.Once));
        }
    }

    public class CreatedAccountEventHandlerTestsFixture
    {
        public Mock<IMediator> Mediator { get; set; }
        public CreatedAccountEvent Message { get; set; }
        public IHandleMessages<CreatedAccountEvent> Handler { get; set; }
        
        public CreatedAccountEventHandlerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            
            Message = new CreatedAccountEvent
            {
                AccountId = 1,
                PublicHashedId = "AAA123",
                Name = "Foo",
                Created = DateTime.UtcNow
            };
            
            Handler = new CreatedAccountEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, null);
        }
    }
}