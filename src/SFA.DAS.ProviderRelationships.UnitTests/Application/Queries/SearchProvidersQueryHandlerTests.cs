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
    public class SearchProvidersQueryHandlerTests : FluentTest<SearchProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndProviderExists_ThenShouldReturnUkprnInSearchProvidersQueryResponse()
        {
            return RunAsync(f => f.SetProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
            });
        }

        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndProviderDoesNotExist_ThenShouldReturnNullUkprnInSearchProvidersQueryResponse()
        {
            return RunAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().BeNull();
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndPProvidersDoesExistAndroviderAlreadyAdded_ThenShouldReturnUkprnAndAccountProviderIdInSearchProvidersQueryResponse()
        {
            return RunAsync(f => f.SetProvider().SetAccountProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
                r.AccountProviderId.Should().Be(f.AccountProvider.Id);
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndProviderDoesExistAndProviderNotAlreadyAdded_ThenShouldReturnUkprnAndNullAccountProviderIdInSearchProvidersQueryResponse()
        {
            return RunAsync(f => f.SetProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
                r.AccountProviderId.Should().BeNull();
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndProviderDoesNotExistAndProviderNotAlreadyAdded_ThenShouldReturnUkprnAndNullAccountProviderIdInSearchProvidersQueryResponse()
        {
            return RunAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().BeNull();
                r.AccountProviderId.Should().BeNull();
            });
        }
    }

    public class SearchProvidersQueryHandlerTestsFixture
    {
        public SearchProvidersQueryHandler Handler { get; set; }
        public SearchProvidersQuery Query { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public Provider Provider { get; set; }
        public Account Account { get; set; }
        public AccountProvider AccountProvider { get; set; }

        public SearchProvidersQueryHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Handler = new SearchProvidersQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            Query = new SearchProvidersQuery(1, 12345678);
        }

        public Task<SearchProvidersQueryResponse> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public SearchProvidersQueryHandlerTestsFixture SetProvider()
        {
            Provider = new ProviderBuilder().WithUkprn(Query.Ukprn);
            
            Db.Providers.Add(Provider);
            Db.SaveChanges();

            return this;
        }

        public SearchProvidersQueryHandlerTestsFixture SetAccountProvider()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);
            AccountProvider = new AccountProviderBuilder().WithId(1).WithAccountId(Account.Id).WithProviderUkprn(Provider.Ukprn);
            
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();
            
            return this;
        }
    }
}
