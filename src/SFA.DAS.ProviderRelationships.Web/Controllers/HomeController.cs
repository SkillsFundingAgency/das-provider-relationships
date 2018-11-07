using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Home;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerRoles.Any)]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HomeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Route]
        public ActionResult Local()
        {
            if (ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL))
            {
                return RedirectToAction("Index", new { accountHashedId = "JRML7V" });
            }

            return Redirect(Url.EmployerPortalAction());
        }
        
        [Route("accounts/{accountHashedId}")]
        public async Task<ActionResult> Index(AccountProvidersRouteValues routeValues)
        {
            var query = new GetAddedProvidersQuery(routeValues.AccountId.Value);
            var response = await _mediator.Send(query);

            var model = _mapper.Map<AccountProvidersViewModel>(response);
            
            return View(model);
        }
    }
}