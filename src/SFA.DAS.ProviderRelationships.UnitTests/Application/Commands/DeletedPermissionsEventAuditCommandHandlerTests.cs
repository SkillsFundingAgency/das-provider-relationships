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
    public class DeletedPermissionsEventAuditCommandHandlerTests : FluentTest<DeletedPermissionsEventAuditCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEventAuditCommand_ThenShouldCreateAuditRecord()
        {
            return TestAsync(f => f.Handle(), f => f.Db.DeletedPermissionsEventAudits.SingleOrDefault(a => a.AccountProviderLegalEntityId == f.Command.AccountProviderLegalEntityId).Should().NotBeNull()
                .And.Match<DeletedPermissionsEventAudit>(a =>
                    a.AccountProviderLegalEntityId == f.Command.AccountProviderLegalEntityId &&
                    a.Ukprn == f.Command.Ukprn &&
                    a.Deleted == f.Command.Deleted &&
                    a.Logged > DateTime.UtcNow.AddHours(-1)));
        }
    }

    public class DeletedPermissionsEventAuditCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public DeletedPermissionsEventAuditCommand Command { get; set; }
        public IRequestHandler<DeletedPermissionsEventAuditCommand, Unit> Handler { get; set; }

        public DeletedPermissionsEventAuditCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Command = new DeletedPermissionsEventAuditCommand(118, 256894321, DateTime.Parse("2018-11-11"));
            Handler = new DeletedPermissionsEventAuditCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}