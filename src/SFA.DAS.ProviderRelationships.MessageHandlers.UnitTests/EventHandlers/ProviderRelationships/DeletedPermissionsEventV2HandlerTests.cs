using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.SendDeletedPermissionsNotification;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    public class DeletedPermissionsEventV2HandlerTests : FluentTest<DeletedPermissionsEventV2HandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEvent_ThenShouldSendNotifyCommand()
        {
            return RunAsync(f => f.Handle(), f => f.VerifySend<SendDeletedPermissionsNotificationCommand>((c, m) =>
                c.AccountLegalEntityId == m.AccountLegalEntityId && c.Ukprn == m.Ukprn));
        }
    }

    public class DeletedPermissionsEventV2HandlerTestsFixture : EventHandlerTestsFixture<DeletedPermissionsEventV2, DeletedPermissionsEventV2Handler>
    {
    }
}