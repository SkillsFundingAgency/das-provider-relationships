using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountOverview;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAccountOverviewQueryHandlerTests : FluentTest<GetAccountOverviewQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetProviderOverviewQuery_ThenShouldReturnGetProviderOverviewQueryResult()
        {
            return RunAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.AccountProvidersCount.Should().Be(f.AccountProviders.Count(ap => ap.AccountId == f.Query.AccountId));
                r.AccountLegalEntitiesCount.Should().Be(f.AccountLegalEntities.Count(ale => ale.AccountId == f.Query.AccountId));
            });
        }

        [Test]
        public Task Handle_WhenNoProvidersAdded_ThenShouldReturnGetProviderOverviewQueryResultWithNoProviders()
        {
            return RunAsync(f => f.Handle(), (f, r) => 
            {
                r.Should().NotBeNull();
                r.AccountProvidersCount.Should().Be(0);
            });
        }

        [Test]
        public Task Handle_WhenUserIsNotOwner_ThenShouldReturnGetProviderOverviewQueryResultWithAddProviderOperationUnauthorizedAndAddUpdatePermissionsOperationUnauthorized()
        {
            return RunAsync(f => f.Handle(), (f, r) => 
            {
                r.Should().NotBeNull();
                r.IsAddProviderOperationAuthorized.Should().BeFalse();
                r.IsUpdatePermissionsOperationAuthorized.Should().BeFalse();
            });
        }

        [Test]
        public Task Handle_WhenUserIsOwner_ThenShouldReturnGetProviderOverviewQueryResultWithAddProviderOperationAuthorizedAndUpdatePermissionsOperationAuthorized()
        {
            return RunAsync(f => f.SetOwner(), f => f.Handle(), (f, r) => 
            {
                r.Should().NotBeNull();
                r.IsAddProviderOperationAuthorized.Should().BeTrue();
                r.IsUpdatePermissionsOperationAuthorized.Should().BeTrue();
            });
        }
    }

    public class GetAccountOverviewQueryHandlerTestsFixture
    {
        public GetAccountOverviewQuery Query { get; set; }
        public IRequestHandler<GetAccountOverviewQuery, GetAccountOverviewQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public List<Provider> Providers { get; set; }
        public List<AccountProvider> AccountProviders { get; set; }
        public List<AccountLegalEntity> AccountLegalEntities { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }

        public GetAccountOverviewQueryHandlerTestsFixture()
        {
            Query = new GetAccountOverviewQuery(1);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderMappings)));
            AuthorizationService = new Mock<IAuthorizationService>();
            Handler = new GetAccountOverviewQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), AuthorizationService.Object);
        }

        public Task<GetAccountOverviewQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountOverviewQueryHandlerTestsFixture SetAccountProviders()
        {
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, Query.AccountId);

            Providers = new List<Provider> {
                EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, 12345678).Set(p => p.Name, "Foo"),
                EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, 87654321).Set(p => p.Name, "Bar")
            };

            AccountProviders = new List<AccountProvider> {
                EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.Id, 2).Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, Providers[0].Ukprn),
                EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.Id, 3).Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, Providers[1].Ukprn)
            };
            
            AccountLegalEntities = new List<AccountLegalEntity>
            {
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 3).Set(ale => ale.AccountId, Account.Id),
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 4).Set(ale => ale.AccountId, Account.Id),
                EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 5).Set(ale => ale.AccountId, 2)
            };
            
            Db.Accounts.Add(Account);
            Db.Providers.AddRange(Providers);
            Db.AccountProviders.AddRange(AccountProviders);
            Db.AccountLegalEntities.AddRange(AccountLegalEntities);
            Db.SaveChanges();

            return this;
        }

        public GetAccountOverviewQueryHandlerTestsFixture SetOwner()
        {
            AuthorizationService.Setup(a => a.IsAuthorizedAsync(EmployerUserRole.Owner)).ReturnsAsync(true);
            
            return this;
        }
    }
}