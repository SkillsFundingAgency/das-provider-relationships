using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Home;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerRoles.Any)]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEnvironment _environment;
        private readonly IEmployerUrls _employerUrls;

        public HomeController(IMediator mediator, IMapper mapper, IEnvironment environment, IEmployerUrls employerUrls)
        {
            _mediator = mediator;
            _mapper = mapper;
            _environment = environment;
            _employerUrls = employerUrls;
        }

        [Route]
        public ActionResult Local()
        {
            if (_environment.IsCurrent(DasEnv.LOCAL))
                return RedirectToAction("Index", new { accountHashedId = "JRML7V" });

            return Redirect(_employerUrls.Homepage());
        }
        
        [Route("accounts/{accountHashedId}")]
        public async Task<ActionResult> Index(AccountProvidersRouteValues routeValues)
        {
            var query = new GetAccountProvidersQuery(routeValues.AccountId.Value);
            var response = await _mediator.Send(query);

            var model = _mapper.Map<AccountProvidersViewModel>(response);
            
            return View(model);
        }
    }
}