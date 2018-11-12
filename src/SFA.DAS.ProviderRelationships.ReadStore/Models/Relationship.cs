using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Relationship : Document
    {
        [JsonProperty("ukprn")]
        public long Ukprn { get; protected set; }

        [JsonProperty("accountProviderLegalEntityId")]
        public long AccountProviderLegalEntityId { get; protected set; }

        [JsonProperty("accountId")]
        public long AccountId { get; protected set; }

        [JsonProperty("accountPublicHashedId")]
        public string AccountPublicHashedId { get; protected set; }

        [JsonProperty("accountName")]
        public string AccountName { get; protected set; }


        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; protected set; }

        [JsonProperty("accountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; protected set; }

        [JsonProperty("accountLegalEntityName")]
        public string AccountLegalEntityName { get; protected set; }


        [JsonProperty("accountProviderId")]
        public int AccountProviderId { get; protected set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; protected set; }

        [JsonProperty("outboxData")]
        public IEnumerable<OutboxMessage> OutboxData {
            get => _outboxData.AsEnumerable();
            protected set => _outboxData = value.ToList();
        }

        [JsonProperty("created")]
        public DateTime Created { get; protected set; }

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; protected set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; protected set; }

        [JsonIgnore]
        private List<OutboxMessage> _outboxData = new List<OutboxMessage>();

        public Relationship(long ukprn, long accountProviderLegalEntityId,
            long accountId, string accountPublicHashedId, string accountName, 
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, 
            int accountProviderId, DateTime created, string messageId)
            : base(1, "permission")
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
            Created = created;

            AddMessageToOutbox(messageId, created);
        }

        [JsonConstructor]
        protected Relationship()
        {
        }

        public void Recreate(long ukprn, long accountProviderLegalEntityId, 
            long accountId, string accountPublicHashedId, string accountName, 
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, 
            int accountProviderId, DateTime reactivated, string messageId)
        {
            void VerifyPermissionCanBeReActivated()
            {
                if (Deleted == null)
                    throw new InvalidOperationException(
                        $"Message {messageId} is trying to recreate a relationship which hasn't been deleted");

                if (Deleted > reactivated)
                    throw new InvalidOperationException(
                        $"Message {messageId} is trying to recreate a relationship which was deleted after the re-activate request");
            }

            if (MessageAlreadyProcessed(messageId))
                return;

            VerifyPermissionCanBeReActivated();

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
            void VerifyPermissionCanBeUpdated()
            {
                if (Deleted != null)
                    throw new InvalidOperationException(
                        $"Message {messageId} is trying to update a Relationship which has already been deleted");

                if (Created > updated)
                    throw new InvalidOperationException(
                        $"Message {messageId} is trying to update a Relationship that was created/re-activated after this update message");
            }

            if (MessageAlreadyProcessed(messageId))
                return;

            VerifyPermissionCanBeUpdated();

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
            void VerifyPermissionCanBeDeleted()
            {
                if (Deleted != null)
                    throw new InvalidOperationException($"Message {messageId} is trying to delete a Relationship which has already been deleted");

                if (Created > deleted)
                    throw new InvalidOperationException($"Message {messageId} is trying to delete a Relationship that has been created/re-activated after this delete request");

                if (Updated > deleted)
                    throw new InvalidOperationException($"Message {messageId} is trying to delete a Relationship that has been updated after this delete request");
            }

            if (MessageAlreadyProcessed(messageId))
                return;

            VerifyPermissionCanBeDeleted();

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
            _outboxData.Add(new OutboxMessage(messageId, created));
        }
    }
}