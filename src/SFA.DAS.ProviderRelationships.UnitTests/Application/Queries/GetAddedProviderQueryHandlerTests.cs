using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAddedProviderQueryHandlerTests : FluentTest<GetAddedProviderQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAddedProviderQuery_ThenShouldReturnGetAddedProviderQueryResponse()
        {
            return RunAsync(f => f.SetProvider(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetAddedProviderQueryResponse>(r2 =>
                    r2.AccountProvider.Id == f.AccountProvider.Id &&
                    r2.AccountProvider.Provider.Ukprn == f.Provider.Ukprn &&
                    r2.AccountProvider.Provider.Name == f.Provider.Name));
        }

        [Test]
        public Task Handle_WhenHandlingAGetAddedProviderQueryAndAProviderIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetAddedProviderQueryHandlerTestsFixture
    {
        public GetAddedProviderQuery Query { get; set; }
        public IRequestHandler<GetAddedProviderQuery, GetAddedProviderQueryResponse> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetAddedProviderQueryHandlerTestsFixture()
        {
            Query = new GetAddedProviderQuery
            {
                AccountId = 1,
                AccountProviderId = 12
            };

            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            ConfigurationProvider = new MapperConfiguration(c =>
            {
                c.AddProfile<AccountProviderMappings>();
                c.AddProfile<ProviderMappings>();
            });
            
            Handler = new GetAddedProviderQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAddedProviderQueryResponse> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAddedProviderQueryHandlerTestsFixture SetProvider()
        {
            Account = new AccountBuilder().WithId(Query.AccountId.Value).Build();
            Provider = new ProviderBuilder().WithUkprn(12345678).Build();
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(Query.AccountProviderId.Value)
                .WithAccountId(Account.Id)
                .WithProviderUkprn(Provider.Ukprn)
                .Build();

            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();

            return this;
        }
    }
}