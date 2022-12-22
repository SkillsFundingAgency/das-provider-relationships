using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAccountProviderLegalEntityQueryHandlerTests : FluentTest<GetAccountProviderLegalEntityQueryHandlerFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAccountProviderLegalEntityQuery_ThenShouldReturnGetAccountProviderLegalEntityQueryResult()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(12345678), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();

                r.AccountProvider.Should().NotBeNull().And.BeOfType<AccountProviderDto>()
                    .And.BeEquivalentTo(new AccountProviderDto {
                        Id = f.AccountProvider.Id,
                        ProviderUkprn = f.Provider.Ukprn,
                        ProviderName = f.Provider.Name
                    });

                r.AccountLegalEntity.Should().NotBeNull().And.BeOfType<AccountLegalEntityDto>()
                    .And.BeEquivalentTo(new AccountLegalEntityDto {
                        Id = f.AccountLegalEntity.Id,
                        Name = f.AccountLegalEntity.Name
                    });

                r.AccountProviderLegalEntity.Should().NotBeNull().And.BeOfType<AccountProviderLegalEntityDto>()
                    .And.BeEquivalentTo(new AccountProviderLegalEntityDto {
                        Id = f.AccountProviderLegalEntity.Id,
                        AccountLegalEntityId = f.AccountLegalEntity.Id,
                        Operations = new List<Operation>
                        {
                            f.Permission.Operation
                        }
                    });

                r.AccountLegalEntitiesCount.Should().Be(f.AccountLegalEntities.Count(ale => ale.AccountId == f.Query.AccountId));
            });
        }

        [Test]
        public Task Handle_WhenHandlingGetAccountProviderLegalEntityQueryForNotBlockedProvider_ThenShouldReturnGetAccountProviderLegalEntityQueryResult()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(12345678), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();

                r.IsProviderBlockedFromRecruit.Should().BeFalse();
            });
        }

        [Test]
        public Task Handle_WhenHandlingGetAccountProviderLegalEntityQueryForBlockedProvider_ThenShouldReturnGetAccountProviderLegalEntityQueryResult()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(GetAccountProviderLegalEntityQueryHandlerFixture.BlockedRecruitProviderUkrpn), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();

                r.IsProviderBlockedFromRecruit.Should().BeTrue();
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
        public const long BlockedRecruitProviderUkrpn = 11112222;
        public GetAccountProviderLegalEntityQuery Query { get; set; }
        public IRequestHandler<GetAccountProviderLegalEntityQuery, GetAccountProviderLegalEntityQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public List<AccountLegalEntity> AccountLegalEntities { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        public ProviderRelationships.Models.Permission Permission { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public Mock<IDasRecruitService> MockRecruitService { get; set; }

        public GetAccountProviderLegalEntityQueryHandlerFixture()
        {
            Query = new GetAccountProviderLegalEntityQuery(1, 2, 3);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderLegalEntityMappings)));
            MockRecruitService = new Mock<IDasRecruitService>();
            SetDasRecruitBlockedProvider();
            Handler = new GetAccountProviderLegalEntityQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), MockRecruitService.Object, ConfigurationProvider);
        }

        public Task<GetAccountProviderLegalEntityQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountProviderLegalEntityQueryHandlerFixture SetAccountProviderLegalEntities(long ukprn)
        {
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, Query.AccountId);
            Provider = EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, ukprn).Set(p => p.Name, "Foo");
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.Id, Query.AccountProviderId).Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, Provider.Ukprn);
            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, Query.AccountLegalEntityId).Set(ale => ale.Name, "Bar").Set(ale => ale.AccountId, Account.Id);

            AccountLegalEntities = new List<AccountLegalEntity>
            {
                AccountLegalEntity,
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 4).Set(ale => ale.AccountId, Account.Id),
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 5).Set(ale => ale.AccountId, 6)
            };

            AccountProviderLegalEntity = EntityActivator.CreateInstance<AccountProviderLegalEntity>().Set(aple => aple.Id, 7).Set(aple => aple.AccountProviderId, AccountProvider.Id).Set(aple => aple.AccountLegalEntityId, AccountLegalEntity.Id);
            Permission = EntityActivator.CreateInstance<ProviderRelationships.Models.Permission>().Set(p => p.Id, 8).Set(p => p.AccountProviderLegalEntityId, AccountProviderLegalEntity.Id).Set(p => p.Operation, Operation.CreateCohort);
            
            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.AddRange(AccountLegalEntities);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();

            return this;
        }

        public GetAccountProviderLegalEntityQueryHandlerFixture SetAccountLegalEntities()
        {
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, Query.AccountId);
            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, Query.AccountLegalEntityId).Set(ale => ale.Name, "Bar").Set(ale => ale.AccountId, Account.Id);
            
            Db.Accounts.Add(Account);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();
            
            return this;
        }

        public GetAccountProviderLegalEntityQueryHandlerFixture SetAccountProviders()
        {
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, Query.AccountId);
            Provider = EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, 12345678).Set(p => p.Name, "Foo");
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.Id, Query.AccountProviderId).Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, Provider.Ukprn);
            
            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();
            
            return this;
        }

        public GetAccountProviderLegalEntityQueryHandlerFixture SetDasRecruitBlockedProvider()
        {
            MockRecruitService.Setup(x => x.GetProviderBlockedStatusAsync(BlockedRecruitProviderUkrpn, default)).ReturnsAsync(new BlockedOrganisationStatus { Status = BlockedOrganisationStatusConstants.Blocked });
            MockRecruitService.Setup(x => x.GetProviderBlockedStatusAsync(It.IsNotIn(new[] { BlockedRecruitProviderUkrpn }), default)).ReturnsAsync(new BlockedOrganisationStatus { Status = BlockedOrganisationStatusConstants.NotBlocked });
            return this;
        }

    }
}