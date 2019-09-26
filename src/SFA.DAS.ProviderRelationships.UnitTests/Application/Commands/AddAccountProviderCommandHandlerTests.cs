using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddAccountProviderCommandHandlerTests : FluentTest<AddAccountProviderCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddAccountProviderCommand_ThenShouldAddAccountProvider()
        {
            return RunAsync(f => f.Handle(), f => f.Db.AccountProviders.SingleOrDefault().Should().NotBeNull()
                .And.Match<AccountProvider>(ap => ap.Account == f.Account && ap.Provider == f.Provider && ap.Created >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenHandlingAddAccountProviderCommand_ThenShouldPublishAddedAccountProviderEvent()
        {
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<AddedAccountProviderEvent>(e => e.AccountId == f.Account.Id && e.ProviderUkprn == f.Provider.Ukprn && e.UserRef == f.User.Ref && e.Added >= f.Now));
        }
        
        [Test]
        public Task Handle_WhenHandlingAddAccountProviderCommand_ThenShouldReturnAccountProviderId()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().Be(f.Db.AccountProviders.Select(ap => ap.Id).Single()));
        }
        
        [Test]
        public Task Handle_WhenHandlingAddAccountProviderCommandAndProviderHasBeenAddedAlready_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetAccountProvider(), f => f.Handle(), (f, r) => r.Should().Throw<Exception>());
        }
    }

    public class AddAccountProviderCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public AddAccountProviderCommand Command { get; set; }
        public IRequestHandler<AddAccountProviderCommand, long> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public User User { get; set; }
        public DateTime Now { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }

        public AddAccountProviderCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, 1);
            User = EntityActivator.CreateInstance<User>().Set(u => u.Ref, Guid.NewGuid());
            Provider = EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, 12345678);

            Db.Accounts.Add(Account);
            Db.Users.Add(User);
            Db.Providers.Add(Provider);
            Db.SaveChanges();
            
            Command = new AddAccountProviderCommand(Account.Id, Provider.Ukprn, User.Ref);
            Now = DateTime.UtcNow;
            UnitOfWorkContext = new UnitOfWorkContext();
            Handler = new AddAccountProviderCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public Task<long> Handle()
        {
            return Handler.Handle(Command, CancellationToken.None);
        }

        public AddAccountProviderCommandHandlerTestsFixture SetAccountProvider()
        {
            Db.AccountProviders.Add(EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, Provider.Ukprn));
            Db.SaveChanges();
            
            return this;
        }
    }
}