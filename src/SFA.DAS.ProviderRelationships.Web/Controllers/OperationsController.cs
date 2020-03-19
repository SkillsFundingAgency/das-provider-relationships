using System;
using System.Collections.Generic;
using System.Linq;
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
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
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
        private readonly IEmployerUrls _employerUrls;

        public OperationsController(IMediator mediator, IMapper mapper, IEmployerUrls employerUrls)
        {
            _mediator = mediator;
            _mapper = mapper;
            _employerUrls = employerUrls;
        }

        [HttpNotFoundForNullModel]
        [Route("Set")]
        public async Task<ActionResult> Set(OperationRouteValue routeValue)
        {
            var currentOperation = routeValue.CurrentOperation;

            var allowedOperations = GetAllowedOperations();

            if (currentOperation.Equals(Operation.NotSet))
            {
                TempData.Clear();
                currentOperation = allowedOperations.First();
            }

            var model = await GetViewModel(routeValue);
            model.Operation = currentOperation;
            var changes = TempData.Get<List<OperationViewModel>>() ?? new List<OperationViewModel>();

            model.IsEnabled = model.IsEditMode ? model.Operations.FirstOrDefault(o => o.Value.Equals(currentOperation))?.IsEnabled : changes.FirstOrDefault(o => o.Value.Equals(currentOperation))?.IsEnabled;

            if (changes.Any(o => o.Value.Equals(currentOperation)))
            {
                model.IsEnabled = changes.First(o => o.Value.Equals(currentOperation)).IsEnabled;
            }
            else
            {
                model.IsEnabled = model.Operations.FirstOrDefault(o => o.Value.Equals(currentOperation))?.IsEnabled;
                changes.Add(new OperationViewModel { Value = currentOperation, IsEnabled = model.IsEnabled });
            }

            model.Operations = changes;

            if((Operation)Enum.Parse(typeof(Operation), routeValue.OperationId.ToString()) == Operation.NotSet || currentOperation == allowedOperations.First())
            {
                model.BackLink = Url.Action("Index", "AccountProviders");
            }
            else
            {
                model.BackLink = Url.Action("Set", "Operations",
                    new OperationRouteValue {
                        AccountProviderId = routeValue.AccountProviderId,
                        AccountLegalEntityId = routeValue.AccountLegalEntityId,
                        OperationId = (short)allowedOperations.Previous(currentOperation)
                    });
            }

            TempData.Set(changes);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route]
        public async Task<ActionResult> Update(OperationRouteValue routeValue)
        {
            var model = await GetViewModel(routeValue);

            if (routeValue.CurrentOperation == Operation.NotSet || !routeValue.IsEnabled.HasValue)
            {
                ModelState.AddModelError("Operation", $"Select yes if you give {model.ProviderName} permission to {model.Operation.GetDescription().ToLower()}");
                return View("Set", model);
            }

            var allowedOperations = GetAllowedOperations();
            // TODO : validate current operation is an allowed one

            var changes = TempData.Get<List<OperationViewModel>>();


            changes.First(o => o.Value.Equals(routeValue.CurrentOperation)).IsEnabled = routeValue.IsEnabled;
            TempData.Set(changes);

            if (!allowedOperations.IsLast(routeValue.CurrentOperation) && !routeValue.IsEditMode)
            {
                model.Operation = allowedOperations.Next(routeValue.CurrentOperation);

                return RedirectToAction("Set", "Operations", new OperationRouteValue {
                    OperationId = (short)model.Operation,
                    AccountProviderId = routeValue.AccountProviderId.Value,
                    AccountLegalEntityId = routeValue.AccountLegalEntityId.Value
                });
            }

            return RedirectToAction("Confirm", new OperationRouteValue {
                AccountProviderId = routeValue.AccountProviderId.Value,
                AccountLegalEntityId = routeValue.AccountLegalEntityId.Value
            });
        }
   

        [HttpNotFoundForNullModel]
        [Route("confirm")]
        public async Task<ActionResult> Confirm(OperationRouteValue routeValue)
        {
            var changes = TempData.Get<List<OperationViewModel>>();

            if (changes == null)
            {
                return RedirectToAction("Get", new GetAccountProviderLegalEntityRouteValues { AccountProviderId = routeValue.AccountProviderId.Value, AccountLegalEntityId = routeValue.AccountLegalEntityId.Value });
            }

            var query = new GetAccountProviderLegalEntityQuery(routeValue.AccountId.Value, routeValue.AccountProviderId.Value, routeValue.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<ConfirmOperationViewModel>(result);

            model.Operations = changes;

            var previousOperation = GetAllowedOperations().Last();
            if (previousOperation == Operation.NotSet)
            {
                model.BackLink = Url.Action("Index", "AccountProviders");
            }
            else
            {
                model.BackLink = Url.Action("Set", "Operations",
                    new OperationRouteValue {
                        AccountProviderId = routeValue.AccountProviderId,
                        AccountLegalEntityId = routeValue.AccountLegalEntityId,
                        OperationId = (short)previousOperation
                    });
            }

            TempData.Set(changes);
            return View(model);
        }               

        private async Task<UpdateOperationViewModel> GetViewModel(OperationRouteValue routeValue)
        {
            var query = new GetAccountProviderLegalEntityQuery(routeValue.AccountId.Value, routeValue.AccountProviderId.Value, routeValue.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);

            return _mapper.Map<UpdateOperationViewModel>(result);
        }

        private static List<Operation> GetAllowedOperations()
        {            
            var allowedList = new List<Operation>();

            foreach (short value in Enum.GetValues(typeof(Operation)))
            {
                if (value != (short)Operation.NotSet)
                {
                    allowedList.Add((Operation)Enum.Parse(typeof(Operation), value.ToString()));
                }
            }

            return allowedList;
        }
    }
}