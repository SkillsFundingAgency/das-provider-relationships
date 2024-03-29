using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountLegalEntityName;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdateAccountLegalEntityNameCommandHandlerTests : FluentTest<UpdateAccountLegalEntityNameCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandledChronologically_ThenShouldUpdateAccountLegalEntityName()
        {
            return TestAsync(f => f.Handle(), f =>
            {
                f.AccountLegalEntity.Name.Should().Be(f.Command.Name);
                f.AccountLegalEntity.Updated.Should().Be(f.Command.Created);
            });
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledNonChronologically_ThenShouldNotUpdateAccountLegalEntityName()
        {
            return TestAsync(f => f.SetAccountLegalEntityUpdatedAfterCommand(), f => f.Handle(), f =>
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
        public IRequestHandler<UpdateAccountLegalEntityNameCommand> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public string OriginalAccountLegalEntityName { get; set; }
        public DateTime Now { get; set; }

        public UpdateAccountLegalEntityNameCommandHandlerTestsFixture()
        {
            OriginalAccountLegalEntityName = "Foo";
            Now = DateTime.UtcNow;
            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>()
                .Set(ale => ale.Id, 1)
                .Set(ale => ale.Name, OriginalAccountLegalEntityName)
                .Set(ale => ale.PublicHashedId, "something")
                .Set(ale => ale.AccountId, 3L);
            Command = new UpdateAccountLegalEntityNameCommand(AccountLegalEntity.Id, "Bar", Now.AddHours(-1));
            Db = new ProviderRelationshipsDbContext(
                new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

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
            AccountLegalEntity.Set(a => a.Updated, Now);
            Db.SaveChanges();
            
            return this;
        }
    }
}