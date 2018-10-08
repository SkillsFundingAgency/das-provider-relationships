using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    [TestFixture]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
    }

    public class ChangedAccountNameEventHandlerTestsFixture : EventHandlerTestsFixture<ChangedAccountNameEvent, ChangedAccountNameEventHandler>
    {
    }
}
    