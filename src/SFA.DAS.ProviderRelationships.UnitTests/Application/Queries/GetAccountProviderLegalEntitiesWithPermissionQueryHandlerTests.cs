using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries;

[TestFixture]
[Parallelizable]
public class GetAccountProviderLegalEntitiesWithPermissionQueryHandlerTests : FluentTest<GetAccountProviderLegalEntitiesWithPermissionQueryHandlerTestsFixture>
{
    [Test]
    public Task Handle_WhenUkprnIsFoundAndAccountProviderLegalEntityHasPermissionForSuppliedOperation_ThenShouldReturnCorrectGetAccountProviderLegalEntitiesWithPermissionQueryResult()
    {
        return TestAsync(f => f.SetAccountProviderLegalEntities(), f => f.Handle(), (f, r) =>
        {
            r.Should().NotBeNull();
            r.Should().BeEquivalentTo(new GetAccountProviderLegalEntitiesWithPermissionQueryResult(new[] {
                new AccountProviderLegalEntityDto
                {
                    AccountId = f.Account.Id,
                    AccountHashedId = f.Account.HashedId,
                    AccountPublicHashedId = f.Account.PublicHashedId,
                    AccountName = f.Account.Name,
                    AccountProviderId = f.AccountProvider.Id,
                    AccountLegalEntityId = f.AccountLegalEntity.Id,
                    AccountLegalEntityPublicHashedId = f.AccountLegalEntity.PublicHashedId,
                    AccountLegalEntityName = f.AccountLegalEntity.Name
                }
            }));
        });
    }

    [Test]
    public Task Handle_WhenUkprnIsFoundButAccountLegalEntityIsDeleted_ThenShouldReturnGetAccountProviderLegalEntitiesWithPermissionQueryResultWithEmptyAccountProviderLegalEntitiesDtos()
    {
        return TestAsync(f => f.SetAccountProviderLegalEntities().SetAccountLegalEntityDeleted(),
            f => f.Handle(), 
            (_, r) => AssertEmptyResult(r));
    }

    [Test]
    public Task Handle_WhenUkprnIsFoundAndAccountProviderLegalEntityHasNotGotPermissionForSuppliedOperation_ThenShouldReturnGetAccountProviderLegalEntitiesWithPermissionQueryResultWithEmptyAccountProviderLegalEntitiesDtos()
    {
        return TestAsync(f => f.SetAccountProviderLegalEntities().RemovePermission(), 
            f => f.Handle(), 
            (_, r) => AssertEmptyResult(r));
    }

    [Test]
    public Task Handle_WhenUkprnIsNotFound_ThenShouldReturnGetAccountProviderLegalEntitiesWithPermissionQueryResultWithEmptyAccountProviderLegalEntitiesDtos()
    {
        return TestAsync(f => f.Handle(), 
            (_, r) => AssertEmptyResult(r));
    }

    public static void AssertEmptyResult(GetAccountProviderLegalEntitiesWithPermissionQueryResult result)
    {
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new GetAccountProviderLegalEntitiesWithPermissionQueryResult(Array.Empty<AccountProviderLegalEntityDto>()));
    }
}

public class GetAccountProviderLegalEntitiesWithPermissionQueryHandlerTestsFixture
{
    public GetAccountProviderLegalEntitiesWithPermissionQueryHandler Handler { get; set; }
    public GetAccountProviderLegalEntitiesWithPermissionQuery Query { get; set; }
    public ProviderRelationshipsDbContext Db { get; set; }
    public IConfigurationProvider ConfigurationProvider { get; set; }
    public Account Account { get; set; }
    public AccountProvider AccountProvider { get; set; }
    public AccountLegalEntity AccountLegalEntity { get; set; }
    public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
    public ProviderRelationships.Models.Permission Permission { get; set; }

    public GetAccountProviderLegalEntitiesWithPermissionQueryHandlerTestsFixture()
    {
        Query = new GetAccountProviderLegalEntitiesWithPermissionQuery(
            88888888, 
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(), 
            new List<Operation>{Operation.Recruitment, Operation.RecruitmentRequiresReview});

        Db = new ProviderRelationshipsDbContext(
            new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        ConfigurationProvider = new MapperConfiguration(c => c.AddProfile<AccountProviderLegalEntityMappings>());
        Handler = new GetAccountProviderLegalEntitiesWithPermissionQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
    }
        
    public Task<GetAccountProviderLegalEntitiesWithPermissionQueryResult> Handle()
    {
        return Handler.Handle(Query, CancellationToken.None);
    }
        
    public GetAccountProviderLegalEntitiesWithPermissionQueryHandlerTestsFixture SetAccountProviderLegalEntities()
    {
        // need to new up one of these, so its static Events is there
        new UnitOfWorkContext();

        Account = EntityActivator.CreateInstance<Account>()
            .Set(a => a.Id, 4660117L)
            .Set(a => a.HashedId, Query.AccountHashedId)
            .Set(a => a.Name, Guid.NewGuid().ToString())
            .Set(a => a.PublicHashedId, Guid.NewGuid().ToString());
            
        AccountProvider = EntityActivator.CreateInstance<AccountProvider>()
            .Set(ap => ap.Id, 49L)
            .Set(ap => ap.AccountId, Account.Id)
            .Set(ap => ap.ProviderUkprn, Query.Ukprn.GetValueOrDefault());

        AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>()
            .Set(ale =>ale.Id,413L)
            .Set(ale => ale.Name, "Legal Entity Name")
            .Set(ale => ale.PublicHashedId, Query.AccountLegalEntityPublicHashedId)
            .Set(ale => ale.AccountId, Account.Id);
            
        AccountProviderLegalEntity = EntityActivator.CreateInstance<AccountProviderLegalEntity>()
            .Set(aple =>aple.Id, 5)
            .Set(aple => aple.AccountProviderId, AccountProvider.Id)
            .Set(aple => aple.AccountLegalEntityId, AccountLegalEntity.Id);
            
        Permission = EntityActivator.CreateInstance<ProviderRelationships.Models.Permission>()
            .Set(p => p.Id, 3)
            .Set(p => p.AccountProviderLegalEntityId, AccountProviderLegalEntity.Id)
            .Set(p => p.Operation, Query.Operations.First());
            
        //todo: if these don't clone, we should
        Db.Accounts.Add(Account);
        Db.AccountProviders.Add(AccountProvider);
        Db.AccountLegalEntities.Add(AccountLegalEntity);
        Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
        Db.Permissions.Add(Permission);
        Db.SaveChanges();
            
        return this;
    }

    public GetAccountProviderLegalEntitiesWithPermissionQueryHandlerTestsFixture SetAccountLegalEntityDeleted()
    {
        Db.Accounts.First().RemoveAccountLegalEntity(Db.AccountLegalEntities.First(), new DateTime(2018, 11, 26));
        Db.SaveChanges();
        return this;
    }
        
    public GetAccountProviderLegalEntitiesWithPermissionQueryHandlerTestsFixture RemovePermission()
    {
        Db.Permissions.RemoveRange(Db.Permissions);
        Db.SaveChanges();
        return this;
    }
}