using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication.Interfaces;
using SFA.DAS.ProviderRelationships.Web.Extensions;

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

        [Route("signout")]
        public ActionResult SignOut()
        {
            // the current sign-out solution will only sign-out the user from the current sub-site and mya
            // a solution to sign out of every sub-site, would be to have 2 sign out endpoints per site
            // InitialiseSignOut & SignOut
            // InitialiseSignOut would redirect to SignOut of first subsite in list
            // SignOut for subsite would sign out that subsite (if currently signed in)
            // then call Signout of next sub-site in list
            // last in list would be MYA, which would do it's current signout
            // e.g. list A -> B -> C -> MYA
            // user signed into B & MYA, selects signout from B
            // sequence...
            // B.InitialiseSignOut -> redirect A.Signout
            // A Signout not signed in, just redirect B.Signout
            // B.Signout signed in so sign out, redirect C.Signout
            // C.Signout not signed in, just redirect MYA.Signout
            // MYA.Signout signout as normal

            _authenticationService.SignOutUser();

            var url = Url.EmployerPortalAction("service/signout");
            return new RedirectResult(url);
        }

        //[Authorize]
        //[HttpGet]
        //[Route("password/change")]
        //public ActionResult HandlePasswordChanged(bool userCancelled = false)
        //{
        //    var url = Url.EmployerPortalAction($"service/password/change?userCancelled={userCancelled}");
        //    return Redirect(url);
        //}

        //[Authorize]
        //[HttpGet]
        //[Route("email/change")]
        //public ActionResult HandleEmailChanged(bool userCancelled = false)
        //{
        //    //todo: employercommitments send email changes to /password/change! is that correct?
        //    var url = Url.EmployerPortalAction($"service/email/change?userCancelled={userCancelled}");
        //    return Redirect(url);
        //}
    }
}