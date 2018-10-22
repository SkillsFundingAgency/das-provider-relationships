using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using StructureMap.Building;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class CosmosClientWrapper : ICosmosClientWrapper
    {
        private DocumentClient _documentClient;
        public CosmosClientWrapper(IDocumentConfiguration cosmosDbConfiguration)
        {
            _documentClient = new DocumentClient(new Uri(cosmosDbConfiguration.Uri), cosmosDbConfiguration.SecurityKey);
        }

        //public async Task<string> ReadDocumentAsync(string collectionName, Guid id)
        //{
        //    //try
        //    //{
        //        var resourceResponse = await _documentClient.ReadDocumentAsync(DocumentUri(collectionName, id.ToString()));
        //        return resourceResponse.Resource.ToString();
        //    //}
        //    //catch (DocumentClientException exc)
        //    //{
        //    //    throw;
        //    //}
        //}

        //public async Task<IEnumerable<TResult>> QueryACollection<TResult>(string collectionName, Expression<Func<TResult, bool>> sqlApiQuery, FeedOptions feedOptions)
        //{
        //    //try
        //    //{
        //    //    var query =
        //    //        _documentClient
        //    //            .CreateDocumentQuery<TResult>(DocumentCollectionUri(collectionName), feedOptions)
        //    //            .Where(sqlApiQuery);

        //    //    return await query.AsDocumentQuery().ExecuteNextAsync<TResult>();
        //    //}
        //    //catch (DocumentClientException documentClientException)
        //    //{
        //    //    if(documentClientException.StatusCode == HttpStatusCode.NotFound)
        //    //        throw new DocumentException("Collection Not Found", HttpStatusCode.NotFound, documentClientException) ;

        //    //    if (documentClientException.StatusCode == HttpStatusCode.BadRequest && documentClientException.Message.StartsWith("Cross partition query is required but disabled"))
        //    //        throw new DocumentException("This Collection is partitioned, the query must include the partition key or you must set the option to query across all partitions.", HttpStatusCode.BadRequest, documentClientException);
                
        //    //    throw;
        //    //}
        //    //catch (JsonSerializationException jsonException)
        //    //{
        //    //    throw new DocumentException(jsonException.Message, HttpStatusCode.InternalServerError, jsonException);
        //    //}
        //}

    }
}
