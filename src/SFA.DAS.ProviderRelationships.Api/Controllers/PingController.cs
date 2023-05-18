namespace SFA.DAS.ProviderRelationships.Api.Controllers;

[AllowAnonymous]
[Route("ping")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}