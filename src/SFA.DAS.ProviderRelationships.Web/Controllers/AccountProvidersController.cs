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
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.Validation.Mvc.Attributes;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [Route("accounts/{accountHashedId}/providers")]
    public class AccountProvidersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmployerUrls _employerUrls;
        private readonly IEmployerAccountAuthorizationHandler _employerAccountAuthorizationHandler;
        private readonly AuthorizationHandlerContext _context;

        public AccountProvidersController(IMediator mediator, IMapper mapper, IEmployerUrls employerUrls, IEmployerAccountAuthorizationHandler employerAccountAuthorizationHandler, AuthorizationHandlerContext context)
        {
            _mediator = mediator;
            _mapper = mapper;
            _employerUrls = employerUrls;
            _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = EmployerUserRole.Any)]
        [Route("")]
        public async Task<IActionResult> Index(AccountProvidersRouteValues routeValues)
        {
            var query = new GetAccountProvidersQuery(routeValues.AccountId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AccountProvidersViewModel>(result);

            return View(model);
        }


        public IActionResult AccountProvidersWithSingleOrganisation(AccountProvidersViewModel model)
        {
            return PartialView(model);
        }

        public IActionResult AccountProvidersWithMultipleOrganisation(AccountProvidersViewModel model)
        {
            return PartialView(model);
        }

        [HttpGet]
        [Authorize(Policy = EmployerUserRole.Owner)]
        [Route("find")]
        public async Task<IActionResult> Find()
        {
            var query = new GetAllProvidersQuery();
            var result = await _mediator.Send(query);
            var model = _mapper.Map<FindProviderViewModel>(result);
            return View(model);
        }

        [Authorize(Policy = EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("find")]
        public async Task<IActionResult> Find(FindProviderEditModel model)
        {
            var ukprn = long.Parse(model.Ukprn);
            var query = new FindProviderToAddQuery(model.AccountId.Value, ukprn);

            var result = await _mediator.Send(query);

            if (result.ProviderNotFound)
            {
                ModelState.AddModelError(nameof(model.Ukprn), ErrorMessages.RequiredUkprn);

                return RedirectToAction("Find");
            }

            if (result.ProviderAlreadyAdded)
            {
                return RedirectToAction("AlreadyAdded", new AlreadyAddedAccountProviderRouteValues { AccountProviderId = result.AccountProviderId.Value });
            }

            return RedirectToAction("Add", new AddAccountProviderRouteValues { Ukprn = result.Ukprn });
        }

        [HttpGet]
        [Authorize(Policy = EmployerUserRole.Owner)]
        [HttpNotFoundForNullModel]
        [Route("add")]
        public async Task<IActionResult> Add(AddAccountProviderRouteValues routeValues)
        {
            var query = new GetProviderToAddQuery(routeValues.Ukprn.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AddAccountProviderViewModel>(result);

            return View(model);
        }

        [Authorize(Policy = EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add")]
        public async Task<IActionResult> Add(AddAccountProviderViewModel model)
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

        [HttpGet]
        [Authorize(Policy = EmployerUserRole.Owner)]
        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}/added")]
        public async Task<IActionResult> Added(AddedAccountProviderRouteValues routeValues)
        {
            var query = new GetAddedAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AddedAccountProviderViewModel>(result);

            return View(model);
        }

        [Authorize(Policy = EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [Authorize(Policy = EmployerUserRole.Owner)]
        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}/alreadyadded")]
        public async Task<IActionResult> AlreadyAdded(AlreadyAddedAccountProviderRouteValues routeValues)
        {
            var query = new GetAddedAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<AlreadyAddedAccountProviderViewModel>(result);

            return View(model);
        }

        [Authorize(Policy = EmployerUserRole.Owner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [Authorize(Policy = EmployerUserRole.Any)]
        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}")]
        public async Task<IActionResult> Get(GetAccountProviderRouteValues routeValues)
        {
            var query = new GetAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<GetAccountProviderViewModel>(result);
           
            model.IsUpdatePermissionsOperationAuthorized = _employerAccountAuthorizationHandler.IsEmployerAuthorised(_context, EmployerUserAuthorisationRole.Owner);

            if (model?.AccountProvider.AccountLegalEntities.Count == 1)
            {
                return RedirectToAction("Permissions", "AccountProviderLegalEntities", new AccountProviderLegalEntityRouteValues { AccountProviderId = model.AccountProvider.Id, AccountLegalEntityId = model.AccountProvider.AccountLegalEntities[0].Id });
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = EmployerUserRole.Owner)]
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
}