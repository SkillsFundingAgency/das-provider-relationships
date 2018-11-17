using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetAccountProviderUkprnsQueryHandlerTests : FluentTest<GetAccountProviderUkprnsQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAccountProviderUkprnsQuery_ThenShouldRetrnGetAccountProviderUkprnsQueryResult()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().NotBeNull().And.BeOfType<GetAccountProviderUkprnsQueryResult>()
                .Which.Ukprns.Should().NotBeNull().And.BeEquivalentTo(f.Ukprns));
        }
    }

    public class GetAccountProviderUkprnsQueryHandlerTestsFixture
    {
        public List<long> Ukprns { get; set; }
        public GetAccountProviderUkprnsQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public GetAccountProviderUkprnsQueryHandler Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }

        public GetAccountProviderUkprnsQueryHandlerTestsFixture()
        {
            Ukprns = new List<long>
            {
                11111111,
                22222222,
                33333333
            };
            
            Query = new GetAccountProviderUkprnsQuery(1);
            CancellationToken = new CancellationToken();
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            Db.AccountProviders.AddRange(Ukprns.Select(u => new AccountProviderBuilder().WithProviderUkprn(u).WithAccountId(Query.AccountId).Build()));
            Db.AccountProviders.Add(new AccountProviderBuilder().WithProviderUkprn(44444444).WithAccountId(2));
            Db.SaveChanges();
            
            Handler = new GetAccountProviderUkprnsQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public Task<GetAccountProviderUkprnsQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }
    }
}