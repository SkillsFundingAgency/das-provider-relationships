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
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Audit.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedPermissionsEventAuditCommandHandlerTests : FluentTest<UpdatedPermissionsEventAuditCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreatedAccountEventAuditCommand_ThenShouldCreateAuditRecord()
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
                    a.TimeLogged > DateTime.UtcNow.AddHours(-1)));
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
            Command = new UpdatedPermissionsEventAuditCommand { AccountId = 112, AccountLegalEntityId = 114, AccountProviderId = 116, AccountProviderLegalEntityId = 118, Ukprn = 256894321, UserRef = Guid.NewGuid(), GrantedOperations = new List<Operation> {
                Operation.CreateCohort
            }, Updated = DateTime.Parse("2018-11-11")};
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
                result += operation.ToString();
                first = false;
            }
            return result;
        }
    }
}
