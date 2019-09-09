using System;
using System.Linq;
using MediatR;
using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Commands.Ping
{
    internal class PingCommandHandler : RequestHandler<PingCommand>
    {
        private readonly IDocumentClient _documentClient;

        public PingCommandHandler(IDocumentClientFactory documentClientFactory)
        {
            _documentClient = documentClientFactory.CreateDocumentClient();
        }
        
        protected override void Handle(PingCommand request)
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