using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships;

[TestFixture]
[Parallelizable]
public class AddedAccountProviderEventHandlerTests : FluentTest<AddedAccountProviderEventHandlerTestsFixture>
{
    [Test]
    public Task Handle_WhenHandlingAddedAccountProviderEvent_ThenShouldSendAuditCommand()
    {
        return TestAsync(f => f.Handle(), f => f.VerifySend<AddedAccountProviderEventAuditCommand>((c, m) =>
            c.AccountProviderId == m.AccountProviderId &&
            c.AccountId == m.AccountId &&
            c.ProviderUkprn == m.ProviderUkprn &&
            c.UserRef == m.UserRef &&
            c.Added == m.Added));
    }
}

public class AddedAccountProviderEventHandlerTestsFixture : EventHandlerTestsFixture<AddedAccountProviderEvent, AddedAccountProviderEventHandler>
{
}