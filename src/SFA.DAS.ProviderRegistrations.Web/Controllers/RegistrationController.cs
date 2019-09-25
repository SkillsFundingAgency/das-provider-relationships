using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRegistrations.Web.Models;

namespace SFA.DAS.ProviderRegistrations.Web.Controllers
{
    [Route("registration")]
    public class RegistrationController : Controller
    {
        [HttpGet]
        [Route("startAccountSetup")]
        public IActionResult StartAccountSetup()
        {
            return View();
        }

        [HttpGet]
        [Route("NewEmployeruser")]
        public IActionResult NewEmployerUser()
        { 
            return View();
        }

        [HttpPost]
        [Route("NewEmployeruser")]
        [ValidateAntiForgeryToken]
        public IActionResult NewEmployeruser(NewEmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewEmployerUser", model);
            }

            return View("ReviewDetails", model);
        }

        [HttpPost]
        [Route("InviteEmployeruser")]
        [ValidateAntiForgeryToken]
        public IActionResult InviteEmployeruser(NewEmployerUserViewModel model, string command)
        {
            if (command == "Change")
            {
                return View("NewEmployerUser", model);
            }

            else return null;
        }
    }
}
