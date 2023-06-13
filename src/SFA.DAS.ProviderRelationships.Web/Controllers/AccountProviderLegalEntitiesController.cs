using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Authorisation;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.Validation.Mvc.Attributes;

namespace SFA.DAS.ProviderRelationships.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerOwnerAccount))]
[Route("accounts/{accountHashedId}/providers/{accountProviderId}/legalentities/{accountLegalEntityId}")]
public class AccountProviderLegalEntitiesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IEmployerUrls _employerUrls;
    private readonly IEncodingService _encodingService;

    public AccountProviderLegalEntitiesController(IMediator mediator, IMapper mapper, IEmployerUrls employerUrls, IEncodingService encodingService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _employerUrls = employerUrls;
        _encodingService = encodingService;
    }

    [HttpGet]
    [HttpNotFoundForNullModel]
    [Route("")]
    public async Task<IActionResult> Permissions(AccountProviderLegalEntityRouteValues routeValues)
    {
        var accountId = _encodingService.Decode(routeValues.AccountHashedId, EncodingType.AccountId);
        var query = new GetAccountProviderLegalEntityQuery(accountId, routeValues.AccountProviderId.Value, routeValues.AccountLegalEntityId.Value);
        var result = await _mediator.Send(query);
        var model = _mapper.Map<AccountProviderLegalEntityViewModel>(result);

        return View(model);
    }

    [HttpPost]
    [Route("")]
    public IActionResult Permissions(AccountProviderLegalEntityViewModel model)
    {
        model.UserRef = User.GetUserRef();

        for (var index = 0; index < model.Permissions.Count; index++)
        {
            if (!model.Permissions[index].State.HasValue)
            {
                ModelState.AddModelError($"Permissions[{index}].State", $"Select the permissions you give {model.AccountProvider.ProviderName}");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return View(AccountProviderLegalEntities.ViewNames.Confirm, model);
    }

    [HttpPost]
    [Route(RouteNames.Confirm)]
    public async Task<IActionResult> Confirm(AccountProviderLegalEntityViewModel model, string command)
    {
        model.UserRef = User.GetUserRef();

        if (command == "Change")
        {
            return View(AccountProviderLegalEntities.ViewNames.Permissions, model);
        }

        if (model.Permissions[1].State == State.No && !model.Confirmation.HasValue)
        {
            ModelState.AddModelError("confirmation", $"Select if you want to change {model.AccountProvider.ProviderName} permissions");
            return View(AccountProviderLegalEntities.ViewNames.Confirm, model);
        }

        if (model.Confirmation.GetValueOrDefault() || model.Permissions[1].State != State.No)
        {
            var accountId = _encodingService.Decode(model.AccountHashedId, EncodingType.AccountId);
            var operations = model.Permissions.ToOperations();
            var update = new UpdatePermissionsCommand(accountId, model.AccountProviderId.Value, model.AccountLegalEntityId.Value, model.UserRef.Value, operations);

            await _mediator.Send(update);

            if (HttpContext.Session.GetString("Invitation").ToNullable<bool>() == true)
            {
                var provider = await _mediator.Send(new GetAccountProviderQuery(accountId, model.AccountProviderId.Value));
                return Redirect($"{_employerUrls.Account(model.AccountHashedId)}/addedprovider/{WebUtility.UrlEncode(provider.AccountProvider.ProviderName)}");
            }

            TempData["PermissionsChanged"] = true;
            TempData["ProviderName"] = model.AccountProvider.ProviderName;
            TempData["LegalEntityName"] = model.AccountLegalEntity.Name;
        }

        return RedirectToAction(AccountProviders.ActionNames.Index, AccountProviders.ControllerName, new { model.AccountHashedId });
    }
}