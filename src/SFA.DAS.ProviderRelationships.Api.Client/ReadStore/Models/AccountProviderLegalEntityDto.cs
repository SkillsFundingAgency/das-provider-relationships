using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models
{
    public interface IAccountProviderLegalEntityDto
    {
        long AccountLegalEntityId { get; }

        long Ukprn { get; }

        IEnumerable<Operation> Operations { get; }

        DateTime? Deleted { get; }
    }

    //todo: make these internal if poss
    //todo: interface so the original and copy don't get out of sync. interface in types
    //todo: if use interface, then should be able to fakeiteasy
    //todo: could we plug in a CamelCaseNamingStrategy? see https://stackoverflow.com/questions/21783137/default-camel-case-of-property-names-in-json-serialization
    public class AccountProviderLegalEntityDto : IAccountProviderLegalEntityDto //: IDocument
    {
        [JsonProperty("accountLegalEntityId")]
        public virtual long AccountLegalEntityId { get; private set; }

        [JsonProperty("ukprn")]
        public virtual long Ukprn { get; private set; }

        [JsonProperty("operations")]
        public virtual IEnumerable<Operation> Operations { get; private set; } = new HashSet<Operation>();

        [JsonProperty("deleted")]
        public virtual DateTime? Deleted { get; private set; }

//        [JsonConstructor]
//        public AccountProviderLegalEntityDto()
//        {
//        }
    }
}