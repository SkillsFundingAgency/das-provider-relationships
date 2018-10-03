using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Messages;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Models
{
    [TestFixture]
    public class HealthCheckTests : FluentTest<HealthCheckTestsFixture>
    {
        [Test]
        public void New_WhenCreatingAHealthCheck_ThenShouldCreateAHealthCheck()
        {
            Run(f => f.New(), (f, r) => r.Should().NotBeNull());
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldSetSentApprenticeshipInfoServiceApiRequestProperty()
        {
            return RunAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.SentApprenticeshipInfoServiceApiRequest >= f.PreRun && h.SentApprenticeshipInfoServiceApiRequest <= f.PostRun));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldSetReceivedApprenticeshipInfoServiceApiResponseProperty()
        {
            return RunAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.ReceivedApprenticeshipInfoServiceApiResponse >= f.PreRun && h.ReceivedApprenticeshipInfoServiceApiResponse <= f.PostRun));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheckAndAApprenticeshipInfoServiceApiExceptionIsThrown_ThenShouldNotSetReceivedApprenticeshipInfoServiceApiResponseProperty()
        {
            return RunAsync(f => f.SetHealthCheck().SetApprenticeshipInfoServiceApiRequestException(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.ReceivedApprenticeshipInfoServiceApiResponse == null));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldSetPublishedProviderRelationshipsEventProperty()
        {
            return RunAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.PublishedProviderRelationshipsEvent >= f.PreRun && h.PublishedProviderRelationshipsEvent <= f.PostRun));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldPublishAHealthCheckEvent()
        {
            return RunAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.UnitOfWorkContext.GetEvents().Should().HaveCount(1)
                .And.AllBeOfType<HealthCheckEvent>()
                .And.AllBeEquivalentTo(new HealthCheckEvent
                {
                    Id = f.HealthCheck.Id,
                    Created = f.HealthCheck.PublishedProviderRelationshipsEvent
                }));
        }

        [Test]
        public void ReceiveEvent_WhenReceivingAHealthCheckEvent_ThenShouldSetReceivedProviderRelationshipsEventProperty()
        {
            Run(f => f.SetHealthCheck(), f => f.ReceiveEvent(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.ReceivedProviderRelationshipsEvent >= f.PreRun && h.ReceivedProviderRelationshipsEvent <= f.PostRun));
        }
    }

    public class HealthCheckTestsFixture
    {
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public HealthCheck HealthCheck { get; set; }
        public Func<Task> ApprenticeshipInfoServiceApiRequest { get; set; }
        public DateTime? PreRun { get; set; }
        public DateTime? PostRun { get; set; }

        public HealthCheckTestsFixture()
        {
            UnitOfWorkContext = new UnitOfWorkContext();
            ApprenticeshipInfoServiceApiRequest = () => Task.CompletedTask;
        }

        public HealthCheck New()
        {
            return new HealthCheck();
        }

        public async Task Run()
        {
            PreRun = DateTime.UtcNow;

            await HealthCheck.Run(ApprenticeshipInfoServiceApiRequest);

            PostRun = DateTime.UtcNow;
        }

        public void ReceiveEvent()
        {
            PreRun = DateTime.UtcNow;

            HealthCheck.ReceiveProviderRelationshipsEvent(new HealthCheckEvent());

            PostRun = DateTime.UtcNow;
        }

        public HealthCheckTestsFixture SetHealthCheck()
        {
            HealthCheck = new HealthCheckBuilder().WithId(1).Build();

            return this;
        }

        public HealthCheckTestsFixture SetApprenticeshipInfoServiceApiRequestException()
        {
            ApprenticeshipInfoServiceApiRequest = () => throw new Exception();

            return this;
        }
    }
}