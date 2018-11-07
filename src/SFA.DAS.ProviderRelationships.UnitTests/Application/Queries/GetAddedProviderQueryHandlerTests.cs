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
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
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
                .And.Match<GetAddedAccountProviderQueryResult>(r2 =>
                    r2.AddedAccountProvider.Id == f.AccountProvider.Id &&
                    r2.AddedAccountProvider.Provider.Ukprn == f.Provider.Ukprn &&
                    r2.AddedAccountProvider.Provider.Name == f.Provider.Name));
        }

        [Test]
        public Task Handle_WhenHandlingAGetAddedProviderQueryAndAProviderIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetAddedProviderQueryHandlerTestsFixture
    {
        public GetAddedAccountProviderQuery Query { get; set; }
        public IRequestHandler<GetAddedAccountProviderQuery, GetAddedAccountProviderQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetAddedProviderQueryHandlerTestsFixture()
        {
            Query = new GetAddedAccountProviderQuery(1, 2);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            ConfigurationProvider = new MapperConfiguration(c =>
            {
                c.AddProfile<AccountProviderMappings>();
                c.AddProfile<ProviderMappings>();
            });
            
            Handler = new GetAddedAccountProviderQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAddedAccountProviderQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAddedProviderQueryHandlerTestsFixture SetProvider()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);
            Provider = new ProviderBuilder().WithUkprn(12345678);
            
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