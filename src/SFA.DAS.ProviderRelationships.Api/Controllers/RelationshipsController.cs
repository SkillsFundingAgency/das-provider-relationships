using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    //todo: not getting providers, getting relationships
    [RoutePrefix("providers/{ukprn}")]
    public class ProvidersController : ApiController
    {
        private readonly IMediator _mediator;

        public ProvidersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("relationships")]
        public async Task<IHttpActionResult> GetRelationshipsWithPermission(long ukprn, string queryOperation)
        //public async Task<IHttpActionResult> GetRelationshipsWithPermission(RelationshipsRequest request)
        {
            // logically it makes sense to return 404 if ukprn is not found even if there is an issue with queryOperation
            // it might not be most performant though
            // could store bad results and only return them if we don't find the provider
            // so instead of throwing exception, store IHttpActionRESULT?
            // check ukprn exists, if not return 404 not found            

            //? gonna make structure nasty, unless call check method with returns error/operation
            //IHttpActionResult errorResult = null;
            
            if (queryOperation == null)
            //if (request.Operation) // mvc handles not matching. use request but with string??
            {
                // logically this would return all relationships
                throw new HttpResponseException(HttpStatusCode.NotImplemented);
            }

            if (!Enum.TryParse(queryOperation, true, out Operation operation))
                return BadRequest();
            
            var result = await _mediator.Send(new GetRelationshipsWithPermissionQuery(ukprn, operation));

            return Ok(new RelationshipsResponse {Relationships = result.Relationships});
        }
    }
    
//    [RoutePrefix("relationships")]
//    public class RelationshipsController : ApiController
//    {
//        //        public long Ukprn { get; set; }
//        //public Operation Operation { get; set; }
//
//        [Route]
//        public IHttpActionResult GetRelationshipsWithPermission()//bool withPermission)
//        {
////            //RequestContext.
////            var queryNameValuePairs = Request.GetQueryNameValuePairs();
////
////            var withPermission = queryNameValuePairs.FirstOrDefault(nvp =>
////                nvp.Key.Equals("withPermission", StringComparison.OrdinalIgnoreCase));
////
////            if (withPermission.Equals(default(KeyValuePair<string, string>)))
////            {
////                // logically this would return all relationships
////                throw new HttpResponseException(HttpStatusCode.NotImplemented);
////            }
//
//            return Ok();
//        }
//    }
}