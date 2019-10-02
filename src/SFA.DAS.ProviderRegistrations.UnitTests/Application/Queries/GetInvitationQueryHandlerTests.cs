using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetInvitationQueryHandlerTests : FluentTest<GetInvitationQueryHandlerTestsFixture>
    {
        public Task Handle_WhenHandlingGetInvitationQueryAndProviderIsFound_ThenShouldReturnGetInvitationQueryResult()
        {
            return RunAsync(f => f.SetInvitations(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetInvitationQuery>(r2 =>
                    r2.Ukprn == f.Invitation.Ukprn));
        }
    }

    public class GetInvitationQueryHandlerTestsFixture
    {
        public GetInvitationQuery Query { get; set; }
        public IRequestHandler<GetInvitationQuery, GetInvitationQueryResult> Handler { get; set; }
        public Invitation Invitation { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        
        public GetInvitationQueryHandlerTestsFixture()
        {
            Query = new GetInvitationQuery("PRN", null, "EmployerOrganisation", "Desc");
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfiles(typeof(InvitationMappings)));
            Handler = new GetInvitationQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetInvitationQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetInvitationQueryHandlerTestsFixture SetInvitations()
        {
            Invitation = EntityActivator.CreateInstance<Invitation>().Set(i => i.Ukprn, "PRN").Set(i => i.EmployerOrganisation, "Org");
            Db.Invitations.Add(Invitation);
            Db.SaveChanges();
            
            return this;
        }
    }
}