using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.Authorization;
using SFA.DAS.ProviderRelationships.Api.RouteValues.Providers;
using SFA.DAS.ProviderRelationships.Application.Commands.RemoveProviderPermissionsForAccountLegalEntity;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("providers")]
    public class ProvidersController : ApiController
    {
        private readonly IMediator _mediator;

        public ProvidersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route]
        [HttpPost]
        [AuthorizeRemoteOnly(Roles = "Write")]
        public async Task<IHttpActionResult> RemoveProviderPermissionsForAccountLegalEntity(
            [FromBody] RemoveProviderPermissionsForAccountLegalEntityRouteValues routeValues)
        {
            if (routeValues.Ukprn == null)
            {
                ModelState.AddModelError(nameof(routeValues.Ukprn), "Currently a Ukprn filter needs to be supplied");
            }

            if (string.IsNullOrWhiteSpace(routeValues.AccountLegalEntityPublicHashedId))
            {
                ModelState.AddModelError(nameof(routeValues.AccountLegalEntityPublicHashedId), "Currently a Publich Hashed Id for an Account Legal Entity needs to be supplied");
            }

            if (routeValues.OperationsToRemove == null || routeValues.OperationsToRemove.Length == 0)
            {
                ModelState.AddModelError(nameof(routeValues.OperationsToRemove), "Currently one or more Operations need to be supplied");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new RemoveProviderPermissionsForAccountLegalEntityCommand(
                ukprn: routeValues.Ukprn.Value,
                accountLegalEntityPublicHashedId: routeValues.AccountLegalEntityPublicHashedId,
                operationsToRemove: routeValues.OperationsToRemove);
            await _mediator.Send(command);

            return Ok();
        }
    }
}