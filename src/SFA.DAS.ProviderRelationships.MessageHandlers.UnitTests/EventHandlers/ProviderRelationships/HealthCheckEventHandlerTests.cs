using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.ReceiveProviderRelationshipsHealthCheckEvent;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships;

[TestFixture]
[Parallelizable]
public class HealthCheckEventHandlerTests : FluentTest<HealthCheckEventHandlerTestsFixture>
{
    [Test]
    public Task Handle_WhenHandlingHealthCheckEvent_ThenShouldSendCreateAccountLegalEntityCommandReceiveProviderRelationshipsHealthCheckEventCommand()
    {
        return TestAsync(f => f.Handle(), f => f.VerifySend<ReceiveProviderRelationshipsHealthCheckEventCommand>((c, m) => c.Id == m.Id));
    }
}

public class HealthCheckEventHandlerTestsFixture : EventHandlerTestsFixture<HealthCheckEvent, HealthCheckEventHandler>
{
}