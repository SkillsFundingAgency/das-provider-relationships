using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Services;


namespace SFA.DAS.ProviderRelationships.Jobs.ScheduledJobs
{
    public class ImportProvidersJob
    {
        private readonly IRoatpService _providerApiClient;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public ImportProvidersJob(IRoatpService providerApiClient, Lazy<ProviderRelationshipsDbContext> db)
        {
            _providerApiClient = providerApiClient;
            _db = db;
        }

        public async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo timer, ILogger logger)
        {
            var providers = await _providerApiClient.GetProviders();
            var batches = providers.Batch(1000).Select(b => b.ToDataTable(p => p.Ukprn, p => p.ProviderName));

            foreach (var batch in batches)
            {
                await _db.Value.ImportProviders(batch);
            }
        }
    }
}