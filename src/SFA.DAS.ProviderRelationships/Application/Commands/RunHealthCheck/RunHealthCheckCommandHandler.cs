using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.Application.Commands.RunHealthCheck;

public class RunHealthCheckCommandHandler : IRequestHandler<RunHealthCheckCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;
    private readonly IRoatpService _roatpService;
    private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;

    public RunHealthCheckCommandHandler(Lazy<ProviderRelationshipsDbContext> db, IRoatpService roatpService, IProviderRelationshipsApiClient providerRelationshipsApiClient)
    {
        _db = db;
        _roatpService = roatpService;
        _providerRelationshipsApiClient = providerRelationshipsApiClient;
    }

    public async Task Handle(RunHealthCheckCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Value.Users.SingleAsync(u => u.Ref == request.UserRef, cancellationToken);
        var healthCheck = user.CreateHealthCheck();
            
        // NOTE: the health check is started from the web application not from the api so the api is being verified
        await healthCheck.Run(() => _roatpService.Ping(), () => _providerRelationshipsApiClient.Ping(cancellationToken));

        _db.Value.HealthChecks.Add(healthCheck);
    }
}