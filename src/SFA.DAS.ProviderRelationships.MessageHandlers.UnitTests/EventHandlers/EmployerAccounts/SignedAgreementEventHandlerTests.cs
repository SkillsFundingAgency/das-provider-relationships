using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.SignedAgreement;
using SFA.DAS.ProviderRelationships.Application.Commands.UpsertUser;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class SignedAgreementEventHandlerTests : FluentTest<SignedAgreementEventHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedPayeSchemeEvent_ThenShouldSendSignedAgreementCommand()
        {
            return RunAsync(f => f.Handle(), f => f.VerifySend<SignedAgreementCommand>((c, m) =>
                c.AccountId == m.AccountId &&
                c.AgreementId == m.AgreementId &&
                c.LegalEntityId == m.LegalEntityId &&
                c.OrganisationName == m.OrganisationName &&
                c.UserName == m.UserName &&
                c.UserRef == m.UserRef &&
                c.CorrelationId == m.CorrelationId));
        }
    }

    public class SignedAgreementEventHandlerTestFixture : EventHandlerTestsFixture<SignedAgreementEvent, SignedAgreementEventHandler>
    {
    }
}