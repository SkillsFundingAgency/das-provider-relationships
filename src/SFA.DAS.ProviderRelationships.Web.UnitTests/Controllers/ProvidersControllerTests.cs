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
        public Task Add_WhenGettingTheAddAction_ThenShouldReturnTheAddView()
        {
            return RunAsync(f => f.Add(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddProviderViewModel>(m => m.Provider == f.GetProviderQueryResponse.Provider);
            });
        }

        [Test]
        public Task Add_WhenPostingTheAddActionAndTheConfirmOptionIsSelected_ThenShouldAddAccountProvider()
        {
            return RunAsync(f => f.PostAdd("Confirm"), f => f.Mediator.Verify(m => m.Send(
                It.Is<AddAccountProviderCommand>(c => 
                    c.AccountId == f.AddProviderViewModel.AccountId &&
                    c.UserRef == f.AddProviderViewModel.UserRef &&
                    c.Ukprn == f.AddProviderViewModel.Ukprn),
                CancellationToken.None), Times.Once));
        }

        [Test]
        public Task Add_WhenPostingTheAddActionAndTheConfirmOptionIsSelected_ThenShouldRedirectToTheAddedAction()
        {
            return RunAsync(f => f.PostAdd("Confirm"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Added") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["accountProviderId"].Equals(f.AccountProviderId)));
        }

        [Test]
        public Task Add_WhenPostingTheAddActionAndTheReEnterUkprnOptionIsSelected_ThenShouldNotAddAccountProvider()
        {
            return RunAsync(f => f.PostAdd("ReEnterUkprn"), f => f.Mediator.Verify(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None), Times.Never));
        }

        [Test]
        public Task Add_WhenPostingTheAddActionAndTheReEnterUkprnOptionIsSelected_ThenShouldRedirectToTheSearchAction()
        {
            return RunAsync(f => f.PostAdd("ReEnterUkprn"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Search") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public Task Add_WhenPostingTheAddActionAndNoOptionIsSelected_ThenShouldThrowException()
        {
            return RunAsync(f => f.PostAdd(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }

        [Test]
        public Task Added_WhenGettingTheAddedAction_ThenShouldReturnTheAddedView()
        {
            return RunAsync(f => f.Added(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddedProviderViewModel>(m => m.AccountProvider == f.GetAddedProviderQueryResponse.AccountProvider);
            });
        }
    }

    public class ProvidersControllerTestsFixture
    {
        public ProvidersController ProvidersController { get; set; }
        public SearchProvidersViewModel SearchViewModel { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public SearchProvidersQueryResponse SearchProvidersQueryResponse { get; set; }
        public AddProviderParameters AddProviderParameters { get; set; }
        public GetProviderQueryResponse GetProviderQueryResponse { get; set; }
        public AddProviderViewModel AddProviderViewModel { get; set; }
        public int AccountProviderId { get; set; }
        public GetAddedProviderQueryResponse GetAddedProviderQueryResponse { get; set; }
        public AddedProviderParameters AddedProviderParameters { get; set; }

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
                Ukprn = "12345678"
            };

            SearchProvidersQueryResponse = new SearchProvidersQueryResponse
            {
                Ukprn = 12345678
            };

            Mediator.Setup(m => m.Send(It.Is<SearchProvidersQuery>(q => q.Ukprn == SearchViewModel.Ukprn), CancellationToken.None)).ReturnsAsync(SearchProvidersQueryResponse);

            return ProvidersController.Search(SearchViewModel);
        }

        public Task<ActionResult> Add()
        {
            AddProviderParameters = new AddProviderParameters
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

            Mediator.Setup(m => m.Send(It.Is<GetProviderQuery>(q => q.Ukprn == AddProviderParameters.Ukprn), CancellationToken.None)).ReturnsAsync(GetProviderQueryResponse);

            return ProvidersController.Add(AddProviderParameters);
        }

        public Task<ActionResult> PostAdd(string choice = null)
        {
            AddProviderViewModel = new AddProviderViewModel
            {
                AccountId = 1,
                UserRef = Guid.NewGuid(),
                Ukprn = 12345678,
                Choice = choice
            };

            AccountProviderId = 12;

            Mediator.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderId);
            
            return ProvidersController.Add(AddProviderViewModel);
        }

        public Task<ActionResult> Added()
        {
            AddedProviderParameters = new AddedProviderParameters
            {
                AccountId = 1,
                AccountProviderId = 12
            };
            
            GetAddedProviderQueryResponse = new GetAddedProviderQueryResponse
            {
                AccountProvider = new AccountProviderDto
                {
                    Id = 12,
                    Provider = new ProviderDto
                    {
                        Ukprn = 12345678,
                        Name = "Foo"
                    }
                }
            };

            Mediator.Setup(m => m.Send(It.Is<GetAddedProviderQuery>(q => q.AccountId == AddedProviderParameters.AccountId && q.AccountProviderId == AddedProviderParameters.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAddedProviderQueryResponse);
            
            return ProvidersController.Added(AddedProviderParameters);
        }
    }
}