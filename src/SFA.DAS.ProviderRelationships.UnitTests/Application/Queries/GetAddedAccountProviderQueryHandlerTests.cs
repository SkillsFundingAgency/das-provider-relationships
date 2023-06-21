using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAddedAccountProviderQueryHandlerTests : FluentTest<GetAddedAccountProviderQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAddedProviderQuery_ThenShouldReturnGetAddedProviderQueryResult()
        {
            return TestAsync(f => f.SetAccountProvider(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetAddedAccountProviderQueryResult>(r2 =>
                    r2.AccountProvider.Id == f.AccountProvider.Id &&
                    r2.AccountProvider.ProviderUkprn == f.Provider.Ukprn &&
                    r2.AccountProvider.ProviderName == f.Provider.Name));
        }

        [Test]
        public Task Handle_WhenHandlingGetAddedProviderQueryAndProviderIsNotFound_ThenShouldReturnNull()
        {
            return TestAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetAddedAccountProviderQueryHandlerTestsFixture
    {
        public GetAddedAccountProviderQuery Query { get; set; }
        public IRequestHandler<GetAddedAccountProviderQuery, GetAddedAccountProviderQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetAddedAccountProviderQueryHandlerTestsFixture()
        {
            Query = new GetAddedAccountProviderQuery(1, 2);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(new List<Profile>{new AccountProviderMappings()}));
            
            Handler = new GetAddedAccountProviderQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAddedAccountProviderQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAddedAccountProviderQueryHandlerTestsFixture SetAccountProvider()
        {
            Account = EntityActivator.CreateInstance<Account>()
                .Set(a => a.Id, Query.AccountId)
                .Set(a => a.Name, Guid.NewGuid().ToString())
                .Set(a => a.HashedId, Guid.NewGuid().ToString())
                .Set(a => a.PublicHashedId, Guid.NewGuid().ToString());
            Provider = EntityActivator.CreateInstance<Provider>()
                .Set(p => p.Ukprn, 12345678)
                .Set(p => p.Name, Guid.NewGuid().ToString());
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>()
                .Set(ap => ap.Id, Query.AccountProviderId)
                .Set(ap => ap.AccountId, Account.Id)
                .Set(ap => ap.ProviderUkprn, Provider.Ukprn);

            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();

            return this;
        }
    }
}