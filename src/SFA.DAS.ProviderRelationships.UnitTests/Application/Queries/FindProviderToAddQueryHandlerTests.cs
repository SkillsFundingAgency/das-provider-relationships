using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class FindProviderToAddQueryHandlerTests : FluentTest<FindProviderToAddQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderExists_ThenShouldReturnUkprnInFindProviderToAddQueryResult()
        {
            return RunAsync(f => f.SetProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
            });
        }

        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderDoesNotExist_ThenShouldReturnNullUkprnInFindProviderToAddQueryResult()
        {
            return RunAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().BeNull();
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProvidersExistsAndProviderAlreadyAdded_ThenShouldReturnUkprnAndAccountProviderIdInFindProviderToAddQueryResult()
        {
            return RunAsync(f => f.SetProvider().SetAccountProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
                r.AccountProviderId.Should().Be(f.AccountProvider.Id);
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderExistsAndProviderNotAlreadyAdded_ThenShouldReturnUkprnAndNullAccountProviderIdInFindProviderToAddQueryResult()
        {
            return RunAsync(f => f.SetProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
                r.AccountProviderId.Should().BeNull();
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderDoesNotExistAndProviderNotAlreadyAdded_ThenShouldReturnNullUkprnAndNullAccountProviderIdInFindProviderToAddQueryResult()
        {
            return RunAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().BeNull();
                r.AccountProviderId.Should().BeNull();
            });
        }
    }

    public class FindProviderToAddQueryHandlerTestsFixture
    {
        public FindProviderToAddQueryHandler Handler { get; set; }
        public FindProviderToAddQuery Query { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public Provider Provider { get; set; }
        public Account Account { get; set; }
        public AccountProvider AccountProvider { get; set; }

        public FindProviderToAddQueryHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Handler = new FindProviderToAddQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            Query = new FindProviderToAddQuery(1, 12345678);
        }

        public Task<FindProviderToAddQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public FindProviderToAddQueryHandlerTestsFixture SetProvider()
        {
            Provider = new ProviderBuilder().WithUkprn(Query.Ukprn);
            
            Db.Providers.Add(Provider);
            Db.SaveChanges();

            return this;
        }

        public FindProviderToAddQueryHandlerTestsFixture SetAccountProvider()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);
            AccountProvider = new AccountProviderBuilder().WithId(1).WithAccountId(Account.Id).WithProviderUkprn(Provider.Ukprn);
            
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();
            
            return this;
        }
    }
}