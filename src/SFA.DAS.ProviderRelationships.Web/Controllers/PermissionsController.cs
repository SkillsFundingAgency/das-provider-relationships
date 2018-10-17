using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [Authorize]
    [RoutePrefix("accounts/{hashedAccountId}")]
    public class PermissionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PermissionsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Route]
        public ActionResult Index()
        {
            return View();
        }

        [Route("providers/search")]
        public ActionResult SearchProviders()
        {
            return View(new SearchProvidersViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("providers/search")]
        public async Task<ActionResult> SearchProviders(SearchProvidersViewModel model)
        {
            var response = await _mediator.Send(model.SearchProvidersQuery);

            return RedirectToAction("AddProvider", new { ukprn = response.Ukprn });
        }

        [HttpNotFoundForNullModel]
        [Route("providers/add")]
        public async Task<ActionResult> AddProvider(GetProviderQuery query)
        {
            var response = await _mediator.Send(query);
            var model = _mapper.Map<AddProviderViewModel>(response);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("providers/add")]
        public ActionResult AddProvider(AddProviderViewModel model)
        {
            switch (model.Choice)
            {
                case "Confirm":
                    return RedirectToAction("Index", new { ukprn = model.Ukprn });
                case "ReEnterUkprn":
                    return RedirectToAction("SearchProviders");
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice));
            }
        }
    }
}