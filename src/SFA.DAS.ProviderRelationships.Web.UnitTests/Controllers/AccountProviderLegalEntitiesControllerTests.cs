using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class AccountProviderLegalEntitiesControllerTests : FluentTest<AccountProviderLegalEntitiesControllerTestsFixture>
    {
        [Test]
        public void Permissions_WhenGettingPermissionsAction_ThenShouldReturnView()
        {
            TestAsync(f => f.Permissions(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "Permissions");
            });
        }

        [Test]
        public Task Update_WhenPostingPermissionsActionWithoutConfirmationSet_ThenShouldSetErrorState()
        {
            return TestAsync(f => f.CreateSession(), f => f.PostUpdate(null, null, State.No), (f, r) => r.Should().NotBeNull().And.Match<ViewResult>(
                v => v.ViewName.Equals("Confirm") &&
                     f.AccountProviderLegalEntitiesController.ModelState.ContainsKey("Confirmation")));
        }

        [Test]
        public Task Update_WhenPostingPermissionsActionWithConfirmation_ThenShouldSendUpdatePermissionsCommand()
        {
            return TestAsync(f => f.CreateSession(), f => f.PostUpdate(true, null, State.No), f => f.Mediator.Verify(m => m.Send(
                It.Is<UpdatePermissionsCommand>(c =>
                    c.AccountId == f.AccountId &&
                    c.UserRef == f.AccountProviderLegalEntityViewModel.UserRef &&
                    c.AccountProviderId == f.AccountProviderLegalEntityViewModel.AccountProviderId &&
                    c.AccountLegalEntityId == f.AccountProviderLegalEntityViewModel.AccountLegalEntityId &&
                    c.GrantedOperations.SetEquals(f.AccountProviderLegalEntityViewModel.Permissions.ToOperations())),
                CancellationToken.None), Times.Once));
        }

        [Test]
        public Task Update_WhenPostingPermissionsActionWithNoConfirmation_ThenShouldNotSendUpdatePermissionsCommand()
        {
            return TestAsync(f => f.CreateSession(), f => f.PostUpdate(false, null, State.No), f => f.Mediator.Verify(m => m.Send(
                It.Is<UpdatePermissionsCommand>(c =>
                    c.AccountId == f.AccountId &&
                    c.UserRef == f.AccountProviderLegalEntityViewModel.UserRef &&
                    c.AccountProviderId == f.AccountProviderLegalEntityViewModel.AccountProviderId &&
                    c.AccountLegalEntityId == f.AccountProviderLegalEntityViewModel.AccountLegalEntityId &&
                    c.GrantedOperations.SetEquals(f.AccountProviderLegalEntityViewModel.Permissions.ToOperations())),
                CancellationToken.None), Times.Never));
        }

        [Test]
        public Task Update_WhenPostingPermissionsActionWithChangeCommand_ThenShouldReturnPermissionsView()
        {
            return TestAsync(f => f.CreateSession(), f => f.PostUpdate(false, "Change", State.Yes), (f, r) => r.Should().NotBeNull().And.Match<ViewResult>(
                v => v.ViewName.Equals("Permissions")));
        }

        [Test]
        public Task Update_WhenPostingPermissionsActionWithConfirmation_ThenShouldRedirectToAccountProvidersIndexActionWithTempDataSetCorrectly()
        {
            return TestAsync(
                f => f.CreateSession(),
                f => f.PostUpdate(true, null, State.No),
                (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                    a.ActionName.Equals("Index") &&
                    a.ControllerName.Equals("AccountProviders") &&
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
            return TestAsync(
                f => f.CreateSessionFromInvitation(),
                f => f.PostUpdate(true, null, State.No),
                (f, r) => r.Should().NotBeNull().And.Match<RedirectResult>(a =>
                a.Url.Equals("https://localhost/accounts/ABC123/teams/addedprovider/Foo+Bar")));
        }
    }

    public class AccountProviderLegalEntitiesControllerTestsFixture
    {
        public AccountProviderLegalEntitiesController AccountProviderLegalEntitiesController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public Mock<IEncodingService> EncodingService { get; set; }
        public IMapper Mapper { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        public AccountProviderLegalEntityRouteValues AccountProviderLegalEntityRouteValues { get; set; }
        public GetAccountProviderLegalEntityQueryResult GetAccountProviderLegalEntityQueryResult { get; set; }
        public AccountProviderLegalEntityViewModel GetAccountProviderLegalEntityViewModel { get; set; }
        public AccountProviderLegalEntityViewModel AccountProviderLegalEntityViewModel { get; set; }
        public GetUpdatedAccountProviderLegalEntityQueryResult GetUpdatedAccountProviderLegalEntityQueryResult { get; set; }
        public GetAccountProviderQueryResult GetAccountProviderQueryResult { get; set; }
        public long AccountId = 1;

        public AccountProviderLegalEntitiesControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            EncodingService = new Mock<IEncodingService>();
            Mapper = new MapperConfiguration(c => c.AddProfiles(new[] { new AccountProviderLegalEntityMappings() })).CreateMapper();
            EmployerUrls = new Mock<IEmployerUrls>();

            AccountProviderLegalEntitiesController = new AccountProviderLegalEntitiesController(Mediator.Object, Mapper, EmployerUrls.Object, EncodingService.Object);
        }

        public Task<IActionResult> Permissions()
        {
            AccountProviderLegalEntityRouteValues = new AccountProviderLegalEntityRouteValues {
                AccountHashedId = "ABC123",
                AccountProviderId = 2,
                AccountLegalEntityId = 3
            };

            EncodingService.Setup(x =>
                x.Decode(AccountProviderLegalEntityRouteValues.AccountHashedId, EncodingType.AccountId)).Returns(AccountId);

            GetAccountProviderLegalEntityQueryResult = new GetAccountProviderLegalEntityQueryResult(
                new AccountProviderDto(),
                new AccountLegalEntityDto(),
                new AccountProviderLegalEntityDto {
                    Operations = new List<Operation>
                    {
                        Operation.CreateCohort
                    }
                },
                2,
                false);

            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderLegalEntityQuery>(q =>
                    q.AccountId == AccountId &&
                    q.AccountProviderId == AccountProviderLegalEntityRouteValues.AccountProviderId &&
                    q.AccountLegalEntityId == AccountProviderLegalEntityRouteValues.AccountLegalEntityId), CancellationToken.None))
                .ReturnsAsync(GetAccountProviderLegalEntityQueryResult);

            return AccountProviderLegalEntitiesController.Permissions(AccountProviderLegalEntityRouteValues);
        }

        public Task<IActionResult> PostUpdate(bool? confirmation, string command, State recruitState)
        {
            AccountProviderLegalEntityViewModel = new AccountProviderLegalEntityViewModel {
                AccountHashedId = "ABC123",
                UserRef = Guid.NewGuid(),
                AccountProviderId = 2,
                AccountLegalEntityId = 3,
                AccountLegalEntity = new AccountLegalEntityDto {
                    Name = "ALE LTD"
                },
                AccountProvider = new AccountProviderDto {
                    ProviderName = "PROVIDER COLLEGE"
                },
                Permissions = new List<PermissionViewModel>
                {
                    new PermissionViewModel
                    {
                        Value = Permission.CreateCohort,
                        State = State.Yes
                    },
                    new PermissionViewModel
                    {
                        Value = Permission.Recruitment,
                        State = recruitState
                    }
                },
                Confirmation = confirmation
            };

            EncodingService.Setup(x =>
                x.Decode(AccountProviderLegalEntityViewModel.AccountHashedId, EncodingType.AccountId)).Returns(AccountId);

            GetAccountProviderQueryResult = new GetAccountProviderQueryResult(
                new SFA.DAS.ProviderRelationships.Types.Dtos.AccountProviderDto {
                    Id = 2,
                    ProviderName = "Foo Bar"
                });

            Mediator.Setup(m => m.Send(It.IsAny<UpdatePermissionsCommand>(), CancellationToken.None)).ReturnsAsync(Unit.Value);
            Mediator.Setup(m => m.Send(It.IsAny<GetAccountProviderQuery>(), CancellationToken.None)).ReturnsAsync(GetAccountProviderQueryResult);

            return AccountProviderLegalEntitiesController.Confirm(AccountProviderLegalEntityViewModel, command);
        }

        public Task<IActionResult> Updated()
        {
            AccountProviderLegalEntityRouteValues = new AccountProviderLegalEntityRouteValues {
                AccountHashedId = "ABC123",
                AccountProviderId = 2,
                AccountLegalEntityId = 3
            };

            const long accountId = 1;

            EncodingService.Setup(x =>
                x.Decode(AccountProviderLegalEntityRouteValues.AccountHashedId, EncodingType.AccountId)).Returns(accountId);

            GetUpdatedAccountProviderLegalEntityQueryResult = new GetUpdatedAccountProviderLegalEntityQueryResult(
                new Application.Queries.GetUpdatedAccountProviderLegalEntity.Dtos.AccountProviderLegalEntityDto {
                    Id = 4,
                    ProviderName = "Foo",
                    AccountLegalEntityName = "Bar"
                }, 2);

            Mediator.Setup(m => m.Send(
                    It.Is<GetUpdatedAccountProviderLegalEntityQuery>(q =>
                        q.AccountId == accountId &&
                        q.AccountProviderId == AccountProviderLegalEntityRouteValues.AccountProviderId &&
                        q.AccountLegalEntityId == AccountProviderLegalEntityRouteValues.AccountLegalEntityId),
                    CancellationToken.None))
                .ReturnsAsync(GetUpdatedAccountProviderLegalEntityQueryResult);

            return AccountProviderLegalEntitiesController.Permissions(AccountProviderLegalEntityRouteValues);
        }

        public AccountProviderLegalEntitiesControllerTestsFixture CreateSession()
        {
            var context = new DefaultHttpContext() { Session = Mock.Of<ISession>() };
            AccountProviderLegalEntitiesController.ControllerContext = new ControllerContext() {
                HttpContext = context
            };
            AccountProviderLegalEntitiesController.TempData = new TempDataDictionary(context, Mock.Of<ITempDataProvider>());
            return this;
        }

        public AccountProviderLegalEntitiesControllerTestsFixture CreateSessionFromInvitation()
        {
            var session = new Mock<ISession>();
            var expectedValue = System.Text.Encoding.UTF8.GetBytes("true");
            session
                .Setup(s => s.TryGetValue("Invitation", out expectedValue))
                .Returns(true);
            EmployerUrls
                .Setup(e => e.Account(It.IsAny<string>()))
                .Returns("https://localhost/accounts/ABC123/teams");
            AccountProviderLegalEntitiesController.ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext() { Session = session.Object }
            };
            return this;
        }
    }
}