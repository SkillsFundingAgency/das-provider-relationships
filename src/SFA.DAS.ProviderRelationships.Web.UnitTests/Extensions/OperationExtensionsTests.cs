using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Extensions
{
    [TestFixture]
    [Parallelizable]
    public class PermissionOperationExtensionsTests : FluentTest<object>
    {
        [Test]
        public void GetDescription_WhenGettingDescription_ThenShouldReturnDescription()
        {
            Run(f => Permission.CreateCohort.GetDescription(), (f, r) => r.Should().Be("Add apprentice records"));
            Run(f => Permission.Recruitment.GetDescription(), (f, r) => r.Should().Be("Recruit apprentices"));
        }
    }
}