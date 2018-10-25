using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class RunHealthCheckCommandHandler : AsyncRequestHandler<RunHealthCheckCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IProviderApiClient _providerApiClient;

        public RunHealthCheckCommandHandler(Lazy<ProviderRelationshipsDbContext> db, IProviderApiClient providerApiClient)
        {
            _db = db;
            _providerApiClient = providerApiClient;
        }

        protected override async Task Handle(RunHealthCheckCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Value.Users.SingleAsync(u => u.Ref == request.UserRef.Value, cancellationToken);
            var healthCheck = user.CreateHealthCheck();

            await healthCheck.Run(_providerApiClient.FindAllAsync);

            _db.Value.HealthChecks.Add(healthCheck);
        }
    }
}