using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.ActionParameters.Relationships;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.HttpErrorResult;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Errors;
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
        public Task WhenUkprIsMissing_ThenShouldReturnNotImplemented()
        {
            return RunAsync(f => f.SetUkprn(null), f => f.CallGet(),
                (f, r) => r.Should().Match<ErrorResult>(er => 
                    er.HttpStatusCode == HttpStatusCode.NotImplemented
                    && er.ErrorResponse != null
                    && er.ErrorResponse.ErrorCode == RelationshipsErrorCodes.MissingUkprnFilter));
        }
        
        [Test]
        public Task WhenOperationIsMissing_ThenShouldReturnNotImplemented()
        {
            return RunAsync(f => f.SetOperation(null), f => f.CallGet(),
                (f, r) => r.Should().Match<ErrorResult>(er => 
                    er.HttpStatusCode == HttpStatusCode.NotImplemented
                    && er.ErrorResponse != null
                    && er.ErrorResponse.ErrorCode == RelationshipsErrorCodes.MissingOperationFilter));
        }
        
        [Test]
        public Task WhenUnknownOperationIsSupplied_ThenShouldReturnBadRequest()
        {
            return RunAsync(f => f.SetOperation("SqueezeCohort"), f => f.CallGet(),
                (f, r) => r.Should().Match<ErrorResult>(er => 
                    er.HttpStatusCode == HttpStatusCode.BadRequest
                    && er.ErrorResponse != null
                    && er.ErrorResponse.ErrorCode == RelationshipsErrorCodes.UnknownOperationFilter));
        }
    }

    public class GetTestsFixture
    {
        public GetAccountProviderLegalEntitiesParameters GetAccountProviderLegalEntitiesParameters { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public AccountProviderLegalEntitiesController AccountProviderLegalEntitiesController { get; set; }
        public GetAccountProviderLegalEntitiesWithPermissionQueryResult Result { get; set; }

        public GetTestsFixture()
        {
            GetAccountProviderLegalEntitiesParameters = new GetAccountProviderLegalEntitiesParameters
            {
                Ukprn = 12345678L,
                Operation = "CreateCohort"
            };

            Mediator = new Mock<IMediator>();

            Result = new GetAccountProviderLegalEntitiesWithPermissionQueryResult(new [] {
                new AccountProviderLegalEntityDto {AccountId = 41L, AccountLegalEntityId = 4131L, AccountLegalEntityName = "AccountLegalEntityName", AccountLegalEntityPublicHashedId = "ALEPHI", AccountName = "AccountName", AccountProviderId = 491L, AccountPublicHashedId = "ACCPHI" }
            });
            
            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderLegalEntitiesWithPermissionQuery>(q => q.Ukprn == GetAccountProviderLegalEntitiesParameters.Ukprn.Value && q.Operation == Operation.CreateCohort), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result);
            
            AccountProviderLegalEntitiesController = new AccountProviderLegalEntitiesController(Mediator.Object);
        }

        public async Task<IHttpActionResult> CallGet()
        {
            return await AccountProviderLegalEntitiesController.Get(GetAccountProviderLegalEntitiesParameters); //, CancellationToken.None);
        }

        public GetTestsFixture SetUkprn(long? ukprn)
        {
            GetAccountProviderLegalEntitiesParameters.Ukprn = ukprn;
            return this;
        }
        
        public GetTestsFixture SetOperation(string operation)
        {
            GetAccountProviderLegalEntitiesParameters.Operation = operation;
            return this;
        }
    }
}