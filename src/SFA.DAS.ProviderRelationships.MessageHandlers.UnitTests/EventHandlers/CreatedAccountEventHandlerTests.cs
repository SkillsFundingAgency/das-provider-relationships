using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class CreatedAccountEventHandlerTests : FluentTest<CreatedAccountEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldAddAccount()
        {
            return RunAsync(f => f.Handle(), f => f.Db.Accounts.SingleOrDefault().Should().NotBeNull()
                .And.Match<Account>(a => 
                    a.Id == f.Message.AccountId &&
                    a.Name == f.Message.Name &&
                    a.Created == f.Message.Created));
        }
    }

    public class CreatedAccountEventHandlerTestsFixture
    {
        public DateTime Now { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public CreatedAccountEvent Message { get; set; }
        public IHandleMessages<CreatedAccountEvent> Handler { get; set; }

        public CreatedAccountEventHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            Message = new CreatedAccountEvent
            {
                AccountId = 123,
                Name = "Acme",
                Created = DateTime.UtcNow
            };
            
            Handler = new CreatedAccountEventHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Message, null);
            await Db.SaveChangesAsync();
        }
    }
}