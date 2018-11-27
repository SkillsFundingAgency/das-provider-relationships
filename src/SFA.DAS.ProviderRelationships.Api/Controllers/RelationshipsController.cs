using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.ActionParameters.Relationships;
using SFA.DAS.ProviderRelationships.Api.Authentication;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("relationships")]
    [AuthorizeRemoteOnly(Roles = "Read")]
    public class RelationshipsController : ApiController
    {
        private readonly IMediator _mediator;

        public RelationshipsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Get relationships with optional (currently mandatory) filters
        /// </summary>
        /// <remarks>
        /// It would be nice to return a 404 if there is no provider with the supplied ukprn, but currently we just return an empty set
        /// </remarks>
        /// <param name="parameters">Members
        /// Ukprn: Filter relationships to only those for this provider
        /// Operation: Filter relationships to only those which have this permission
        /// </param>
        [Route]
        //todo: cancel on client disconnects: https://github.com/aspnet/Mvc/issues/5239
        public async Task<IHttpActionResult> Get([FromUri] GetRelationshipsParameters parameters) // , CancellationToken cancellationToken)
        {
            //todo: distinguish between not founds (put error message in response body indicating what was not found)? return empty?
            //todo: in general, include error response in body, something like {ErrorCode: x, ErrorMessage: ""}

            // we could accept non-nullable, but we might want to return all relationships
            if (parameters.Ukprn == null)
            {
                // logically this would return all relationships (filtered by operation if supplied)
                throw new HttpResponseException(HttpStatusCode.NotImplemented);
            }

            //operation not supplied or value didn't match enum value
            if (parameters.Operation == null)
            {
                // logically this would return all relationships (filtered by ukprn if supplied)
                throw new HttpResponseException(HttpStatusCode.NotImplemented);
            }
            
            var result = await _mediator.Send(new GetRelationshipsWithPermissionQuery(parameters.Ukprn.Value, parameters.Operation.Value)); //, cancellationToken);

            return Ok(new RelationshipsResponse {Relationships = result.Relationships});
        }
    }
}