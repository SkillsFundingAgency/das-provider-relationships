using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
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
    public class GetProviderQueryHandlerTests : FluentTest<GetProviderQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAGetProviderQueryAndAProviderIsFound_ThenShouldReturnAGetProviderQueryResponse()
        {
            return RunAsync(f => f.SetProvider(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetProviderQueryResult>(r2 =>
                    r2.Provider.Ukprn == f.Provider.Ukprn &&
                    r2.Provider.Name == f.Provider.Name));
        }

        [Test]
        public Task Handle_WhenHandlingAGetProviderQueryAndAProviderIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetProviderQueryHandlerTestsFixture
    {
        public GetProviderQuery Query { get; set; }
        public GetProviderQueryHandler Handler { get; set; }
        public Provider Provider { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetProviderQueryHandlerTestsFixture()
        {
            Query = new GetProviderQuery(12345678);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfile<ProviderMappings>());
            Handler = new GetProviderQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetProviderQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetProviderQueryHandlerTestsFixture SetProvider()
        {
            Provider = new ProviderBuilder().WithUkprn(Query.Ukprn).WithName("Foo");
            
            Db.Providers.Add(Provider);
            Db.SaveChanges();

            return this;
        }
    }
}