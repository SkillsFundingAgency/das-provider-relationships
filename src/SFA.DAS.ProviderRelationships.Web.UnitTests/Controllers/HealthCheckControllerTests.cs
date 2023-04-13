using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Commands.RunHealthCheck;
using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck;
using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck.Dtos;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.RouteValues.HealthCheck;
using SFA.DAS.ProviderRelationships.Web.ViewModels.HealthCheck;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class HealthCheckControllerTests : FluentTest<HealthCheckControllerTestsFixture>
    {
        [Test]
        public Task Index_WhenGettingTheIndexAction_ThenShouldReturnTheIndexView()
        {
            return TestAsync(
                f => f.Index(),
                (f, r) =>
                {
                    r.As<ViewResult>().Model.Should().NotBeNull().And
                        .Match<HealthCheckViewModel>(m => m.HealthCheck == f.GetHealthCheckQueryResult.HealthCheck);
                });
        }

        [Test]
        public Task Index_WhenPostingTheIndexAction_ThenShouldRedirectToTheIndexAction()
        {
            return TestAsync(
                f => f.PostIndex(), 
                (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals("Index") &&
                a.ControllerName == null));
        }
    }

    public class HealthCheckControllerTestsFixture
    {
        public HealthCheckController HealthCheckController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public GetHealthCheckQueryResult GetHealthCheckQueryResult { get; set; }
        public HealthCheckRouteValues HealthCheckRouteValues { get; set; }

        public HealthCheckControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddMaps(new [] {
                typeof(HealthCheckViewModel),
                typeof(HealthCheck)
            })).CreateMapper();
            HealthCheckController = new HealthCheckController(Mediator.Object, Mapper);
        }

        public Task<IActionResult> Index()
        {
            GetHealthCheckQueryResult = new GetHealthCheckQueryResult(new HealthCheckDto());

            Mediator
                .Setup(m => m.Send(It.IsAny<GetHealthCheckQuery>(), CancellationToken.None))
                .ReturnsAsync(GetHealthCheckQueryResult);

            return HealthCheckController.Index();
        }

        public Task<IActionResult> PostIndex()
        {
            HealthCheckRouteValues = new HealthCheckRouteValues { UserRef = Guid.NewGuid() };
            
            Mediator.Setup(m => m.Send(It.Is<RunHealthCheckCommand>(c => c.UserRef == HealthCheckRouteValues.UserRef), CancellationToken.None));

            return HealthCheckController.Index(HealthCheckRouteValues);
        }
    }
}