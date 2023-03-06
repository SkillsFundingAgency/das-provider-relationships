﻿using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.Validation.Mvc.Attributes;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [Authorize(Policy = EmployerUserRole.Owner)]
    [Route("accounts/{accountHashedId}/providers/{accountProviderId}/legalentities/{accountLegalEntityId}")]
    public class AccountProviderLegalEntitiesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmployerUrls _employerUrls;

        public AccountProviderLegalEntitiesController(IMediator mediator, IMapper mapper, IEmployerUrls employerUrls)
        {
            _mediator = mediator;
            _mapper = mapper;
            _employerUrls = employerUrls;
        }

        [HttpGet]
        [HttpNotFoundForNullModel]
        [Route("")]
        public async Task<IActionResult> Permissions(AccountProviderLegalEntityRouteValues routeValues)
        {
            var query = new GetAccountProviderLegalEntityQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value, routeValues.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AccountProviderLegalEntityViewModel>(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("")]
        public IActionResult Permissions(AccountProviderLegalEntityViewModel model)
        {
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

            return View("Confirm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Confirm")]
        public async Task<IActionResult> Confirm(AccountProviderLegalEntityViewModel model, string command)
        {
            if (command == "Change")
            {
                return View("Permissions", model);
            }

            if (model.Permissions[1].State == State.No && !model.Confirmation.HasValue)
            {
                ModelState.AddModelError("confirmation", $"Select if you want to change {model.AccountProvider.ProviderName} permissions");
                return View("Confirm", model);
            }

            if (model.Confirmation.GetValueOrDefault() || model.Permissions[1].State != State.No)
            {
                var operations = model.Permissions.ToOperations();
                var update = new UpdatePermissionsCommand(model.AccountId.Value, model.AccountProviderId.Value, model.AccountLegalEntityId.Value, model.UserRef.Value, operations);

                await _mediator.Send(update);

                if (HttpContext.Session.GetString("Invitation").ToNullable<bool>() == true)
                {
                    var provider = await _mediator.Send(new GetAccountProviderQuery(model.AccountId.Value, model.AccountProviderId.Value));
                    return Redirect($"{_employerUrls.Account()}/addedprovider/{WebUtility.UrlEncode(provider.AccountProvider.ProviderName)}");
                }

                TempData["PermissionsChanged"] = true;
                TempData["ProviderName"] = model.AccountProvider.ProviderName;
                TempData["LegalEntityName"] = model.AccountLegalEntity.Name;
            }

            return RedirectToAction("Index", "AccountProviders");
        }
    }
}