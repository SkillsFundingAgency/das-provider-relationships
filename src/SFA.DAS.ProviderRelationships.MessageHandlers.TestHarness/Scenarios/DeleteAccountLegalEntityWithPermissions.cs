using System;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios
{
    public class DeleteAccountLegalEntityWithPermissions
    {
        private readonly IMessageSession _messageSession;
        private readonly IMediator _mediator;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public DeleteAccountLegalEntityWithPermissions(IMessageSession messageSession, IMediator mediator, Lazy<ProviderRelationshipsDbContext> db)
        {
            _messageSession = messageSession;
            _mediator = mediator;
            _db = db;
        }

        public async Task Run()
        {
            var userRef = Guid.NewGuid();
            const string userName = "Bob Loblaw";
            const long accountId = 1;
            const long accountLegalEntityId = 5;
            const long ukprn = 7;
            const string accountPublicHashedId = "ACCPUB";
            const string accountLegalEntityPublicHashedId = "ALEPUB";
            const string originalAccountName = "Account Name";
            const long legalEntityId = 8;
            const string originalLegalEntityName = "Legal Entity";
            const string providerName = "provider name";
            
            await _messageSession.Publish(new CreatedAccountEvent { AccountId = accountId, PublicHashedId = accountPublicHashedId, Name = originalAccountName, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow });

            await _messageSession.Publish(new AddedLegalEntityEvent { AccountLegalEntityId = accountLegalEntityId, AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId, OrganisationName = originalLegalEntityName, AccountId = accountId, LegalEntityId = legalEntityId, AgreementId = 2, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow });

            await _mediator.Send(new AddProviderRelationshipCommand
            {
                AccountLegalEntityId = accountLegalEntityId,
                Ukprn = ukprn,
                ProviderName = providerName
            });

            await _db.Value.SaveChangesAsync();
            
            await _mediator.Send(new SetPermissionsCommand
            {
                AccountLegalEntityId = accountLegalEntityId, Ukprn = ukprn, UserName = userName, UserRef = userRef,
                Permissions = new[]
                {
                    new Application.Commands.Permission { Type = PermissionType.CreateCohort, Granted = true}
                    //new Application.Commands.Permission { Type = PermissionType.Test, Granted = true}
                }
            });

            await _db.Value.SaveChangesAsync();
            
            await _messageSession.Publish(new RemovedLegalEntityEvent { AccountLegalEntityId = accountLegalEntityId, OrganisationName = originalLegalEntityName, AccountId = accountId, LegalEntityId = legalEntityId, AgreementId = 2, AgreementSigned = true, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow });
        }
    }
}