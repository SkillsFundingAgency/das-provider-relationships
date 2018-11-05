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
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddAccountProviderCommandTests : FluentTest<AddAccountProviderCommandTestsFixture>
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
                .And.Match<AddedAccountProviderEvent>(e => e.AccountId == f.Account.Id && e.ProviderUkprn == f.Provider.Ukprn && e.UserRef == f.User.Ref && e.Created >= f.Now));
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

    public class AddAccountProviderCommandTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public AddAccountProviderCommand Command { get; set; }
        public IRequestHandler<AddAccountProviderCommand, int> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public User User { get; set; }
        public DateTime Now { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }

        public AddAccountProviderCommandTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            Account = new AccountBuilder().WithId(1);
            User = new UserBuilder().WithRef(Guid.NewGuid());
            Provider = new ProviderBuilder().WithUkprn(12345678);

            Db.Accounts.Add(Account);
            Db.Users.Add(User);
            Db.Providers.Add(Provider);
            Db.SaveChanges();
            
            Command = new AddAccountProviderCommand(Account.Id, User.Ref, Provider.Ukprn);
            Now = DateTime.UtcNow;
            UnitOfWorkContext = new UnitOfWorkContext();
            Handler = new AddAccountProviderCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public Task<int> Handle()
        {
            return Handler.Handle(Command, CancellationToken.None);
        }

        public AddAccountProviderCommandTestsFixture SetAccountProvider()
        {
            Db.AccountProviders.Add(new AccountProviderBuilder().WithAccountId(Account.Id).WithProviderUkprn(Provider.Ukprn));
            Db.SaveChanges();
            
            return this;
        }
    }
}