using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddInvitationCommandHandlerTests : FluentTest<AddInvitationCommandHandlerTestFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddInvitationCommand_ThenShouldAddInvitation()
        {
            return RunAsync(f => f.Handle(), f => f.Db.Invitations.SingleOrDefault().Should().NotBeNull()
                .And.Match<Invitation>(a => 
                    a.Ukprn == f.Command.Ukprn &&
                    a.UserRef == f.Command.UserRef &&
                    a.EmployerFirstName == f.Command.EmployerFirstName &&
                    a.EmployerLastName == f.Command.EmployerLastName &&
                    a.EmployerOrganisation == f.Command.EmployerOrganisation && 
                    a.EmployerEmail == f.Command.EmployerEmail));
        }

        [Test]
        public Task Handle_WhenHandlingAddInvitationCommand_ThenShouldReturnReference()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().Be(f.Db.Invitations.Select(ap => ap.Reference.ToString()).Single()));
        }
    }

    public class AddInvitationCommandHandlerTestFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public AddInvitationCommand Command { get; set; }
        public IRequestHandler<AddInvitationCommand, string> Handler { get; set; }
        
        public AddInvitationCommandHandlerTestFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new AddInvitationCommand(12345, Guid.NewGuid().ToString(), "Organisation", "John", "Smith", "john@gov.uk");
            Handler = new AddInvitationCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task<string> Handle()
        {
            return await Handler.Handle(Command, CancellationToken.None);
        }
    }
}