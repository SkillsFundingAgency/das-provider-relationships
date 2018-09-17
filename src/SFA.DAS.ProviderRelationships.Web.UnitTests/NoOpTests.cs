using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests
{
    [TestFixture]
    public class NoOpTests : FluentTest<NoOpTestsFixture>
    {
        [Test]
        public void NoOp_WhenRunningATest_ThenTestsFixtureShouldNotBeNull()
        {
            Run(f => f.Should().NotBeNull());
        }
    }

    public class NoOpTestsFixture : FluentTestFixture
    {
    }
}