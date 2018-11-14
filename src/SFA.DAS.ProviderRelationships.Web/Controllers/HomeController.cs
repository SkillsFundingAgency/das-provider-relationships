using System.Web.Mvc;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Extensions;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships, EmployerRole.Any)]
    public class HomeController : Controller
    {
        [Route]
        public ActionResult Local()
        {
            if (ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL))
            {
                return RedirectToAction("Index", new { accountHashedId = "JRML7V" });
            }

            return Redirect(Url.EmployerPortalAction());
        }
        
        [Route("accounts/{accountHashedId}")]
        public ActionResult Index()
        {
            return View();
        }
    }
}