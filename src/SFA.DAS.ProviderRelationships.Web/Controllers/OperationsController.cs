using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.RouteValues.Operations;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Operations;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships, EmployerUserRole.Owner)]
    [RoutePrefix("accounts/{accountHashedId}/providers/{accountProviderId}/legalentities/{accountLegalEntityId}/operations/{operationId}")]
    public class OperationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OperationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpNotFoundForNullModel]
        [Route("Set")]
        public async Task<ActionResult> Set(OperationRouteValue routeValue)
        {
            var model = await GetViewModel(routeValue);

            var helper = new OperationsHelper(routeValue, TempData, Url);
            helper.Set(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route]
        public async Task<ActionResult> Update(OperationRouteValue routeValue)
        {
            var model = await GetViewModel(routeValue);

            var helper = new OperationsHelper(routeValue, TempData, Url);
            helper.Update(model);

            if (!helper.IsValid())
            {
                ModelState.AddModelError("Operation", $"Select yes if you give {model.ProviderName} permission to {model.Operation.GetDescription().ToLower()}");
                return View("Set", model);
            }

            var next = helper.Next();

            if (next == Operation.NotSet)
            {
                return RedirectToAction("Confirm", new OperationRouteValue {
                    AccountProviderId = routeValue.AccountProviderId.Value,
                    AccountLegalEntityId = routeValue.AccountLegalEntityId.Value
                });
            }

            return RedirectToAction("Set", "Operations", new OperationRouteValue {
                OperationId = (short)next,
                AccountProviderId = routeValue.AccountProviderId.Value,
                AccountLegalEntityId = routeValue.AccountLegalEntityId.Value
            });

        }


        [HttpNotFoundForNullModel]
        [Route("confirm")]
        public async Task<ActionResult> Confirm(OperationRouteValue routeValue)
        {
            var helper = new OperationsHelper(routeValue, TempData, Url);

            if (helper.Changes == null)
            {
                return RedirectToAction("Get", new GetAccountProviderLegalEntityRouteValues { AccountProviderId = routeValue.AccountProviderId.Value, AccountLegalEntityId = routeValue.AccountLegalEntityId.Value });
            }

            var model = await GetConfirmViewModel(routeValue);
            helper.Confirm(model);
;
            return View(model);
        }

        private async Task<UpdateOperationViewModel> GetViewModel(OperationRouteValue routeValue)
        {
            var query = new GetAccountProviderLegalEntityQuery(routeValue.AccountId.Value, routeValue.AccountProviderId.Value, routeValue.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);

            return _mapper.Map<UpdateOperationViewModel>(result);
        }

        private async Task<ConfirmOperationViewModel> GetConfirmViewModel(OperationRouteValue routeValue)
        {
            var query = new GetAccountProviderLegalEntityQuery(routeValue.AccountId.Value, routeValue.AccountProviderId.Value, routeValue.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);
            return _mapper.Map<ConfirmOperationViewModel>(result);
        }
    }
}