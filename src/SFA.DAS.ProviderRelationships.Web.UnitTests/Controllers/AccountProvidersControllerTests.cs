using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.Urls;
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
                model.IsAddProviderOperationAuthorized.Should().Be(f.GetAccountProvidersQueryResult.IsAddProviderOperationAuthorized);
                model.IsUpdatePermissionsOperationAuthorized.Should().Be(f.GetAccountProvidersQueryResult.IsUpdatePermissionsOperationAuthorized);
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
            return RunAsync(f => f.PostAdd(), (f, r) => r.Should().Throw<ValidationException>());
        }

        [Test]
        public Task Add_WhenInvitationIsValid_ThenShouldSendAddAccountProviderCommand()
        {
            return RunAsync(f => f.CreateSession(), f => f.Invitation(), f => f.Mediator.Verify(m => m.Send(
                It.Is<AddAccountProviderCommand>(c =>
                    c.AccountId == f.AddAccountProviderViewModel.AccountId &&
                    c.UserRef == f.AddAccountProviderViewModel.UserRef &&
                    c.Ukprn == f.AddAccountProviderViewModel.Ukprn),
                CancellationToken.None), Times.Once));
        }

        [Test]
        public Task Add_WhenInvitationIsValid_ThenShouldRedirectToAddedAction()
        {
            return RunAsync(f => f.CreateSession(),f => f.Invitation(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderId"].Equals(f.AccountProviderId)));
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
                model.IsUpdatePermissionsOperationAuthorized.Should().Be(f.GetAccountProviderQueryResult.IsUpdatePermissionsOperationAuthorized);
            });
        }

        [Test]
        public Task Get_WhenAccountHasSingleAccountLegalEntity_ThenShouldRedirectToAccountProviderLegalEntitiesGetAction()
        {
            return RunAsync(f => f.Get(1), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["Controller"].Equals("AccountProviderLegalEntities") &&
                a.RouteValues["AccountProviderId"].Equals(f.GetAccountProviderQueryResult.AccountProvider.Id) &&
                a.RouteValues["AccountLegalEntityId"].Equals(f.GetAccountProviderQueryResult.AccountProvider.AccountLegalEntities[0].Id)));
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
        public GetInvitationByIdQueryResult GetInvitationByIdQueryResult { get; set; }
        public AddAccountProviderViewModel AddAccountProviderViewModel { get; set; }
        public long AccountProviderId { get; set; }
        public GetAddedAccountProviderQueryResult GetAddedAccountProviderQueryResult { get; set; }
        public AddedAccountProviderRouteValues AddedAccountProviderRouteValues { get; set; }
        public InvitationAccountProviderRouteValues InvitationAccountProviderRouteValues { get; set; }
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
                new List<Application.Queries.GetAccountProviders.Dtos.AccountProviderDto>
                {
                    new Application.Queries.GetAccountProviders.Dtos.AccountProviderDto
                    {
                        Id = 2,
                        ProviderName = "Foo"
                    }
                },
                2,
                true);

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
            
            GetProviderToAddQueryResult = new GetProviderToAddQueryResult(
                new Application.Queries.GetProviderToAdd.Dtos.ProviderDto
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
                Provider = new Application.Queries.GetProviderToAdd.Dtos.ProviderDto { Name ="ProviderName" ,Ukprn = 10083920 },
                AccountId = 1,
                UserRef = Guid.NewGuid(),
                Ukprn = 12345678,
                Choice = choice
            };

            AccountProviderId = 2;

            Mediator.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderId);

            var contex = new System.ComponentModel.DataAnnotations.ValidationContext(AddAccountProviderViewModel);
            Validator.ValidateObject(AddAccountProviderViewModel, contex);

            return AccountProvidersController.Add(AddAccountProviderViewModel);
        }

        public Task<ActionResult> Added()
        {
            AddedAccountProviderRouteValues = new AddedAccountProviderRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2
            };
            
            GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new Application.Queries.GetAddedAccountProvider.Dtos.AccountProviderDto
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
            
            GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new Application.Queries.GetAddedAccountProvider.Dtos.AccountProviderDto
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
                new Application.Queries.GetAccountProvider.Dtos.AccountProviderDto
                {
                    Id = 2,
                    ProviderName = "Foo",
                    AccountLegalEntities = Enumerable.Range(3, accountLegalEntitiesCount ?? 2)
                        .Select(i => new Application.Queries.GetAccountProvider.Dtos.AccountLegalEntityDto
                        {
                            Id = i,
                            Name = i.ToString(),
                            Operations = new List<Operation>
                            {
                                Operation.CreateCohort
                            }
                        })
                        .ToList()
                },
                true);
            
            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderQuery>(q => q.AccountId == GetAccountProviderRouteValues.AccountId && q.AccountProviderId == GetAccountProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAccountProviderQueryResult);
            
            return AccountProvidersController.Get(GetAccountProviderRouteValues);
        }

        public Task<ActionResult> Invitation()
        {
            var userRef = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var ukprn = 12345678;
            var accountId = 1;

            InvitationAccountProviderRouteValues = new InvitationAccountProviderRouteValues {
                AccountId = accountId,
                CorrelationId =correlationId,
                UserRef =userRef
            };

            GetInvitationByIdQueryResult = new GetInvitationByIdQueryResult(new InvitationDto {
                EmployerEmail = "foo@foo.com",
                EmployerFirstName = "John",
                EmployerLastName = "Smtih",
                EmployerOrganisation = "ESFA",
                Ukprn = ukprn,
                Status = 0
            });

            AddAccountProviderViewModel = new AddAccountProviderViewModel {
                AccountId = accountId,
                UserRef = userRef,
                Ukprn = ukprn
            };

            FindProviderViewModel = new FindProviderViewModel {
                AccountId = accountId,
                Ukprn = ukprn.ToString()
            };

            AccountProviderId = 2;

            FindProvidersQueryResult = new FindProviderToAddQueryResult(12345678, null);

            Mediator.Setup(m => m.Send(It.Is<FindProviderToAddQuery>(q => q.AccountId == 1 && q.Ukprn == 12345678), CancellationToken.None)).ReturnsAsync(FindProvidersQueryResult);
            Mediator.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderId);
            Mediator.Setup(m => m.Send(It.IsAny<GetInvitationByIdQuery>(), CancellationToken.None)).ReturnsAsync(GetInvitationByIdQueryResult);

            return AccountProvidersController.Invitation(InvitationAccountProviderRouteValues);
        }

        public AccountProvidersControllerTestsFixture CreateSession()
        {
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            AccountProvidersController.ControllerContext = new ControllerContext(context.Object, new RouteData(), AccountProvidersController);

            return this;
        }
    }
}