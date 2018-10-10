using System;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = IoC.Initialize())
            {
                var nServiceBusConfig = new NServiceBusConfig(container);
                try
                {
                    nServiceBusConfig.Start();

                    var userRef = Guid.NewGuid();
                    const string userName = "Bob Loblaw";
                    const long accountId = 1;
                    const long accountLegalEntityId = 5;
                    const string accountPublicHashedId = "ACCPUB";
                    const string accountLegalEntityPublicHashedId = "ALEPUB";
                    const string originalAccountName = "Account Name";
                    const string updatedAccountName = "New Account Name";
                    const long legalEntityId = 8;
                    const string originalLegalEntityName = "Legal Entity";
                    const string updatedLegalEntityName = "New Legal Entity";
                    
                    //todo: need mechanism to set permissions. new permission created/updates event. who owns? probably employercommitments somewhere
                    
                    nServiceBusConfig.Endpoint.Publish(new CreatedAccountEvent 
                        {AccountId = accountId, PublicHashedId = accountPublicHashedId, Name = originalAccountName, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow}).GetAwaiter().GetResult();
                    
                    nServiceBusConfig.Endpoint.Publish(new ChangedAccountNameEvent 
                        {AccountId = accountId, PreviousName = originalAccountName, CurrentName = updatedAccountName, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow}).GetAwaiter().GetResult();
                    
                    // note: no address in created, but in updated
                    nServiceBusConfig.Endpoint.Publish(new AddedLegalEntityEvent
                        {AccountLegalEntityId = accountLegalEntityId, AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId, OrganisationName = originalLegalEntityName, AccountId = accountId, LegalEntityId = legalEntityId, AgreementId = 2, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow}).GetAwaiter().GetResult();

                    nServiceBusConfig.Endpoint.Publish(new UpdatedLegalEntityEvent
                        { AccountLegalEntityId = accountLegalEntityId, Name = updatedLegalEntityName, Address = "New LE Address", UserName = userName, UserRef = userRef, Created = DateTime.UtcNow});

                    nServiceBusConfig.Endpoint.Publish(new RemovedLegalEntityEvent
                        { AccountLegalEntityId = accountLegalEntityId, OrganisationName = updatedLegalEntityName, AccountId = accountId, LegalEntityId = legalEntityId, AgreementId = 2, AgreementSigned = true, UserName = userName, UserRef = userRef, Created = DateTime.UtcNow});
                }
                finally
                {
                    nServiceBusConfig.Stop();
                }
            }
        }
    }
}
