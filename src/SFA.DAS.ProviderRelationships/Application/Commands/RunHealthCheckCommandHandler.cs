using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class RunHealthCheckCommandHandler : AsyncRequestHandler<RunHealthCheckCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IProviderApiClient _providerApiClient;
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;

        public RunHealthCheckCommandHandler(Lazy<ProviderRelationshipsDbContext> db, IProviderApiClient providerApiClient, IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _db = db;
            _providerApiClient = providerApiClient;
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        protected override async Task Handle(RunHealthCheckCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Value.Users.SingleAsync(u => u.Ref == request.UserRef, cancellationToken);
            var healthCheck = user.CreateHealthCheck();

            await healthCheck.Run(_providerApiClient.FindAllAsync, _providerRelationshipsApiClient.HealthCheck);

            _db.Value.HealthChecks.Add(healthCheck);
        }
    }
}