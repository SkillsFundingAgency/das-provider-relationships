using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues.Operations;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Operations;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class OperationsControllerTests : FluentTest<OperationsControllerTestsFixture>
    {
        [Test]
        public Task Set_WhenGettingSetForTheFirstTime_ThenShouldReturnExpectedView()
        {
            return RunAsync(f => f.CreateSession(), f => f.Set(Operation.NotSet), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<UpdateOperationViewModel>().Which;

                model.Operation.Should().Be(Operation.CreateCohort);

                model.AccountLegalEntityId.Value.Should().Be(f.GetAccountProviderLegalEntityQueryResult.AccountLegalEntity.Id);
                model.AccountProviderId.Value.Should().Be(f.GetAccountProviderLegalEntityQueryResult.AccountProvider.Id);
                model.AccountLegalEntitiesCount.Should().Be(f.GetAccountProviderLegalEntityQueryResult.AccountLegalEntitiesCount);
            });
        }
    }

    public class OperationsControllerTestsFixture
    {
        public OperationsController OperationsController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public OperationRouteValue OperationRouteValue { get; set; }
        public GetAccountProviderLegalEntityQueryResult GetAccountProviderLegalEntityQueryResult { get; set; }

        public OperationsControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderLegalEntityMappings))).CreateMapper();
            OperationsController = new OperationsController(Mediator.Object, Mapper);
        }

        public OperationsControllerTestsFixture CreateSession()
        {
            var context = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(context.Object, new RouteData());
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            OperationsController.ControllerContext = new ControllerContext(context.Object, new RouteData(), OperationsController);
            OperationsController.Url = new UrlHelper(requestContext);
            return this;
        }

        public Task<ActionResult> Set(Operation operation)
        {
            OperationRouteValue = new OperationRouteValue {
                AccountId = 1,
                AccountProviderId = 2,
                AccountLegalEntityId = 3,
                OperationId = (short)operation
            };

            GetAccountProviderLegalEntityQueryResult = new GetAccountProviderLegalEntityQueryResult(
               new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderDto(),
               new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountLegalEntityDto(),
               new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderLegalEntityDto(),
               0);

            Mediator.Setup(m => m.Send(
                    It.Is<GetAccountProviderLegalEntityQuery>(q =>
                        q.AccountId == OperationRouteValue.AccountId &&
                        q.AccountProviderId == OperationRouteValue.AccountProviderId &&
                        q.AccountLegalEntityId == OperationRouteValue.AccountLegalEntityId),
                    CancellationToken.None))
                .ReturnsAsync(GetAccountProviderLegalEntityQueryResult);

            return OperationsController.Set(OperationRouteValue);
        }

    }
}