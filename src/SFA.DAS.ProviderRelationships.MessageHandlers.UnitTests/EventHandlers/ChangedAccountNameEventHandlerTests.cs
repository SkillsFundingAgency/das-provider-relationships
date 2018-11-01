using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingChangedAccountNameEvent_ThenShouldChangeAccountName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.Message.CurrentName);
                f.Account.Updated.Should().Be(f.Message.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingChangedAccountNameEventOutOfOrder_ThenShouldNotChangeAccountName()
        {
            return RunAsync(f => f.SetAccountPreviouslyUpdated(), f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.Message.PreviousName);
                f.Account.Updated.Should().BeBefore(f.Now);
            });
        }
    }

    public class ChangedAccountNameEventHandlerTestsFixture : EventHandlerTestsFixture<ChangedAccountNameEvent>
    {
        public Account Account { get; set; }

        public ChangedAccountNameEventHandlerTestsFixture()
            : base(db => new ChangedAccountNameEventHandler(db))
        {
            Message = new ChangedAccountNameEvent
            {
                AccountId = 123,
                PreviousName = "Acme",
                CurrentName = "Acme Inc",
                Created = DateTime.UtcNow
            };

            Account = new AccountBuilder()
                .WithId(Message.AccountId)
                .WithName(Message.PreviousName)
                .Build();

            Db.Accounts.Add(Account);
            Db.SaveChanges();
        }

        public ChangedAccountNameEventHandlerTestsFixture SetAccountPreviouslyUpdated()
        {
            Account = new AccountBuilder()
                .WithId(Message.AccountId)
                .WithName(Message.PreviousName)
                .WithUpdated(DateTime.UtcNow.AddHours(-1))
                .Build();
            
            return this;
        }
    }
}