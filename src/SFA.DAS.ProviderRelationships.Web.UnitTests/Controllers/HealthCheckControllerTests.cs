using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.RunHealthCheck;

using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck;
using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck.Dtos;
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
            return TestAsync(f => f.Index(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<HealthCheckViewModel>(m => m.HealthCheck == f.GetHealthCheckQueryResult.HealthCheck);
            });
        }

        [Test]
        public Task Index_WhenPostingTheIndexAction_ThenShouldRedirectToTheIndexAction()
        {
            return TestAsync(f => f.PostIndex(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Index") &&
                a.RouteValues["Controller"] == null));
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
            Mapper = new MapperConfiguration(c => {}).CreateMapper();
            HealthCheckController = new HealthCheckController(Mediator.Object, Mapper);
        }

        public Task<ActionResult> Index()
        {
            GetHealthCheckQueryResult = new GetHealthCheckQueryResult(new HealthCheckDto());

            Mediator.Setup(m => m.Send(It.IsAny<GetHealthCheckQuery>(), CancellationToken.None)).ReturnsAsync(GetHealthCheckQueryResult);

            return HealthCheckController.Index();
        }

        public Task<ActionResult> PostIndex()
        {
            HealthCheckRouteValues = new HealthCheckRouteValues { UserRef = Guid.NewGuid() };
            
            Mediator.Setup(m => m.Send(It.Is<RunHealthCheckCommand>(c => c.UserRef == HealthCheckRouteValues.UserRef), CancellationToken.None)).ReturnsAsync(Unit.Value);

            return HealthCheckController.Index(HealthCheckRouteValues);
        }
    }
}