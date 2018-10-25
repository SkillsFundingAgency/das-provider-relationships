using System.Web.Mvc;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Extensions;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerRoles.Any)]
    public class HomeController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            if (ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL))
            {
                return RedirectToAction("Index", "Permissions", new { hashedAccountId = "ABC123" });
            }

            return Redirect(Url.EmployerPortalAction());
        }
    }
}