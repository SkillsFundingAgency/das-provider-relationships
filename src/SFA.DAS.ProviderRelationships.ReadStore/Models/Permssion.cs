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
            if (MessageAlreadyPrcessed(messageId))
                return;

            if (Deleted == null)
                throw new Exception($"Message {messageId} is trying to recreate a relationship which hasn't been deleted");

            if (Deleted > reactivated)
                throw new Exception($"Message {messageId} is trying to recreate a relationship which was deleted afterwards");

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

        public void UpdatePermissions(HashSet<Operation> grants, HashSet<Operation> revokes, DateTime created, string messageId)
        {
            if (MessageAlreadyPrcessed(messageId))
                return;

            var permissionsWhichExistInBothSets = grants.Join(revokes, g => g, r => r, (g, r) => g);

            if (permissionsWhichExistInBothSets.Any())
                throw new Exception("Permissions cannot both be granted and revoked in the same request");

            var grantsWhichAreAlreadyPresent = Operations.Join(grants, o => o, g => g, (o, g) => o);
            if (grantsWhichAreAlreadyPresent.Any())
                throw new Exception("Permissions have already been granted");

            Operations = grants;

            AddMessageToOutbox(messageId, created);
        }

        private bool MessageAlreadyPrcessed(string messageId)
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