using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("accounts/{hashedAccountId}")]
    [Authorize]
    public class HomeController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            return View();
        }
    }
}