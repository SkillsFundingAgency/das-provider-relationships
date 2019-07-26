using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class UpdatedPermissionsEvent
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long AccountProviderId { get; }
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public Guid UserRef { get; }
        public string UserEmailAddress { get; }
        public string UserFirstName { get; }
        public string UserLastName { get; }
        public HashSet<Operation> GrantedOperations { get; }
        public DateTime Updated { get;}

        public UpdatedPermissionsEvent(long accountId, long accountLegalEntityId, long accountProviderId,
                                    long accountProviderLegalEntityId, long ukprn, Guid userRef,
                                    string userEmailAddress, string firstName, string lastName,
                                    HashSet<Operation> grantedOperations, DateTime updated)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            UserRef = userRef;
            UserEmailAddress = userEmailAddress;
            UserFirstName = firstName;
            UserLastName = lastName;
            GrantedOperations = grantedOperations;
            Updated = updated;
        }
    }
}