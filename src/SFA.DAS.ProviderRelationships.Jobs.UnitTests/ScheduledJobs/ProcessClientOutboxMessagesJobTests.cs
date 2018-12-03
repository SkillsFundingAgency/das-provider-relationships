using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NServiceBus.ClientOutbox;
using SFA.DAS.Testing;
using ProcessClientOutboxMessagesJob = SFA.DAS.ProviderRelationships.Jobs.ScheduledJobs.ProcessClientOutboxMessagesJob;

namespace SFA.DAS.ProviderRelationships.Jobs.UnitTests.ScheduledJobs
{
    [TestFixture]
    [Parallelizable]
    public class ProcessClientOutboxMessagesJobTests : FluentTest<ProcessClientOutboxMessagesJobTestsFixture>
    {
        [Test]
        public Task Run_WhenRunningProcessClientOutboxMessagesJob_ThenShouldProcessClientOutboxMessages()
        {
            return RunAsync(f => f.Run(), f => f.ProcessClientOutboxMessagesJob.Verify(j => j.RunAsync(), Times.Once));
        }
    }

    public class ProcessClientOutboxMessagesJobTestsFixture
    {
        public Mock<IProcessClientOutboxMessagesJob> ProcessClientOutboxMessagesJob { get; set; }
        public ProcessClientOutboxMessagesJob Job { get; set; }

        public ProcessClientOutboxMessagesJobTestsFixture()
        {
            ProcessClientOutboxMessagesJob = new Mock<IProcessClientOutboxMessagesJob>();
            Job = new ProcessClientOutboxMessagesJob(ProcessClientOutboxMessagesJob.Object);
        }

        public Task Run()
        {
            return Job.Run(null, null);
        }
    }
}