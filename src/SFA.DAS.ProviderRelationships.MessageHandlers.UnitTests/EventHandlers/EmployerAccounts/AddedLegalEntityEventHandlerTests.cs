using System.Threading.Tasks;
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
            return RunAsync(f => f.Handle(), f => f.VerifySend<AddAccountLegalEntityCommand>((c, m) => 
                c.AccountId == m.AccountId &&
                c.AccountLegalEntityId == m.AccountLegalEntityId &&
                c.AccountLegalEntityPublicHashedId == m.AccountLegalEntityPublicHashedId &&
                c.OrganisationName == m.OrganisationName &&
                c.Created == m.Created));
        }
    }

    public class AddedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<AddedLegalEntityEvent, AddedLegalEntityEventHandler>
    {
        public AddedLegalEntityEventHandlerTestsFixture()
        {
            Message.AccountId = 1;
            Message.AccountLegalEntityId = 2;
            Message.AccountLegalEntityPublicHashedId = "ALE123";
            Message.OrganisationName = "Foo";
        }
    }
}