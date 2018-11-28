using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAccountProvidersQueryHandlerTests : FluentTest<GetAccountProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAddedProvidersQuery_ThenShouldReturnGetAddedProvidersQueryResult()
        {
            return RunAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                
                r.AccountProviders.Should().NotBeNull().And.BeEquivalentTo(
                    new AccountProviderSummaryDto
                    {
                        Id = f.AccountProvider.Id,
                        ProviderName = f.Provider.Name
                    });
                
                r.AccountLegalEntitiesCount.Should().Be(f.AccountLegalEntities.Count(ale => ale.AccountId == f.Query.AccountId));
            });
        }

        [Test]
        public Task Handle_WhenHandlingGetAddedProvidersQueryNoProvidersAdded_ThenShouldReturnGetAddedProvidersQueryResponseWithEmptyProvidersList()
        {
            return RunAsync(f => f.Handle(), (f, r) => 
            {
                r.Should().NotBeNull();
                r.AccountProviders.Should().NotBeNull().And.BeEmpty();
            });
        }
    }

    public class GetAccountProvidersQueryHandlerTestsFixture
    {
        public GetAccountProvidersQuery Query { get; set; }
        public IRequestHandler<GetAccountProvidersQuery, GetAccountProvidersQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public List<AccountLegalEntity> AccountLegalEntities { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetAccountProvidersQueryHandlerTestsFixture()
        {
            Query = new GetAccountProvidersQuery(1);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderMappings)));
            Handler = new GetAccountProvidersQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAccountProvidersQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountProvidersQueryHandlerTestsFixture SetAccountProviders()
        {
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, Query.AccountId);
            Provider = EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, 12345678).Set(p => p.Name, "Foo");
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.Id, 2).Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, Provider.Ukprn);
          
            AccountLegalEntities = new List<AccountLegalEntity>
            {
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 3).Set(ale => ale.AccountId, Account.Id),
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 4).Set(ale => ale.AccountId, Account.Id),
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 5).Set(ale => ale.AccountId, 2)
            };
            
            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.AddRange(AccountLegalEntities);
            Db.SaveChanges();

            return this;
        }
    }
}