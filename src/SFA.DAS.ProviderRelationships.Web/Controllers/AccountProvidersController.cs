using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Validation;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorisation;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.Validation.Mvc.Attributes;

namespace SFA.DAS.ProviderRelationships.Web.Controllers;

[Route("accounts/{accountHashedId}/providers")]
public class AccountProvidersController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IEmployerUrls _employerUrls;
    private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;
    private readonly IEncodingService _encodingService;
    private readonly ILogger<AccountProvidersController> _logger;
    private readonly IUserAccountService _userAccountService;

    public AccountProvidersController(
        IMediator mediator,
        IMapper mapper,
        IEmployerUrls employerUrls,
        IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler,
        IEncodingService encodingService,
        ILogger<AccountProvidersController> logger,
        IUserAccountService userAccountService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _employerUrls = employerUrls;
        _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
        _encodingService = encodingService;
        _logger = logger;
        _userAccountService = userAccountService;
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerOrViewerAccount))]
    [Route("")]
    public async Task<IActionResult> Index(string accountHashedId)
    {
        var query = new GetAccountProvidersQuery(_encodingService.Decode(accountHashedId, EncodingType.AccountId));
        var result = await _mediator.Send(query);
        var model = _mapper.Map<AccountProvidersViewModel>(result);

        await CheckExistsAndUpsertNewUser();

        return View(model);
    }


    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route(RouteNames.Find)]
    public async Task<IActionResult> Find()
    {
        var query = new GetAllProvidersQuery();
        var result = await _mediator.Send(query);
        var model = _mapper.Map<FindProviderViewModel>(result);
        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route(RouteNames.Find)]
    public async Task<IActionResult> Find(FindProviderEditModel model)
    {
        var accountId = _encodingService.Decode(model.AccountHashedId, EncodingType.AccountId);
        var ukprn = long.Parse(model.Ukprn);
        var query = new FindProviderToAddQuery(accountId, ukprn);

        var result = await _mediator.Send(query);

        if (result.ProviderNotFound)
        {
            ModelState.AddModelError(nameof(model.Ukprn), ErrorMessages.RequiredUkprn);

            return RedirectToAction(AccountProviders.ActionNames.Find, new { model.AccountHashedId });
        }

        if (result.ProviderAlreadyAdded)
        {
            return RedirectToAction(AccountProviders.ActionNames.AlreadyAdded, new AlreadyAddedAccountProviderRouteValues { AccountProviderId = result.AccountProviderId.Value, AccountHashedId = model.AccountHashedId });
        }

        return RedirectToAction(AccountProviders.ActionNames.Add, new { result.Ukprn, model.AccountHashedId });
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [HttpNotFoundForNullModel]
    [Route(RouteNames.Add)]
    public async Task<IActionResult> Add(AddAccountProviderRouteValues routeValues)
    {
        var query = new GetProviderToAddQuery(routeValues.Ukprn.Value);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<AddAccountProviderViewModel>(result);

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route(RouteNames.Add)]
    public async Task<IActionResult> Add(AddAccountProviderViewModel model)
    {
        _logger.LogInformation("Starting controller action 'Add' in {TypeName}.", nameof(AccountProvidersController));
        
        model.UserRef = User.GetUserRef();
        model.AccountId = _encodingService.Decode(model.AccountHashedId, EncodingType.AccountId);
        
        _logger.LogInformation("UserRef: '{UserRef}', AccountHashedId: '{AccountHashedId}' AccountId: '{AccountId}', UkPrn: '{UkPrn}'.", model.UserRef, model.AccountHashedId, model.AccountId, model.Ukprn);
        
        switch (model.Choice)
        {
            case "Confirm":
                var command = new AddAccountProviderCommand(model.AccountId.Value, model.Ukprn.Value, model.UserRef.Value);
                var accountProviderId = await _mediator.Send(command);

                return RedirectToAction(AccountProviders.ActionNames.Added, new { AccountProviderId = accountProviderId, model.AccountHashedId });
            case "ReEnterUkprn":
                return RedirectToAction(AccountProviders.ActionNames.Find, new { model.AccountHashedId });
            default:
                throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
        }
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [HttpNotFoundForNullModel]
    [Route("{accountProviderId}/added")]
    public async Task<IActionResult> Added(AddedAccountProviderRouteValues routeValues)
    {
        var accountId = _encodingService.Decode(routeValues.AccountHashedId, EncodingType.AccountId);
        var query = new GetAddedAccountProviderQuery(accountId, routeValues.AccountProviderId.Value);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<AddedAccountProviderViewModel>(result);

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route("{accountProviderId}/added")]
    public IActionResult Added(AddedAccountProviderViewModel model)
    {
        switch (model.Choice)
        {
            case "SetPermissions":
                return RedirectToAction(AccountProviders.ActionNames.Get, new GetAccountProviderRouteValues { AccountProviderId = model.AccountProviderId.Value, AccountHashedId = model.AccountHashedId });
            case "AddTrainingProvider":
                return RedirectToAction(AccountProviders.ActionNames.Find, new { model.AccountHashedId });
            case "GoToHomepage":
                return Redirect(_employerUrls.Account());
            default:
                throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
        }
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [HttpNotFoundForNullModel]
    [Route("{accountProviderId}/alreadyadded")]
    public async Task<IActionResult> AlreadyAdded(AlreadyAddedAccountProviderRouteValues routeValues)
    {
        var accountId = _encodingService.Decode(routeValues.AccountHashedId, EncodingType.AccountId);
        var query = new GetAddedAccountProviderQuery(accountId, routeValues.AccountProviderId.Value);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<AlreadyAddedAccountProviderViewModel>(result);

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route("{accountProviderId}/alreadyadded")]
    public IActionResult AlreadyAdded(AlreadyAddedAccountProviderViewModel model)
    {
        switch (model.Choice)
        {
            case "SetPermissions":
                return RedirectToAction(AccountProviders.ActionNames.Get, new { model.AccountHashedId });
            case "AddTrainingProvider":
                return RedirectToAction(AccountProviders.ActionNames.Find, new { model.AccountHashedId });
            default:
                throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
        }
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerOrViewerAccount))]
    [HttpNotFoundForNullModel]
    [Route("{accountProviderId}")]
    public async Task<IActionResult> Get(GetAccountProviderRouteValues routeValues)
    {
        var accountId = _encodingService.Decode(routeValues.AccountHashedId, EncodingType.AccountId);
        var query = new GetAccountProviderQuery(accountId, routeValues.AccountProviderId.Value);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<GetAccountProviderViewModel>(result);

        model.IsUpdatePermissionsOperationAuthorized = await _employerAccountAuthorizationHandler.CheckUserAccountAccess(User, EmployerUserRole.Owner);

        if (model?.AccountProvider.AccountLegalEntities.Count == 1)
        {
            return RedirectToAction(AccountProviderLegalEntities.ActionNames.Permissions, AccountProviderLegalEntities.ControllerName, new AccountProviderLegalEntityRouteValues {
                AccountHashedId = routeValues.AccountHashedId,
                AccountProviderId = model.AccountProvider.Id,
                AccountLegalEntityId = model.AccountProvider.AccountLegalEntities[0].Id
            });
        }

        return View(model);
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route("invitation/{correlationId}")]
    public async Task<IActionResult> Invitation(InvitationAccountProviderRouteValues routeValues)
    {
        HttpContext.Session.SetString("Invitation", "true");

        await CheckExistsAndUpsertNewUser();
        
        var invitation = await _mediator.Send(new GetInvitationByIdQuery(routeValues.CorrelationId.Value));

        var accountId = _encodingService.Decode(routeValues.AccountHashedId, EncodingType.AccountId);
        var verify = await _mediator.Send(new FindProviderToAddQuery(accountId, invitation.Invitation.Ukprn));

        if (verify.ProviderNotFound || verify.ProviderAlreadyAdded)
        {
            return RedirectToAction(AccountProviders.ActionNames.Index, new { routeValues.AccountHashedId });
        }

        var accountProviderId = await _mediator.Send(new AddAccountProviderCommand(accountId, invitation.Invitation.Ukprn, routeValues.UserRef.Value, routeValues.CorrelationId));

        return RedirectToAction(AccountProviders.ActionNames.Get, new { AccountProviderId = accountProviderId, routeValues.AccountHashedId });
    }
    
    
    private async Task CheckExistsAndUpsertNewUser()
    {
        var userId = HttpContext.User.Identities.FirstOrDefault().Claims
            .FirstOrDefault(c => c.Type == EmployerClaims.IdamsUserIdClaimTypeIdentifier)?.Value.ToString();
        var email = HttpContext.User.Identities.FirstOrDefault().Claims
            .FirstOrDefault(c => c.Type == EmployerClaims.IdamsUserEmailClaimTypeIdentifier)?.Value.ToString();
        var userResult = await _userAccountService.GetUserAccounts(userId, email);

        if (!string.IsNullOrEmpty(userResult?.FirstName))
        {
            await _mediator.Send(new CreateOrUpdateUserCommand(
                Guid.Parse(userResult.EmployerUserId),
                email,
                userResult.FirstName,
                userResult.LastName
            ));
        }
    }
}