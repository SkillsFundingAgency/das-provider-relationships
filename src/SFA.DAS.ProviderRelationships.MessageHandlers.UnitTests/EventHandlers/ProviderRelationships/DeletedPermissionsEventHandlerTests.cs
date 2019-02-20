using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    public class DeletedPermissionsEventHandlerTests : FluentTest<DeletedPermissionsEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEvent_ThenShouldSendDeletePermissionsCommand()
        {
            return RunAsync(f => f.Handle(),f => f.VerifySend<DeletePermissionsCommand>((c, m) =>
                        c.AccountProviderLegalEntityId == m.AccountProviderLegalEntityId &&
                        c.Ukprn == m.Ukprn &&
                        c.Deleted == m.Deleted &&
                        c.MessageId == f.MessageId));
        }

        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEvent_ThenShouldSendAuditCommand()
        {
            return RunAsync(f => f.Handle(), f => f.VerifySend<DeletedPermissionsEventAuditCommand>((c, m) =>
                c.Deleted == m.Deleted && c.AccountProviderLegalEntityId == m.AccountProviderLegalEntityId && c.Ukprn == m.Ukprn));
        }

        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEvent_ThenShouldSendNotifyCommand()
        {
            return RunAsync(f => f.Handle(), f => f.VerifySend<DeletedPermissionsEventNotifyCommand>((c, m) =>
                c.AccountId == m.AccountId && c.AccountProviderId == m.AccountProviderId));
        }
    }

    public class DeletedPermissionsEventHandlerTestsFixture : EventHandlerTestsFixture<DeletedPermissionsEvent, DeletedPermissionsEventHandler>
    {
    }
}