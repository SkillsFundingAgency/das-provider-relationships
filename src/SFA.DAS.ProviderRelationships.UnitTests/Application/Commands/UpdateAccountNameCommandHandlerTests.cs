using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountName;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdateAccountNameCommandHandlerTests : FluentTest<UpdateAccountNameCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandledChronologically_ThenShouldUpdateAccountName()
        {
            return TestAsync(f => f.Handle(), f =>
            {
                f.Account.Name.Should().Be(f.Command.Name);
                f.Account.Updated.Should().Be(f.Command.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledNonChronologically_ThenShouldNotUpdateAccountName()
        {
            return TestAsync(f => f.SetAccountUpdatedAfterCommand(), f => f.Handle(), f =>
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
        public IRequestHandler<UpdateAccountNameCommand> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public string OriginalAccountName { get; set; }
        public DateTime Now { get; set; }

        public UpdateAccountNameCommandHandlerTestsFixture()
        {
            OriginalAccountName = "Foo";
            Now = DateTime.UtcNow;
            Account = EntityActivator.CreateInstance<Account>()
                .Set(a => a.Id, 1)
                .Set(a => a.Name, OriginalAccountName)
                .Set(a => a.HashedId, Guid.NewGuid().ToString())
                .Set(a => a.PublicHashedId, Guid.NewGuid().ToString());
            Command = new UpdateAccountNameCommand(Account.Id, "Bar", Now.AddHours(-1));
            Db = new ProviderRelationshipsDbContext(
                new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

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
            Account.Set(a => a.Updated, Now);
            Db.SaveChanges();
            
            return this;
        }
    }
}