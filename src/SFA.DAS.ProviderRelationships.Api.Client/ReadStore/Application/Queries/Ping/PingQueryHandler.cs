using System;
using System.Linq;
using MediatR;
using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.Ping
{
    internal class PingQueryHandler : RequestHandler<PingQuery>
    {
        private readonly IDocumentClient _documentClient;

        public PingQueryHandler(IDocumentClientFactory documentClientFactory)
        {
            _documentClient = documentClientFactory.CreateDocumentClient();
        }
        
        protected override void Handle(PingQuery request)
        {
            var value = _documentClient.CreateDatabaseQuery()
                .Where(d => d.Id == DocumentSettings.DatabaseName)
                .Select(d => 1)
                .AsEnumerable()
                .FirstOrDefault();

            if (value == 0)
            {
                throw new Exception("Read store database ping failed");
            }
        }
    }
}