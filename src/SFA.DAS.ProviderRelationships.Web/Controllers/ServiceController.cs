﻿using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("service")]
    public class ServiceController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthenticationUrls _authenticationUrls;

        public ServiceController(IAuthenticationService authenticationService, IAuthenticationUrls authenticationUrls)
        {
            _authenticationService = authenticationService;
            _authenticationUrls = authenticationUrls;
        }

        [Route("signout")]
        public ActionResult SignOut()
        {
            _authenticationService.SignOutUser();

            return new RedirectResult(_authenticationUrls.LogoutEndpoint);
        }

        [Route("signoutcleanup")]
        public void SignOutCleanup()
        {
            _authenticationService.SignOutUser();
        }
    }
}