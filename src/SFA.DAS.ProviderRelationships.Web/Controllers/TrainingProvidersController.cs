using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using SFA.DAS.ProviderRelationships.Web.ViewModels;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("trainingproviders")]
    public class TrainingProvidersController : Controller
    {
        private readonly IMediator _mediator;

        public TrainingProvidersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("search")]
        public ActionResult Search()
        {
            return View(new SearchTrainingProvidersViewModel());
        }

        [HttpPost]
        [Route("search")]
        public async Task<ActionResult> Search(SearchTrainingProvidersViewModel model)
        {
            var response = await _mediator.Send(model.SearchTrainingProvidersQuery);

            return RedirectToAction("Add", new { ukprn = response.TrainingProvider.Ukprn });
        }

        [Route("add")]
        public ActionResult Add()
        {
            return View();
        }
    }
}