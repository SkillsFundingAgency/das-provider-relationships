using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
    }
}
