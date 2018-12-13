using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class AddedAccountProviderEventAuditCommandHandlerTests : FluentTest<AddedAccountProviderEventAuditCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedAccountProviderEventAuditCommand_ThenShouldCreateAuditRecord()
        {
            return RunAsync(f => f.Handle(), f => f.Db.AddedAccountProviderEventAudits.SingleOrDefault(a => a.AccountProviderId == f.Command.AccountProviderId).Should().NotBeNull()
                .And.Match<AddedAccountProviderEventAudit>(a =>
                    a.AccountProviderId == f.Command.AccountProviderId &&
                    a.AccountId == f.Command.AccountId &&
                    a.ProviderUkprn == f.Command.ProviderUkprn &&
                    a.UserRef == f.Command.UserRef &&
                    a.Added == f.Command.Added &&
                    a.TimeLogged > DateTime.UtcNow.AddHours(-1)));
        }
    }

    public class AddedAccountProviderEventAuditCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public AddedAccountProviderEventAuditCommand Command { get; set; }
        public IRequestHandler<AddedAccountProviderEventAuditCommand, Unit> Handler { get; set; }

        public AddedAccountProviderEventAuditCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new AddedAccountProviderEventAuditCommand(112, 114, 118277339, Guid.NewGuid(), DateTime.Parse("2018-11-11"));
            Handler = new AddedAccountProviderEventAuditCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}
