using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships, EmployerRole.Any)]
    [RoutePrefix("accounts/{accountHashedId}/providers/{accountProviderId}/legalentities")]
    public class AccountProviderLegalEntitiesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AccountProviderLegalEntitiesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpNotFoundForNullModel]
        [Route("{accountLegalEntityId}")]
        public async Task<ActionResult> Get(GetAccountProviderLegalEntityRouteValues routeValues)
        {
            var query = new GetAccountProviderLegalEntityQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value, routeValues.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<GetAccountProviderLegalEntityViewModel>(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{accountLegalEntityId}")]
        public ActionResult Get(GetAccountProviderLegalEntityViewModel model)
        {
            return RedirectToAction("Update");
        }
    }
}