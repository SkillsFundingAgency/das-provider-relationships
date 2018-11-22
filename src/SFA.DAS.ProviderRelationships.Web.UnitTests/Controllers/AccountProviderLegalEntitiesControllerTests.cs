using System.Collections.Generic;
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
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    public class AccountProviderLegalEntitiesControllerTests : FluentTest<AccountProviderLegalEntitiesControllerTestsFixture>
    {
        [Test]
        public Task Get_WhenGettingGetAction_ThenShouldReturnShowView()
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
    }

    public class AccountProviderLegalEntitiesControllerTestsFixture
    {
        public AccountProviderLegalEntitiesController AccountProvidersController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public GetAccountProviderLegalEntityRouteValues GetAccountProviderLegalEntityRouteValues { get; set; }
        public GetAccountProviderLegalEntityQueryResult GetAccountProviderLegalEntityQueryResult { get; set; }

        public AccountProviderLegalEntitiesControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderLegalEntityMappings))).CreateMapper();
            AccountProvidersController = new AccountProviderLegalEntitiesController(Mediator.Object, Mapper);
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
            
            return AccountProvidersController.Get(GetAccountProviderLegalEntityRouteValues);
        }
    }
}