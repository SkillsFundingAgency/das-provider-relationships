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
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
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
            return TestAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();

                r.AccountProviders.Should().NotBeNull().And.BeEquivalentTo(
                    new List<AccountProviderDto>{
                    new AccountProviderDto {
                        Id = f.AccountProvider.Id,
                        ProviderName = f.Provider.Name,
                        ProviderUkprn = f.Provider.Ukprn,
                        AccountLegalEntities = new List<AccountLegalEntityDto> {
                            new AccountLegalEntityDto {
                                Id = 3,
                                HadPermissions = true,
                                Name = f.AccountLegalEntities.Single(entity => entity.Id == 3).Name,
                                Operations = new List<Operation> {Operation.CreateCohort}
                            },
                            new AccountLegalEntityDto {
                                Id = 4,
                                HadPermissions = false,
                                Name = f.AccountLegalEntities.Single(entity => entity.Id == 4).Name,
                                Operations = new List<Operation>()
                            }
                        }
                    }});
                
                r.AccountLegalEntitiesCount.Should().Be(f.AccountLegalEntities.Count(ale => ale.AccountId == f.Query.AccountId));
            });
        }

        [Test]
        public Task Handle_WhenNoProvidersAdded_ThenShouldReturnGetAddedProvidersQueryResultWithEmptyProvidersList()
        {
            return TestAsync(f => f.Handle(), (f, r) => 
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
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        public ProviderRelationships.Models.Permission Permission { get; set; }
        public List<AccountLegalEntity> AccountLegalEntities { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetAccountProvidersQueryHandlerTestsFixture()
        {
            Query = new GetAccountProvidersQuery(1);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ConfigurationProvider = new MapperConfiguration(
                c => c.AddProfiles(new List<Profile> {
                    new AccountProviderMappings(),
                    new AccountLegalEntityMappings()
                }));
            Handler = new GetAccountProvidersQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAccountProvidersQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountProvidersQueryHandlerTestsFixture SetAccountProviders()
        {
            Account = EntityActivator.CreateInstance<Account>()
                .Set(a => a.Id, Query.AccountId)
                .Set(a => a.Name, Guid.NewGuid().ToString())
                .Set(a => a.HashedId, Guid.NewGuid().ToString())
                .Set(a => a.PublicHashedId, Guid.NewGuid().ToString());
            Provider = EntityActivator.CreateInstance<Provider>()
                .Set(p => p.Ukprn, 12345678)
                .Set(p => p.Name, "Foo");
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>()
                .Set(ap => ap.Id, 2)
                .Set(ap => ap.AccountId, Account.Id)
                .Set(ap => ap.ProviderUkprn, Provider.Ukprn);
          
            AccountLegalEntities = new List<AccountLegalEntity>
            {
                EntityActivator.CreateInstance<AccountLegalEntity>()
                    .Set(ale => ale.Id, 3)
                    .Set(ale => ale.AccountId, Account.Id)
                    .Set(ale => ale.Name, Guid.NewGuid().ToString())
                    .Set(ale => ale.PublicHashedId, Guid.NewGuid().ToString()),
                EntityActivator.CreateInstance<AccountLegalEntity>()
                    .Set(ale => ale.Id, 4)
                    .Set(ale => ale.AccountId, Account.Id)
                    .Set(ale => ale.Name, Guid.NewGuid().ToString())
                    .Set(ale => ale.PublicHashedId, Guid.NewGuid().ToString()),
                EntityActivator.CreateInstance<AccountLegalEntity>()
                    .Set(ale => ale.Id, 5)
                    .Set(ale => ale.AccountId, Account.Id+1)
                    .Set(ale => ale.Name, Guid.NewGuid().ToString())
                    .Set(ale => ale.PublicHashedId, Guid.NewGuid().ToString())
            };

            AccountProviderLegalEntity = EntityActivator.CreateInstance<AccountProviderLegalEntity>()
                .Set(aple => aple.Id, 8)
                .Set(aple => aple.AccountLegalEntityId, 3)
                .Set(aple => aple.AccountProviderId, 2);

            Permission = EntityActivator.CreateInstance<ProviderRelationships.Models.Permission>()
                .Set(p => p.Id, 4)
                .Set(p => p.Operation, Operation.CreateCohort)
                .Set(p => p.AccountProviderLegalEntityId, 8);

            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.AddRange(AccountLegalEntities);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();

            return this;
        }
    }
}