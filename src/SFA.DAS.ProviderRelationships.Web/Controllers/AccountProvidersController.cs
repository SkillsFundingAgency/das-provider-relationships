using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Validation;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships, EmployerRole.Any)]
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
        
        [Route]
        public async Task<ActionResult> Index(AccountProvidersRouteValues routeValues)
        {
            var query = new GetAccountProvidersQuery(routeValues.AccountId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AccountProvidersViewModel>(result);
            
            return View(model);
        }

        [Route("find")]
        public ActionResult Find()
        {
            return View(new FindProviderViewModel());
        }

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

        [HttpNotFoundForNullModel]
        [Route("add")]
        public async Task<ActionResult> Add(AddAccountProviderRouteValues routeValues)
        {
            var query = new GetProviderToAddQuery(routeValues.Ukprn.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AddAccountProviderViewModel>(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add")]
        public async Task<ActionResult> Add(AddAccountProviderViewModel model)
        {
            switch (model.Choice)
            {
                case "Confirm":
                    var command = new AddAccountProviderCommand(model.AccountId.Value, model.UserRef.Value, model.Ukprn.Value);
                    var accountProviderId = await _mediator.Send(command);

                    return RedirectToAction("Added", new AddedAccountProviderRouteValues { AccountProviderId = accountProviderId });
                case "ReEnterUkprn":
                    return RedirectToAction("Find");
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
            }
        }

        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}/added")]
        public async Task<ActionResult> Added(AddedAccountProviderRouteValues routeValues)
        {
            var query = new GetAddedAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AddedAccountProviderViewModel>(result);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{accountProviderId}/added")]
        public ActionResult Added(AddedAccountProviderViewModel model)
        {
            switch (model.Choice)
            {
                case "SetPermissions":
                    return RedirectToAction("Get", new GetAccountProviderRouteValues { AccountProviderId = model.AccountProviderId });
                case "AddTrainingProvider":
                    return RedirectToAction("Find");
                case "GoToHomepage":
                    return Redirect(_employerUrls.Account());
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
            }
        }

        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}/alreadyadded")]
        public async Task<ActionResult> AlreadyAdded(AlreadyAddedAccountProviderRouteValues routeValues)
        {
            var query = new GetAddedAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AlreadyAddedAccountProviderViewModel>(result);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{accountProviderId}/alreadyadded")]
        public ActionResult AlreadyAdded(AlreadyAddedAccountProviderViewModel model)
        {
            switch (model.Choice)
            {
                case "SetPermissions":
                    return RedirectToAction("Get", new GetAccountProviderRouteValues { AccountProviderId = model.AccountProviderId });
                case "AddTrainingProvider":
                    return RedirectToAction("Find");
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
            }
        }

        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}")]
        public async Task<ActionResult> Get(GetAccountProviderRouteValues routeValues)
        {
            var query = new GetAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<GetAccountProviderViewModel>(result);
            
            return View(model);
        }
    }
}