using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    [TestFixture]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingChangedAccountNameEvent_ThenShouldChangeAccountName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.Message.CurrentName);
                f.Account.Updated.Should().Be(f.Message.Created);
                f.Account.UpdatedAt.Should().BeAfter(f.Now);
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingChangedAccountNameEventOutOfOrder_ThenShouldNotChangeAccountName()
        {
            return RunAsync(f => f.SetAccountPreviouslyUpdated(), f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.Message.PreviousName);
                f.Account.Updated.Should().BeBefore(f.Now);
                f.Account.UpdatedAt.Should().BeNull();
            });
        }
    }

    public class ChangedAccountNameEventHandlerTestsFixture
    {
        public DateTime Now { get; set; }
        public ChangedAccountNameEvent Message { get; set; }
        public Account Account { get; set; }
        public IHandleMessages<ChangedAccountNameEvent> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public DbContextOptions<ProviderRelationshipsDbContext> DbContextOptions { get; set; }

        public ChangedAccountNameEventHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            
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
            
            DbContextOptions = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Db = new ProviderRelationshipsDbContext(DbContextOptions);

            Db.Accounts.Add(Account);
            Db.SaveChanges();
            
            Handler = new ChangedAccountNameEventHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Message, null);
            await Db.SaveChangesAsync();
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