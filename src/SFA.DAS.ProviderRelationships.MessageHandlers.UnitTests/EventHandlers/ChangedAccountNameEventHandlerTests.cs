using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using Z.EntityFramework.Plus;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenEventIsHandledChronologically_ThenShouldChangeAccountName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.Message.CurrentName);
                f.Account.Updated.Should().Be(f.Message.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledNonChronologically_ThenShouldNotChangeAccountName()
        {
            return RunAsync(f => f.SetAccountUpdatedAfterEvent(), f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.OriginalAccountName);
                f.Account.Updated.Should().Be(f.Now);
            });
        }
    }

    public class ChangedAccountNameEventHandlerTestsFixture : EventHandlerTestsFixture<ChangedAccountNameEvent>
    {
        public string OriginalAccountName { get; set; }
        public Account Account { get; set; }

        public ChangedAccountNameEventHandlerTestsFixture()
            : base(db => new ChangedAccountNameEventHandler(db))
        {
            OriginalAccountName = "Foo";
            
            Message = new ChangedAccountNameEvent
            {
                AccountId = 1,
                CurrentName = "Bar",
                Created = Now.AddHours(-1)
            };

            Account = new AccountBuilder()
                .WithId(Message.AccountId)
                .WithName(OriginalAccountName);

            Db.Accounts.Add(Account);
            Db.SaveChanges();
        }

        public ChangedAccountNameEventHandlerTestsFixture SetAccountUpdatedAfterEvent()
        {
            Account.SetPropertyTo(a => a.Updated, Now);
            Db.SaveChanges();
            
            return this;
        }
    }
}