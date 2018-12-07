using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Audit.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class DeletedPermissionsEventAuditCommandHandlerTests : FluentTest<DeletedPermissionsEventAuditCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEventAuditCommand_ThenShouldCreateAuditRecord()
        {
            return RunAsync(f => f.Handle(), f => f.Db.DeletedPermissionsEventAudits.SingleOrDefault(a => a.AccountProviderLegalEntityId == f.Command.AccountProviderLegalEntityId).Should().NotBeNull()
                .And.Match<DeletedPermissionsEventAudit>(a =>
                    a.AccountProviderLegalEntityId == f.Command.AccountProviderLegalEntityId &&
                    a.Ukprn == f.Command.Ukprn &&
                    a.Deleted == f.Command.Deleted &&
                    a.TimeLogged > DateTime.UtcNow.AddHours(-1)));
        }
    }

    public class DeletedPermissionsEventAuditCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public DeletedPermissionsEventAuditCommand Command { get; set; }
        public IRequestHandler<DeletedPermissionsEventAuditCommand, Unit> Handler { get; set; }

        public DeletedPermissionsEventAuditCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new DeletedPermissionsEventAuditCommand {
                AccountProviderLegalEntityId = 118,
                Ukprn = 256894321,
                Deleted = DateTime.Parse("2018-11-11")
            };
            Handler = new DeletedPermissionsEventAuditCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}