using System;
using System.Threading.Tasks;
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
            return RunAsync(f => f.Handle(), f => f.VerifySend<CreateAccountCommand>((c, m) => 
                c.AccountId == m.AccountId &&
                c.HashedId == m.HashedId &&
                c.PublicHashedId == m.PublicHashedId &&
                c.Name == m.Name &&
                c.Created == m.Created));
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