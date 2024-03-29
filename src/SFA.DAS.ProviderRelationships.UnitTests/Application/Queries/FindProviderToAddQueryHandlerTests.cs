﻿using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    public class FindProviderToAddQueryHandlerTests : FluentTest<FindProviderToAddQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderExists_ThenShouldReturnUkprnInFindProviderToAddQueryResult()
        {
            return  TestAsync(f => f.SetProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
            });
        }

        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderDoesNotExist_ThenShouldReturnNullUkprnInFindProviderToAddQueryResult()
        {
            return TestAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().BeNull();
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProvidersExistsAndProviderAlreadyAdded_ThenShouldReturnUkprnAndAccountProviderIdInFindProviderToAddQueryResult()
        {
            return TestAsync(f => f.SetProvider().SetAccountProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
                r.AccountProviderId.Should().Be(f.AccountProvider.Id);
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderExistsAndProviderNotAlreadyAdded_ThenShouldReturnUkprnAndNullAccountProviderIdInFindProviderToAddQueryResult()
        {
            return TestAsync(f => f.SetProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(f.Provider.Ukprn);
                r.AccountProviderId.Should().BeNull();
            });
        }
        
        [Test]
        public Task Handle_WhenHandlingFindProviderToAddQueryAndProviderDoesNotExistAndProviderNotAlreadyAdded_ThenShouldReturnNullUkprnAndNullAccountProviderIdInFindProviderToAddQueryResult()
        {
            return TestAsync(f => f.Handle(), (f, r) =>
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
            Db = new ProviderRelationshipsDbContext(
                new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);
            Handler = new FindProviderToAddQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            Query = new FindProviderToAddQuery(1, 12345678);
        }

        public Task<FindProviderToAddQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public FindProviderToAddQueryHandlerTestsFixture SetProvider()
        {
            Provider = EntityActivator.CreateInstance<Provider>()
                .Set(p => p.Ukprn, Query.Ukprn)
                .Set(p => p.Name, Guid.NewGuid().ToString());

            Db.Providers.Add(Provider);
            Db.SaveChanges();

            return this;
        }

        public FindProviderToAddQueryHandlerTestsFixture SetAccountProvider()
        {
            Account = EntityActivator.CreateInstance<Account>()
                .Set(a => a.Id, Query.AccountId);
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>()
                .Set(ap => ap.Id, 1)
                .Set(ap => ap.AccountId, Account.Id)
                .Set(ap => ap.ProviderUkprn, Provider.Ukprn);
            
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();
            
            return this;
        }
    }
}