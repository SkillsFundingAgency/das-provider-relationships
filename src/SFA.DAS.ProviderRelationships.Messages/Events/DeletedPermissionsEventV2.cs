using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
#pragma warning disable 618
    public class DeletedPermissionsEventV2 : DeletedPermissionsEvent
#pragma warning restore 618
    {
        public long AccountId { get; }
        public long AccountProviderId { get; }

        public DeletedPermissionsEventV2(long accountProviderLegalEntityId, long ukprn, long accountId, long accountProviderId, DateTime deleted)
            : base(accountProviderLegalEntityId, ukprn, deleted)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
        }
    }
}