using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class Permission : Document
    {
        [JsonProperty("employerAccountId")]
        public virtual long EmployerAccountId { get; protected set; }

        [JsonProperty("employerAccountPublicHashedId")]
        public virtual string EmployerAccountPublicHashedId { get; protected set; }

        [JsonProperty("employerAccountName")]
        public virtual string EmployerAccountName { get; protected set; }

        [JsonProperty("employerAccountLegalEntityId")]
        public virtual long EmployerAccountLegalEntityId { get; protected set; }

        [JsonProperty("employerAccountLegalEntityPublicHashedId")]
        public virtual string EmployerAccountLegalEntityPublicHashedId { get; protected set; }

        [JsonProperty("employerAccountLegalEntityName")]
        public virtual string EmployerAccountLegalEntityName { get; protected set; }

        [JsonProperty("employerAccountProviderId")]
        public virtual int EmployerAccountProviderId { get; protected set; }

        [JsonProperty("ukprn")]
        public virtual long Ukprn { get; protected set; }

        [JsonProperty("operations")]
        public virtual IEnumerable<Operation> Operations { get; protected set; } = new List<Operation>();
        
        public Permission(long employerAccountId, string employerAccountPublicHashedId, string employerAccountName, long employerAccountLegalEntityId, string employerAccountLegalEntityPublicHashedId, string employerAccountLegalEntityName, int employerAccountProviderId, long ukprn)
            : this()
        {
            EmployerAccountId = employerAccountId;
            EmployerAccountPublicHashedId = employerAccountPublicHashedId;
            EmployerAccountName = employerAccountName;
            EmployerAccountLegalEntityId = employerAccountLegalEntityId;
            EmployerAccountLegalEntityPublicHashedId = employerAccountLegalEntityPublicHashedId;
            EmployerAccountLegalEntityName = employerAccountLegalEntityName;
            EmployerAccountProviderId = employerAccountProviderId;
            Ukprn = ukprn;
        }

        protected Permission() : base(1, "permission")
        {
        }
    }
}