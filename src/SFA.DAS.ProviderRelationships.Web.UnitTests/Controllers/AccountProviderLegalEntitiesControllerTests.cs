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
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
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
                    }
                });
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
            return RunAsync(f => f.GetUpdate(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a => 
                a.RouteValues["Action"].Equals("Get") &&
                a.RouteValues["AccountProviderId"].Equals(f.UpdateAccountProviderLegalEntityRouteValues.AccountProviderId) &&
                a.RouteValues["AccountLegalEntityId"].Equals(f.UpdateAccountProviderLegalEntityRouteValues.AccountLegalEntityId)));
        }

        [Test]
        public Task Update_WhenGettingUpdateActionAndTempDataOperationsIsNotNull_ThenShouldReturnUpdateView()
        {
            return RunAsync(f => f.GetUpdate(Operation.CreateCohort), (f, r) =>
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
            return RunAsync(f => f.PostUpdate(), f => f.Mediator.Verify(m => m.Send(
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
            return RunAsync(f => f.PostUpdate(), (f, r) => r.Should().NotBeNull().And.Match<RedirectToRouteResult>(a =>
                a.RouteValues["Action"].Equals("Updated") &&
                a.RouteValues["Controller"] == null &&
                a.RouteValues["AccountProviderLegalEntityId"].Equals(f.AccountProviderLegalEntityId)));
        }
    }

    public class AccountProviderLegalEntitiesControllerTestsFixture
    {
        public AccountProviderLegalEntitiesController AccountProviderLegalEntitiesController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public GetAccountProviderLegalEntityRouteValues GetAccountProviderLegalEntityRouteValues { get; set; }
        public GetAccountProviderLegalEntityQueryResult GetAccountProviderLegalEntityQueryResult { get; set; }
        public GetAccountProviderLegalEntityViewModel GetAccountProviderLegalEntityViewModel { get; set; }
        public UpdateAccountProviderLegalEntityRouteValues UpdateAccountProviderLegalEntityRouteValues { get; set; }
        public long AccountProviderLegalEntityId { get; set; }
        public UpdateAccountProviderLegalEntityViewModel UpdateAccountProviderLegalEntityViewModel { get; set; }

        public AccountProviderLegalEntitiesControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderLegalEntityMappings))).CreateMapper();
            AccountProviderLegalEntitiesController = new AccountProviderLegalEntitiesController(Mediator.Object, Mapper);
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
                new AccountProviderBasicDto(),
                new AccountLegalEntityBasicDto(),
                new AccountProviderLegalEntityDto
                {
                    Permissions = new List<PermissionDto>
                    {
                        new PermissionDto
                        {
                            Operation = Operation.CreateCohort
                        }
                    }
                });
            
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

        public Task<ActionResult> GetUpdate(params Operation[] grantedOperations)
        {
            UpdateAccountProviderLegalEntityRouteValues = new UpdateAccountProviderLegalEntityRouteValues
            {
                AccountId = 1,
                AccountProviderId = 2,
                AccountLegalEntityId = 3
            };
            
            if (grantedOperations.Any())
            {
                AccountProviderLegalEntitiesController.TempData.Add(grantedOperations
                .Select(o => new OperationViewModel
                {
                    Value = o,
                    IsEnabled = true
                })
                .ToList());
            }
            
            GetAccountProviderLegalEntityQueryResult = new GetAccountProviderLegalEntityQueryResult(
                new AccountProviderBasicDto(),
                new AccountLegalEntityBasicDto(),
                new AccountProviderLegalEntityDto
                {
                    Permissions = new List<PermissionDto>
                    {
                        new PermissionDto
                        {
                            Operation = Operation.CreateCohort
                        }
                    }
                });
            
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

            AccountProviderLegalEntityId = 4;

            Mediator.Setup(m => m.Send(It.IsAny<UpdatePermissionsCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderLegalEntityId);

            return AccountProviderLegalEntitiesController.Update(UpdateAccountProviderLegalEntityViewModel);
        }
    }
}