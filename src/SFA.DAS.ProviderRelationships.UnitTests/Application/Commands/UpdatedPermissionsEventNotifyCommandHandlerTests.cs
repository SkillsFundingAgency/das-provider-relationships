using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Helpers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;
using User = SFA.DAS.ProviderRelationships.Models.User;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class SendUpdatedPermissionsNotificationCommandHandlerTests : FluentTest<SendUpdatedPermissionsNotificationCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_ThenShouldCallClientToNotify() =>
            RunAsync(
                arrange: f =>
                {
                    f.Command.GrantedOperations = new HashSet<Operation> { Operation.CreateCohort, Operation.Recruitment };
                },
                act: async f =>
                {
                    await f.Handle();
                },
                assert: f =>
                {
                    f.Client.Verify(c => c.SendEmailToAllProviderRecipients(f.Provider.Ukprn, It.Is<ProviderEmailRequest>(r => r.TemplateId == "UpdatedProviderPermissionsNotification")));
                });

        [Test]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_ThenShouldCallClientToNotifyWithOrganisationAndProviderName() =>
            RunAsync(
                arrange: f =>
                {
                    f.Command.GrantedOperations = new HashSet<Operation> { Operation.CreateCohort, Operation.Recruitment };
                },
                act: async f =>
                {
                    await f.Handle();
                },
                assert: f =>
                {
                    f.Client.Verify(c => c.SendEmailToAllProviderRecipients(f.Provider.Ukprn, It.Is<ProviderEmailRequest>(r =>
                    r.TemplateId == "UpdatedProviderPermissionsNotification" &&
                    r.Tokens["organisation_name"] == f.AccountLegalEntity.Name &&
                    r.Tokens["training_provider_name"] == f.Provider.Name &&
                    r.Tokens["manage_recruitment_emails_url"] == f.ManageNotificationsUrl
                    )));
                });

        [Test]
        [TestCase(new Operation[] { Operation.CreateCohort, Operation.Recruitment }, "• add apprentice records\r\n• create and publish job adverts\r\n")]
        [TestCase(new Operation[] { Operation.CreateCohort, Operation.Recruitment, Operation.RecruitmentRequiresReview }, "• add apprentice records\r\n• create job adverts\r\n")]
        [TestCase(new Operation[] { Operation.Recruitment, Operation.RecruitmentRequiresReview }, "• cannot add apprentice records\r\n• create job adverts\r\n")]
        [TestCase(new Operation[] { Operation.Recruitment }, "• cannot add apprentice records\r\n• create and publish job adverts\r\n")]
        [TestCase(new Operation[] { Operation.CreateCohort }, "• add apprentice records\r\n• cannot create job adverts\r\n")]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_ThenShouldCallClientToNotifyWithPermissions(
            Operation[] operations,
            string expectedSetPermissionPart2) =>
           RunAsync(
               arrange: f =>
               {
                   f.Command.PreviousOperations = new HashSet<Operation> { };
                   f.Command.GrantedOperations = new HashSet<Operation>(operations);
               },
               act: async f =>
               {
                   await f.Handle();
               },
               assert: f =>
               {
                   Assert.IsNotNull(f.ResultEmailRequest);
                   Assert.AreEqual(f.AccountLegalEntity.Name, f.ResultEmailRequest.Tokens["organisation_name"]);
                   Assert.AreEqual(f.Provider.Name, f.ResultEmailRequest.Tokens["training_provider_name"]);
                   Assert.AreEqual("set your apprenticeship service permissions to:", f.ResultEmailRequest.Tokens["part1_text"]);
                   Assert.AreEqual(expectedSetPermissionPart2, f.ResultEmailRequest.Tokens["part2_text"]);
               });
    }

    public class SendUpdatedPermissionsNotificationCommandHandlerTestsFixture
    {
        public SendUpdatedPermissionsNotificationCommand Command { get; set; }
        public IRequestHandler<SendUpdatedPermissionsNotificationCommand, Unit> Handler { get; set; }
        public Mock<IPasAccountApiClient> Client { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public long Ukprn { get; set; }

        public Account Account;
        public Provider Provider;
        public User User;
        public AccountLegalEntity AccountLegalEntity;
        public AccountProvider AccountProvider;
        public AccountProviderLegalEntity AccountProviderLegalEntity;
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public ProviderEmailRequest ResultEmailRequest { get; set; }
        public string ManageNotificationsUrl = "http://recruit/notifications";

        public SendUpdatedPermissionsNotificationCommandHandlerTestsFixture()
        {
            var providerUrls = new Mock<IProviderUrls>();
            providerUrls.Setup(x => x.ProviderManageRecruitEmails(It.Is<string>(s => s == Provider.Ukprn.ToString()))).Returns(ManageNotificationsUrl);
            UnitOfWorkContext = new UnitOfWorkContext();

            CreateDb();
            CreateDefaultEntities();

            Client = new Mock<IPasAccountApiClient>();
            Handler = new SendUpdatedPermissionsNotificationCommandHandler(Client.Object, providerUrls.Object, new Lazy<ProviderRelationshipsDbContext>(() => Db));

            Command = new SendUpdatedPermissionsNotificationCommand(
                ukprn: 299792458,
                accountLegalEntityId: 12345,
                new HashSet<Operation>(),
                new HashSet<Operation>());

            Client
                .Setup(c => c.SendEmailToAllProviderRecipients(Provider.Ukprn, It.IsAny<ProviderEmailRequest>()))
                .Callback((long ukprn, ProviderEmailRequest provEmail) =>
                {
                    ResultEmailRequest = provEmail;
                })
                .Returns(Task.FromResult(0));
        }

        private void CreateDefaultEntities()
        {
            Account = new Account(
                id: 1,
                hashedId: "HashedId",
                publicHashedId: "PublicHashedId",
                name: "Account",
                created: DateTime.UtcNow);
            Db.Add(Account);

            AccountLegalEntity = new AccountLegalEntity(
                account: Account,
                id: 12345,
                publicHashedId: "ALE1",
                name: "Account legal entity 1",
                created: DateTime.UtcNow);
            Db.Add(AccountLegalEntity);

            Provider = EntityActivator.CreateInstance<Provider>().Set(x => x.Ukprn, 299792458).Set(x => x.Name, "Provider Name");
            Db.Add(Provider);

            User = new User(Guid.NewGuid(), "me@home.com", "Bill", "Gates");
            Db.Add(User);

            AccountProvider = new AccountProvider(Account, Provider, User, null);
            AccountProvider.Set(x => x.Id, 23);
            Db.Add(AccountProvider);

            AccountProviderLegalEntity = new AccountProviderLegalEntity(
                accountProvider: AccountProvider,
                accountLegalEntity: AccountLegalEntity,
                user: User,
                grantedOperations: new HashSet<Operation>(new[] { Operation.CreateCohort, Operation.Recruitment }));
            AccountProviderLegalEntity.Set(x => x.Id, 34);
            Db.Add(AccountProviderLegalEntity);

            Db.SaveChanges();
        }

        private void CreateDb()
        {
            var optionsBuilder =
              new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .ConfigureWarnings(warnings =>
                      warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)
                  );
            Db = new ProviderRelationshipsDbContext(optionsBuilder.Options);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
        }
    }
}
