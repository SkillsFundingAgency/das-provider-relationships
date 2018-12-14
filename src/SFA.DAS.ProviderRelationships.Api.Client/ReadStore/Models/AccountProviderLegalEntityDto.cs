using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Types.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models
{
    //todo: interface so the original and copy don't get out of sync. interface in types
    //todo: if use interface, then should be able to fakeiteasy
    //todo: could we plug in a CamelCaseNamingStrategy? see https://stackoverflow.com/questions/21783137/default-camel-case-of-property-names-in-json-serialization
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