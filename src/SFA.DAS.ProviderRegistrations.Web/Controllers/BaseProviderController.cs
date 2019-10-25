using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderRegistrations.Web.Controllers
{
    public class BaseProviderController : Controller
    {
        public string ProviderId { get; set; }
    }
}
