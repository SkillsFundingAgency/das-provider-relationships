using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}