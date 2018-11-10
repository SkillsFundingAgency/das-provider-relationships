using System;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios
{
    public class DeleteLegalEntity
    {
        private readonly IMessageSession _messageSession;
        private readonly IMediator _mediator;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public DeleteLegalEntity(IMessageSession messageSession, IMediator mediator, Lazy<ProviderRelationshipsDbContext> db)
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
            //const long ukprn = 7;
            const string accountPublicHashedId = "ACCPUB";
            const string accountLegalEntityPublicHashedId = "ALEPUB";
            const string originalAccountName = "Account Name";
            const long legalEntityId = 8;
            const string originalLegalEntityName = "Legal Entity";
            //const string providerName = "provider name";
            
            await _messageSession.Publish(new CreatedAccountEvent { AccountId = accountId, PublicHashedId = accountPublicHashedId, Name = originalAccountName, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow });
            await _messageSession.Publish(new AddedLegalEntityEvent { AccountLegalEntityId = accountLegalEntityId, AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId, OrganisationName = originalLegalEntityName, AccountId = accountId, LegalEntityId = legalEntityId, AgreementId = 2, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow });

            // Add account provider

            await _db.Value.SaveChangesAsync();
            
            // Add permissions

            await _db.Value.SaveChangesAsync();
            
            await _messageSession.Publish(new RemovedLegalEntityEvent { AccountLegalEntityId = accountLegalEntityId, OrganisationName = originalLegalEntityName, AccountId = accountId, LegalEntityId = legalEntityId, AgreementId = 2, AgreementSigned = true, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow });
        }
    }
}