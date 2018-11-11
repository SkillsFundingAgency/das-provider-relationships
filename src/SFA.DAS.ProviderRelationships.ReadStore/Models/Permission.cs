using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Permission : Document
    {

        [JsonProperty("ukprn")]
        public virtual long Ukprn { get; protected set; }

        [JsonProperty("accountProviderLegalEntityId")]
        public virtual long AccountProviderLegalEntityId { get; protected set; }

        [JsonProperty("accountId")]
        public virtual long AccountId { get; protected set; }

        [JsonProperty("accountPublicHashedId")]
        public virtual string AccountPublicHashedId { get; protected set; }

        [JsonProperty("accountName")]
        public virtual string AccountName { get; protected set; }


        [JsonProperty("accountLegalEntityId")]
        public virtual long AccountLegalEntityId { get; protected set; }

        [JsonProperty("accountLegalEntityPublicHashedId")]
        public virtual string AccountLegalEntityPublicHashedId { get; protected set; }

        [JsonProperty("accountLegalEntityName")]
        public virtual string AccountLegalEntityName { get; protected set; }


        [JsonProperty("accountProviderId")]
        public virtual int AccountProviderId { get; protected set; }

        [JsonProperty("operations")]
        public virtual IEnumerable<Operation> Operations { get; protected set; } = new HashSet<Operation>();

        [JsonProperty("outboxData")]
        public virtual IEnumerable<OutboxDataItem> OutboxData { get; set; } = new List<OutboxDataItem>();

        [JsonProperty("created")]
        public virtual DateTime Created { get; protected set; }

        [JsonProperty("deleted")]
        public virtual DateTime? Deleted { get; protected set; }

        [JsonProperty("updated")]
        public virtual DateTime? Updated { get; protected set; }

        public static Permission Create(long ukprn, long accountProviderLegalEntityId,
                long accountId, string accountPublicHashedId, string accountName,
                long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName,
                int accountProviderId, DateTime created, string messageId) 
        {
            var permission = new Permission(ukprn, accountProviderLegalEntityId,
                accountId, accountPublicHashedId, accountName,
                accountLegalEntityId, accountLegalEntityPublicHashedId, accountLegalEntityName,
                accountProviderId) {Created = created};

            permission.AddMessageToOutbox(messageId, created);
            return permission;
        }

        protected Permission(long ukprn, long accountProviderLegalEntityId,
            long accountId, string accountPublicHashedId, string accountName, 
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, 
            int accountProviderId)
            : this()
        {
            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;

            AccountId = accountId;
            AccountPublicHashedId = accountPublicHashedId;
            AccountName = accountName;

            AccountLegalEntityId = accountLegalEntityId;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            AccountLegalEntityName = accountLegalEntityName;

            AccountProviderId = accountProviderId;
        }

        protected Permission() : base(1, "permission")
        {
        }

        public void ReActivateRelationship(long ukprn, long accountProviderLegalEntityId, 
            long accountId, string accountPublicHashedId, string accountName, 
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, 
            int accountProviderId, DateTime reactivated, string messageId)
        {
            if (MessageAlreadyProcessed(messageId))
                return;

            if (Deleted == null)
                throw new InvalidOperationException($"Message {messageId} is trying to recreate a relationship which hasn't been deleted");

            if (Deleted > reactivated)
                throw new InvalidOperationException($"Message {messageId} is trying to recreate a relationship which was deleted after the re-activate request");

            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;

            AccountId = accountId;
            AccountPublicHashedId = accountPublicHashedId;
            AccountName = accountName;

            AccountLegalEntityId = accountLegalEntityId;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            AccountLegalEntityName = accountLegalEntityName;

            AccountProviderId = accountProviderId;
            Created = reactivated;
            Deleted = null;

            AddMessageToOutbox(messageId, reactivated);
        }

        public void UpdatePermissions(HashSet<Operation> grants, DateTime updated, string messageId)
        {
            if (MessageAlreadyProcessed(messageId))
                return;

            if (Deleted != null)
                throw new InvalidOperationException($"Message {messageId} is trying to update a Permission has been deleted");

            if (Created > updated)
                throw new InvalidOperationException($"Message {messageId} is trying to update a Permission was been created/re-activated after this update message");

            // Swallow old updates 
            if (Updated > updated)
            {
                AddMessageToOutbox(messageId, updated);
                return;
            }

            Operations = grants;
            Updated = updated;
            AddMessageToOutbox(messageId, updated);
        }

        public void DeleteRelationship(DateTime deleted, string messageId)
        {
            if (MessageAlreadyProcessed(messageId))
                return;

            if (Deleted != null)
                throw new InvalidOperationException($"Message {messageId} is trying to delete a Permission has already been deleted");

            if (Created > deleted)
                throw new InvalidOperationException($"Message {messageId} is trying to delete a Permission has been created/re-activated after this delete request");

            if (Updated > deleted)
                throw new InvalidOperationException($"Message {messageId} is trying to delete a Permission has been updated after this delete request");

            Operations = new HashSet<Operation>();
            Deleted = deleted;

            AddMessageToOutbox(messageId, deleted);
        }

        private bool MessageAlreadyProcessed(string messageId)
        {
            return OutboxData.Any(x => x.MessageId == messageId);
        }

        private void AddMessageToOutbox(string messageId, DateTime created)
        {
            if (messageId is null) throw new ArgumentNullException(nameof(messageId));
            OutboxData = OutboxData.Append(new OutboxDataItem(messageId, created));
        }
    }
}