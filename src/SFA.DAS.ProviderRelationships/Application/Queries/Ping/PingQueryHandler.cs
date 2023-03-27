using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.Ping;

public class PingQueryHandler : IRequestHandler<PingQuery>
{
    private readonly IDocumentClient _documentClient;

    public PingQueryHandler(IDocumentClientFactory documentClientFactory)
    {
        _documentClient = documentClientFactory.CreateDocumentClient();
    }

    public Task Handle(PingQuery request, CancellationToken cancellationToken)
    {
        var value = _documentClient.CreateDatabaseQuery()
            .Where(d => d.Id == DocumentSettings.DatabaseName)
            .Select(d => 1)
            .AsEnumerable()
            .FirstOrDefault();

        if (value == 0)
        {
            throw new PingQueryException("Read store database ping failed");
        }

        return Task.CompletedTask;
    }
}

[Serializable]
public class PingQueryException : Exception
{
    public PingQueryException(string message): base(message) { }

    protected PingQueryException(SerializationInfo info, StreamingContext context): base(info, context) { }
}