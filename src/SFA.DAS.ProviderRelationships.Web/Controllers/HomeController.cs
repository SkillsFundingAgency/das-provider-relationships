using System.Web.Mvc;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnvironmentService _environmentService;
        private readonly IEmployerUrls _employerUrls;

        public HomeController(IEnvironmentService environmentService, IEmployerUrls employerUrls)
        {
            _environmentService = environmentService;
            _employerUrls = employerUrls;
        }

        [Route]
        public ActionResult Index()
        {
            if (_environmentService.IsCurrent(DasEnv.LOCAL))
            {
                return RedirectToAction("Index", "AccountProviders", new { accountHashedId = "JRML7V" });
            }

            return Redirect(_employerUrls.Homepage());
        }
    }
}