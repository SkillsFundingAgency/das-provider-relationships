using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.ActionParameters.Relationships;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("relationships")]
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
        /// <param name="parameters">Members
        /// ukprn: Filter relationships to only those for this provider
        /// operation: Filter relationships to only those which have this permission
        /// </param>
        public async Task<IHttpActionResult> Get([FromUri] GetRelationshipsParameters parameters)
        {
            // logically it makes sense to return 404 if ukprn is not found even if there is an issue with queryOperation
            // it might not be most performant though
            // could store bad results and only return them if we don't find the provider
            // so instead of throwing exception, store IHttpActionRESULT?
            // check ukprn exists, if not return 404 not found            

            //? gonna make structure nasty, unless call check method with returns error/operation
            //IHttpActionResult errorResult = null;

            // we could accept non-nullable, but we might want to return all relationships
            if (parameters.ukprn == null)
            {
                // logically this would return all relationships (filtered by operation if supplied)
                throw new HttpResponseException(HttpStatusCode.NotImplemented);
            }
            
            if (parameters.operation == null)
            //if (request.Operation) // mvc handles not matching. use request but with string??
            {
                // logically this would return all relationships (filtered by ukprn if supplied)
                throw new HttpResponseException(HttpStatusCode.NotImplemented);
            }

            if (!Enum.TryParse(parameters.operation, true, out Operation operation))
                return BadRequest();
            
            var result = await _mediator.Send(new GetRelationshipsWithPermissionQuery(parameters.ukprn.Value, operation));

            return Ok(new RelationshipsResponse {Relationships = result.Relationships});
        }
    }
}