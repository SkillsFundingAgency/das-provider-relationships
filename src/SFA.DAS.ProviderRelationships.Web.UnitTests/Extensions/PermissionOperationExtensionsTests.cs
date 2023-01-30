using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions;
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
            Test(f => Permission.CreateCohort.GetDescription(), (f, r) => r.Should().Be("Add apprentice records"));
            Test(f => Permission.Recruitment.GetDescription(), (f, r) => r.Should().Be("Recruit apprentices"));
        }

        [TestCase(Permission.CreateCohort, State.No, "Do not allow")]
        [TestCase(Permission.CreateCohort, State.Yes, "Allow")]
        [TestCase(Permission.CreateCohort, State.Conditional, "")]
        [TestCase(Permission.Recruitment, State.No, "Do not allow")]
        [TestCase(Permission.Recruitment, State.Yes, "Allow")]
        [TestCase(Permission.Recruitment, State.Conditional, "Allow, but I want to review adverts before they�re advertised")]

        public void GetStateDescription_WhenGettingStateDescription_ThenShouldReturnDescription(Permission permission, State state, string expected)
        {
            var permissionViewModel = new PermissionViewModel {State = state, Value = permission};
            Assert.AreEqual(expected, permissionViewModel.Status());
        }

        [TestCaseSource(nameof(OperationToPermissions))]
        public void ToPermissions_WhenMappingOperationsToPermissions_ThenShouldMapCorrectly(List<Operation> operations, List<PermissionViewModel> permissions)
        {
            var result = operations.ToPermissions();

            CollectionAssert.AreEquivalent(permissions, result);
        }

        [TestCaseSource(nameof(OperationToPermissions))]
        public void ToOperations_WhenMappingOperationsToPermissions_ThenShouldMapCorrectly(List<Operation> operations, List<PermissionViewModel> permissions)
        {
            var result = permissions.ToOperations();

            CollectionAssert.AreEquivalent(operations.ToHashSet(), result);
        }

        private static readonly object[] OperationToPermissions = {
            new object[] {
                new List<Operation> (),
                new List<PermissionViewModel>
                {
                    new PermissionViewModel {Value = Permission.Recruitment, State = State.No},
                    new PermissionViewModel {Value = Permission.CreateCohort, State = State.No}
                }
            },
            new object[] {
                new List<Operation> {Operation.Recruitment},
                new List<PermissionViewModel>
                {
                    new PermissionViewModel {Value = Permission.Recruitment, State = State.Yes},
                    new PermissionViewModel {Value = Permission.CreateCohort, State = State.No}
                }
            },
            new object[] {
                new List<Operation> {Operation.CreateCohort},
                new List<PermissionViewModel>
                {
                    new PermissionViewModel {Value = Permission.Recruitment, State = State.No},
                    new PermissionViewModel {Value = Permission.CreateCohort, State = State.Yes}
                }
            },
            new object[] {
                new List<Operation> {Operation.Recruitment, Operation.CreateCohort},
                new List<PermissionViewModel>
                {
                    new PermissionViewModel {Value = Permission.Recruitment, State = State.Yes},
                    new PermissionViewModel {Value = Permission.CreateCohort, State = State.Yes}
                }
            },
            new object[] {
                new List<Operation> {Operation.Recruitment, Operation.RecruitmentRequiresReview},
                new List<PermissionViewModel>
                {
                    new PermissionViewModel {Value = Permission.Recruitment, State = State.Conditional},
                    new PermissionViewModel {Value = Permission.CreateCohort, State = State.No}
                }
            },
            new object[] {
                new List<Operation> {Operation.CreateCohort, Operation.RecruitmentRequiresReview, Operation.Recruitment},
                new List<PermissionViewModel>
                {
                    new PermissionViewModel {Value = Permission.Recruitment, State = State.Conditional},
                    new PermissionViewModel {Value = Permission.CreateCohort, State = State.Yes}
                }
            }
        };
    }
}