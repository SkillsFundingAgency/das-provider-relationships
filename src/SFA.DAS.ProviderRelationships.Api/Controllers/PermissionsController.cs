using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.Authorization;
using SFA.DAS.ProviderRelationships.Api.RouteValues.Permissions;
using SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Application.Queries.Ping;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("permissions")]
    public class PermissionsController : ApiController
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("has")]
        [HttpGet]
        [AuthorizeRemoteOnly(Roles = "Read")]
        public async Task<IHttpActionResult> HasPermission([FromUri] HasPermissionRouteValues routeValues, CancellationToken cancellationToken)
        {
            if (routeValues.Ukprn == null)
            {
                ModelState.AddModelError(nameof(routeValues.Ukprn), "A Ukprn needs to be supplied");
            }

            if (routeValues.AccountLegalEntityId == null)
            {
                ModelState.AddModelError(nameof(routeValues.Ukprn), "A Account Legal Entity Id needs to be supplied");
            }

            if (routeValues.Operation == null)
            {
                ModelState.AddModelError(nameof(routeValues.Operation), "A Operation needs to be supplied");
            }

            var hasPermission = await _mediator.Send(new HasPermissionQuery(routeValues.Ukprn.Value, routeValues.AccountLegalEntityId.Value, routeValues.Operation.Value), cancellationToken);
            return Ok(hasPermission);
        }

        [Route("has-relationship-with")]
        [HttpGet]
        [AuthorizeRemoteOnly(Roles = "Read")]
        public async Task<IHttpActionResult> HasRelationshipWithPermission([FromUri] HasRelationshipWithPermissionRouteValues routeValues, CancellationToken cancellationToken)
        {
            if (routeValues.Ukprn == null)
            {
                ModelState.AddModelError(nameof(routeValues.Ukprn), "A Ukprn needs to be supplied");
            }

            if (routeValues.Operation == null)
            {
                ModelState.AddModelError(nameof(routeValues.Operation), "A Operation needs to be supplied");
            }

            var hasPermission = await _mediator.Send(new HasRelationshipWithPermissionQuery(routeValues.Ukprn.Value, routeValues.Operation.Value), cancellationToken);
            return Ok(hasPermission);
        }

        [Route("ping")]
        [HttpGet]
        [AuthorizeRemoteOnly(Roles = "Read")]
        public async Task<IHttpActionResult> Ping(CancellationToken cancellationToken)
        {
            await _mediator.Send(new PingQuery(), cancellationToken);
            return Ok();
        }

        [Route("revoke")]
        [HttpPost]
        [AuthorizeRemoteOnly(Roles = "Write")]
        public async Task<IHttpActionResult> Revoke([FromBody] RevokePermissionsRouteValues routeValues)
        {
            if (routeValues.Ukprn == null)
            {
                ModelState.AddModelError(nameof(routeValues.Ukprn), "A Ukprn needs to be supplied");
            }

            if (string.IsNullOrWhiteSpace(routeValues.AccountLegalEntityPublicHashedId))
            {
                ModelState.AddModelError(nameof(routeValues.AccountLegalEntityPublicHashedId), "A Public Hashed Id for an Account Legal Entity needs to be supplied");
            }

            if (routeValues.OperationsToRevoke == null || routeValues.OperationsToRevoke.Length == 0)
            {
                ModelState.AddModelError(nameof(routeValues.OperationsToRevoke), "One or more operations need to be supplied");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new RevokePermissionsCommand(
                ukprn: routeValues.Ukprn.Value,
                accountLegalEntityPublicHashedId: routeValues.AccountLegalEntityPublicHashedId,
                operationsToRevoke: routeValues.OperationsToRevoke);
            await _mediator.Send(command);

            return Ok();
        }
    }
}