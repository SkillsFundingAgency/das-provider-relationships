using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.ActionParameters.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Api.Authentication;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("accountproviderlegalentities")]
    [AuthorizeRemoteOnly(Roles = "Read")]
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
        /// It would be nice to return a 404 if there is no provider with the supplied ukprn, but currently we just return an empty set.
        /// It would also be nice to cancel on client disconnects, see https://github.com/aspnet/Mvc/issues/5239
        /// </remarks>
        /// <param name="parameters">GetAccountProviderLegalEntitiesParameters members:
        /// Ukprn: Filter AccountProviderLegalEntities to only those for this provider (we could accept non-nullable, but we might want to return unfiltered by ukprn)
        /// Operation: Filter AccountProviderLegalEntities to only those which have this permission
        /// </param>
        [Route]
        public async Task<IHttpActionResult> Get([FromUri] GetAccountProviderLegalEntitiesParameters parameters, CancellationToken cancellationToken)
        {
            var operation = Operation.CreateCohort;
            
            if (parameters.Ukprn == null)
            {
                // logically this would return all relationships (filtered by operation if supplied)
                ModelState.AddModelError(nameof(parameters.Ukprn), "Currently an Ukprn filter needs to be supplied");
            }

            if (parameters.Operation == null)
            {
                // logically this would return all relationships (filtered by ukprn if supplied)
                ModelState.AddModelError(nameof(parameters.Operation), "Currently an Operation filter needs to be supplied");
            }
            else if (!Enum.TryParse(parameters.Operation, true, out operation))
            {
                ModelState.AddModelError(nameof(parameters.Operation), "The Operation filter value supplied is not recognised as an Operation");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _mediator.Send(new GetAccountProviderLegalEntitiesWithPermissionQuery(parameters.Ukprn.Value, operation), cancellationToken);

            return Ok(new GetAccountProviderLegalEntitiesWithPermissionResponse {AccountProviderLegalEntities = result.AccountProviderLegalEntities});
        }
    }
}