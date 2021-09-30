using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;
using SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.AccountProviderLegalEntities
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public Task WhenValidUkprnAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
        {
            return RunAsync(f =>
                {
                    f.SetUkprn(12345678);
                    f.SetOperation(Operation.CreateCohort);
                },
                f => f.CallGet(), 
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>>();
                    ((OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>)r).Content.Should().BeEquivalentTo(f.Result);
                });
        }

        [Test]
        public Task WhenValidAccountHashedIdAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
        {
            return RunAsync(f =>
                {
                    f.SetAccountHashedId("TEST");
                    f.SetOperation(Operation.CreateCohort);
                },
                f => f.CallGet(),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>>();
                    ((OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>)r).Content.Should().BeEquivalentTo(f.Result);
                });
        }

        [Test]
        public Task WhenValidAccountHashedIdAndAccountLegalIdPublicHashedIdAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
        {
            return RunAsync(f =>
                {
                    f.SetAccountHashedId("XYZ123");
                    f.SetAccountLegalEntityPublicHashedId("ABC123");
                    f.SetOperation(Operation.CreateCohort);
                },
                f => f.CallGet(),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>>();
                    ((OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>)r).Content.Should().BeEquivalentTo(f.Result);
                });
        }
        
        [Test]
        public Task WhenValidAccountHashedIdAndAccountLegalIdPublicHashedIdAndOperationsAreSupplied_ThenShouldReturnRelationshipsFromQuery()
        {
            return RunAsync(f =>
                {
                    f.SetAccountHashedId("XYZ123");
                    f.SetAccountLegalEntityPublicHashedId("ABC123");
                    f.SetOperations(new List<Operation>{Operation.CreateCohort});
                },
                f => f.CallGet(),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>>();
                    ((OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>)r).Content.Should().BeEquivalentTo(f.Result);
                });
        }

        [Test]
        public Task WhenUkprnAndAccountHashedIdIsMissing_ThenShouldReturnBadRequest()
        {
            return RunAsync(f =>
                {
                    f.SetOperation(Operation.Recruitment);
                },
                f => f.CallGet(),
                (f, r) =>
                {
                    r.AssertModelError(nameof(GetAccountProviderLegalEntitiesRouteValues.Ukprn), "Currently a Ukprn filter needs to be supplied");
                    r.AssertModelError(nameof(GetAccountProviderLegalEntitiesRouteValues.AccountHashedId), "Currently an AccountHashedId filter needs to be supplied");
                });
        }

        [Test]
        public Task WhenOperationIsMissing_ThenShouldReturnBadRequest()
        {
            return RunAsync(f =>
                {
                    f.SetAccountHashedId("ABC123");
                    f.SetUkprn(12345678);
                },
                f => f.CallGet(),
                (f, r) => r.AssertModelError(nameof(GetAccountProviderLegalEntitiesRouteValues.Operation), "Currently an Operation filter needs to be supplied"));
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

        public async Task<IHttpActionResult> CallGet()
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
}