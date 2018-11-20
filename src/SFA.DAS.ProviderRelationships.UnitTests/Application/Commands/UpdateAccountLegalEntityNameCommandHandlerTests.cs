using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdateAccountLegalEntityNameCommandHandlerTests : FluentTest<UpdateAccountLegalEntityNameCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandledChronologically_ThenShouldUpdateAccountLegalEntityName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(f.Command.Name);
                f.AccountLegalEntity.Updated.Should().Be(f.Command.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledChronologically_ThenShouldPublishUpdatedAccountLegalEntityNameEvent()
        {
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdatedAccountLegalEntityNameEvent>(e =>
                    e.AccountLegalEntityId == f.AccountLegalEntity.Id &&
                    e.AccountId == f.AccountLegalEntity.AccountId &&
                    e.Name == f.AccountLegalEntity.Name &&
                    e.Updated == f.AccountLegalEntity.Updated));
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledNonChronologically_ThenShouldNotUpdateAccountLegalEntityName()
        {
            return RunAsync(f => f.SetAccountLegalEntityUpdatedAfterCommand(), f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(f.OriginalAccountLegalEntityName);
                f.AccountLegalEntity.Updated.Should().Be(f.Now);
            });
        }
    }

    public class UpdateAccountLegalEntityNameCommandHandlerTestsFixture
    {
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public UpdateAccountLegalEntityNameCommand Command { get; set; }
        public IRequestHandler<UpdateAccountLegalEntityNameCommand, Unit> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public string OriginalAccountLegalEntityName { get; set; }
        public DateTime Now { get; set; }

        public UpdateAccountLegalEntityNameCommandHandlerTestsFixture()
        {
            OriginalAccountLegalEntityName = "Foo";
            Now = DateTime.UtcNow;
            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(1).WithName(OriginalAccountLegalEntityName);
            Command = new UpdateAccountLegalEntityNameCommand(AccountLegalEntity.Id, "Bar", Now.AddHours(-1));
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();

            Handler = new UpdateAccountLegalEntityNameCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public UpdateAccountLegalEntityNameCommandHandlerTestsFixture SetAccountLegalEntityUpdatedAfterCommand()
        {
            AccountLegalEntity.SetPropertyTo(a => a.Updated, Now);
            Db.SaveChanges();
            
            return this;
        }
    }
}