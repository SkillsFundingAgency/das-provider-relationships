using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountLegalEntity;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class AddedLegalEntityEventHandlerTests : FluentTest<AddedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedLegalEntityEvent_ThenShouldSendAddAccountLegalEntityCommand()
        {
            return RunAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<AddAccountLegalEntityCommand>(c => 
                c.AccountId == f.Message.AccountId &&
                c.AccountLegalEntityId == f.Message.AccountLegalEntityId &&
                c.AccountLegalEntityPublicHashedId == f.Message.AccountLegalEntityPublicHashedId &&
                c.OrganisationName == f.Message.OrganisationName &&
                c.Created == f.Message.Created), CancellationToken.None), Times.Once));
        }
    }

    public class AddedLegalEntityEventHandlerTestsFixture
    {
        public Mock<IMediator> Mediator { get; set; }
        public AddedLegalEntityEvent Message { get; set; }
        public IHandleMessages<AddedLegalEntityEvent> Handler { get; set; }
        
        public AddedLegalEntityEventHandlerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            
            Message = new AddedLegalEntityEvent
            {
                AccountId = 1,
                AccountLegalEntityId = 2,
                AccountLegalEntityPublicHashedId = "ALE123",
                OrganisationName = "Foo",
                Created = DateTime.UtcNow
            };
            
            Handler = new AddedLegalEntityEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, null);
        }
    }
}