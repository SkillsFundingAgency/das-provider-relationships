using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddAccountLegalEntityCommandHandlerTests : FluentTest<AddAccountLegalEntityCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddAccountLegalEntityCommand_ThenShouldAddAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.Db.AccountLegalEntities.SingleOrDefault(ale => ale.Id == f.Command.AccountLegalEntityId).Should().NotBeNull()
                .And.Match<AccountLegalEntity>(a => 
                    a.Id == f.Command.AccountLegalEntityId &&
                    a.PublicHashedId == f.Command.AccountLegalEntityPublicHashedId &&
                    a.Account == f.Account &&
                    a.AccountId == f.Command.AccountId &&
                    a.Name == f.Command.OrganisationName &&
                    a.Created == f.Command.Created));
        }
    }

    //todo: we could do with a base class for these command test fixtures
    public class AddAccountLegalEntityCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public AddAccountLegalEntityCommand Command { get; set; }
        public IRequestHandler<AddAccountLegalEntityCommand, Unit> Handler { get; set; }
        public Account Account { get; set; }
        
        public AddAccountLegalEntityCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, 1);

            Db.Accounts.Add(Account);
            Db.SaveChanges();
            
            Command = new AddAccountLegalEntityCommand(Account.Id, 2, "ALE123", "Foo", DateTime.UtcNow);
            Handler = new AddAccountLegalEntityCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}