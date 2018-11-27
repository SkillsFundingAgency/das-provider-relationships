using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
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
    public class GetRelationshipsWithPermissionQueryHandlerTests : FluentTest<GetRelationshipsWithPermissionQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenUkprnIsFoundAndRelationshipHasPermissionForSuppliedOperation_ThenShouldReturnCorrectGetRelationshipsWithPermissionQueryResult()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Should().BeEquivalentTo(new GetRelationshipsWithPermissionQueryResult(new[] {
                    new RelationshipDto
                    {
                        //todo: why are we returning what was passed in to us?
                        Ukprn = f.Query.Ukprn,
                        EmployerAccountId = f.Account.Id,
                        EmployerAccountPublicHashedId = f.Account.PublicHashedId,
                        EmployerAccountName = f.Account.Name,
                        EmployerAccountProviderId = f.AccountProvider.Id,
                        EmployerAccountLegalEntityId = f.AccountLegalEntity.Id,
                        EmployerAccountLegalEntityPublicHashedId = f.AccountLegalEntity.PublicHashedId,
                        EmployerAccountLegalEntityName = f.AccountLegalEntity.Name
                    }
                }));
            });
        }

        [Test, Ignore("needs latest master")]
        public Task Handle_WhenUkprnIsFoundButAccountLegalEntityIsDeleted_ThenShouldReturnGetRelationshipsWithPermissionQueryResultWithEmptyRelationshipDtos()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities().SetAccountLegalEntityDeleted(), f => f.Handle(),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeEquivalentTo(new GetRelationshipsWithPermissionQueryResult(new RelationshipDto[0]));
                });
        }

        [Test, Ignore("for now")]
        public Task Handle_WhenUkprnIsFoundAndRelationshipHasNotGotPermissionForSuppliedOperation_ThenShouldReturnCorrectGetRelationshipsWithPermissionQueryResult()
        {
            throw new NotImplementedException();
        }

        [Test, Ignore("for now")]
        public Task Handle_WhenUkprnIsNotFound_ThenShouldReturnSumfink()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }

        
        [Test, Ignore("for now")]
        public Task Handle_WhenHandlingRelationshipsWithPermissionQueryAndUkprnIsNotFound_ThenShouldReturnSumfink()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetRelationshipsWithPermissionQueryHandlerTestsFixture
    {
        public GetRelationshipsWithPermissionQueryHandler Handler { get; set; }
        public GetRelationshipsWithPermissionQuery Query { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public Account Account { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        public Permission Permission { get; set; }

        public GetRelationshipsWithPermissionQueryHandlerTestsFixture()
        {
            Query = new GetRelationshipsWithPermissionQuery(88888888, Operation.CreateCohort);

            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfile<AccountProviderLegalEntityMappings>());
            Handler = new GetRelationshipsWithPermissionQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }
        
        public Task<GetRelationshipsWithPermissionQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }
        
        public GetRelationshipsWithPermissionQueryHandlerTestsFixture SetAccountProviderLegalEntities()
        {
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, 4660117L);
            
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>()
                .Set(ap => ap.Id, 49L)
                .Set(ap => ap.AccountId, Account.Id)
                .Set(ap =>ap.ProviderUkprn, Query.Ukprn);

            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>()
                .Set(ale =>ale.Id,413L)
                .Set(ale => ale.Name, "Legal Entity Name")
                .Set(ale => ale.AccountId, Account.Id);
            
            AccountProviderLegalEntity = EntityActivator.CreateInstance<AccountProviderLegalEntity>()
                .Set(aple =>aple.Id, 5)
                .Set(aple => aple.AccountProviderId, AccountProvider.Id)
                .Set(aple => aple.AccountLegalEntityId, AccountLegalEntity.Id);
            
            Permission = EntityActivator.CreateInstance<Permission>()
                .Set(p => p.Id, 3)
                .Set(p => p.AccountProviderLegalEntityId, AccountProviderLegalEntity.Id)
                .Set(p => p.Operation, Query.Operation);
            
            //todo: if these don't clone, we should
            Db.Accounts.Add(Account);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();
            
            return this;
        }

        public GetRelationshipsWithPermissionQueryHandlerTestsFixture SetAccountLegalEntityDeleted()
        {
            Db.Accounts.First().RemoveAccountLegalEntity(Db.AccountLegalEntities.First(), new DateTime(2018, 11, 26));
            Db.SaveChanges();
            return this;
        }
    }
}