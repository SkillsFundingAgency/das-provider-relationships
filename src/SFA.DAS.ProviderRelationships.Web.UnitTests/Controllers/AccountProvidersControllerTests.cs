using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.Testing;
using SFA.DAS.Testing.AutoFixture;
using Operation = SFA.DAS.ProviderRelationships.Types.Models.Operation;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers;

[TestFixture]
[Parallelizable]
public class AccountProvidersControllerTests : FluentTest<AccountProvidersControllerTestsFixture>
{
    [Test]
    public Task Index_WhenGettingIndexAction_ThenShouldReturnIndexView()
    {
        return TestAsync(f => f.Index(), (f, r) =>
        {
            var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<AccountProvidersViewModel>().Which;

            model.AccountProviders.Should().BeEquivalentTo(f.GetAccountProvidersQueryResult.AccountProviders);
            model.AccountLegalEntitiesCount.Should().Be(f.GetAccountProvidersQueryResult.AccountLegalEntitiesCount);
        });
    }

    [Test]
    public void Find_WhenGettingFindAction_ThenShouldReturnFindView()
    {
        TestAsync(f => f.Find(), (f, r) =>
        {
            r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
            r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<FindProviderViewModel>();
        });
    }

    [Test]
    public Task Find_WhenPostingFindActionAndProviderExists_ThenShouldRedirectToAddAction()
    {
        return TestAsync(
            fixture => fixture.PostFind(true),
            (fixture, result) => result.Should().NotBeNull().And.Match<RedirectToActionResult>(actionResult =>
                actionResult.ActionName.Equals(AccountProviders.ActionNames.Add) &&
                actionResult.ControllerName == null &&
                actionResult.RouteValues[RouteValueKeys.UkPrn].Equals(fixture.FindProvidersQueryResult.Ukprn) &&
                actionResult.RouteValues[RouteValueKeys.AccountHashedId].Equals(fixture.AccountHashedId)
                )
            );
    }

    [Test]
    public Task Find_WhenPostingFindActionAndProviderDoesNotExist_ThenShouldAddModelError()
    {
        return TestAsync(
            fixture => fixture.PostFind(),
            fixture => fixture.AccountProvidersController.ModelState.ContainsKey(nameof(FindProviderEditModel.Ukprn)).Should().BeTrue()
            );
    }

    [Test]
    public Task Find_WhenPostingFindActionAndProviderDoesNotExist_ThenShouldRedirectToFindAction()
    {
        return TestAsync(
            fixture => fixture.PostFind(),
            (fixture, result) => result.Should().NotBeNull().And.Match<RedirectToActionResult>(actionResult =>
                actionResult.ActionName.Equals(AccountProviders.ActionNames.Find) &&
                actionResult.RouteValues[RouteValueKeys.AccountHashedId].Equals(fixture.AccountHashedId) &&
                actionResult.ControllerName == null)
            );
    }

