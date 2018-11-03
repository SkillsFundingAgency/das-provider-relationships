using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class SearchProvidersQueryHandlerTests : FluentTest<SearchProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndAProviderExists_ThenShouldReturnValidSearchProvidersQueryResponse()
        {
            return RunAsync(f => f.SetProvider(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(long.Parse(f.Query.Ukprn));
                r.ProviderExists.Should().BeTrue();
            });
        }

        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndAProviderDoesNotExist_ThenShouldReturnInvalidSearchProvidersQueryResponse()
        {
            return RunAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(long.Parse(f.Query.Ukprn));
                r.ProviderExists.Should().BeFalse();
            });
        }
    }

    public class SearchProvidersQueryHandlerTestsFixture
    {
        public SearchProvidersQueryHandler Handler { get; set; }
        public SearchProvidersQuery Query { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }

        public SearchProvidersQueryHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Handler = new SearchProvidersQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            Query = new SearchProvidersQuery("12345678");
        }

        public Task<SearchProvidersQueryResponse> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public SearchProvidersQueryHandlerTestsFixture SetProvider()
        {
            var provider = new ProviderBuilder().WithUkprn(long.Parse(Query.Ukprn)).Build();
            
            Db.Providers.Add(provider);
            Db.SaveChanges();

            return this;
        }
    }
}
