using System.Security.Policy;
using System.Web.Mvc;
using Microsoft.Azure;
using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("service")]
    public class ServiceController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public ServiceController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // GET: Service

        [Route("signout")]
        public ActionResult SignOut()
        {
            _authenticationService.SignOutUser();

            //todo: we'll probably want some sort of url helper at some point
            return new RedirectResult($"{CloudConfigurationManager.GetSetting("MyaBaseUrl")}service/signout");
        }

        //[Authorize]
        //[HttpGet]
        //[Route("password/change")]
        //public ActionResult HandlePasswordChanged(bool userCancelled = false)
        //{
        //    var url = Url.ExternalMyaUrlAction("service", $"password/change?userCancelled={userCancelled}", true);
        //    return Redirect(url);
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("email/change")]
        //public ActionResult HandleEmailChanged(bool userCancelled = false)
        //{
        //    var url = Url.ExternalMyaUrlAction("service", $"password/change?userCancelled={userCancelled}", true);
        //    return Redirect(url);
        //}
    }
}