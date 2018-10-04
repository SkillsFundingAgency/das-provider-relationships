using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Account : Entity
    {
        //public virtual int Id { get; set; }
        public virtual long AccountId { get; set; }
        public virtual string Name { get; set; }
        public virtual string PublicHashedId { get; set; }

        public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; set; }
    }
}
