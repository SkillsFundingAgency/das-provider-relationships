namespace SFA.DAS.ProviderRelationships.Api.Controllers;

[Route("ping")]
public class PingController : ControllerBase
{
    public IActionResult Get()
    {
        return Ok();
    }
}