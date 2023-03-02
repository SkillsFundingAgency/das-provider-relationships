using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.RouteValues.Permissions;
using SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;
using SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.Permissions;

[TestFixture]
[Parallelizable]
public class RevokeTests : FluentTest<RevokeTestsFixture>
{
    [Test]
    public Task WhenUkprnIsNull_ThenShouldReturnBadRequest() =>
        TestAsync(
            arrange: fixture =>
            {
                fixture.RevokePermissionsRouteValues.Ukprn = null;
            },
            act: async fixture => await fixture.PermissionsController.Revoke(fixture.RevokePermissionsRouteValues),
            assert: (_, result) =>
            {
                result.Should().BeAssignableTo<BadRequestObjectResult>();
                result.AssertModelError(nameof(RevokePermissionsRouteValues.Ukprn), "A Ukprn needs to be supplied");
            }
        );

    [Test]
    public Task WhenAccountLegalEntityPublicHashedIdIsNull_ThenShouldReturnBadRequest() =>
        TestAsync(
            arrange: fixture =>
            {
                fixture.RevokePermissionsRouteValues.AccountLegalEntityPublicHashedId = null;
            },
            act: async fixture => await fixture.PermissionsController.Revoke(fixture.RevokePermissionsRouteValues),
            assert: (_, result) =>
            {
                result.Should().BeAssignableTo<BadRequestObjectResult>();
                result.AssertModelError(nameof(RevokePermissionsRouteValues.AccountLegalEntityPublicHashedId), "A Public Hashed Id for an Account Legal Entity needs to be supplied");
            }
        );

    [TestCase(null)]
    [TestCase(new Operation[0])]
    public Task WhenOperationsToRemoveIdIsNullOrEmpty_ThenShouldReturnBadRequest(Operation[] operations) =>
        TestAsync(
            arrange: fixture =>
            {
                fixture.RevokePermissionsRouteValues.OperationsToRevoke = operations;
            },
            act: async fixture => await fixture.PermissionsController.Revoke(fixture.RevokePermissionsRouteValues),
            assert: (_, result) =>
            {
                result.Should().BeAssignableTo<BadRequestObjectResult>();
                result.AssertModelError(nameof(RevokePermissionsRouteValues.OperationsToRevoke), "One or more operations need to be supplied");
            }
        );

    [Test]
    public Task WhenRequestIsValid_ThenShouldExecuteCommand() =>
        TestAsync(
            act: async fixture => await fixture.PermissionsController.Revoke(fixture.RevokePermissionsRouteValues),
            assert: (fixture, result) =>
            {
                result.Should().BeAssignableTo<OkResult>();
                fixture.Mediator.Verify(x => x.Send(
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
            operationsToRevoke: new[] { Operation.Recruitment });

        Mediator = new Mock<IMediator>();
        Mediator
            .Setup(x => x.Send(It.IsAny<RevokePermissionsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        PermissionsController = new PermissionsController(Mediator.Object);
    }
}