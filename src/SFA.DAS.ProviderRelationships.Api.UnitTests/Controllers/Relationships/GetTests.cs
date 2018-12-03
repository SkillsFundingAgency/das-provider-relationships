using System.Linq;
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
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.Relationships
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public Task WhenValidUkprnAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
        {
            return RunAsync(f => f.CallGet(), 
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>>();
                    ((OkNegotiatedContentResult<GetAccountProviderLegalEntitiesWithPermissionResponse>)r).Content.Should().BeEquivalentTo(f.Result);
                });
        }

        [Test]
        public Task WhenUkprIsMissing_ThenShouldReturnBadRequest()
        {
            return RunAsync(f => f.SetUkprn(null), f => f.CallGet(),
                (f, r) => f.AssertSingleModelError(r, nameof(GetAccountProviderLegalEntitiesRouteValues.Ukprn), "Currently an Ukprn filter needs to be supplied"));
        }
        
        [Test]
        public Task WhenOperationIsMissing_ThenShouldReturnBadRequest()
        {
            return RunAsync(f => f.SetOperation(null), f => f.CallGet(),
                (f, r) => f.AssertSingleModelError(r, nameof(GetAccountProviderLegalEntitiesRouteValues.Operation), "Currently an Operation filter needs to be supplied"));
        }
        
        [Test]
        public Task WhenUnknownOperationIsSupplied_ThenShouldReturnBadRequest()
        {
            return RunAsync(f => f.SetOperation("SqueezeCohort"), f => f.CallGet(),
                (f, r) => f.AssertSingleModelError(r, nameof(GetAccountProviderLegalEntitiesRouteValues.Operation), "The Operation filter value supplied is not recognised as an Operation"));
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
            GetAccountProviderLegalEntitiesRouteValues = new GetAccountProviderLegalEntitiesRouteValues
            {
                Ukprn = 12345678L,
                Operation = "CreateCohort"
            };

            Mediator = new Mock<IMediator>();

            Result = new GetAccountProviderLegalEntitiesWithPermissionQueryResult(new [] {
                new AccountProviderLegalEntityDto {AccountId = 41L, AccountLegalEntityId = 4131L, AccountLegalEntityName = "AccountLegalEntityName", AccountLegalEntityPublicHashedId = "ALEPHI", AccountName = "AccountName", AccountProviderId = 491L, AccountPublicHashedId = "ACCPHI" }
            });
            
            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderLegalEntitiesWithPermissionQuery>(q => q.Ukprn == GetAccountProviderLegalEntitiesRouteValues.Ukprn.Value && q.Operation == Operation.CreateCohort), It.IsAny<CancellationToken>()))
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
        
        public GetTestsFixture SetOperation(string operation)
        {
            GetAccountProviderLegalEntitiesRouteValues.Operation = operation;
            return this;
        }

        public void AssertSingleModelError(object result, string propertyName, string message)
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<InvalidModelStateResult>();
            var modelStateDictionary = ((InvalidModelStateResult) result).ModelState;
            modelStateDictionary.Count.Should().Be(1);
            modelStateDictionary.Keys.First().Should().Be(propertyName);
            var modelState = modelStateDictionary.Values.First();
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors.First().ErrorMessage.Should().Be(message);
        }
    }
}