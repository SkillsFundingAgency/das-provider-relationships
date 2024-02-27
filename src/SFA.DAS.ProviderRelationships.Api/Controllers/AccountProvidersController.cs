using SFA.DAS.ProviderRelationships.Api.Authorization;
using SFA.DAS.ProviderRelationships.Api.Models.Requests;
using SFA.DAS.ProviderRelationships.Api.Models.Responses;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Controllers;

[Authorize(Policy = ApiRoles.Read)]
[Route("accounts/{accountId}/providers")]
public class AccountProvidersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountProvidersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get account/provider relationships
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    public async Task<IActionResult> Get(
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

        var result = await _mediator.Send(new GetAccountProvidersQuery(accountId), cancellationToken);

        var response = new GetAccountProvidersResponse {
            AccountId = accountId,
            AccountProviders = result.AccountProviders
        };

        return Ok(response);
    }

    [HttpPost]
    [Route("invitation")]
    public async Task<IActionResult> Invitation(
       [FromRoute] long accountId,
       [FromBody] AddAccountProviderFromInvitationPostRequest request,
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

        await CheckExistsAndUpsertNewUser(request.UserRef, request.Email, request.FirstName, request.LastName);

        var verify = await _mediator.Send(new FindProviderToAddQuery(accountId, request.Ukprn), cancellationToken);

        if (verify.ProviderNotFound)
        {
            return NotFound();
        }
        if (verify.ProviderAlreadyAdded)
        {
            return Ok();
        }

        var accountProviderId = await _mediator.Send(new AddAccountProviderCommand(accountId, request.Ukprn, request.UserRef, request.CorrelationId), cancellationToken);

        return Ok(new AddAccountProviderFromInvitationResponse {
            AccountProviderId = accountProviderId
        });
    }

    private async Task CheckExistsAndUpsertNewUser(Guid userId, string email, string firstName, string lastName)
    {
        await _mediator.Send(new CreateOrUpdateUserCommand(
                        userId,
                        email,
                        firstName,
                        lastName
           ));
    }
}