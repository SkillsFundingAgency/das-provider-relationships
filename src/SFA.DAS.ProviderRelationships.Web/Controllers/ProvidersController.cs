using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerRoles.Any)]
    [RoutePrefix("accounts/{accountHashedId}/providers")]
    public class ProvidersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProvidersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Route("search")]
        public ActionResult Search()
        {
            return View(new SearchProvidersViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("search")]
        public async Task<ActionResult> Search(SearchProvidersViewModel model)
        {
            var query = new SearchProvidersQuery(model.Ukprn);
            var response = await _mediator.Send(query);

            return RedirectToAction("Add", new { ukprn = response.Ukprn });
        }

        [HttpNotFoundForNullModel]
        [Route("add")]
        public async Task<ActionResult> Add(AddProviderParameters parameters)
        {
            var query = new GetProviderQuery(parameters.Ukprn.Value);
            var response = await _mediator.Send(query);
            var model = _mapper.Map<AddProviderViewModel>(response);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add")]
        public async Task<ActionResult> Add(AddProviderViewModel model)
        {
            switch (model.Choice)
            {
                case "Confirm":
                    var command = new AddAccountProviderCommand(model.AccountId.Value, model.UserRef.Value, model.Ukprn.Value);
                    var accountProviderId = await _mediator.Send(command);

                    return RedirectToAction("Added", new { accountProviderId = accountProviderId });
                case "ReEnterUkprn":
                    return RedirectToAction("Search");
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice));
            }
        }

        [HttpNotFoundForNullModel]
        [Route("{accountProviderId}/added")]
        public async Task<ActionResult> Added(AddedProviderParameters parameters)
        {
            var query = new GetAddedProviderQuery(parameters.AccountId.Value, parameters.AccountProviderId.Value);
            var response = await _mediator.Send(query);
            var model = _mapper.Map<AddedProviderViewModel>(response);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{accountProviderId}/added")]
        public ActionResult Added(AddedProviderViewModel model)
        {
            switch (model.Choice)
            {
                case "SetPermissions":
                    return RedirectToAction("Index", "Permissions", new { accountProviderId = model.AccountProviderId });
                case "AddTrainingProvider":
                    return RedirectToAction("Search");
                case "GoToHomepage":
                    return RedirectToAction("Index", "Home");
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice));
            }
        }
    }
}