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
        [Route("newEmployeruser")]
        public IActionResult NewEmployerUser()
        {
            return View();
        }

        [HttpPost]
        [Route("newEmployeruser")]
        [ValidateAntiForgeryToken]
        public IActionResult NewEmployerUser(NewEmployerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return View("ReviewDetails", model);
        }
    }
}
