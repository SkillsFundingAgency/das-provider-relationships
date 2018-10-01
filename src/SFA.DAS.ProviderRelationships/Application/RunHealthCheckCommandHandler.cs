using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.ProviderRelationships.Application
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
            var healthCheck = new HealthCheck(request.UserRef.Value);

            await healthCheck.Run(
                () => Task.CompletedTask,
                () => _providerApiClient.FindAllAsync());

            _db.Value.HealthChecks.Add(healthCheck);
        }
    }
}