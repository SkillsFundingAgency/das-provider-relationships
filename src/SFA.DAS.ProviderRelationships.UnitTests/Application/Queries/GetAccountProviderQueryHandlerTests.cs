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
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAccountProviderQueryHandlerTests : FluentTest<GetAccountProviderQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAccountProviderQuery_ThenShouldReturnGetAccountProviderQueryResult()
        {
            return RunAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                
                r.AccountProvider.Should().NotBeNull().And.BeOfType<AccountProviderDto>()
                    .And.BeEquivalentTo(new AccountProviderDto
                    {
                        Id = f.AccountProvider.Id,
                        ProviderUkprn = f.Provider.Ukprn,
                        ProviderName = f.Provider.Name,
                        AccountProviderLegalEntities = new List<AccountProviderLegalEntitySummaryDto>
                        {
                            new AccountProviderLegalEntitySummaryDto
                            {
                                Id = f.AccountProviderLegalEntity.Id,
                                AccountLegalEntityId = f.AccountLegalEntity.Id,
                                Permissions = new List<PermissionDto>
                                {
                                    new PermissionDto
                                    {
                                        Id = f.Permission.Id,
                                        Operation = f.Permission.Operation
                                    }
                                }
                            }
                        }
                    });
                
                r.AccountLegalEntities.Should().NotBeNull().And.BeOfType<List<AccountLegalEntityBasicDto>>()
                    .And.BeEquivalentTo(new List<AccountLegalEntityBasicDto>
                    {
                        new AccountLegalEntityBasicDto
                        {
                            Id = f.AccountLegalEntity.Id,
                            Name = f.AccountLegalEntity.Name
                        }
                    });
            });
        }

        [Test]
        public Task Handle_WhenAccountProviderIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetAccountProviderQueryHandlerTestsFixture
    {
        public GetAccountProviderQuery Query { get; set; }
        public IRequestHandler<GetAccountProviderQuery, GetAccountProviderQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        public Permission Permission { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        
        public GetAccountProviderQueryHandlerTestsFixture()
        {
            Query = new GetAccountProviderQuery(1, 2);
            
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderMappings)));
            
            Handler = new GetAccountProviderQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAccountProviderQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountProviderQueryHandlerTestsFixture SetAccountProviders()
        {
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, Query.AccountId);
            Provider = EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, 12345678).Set(p => p.Name, "Foo");
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.Id, Query.AccountProviderId).Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, Provider.Ukprn);
            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 3).Set(ale => ale.Name, "Bar").Set(ale => ale.AccountId, Account.Id);
            AccountProviderLegalEntity = EntityActivator.CreateInstance<AccountProviderLegalEntity>().Set(aple => aple.Id, 4).Set(aple => aple.AccountProviderId, AccountProvider.Id).Set(aple => aple.AccountLegalEntityId, AccountLegalEntity.Id);
            Permission = EntityActivator.CreateInstance<Permission>().Set(p => p.Id, 5).Set(p => p.AccountProviderLegalEntityId, AccountProviderLegalEntity.Id).Set(p => p.Operation, Operation.CreateCohort);
            
            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();
            
            return this;
        }
    }
}