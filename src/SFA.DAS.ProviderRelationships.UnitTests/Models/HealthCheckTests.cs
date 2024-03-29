﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class HealthCheckTests : FluentTest<HealthCheckTestsFixture>
    {
        [Test]
        public void New_WhenCreatingAHealthCheck_ThenShouldCreateAHealthCheck()
        {
            Test(f => f.New(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.User.Should().Be(f.User);
            });
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldSetSentApprenticeshipInfoServiceApiRequestProperty()
        {
            return TestAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.SentApprenticeshipInfoServiceApiRequest >= f.PreRun && h.SentApprenticeshipInfoServiceApiRequest <= f.PostRun));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldSetReceivedApprenticeshipInfoServiceApiResponseProperty()
        {
            return TestAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.ReceivedApprenticeshipInfoServiceApiResponse >= f.PreRun && h.ReceivedApprenticeshipInfoServiceApiResponse <= f.PostRun));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheckAndAApprenticeshipInfoServiceApiExceptionIsThrown_ThenShouldNotSetReceivedApprenticeshipInfoServiceApiResponseProperty()
        {
            return TestAsync(f => f.SetHealthCheck().SetApprenticeshipInfoServiceApiRequestException(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.ReceivedApprenticeshipInfoServiceApiResponse == null));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldSetPublishedProviderRelationshipsEventProperty()
        {
            return TestAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.PublishedProviderRelationshipsEvent >= f.PreRun && h.PublishedProviderRelationshipsEvent <= f.PostRun));
        }

        [Test]
        public Task Run_WhenRunningAHealthCheck_ThenShouldPublishAHealthCheckEvent()
        {
            return TestAsync(f => f.SetHealthCheck(), f => f.Run(), f => f.UnitOfWorkContext.GetEvents().Should().HaveCount(1)
                .And.AllBeOfType<HealthCheckEvent>()
                .And.AllBeEquivalentTo(new HealthCheckEvent(f.HealthCheck.Id, f.HealthCheck.PublishedProviderRelationshipsEvent)));
        }

        [Test]
        public void ReceiveEvent_WhenReceivingAHealthCheckEvent_ThenShouldSetReceivedProviderRelationshipsEventProperty()
        {
            Test(f => f.SetHealthCheck(), f => f.ReceiveEvent(), f => f.HealthCheck.Should().Match<HealthCheck>(h => h.ReceivedProviderRelationshipsEvent >= f.PreRun && h.ReceivedProviderRelationshipsEvent <= f.PostRun));
        }
    }

    public class HealthCheckTestsFixture
    {
        public User User { get; set; }
        public HealthCheck HealthCheck { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public Func<Task> ApprenticeshipInfoServiceApiRequest { get; set; }
        public Func<Task> ProviderRelationshipsApiRequest { get; set; }
        public DateTime? PreRun { get; set; }
        public DateTime? PostRun { get; set; }

        public HealthCheckTestsFixture()
        {
            User = EntityActivator.CreateInstance<User>().Set(u => u.Ref, Guid.NewGuid());
            UnitOfWorkContext = new UnitOfWorkContext();
            ApprenticeshipInfoServiceApiRequest = () => Task.CompletedTask;
            ProviderRelationshipsApiRequest = () => Task.CompletedTask;
        }

        public HealthCheck New()
        {
            return new HealthCheck(User);
        }

        public async Task Run()
        {
            PreRun = DateTime.UtcNow;

            await HealthCheck.Run(ApprenticeshipInfoServiceApiRequest, ProviderRelationshipsApiRequest);

            PostRun = DateTime.UtcNow;
        }

        public void ReceiveEvent()
        {
            PreRun = DateTime.UtcNow;

            HealthCheck.ReceiveProviderRelationshipsEvent();

            PostRun = DateTime.UtcNow;
        }

        public HealthCheckTestsFixture SetHealthCheck()
        {
            HealthCheck = EntityActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 1);

            return this;
        }

        public HealthCheckTestsFixture SetApprenticeshipInfoServiceApiRequestException()
        {
            ApprenticeshipInfoServiceApiRequest = () => throw new Exception();

            return this;
        }
    }
}