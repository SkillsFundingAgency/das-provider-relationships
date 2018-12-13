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
    public class CreatedAccountEventAuditCommandHandlerTests : FluentTest<CreatedAccountEventAuditCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreatedAccountEventAuditCommand_ThenShouldCreateAuditRecord()
        {
            return RunAsync(f => f.Handle(), f => f.Db.CreatedAccountEventAudits.SingleOrDefault(a => a.UserRef == f.Command.UserRef).Should().NotBeNull()
                .And.Match<CreatedAccountEventAudit>(a =>
                    a.UserName == f.Command.UserName &&
                    a.AccountId == f.Command.AccountId &&
                    a.Name == f.Command.Name &&
                    a.PublicHashedId == f.Command.PublicHashedId &&
                    a.UserRef == f.Command.UserRef &&
                    a.HashedId == f.Command.HashedId));
        }
    }

    public class CreatedAccountEventAuditCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public CreatedAccountEventAuditCommand Command { get; set; }
        public IRequestHandler<CreatedAccountEventAuditCommand, Unit> Handler { get; set; }

        public CreatedAccountEventAuditCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new CreatedAccountEventAuditCommand{ AccountId = 112, Name = "Bobo", UserRef = Guid.NewGuid(), UserName = "User One", PublicHashedId = "hashedid11111", HashedId = "38jfkd"};
            Handler = new CreatedAccountEventAuditCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}
