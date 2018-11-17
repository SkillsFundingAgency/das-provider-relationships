using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
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
    public class GetAccountProviderUkprnsByAccountIdQueryHandlerTests : FluentTest<GetAccountProviderUkprnsByAccountIdQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAccountProviderUkprnsByAccountIdQuery_ThenShouldRetrnGetAccountProviderUkprnsByAccountIdQueryResult()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().NotBeNull().And.BeOfType<GetAccountProviderUkprnsByAccountIdQueryResult>()
                .Which.Ukprns.Should().NotBeNull().And.BeEquivalentTo(f.Ukprns));
        }
    }

    public class GetAccountProviderUkprnsByAccountIdQueryHandlerTestsFixture
    {
        public List<long> Ukprns { get; set; }
        public GetAccountProviderUkprnsByAccountIdQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IRequestHandler<GetAccountProviderUkprnsByAccountIdQuery, GetAccountProviderUkprnsByAccountIdQueryResult> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }

        public GetAccountProviderUkprnsByAccountIdQueryHandlerTestsFixture()
        {
            Ukprns = new List<long>
            {
                11111111,
                22222222,
                33333333
            };
            
            Query = new GetAccountProviderUkprnsByAccountIdQuery(1);
            CancellationToken = new CancellationToken();
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            Db.AccountProviders.AddRange(Ukprns.Select(u => new AccountProviderBuilder().WithProviderUkprn(u).WithAccountId(Query.AccountId).Build()));
            Db.AccountProviders.Add(new AccountProviderBuilder().WithProviderUkprn(44444444).WithAccountId(2));
            Db.SaveChanges();
            
            Handler = new GetAccountProviderUkprnsByAccountIdQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public Task<GetAccountProviderUkprnsByAccountIdQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }
    }
}