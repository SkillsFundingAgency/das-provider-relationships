using System.Web.Http;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("healthcheck")]
    public class HealthCheckController : ApiController
    {
        [Route]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}