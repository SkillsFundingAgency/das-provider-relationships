using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.Authorization;
using SFA.DAS.ProviderRelationships.Api.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [AuthorizeRemoteOnly(Roles = "Read")]
    [RoutePrefix("accountproviderlegalentities")]
    public class AccountProviderLegalEntitiesController : ApiController
    {
        private readonly IMediator _mediator;

        public AccountProviderLegalEntitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Get relationships with optional (currently mandatory) filters
        /// </summary>
        /// <remarks>
        /// It would be nice to cancel on client disconnects, see https://github.com/aspnet/Mvc/issues/5239
        /// </remarks>
        /// <param name="routeValues">GetAccountProviderLegalEntitiesParameters members:
        /// Ukprn: Filter AccountProviderLegalEntities to only those for this provider (we could accept non-nullable, but we might want to return unfiltered by ukprn)
        /// Operation: Filter AccountProviderLegalEntities to only those which have this permission
        /// </param>
        [Route]
        public async Task<IHttpActionResult> Get([FromUri] GetAccountProviderLegalEntitiesRouteValues routeValues, CancellationToken cancellationToken)
        {
            if (routeValues.Ukprn == null && string.IsNullOrWhiteSpace(routeValues.AccountHashedId))
            {
                ModelState.AddModelError(nameof(routeValues.Ukprn), "Currently a Ukprn filter needs to be supplied");
                ModelState.AddModelError(nameof(routeValues.AccountHashedId), "Currently an AccountHashedId filter needs to be supplied");
            }

            if (routeValues.Operation == null)
            {
                ModelState.AddModelError(nameof(routeValues.Operation), "Currently an Operation filter needs to be supplied");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _mediator.Send(new GetAccountProviderLegalEntitiesWithPermissionQuery(routeValues.Ukprn, routeValues.AccountHashedId, routeValues.AccountLegalEntityPublicHashedId, routeValues.Operation.Value), cancellationToken);

            return Ok(new GetAccountProviderLegalEntitiesWithPermissionResponse { AccountProviderLegalEntities = result.AccountProviderLegalEntities });
        }
    }
}