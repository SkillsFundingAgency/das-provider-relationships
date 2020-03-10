using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd;
using SFA.DAS.ProviderRelationships.Validation;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships)]
    [RoutePrefix("accounts/{accountHashedId}/providers")]
    public class AccountProvidersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmployerUrls _employerUrls;

        public AccountProvidersController(IMediator mediator, IMapper mapper, IEmployerUrls employerUrls)
        {
            _mediator = mediator;
            _mapper = mapper;
            _employerUrls = employerUrls;
        }
        
        [DasAuthorize(EmployerUserRole.Any)]
        [Route]
        public async Task<ActionResult> Index(AccountProvidersRouteValues routeValues)
        {
            var query = new GetAccountProvidersQuery(routeValues.AccountId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AccountProvidersViewModel>(result);
            
            return View(model);
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [Route("find")]
        public ActionResult Find()
        {
            return View(new FindProviderViewModel());
        }
        
        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("find")]
        public async Task<ActionResult> Find(FindProviderViewModel model)
        {
            var ukprn = long.Parse(model.Ukprn);
            var query = new FindProviderToAddQuery(model.AccountId.Value, ukprn);
            var result = await _mediator.Send(query);

            if (result.ProviderNotFound)
            {
                ModelState.AddModelError(nameof(model.Ukprn), ErrorMessages.InvalidUkprn);

                return RedirectToAction("Find");
            }

            if (result.ProviderAlreadyAdded)
            {
                return RedirectToAction("AlreadyAdded", new AlreadyAddedAccountProviderRouteValues { AccountProviderId = result.AccountProviderId.Value });
            }

            return RedirectToAction("Add", new AddAccountProviderRouteValues { Ukprn = result.Ukprn });
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpNotFoundForNullModel]
        [Route("add")]
        public async Task<ActionResult> Add(AddAccountProviderRouteValues routeValues)
        {
            var query = new GetProviderToAddQuery(routeValues.Ukprn.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AddAccountProviderViewModel>(result);

            return View(model);
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add")]
        public async Task<ActionResult> Add(AddAccountProviderViewModel model)
        {           
            switch (model.Choice)
            {
                case "Confirm":
                    var command = new AddAccountProviderCommand(model.AccountId.Value, model.Ukprn.Value, model.UserRef.Value);
                    var accountProviderId = await _mediator.Send(command);

                    return RedirectToAction("Added", new AddedAccountProviderRouteValues { AccountProviderId = accountProviderId });
                case "ReEnterUkprn":
                    return RedirectToAction("Find");
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
            }
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}/added")]
        public async Task<ActionResult> Added(AddedAccountProviderRouteValues routeValues)
        {
            var query = new GetAddedAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AddedAccountProviderViewModel>(result);
            
            return View(model);
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{accountProviderId}/added")]
        public ActionResult Added(AddedAccountProviderViewModel model)
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

        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}/alreadyadded")]
        public async Task<ActionResult> AlreadyAdded(AlreadyAddedAccountProviderRouteValues routeValues)
        {
            var query = new GetAddedAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AlreadyAddedAccountProviderViewModel>(result);

            return View(model);
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{accountProviderId}/alreadyadded")]
        public ActionResult AlreadyAdded(AlreadyAddedAccountProviderViewModel model)
        {
            switch (model.Choice)
            {
                case "SetPermissions":
                    return RedirectToAction("Get", new GetAccountProviderRouteValues { AccountProviderId = model.AccountProviderId.Value });
                case "AddTrainingProvider":
                    return RedirectToAction("Find");
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
            }
        }

        [DasAuthorize(EmployerUserRole.Any)]
        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}")]
        public async Task<ActionResult> Get(GetAccountProviderRouteValues routeValues)
        {
            var query = new GetAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<GetAccountProviderViewModel>(result);

            if (model?.AccountProvider.AccountLegalEntities.Count == 1)
            {
                return RedirectToAction("Get", "AccountProviderLegalEntities", new GetAccountProviderLegalEntityRouteValues { AccountProviderId = model.AccountProvider.Id, AccountLegalEntityId = model.AccountProvider.AccountLegalEntities[0].Id });
            }
            
            return View(model);
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        [Route("invitation/{correlationId}")]
        public async Task<ActionResult> Invitation(InvitationAccountProviderRouteValues routeValues)
        {
            Session["Invitation"] = true;

            var invitation = await _mediator.Send(new GetInvitationByIdQuery(routeValues.CorrelationId.Value));

            var verify = await _mediator.Send(new FindProviderToAddQuery(routeValues.AccountId.Value, invitation.Invitation.Ukprn));

            if (verify.ProviderNotFound || verify.ProviderAlreadyAdded)
            {
                return RedirectToAction("Index");
            }

            var accountProviderId = await _mediator.Send(new AddAccountProviderCommand(routeValues.AccountId.Value, invitation.Invitation.Ukprn, routeValues.UserRef.Value, routeValues.CorrelationId));
            
            return RedirectToAction("Get", new GetAccountProviderRouteValues { AccountProviderId = accountProviderId });
        }
    }
}