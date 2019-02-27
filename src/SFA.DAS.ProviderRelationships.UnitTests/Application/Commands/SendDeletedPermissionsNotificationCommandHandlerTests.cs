using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Application.Commands.SendDeletedPermissionsNotification;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class SendDeletedPermissionsNotificationCommandHandlerTests : FluentTest<SendDeletedPermissionsNotificationCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_ThenShouldCallClientToNotify()
        {
            return RunAsync(f => f.Handle(),
                f => f.Client.Verify(c => c.SendEmailToAllProviderRecipients(f.Ukprn,
                    It.Is<ProviderEmailRequest>(r =>
                        r.TemplateId == "DeletedPermissionsEventNotification" &&
                        r.Tokens["organisation_name"] == f.OrganisationName
                    ))));
        }
    }

    public class SendDeletedPermissionsNotificationCommandHandlerTestsFixture
    {
        public SendDeletedPermissionsNotificationCommand Command { get; set; }
        public IRequestHandler<SendDeletedPermissionsNotificationCommand, Unit> Handler { get; set; }
        public Mock<IPasAccountApiClient> Client { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string OrganisationName { get; set; }

        public SendDeletedPermissionsNotificationCommandHandlerTestsFixture()
        {
            Ukprn = 228876542;
            AccountLegalEntityId = 116;
            OrganisationName = "TestOrg";

            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Command = new SendDeletedPermissionsNotificationCommand(Ukprn, AccountLegalEntityId);
            Client = new Mock<IPasAccountApiClient>();

            Db.AccountLegalEntities.Add(EntityActivator.CreateInstance<AccountLegalEntity>().Set(a => a.Id, AccountLegalEntityId).Set(a => a.Name, OrganisationName));
            Db.SaveChanges();

            Handler = new SendDeletedPermissionsNotificationCommandHandler(Client.Object, new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
        }


    }
}