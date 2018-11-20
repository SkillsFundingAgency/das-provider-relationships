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
    public class UpdatedLegalEntityEventHandlerTests : FluentTest<UpdatedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingUpdatedLegalEntityEvent_ThenShouldSendUpdateAccountLegalEntityNameCommand()
        {
            return RunAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<UpdateAccountLegalEntityNameCommand>(c => 
                c.AccountLegalEntityId == f.Message.AccountLegalEntityId &&
                c.Name == f.Message.Name &&
                c.Created == f.Message.Created), CancellationToken.None), Times.Once));
        }
    }
    
    public class UpdatedLegalEntityEventHandlerTestsFixture
    {
        public Mock<IMediator> Mediator { get; set; }
        public UpdatedLegalEntityEvent Message { get; set; }
        public IHandleMessages<UpdatedLegalEntityEvent> Handler { get; set; }
        
        public UpdatedLegalEntityEventHandlerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            
            Message = new UpdatedLegalEntityEvent
            {
                AccountLegalEntityId = 1,
                Name = "Foo",
                Created = DateTime.UtcNow
            };
            
            Handler = new UpdatedLegalEntityEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, null);
        }
    }
}