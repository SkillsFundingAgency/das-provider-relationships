using System;
using System.Net;
using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public DocumentException(string message, HttpStatusCode httpStatusCode, DocumentClientException e) : base(message, e)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
