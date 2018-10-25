using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.NServiceBus.ClientOutbox;

namespace SFA.DAS.ProviderRelationships.Jobs.Functions
{
    public class ProcessClientOutboxMessagesFunction
    {
        private readonly IProcessClientOutboxMessagesJob _processClientOutboxMessagesJob;

        public ProcessClientOutboxMessagesFunction(IProcessClientOutboxMessagesJob processClientOutboxMessagesJob)
        {
            _processClientOutboxMessagesJob = processClientOutboxMessagesJob;
        }

        public Task RunAsync([TimerTrigger("0 */10 * * * *")] TimerInfo timer, TraceWriter logger)
        {
            return _processClientOutboxMessagesJob.RunAsync();
        }
    }
}