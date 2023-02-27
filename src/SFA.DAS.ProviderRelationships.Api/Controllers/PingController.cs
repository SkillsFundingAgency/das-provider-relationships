
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("ping")]
    public class PingController : ControllerBase
    {
        [Route]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}