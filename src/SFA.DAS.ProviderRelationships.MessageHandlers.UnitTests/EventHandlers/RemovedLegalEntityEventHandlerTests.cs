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
using SFA.DAS.ProviderRelationships.UnitTests;
using SFA.DAS.Testing;
using Z.EntityFramework.Plus;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class RemovedLegalEntityEventHandlerTests : FluentTest<RemovedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingRemovedLegalEntityEvent_ThenShouldDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.Db.AccountLegalEntities.Should().BeEmpty());
        }
    }

    public class RemovedLegalEntityEventHandlerTestsFixture
    {
        public DateTime Now { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public RemovedLegalEntityEvent Message { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public IHandleMessages<RemovedLegalEntityEvent> Handler { get; set; }

        public RemovedLegalEntityEventHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            
            Message = new RemovedLegalEntityEvent
            {
                AccountLegalEntityId = 456,
                Created = DateTime.UtcNow
            };
            
            AccountLegalEntity = new AccountLegalEntityBuilder()
                .WithId(Message.AccountLegalEntityId)
                .Build();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            
            BatchDeleteManager.InMemoryDbContextFactory = () => new ProviderRelationshipsDbContext(dbContextOptionsBuilder.Options);
            
            Db = new ProviderRelationshipsDbContext(dbContextOptionsBuilder.Options);

            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();
            
            Handler = new RemovedLegalEntityEventHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Message, null);
            await Db.SaveChangesAsync();
        }
    }
}