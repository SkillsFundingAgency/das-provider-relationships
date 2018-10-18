using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Provider
    {
        public virtual long Ukprn { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        //public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; set; }
        public virtual ICollection<AccountLegalEntityProvider> AccountLegalEntityProviders { get; protected set; }
    }
}