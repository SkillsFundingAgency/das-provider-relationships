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
    public class UpdateAccountNameCommandHandlerTests : FluentTest<UpdateAccountNameCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandledChronologically_ThenShouldUpdateAccountName()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.Command.Name);
                f.Account.Updated.Should().Be(f.Command.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledChronologically_ThenShouldPublishUpdatedAccountNameEvent()
        {
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdatedAccountNameEvent>(e =>
                    e.AccountId == f.Account.Id &&
                    e.Name == f.Account.Name &&
                    e.Created == f.Account.Updated));
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledNonChronologically_ThenShouldNotUpdateAccountName()
        {
            return RunAsync(f => f.SetAccountUpdatedAfterCommand(), f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.OriginalAccountName);
                f.Account.Updated.Should().Be(f.Now);
            });
        }
    }

    public class UpdateAccountNameCommandHandlerTestsFixture
    {
        public Account Account { get; set; }
        public UpdateAccountNameCommand Command { get; set; }
        public IRequestHandler<UpdateAccountNameCommand, Unit> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public string OriginalAccountName { get; set; }
        public DateTime Now { get; set; }

        public UpdateAccountNameCommandHandlerTestsFixture()
        {
            OriginalAccountName = "Foo";
            Now = DateTime.UtcNow;
            Account = new AccountBuilder().WithId(1).WithName(OriginalAccountName);
            Command = new UpdateAccountNameCommand(Account.Id, "Bar", Now.AddHours(-1));
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            Db.Accounts.Add(Account);
            Db.SaveChanges();

            Handler = new UpdateAccountNameCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public UpdateAccountNameCommandHandlerTestsFixture SetAccountUpdatedAfterCommand()
        {
            Account.SetPropertyTo(a => a.Updated, Now);
            Db.SaveChanges();
            
            return this;
        }
    }
}