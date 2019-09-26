using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedPermissionsEventAuditCommandHandlerTests : FluentTest<UpdatedPermissionsEventAuditCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingUpdatedPermissionsEventAuditCommand_ThenShouldCreateAuditRecord()
        {
            return RunAsync(f => f.Handle(), f => f.Db.UpdatedPermissionsEventAudits.SingleOrDefault(a => a.UserRef == f.Command.UserRef).Should().NotBeNull()
                .And.Match<UpdatedPermissionsEventAudit>(a =>
                    a.AccountId == f.Command.AccountId &&
                    a.AccountLegalEntityId == f.Command.AccountLegalEntityId &&
                    a.AccountProviderId == f.Command.AccountProviderId &&
                    a.AccountProviderLegalEntityId == f.Command.AccountProviderLegalEntityId &&
                    a.Ukprn == f.Command.Ukprn &&
                    a.UserRef == f.Command.UserRef &&
                    a.GrantedOperations == f.ConstructOperationsAudit(f.Command.GrantedOperations) &&
                    a.Updated == f.Command.Updated &&
                    a.Logged > DateTime.UtcNow.AddHours(-1)));
        }
    }

    public class UpdatedPermissionsEventAuditCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public UpdatedPermissionsEventAuditCommand Command { get; set; }
        public IRequestHandler<UpdatedPermissionsEventAuditCommand, Unit> Handler { get; set; }

        public UpdatedPermissionsEventAuditCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            Command = new UpdatedPermissionsEventAuditCommand(112, 114, 116, 118, 256894321, Guid.NewGuid(),
                new HashSet<Operation> {
                    Operation.CreateCohort
                }, DateTime.Parse("2018-11-11"));
            Handler = new UpdatedPermissionsEventAuditCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public string ConstructOperationsAudit(IEnumerable<Operation> operations)
        {
            var first = true;
            var result = string.Empty;
            foreach (var operation in operations)
            {
                if(!first) { result += ","; }
                result += ((short)operation).ToString();
                first = false;
            }
            return result;
        }
    }
}
