using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
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
        public Task AddProvider_WhenPostingTheAddActionAndTheConfirmOptionIsSelected_ThenShouldRedirectToTheAddedAction()
        {
            return RunAsync(f => f.PostAdd("Confirm"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a => 
                a.RouteValues["Action"].Equals("Added") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["accountProviderId"].Equals(f.AccountProviderId)));
        }

        [Test]
        public Task AddProvider_WhenPostingTheAddActionAndTheReEnterUkprnOptionWasSelected_ThenShouldRedirectToTheSearchAction()
        {
            return RunAsync(f => f.PostAdd("ReEnterUkprn"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Search") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public Task AddProvider_WhenPostingTheAddActionAndNoOptionWasSelected_ThenShouldThrowException()
        {
            return RunAsync(f => f.PostAdd(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
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
        public int AccountProviderId { get; set; }

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

        public Task<ActionResult> PostAdd(string choice = null)
        {
            AddViewModel = new AddProviderViewModel
            {
                AddAccountProviderCommand = new AddAccountProviderCommand
                {
                    AccountId = 1,
                    Ukprn = 12345678
                },
                Choice = choice
            };

            AccountProviderId = 12;

            Mediator.Setup(m => m.Send(AddViewModel.AddAccountProviderCommand, CancellationToken.None)).ReturnsAsync(AccountProviderId);
            
            return ProvidersController.Add(AddViewModel);
        }
    }
}