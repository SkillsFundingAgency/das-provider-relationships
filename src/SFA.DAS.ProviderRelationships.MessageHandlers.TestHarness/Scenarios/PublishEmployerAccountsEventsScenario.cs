using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios;

public class PublishEmployerAccountsEventsScenario
{
    private readonly IMessageSession _messageSession;

    public PublishEmployerAccountsEventsScenario(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }

    public async Task Run()
    {
        var userRef = Guid.NewGuid();
        const string userName = "Bob Loblaw";
        const long accountId = 10;
        const string accountPublicHashedId = "ACCPUB";
        const string originalAccountName = "Account Name";
        const string updatedAccountName = "New Account Name";
        const long legalEntityId = 20;
        const long accountLegalEntityId = 1020;
        const string accountLegalEntityPublicHashedId = "ALEPUB";
        const string originalAccountLegalEntityName = "Legal Entity";
        const string updatedAccountLegalEntityName = "New Legal Entity";
        const string accountHashedId = "AHEAHE";

        await _messageSession.Publish(new CreatedAccountEvent {
            AccountId = accountId,
            PublicHashedId = accountPublicHashedId,
            Name = originalAccountName,
            UserName = userName,
            UserRef = userRef,
            Created = DateTime.UtcNow,
            HashedId = accountHashedId
        });

        await _messageSession.Publish(new ChangedAccountNameEvent {
            AccountId = accountId,
            PreviousName = originalAccountName,
            CurrentName = updatedAccountName,
            UserName = userName,
            UserRef = userRef,
            Created = DateTime.UtcNow,
            HashedAccountId = accountPublicHashedId
        });

        await _messageSession.Publish(new AddedLegalEntityEvent {
            AccountLegalEntityId = accountLegalEntityId,
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId,
            OrganisationName = originalAccountLegalEntityName,
            AccountId = accountId,
            LegalEntityId = legalEntityId,
            AgreementId = 2,
            UserName = userName,
            UserRef = userRef,
            Created = DateTime.UtcNow
        });

        await _messageSession.Publish(new UpdatedLegalEntityEvent {
            AccountLegalEntityId = accountLegalEntityId,
            Name = updatedAccountLegalEntityName,
            Address = "New LE Address",
            UserName = userName,
            UserRef = userRef,
            Created = DateTime.UtcNow
        });

        await _messageSession.Publish(new RemovedLegalEntityEvent {
            AccountLegalEntityId = accountLegalEntityId,
            OrganisationName = updatedAccountLegalEntityName,
            AccountId = accountId,
            LegalEntityId = legalEntityId,
            AgreementId = 2,
            AgreementSigned = true,
            UserName = userName,
            UserRef = userRef,
            Created = DateTime.UtcNow
        });
    }
}