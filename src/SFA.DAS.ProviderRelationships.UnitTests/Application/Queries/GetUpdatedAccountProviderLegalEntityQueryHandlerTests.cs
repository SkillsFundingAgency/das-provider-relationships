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
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetUpdatedAccountProviderLegalEntityQueryHandlerTests : FluentTest<GetUpdatedAccountProviderLegalEntityQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetUpdatedAccountProviderLegalEntityQuery_ThenShouldReturnGetUpdatedAccountProviderLegalEntityQueryResult()
        {
            return TestAsync(f => f.SetAccountProviderLegalEntity(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetUpdatedAccountProviderLegalEntityQueryResult>(r2 =>
                    r2.AccountProviderLegalEntity.Id == f.AccountProviderLegalEntity.Id &&
                    r2.AccountProviderLegalEntity.ProviderName == f.Provider.Name &&
                    r2.AccountProviderLegalEntity.AccountLegalEntityName == f.AccountLegalEntity.Name &&
                    r2.AccountLegalEntitiesCount == f.AccountLegalEntities.Count(ale => ale.AccountId == f.Query.AccountId)));
        }

        [Test]
        public Task Handle_WhenAccountProviderLegalEntityIsNotFound_ThenShouldReturnNull()
        {
            return TestAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetUpdatedAccountProviderLegalEntityQueryHandlerTestsFixture
    {
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public List<AccountLegalEntity> AccountLegalEntities { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public GetUpdatedAccountProviderLegalEntityQuery Query { get; set; }
        public IRequestHandler<GetUpdatedAccountProviderLegalEntityQuery, GetUpdatedAccountProviderLegalEntityQueryResult> Handler { get; set; }

        public GetUpdatedAccountProviderLegalEntityQueryHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(new List<Profile>(){new AccountProviderLegalEntityMappings()}));
            Query = new GetUpdatedAccountProviderLegalEntityQuery(1, 3, 4);
            Handler = new GetUpdatedAccountProviderLegalEntityQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetUpdatedAccountProviderLegalEntityQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetUpdatedAccountProviderLegalEntityQueryHandlerTestsFixture SetAccountProviderLegalEntity()
        {
            Account = EntityActivator.CreateInstance<Account>()
                .Set(a => a.Id, Query.AccountId);
            
            Provider = EntityActivator.CreateInstance<Provider>()
                .Set(p => p.Ukprn, 12345678)
                .Set(p => p.Name, "Provider");
            
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>()
                .Set(ap => ap.Id, Query.AccountProviderId)
                .Set(ap => ap.AccountId, Account.Id)
                .Set(ap => ap.Provider, Provider);
            
            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>()
                .Set(ale => ale.Id, Query.AccountLegalEntityId)
                .Set(ale => ale.Name, "Account legal entity A")
                .Set(ale => ale.AccountId, Account.Id)
                .Set(ale => ale.PublicHashedId, Guid.NewGuid().ToString());
            
            AccountLegalEntities = new List<AccountLegalEntity>
            {
                AccountLegalEntity,
                EntityActivator.CreateInstance<AccountLegalEntity>()
                    .Set(ale => ale.Id, 5)
                    .Set(ale => ale.Name, "Account legal entity B")
                    .Set(ale => ale.AccountId, Account.Id)
                    .Set(ale => ale.PublicHashedId, Guid.NewGuid().ToString()),
                EntityActivator.CreateInstance<AccountLegalEntity>()
                    .Set(ale => ale.Id, 6)
                    .Set(ale => ale.Name, "Account legal entity C")
                    .Set(ale => ale.AccountId, 2)
                    .Set(ale => ale.PublicHashedId, Guid.NewGuid().ToString())
            };
            
            AccountProviderLegalEntity = EntityActivator.CreateInstance<AccountProviderLegalEntity>()
                .Set(aple => aple.Id, 7)
                .Set(aple => aple.AccountProvider, AccountProvider)
                .Set(aple => aple.AccountLegalEntity, AccountLegalEntity);

            Db.AccountLegalEntities.AddRange(AccountLegalEntities);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.SaveChanges();
            
            return this;
        }
    }
}