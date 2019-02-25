using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Commands.DeletedPermissionsEventNotify;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class DeletedPermissionsEventNotifyCommandHandlerTests : FluentTest<DeletedPermissionsEventNotifyCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingDeletedPermissionsEventNotifyCommand_ThenShouldCallClientToNotify()
        {
            return RunAsync(f => f.Handle(),
                f => f.Client.Verify(c => c.SendEmailToAllProviderRecipients(f.AccountProviderId,
                    It.Is<ProviderEmailRequest>(r =>
                        r.TemplateId == "DeletedPermissionsEventNotification" &&
                        r.Tokens["organisation_name"] == f.OrganisationName
                    ))));
        }
    }

    public class DeletedPermissionsEventNotifyCommandHandlerTestsFixture
    {
        public DeletedPermissionsEventNotifyCommand Command { get; set; }
        public IRequestHandler<DeletedPermissionsEventNotifyCommand, Unit> Handler { get; set; }
        public Mock<IPasAccountApiClient> Client { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public long AccountProviderId { get; set; }
        public long AccountId { get; set; }
        public string OrganisationName { get; set; }

        public DeletedPermissionsEventNotifyCommandHandlerTestsFixture()
        {
            AccountProviderId = 112;
            AccountId = 114;
            OrganisationName = "TestOrg";

            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Command = new DeletedPermissionsEventNotifyCommand(AccountProviderId, AccountId);
            Client = new Mock<IPasAccountApiClient>();

            Db.Accounts.Add(EntityActivator.CreateInstance<Account>().Set(a => a.Id, AccountId).Set(a => a.Name, OrganisationName));
            Db.SaveChanges();

            Handler = new DeletedPermissionsEventNotifyCommandHandler(Client.Object, new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
        }


    }
}