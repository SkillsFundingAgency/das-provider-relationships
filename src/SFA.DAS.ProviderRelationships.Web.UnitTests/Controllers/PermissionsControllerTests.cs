using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.ViewModels;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class PermissionsControllerTests : FluentTest<PermissionsControllerTestsFixture>
    {
        [Test]
        public void Search_WhenGettingTheSearchProvidersAction_ThenShouldReturnTheSearchProvidersView()
        {
            Run(f => f.Search(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<SearchProvidersViewModel>();
            });
        }

        [Test]
        public Task Search_WhenPostingTheSearchProvidersAction_ThenShouldRedirectToTheAddProviderAction()
        {
            return RunAsync(f => f.PostSearch(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("AddProvider") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["ukprn"].Equals(f.SearchProvidersQueryResponse.Ukprn)));
        }

        [Test]
        public Task AddProvider_WhenGettingTheAddProviderAction_ThenShouldReturnTheAddProviderView()
        {
            return RunAsync(f => f.Add(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddProviderViewModel>(m => m.Provider == f.GetProviderQueryResponse.Provider);
            });
        }

        [Test]
        public void AddProvider_WhenPostingTheAddProviderActionAndTheConfirmOptionIsSelected_ThenShouldRedirectToTheHomeAction()
        {
            Run(f => f.PostAdd("Confirm"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a => 
                a.RouteValues["Action"].Equals("Index") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["ukprn"].Equals(f.AddProviderViewModel.Ukprn)));
        }

        [Test]
        public void AddProvider_WhenPostingTheAddProviderActionAndTheReEnterUkprnOptionWasSelected_ThenShouldRedirectToTheSearchAction()
        {
            Run(f => f.PostAdd("ReEnterUkprn"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("SearchProviders") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public void AddProvider_WhenPostingTheAddProviderActionAndNoOptionWasSelected_ThenShouldThrowException()
        {
            Run(f => f.PostAdd(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }
    }

    public class PermissionsControllerTestsFixture
    {
        public PermissionsController PermissionsController { get; set; }
        public SearchProvidersViewModel SearchProvidersViewModel { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public SearchProvidersQueryResponse SearchProvidersQueryResponse { get; set; }
        public GetProviderQuery GetProviderQuery { get; set; }
        public GetProviderQueryResponse GetProviderQueryResponse { get; set; }
        public AddProviderViewModel AddProviderViewModel { get; set; }

        public PermissionsControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfile<ProviderMappings>()).CreateMapper();
            PermissionsController = new PermissionsController(Mediator.Object, Mapper);
        }

        public ActionResult Search()
        {
            return PermissionsController.SearchProviders();
        }

        public Task<ActionResult> PostSearch()
        {
            SearchProvidersViewModel = new SearchProvidersViewModel
            {
                SearchProvidersQuery = new SearchProvidersQuery
                {
                    Ukprn = "12345678"
                }
            };

            SearchProvidersQueryResponse = new SearchProvidersQueryResponse
            {
                Ukprn = 12345678
            };

            Mediator.Setup(m => m.Send(SearchProvidersViewModel.SearchProvidersQuery, CancellationToken.None)).ReturnsAsync(SearchProvidersQueryResponse);

            return PermissionsController.SearchProviders(SearchProvidersViewModel);
        }

        public Task<ActionResult> Add()
        {
            GetProviderQuery = new GetProviderQuery
            {
                Ukprn = "12345678"
            };

            GetProviderQueryResponse = new GetProviderQueryResponse
            {
                Provider = new ProviderDto
                {
                    Ukprn = 12345678,
                    Name = "Foo"
                }
            };

            Mediator.Setup(m => m.Send(GetProviderQuery, CancellationToken.None)).ReturnsAsync(GetProviderQueryResponse);

            return PermissionsController.AddProvider(GetProviderQuery);
        }

        public ActionResult PostAdd(string choice = null)
        {
            AddProviderViewModel = new AddProviderViewModel
            {
                Choice = choice,
                Ukprn = "12345678"
            };

            return PermissionsController.AddProvider(AddProviderViewModel);
        }
    }
}