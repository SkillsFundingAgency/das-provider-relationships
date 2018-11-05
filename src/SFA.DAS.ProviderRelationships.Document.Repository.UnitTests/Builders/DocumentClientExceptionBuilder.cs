using System.Net;
using System.Reflection;
using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Builders
{
    public static class DocumentClientExceptionBuilder
    {
        public static DocumentClientException Build(Error error, HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);
            
            var documentClientExceptionInstance = type.Assembly.CreateInstance(
                type.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { error, null, httpStatusCode }, null, null);

            return (DocumentClientException)documentClientExceptionInstance;
        }
    }
}