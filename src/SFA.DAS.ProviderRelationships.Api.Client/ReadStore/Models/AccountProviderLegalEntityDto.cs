using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models
{
    //todo: make these internal if poss
    //todo: interface so the original and copy don't get out of sync. interface in types
    public class AccountProviderLegalEntityDto : IDocument
    {
        #region From Document

        [JsonProperty("id")]
        public Guid Id { get; protected set; }

        [JsonIgnore]
        public string ETag { get; protected set; }
        
        #endregion From Document

        //todo: needs etag string
        
        [JsonProperty("accountId")]
        public long AccountId { get; private set; }

        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; private set; }

        [JsonProperty("accountProviderId")]
        public long AccountProviderId { get; private set; }

        [JsonProperty("accountProviderLegalEntityId")]
        public long AccountProviderLegalEntityId { get; private set; }

        [JsonProperty("ukprn")]
        public long Ukprn { get; private set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; private set; } = new HashSet<Operation>();

        [JsonProperty("created")]
        public DateTime Created { get; private set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; private set; }

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; private set; }

        public AccountProviderLegalEntityDto(long accountId, long accountLegalEntityId, long accountProviderId, long accountProviderLegalEntityId, long ukprn, HashSet<Operation> grantedOperations, DateTime created)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Operations = grantedOperations;
            Created = created;
        }

        [JsonConstructor]
        private AccountProviderLegalEntityDto()
        {
        }
    }
}