using SFA.DAS.ProviderRelationships.Api.Authorization;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Controllers;

[Authorize(Policy = ApiRoles.Read)]
[Route("accounts/{accountId}/permissions")]
public class AccountProvidersController : ControllerBase
{
    /// <summary>
    /// Get account/provider relationships
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    public async Task<IActionResult> Get(
    IMediator mediator,
    [FromRoute] long accountId, 
    CancellationToken cancellationToken)
    {
        if (accountId == 0)
        {
            ModelState.AddModelError(nameof(accountId), "Account ID needs to be set");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
            
   
        var result = await mediator.Send(new GetAccountProvidersQuery(accountId), cancellationToken);

        var response = new GetAccountProvidersResponse 
        {
            AccountId = accountId,
            AccountProviders = result.AccountProviders
        };

        return Ok(response);
    }
}