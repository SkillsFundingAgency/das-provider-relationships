using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountLegalEntity : Entity
    {
        //public virtual int Id { get; protected set; }
        public virtual long AccountLegalEntityId { get; set; }
        public virtual string Name { get; set; }
        public virtual string PublicHashedId { get; set; }

        public virtual int AccountId { get; set; }

        //[ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        public virtual ICollection<Permission> AccountLegalEntities { get; set; }
    }
}
