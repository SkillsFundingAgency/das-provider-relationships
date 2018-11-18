using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using Z.EntityFramework.Plus;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class DeleteAccountLegalEntityPermissionsCommandHandlerTests : FluentTest<DeleteAccountLegalEntityPermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingADeleteAccountLegalEntityPermissionsCommand_ThenShouldDeleteAccountLegalEntityPermissions()
        {
            return RunAsync(f => f.Handle(), f =>
            {
                f.Db.Permissions.Any().Should().BeTrue();
                f.Db.Permissions.Any(p => p.AccountLegalEntityId == f.AccountLegalEntityId).Should().BeFalse();
            });
        }
    }

    public class DeleteAccountLegalEntityPermissionsCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public long AccountLegalEntityId { get; set; }
        public DeleteAccountLegalEntityPermissionsCommand DeleteAccountLegalEntityPermissionsCommand { get; set; }
        public IRequestHandler<DeleteAccountLegalEntityPermissionsCommand, Unit> Handler { get; set; }

        public DeleteAccountLegalEntityPermissionsCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            
            BatchDeleteManager.InMemoryDbContextFactory = () => Db;
            
            AccountLegalEntityId = 1;
            DeleteAccountLegalEntityPermissionsCommand = new DeleteAccountLegalEntityPermissionsCommand(AccountLegalEntityId);

            Db.Permissions.Add(new PermissionBuilder().WithAccountLegalEntityId(AccountLegalEntityId));
            Db.Permissions.Add(new PermissionBuilder().WithAccountLegalEntityId(AccountLegalEntityId));
            Db.Permissions.Add(new PermissionBuilder().WithAccountLegalEntityId(2));
            Db.SaveChanges();

            Handler = new DeleteAccountLegalEntityPermissionsCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(DeleteAccountLegalEntityPermissionsCommand, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}