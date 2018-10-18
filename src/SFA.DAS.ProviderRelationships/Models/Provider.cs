using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Provider
    {
        public virtual long Ukprn { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        //public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; set; }
        public virtual ICollection<AccountLegalEntityProvider> AccountLegalEntityProviders { get; protected set; } = new List<AccountLegalEntityProvider>();
        
        //todo: how best to handle created?
        public Provider(long ukprn, string name, DateTime created)
        {
            Ukprn = ukprn;
            Name = name;
            Created = created;
        }

        protected Provider()
        {
        }
    }
}