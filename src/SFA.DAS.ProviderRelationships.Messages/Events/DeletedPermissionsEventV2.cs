using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
#pragma warning disable 618
    public class DeletedPermissionsEventV2 : DeletedPermissionsEvent
#pragma warning restore 618
    {
        public long AccountLegalEntityId { get; }

        public DeletedPermissionsEventV2(long accountProviderLegalEntityId, long ukprn, long accountLegalEntityId, DateTime deleted)
            : base(accountProviderLegalEntityId, ukprn, deleted)
        {
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}