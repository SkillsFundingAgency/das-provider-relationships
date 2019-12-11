using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class AccountProviderLegalEntitiesControllerTests : FluentTest<AccountProviderLegalEntitiesControllerTestsFixture>
    {
        [Test]
        public Task Get_WhenGettingGetAction_ThenShouldReturnGetView()
        {
            return RunAsync(f => f.Get(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");

                var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<GetAccountProviderLegalEntityViewModel>().Which;

                model.AccountProvider.Should().BeSameAs(f.GetAccountProviderLegalEntityQueryResult.AccountProvider);
                model.AccountLegalEntity.Should().BeEquivalentTo(f.GetAccountProviderLegalEntityQueryResult.AccountLegalEntity);
                
                model.Operations.Should().BeEquivalentTo(new List<OperationViewModel>
                {
                    new OperationViewModel
                    {
                        IsEnabled = true,
                        Value = Operation.CreateCohort
                    },
                    new OperationViewModel
                    {
                        IsEnabled = false,
                        Value = Operation.Recruitment
                    }
                });
                
                model.AccountLegalEntitiesCount.Should().Be(f.GetAccountProviderLegalEntityQueryResult.AccountLegalEntitiesCount);
            });
        }

        [Test]
        public void Get_WhenPostingGetAction_ThenShouldAddOperationsToTempData()
        {
            Run(f => f.PostGet(), f => f.AccountProviderLegalEntitiesController.TempData.Get<List<OperationViewModel>>().Should().NotBeNull()
                .And.BeSameAs(f.GetAccountProviderLegalEntityViewModel.Operations));
        }

        [Test]
        public void Get_WhenPostingGetAction_ThenShouldRedirectToUpdateAction()
        {
            Run(f => f.PostGet(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Update") &&
                a.RouteValues["AccountProviderId"].Equals(f.GetAccountProviderLegalEntityViewModel.AccountProviderId.Value) &&
                a.RouteValues["AccountLegalEntityId"].Equals(f.GetAccountProviderLegalEntityViewModel.AccountLegalEntityId.Value)));
        }

        [Test]
        public Task Update_WhenGettingUpdateActionAndTempDataOperationsIsNull_ThenShouldRedirectToGetAction()
        {
            return RunAsync(f => f.Update(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a => 
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["AccountProviderId"].Equals(f.UpdateAccountProviderLegalEntityRouteValues.AccountProviderId) &&
                a.RouteValues["AccountLegalEntityId"].Equals(f.UpdateAccountProviderLegalEntityRouteValues.AccountLegalEntityId)));
        }

        [Test]
        public Task Update_WhenGettingUpdateActionAndTempDataOperationsIsNotNull_ThenShouldReturnUpdateView()
        {
            return RunAsync(f => f.Update(Operation.CreateCohort), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");

                var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<UpdateAccountProviderLegalEntityViewModel>().Which;

                model.AccountProvider.Should().BeSameAs(f.GetAccountProviderLegalEntityQueryResult.AccountProvider);
                model.AccountLegalEntity.Should().BeSameAs(f.GetAccountProviderLegalEntityQueryResult.AccountLegalEntity);
                model.Operations.Should().BeSameAs(f.AccountProviderLegalEntitiesController.TempData.Get<List<OperationViewModel>>());
            });
        }

        [Test]
        public Task Update_WhenPostingUpdateAction_ThenShouldSendUpdatePermissionsCommand()
        {
            return RunAsync(f => f.CreateSession(), f => f.PostUpdate(), f => f.Mediator.Verify(m => m.Send(
                It.Is<UpdatePermissionsCommand>(c => 
                    c.AccountId == f.UpdateAccountProviderLegalEntityViewModel.AccountId &&
                    c.UserRef == f.UpdateAccountProviderLegalEntityViewModel.UserRef &&
                    c.AccountProviderId == f.UpdateAccountProviderLegalEntityViewModel.AccountProviderId &&
                    c.AccountLegalEntityId == f.UpdateAccountProviderLegalEntityViewModel.AccountLegalEntityId &&
                    c.GrantedOperations.SetEquals(f.UpdateAccountProviderLegalEntityViewModel.Operations.Where(o => o.IsEnabled).Select(o => o.Value))),
                CancellationToken.None), Times.Once));
        }

        [Test]
        public Task Update_WhenPostingUpdateAction_ThenShouldRedirectToUpdatedAction()
        {
            return RunAsync(f => f.CreateSession(), f => f.PostUpdate(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Updated") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderId"].Equals(f.UpdateAccountProviderLegalEntityViewModel.AccountProviderId) &&
                a.RouteValues["AccountLegalEntityId"].Equals(f.UpdateAccountProviderLegalEntityViewModel.AccountLegalEntityId)));
        }

        [Test]
        public Task Update_WhenPostingUpdateActionFromInvitation_ThenShouldRedirectToEmployerAccountUrl()
        {
            return RunAsync(f => f.CreateSessionFromInvitation(), f => f.PostUpdate(), (f, r) => r.Should().NotBeNull().And.Match<RedirectResult>(a =>
                a.Url.Equals("https://localhost/accounts/ABC123/teams/addedprovider/Foo+Bar")));
        }

        [Test]
        public Task Updated_WhenGettingUpdatedAction_ThenShouldReturnView()
        {
            return RunAsync(f => f.CreateSession(), f => f.Updated(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");

                var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<UpdatedAccountProviderLegalEntityViewModel>().Which;
                
                model.AccountProviderLegalEntity.Should().BeSameAs(f.GetUpdatedAccountProviderLegalEntityQueryResult.AccountProviderLegalEntity);
                model.AccountLegalEntitiesCount.Should().Be(f.GetUpdatedAccountProviderLegalEntityQueryResult.AccountLegalEntitiesCount);
            });
        }

        [Test]
        public void Updated_WhenPostingUpdatedActionAndSetPermissionsOptionIsSelected_ThenShouldRedirectToPermissionsIndexAction()
        {
            Run(f => f.PostUpdated("SetPermissions"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["Controller"].Equals("AccountProviders") &&
                a.RouteValues["AccountProviderId"].Equals(f.UpdatedAccountProviderLegalEntityViewModel.AccountProviderId)));
        }

        [Test]
        public void Updated_WhenPostingUpdatedActionAndAddTrainingProviderOptionIsSelected_ThenShouldRedirectToFindAction()
        {
            Run(f => f.PostUpdated("AddTrainingProvider"), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Find") &&
                a.RouteValues["Controller"].Equals("AccountProviders")));
        }

        [Test]
        public void Updated_WhenPostingUpdatedActionAndGoToHomepageOptionIsSelected_ThenShouldRedirectToHomeUrl()
        {
            Run(f => f.PostUpdated("GoToHomepage"), (f, r) => r.Should().NotBeNull().And.Match<RedirectResult>(a =>
                a.Url == $"https://localhost/accounts/ABC123/teams"));
        }

        [Test]
        public void Added_WhenPostingAddedActionAndNoOptionIsSelected_ThenShouldThrowException()
        {
            Run(f => f.PostUpdated(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
        }
    }

    public class AccountProviderLegalEntitiesControllerTestsFixture
    {
        public AccountProviderLegalEntitiesController AccountProviderLegalEntitiesController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        public GetAccountProviderLegalEntityRouteValues GetAccountProviderLegalEntityRouteValues { get; set; }
        public GetAccountProviderLegalEntityQueryResult GetAccountProviderLegalEntityQueryResult { get; set; }
        public GetAccountProviderLegalEntityViewModel GetAccountProviderLegalEntityViewModel { get; set; }
        public UpdateAccountProviderLegalEntityRouteValues UpdateAccountProviderLegalEntityRouteValues { get; set; }
        public UpdateAccountProviderLegalEntityViewModel UpdateAccountProviderLegalEntityViewModel { get; set; }
        public GetUpdatedAccountProviderLegalEntityQueryResult GetUpdatedAccountProviderLegalEntityQueryResult { get; set; }
        public UpdatedAccountProviderLegalEntityRouteValues UpdatedAccountProviderLegalEntityRouteValues { get; set; }
        public UpdatedAccountProviderLegalEntityViewModel UpdatedAccountProviderLegalEntityViewModel { get; set; }
        public GetAccountProviderQueryResult GetAccountProviderQueryResult { get; set; }

        public AccountProviderLegalEntitiesControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderLegalEntityMappings))).CreateMapper();
            EmployerUrls = new Mock<IEmployerUrls>();
            AccountProviderLegalEntitiesController = new AccountProviderLegalEntitiesController(Mediator.Object, Mapper, EmployerUrls.Object);
        }

        public Task<ActionResult> Get()
        {
            GetAccountProviderLegalEntityRouteValues = new GetAccountProviderLegalEntityRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2,
                AccountLegalEntityId = 3
            };
            
            GetAccountProviderLegalEntityQueryResult = new GetAccountProviderLegalEntityQueryResult(
                new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderDto(),
                new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountLegalEntityDto(),
                new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderLegalEntityDto
                {
                    Operations = new List<Operation>
                    {
                        Operation.CreateCohort
                    }
                },
                2,
                false);
            
            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderLegalEntityQuery>(q => 
                    q.AccountId == GetAccountProviderLegalEntityRouteValues.AccountId &&
                    q.AccountProviderId == GetAccountProviderLegalEntityRouteValues.AccountProviderId &&
                    q.AccountLegalEntityId == GetAccountProviderLegalEntityRouteValues.AccountLegalEntityId), CancellationToken.None))
                .ReturnsAsync(GetAccountProviderLegalEntityQueryResult);
            
            return AccountProviderLegalEntitiesController.Get(GetAccountProviderLegalEntityRouteValues);
        }

        public ActionResult PostGet()
        {
            GetAccountProviderLegalEntityViewModel = new GetAccountProviderLegalEntityViewModel
            {
                AccountProviderId = 2,
                AccountLegalEntityId = 3,
                Operations = new List<OperationViewModel>
                {
                    new OperationViewModel
                    {
                        Value = Operation.CreateCohort,
                        IsEnabled = true
                    }
                }
            };
            
            return AccountProviderLegalEntitiesController.Get(GetAccountProviderLegalEntityViewModel);
        }

        public Task<ActionResult> Update(params Operation[] grantedOperations)
        {
            UpdateAccountProviderLegalEntityRouteValues = new UpdateAccountProviderLegalEntityRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2,
                AccountLegalEntityId = 3
            };
            
            if (grantedOperations.Any())
            {
                AccountProviderLegalEntitiesController.TempData.Set(grantedOperations
                .Select(o => new OperationViewModel
                {
                    Value = o,
                    IsEnabled = true
                })
                .ToList());
            }
            
            GetAccountProviderLegalEntityQueryResult = new GetAccountProviderLegalEntityQueryResult(
                new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderDto(),
                new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountLegalEntityDto(),
                new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderLegalEntityDto
                {
                    Operations = new List<Operation>
                    {
                        Operation.CreateCohort
                    }
                },
                2,
                false);
            
            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderLegalEntityQuery>(q =>
                    q.AccountId == UpdateAccountProviderLegalEntityRouteValues.AccountId &&
                    q.AccountProviderId == UpdateAccountProviderLegalEntityRouteValues.AccountProviderId &&
                    q.AccountLegalEntityId == UpdateAccountProviderLegalEntityRouteValues.AccountLegalEntityId), CancellationToken.None))
                .ReturnsAsync(GetAccountProviderLegalEntityQueryResult);
            
            return AccountProviderLegalEntitiesController.Update(UpdateAccountProviderLegalEntityRouteValues);
        }

        public Task<ActionResult> PostUpdate()
        {
            UpdateAccountProviderLegalEntityViewModel = new UpdateAccountProviderLegalEntityViewModel
            {
                AccountId = 1,
                UserRef = Guid.NewGuid(),
                AccountProviderId = 2,
                AccountLegalEntityId = 3,
                Operations = new List<OperationViewModel>
                {
                    new OperationViewModel
                    {
                        Value = Operation.CreateCohort,
                        IsEnabled = true
                    }
                }
            };

            GetAccountProviderQueryResult = new GetAccountProviderQueryResult(
                new Application.Queries.GetAccountProvider.Dtos.AccountProviderDto {
                    Id = 2,
                    ProviderName = "Foo Bar"
                },
                true);

            Mediator.Setup(m => m.Send(It.IsAny<UpdatePermissionsCommand>(), CancellationToken.None)).ReturnsAsync(Unit.Value);
            Mediator.Setup(m => m.Send(It.IsAny<GetAccountProviderQuery>(), CancellationToken.None)).ReturnsAsync(GetAccountProviderQueryResult);

            return AccountProviderLegalEntitiesController.Update(UpdateAccountProviderLegalEntityViewModel);
        }

        public Task<ActionResult> Updated()
        {
            UpdatedAccountProviderLegalEntityRouteValues = new UpdatedAccountProviderLegalEntityRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2,
                AccountLegalEntityId = 3
            };
            
            GetUpdatedAccountProviderLegalEntityQueryResult = new GetUpdatedAccountProviderLegalEntityQueryResult(
                new Application.Queries.GetUpdatedAccountProviderLegalEntity.Dtos.AccountProviderLegalEntityDto
                {
                    Id = 4,
                    ProviderName = "Foo",
                    AccountLegalEntityName = "Bar"
                }, 2);

            Mediator.Setup(m => m.Send(
                    It.Is<GetUpdatedAccountProviderLegalEntityQuery>(q => 
                        q.AccountId == UpdatedAccountProviderLegalEntityRouteValues.AccountId &&
                        q.AccountProviderId == UpdatedAccountProviderLegalEntityRouteValues.AccountProviderId &&
                        q.AccountLegalEntityId == UpdatedAccountProviderLegalEntityRouteValues.AccountLegalEntityId),
                    CancellationToken.None))
                .ReturnsAsync(GetUpdatedAccountProviderLegalEntityQueryResult);
            
            return AccountProviderLegalEntitiesController.Updated(UpdatedAccountProviderLegalEntityRouteValues);
        }

        public ActionResult PostUpdated(string choice = null)
        {
            UpdatedAccountProviderLegalEntityViewModel = new UpdatedAccountProviderLegalEntityViewModel
            {
                AccountProviderId = 2,
                Choice = choice
            };

            EmployerUrls.Setup(eu => eu.Account(null))
                .Returns($"https://localhost/accounts/ABC123/teams");

            return AccountProviderLegalEntitiesController.Updated(UpdatedAccountProviderLegalEntityViewModel);
        }

        public AccountProviderLegalEntitiesControllerTestsFixture CreateSession()
        {
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            AccountProviderLegalEntitiesController.ControllerContext = new ControllerContext(context.Object, new RouteData(), AccountProviderLegalEntitiesController);

            return this;
        }

        public AccountProviderLegalEntitiesControllerTestsFixture CreateSessionFromInvitation()
        {
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();

            session.Setup(s => s["Invitation"]).Returns(true);
            context.Setup(x => x.Session).Returns(session.Object);
            EmployerUrls.Setup(e => e.Account(It.IsAny<string>())).Returns("https://localhost/accounts/ABC123/teams");

            AccountProviderLegalEntitiesController.ControllerContext = new ControllerContext(context.Object, new RouteData(), AccountProviderLegalEntitiesController);
            return this;
        }
    }
}