    [Test]
    public Task Find_WhenPostingFindActionAndProviderAlreadyAdded_ThenShouldRedirectToAlreadyAddedAction()
    {
        return TestAsync(
            f => f.PostFind(true, true),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviders.ActionNames.AlreadyAdded) &&
                a.ControllerName == null &&
                a.RouteValues[RouteValueKeys.AccountProviderId].Equals(f.FindProvidersQueryResult.AccountProviderId) &&
                a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId)
                )
            );
    }

    [Test]
    public Task Add_WhenGettingAddAction_ThenShouldReturnAddView()
    {
        return TestAsync(
            f => f.Add(),
            (f, r) =>
            {
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddAccountProviderViewModel>(m => m.Provider == f.GetProviderToAddQueryResult.Provider);
            });
    }

    [Test]
    public Task Add_WhenPostingAddActionAndConfirmOptionIsSelected_ThenShouldSendAddAccountProviderCommand()
    {
        return TestAsync(f => f.PostAdd("Confirm"), f => f.Mediator.Verify(m => m.Send(
            It.Is<AddAccountProviderCommand>(c =>
                c.AccountId == f.AddAccountProviderViewModel.AccountId &&
                c.UserRef == f.AddAccountProviderViewModel.UserRef &&
                c.Ukprn == f.AddAccountProviderViewModel.Ukprn),
            CancellationToken.None), Times.Once));
    }

    [Test]
    public Task Add_WhenPostingAddActionAndConfirmOptionIsSelected_ThenShouldRedirectToAddedAction()
    {
        return TestAsync(
            f => f.PostAdd("Confirm"),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                        a.ActionName.Equals(AccountProviders.ActionNames.Added) &&
                        a.ControllerName == null &&
                        a.RouteValues[RouteValueKeys.AccountProviderId].Equals(f.AccountProviderId) &&
                        a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId)
                        )
                );
    }

    [Test]
    public Task Add_WhenPostingAddActionAndReEnterUkprnOptionIsSelected_ThenShouldNotAddAccountProvider()
    {
        return TestAsync(f => f.PostAdd("ReEnterUkprn"), f => f.Mediator.Verify(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None), Times.Never));
    }

    [Test]
    public Task Add_WhenPostingAddActionAndReEnterUkprnOptionIsSelected_ThenShouldRedirectToFindAction()
    {
        return TestAsync(
            f => f.PostAdd("ReEnterUkprn"),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviders.ActionNames.Find) &&
                a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId) &&
                a.ControllerName == null));
    }

    [Test]
    public Task Add_WhenPostingAddActionAndNoOptionIsSelected_ThenShouldThrowException()
    {
        return TestExceptionAsync(
            f => f.PostAdd(),
            (f, r) => r.Should().ThrowAsync<ValidationException>());
    }

    [Test]
    public Task Add_WhenInvitationIsValid_ThenShouldSendAddAccountProviderCommand()
    {
        return TestAsync(f => f.CreateSession(), f => f.Invitation(), f => f.Mediator.Verify(m => m.Send(
            It.Is<AddAccountProviderCommand>(c =>
                c.AccountId == f.AddAccountProviderViewModel.AccountId &&
                c.UserRef == f.AddAccountProviderViewModel.UserRef &&
                c.Ukprn == f.AddAccountProviderViewModel.Ukprn),
            CancellationToken.None), Times.Once));
    }

    [Test]
    public Task Add_WhenInvitationIsValid_ThenShouldRedirectToAddedAction()
    {
        return TestAsync(
            f => f.CreateSession(),
            f => f.Invitation(),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviders.ActionNames.Get) &&
                a.ControllerName == null &&
                a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId) &&
                a.RouteValues[RouteValueKeys.AccountProviderId].Equals(f.AccountProviderId)));
    }

    [Test]
    public Task Added_WhenGettingAddedAction_ThenShouldReturnAddedView()
    {
        return TestAsync(f => f.Added(), (f, r) =>
        {
            r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AddedAccountProviderViewModel>(m => m.AccountProvider == f.GetAddedAccountProviderQueryResult.AccountProvider);
        });
    }

    [Test]
    public void Added_WhenPostingAddedActionAndSetPermissionsOptionIsSelected_ThenShouldRedirectToPermissionsIndexAction()
    {
        Test(
            f => f.PostAdded("SetPermissions"),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviders.ActionNames.Get) &&
                a.ControllerName == null &&
                a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId) &&
                a.RouteValues[RouteValueKeys.AccountProviderId].Equals(f.AddedAccountProviderViewModel.AccountProviderId)));
    }

    [Test]
    public void Added_WhenPostingAddedActionAndAddTrainingProviderOptionIsSelected_ThenShouldRedirectToFindAction()
    {
        Test(
            f => f.PostAdded("AddTrainingProvider"),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviders.ActionNames.Find) &&
                a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId) &&
                a.ControllerName == null));
    }

    [Test]
    public void Added_WhenPostingAddedActionAndGoToHomepageOptionIsSelected_ThenShouldRedirectToHomeUrl()
    {
        Test(f => f.PostAdded("GoToHomepage"), (f, r) => r.Should().NotBeNull().And.Match<RedirectResult>(a =>
            a.Url == $"https://localhost/accounts/ABC123/teams"));
    }

    [Test]
    public void Added_WhenPostingAddedActionAndNoOptionIsSelected_ThenShouldThrowException()
    {
        TestException(f => f.PostAdded(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
    }

    [Test]
    public Task AlreadyAdded_WhenGettingAlreadyAddedAction_ThenShouldReturnAlreadyAddedView()
    {
        return TestAsync(
            f => f.AlreadyAdded(),
            (f, r) =>
            {
                r.As<ViewResult>().Model.Should().NotBeNull().And.Match<AlreadyAddedAccountProviderViewModel>(m => m.AccountProvider == f.GetAddedAccountProviderQueryResult.AccountProvider);
            });
    }

    [Test]
    public void AlreadyAdded_WhenPostingAlreadyAddedActionAndSetPermissionsOptionIsSelected_ThenShouldRedirectToPermissionsIndexAction()
    {
        Test(
            f => f.PostAlreadyAdded("SetPermissions"),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviders.ActionNames.Get) &&
                a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId) &&
                a.ControllerName == null));
    }

    [Test]
    public void AlreadyAdded_WhenPostingAlreadyAddedActionAndAddTrainingProviderOptionIsSelected_ThenShouldRedirectToFindAction()
    {
        Test(
            f => f.PostAlreadyAdded("AddTrainingProvider"),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviders.ActionNames.Find) &&
                a.RouteValues[RouteValueKeys.AccountHashedId].Equals(f.AccountHashedId) &&
                a.ControllerName == null));
    }

    [Test]
    public void AlreadyAdded_WhenPostingAlreadyAddedActionAndNoOptionIsSelected_ThenShouldThrowException()
    {
        TestException(f => f.PostAlreadyAdded(), (f, r) => r.Should().Throw<ArgumentOutOfRangeException>());
    }

    [Test]
    public Task Get_WhenAccountHasMultipleAccountLegalEntities_ThenShouldReturnGetView()
    {
        return TestAsync(f => f.Get(), (f, r) =>
        {
            var model = r.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<GetAccountProviderViewModel>().Which;

            model.AccountProvider.Should().BeSameAs(f.GetAccountProviderQueryResult.AccountProvider);
        });
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public Task Get_ShouldCheckIfEmployerAuthorisedAndSetModelIsUpdatePermissionsOperationAuthorized(bool expected)
    {
        return TestAsync(fixture =>
        {
            fixture.EmployerAccountAuthorisationHandler.Setup(x =>
                x.CheckUserAccountAccess(It.IsAny<ClaimsPrincipal>(),
                    EmployerUserRole.Owner)).Returns(expected);

            return fixture.Get();
        }, (fixture, result) =>
        {
            var model = result.As<ViewResult>().Model.Should().NotBeNull().And.BeOfType<GetAccountProviderViewModel>().Which;

            fixture.EmployerAccountAuthorisationHandler.Verify(x => x.CheckUserAccountAccess(It.IsAny<ClaimsPrincipal>(), EmployerUserRole.Owner));
            model.AccountProvider.Should().BeSameAs(fixture.GetAccountProviderQueryResult.AccountProvider);
            model.IsUpdatePermissionsOperationAuthorized.Should().Be(expected);
        });
    }

    [Test]
    public Task Get_WhenAccountHasSingleAccountLegalEntity_ThenShouldRedirectToAccountProviderLegalEntitiesPermissionsAction()
    {
        return TestAsync(
            f => f.Get(1),
            (f, r) => r.Should().NotBeNull().And.Match<RedirectToActionResult>(a =>
                a.ActionName.Equals(AccountProviderLegalEntities.ActionNames.Permissions) &&
                a.ControllerName.Equals(AccountProviderLegalEntities.ControllerName) &&
                a.RouteValues[RouteValueKeys.AccountProviderId].Equals(f.GetAccountProviderQueryResult.AccountProvider.Id) &&
                a.RouteValues[RouteValueKeys.AccountLegalEntityId].Equals(f.GetAccountProviderQueryResult.AccountProvider.AccountLegalEntities[0].Id)));
    }
}

