using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using static MoreLinq.Extensions.BatchExtension;
using static MoreLinq.Extensions.ToDataTableExtension;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.ProviderRelationships.Jobs.TriggeredJobs
{
    public class ImportProvidersJob
    {
        private readonly IProviderApiClient _providerApiClient;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public ImportProvidersJob(IProviderApiClient providerApiClient, Lazy<ProviderRelationshipsDbContext> db)
        {
            _providerApiClient = providerApiClient;
            _db = db;
        }

        public async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo timer, ILogger logger)
        {
            var providers = await _providerApiClient.FindAllAsync();
            var batches = providers.Batch(1000).Select(b => b.ToDataTable(p => p.Ukprn, p => p.ProviderName));

            foreach (var batch in batches)
            {
                await _db.Value.Database.ImportProviders(batch);
            }
        }
    }
}