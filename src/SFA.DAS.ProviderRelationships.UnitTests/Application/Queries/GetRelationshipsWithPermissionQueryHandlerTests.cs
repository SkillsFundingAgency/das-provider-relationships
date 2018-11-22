using System;
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
        public Task Handle_WhenHandlingRelationshipsWithPermissionQuery_ThenShouldReturnCorrectGetRelationshipsWithPermissionQueryResult()
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
            Account = new AccountBuilder().WithId(4660117L);
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(49L)
                .WithAccountId(Account.Id)
                .WithProviderUkprn(Query.Ukprn);

            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(413L).WithName("Legal Entity Name").WithAccountId(Account.Id);
            AccountProviderLegalEntity = new AccountProviderLegalEntityBuilder().WithId(5).WithAccountProviderId(AccountProvider.Id).WithAccountLegalEntityId(AccountLegalEntity.Id);
            Permission = new PermissionBuilder().WithId(3).WithAccountProviderLegalEntityId(AccountProviderLegalEntity.Id).WithOperation(Query.Operation);
            
            //todo: if these don't clone, we should
            Db.Accounts.Add(Account);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();
            
            return this;
        }
    }
}