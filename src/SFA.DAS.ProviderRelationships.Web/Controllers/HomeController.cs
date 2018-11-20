using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnvironment _environment;
        private readonly IEmployerUrls _employerUrls;

        public HomeController(IEnvironment environment, IEmployerUrls employerUrls)
        {
            _environment = environment;
            _employerUrls = employerUrls;
        }

        [Route]
        public ActionResult Index()
        {
            if (_environment.IsCurrent(DasEnv.LOCAL))
            {
                return RedirectToAction("Index", "AccountProviders", new { accountHashedId = "JRML7V" });
            }

            return Redirect(_employerUrls.Homepage());
        }
    }
}