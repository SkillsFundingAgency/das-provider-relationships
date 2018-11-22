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
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetRelationshipsWithPermissionQueryHandlerTests : FluentTest<GetRelationshipsWithPermissionQueryHandlerTestsFixture>
    {
        [Test, Ignore("not written yet, just want to check in progress")]
        public Task Handle_WhenHandlingRelationshipsWithPermissionQuery_ThenShouldReturnCorrectGetRelationshipsWithPermissionQueryResult()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(), f => f.Handle(), (f, r) => r.Should().NotBeNull());
            //todo:
//                .And.Match<GetRelationshipsWithPermissionQueryResult>(qr =>
//                    qr.Relationships.Provider.Ukprn == f.Provider.Ukprn &&
//                    r2.Provider.Name == f.Provider.Name));
        }
    }

    public class GetRelationshipsWithPermissionQueryHandlerTestsFixture
    {
        public GetRelationshipsWithPermissionQueryHandler Handler { get; set; }
        public GetRelationshipsWithPermissionQuery Query { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
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
        
//        var relationships = await _db.Value.AccountProviderLegalEntities
//            .Where(aple => aple.AccountProvider.ProviderUkprn == request.Ukprn
//                           && aple.Permissions.Any(p => p.Operation == request.Operation))
//            //.OrderBy(ap => ap.Provider.Name)
//            .Include(aple => aple.AccountProvider)
//            .ThenInclude(ap => ap.Account)
//            .Include(aple => aple.AccountLegalEntity)
//            .ProjectTo<RelationshipDto>(_configurationProvider)
//            .ToListAsync(cancellationToken);
        
        public GetRelationshipsWithPermissionQueryHandlerTestsFixture SetAccountProviderLegalEntities()
        {
            Account = new AccountBuilder().WithId(4660117L);
            //Provider = new ProviderBuilder().WithUkprn(Query.Ukprn).WithName("Provider Name");
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(49L)
                .WithAccountId(Account.Id)
                .WithProviderUkprn(Provider.Ukprn);

            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(413L).WithName("Legal Entity Name").WithAccountId(Account.Id);
            AccountProviderLegalEntity = new AccountProviderLegalEntityBuilder().WithId(5).WithAccountProviderId(AccountProvider.Id).WithAccountLegalEntityId(AccountLegalEntity.Id);
            Permission = new PermissionBuilder().WithId(3).WithAccountProviderLegalEntityId(AccountProviderLegalEntity.Id).WithOperation(Query.Operation);
            
            Db.Accounts.Add(Account);
            //Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();
            
            return this;
        }
    }
}