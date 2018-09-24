using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            return View();
        }
    }
}