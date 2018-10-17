using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [Authorize]
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
        public async Task<ActionResult> Index(GetHealthCheckQuery query)
        {
            var response = await _mediator.Send(query);
            var model = _mapper.Map<HealthCheckViewModel>(response);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route]
        public async Task<ActionResult> Index(RunHealthCheckCommand command)
        {
            await _mediator.Send(command);

            return RedirectToAction("Index");
        }
    }
}