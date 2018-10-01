using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.NServiceBus.ClientOutbox;

namespace SFA.DAS.ProviderRelationships.Jobs
{
    public class ProcessClientOutboxMessagesJob
    {
        private readonly IProcessClientOutboxMessagesJob _processClientOutboxMessagesJob;

        public ProcessClientOutboxMessagesJob(IProcessClientOutboxMessagesJob processClientOutboxMessagesJob)
        {
            _processClientOutboxMessagesJob = processClientOutboxMessagesJob;
        }

        public Task RunAsync([TimerTrigger("0 */10 * * * *")] TimerInfo timer, TraceWriter logger)
        {
            return _processClientOutboxMessagesJob.RunAsync();
        }
    }
}