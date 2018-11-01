using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests
{
    public static class CosmosDbHelper
    {
        public static DocumentClientException CreateDocumentClientExceptionForTesting(
            Error error, HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);

            // we are using the overload with 3 parameters (error, responseheaders, statuscode)
            var documentClientExceptionInstance = type.Assembly.CreateInstance(type.FullName,
                false, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { error, (HttpResponseHeaders)null, httpStatusCode }, null, null);

            return (DocumentClientException)documentClientExceptionInstance;
        }

    }
}
