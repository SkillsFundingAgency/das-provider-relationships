using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.ViewModels
{
    [TestFixture]
    [Parallelizable]
    public class PermissionViewModelUnitTests : FluentTest<object>
    {
        [TestCase(Permission.CreateCohort, State.No, "Cannot add apprentice records")]
        [TestCase(Permission.CreateCohort, State.Yes, "Add apprentice records")]
        [TestCase(Permission.CreateCohort, State.Conditional, "")]
        [TestCase(Permission.Recruitment, State.No, "Cannot create job adverts")]
        [TestCase(Permission.Recruitment, State.Yes, "Create and publish job adverts")]
        [TestCase(Permission.Recruitment, State.Conditional, "Create job adverts")]

        public void GetModelString_WhenGettingModelString_ThenShouldReturnDescription(Permission permission, State state, string expected)
        {
            var permissionViewModel = new PermissionViewModel {State = state, Value = permission};
            Assert.AreEqual(expected, permissionViewModel.ToString());
        }
    }
}
