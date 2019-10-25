using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationByIdQueryHandlerTests : FluentTest<GetInvitationByIdQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsFound_ThenShouldReturnGetInvitationByIdQueryResult()
        {
            return RunAsync(f => f.SetInvitation(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetInvitationByIdQueryResult>(r2 =>
                    r2.Invitation.EmployerOrganisation == f.Invitation.EmployerOrganisation &&
                    r2.Invitation.Ukprn == f.Invitation.Ukprn));
        }

        [Test]
        public Task Handle_WhenHandlingGetInvitationByIdQueryAndInvitationIsNotFound_ThenShouldReturnNull()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetInvitationByIdQueryHandlerTestsFixture
    { 
        public GetInvitationByIdQuery Query { get; set; }
        public GetInvitationByIdQueryHandler Handler { get; set; }
        public Invitation Invitation { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetInvitationByIdQueryHandlerTestsFixture()
        {
            Query = new GetInvitationByIdQuery(Guid.NewGuid());
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMappings)));
            Handler = new GetInvitationByIdQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetInvitationByIdQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetInvitationByIdQueryHandlerTestsFixture SetInvitation()
        {
            Invitation = EntityActivator.CreateInstance<Invitation>().Set(p => p.Reference, Query.CorrelationId).Set(p => p.EmployerOrganisation, "Foo").Set(p => p.Ukprn, 12345);
            
            Db.Invitations.Add(Invitation);
            Db.SaveChanges();

            return this;
        }
    }
}