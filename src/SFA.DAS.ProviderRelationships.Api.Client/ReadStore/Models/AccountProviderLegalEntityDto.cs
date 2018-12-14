using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models
{
    //todo: make these internal if poss
    //todo: interface so the original and copy don't get out of sync. interface in types
    //todo: if use interface, then should be able to fakeiteasy
    public class AccountProviderLegalEntityDto : IDocument
    {
        #region From Document

        //todo: not needed? (apart from interface implementation)
        [JsonProperty("id")]
        public Guid Id { get; protected set; }

        //todo: not needed? (apart from interface implementation)
        [JsonIgnore]
        public string ETag { get; protected set; }
        
        #endregion From Document

        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; private set; }

        [JsonProperty("ukprn")]
        public long Ukprn { get; private set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; private set; } = new HashSet<Operation>();

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; private set; }

        [JsonConstructor]
        private AccountProviderLegalEntityDto()
        {
        }
    }
}