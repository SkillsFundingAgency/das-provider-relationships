using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Audit.Commands;
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
                c.HashedId == f.Message.HashedId &&
                c.PublicHashedId == f.Message.PublicHashedId &&
                c.Name == f.Message.Name &&
                c.Created == f.Message.Created), CancellationToken.None), Times.Once));
        }

        [Test]
        public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldSendAuditCommand()
        {
            return RunAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<CreatedAccountEventAuditCommand>(c =>
                c.AccountId == f.Message.AccountId &&
                c.UserRef == f.Message.UserRef &&
                c.UserName == f.Message.UserName &&
                c.Name == f.Message.Name &&
                c.PublicHashedId == f.Message.PublicHashedId), CancellationToken.None), Times.Once));
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
                HashedId = "AAA111",
                PublicHashedId = "AAA222",
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