using System.Threading.Tasks;
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
            return RunAsync(f => f.Handle(), f => f.VerifySend<RemoveAccountLegalEntityCommand>((c, m) =>
                c.AccountId == m.AccountId && c.AccountLegalEntityId == m.AccountLegalEntityId && c.Removed == m.Created));
        }
    }

    public class RemovedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<RemovedLegalEntityEvent, RemovedLegalEntityEventHandler>
    {
    }
}