using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.Authorization;
using SFA.DAS.ProviderRelationships.Application.Commands.UnsubscribeById;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [AuthorizeRemoteOnly(Roles = "Read")]
    [RoutePrefix("unsubscribe")]
    public class UnsubscribeController : ApiController
    {
        private readonly IMediator _mediator;

        public UnsubscribeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("{correlationId}")]
        public async Task<IHttpActionResult> Get(string correlationId, CancellationToken cancellationToken)
        {
            Guid correlationGuid;

            if (!Guid.TryParse(correlationId, out correlationGuid))
            {
                ModelState.AddModelError(nameof(correlationId), "An invalid correlation id was supplied");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mediator.Send(new UnsubscribeByIdCommand(correlationGuid), cancellationToken);
            return Ok();
        }
    }
}