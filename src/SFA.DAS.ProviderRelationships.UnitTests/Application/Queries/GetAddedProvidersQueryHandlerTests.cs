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
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAddedProvidersQueryHandlerTests : FluentTest<GetAddedProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAddedProvidersQuery_ThenShouldReturnGetAddedProvidersQueryResponse()
        {
            return RunAsync(f => f.SetProviders(), f => f.Handle(), (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.AccountProviders.Should().NotBeNull();
                    r.AccountProviders.Should().BeEquivalentTo(new AccountProviderDto
                    {
                        Id = f.AccountProvider.Id,
                        Provider = new ProviderDto
                        {
                            Ukprn = f.Provider.Ukprn,
                            Name = f.Provider.Name
                        }
                    });
                });
        }

        [Test]
        public Task Handle_WhenHandlingGetAddedProvidersQueryNoProvidersAdded_ThenShouldReturnGetAddedProvidersQueryResponseWithEmptyProvidersList()
        {
            return RunAsync(f => f.Handle(), (f, r) => 
            {
                r.Should().NotBeNull();
                r.AccountProviders.Should().NotBeNull();
                r.AccountProviders.Should().BeEmpty();
            });
        }
    }

    public class GetAddedProvidersQueryHandlerTestsFixture
    {
        public GetAddedProvidersQuery Query { get; set; }
        public IRequestHandler<GetAddedProvidersQuery, GetAddedProvidersQueryResponse> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetAddedProvidersQueryHandlerTestsFixture()
        {
            Query = new GetAddedProvidersQuery(5555);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            ConfigurationProvider = new MapperConfiguration(c =>
            {
                c.AddProfile<AccountProviderMappings>();
                c.AddProfile<ProviderMappings>();
            });
            
            Handler = new GetAddedProvidersQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAddedProvidersQueryResponse> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAddedProvidersQueryHandlerTestsFixture SetProviders()
        {
            Account = new AccountBuilder().WithId(Query.AccountId).Build();
            Provider = new ProviderBuilder().WithUkprn(12345678).WithName("ProviderName").Build();
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(999)
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