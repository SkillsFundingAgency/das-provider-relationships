using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels.HealthCheck;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships)]
    [RoutePrefix("healthcheck")]
    public class HealthCheckController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HealthCheckController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Route]
        public async Task<ActionResult> Index()
        {
            var query = new GetHealthCheckQuery();
            var response = await _mediator.Send(query);
            var model = _mapper.Map<HealthCheckViewModel>(response);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route]
        public async Task<ActionResult> Index(HealthCheckRouteValues routeValues)
        {
            var command = new RunHealthCheckCommand(routeValues.UserRef.Value);
            
            await _mediator.Send(command);

            return RedirectToAction("Index");
        }
    }
}