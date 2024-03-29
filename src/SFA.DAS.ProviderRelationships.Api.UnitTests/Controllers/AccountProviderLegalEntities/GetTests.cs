using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.AccountProviderLegalEntities;

[TestFixture]
[Parallelizable]
public class GetTests : FluentTest<GetTestsFixture>
{
    [Test]
    public Task WhenValidUkprnAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
    {
        return TestAsync(fixture =>
            {
                fixture.SetUkprn(12345678);
                fixture.SetOperation(Operation.CreateCohort);
            },
            fixture => fixture.CallGet(), 
            (fixture, result) =>
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult)result).Value;
                value.Should().BeOfType<GetAccountProviderLegalEntitiesWithPermissionResponse>();
                value.Should().BeEquivalentTo(fixture.Result);
            });
    }

    [Test]
    public Task WhenValidAccountHashedIdAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
    {
        return TestAsync(fixture =>
            {
                fixture.SetAccountHashedId("TEST");
                fixture.SetOperation(Operation.CreateCohort);
            },
            fixture => fixture.CallGet(),
            (fixture, result) =>
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult)result).Value;
                value.Should().BeOfType<GetAccountProviderLegalEntitiesWithPermissionResponse>();
                value.Should().BeEquivalentTo(fixture.Result);
            });
    }

    [Test]
    public Task WhenValidAccountHashedIdAndAccountLegalIdPublicHashedIdAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
    {
        return TestAsync(fixture =>
            {
                fixture.SetAccountHashedId("XYZ123");
                fixture.SetAccountLegalEntityPublicHashedId("ABC123");
                fixture.SetOperation(Operation.CreateCohort);
            },
            fixture => fixture.CallGet(),
            (fixture, result) =>
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult)result).Value;
                value.Should().BeOfType<GetAccountProviderLegalEntitiesWithPermissionResponse>();
                value.Should().BeEquivalentTo(fixture.Result);
            });
    }
        
    [Test]
    public Task WhenValidAccountHashedIdAndAccountLegalIdPublicHashedIdAndOperationsAreSupplied_ThenShouldReturnRelationshipsFromQuery()
    {
        return TestAsync(fixture =>
            {
                fixture.SetAccountHashedId("XYZ123");
                fixture.SetAccountLegalEntityPublicHashedId("ABC123");
                fixture.SetOperations(new List<Operation>{Operation.CreateCohort});
            },
            fixture => fixture.CallGet(),
            (fixture, result) =>
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult)result).Value;
                value.Should().BeOfType<GetAccountProviderLegalEntitiesWithPermissionResponse>();
                value.Should().BeEquivalentTo(fixture.Result);
            });
    }

    [Test]
    public Task WhenUkprnAndAccountHashedIdIsMissing_ThenShouldReturnBadRequest()
    {
        return TestAsync(fixture =>
            {
                fixture.SetOperation(Operation.Recruitment);
            },
            fixture => fixture.CallGet(),
            (_, result) =>
            {
                result.AssertModelError(nameof(GetAccountProviderLegalEntitiesRouteValues.Ukprn), "Currently a Ukprn filter needs to be supplied");
                result.AssertModelError(nameof(GetAccountProviderLegalEntitiesRouteValues.AccountHashedId), "Currently an AccountHashedId filter needs to be supplied");
            });
    }

    [Test]
    public Task WhenOperationIsMissing_ThenShouldReturnBadRequest()
    {
        return TestAsync(fixture =>
            {
                fixture.SetAccountHashedId("ABC123");
                fixture.SetUkprn(12345678);
            },
            fixture => fixture.CallGet(),
            (_, result) => result.AssertModelError(nameof(GetAccountProviderLegalEntitiesRouteValues.Operation), "Currently an Operation filter needs to be supplied"));
    }
}

public class GetTestsFixture
{
    public GetAccountProviderLegalEntitiesRouteValues GetAccountProviderLegalEntitiesRouteValues { get; set; }
    public Mock<IMediator> Mediator { get; set; }
    public AccountProviderLegalEntitiesController AccountProviderLegalEntitiesController { get; set; }
    public GetAccountProviderLegalEntitiesWithPermissionQueryResult Result { get; set; }

    public GetTestsFixture()
    {
        Mediator = new Mock<IMediator>();

        GetAccountProviderLegalEntitiesRouteValues = new GetAccountProviderLegalEntitiesRouteValues();

        Result = new GetAccountProviderLegalEntitiesWithPermissionQueryResult(new[] {
            new AccountProviderLegalEntityDto {
                AccountId = 41L,
                AccountHashedId = "TRE567",
                AccountLegalEntityId = 4131L, 
                AccountLegalEntityName = "AccountLegalEntityName",
                AccountLegalEntityPublicHashedId = "ALEPHI", 
                AccountName = "AccountName", 
                AccountProviderId = 491L,
                AccountPublicHashedId = "ACCPHI"
            }
        });

        Mediator.Setup(m =>
                m.Send(
                    It.Is<GetAccountProviderLegalEntitiesWithPermissionQuery>(q =>
                        q.Ukprn == GetAccountProviderLegalEntitiesRouteValues.Ukprn &&
                        q.Operations.Contains(Operation.CreateCohort) &&
                        q.AccountHashedId == GetAccountProviderLegalEntitiesRouteValues.AccountHashedId &&
                        q.AccountLegalEntityPublicHashedId == GetAccountProviderLegalEntitiesRouteValues.AccountLegalEntityPublicHashedId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result);

        AccountProviderLegalEntitiesController = new AccountProviderLegalEntitiesController(Mediator.Object);
    }

    public async Task<IActionResult> CallGet()
    {
        return await AccountProviderLegalEntitiesController.Get(GetAccountProviderLegalEntitiesRouteValues, CancellationToken.None);
    }

    public GetTestsFixture SetUkprn(long? ukprn)
    {
        GetAccountProviderLegalEntitiesRouteValues.Ukprn = ukprn;
        return this;
    }

    public GetTestsFixture SetAccountHashedId(string accountHashedId)
    {
        GetAccountProviderLegalEntitiesRouteValues.AccountHashedId = accountHashedId;
        return this;
    }

    public GetTestsFixture SetAccountLegalEntityPublicHashedId(string accountLegalEntityPublicHashedId)
    {
        GetAccountProviderLegalEntitiesRouteValues.AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
        return this;
    }

    public GetTestsFixture SetOperation(Operation? operation)
    {
        GetAccountProviderLegalEntitiesRouteValues.Operation = operation;
        return this;
    }

    public GetTestsFixture SetOperations(List<Operation> operations)
    {
        GetAccountProviderLegalEntitiesRouteValues.Operations = operations;
        return this;
    }

}