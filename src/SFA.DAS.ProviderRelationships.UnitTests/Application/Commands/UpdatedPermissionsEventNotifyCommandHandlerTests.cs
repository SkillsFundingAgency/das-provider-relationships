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
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommandd_ThenShouldCallClientToNotify() =>
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
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommandd_ThenShouldCallClientToNotifyWithOrganisationAndProviderName() =>
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
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommandd_ThenShouldCallClientToNotifyWithOrganisationAndPrviderAndPermissionSet() =>
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
                   r.Tokens["training_provider_name"] == f.Provider.Name &&
                   r.Tokens["permissions_set"] == f.GetOperationName(f.AccountProviderLegalEntity.Permissions.Select(x => x.Operation))                   
                   )));
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


        public SendUpdatedPermissionsNotificationCommandHandlerTestsFixture()
        {
            UnitOfWorkContext = new UnitOfWorkContext();
            
            CreateDb();            
            CreateDefaultEntities();                       
            
            Client = new Mock<IPasAccountApiClient>();
            Handler = new SendUpdatedPermissionsNotificationCommandHandler(Client.Object, new Lazy<ProviderRelationshipsDbContext>(() => Db));

            Command = new SendUpdatedPermissionsNotificationCommand(ukprn: 299792458, accountLegalEntityId: 12345);
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

        public string GetOperationName(IEnumerable<Operation> permissions)
        {
            return string.Join(" and ", permissions?
             .Where(ope => !string.IsNullOrEmpty(ope.ToString()))
             .Select(ope => $"{ope.GetType().GetMember(ope.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name}"));
        }

    }
}