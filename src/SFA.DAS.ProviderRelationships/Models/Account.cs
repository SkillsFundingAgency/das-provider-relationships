using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Account : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual long AccountId { get; set; }
        public virtual string Name { get; set; }
        public virtual string PublicHashedId { get; set; }

        public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; set; }
    }
}
