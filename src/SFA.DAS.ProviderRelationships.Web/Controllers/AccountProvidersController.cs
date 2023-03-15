using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Validation;
using SFA.DAS.ProviderRelationships.Web.Authorisation;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;
using SFA.DAS.ProviderRelationships.Web.Extensions;
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

    public AccountProvidersController(
        IMediator mediator,
        IMapper mapper,
        IEmployerUrls employerUrls,
        IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler,
        IEncodingService encodingService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _employerUrls = employerUrls;
        _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
        _encodingService = encodingService;
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerOrViewerAccount))]
    [Route("")]
    public async Task<IActionResult> Index(string accountHashedId)
    {
        var accountId = _encodingService.Decode(accountHashedId, EncodingType.AccountId);
        var query = new GetAccountProvidersQuery(accountId);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<AccountProvidersViewModel>(result);

        return View(model);
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route("find")]
    public async Task<IActionResult> Find()
    {
        var query = new GetAllProvidersQuery();
        var result = await _mediator.Send(query);
        var model = _mapper.Map<FindProviderViewModel>(result);
        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route("find")]
    public async Task<IActionResult> Find(FindProviderEditModel model)
    {
        var accountId = _encodingService.Decode(model.AccountHashedId, EncodingType.AccountId);
        var ukprn = long.Parse(model.Ukprn);
        var query = new FindProviderToAddQuery(accountId, ukprn);

        var result = await _mediator.Send(query);

        if (result.ProviderNotFound)
        {
            ModelState.AddModelError(nameof(model.Ukprn), ErrorMessages.RequiredUkprn);

            return RedirectToAction("Find", new { model.AccountHashedId });
        }

        if (result.ProviderAlreadyAdded)
        {
            return RedirectToAction("AlreadyAdded", new AlreadyAddedAccountProviderRouteValues { AccountProviderId = result.AccountProviderId.Value, AccountHashedId = model.AccountHashedId });
        }

        return RedirectToAction("Add", new { result.Ukprn, model.AccountHashedId });
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [HttpNotFoundForNullModel]
    [Route("add")]
    public async Task<IActionResult> Add(AddAccountProviderRouteValues routeValues)
    {
        var query = new GetProviderToAddQuery(routeValues.Ukprn.Value);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<AddAccountProviderViewModel>(result);

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route("add")]
    public async Task<IActionResult> Add(AddAccountProviderViewModel model)
    {
        model.UserRef = User.GetUserRef();
        model.AccountId = _encodingService.Decode(model.AccountHashedId, EncodingType.AccountId);

        switch (model.Choice)
        {
            case "Confirm":
                var command = new AddAccountProviderCommand(model.AccountId.Value, model.Ukprn.Value, model.UserRef.Value);
                var accountProviderId = await _mediator.Send(command);

                return RedirectToAction("Added", new AddedAccountProviderRouteValues { AccountProviderId = accountProviderId, AccountHashedId = model.AccountHashedId });
            case "ReEnterUkprn":
                return RedirectToAction("Find", new { model.AccountHashedId });
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
                return RedirectToAction("Get", new GetAccountProviderRouteValues { AccountProviderId = model.AccountProviderId.Value });
            case "AddTrainingProvider":
                return RedirectToAction("Find");
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
                return RedirectToAction("Get");
            case "AddTrainingProvider":
                return RedirectToAction("Find");
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
        var query = new GetAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<GetAccountProviderViewModel>(result);

        model.IsUpdatePermissionsOperationAuthorized = _employerAccountAuthorizationHandler.CheckUserAccountAccess(User, EmployerUserRole.Owner);

        if (model?.AccountProvider.AccountLegalEntities.Count == 1)
        {
            return RedirectToAction("Permissions", "AccountProviderLegalEntities", new AccountProviderLegalEntityRouteValues { AccountProviderId = model.AccountProvider.Id, AccountLegalEntityId = model.AccountProvider.AccountLegalEntities[0].Id });
        }

        return View(model);
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
    [Route("invitation/{correlationId}")]
    public async Task<IActionResult> Invitation(InvitationAccountProviderRouteValues routeValues)
    {
        HttpContext.Session.SetString("Invitation", "true");

        var invitation = await _mediator.Send(new GetInvitationByIdQuery(routeValues.CorrelationId.Value));

        var verify = await _mediator.Send(new FindProviderToAddQuery(routeValues.AccountId.Value, invitation.Invitation.Ukprn));

        if (verify.ProviderNotFound || verify.ProviderAlreadyAdded)
        {
            return RedirectToAction("Index");
        }

        var accountProviderId = await _mediator.Send(new AddAccountProviderCommand(routeValues.AccountId.Value, invitation.Invitation.Ukprn, routeValues.UserRef.Value, routeValues.CorrelationId));

        return RedirectToAction("Get", new { AccountProviderId = accountProviderId });
    }
}