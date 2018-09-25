using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using SFA.DAS.ProviderRelationships.Web.ViewModels;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("accounts/{hashedAccountId}/providers")]
    public class ProvidersController : Controller
    {
        private readonly IMediator _mediator;

        public ProvidersController(IMediator mediator)
        {
            _mediator = mediator;
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
            var response = await _mediator.Send(model.SearchProvidersQuery);

            return RedirectToAction("Add", new { ukprn = response.Provider.Ukprn });
        }

        [Route("add")]
        public ActionResult Add()
        {
            return View();
        }
    }
}