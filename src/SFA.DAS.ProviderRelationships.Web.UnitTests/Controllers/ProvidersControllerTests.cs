using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Providers;
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
        public Task Search_WhenPostingTheSearchActionAndProviderExists_ThenShouldRedirectToTheAddAction()
        {
            return RunAsync(f => f.PostSearch(true), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Add") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["Ukprn"].Equals(f.SearchProvidersQueryResult.Ukprn)));
        }

        [Test]
        public Task Search_WhenPostingTheSearchActionAndProviderDoesNotExist_ThenShouldAddModelError()
        {
            return RunAsync(f => f.PostSearch(), f => f.ProvidersController.ModelState.ContainsKey(nameof(SearchProvidersViewModel.Ukprn)).Should().BeTrue());
        }

        [Test]
        public Task Search_WhenPostingTheSearchActionAndProviderDoesNotExist_ThenShouldRedirectToTheSearchAction()
        {
            return RunAsync(f => f.PostSearch(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Search") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public Task Search_WhenPostingTheSearchActionAndProviderAlreadyAdded_ThenShouldRedirectToTheAlreadyAddedAction()
        {
            return RunAsync(f => f.PostSearch(true, true), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("AlreadyAdded") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderId"].Equals(f.SearchProvidersQueryResult.AccountProviderId)));
        }

        [Test]
        public Task Add_WhenGettingTheAddAction_ThenShouldReturnTheAddView()
        {
            return RunAsync(f => f.Add(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddProviderViewModel>(m => m.Provider == f.GetProviderQueryResult.Provider);
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
                a.RouteValues["AccountProviderId"].Equals(f.AccountProviderId)));
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
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddedProviderViewModel>(m => m.AccountProvider == f.GetAddedAccountProviderQueryResult.AccountProvider);
            });
        }

        [Test]
        public void Added_WhenPostingTheAddedActionAndTheSetPermissionsOptionIsSelected_ThenShouldRedirectToThePermissionsIndexAction()
        {
            Run(f => f.PostAdded("SetPermissions"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Index") &&
                a.RouteValues["Controller"].Equals("Permissions") &&
                a.RouteValues["AccountProviderId"].Equals(f.AddedProviderViewModel.AccountProviderId)));
        }

        [Test]
        public void Added_WhenPostingTheAddedActionAndTheAddTrainingProviderOptionIsSelected_ThenShouldRedirectToTheSearchAction()
        {
            Run(f => f.PostAdded("AddTrainingProvider"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Search") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public void Added_WhenPostingTheAddedActionAndTheGoToHomepageOptionIsSelected_ThenShouldRedirectToTheHomeIndexAction()
        {
            Run(f => f.PostAdded("GoToHomepage"), (f, r) => r.Should().NotBeNull().And.Match<RedirectResult>(a =>
                a.Url == $"https://localhost/accounts/ABC123/teams"));
        }

        [Test]
        public void Added_WhenPostingTheAddedActionAndNoOptionIsSelected_ThenShouldThrowException()
        {
            Run(f => f.PostAdded(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }

        [Test]
        public Task AlreadyAdded_WhenGettingTheAlreadyAddedAction_ThenShouldReturnTheAlreadyAddedView()
        {
            return RunAsync(f => f.AlreadyAdded(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AlreadyAddedProviderViewModel>(m => m.AccountProvider == f.GetAddedAccountProviderQueryResult.AccountProvider);
            });
        }

        [Test]
        public void AlreadyAdded_WhenPostingTheAlreadyAddedActionAndTheSetPermissionsOptionIsSelected_ThenShouldRedirectToThePermissionsIndexAction()
        {
            Run(f => f.PostAlreadyAdded("SetPermissions"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Index") &&
                a.RouteValues["Controller"].Equals("Permissions") &&
                a.RouteValues["AccountProviderId"].Equals(f.AlreadyAddedProviderViewModel.AccountProviderId)));
        }

        [Test]
        public void AlreadyAdded_WhenPostingTheAlreadyAddedActionAndTheAddTrainingProviderOptionIsSelected_ThenShouldRedirectToTheSearchAction()
        {
            Run(f => f.PostAlreadyAdded("AddTrainingProvider"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Search") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public void AlreadyAdded_WhenPostingTheAlreadyAddedActionAndNoOptionIsSelected_ThenShouldThrowException()
        {
            Run(f => f.PostAlreadyAdded(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }
    }

    public class ProvidersControllerTestsFixture
    {
        public ProvidersController ProvidersController { get; set; }
        public SearchProvidersViewModel SearchViewModel { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        public RequestContext RequestContext { get; set; }
        public SearchProvidersQueryResult SearchProvidersQueryResult { get; set; }
        public AddProviderRouteValues AddProviderRouteValues { get; set; }
        public GetProviderQueryResult GetProviderQueryResult { get; set; }
        public AddProviderViewModel AddProviderViewModel { get; set; }
        public int AccountProviderId { get; set; }
        public GetAddedAccountProviderQueryResult GetAddedAccountProviderQueryResult { get; set; }
        public AddedProviderRouteValues AddedProviderRouteValues { get; set; }
        public AddedProviderViewModel AddedProviderViewModel { get; set; }
        public AlreadyAddedProviderRouteValues AlreadyAddedProviderRouteValues { get; set; }
        public AlreadyAddedProviderViewModel AlreadyAddedProviderViewModel { get; set; }
        
        public ProvidersControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfile<ProviderMappings>()).CreateMapper();
            EmployerUrls = new Mock<IEmployerUrls>();
            RequestContext = new RequestContext();

            ProvidersController = new ProvidersController(Mediator.Object, Mapper, EmployerUrls.Object);
        }

        public ActionResult Search()
        {
            return ProvidersController.Search();
        }

        public Task<ActionResult> PostSearch(bool providerExists = false, bool providerAlreadyAdded = false)
        {
            var accountId = 1;
            var ukprn = 12345678;
            var accountProviderId = 2;
            
            SearchViewModel = new SearchProvidersViewModel
            {
                AccountId = accountId,
                Ukprn = ukprn.ToString()
            };

            SearchProvidersQueryResult = new SearchProvidersQueryResult(providerExists ? ukprn : (long?)null, providerAlreadyAdded ? accountProviderId : (int?)null);

            Mediator.Setup(m => m.Send(It.Is<SearchProvidersQuery>(q => q.AccountId == accountId && q.Ukprn == ukprn), CancellationToken.None)).ReturnsAsync(SearchProvidersQueryResult);

            return ProvidersController.Search(SearchViewModel);
        }

        public Task<ActionResult> Add()
        {
            AddProviderRouteValues = new AddProviderRouteValues
            {
                Ukprn = 12345678
            };
            
            GetProviderQueryResult = new GetProviderQueryResult(new ProviderDto
            {
                Ukprn = 12345678,
                Name = "Foo"
            });

            Mediator.Setup(m => m.Send(It.Is<GetProviderQuery>(q => q.Ukprn == AddProviderRouteValues.Ukprn), CancellationToken.None)).ReturnsAsync(GetProviderQueryResult);

            return ProvidersController.Add(AddProviderRouteValues);
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

            AccountProviderId = 2;

            Mediator.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderId);
            
            return ProvidersController.Add(AddProviderViewModel);
        }

        public Task<ActionResult> Added()
        {
            AddedProviderRouteValues = new AddedProviderRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2
            };
            
            GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new AddedAccountProviderDto
            {
                Id = 2,
                Provider = new ProviderDto
                {
                    Ukprn = 12345678,
                    Name = "Foo"
                }
            });

            Mediator.Setup(m => m.Send(It.Is<GetAddedAccountProviderQuery>(q => q.AccountId == AddedProviderRouteValues.AccountId && q.AccountProviderId == AddedProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAddedAccountProviderQueryResult);
            
            return ProvidersController.Added(AddedProviderRouteValues);
        }

        public ActionResult PostAdded(string choice = null)
        {
            AddedProviderViewModel = new AddedProviderViewModel
            {
                AccountProviderId = 2,
                Choice = choice
            };

            EmployerUrls.Setup(au => au.Account(null))
                .Returns($"https://localhost/accounts/ABC123/teams");

            return ProvidersController.Added(AddedProviderViewModel);
        }

        public Task<ActionResult> AlreadyAdded()
        {
            AlreadyAddedProviderRouteValues = new AlreadyAddedProviderRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2
            };
            
            GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new AddedAccountProviderDto
            {
                Id = 2,
                Provider = new ProviderDto
                {
                    Ukprn = 12345678,
                    Name = "Foo"
                }
            });

            Mediator.Setup(m => m.Send(It.Is<GetAddedAccountProviderQuery>(q => q.AccountId == AlreadyAddedProviderRouteValues.AccountId && q.AccountProviderId == AlreadyAddedProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAddedAccountProviderQueryResult);
            
            return ProvidersController.AlreadyAdded(AlreadyAddedProviderRouteValues);
        }

        public ActionResult PostAlreadyAdded(string choice = null)
        {
            AlreadyAddedProviderViewModel = new AlreadyAddedProviderViewModel
            {
                AccountProviderId = 2,
                Choice = choice
            };
            
            return ProvidersController.AlreadyAdded(AlreadyAddedProviderViewModel);
        }
    }
}