using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.ReceiveProviderRelationshipsHealthCheckEvent;

public class ReceiveProviderRelationshipsHealthCheckEventCommandHandler : IRequestHandler<ReceiveProviderRelationshipsHealthCheckEventCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public ReceiveProviderRelationshipsHealthCheckEventCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(ReceiveProviderRelationshipsHealthCheckEventCommand request, CancellationToken cancellationToken)
    {
        var healthCheck = await _db.Value.HealthChecks.SingleAsync(h => h.Id == request.Id, cancellationToken);

        healthCheck.ReceiveProviderRelationshipsEvent();
    }
}