public class AccountProvidersControllerTestsFixture
{
    public AccountProvidersController AccountProvidersController { get; set; }
    public FindProviderViewModel FindProviderViewModel { get; set; }
    public Mock<IMediator> Mediator { get; set; }
    public Mock<IEncodingService> EncodingService { get; set; }
    public Mock<IEmployerAccountAuthorisationHandler> EmployerAccountAuthorisationHandler { get; set; }
    public IMapper Mapper { get; set; }
    public Mock<IEmployerUrls> EmployerUrls { get; set; }
    public GetAccountProvidersQueryResult GetAccountProvidersQueryResult { get; set; }
    public FindProviderToAddQueryResult FindProvidersQueryResult { get; set; }
    public AddAccountProviderRouteValues AddAccountProviderRouteValues { get; set; }
    public GetProviderToAddQueryResult GetProviderToAddQueryResult { get; set; }
    public GetInvitationByIdQueryResult GetInvitationByIdQueryResult { get; set; }
    public AddAccountProviderViewModel AddAccountProviderViewModel { get; set; }
    public string AccountHashedId { get; set; } = "ABC123";
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
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, Guid.NewGuid().ToString()) }));
        Mediator = new Mock<IMediator>();
        EncodingService = new Mock<IEncodingService>();
        Mapper = new MapperConfiguration(c => c.AddProfile(typeof(AccountProviderMappings))).CreateMapper();
        EmployerUrls = new Mock<IEmployerUrls>();
        EmployerAccountAuthorisationHandler = new Mock<IEmployerAccountAuthorisationHandler>();

        AccountProvidersController = new AccountProvidersController(
            Mediator.Object,
            Mapper,
            EmployerUrls.Object,
            EmployerAccountAuthorisationHandler.Object,
            EncodingService.Object);

        AccountProvidersController.ControllerContext = new ControllerContext() {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    public Task<IActionResult> Index()
    {
        const long accountId = 1;

        EncodingService.Setup(x => x.Decode(AccountHashedId, EncodingType.AccountId)).Returns(accountId);

        GetAccountProvidersQueryResult = new GetAccountProvidersQueryResult(
            new List<AccountProviderDto> {
                new() {
                    Id = 2,
                    ProviderName = "Foo"
                }
            },
            2);

        Mediator.Setup(m =>
                m.Send(It.Is<GetAccountProvidersQuery>(q => q.AccountId == accountId),
                    CancellationToken.None))
            .ReturnsAsync(GetAccountProvidersQueryResult);

        return AccountProvidersController.Index(AccountHashedId);
    }

    public Task<IActionResult> Find()
    {
        return AccountProvidersController.Find();
    }

    public Task<IActionResult> PostFind(bool providerExists = false, bool providerAlreadyAdded = false)
    {
        var accountId = 1;
        var ukprn = 12345678;
        var accountProviderId = 2;

        var findProviderEditModel = new FindProviderEditModel {
            AccountHashedId = AccountHashedId,
            Ukprn = ukprn.ToString()
        };

        EncodingService.Setup(x => x.Decode(AccountHashedId, EncodingType.AccountId)).Returns(accountId);

        FindProvidersQueryResult = new FindProviderToAddQueryResult(providerExists ? ukprn : (long?)null, providerAlreadyAdded ? accountProviderId : (int?)null);

        Mediator.Setup(m => m.Send(It.Is<FindProviderToAddQuery>(q => q.AccountId == accountId && q.Ukprn == ukprn), CancellationToken.None)).ReturnsAsync(FindProvidersQueryResult);

        return AccountProvidersController.Find(findProviderEditModel);
    }

    public Task<IActionResult> Add()
    {
        AddAccountProviderRouteValues = new AddAccountProviderRouteValues {
            Ukprn = 12345678
        };

        GetProviderToAddQueryResult = new GetProviderToAddQueryResult(
            new Application.Queries.GetProviderToAdd.Dtos.ProviderDto {
                Ukprn = 12345678,
                Name = "Foo"
            });

        Mediator.Setup(m => m.Send(It.Is<GetProviderToAddQuery>(q => q.Ukprn == AddAccountProviderRouteValues.Ukprn), CancellationToken.None)).ReturnsAsync(GetProviderToAddQueryResult);

        return AccountProvidersController.Add(AddAccountProviderRouteValues);
    }

    public Task<IActionResult> PostAdd(string choice = null)
    {
        AddAccountProviderViewModel = new AddAccountProviderViewModel {
            Provider = new Application.Queries.GetProviderToAdd.Dtos.ProviderDto { Name = "ProviderName", Ukprn = 10083920 },
            AccountId = 1,
            UserRef = Guid.NewGuid(),
            Ukprn = 12345678,
            Choice = choice,
            AccountHashedId = AccountHashedId
        };

        AccountProviderId = 2;

        Mediator.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderId);

        var context = new ValidationContext(AddAccountProviderViewModel);
        Validator.ValidateObject(AddAccountProviderViewModel, context);

        return AccountProvidersController.Add(AddAccountProviderViewModel);
    }

    public Task<IActionResult> Added()
    {
        const long accountId = 1;

        AddedAccountProviderRouteValues = new AddedAccountProviderRouteValues {
            AccountHashedId = AccountHashedId,
            AccountProviderId = 2
        };

        GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new Application.Queries.GetAddedAccountProvider.Dtos.AccountProviderDto {
            Id = 2,
            ProviderUkprn = 12345678,
            ProviderName = "Foo"
        });

        EncodingService.Setup(x => x.Decode(AccountHashedId, EncodingType.AccountId)).Returns(accountId);

        Mediator.Setup(m => m.Send(It.Is<GetAddedAccountProviderQuery>(q => q.AccountId == accountId && q.AccountProviderId == AddedAccountProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAddedAccountProviderQueryResult);

        return AccountProvidersController.Added(AddedAccountProviderRouteValues);
    }

    public IActionResult PostAdded(string choice = null)
    {
        AddedAccountProviderViewModel = new AddedAccountProviderViewModel {
            AccountProviderId = 2,
            Choice = choice,
            AccountHashedId = AccountHashedId
        };

        EmployerUrls.Setup(eu => eu.Account(null))
            .Returns($"https://localhost/accounts/ABC123/teams");

        return AccountProvidersController.Added(AddedAccountProviderViewModel);
    }

    public Task<IActionResult> AlreadyAdded()
    {
        const long accountId = 1;

        AlreadyAddedAccountProviderRouteValues = new AlreadyAddedAccountProviderRouteValues {
            AccountHashedId = AccountHashedId,
            AccountProviderId = 2
        };

        GetAddedAccountProviderQueryResult = new GetAddedAccountProviderQueryResult(new Application.Queries.GetAddedAccountProvider.Dtos.AccountProviderDto {
            Id = 2,
            ProviderUkprn = 12345678,
            ProviderName = "Foo"
        });

        EncodingService.Setup(x => x.Decode(AccountHashedId, EncodingType.AccountId)).Returns(accountId);

        Mediator.Setup(m => m.Send(It.Is<GetAddedAccountProviderQuery>(q => q.AccountId == accountId && q.AccountProviderId == AlreadyAddedAccountProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAddedAccountProviderQueryResult);

        return AccountProvidersController.AlreadyAdded(AlreadyAddedAccountProviderRouteValues);
    }

    public IActionResult PostAlreadyAdded(string choice = null)
    {
        AlreadyAddedAccountProviderViewModel = new AlreadyAddedAccountProviderViewModel {
            AccountProviderId = 2,
            Choice = choice,
            AccountHashedId = AccountHashedId
        };

        return AccountProvidersController.AlreadyAdded(AlreadyAddedAccountProviderViewModel);
    }

    public Task<IActionResult> Get(int? accountLegalEntitiesCount = null)
    {
        const long accountId = 1;

        GetAccountProviderRouteValues = new GetAccountProviderRouteValues {
            AccountHashedId = AccountHashedId,
            AccountProviderId = 2
        };

        GetAccountProviderQueryResult = new GetAccountProviderQueryResult(
            new AccountProviderDto {
                Id = 2,
                ProviderName = "Foo",
                AccountLegalEntities = Enumerable.Range(3, accountLegalEntitiesCount ?? 2)
                    .Select(i => new AccountLegalEntityDto {
                        Id = i,
                        Name = i.ToString(),
                        Operations = new List<Operation>
                        {
                            Operation.CreateCohort
                        }
                    })
                    .ToList()
            });

        EncodingService.Setup(x => x.Decode(AccountHashedId, EncodingType.AccountId)).Returns(accountId);
        Mediator.Setup(m => m.Send(It.Is<GetAccountProviderQuery>(q => q.AccountId == accountId && q.AccountProviderId == GetAccountProviderRouteValues.AccountProviderId), CancellationToken.None)).ReturnsAsync(GetAccountProviderQueryResult);

        return AccountProvidersController.Get(GetAccountProviderRouteValues);
    }

    public Task<IActionResult> Invitation()
    {
        var userRef = Guid.NewGuid();
        var correlationId = Guid.NewGuid();
        var ukprn = 12345678;
        var accountId = 1;

        InvitationAccountProviderRouteValues = new InvitationAccountProviderRouteValues {
            AccountHashedId = AccountHashedId,
            CorrelationId = correlationId,
            UserRef = userRef
        };

        GetInvitationByIdQueryResult = new GetInvitationByIdQueryResult(new InvitationDto() {
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
            Ukprn = ukprn.ToString()
        };

        AccountProviderId = 2;

        FindProvidersQueryResult = new FindProviderToAddQueryResult(12345678, null);

        EncodingService.Setup(x => x.Decode(AccountHashedId, EncodingType.AccountId)).Returns(accountId);

        Mediator.Setup(m => m.Send(It.Is<FindProviderToAddQuery>(q => q.AccountId == accountId && q.Ukprn == 12345678), CancellationToken.None)).ReturnsAsync(FindProvidersQueryResult);
        Mediator.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), CancellationToken.None)).ReturnsAsync(AccountProviderId);
        Mediator.Setup(m => m.Send(It.IsAny<GetInvitationByIdQuery>(), CancellationToken.None)).ReturnsAsync(GetInvitationByIdQueryResult);

        return AccountProvidersController.Invitation(InvitationAccountProviderRouteValues);
    }

    public AccountProvidersControllerTestsFixture CreateSession()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, Guid.NewGuid().ToString()) }));
        AccountProvidersController.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { Session = Mock.Of<ISession>(), User = user } };

        return this;
    }
}