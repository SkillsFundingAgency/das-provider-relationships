using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;
using SFA.DAS.ProviderRegistrations.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetUnsubscribedQueryHandlerTests : FluentTest<GetUnsubscribedQueryHandlerTestsFixture>
    {
        public Task Handle_WhenHandlingGetUnsubscribedQueryAndParametersAreMatched_ThenShouldReturnGetATrueResult()
        {
            return RunAsync(f => f.SetUnsubscribe(), f => f.Handle(), (f, r) =>
            {
                r.Should().BeTrue();
            });
        }
    }

    public class GetUnsubscribedQueryHandlerTestsFixture
    {
        public GetUnsubscribedQuery Query { get; set; }
        public IRequestHandler<GetUnsubscribedQuery, bool> Handler { get; set; }
        public Unsubscribe Unsubscribe { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        
        public GetUnsubscribedQueryHandlerTestsFixture()
        {
            Query = new GetUnsubscribedQuery(12345, "email@email.com");
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Handler = new GetUnsubscribedQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public Task<bool> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetUnsubscribedQueryHandlerTestsFixture SetUnsubscribe()
        {
            Unsubscribe = EntityActivator.CreateInstance<Unsubscribe>().Set(i => i.Ukprn, 12345).Set(i => i.EmailAddress, "email@email.com");
            Db.Unsubscribed.Add(Unsubscribe);
            Db.SaveChanges();
            
            return this;
        }
    }
}