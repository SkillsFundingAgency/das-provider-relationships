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
                act: async f =>
                {
                    await f.Handle();
                },
                assert: f =>
                {
                    f.Client.Verify(c => c.SendEmailToAllProviderRecipients(f.Provider.Ukprn, It.Is<ProviderEmailRequest>(r => r.TemplateId == "UpdatedPermissionsEventNotification")));
                });

        [Test]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_ThenShouldCallClientToNotifyWithOrganisationAndProviderName() =>
            RunAsync(
                act: async f =>
                {
                    await f.Handle();
                },
                assert: f =>
                {
                    f.Client.Verify(c => c.SendEmailToAllProviderRecipients(f.Provider.Ukprn, It.Is<ProviderEmailRequest>(r =>
                    r.TemplateId == "UpdatedPermissionsEventNotification" &&
                    r.Tokens["organisation_name"] == f.AccountLegalEntity.Name &&
                    r.Tokens["training_provider_name"] == f.Provider.Name
                    )));
                });

        [Test]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_BothPermissionsAdded_ThenShouldCallClientToNotifyPermissionSet() =>
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
                   Assert.IsNotNull(f.ResultEmailRequest);
                   Assert.AreEqual(f.AccountLegalEntity.Name, f.ResultEmailRequest.Tokens["organisation_name"]);
                   Assert.AreEqual(f.Provider.Name, f.ResultEmailRequest.Tokens["training_provider_name"]);
                   Assert.AreEqual("changed your apprenticeship service permissions.", f.ResultEmailRequest.Tokens["part1_text"]);
                   Assert.AreEqual("You can now add apprentice records and recruit apprentices on their behalf.", f.ResultEmailRequest.Tokens["part2_text"]);
               });

        [Test]
        [TestCase(Operation.CreateCohort, "You can now add apprentice records on their behalf.")]
        [TestCase(Operation.Recruitment, "You can now recruit apprentices on their behalf.")]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_SinglePermissionAdded_ThenShouldCallClientToNotifyWithPermissionSet(Operation grantedOperation, string expectedSetPermissionString) =>
           RunAsync(
               arrange: f =>
               {
                   f.Command.GrantedOperations = new HashSet<Operation> { grantedOperation };
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
                   Assert.AreEqual("changed your apprenticeship service permissions.", f.ResultEmailRequest.Tokens["part1_text"]);
                   Assert.AreEqual(expectedSetPermissionString, f.ResultEmailRequest.Tokens["part2_text"]);
               });

        [Test]
        [TestCase(Operation.CreateCohort, "removed your permission to recruit apprentices.", "You can still add apprentice records on their behalf.")]
        [TestCase(Operation.Recruitment, "removed your permission to add apprentice records.", "You can still recruit apprentices on their behalf.")]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_BothPermissionSet_SinglePermissionRemoved_ThenShouldCallClientToNotifyWithPermissionRemoved(
            Operation remainingGrantedOperation, 
            string expectedSetPermissionPart1, 
            string expectedSetPermissionPart2) =>
           RunAsync(
               arrange: f =>
               {
                   f.Command.PreviousOperations = new HashSet<Operation> { Operation.CreateCohort, Operation.Recruitment };
                   f.Command.GrantedOperations = new HashSet<Operation> { remainingGrantedOperation };
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
                   Assert.AreEqual(expectedSetPermissionPart1, f.ResultEmailRequest.Tokens["part1_text"]);
                   Assert.AreEqual(expectedSetPermissionPart2, f.ResultEmailRequest.Tokens["part2_text"]);
               });

        [Test]
        [TestCase(new Operation[] { Operation.CreateCohort, Operation.Recruitment }, "removed your permission to add apprentice records and recruit apprentices.", "You cannot do anything in the apprenticeship service on their behalf at the moment.")]
        [TestCase(new Operation[] { Operation.Recruitment }, "removed your permission to recruit apprentices.", "You cannot do anything in the apprenticeship service on their behalf at the moment.")]
        [TestCase(new Operation[] { Operation.CreateCohort }, "removed your permission to add apprentice records.", "You cannot do anything in the apprenticeship service on their behalf at the moment.")]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_AllPermissionsRemoved_ThenShouldCallClientToNotifyWithPermissionRemoved(
            Operation[] previousSetOperations,
            string expectedSetPermissionPart1,
            string expectedSetPermissionPart2) =>
           RunAsync(
               arrange: f =>
               {
                   f.Command.PreviousOperations = new HashSet<Operation>(previousSetOperations);
                   f.Command.GrantedOperations = new HashSet<Operation> {  };
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
                   Assert.AreEqual(expectedSetPermissionPart1, f.ResultEmailRequest.Tokens["part1_text"]);
                   Assert.AreEqual(expectedSetPermissionPart2, f.ResultEmailRequest.Tokens["part2_text"]);
               });
 
        [Test]
        [TestCase(Operation.Recruitment, Operation.CreateCohort, "\u2022 given you permission to add apprentice records\r\n\u2022 removed your permission to recruit apprentices")]
        [TestCase(Operation.CreateCohort, Operation.Recruitment, "\u2022 given you permission to recruit apprentices\r\n\u2022 removed your permission to add apprentice records")]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_AlternatingPermissions_ThenShouldCallClientToNotifyWithPermissionRemovedAndPermissionAdded(
            Operation previousSetOperations,
            Operation grantedOperation,
            string expectedSetPermissionPart2) =>
           RunAsync(
               arrange: f =>
               {
                   f.Command.PreviousOperations = new HashSet<Operation> { previousSetOperations };
                   f.Command.GrantedOperations = new HashSet<Operation> { grantedOperation };
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
                   Assert.AreEqual(":", f.ResultEmailRequest.Tokens["part1_text"]);
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


        public SendUpdatedPermissionsNotificationCommandHandlerTestsFixture()
        {
            UnitOfWorkContext = new UnitOfWorkContext();

            CreateDb();
            CreateDefaultEntities();

            Client = new Mock<IPasAccountApiClient>();
            Handler = new SendUpdatedPermissionsNotificationCommandHandler(Client.Object, new Lazy<ProviderRelationshipsDbContext>(() => Db));

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
