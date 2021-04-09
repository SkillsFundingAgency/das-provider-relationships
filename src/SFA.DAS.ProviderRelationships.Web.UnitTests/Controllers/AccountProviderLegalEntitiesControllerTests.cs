using System;
using System.Collections.Generic;
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
using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Operations;
using SFA.DAS.Testing;
using AccountProviderDto = SFA.DAS.ProviderRelationships.Types.Dtos.AccountProviderDto;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class AccountProviderLegalEntitiesControllerTests : FluentTest<AccountProviderLegalEntitiesControllerTestsFixture>
    {
        [Test]
        public void Permissions_WhenGettingPermissionsAction_ThenShouldReturnView()
        {
            RunAsync(f => f.Permissions(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "Permissions");
            });
        }
        
        [Test]
        public Task Update_WhenPostingPermissionsAction_ThenShouldSendUpdatePermissionsCommand()
        {
            return RunAsync(f => f.CreateSession(), f => f.PostUpdate(), f => f.Mediator.Verify(m => m.Send(
                It.Is<UpdatePermissionsCommand>(c => 
                    c.AccountId == f.AccountProviderLegalEntityViewModel.AccountId &&
                    c.UserRef == f.AccountProviderLegalEntityViewModel.UserRef &&
                    c.AccountProviderId == f.AccountProviderLegalEntityViewModel.AccountProviderId &&
                    c.AccountLegalEntityId == f.AccountProviderLegalEntityViewModel.AccountLegalEntityId &&
                    c.GrantedOperations.SetEquals(f.AccountProviderLegalEntityViewModel.Operations.Where(o => o.IsEnabled.Value).Select(o => o.Value))),
                CancellationToken.None), Times.Once));
        }

        [Test]
        public Task Update_WhenPostingPermissionsAction_ThenShouldRedirectToAccountProvidersIndexActionWithTempDataSetCorrectly()
        {
            return RunAsync(f => f.CreateSession(), f => f.PostUpdate(), (f , r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Index") &&
                a.RouteValues["Controller"].Equals("AccountProviders") && 
                f.AccountProviderLegalEntitiesController.TempData.ContainsKey("PermissionsChanged") &&
                f.AccountProviderLegalEntitiesController.TempData.ContainsKey("ProviderName") &&
                f.AccountProviderLegalEntitiesController.TempData.ContainsKey("LegalEntityName") &&
                f.AccountProviderLegalEntitiesController.TempData["PermissionsChanged"].Equals(true) &&
                f.AccountProviderLegalEntitiesController.TempData["ProviderName"].Equals("PROVIDER COLLEGE") &&
                f.AccountProviderLegalEntitiesController.TempData["LegalEntityName"].Equals("ALE LTD")));
        }

        [Test]
        public Task Update_WhenPostingPermissionsActionFromInvitation_ThenShouldRedirectToEmployerAccountUrl()
        {
            return RunAsync(f => f.CreateSessionFromInvitation(), f => f.PostUpdate(), (f, r) => r.Should().NotBeNull().And.Match<RedirectResult>(a =>
                a.Url.Equals("https://localhost/accounts/ABC123/teams/addedprovider/Foo+Bar")));
        }
    }

    public class AccountProviderLegalEntitiesControllerTestsFixture
    {
        public AccountProviderLegalEntitiesController AccountProviderLegalEntitiesController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        public AccountProviderLegalEntityRouteValues AccountProviderLegalEntityRouteValues { get; set; }
        public GetAccountProviderLegalEntityQueryResult GetAccountProviderLegalEntityQueryResult { get; set; }
        public AccountProviderLegalEntityViewModel GetAccountProviderLegalEntityViewModel { get; set; }
        public AccountProviderLegalEntityViewModel AccountProviderLegalEntityViewModel { get; set; }
        public GetUpdatedAccountProviderLegalEntityQueryResult GetUpdatedAccountProviderLegalEntityQueryResult { get; set; }
        public GetAccountProviderQueryResult GetAccountProviderQueryResult { get; set; }

        public AccountProviderLegalEntitiesControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderLegalEntityMappings))).CreateMapper();
            EmployerUrls = new Mock<IEmployerUrls>();

            AccountProviderLegalEntitiesController = new AccountProviderLegalEntitiesController(Mediator.Object, Mapper, EmployerUrls.Object);
        }

        public Task<ActionResult> Permissions()
        {
            AccountProviderLegalEntityRouteValues = new AccountProviderLegalEntityRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2,
                AccountLegalEntityId = 3
            };
            
            GetAccountProviderLegalEntityQueryResult = new GetAccountProviderLegalEntityQueryResult(
                new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderDto(),
                new AccountLegalEntityDto(),
                new AccountProviderLegalEntityDto
                {
                    Operations = new List<Operation>
                    {
                        Operation.CreateCohort
                    }
                },
                2,
                false);
            
            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderLegalEntityQuery>(q => 
                    q.AccountId == AccountProviderLegalEntityRouteValues.AccountId &&
                    q.AccountProviderId == AccountProviderLegalEntityRouteValues.AccountProviderId &&
                    q.AccountLegalEntityId == AccountProviderLegalEntityRouteValues.AccountLegalEntityId), CancellationToken.None))
                .ReturnsAsync(GetAccountProviderLegalEntityQueryResult);
            
            return AccountProviderLegalEntitiesController.Permissions(AccountProviderLegalEntityRouteValues);
        }

        public Task<ActionResult> PostUpdate()
        {
            AccountProviderLegalEntityViewModel = new AccountProviderLegalEntityViewModel
            {
                AccountId = 1,
                UserRef = Guid.NewGuid(),
                AccountProviderId = 2,
                AccountLegalEntityId = 3,
                AccountLegalEntity = new AccountLegalEntityDto 
                {
                    Name = "ALE LTD"
                },
                AccountProvider = new Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderDto
                {
                    ProviderName = "PROVIDER COLLEGE"
                },
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
                new AccountProviderDto {
                    Id = 2,
                    ProviderName = "Foo Bar"
                },
                true);

            Mediator.Setup(m => m.Send(It.IsAny<UpdatePermissionsCommand>(), CancellationToken.None)).ReturnsAsync(Unit.Value);
            Mediator.Setup(m => m.Send(It.IsAny<GetAccountProviderQuery>(), CancellationToken.None)).ReturnsAsync(GetAccountProviderQueryResult);

            return AccountProviderLegalEntitiesController.Permissions(AccountProviderLegalEntityViewModel);
        }

        public Task<ActionResult> Updated()
        {
            AccountProviderLegalEntityRouteValues = new AccountProviderLegalEntityRouteValues
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
                        q.AccountId == AccountProviderLegalEntityRouteValues.AccountId &&
                        q.AccountProviderId == AccountProviderLegalEntityRouteValues.AccountProviderId &&
                        q.AccountLegalEntityId == AccountProviderLegalEntityRouteValues.AccountLegalEntityId),
                    CancellationToken.None))
                .ReturnsAsync(GetUpdatedAccountProviderLegalEntityQueryResult);
            
            return AccountProviderLegalEntitiesController.Permissions(AccountProviderLegalEntityRouteValues);
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