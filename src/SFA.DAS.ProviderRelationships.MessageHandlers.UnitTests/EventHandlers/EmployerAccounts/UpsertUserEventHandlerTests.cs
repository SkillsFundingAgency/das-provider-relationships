using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.UpsertUser;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class UpsertUserEventHandlerTests : FluentTest<UpsertUserEventHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedPayeSchemeEvent_ThenShouldSendUpsertUserCommand()
        {
            return RunAsync(f => f.Handle(), f => f.VerifySend<UpsertUserCommand>((c, m) =>
                c.Created == m.Created &&
                c.CorrelationId == m.CorrelationId &&
                c.UserRef == m.UserRef));
        }
    }

    public class UpsertUserEventHandlerTestFixture : EventHandlerTestsFixture<UpsertedUserEvent, UpsertedUserEventHandler>
    {
    }
}