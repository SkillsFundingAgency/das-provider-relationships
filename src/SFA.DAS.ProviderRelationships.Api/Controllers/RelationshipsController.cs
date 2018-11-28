using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.ActionParameters.Relationships;
using SFA.DAS.ProviderRelationships.Api.Authentication;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    public class Error
    {
        public string ErrorCode { get; }
        public string Message { get; }
        
        public Error(string errorCode = null, string message = null)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
    
    public class ErrorResult : IHttpActionResult
    {
        private readonly HttpStatusCode _httpStatusCode;
        private readonly Error _error;
        private readonly HttpRequestMessage _request;

        public ErrorResult(HttpStatusCode httpStatusCode, Error error, HttpRequestMessage request)
        {
            _httpStatusCode = httpStatusCode;
            _error = error;
            _request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(_httpStatusCode)
            {
                Content = new ObjectContent<Error>(_error, new JsonMediaTypeFormatter()),
                RequestMessage = _request
            });
        }
    }
    
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

//todo:extension?
//todo: centralise all error codes, so client's have access
//many ways to handle errors, so we may want to change this, but for now
                const string missingUkprnFilterErrorCode = "F99D3D9C-ACEC-4B1A-A722-45223ADE35A0";

                return new ErrorResult(HttpStatusCode.NotImplemented, new Error(missingUkprnFilterErrorCode, "Currently an Ukprn filter needs to be supplied"), Request);
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