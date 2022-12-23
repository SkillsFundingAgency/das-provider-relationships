using System;
using System.Collections.Generic;
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
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
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
    public class GetAccountProviderQueryHandlerTests : FluentTest<GetAccountProviderQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAccountProviderQuery_ThenShouldReturnGetAccountProviderQueryResult()
        {
            return TestAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                
                r.AccountProvider.Should().NotBeNull().And.BeOfType<AccountProviderDto>()
                    .And.BeEquivalentTo(new AccountProviderDto
                    {
                        Id = f.AccountProvider.Id,
                        ProviderUkprn = f.Provider.Ukprn,
                        ProviderName = f.Provider.Name,
                        AccountLegalEntities = new List<AccountLegalEntityDto>
                        {
                            new AccountLegalEntityDto
                            {
                                Id = f.AccountLegalEntity.Id,
                                Name = f.AccountLegalEntity.Name,
                                HadPermissions = true,
                                Operations = new List<Operation>
                                {
                                    f.Permission.Operation
                                }
                            }
                        }
                    });
            });
        }

        [Test]
        public Task Handle_WhenAccountProviderIsNotFound_ThenShouldReturnNull()
        {
            return TestAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }

        [Test]
        public Task Handle_WhenUserIsNotOwner_ThenShouldReturnGetAccountProviderQueryResultWithUpdatePermissionsOperationUnauthorized()
        {
            return TestAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsUpdatePermissionsOperationAuthorized.Should().BeFalse();
            });
        }

        [Test]
        public Task Handle_WhenUserIsNotOwner_ThenShouldReturnGetAccountProviderQueryResultWithUpdatePermissionsOperationAuthorized()
        {
            return TestAsync(f => f.SetAccountProviders().SetOwner(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.IsUpdatePermissionsOperationAuthorized.Should().BeTrue();
            });
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
        public ProviderRelationships.Models.Permission Permission { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
        
        public GetAccountProviderQueryHandlerTestsFixture()
        {
            Query = new GetAccountProviderQuery(1, 2);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(new List<Profile>{ new AccountProviderMappings()}));
            AuthorizationService = new Mock<IAuthorizationService>();
            Handler = new GetAccountProviderQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider, AuthorizationService.Object);
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
            Permission = EntityActivator.CreateInstance<ProviderRelationships.Models.Permission>().Set(p => p.Id, 5).Set(p => p.AccountProviderLegalEntityId, AccountProviderLegalEntity.Id).Set(p => p.Operation, Operation.CreateCohort);
            
            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            Db.Permissions.Add(Permission);
            Db.SaveChanges();
            
            return this;
        }

        public GetAccountProviderQueryHandlerTestsFixture SetOwner()
        {
            AuthorizationService.Setup(a => a.IsAuthorizedAsync(EmployerUserRole.Owner)).ReturnsAsync(true);
            
            return this;
        }
    }
}