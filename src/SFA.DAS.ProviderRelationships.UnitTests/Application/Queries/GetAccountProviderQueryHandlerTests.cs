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
                                AccountProviderId = f.AccountProvider.Id,
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
                
                r.AccountLegalEntities.Should().NotBeNull().And.BeOfType<List<AccountLegalEntityDto>>()
                    .And.BeEquivalentTo(new List<AccountLegalEntityDto>
                    {
                        new AccountLegalEntityDto
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
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            ConfigurationProvider = new MapperConfiguration(c =>
            {
                c.AddProfile<AccountLegalEntityMappings>();
                c.AddProfile<AccountProviderMappings>();
                c.AddProfile<AccountProviderLegalEntityMappings>();
                c.AddProfile<PermissionMappings>();
            });
            
            Handler = new GetAccountProviderQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAccountProviderQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountProviderQueryHandlerTestsFixture SetAccountProviders()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);
            Provider = new ProviderBuilder().WithUkprn(11111111).WithName("Foo");
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(Query.AccountProviderId)
                .WithAccountId(Account.Id)
                .WithProviderUkprn(Provider.Ukprn);

            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(4).WithName("Bar").WithAccountId(Account.Id);
            AccountProviderLegalEntity = new AccountProviderLegalEntityBuilder().WithId(5).WithAccountProviderId(AccountProvider.Id).WithAccountLegalEntityId(AccountLegalEntity.Id);
            Permission = new PermissionBuilder().WithId(3).WithAccountProviderLegalEntityId(AccountProviderLegalEntity.Id).WithOperation(Operation.CreateCohort);
            
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