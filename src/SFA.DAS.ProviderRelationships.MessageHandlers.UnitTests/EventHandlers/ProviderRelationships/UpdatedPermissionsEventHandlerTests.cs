using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships;

[TestFixture]
[Parallelizable]
internal class UpdatedPermissionsEventHandlerTests : FluentTest<UpdatedPermissionsEventHandlerTestsFixture>
{
    [Test]
    public Task Handle_WhenHandlingEvent_ThenShouldSendUpdatePermissionsCommand()
    {
        return TestAsync(f => f.Handle(),f => f.VerifySend<UpdatePermissionsCommand>((c, m) =>
            c.Ukprn == m.Ukprn &&
            c.AccountProviderLegalEntityId == m.AccountProviderLegalEntityId &&
            c.GrantedOperations == m.GrantedOperations &&
            c.Updated == m.Updated &&
            c.MessageId == f.MessageId));
    }

    [Test]
    public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldSendAuditCommand()
    {
        return TestAsync(f => f.Handle(), f => f.VerifySend<UpdatedPermissionsEventAuditCommand>((c, m) =>
            c.UserRef == m.UserRef &&
            m.GrantedOperations.All(fo => c.GrantedOperations.Any(co => co == fo)) &&
            c.AccountId == m.AccountId &&
            c.Updated == m.Updated &&
            c.AccountLegalEntityId == m.AccountLegalEntityId &&
            c.AccountProviderLegalEntityId == m.AccountProviderLegalEntityId &&
            c.AccountProviderId == m.AccountProviderId &&
            c.Ukprn == m.Ukprn));
    }

    [Test]
    public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldSendNotifyCommand()
    {
        return TestAsync(f => f.Handle(), f => f.VerifySend<SendUpdatedPermissionsNotificationCommand>((c, m) =>
            c.AccountLegalEntityId == m.AccountLegalEntityId &&
            c.Ukprn == m.Ukprn));
    }
}

internal class UpdatedPermissionsEventHandlerTestsFixture : EventHandlerTestsFixture<UpdatedPermissionsEvent, UpdatedPermissionsEventHandler>
{
}