using System;
using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class AccountProvidersControllerTests : FluentTest<AccountProvidersControllerTestsFixture>
    {
        [Test]
        public Task Index_WhenGettingIndexAction_ThenShouldReturnIndexView()
        {
            return RunAsync(f => f.Index(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");

                var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<AccountProvidersViewModel>().Which;

                model.AccountProviders.Should().BeEquivalentTo(f.GetAccountProvidersQueryResult.AccountProviders);
                model.AccountLegalEntitiesCount.Should().Be(f.GetAccountProvidersQueryResult.AccountLegalEntitiesCount);
            });
        }
        
        [Test]
        public void Find_WhenGettingFindAction_ThenShouldReturnFindView()
        {
            Run(f => f.Find(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<FindProviderViewModel>();
            });
        }

        [Test]
        public Task Find_WhenPostingFindActionAndProviderExists_ThenShouldRedirectToAddAction()
        {
            return RunAsync(f => f.PostFind(true), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Add") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["Ukprn"].Equals(f.FindProvidersQueryResult.Ukprn)));
        }

        [Test]
        public Task Find_WhenPostingFindActionAndProviderDoesNotExist_ThenShouldAddModelError()
        {
            return RunAsync(f => f.PostFind(), f => f.AccountProvidersController.ModelState.ContainsKey(nameof(FindProviderViewModel.Ukprn)).Should().BeTrue());
        }

        [Test]
        public Task Find_WhenPostingFindActionAndProviderDoesNotExist_ThenShouldRedirectToFindAction()
        {
            return RunAsync(f => f.PostFind(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Find") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public Task Find_WhenPostingFindActionAndProviderAlreadyAdded_ThenShouldRedirectToAlreadyAddedAction()
        {
            return RunAsync(f => f.PostFind(true, true), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("AlreadyAdded") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderId"].Equals(f.FindProvidersQueryResult.AccountProviderId)));
        }

        [Test]
        public Task Add_WhenGettingAddAction_ThenShouldReturnAddView()
        {
            return RunAsync(f => f.Add(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddAccountProviderViewModel>(m => m.Provider == f.GetProviderToAddQueryResult.Provider);
            });
        }

        [Test]
        public Task Add_WhenPostingAddActionAndConfirmOptionIsSelected_ThenShouldSendAddAccountProviderCommand()
        {
            return RunAsync(f => f.PostAdd("Confirm"), f => f.Mediator.Verify(m => m.Send(
                It.Is<AddAccountProviderCommand>(c => 
                    c.AccountId == f.AddAccountProviderViewModel.AccountId &&
                    c.UserRef == f.AddAccountProviderViewModel.UserRef &&
                    c.Ukprn == f.AddAccountProviderViewModel.Ukprn),
                CancellationToken.None), Times.Once));
        }

        [Test]
        public Task Add_WhenPostingAddActionAndConfirmOptionIsSelected_ThenShouldRedirectToAddedAction()
        {
            return RunAsync(f => f.PostAdd("Confirm"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Added") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderId"].Equals(f.AccountProviderId)));
        }

        [Test]
        public Task Add_WhenPostingAddActionAndReEnterUkprnOptionIsSelected_ThenShouldNotAddAccountProvider()
        {
            return RunAsync(f => f.PostAdd("ReEnterUkprn"), f => f.Mediator.Verify(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None), Times.Never));
        }

        [Test]
        public Task Add_WhenPostingAddActionAndReEnterUkprnOptionIsSelected_ThenShouldRedirectToFindAction()
        {
            return RunAsync(f => f.PostAdd("ReEnterUkprn"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Find") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public Task Add_WhenPostingAddActionAndNoOptionIsSelected_ThenShouldThrowException()
        {
            return RunAsync(f => f.PostAdd(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }

        [Test]
        public Task Added_WhenGettingAddedAction_ThenShouldReturnAddedView()
        {
            return RunAsync(f => f.Added(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddedAccountProviderViewModel>(m => m.AccountProvider == f.GetAddedAccountProviderQueryResult.AccountProvider);
            });
        }

        [Test]
        public void Added_WhenPostingAddedActionAndSetPermissionsOptionIsSelected_ThenShouldRedirectToPermissionsIndexAction()
        {
            Run(f => f.PostAdded("SetPermissions"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderId"].Equals(f.AddedAccountProviderViewModel.AccountProviderId)));
        }

        [Test]
        public void Added_WhenPostingAddedActionAndAddTrainingProviderOptionIsSelected_ThenShouldRedirectToFindAction()
        {
            Run(f => f.PostAdded("AddTrainingProvider"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Find") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public void Added_WhenPostingAddedActionAndGoToHomepageOptionIsSelected_ThenShouldRedirectToHomeUrl()
        {
            Run(f => f.PostAdded("GoToHomepage"), (f, r) => r.Should().NotBeNull().And.Match<RedirectResult>(a =>
                a.Url == $"https://localhost/accounts/ABC123/teams"));
        }

        [Test]
        public void Added_WhenPostingAddedActionAndNoOptionIsSelected_ThenShouldThrowException()
        {
            Run(f => f.PostAdded(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }

        [Test]
        public Task AlreadyAdded_WhenGettingAlreadyAddedAction_ThenShouldReturnAlreadyAddedView()
        {
            return RunAsync(f => f.AlreadyAdded(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AlreadyAddedAccountProviderViewModel>(m => m.AccountProvider == f.GetAddedAccountProviderQueryResult.AccountProvider);
            });
        }

        [Test]
        public void AlreadyAdded_WhenPostingAlreadyAddedActionAndSetPermissionsOptionIsSelected_ThenShouldRedirectToPermissionsIndexAction()
        {
            Run(f => f.PostAlreadyAdded("SetPermissions"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderId"].Equals(f.AlreadyAddedAccountProviderViewModel.AccountProviderId)));
        }

        [Test]
        public void AlreadyAdded_WhenPostingAlreadyAddedActionAndAddTrainingProviderOptionIsSelected_ThenShouldRedirectToFindAction()
        {
            Run(f => f.PostAlreadyAdded("AddTrainingProvider"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Find") &&
                a.RouteValues["Controller"] == null));
        }

        [Test]
        public void AlreadyAdded_WhenPostingAlreadyAddedActionAndNoOptionIsSelected_ThenShouldThrowException()
        {
            Run(f => f.PostAlreadyAdded(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }

        [Test]
        public Task Get_WhenAccountHasMultipleAccountLegalEntities_ThenShouldReturnGetView()
        {
            return RunAsync(f => f.Get(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");

                var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<GetAccountProviderViewModel>().Which;

                model.AccountProvider.Should().BeSameAs(f.GetAccountProviderQueryResult.AccountProvider);
                model.AccountLegalEntities.Should().BeEquivalentTo(f.GetAccountProviderQueryResult.AccountLegalEntities);
            });
        }

        [Test]
        public Task Get_WhenAccountHasSingleAccountLegalEntity_ThenShouldRedirectToAccountProviderLegalEntitiesGetAction()
        {
            return RunAsync(f => f.Get(1), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["Controller"].Equals("AccountProviderLegalEntities") &&
                a.RouteValues["AccountLegalEntityId"].Equals(f.GetAccountProviderQueryResult.AccountLegalEntities[0].Id) &&
                a.RouteValues["AccountProviderId"].Equals(f.GetAccountProviderQueryResult.AccountProvider.Id)));
        }
    }

    public class AccountProvidersControllerTestsFixture
    {
        public AccountProvidersController AccountProvidersController { get; set; }
        public FindProviderViewModel FindProviderViewModel { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        public AccountProvidersRouteValues AccountProvidersRouteValues { get; set; }
        public GetAccountProvidersQueryResult GetAccountProvidersQueryResult { get; set; }
        public FindProviderToAddQueryResult FindProvidersQueryResult { get; set; }
        public AddAccountProviderRouteValues AddAccountProviderRouteValues { get; set; }
        public GetProviderToAddQueryResult GetProviderToAddQueryResult { get; set; }
        public AddAccountProviderViewModel AddAccountProviderViewModel { get; set; }
        public long AccountProviderId { get; set; }
        public GetAddedAccountProviderQueryResult GetAddedAccountProviderQueryResult { get; set; }
        public AddedAccountProviderRouteValues AddedAccountProviderRouteValues { get; set; }
        public AddedAccountProviderViewModel AddedAccountProviderViewModel { get; set; }
        public AlreadyAddedAccountProviderRouteValues AlreadyAddedAccountProviderRouteValues { get; set; }
        public AlreadyAddedAccountProviderViewModel AlreadyAddedAccountProviderViewModel { get; set; }
        public GetAccountProviderRouteValues GetAccountProviderRouteValues { get; set; }
        public GetAccountProviderQueryResult GetAccountProviderQueryResult { get; set; }
        
        public AccountProvidersControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfile(typeof(AccountProviderMappings))).CreateMapper();
            EmployerUrls = new Mock<IEmployerUrls>();
            AccountProvidersController = new AccountProvidersController(Mediator.Object, Mapper, EmployerUrls.Object);
        }

        public Task<ActionResult> Index()
        {
            AccountProvidersRouteValues = new AccountProvidersRouteValues
            {
                AccountId = 1
            };
            
            GetAccountProvidersQueryResult = new GetAccountProvidersQueryResult(
                new List<AccountProviderSummaryDto>
                {
                    new AccountProviderSummaryDto
                    {
                        Id = 2,
                        ProviderName = "Foo"
                    }
                },
                2);

            Mediator.Setup(m => m.Send(It.Is<GetAccountProvidersQuery>(q => q.AccountId == AccountProvidersRouteValues.AccountId), CancellationToken.None))
                .ReturnsAsync(GetAccountProvidersQueryResult);
            
            return AccountProvidersController.Index(AccountProvidersRouteValues);
        }

        public ActionResult Find()
        {
            return AccountProvidersController.Find();
        }

        public Task<ActionResult> PostFind(bool providerExists = false, bool providerAlreadyAdded = false)
        {
            var accountId = 1;
            var ukprn = 12345678;
            var accountProviderId = 2;
            
            FindProviderViewModel = new FindProviderViewModel
            {
                AccountId = accountId,
                Ukprn = ukprn.ToString()
            };

            FindProvidersQueryResult = new FindProviderToAddQueryResult(providerExists ? ukprn : (long?)null, providerAlreadyAdded ? accountProviderId : (int?)null);

            Mediator.Setup(m => m.Send(It.Is<FindProviderToAddQuery>(q => q.AccountId == accountId && q.Ukprn == ukprn), CancellationToken.None)).ReturnsAsync(FindProvidersQueryResult);

            return AccountProvidersController.Find(FindProviderViewModel);
        }

        public Task<ActionResult> Add()
        {
            AddAccountProviderRouteValues = new AddAccountProviderRouteValues
            {
                Ukprn = 12345678
            };
            
            GetProviderToAddQueryResult = new GetProviderToAddQueryResult(new ProviderBasicDto
            {
                Ukprn = 12345678,
                Name = "Foo"
            });

            Mediator.Setup(m => m.Send(It.Is<GetProviderToAddQuery>(q => q.Ukprn == AddAccountProviderRouteValues.Ukprn), CancellationToken.None)).ReturnsAsync(GetProviderToAddQueryResult);

            return AccountProvidersController.Add(AddAccountProviderRouteValues);
        }

        public Task<ActionResult> PostAdd(string choice = null)
        {
            AddAccountProviderViewModel = new AddAccountProviderViewModel
            {
                AccountId = 1,
                UserRef = Guid.NewGuid(),
                Ukprn = 12345678,
                Choice = choice
            };

            AccountProviderId = 2;

            Mediator.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderId);
            
            return AccountProvidersController.Add(AddAccountProviderViewModel);
        }

        public Task<ActionResult> Added()
        {
            AddedAccountProviderRouteValues = new AddedAccountProviderRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2
            };
            
            GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new AccountProviderBasicDto
            {
                Id = 2,
                ProviderUkprn = 12345678,
                ProviderName = "Foo"
            });

            Mediator.Setup(m => m.Send(It.Is<GetAddedAccountProviderQuery>(q => q.AccountId == AddedAccountProviderRouteValues.AccountId && q.AccountProviderId == AddedAccountProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAddedAccountProviderQueryResult);
            
            return AccountProvidersController.Added(AddedAccountProviderRouteValues);
        }

        public ActionResult PostAdded(string choice = null)
        {
            AddedAccountProviderViewModel = new AddedAccountProviderViewModel
            {
                AccountProviderId = 2,
                Choice = choice
            };

            EmployerUrls.Setup(eu => eu.Account(null))
                .Returns($"https://localhost/accounts/ABC123/teams");

            return AccountProvidersController.Added(AddedAccountProviderViewModel);
        }

        public Task<ActionResult> AlreadyAdded()
        {
            AlreadyAddedAccountProviderRouteValues = new AlreadyAddedAccountProviderRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2
            };
            
            GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new AccountProviderBasicDto
            {
                Id = 2,
                ProviderUkprn = 12345678,
                ProviderName = "Foo"
            });

            Mediator.Setup(m => m.Send(It.Is<GetAddedAccountProviderQuery>(q => q.AccountId == AlreadyAddedAccountProviderRouteValues.AccountId && q.AccountProviderId == AlreadyAddedAccountProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAddedAccountProviderQueryResult);
            
            return AccountProvidersController.AlreadyAdded(AlreadyAddedAccountProviderRouteValues);
        }

        public ActionResult PostAlreadyAdded(string choice = null)
        {
            AlreadyAddedAccountProviderViewModel = new AlreadyAddedAccountProviderViewModel
            {
                AccountProviderId = 2,
                Choice = choice
            };
            
            return AccountProvidersController.AlreadyAdded(AlreadyAddedAccountProviderViewModel);
        }

        public Task<ActionResult> Get(int? accountLegalEntitiesCount = null)
        {
            GetAccountProviderRouteValues = new GetAccountProviderRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2
            };
            
            GetAccountProviderQueryResult = new GetAccountProviderQueryResult(
                new AccountProviderDto
                {
                    Id = 2,
                    ProviderName = "Foo"
                },
                Enumerable.Range(3, accountLegalEntitiesCount ?? 2)
                    .Select(i => new AccountLegalEntityBasicDto
                    {
                        Id = i,
                        Name = i.ToString()
                    })
                    .ToList());
            
            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderQuery>(q => q.AccountId == GetAccountProviderRouteValues.AccountId && q.AccountProviderId == GetAccountProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAccountProviderQueryResult);
            
            return AccountProvidersController.Get(GetAccountProviderRouteValues);
        }
    }
}