using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.ProviderRelationships.Api.RouteValues.Permissions;
using SFA.DAS.Testing;
using SFA.DAS.ProviderRelationships.Types.Models;
using Moq;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;
using FluentAssertions;
using SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http.Results;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.Permissions
{
    [TestFixture]
    [Parallelizable]
    public class RevokeTests : FluentTest<RevokeTestsFixture>
    {
        [Test]
        public Task WhenUkprnIsNull_ThenShouldReturnBadRequest() =>
            RunAsync(
                arrange: f =>
                {
                    f.RevokePermissionsRouteValues.Ukprn = null;
                },
                act: async f =>
                {
                    return await f.PermissionsController.Revoke(f.RevokePermissionsRouteValues);
                },
                assert: (f, r) =>
                {
                    r.Should().BeAssignableTo<InvalidModelStateResult>();
                    r.AssertSingleModelError(nameof(RevokePermissionsRouteValues.Ukprn), "A Ukprn needs to be supplied");
                }
            );

        [Test]
        public Task WhenAccountLegalEntityPublicHashedIdIsNull_ThenShouldReturnBadRequest() =>
            RunAsync(
                arrange: f =>
                {
                    f.RevokePermissionsRouteValues.AccountLegalEntityPublicHashedId = null;
                },
                act: async f =>
                {
                    return await f.PermissionsController.Revoke(f.RevokePermissionsRouteValues);
                },
                assert: (f, r) =>
                {
                    r.Should().BeAssignableTo<InvalidModelStateResult>();
                    r.AssertSingleModelError(nameof(RevokePermissionsRouteValues.AccountLegalEntityPublicHashedId), "A Public Hashed Id for an Account Legal Entity needs to be supplied");
                }
            );

        [TestCase(null)]
        [TestCase(new Operation[0])]
        public Task WhenOperationsToRemoveIdIsNullOrEmpty_ThenShouldReturnBadRequest(Operation[] operations) =>
            RunAsync(
                arrange: f =>
                {
                    f.RevokePermissionsRouteValues.OperationsToRemove = operations;
                },
                act: async f =>
                {
                    return await f.PermissionsController.Revoke(f.RevokePermissionsRouteValues);
                },
                assert: (f, r) =>
                {
                    r.Should().BeAssignableTo<InvalidModelStateResult>();
                    r.AssertSingleModelError(nameof(RevokePermissionsRouteValues.OperationsToRemove), "One or more operations need to be supplied");
                }
            );

        [Test]
        public Task WhenRequestIsValid_ThenShouldExecuteCommand() =>
            RunAsync(
                act: async f =>
                {
                    return await f.PermissionsController.Revoke(f.RevokePermissionsRouteValues);
                },
                assert: (f, r) =>
                {
                    r.Should().BeAssignableTo<OkResult>();
                    f.Mediator.Verify(x => x.Send(
                        It.Is<RevokePermissionsCommand>(c => 
                            c.Ukprn == 299792458
                            && c.AccountLegalEntityPublicHashedId == "DEADBEEF"
                            && c.OperationsToRevoke.Single() == Operation.Recruitment
                        ),
                        It.IsAny<CancellationToken>()
                    ));
                }
            );
    }

    public class RevokeTestsFixture
    {
        public RevokePermissionsRouteValues RevokePermissionsRouteValues;
        public Mock<IMediator> Mediator;
        public PermissionsController PermissionsController;

        public RevokeTestsFixture()
        {
            RevokePermissionsRouteValues = new RevokePermissionsRouteValues(
                ukprn: 299792458,
                accountLegalEntityPublicHashedId: "DEADBEEF",
                operationsToRemove: new[] { Operation.Recruitment });

            Mediator = new Mock<IMediator>();
            Mediator
                .Setup(x => x.Send(It.IsAny<RevokePermissionsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            PermissionsController = new PermissionsController(Mediator.Object);
        }
    }
}
