namespace SFA.DAS.ProviderRelationships.Api.Controllers;

[Route("ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}