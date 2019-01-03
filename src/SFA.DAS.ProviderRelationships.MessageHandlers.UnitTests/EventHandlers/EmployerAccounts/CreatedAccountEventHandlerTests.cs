using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateAccount;
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
    }

    public class CreatedAccountEventHandlerTestsFixture : EventHandlerTestsFixture<CreatedAccountEvent, CreatedAccountEventHandler>
    {
        public CreatedAccountEventHandlerTestsFixture()
        {
            Message.AccountId = 1;
            Message.HashedId = "AAA111";
            Message.PublicHashedId = "AAA222";
            Message.Name = "Foo";
            Message.Created = DateTime.UtcNow;
        }
    }
}