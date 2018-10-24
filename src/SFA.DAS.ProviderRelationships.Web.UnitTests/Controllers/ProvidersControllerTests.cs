using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
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
    public class ProvidersControllerTests : FluentTest<ProvidersControllerTestsFixture>
    {
        [Test]
        public void Search_WhenGettingTheSearchAction_ThenShouldReturnTheSearchView()
        {
            Run(f => f.Search(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<SearchProvidersViewModel>();
            });
        }

        [Test]
        public Task Search_WhenPostingTheSearchAction_ThenShouldRedirectToTheAddAction()
        {
            return RunAsync(f => f.PostSearch(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Add") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["ukprn"].Equals(f.SearchProvidersQueryResponse.Ukprn)));
        }

        [Test]
        public Task AddProvider_WhenGettingTheAddAction_ThenShouldReturnTheAddView()
        {
            return RunAsync(f => f.Add(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddProviderViewModel>(m => m.Provider == f.GetProviderQueryResponse.Provider);
            });
        }

        [Test]
        public void AddProvider_WhenPostingTheAddActionAndTheConfirmOptionIsSelected_ThenShouldRedirectToTheHomeAction()
        {
            Run(f => f.PostAdd("Confirm"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a => 
                a.RouteValues["Action"].Equals("Added") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["ukprn"].Equals(f.AddViewModel.Ukprn)));
        }

        [Test]
        public void AddProvider_WhenPostingTheAddActionAndTheReEnterUkprnOptionWasSelected_ThenShouldRedirectToTheSearchAction()
        {
            Run(f => f.PostAdd("ReEnterUkprn"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Search") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public void AddProvider_WhenPostingTheAddActionAndNoOptionWasSelected_ThenShouldThrowException()
        {
            Run(f => f.PostAdd(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }
    }

    public class ProvidersControllerTestsFixture
    {
        public ProvidersController ProvidersController { get; set; }
        public SearchProvidersViewModel SearchViewModel { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public SearchProvidersQueryResponse SearchProvidersQueryResponse { get; set; }
        public GetProviderQuery GetProviderQuery { get; set; }
        public GetProviderQueryResponse GetProviderQueryResponse { get; set; }
        public AddProviderViewModel AddViewModel { get; set; }

        public ProvidersControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfile<ProviderMappings>()).CreateMapper();
            ProvidersController = new ProvidersController(Mediator.Object, Mapper);
        }

        public ActionResult Search()
        {
            return ProvidersController.Search();
        }

        public Task<ActionResult> PostSearch()
        {
            SearchViewModel = new SearchProvidersViewModel
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

            Mediator.Setup(m => m.Send(SearchViewModel.SearchProvidersQuery, CancellationToken.None)).ReturnsAsync(SearchProvidersQueryResponse);

            return ProvidersController.Search(SearchViewModel);
        }

        public Task<ActionResult> Add()
        {
            GetProviderQuery = new GetProviderQuery
            {
                Ukprn = 12345678
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

            return ProvidersController.Add(GetProviderQuery);
        }

        public ActionResult PostAdd(string choice = null)
        {
            AddViewModel = new AddProviderViewModel
            {
                Choice = choice,
                Ukprn = "12345678"
            };

            return ProvidersController.Add(AddViewModel);
        }
    }
}