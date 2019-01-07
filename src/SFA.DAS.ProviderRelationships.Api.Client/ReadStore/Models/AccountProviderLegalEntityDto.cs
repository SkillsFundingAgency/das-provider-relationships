using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Types.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models
{
    internal class AccountProviderLegalEntityDto : IAccountProviderLegalEntityDto
    {
        [JsonProperty("accountLegalEntityId")]
        public virtual long AccountLegalEntityId { get; private set; }

        [JsonProperty("ukprn")]
        public virtual long Ukprn { get; private set; }

        [JsonProperty("operations")]
        public virtual IEnumerable<Operation> Operations { get; private set; } = new HashSet<Operation>();

        [JsonProperty("deleted")]
        public virtual DateTime? Deleted { get; private set; }
    }
}