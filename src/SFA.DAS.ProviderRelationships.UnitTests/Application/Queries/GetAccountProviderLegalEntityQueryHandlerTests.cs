using System;
using System.Collections.Generic;
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
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    public class GetAccountProviderLegalEntityQueryHandlerTests : FluentTest<GetAccountProviderLegalEntityQueryHandlerFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAccountProviderLegalEntityQuery_ThenShouldReturnGetAccountProviderLegalEntityQueryResult()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                
                r.AccountProvider.Should().NotBeNull().And.BeOfType<AccountProviderBasicDto>()
                    .And.BeEquivalentTo(new AccountProviderBasicDto
                    {
                        Id = f.AccountProvider.Id,
                        ProviderUkprn = f.Provider.Ukprn,
                        ProviderName = f.Provider.Name
                    });
                
                r.AccountLegalEntity.Should().NotBeNull().And.BeOfType<AccountLegalEntityBasicDto>()
                    .And.BeEquivalentTo(new AccountLegalEntityBasicDto
                    {
                        Id = f.AccountLegalEntity.Id,
                        Name = f.AccountLegalEntity.Name
                    });
                
                r.AccountProviderLegalEntity.Should().NotBeNull().And.BeOfType<AccountProviderLegalEntityDto>()
                    .And.BeEquivalentTo(new AccountProviderLegalEntityDto
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
                    });
            });
        }

        [Test]
        public Task Handle_WhenAccountProviderIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.SetAccountLegalEntities(), f => f.Handle(), (f, r) => r.Should().BeNull());
        }

        [Test]
        public Task Handle_WhenAccountLegalEntityIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetAccountProviderLegalEntityQueryHandlerFixture
    {
        public GetAccountProviderLegalEntityQuery Query { get; set; }
        public IRequestHandler<GetAccountProviderLegalEntityQuery, GetAccountProviderLegalEntityQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        public Permission Permission { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        
        public GetAccountProviderLegalEntityQueryHandlerFixture()
        {
            Query = new GetAccountProviderLegalEntityQuery(1, 2, 3);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            ConfigurationProvider = new MapperConfiguration(c =>
            {
                c.AddProfile<AccountLegalEntityMappings>();
                c.AddProfile<AccountProviderMappings>();
                c.AddProfile<AccountProviderLegalEntityMappings>();
                c.AddProfile<PermissionMappings>();
            });
            
            Handler = new GetAccountProviderLegalEntityQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAccountProviderLegalEntityQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountProviderLegalEntityQueryHandlerFixture SetAccountProviderLegalEntities()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);
            Provider = new ProviderBuilder().WithUkprn(11111111).WithName("Foo");
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(Query.AccountProviderId)
                .WithAccountId(Account.Id)
                .WithProviderUkprn(Provider.Ukprn);

            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(Query.AccountLegalEntityId).WithName("Bar").WithAccountId(Account.Id);
            AccountProviderLegalEntity = new AccountProviderLegalEntityBuilder().WithId(4).WithAccountProviderId(AccountProvider.Id).WithAccountLegalEntityId(AccountLegalEntity.Id);
            Permission = new PermissionBuilder().WithId(5).WithAccountProviderLegalEntityId(AccountProviderLegalEntity.Id).WithOperation(Operation.CreateCohort);
            
            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();
            
            return this;
        }

        public GetAccountProviderLegalEntityQueryHandlerFixture SetAccountLegalEntities()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);

            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(Query.AccountLegalEntityId).WithName("Bar").WithAccountId(Account.Id);
            
            Db.Accounts.Add(Account);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();
            
            return this;
        }

        public GetAccountProviderLegalEntityQueryHandlerFixture SetAccountProviders()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);
            Provider = new ProviderBuilder().WithUkprn(11111111).WithName("Foo");
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(Query.AccountProviderId)
                .WithAccountId(Account.Id)
                .WithProviderUkprn(Provider.Ukprn);
            
            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();
            
            return this;
        }
    }
}