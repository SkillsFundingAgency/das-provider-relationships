using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountLegalEntity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual long AccountLegalEntityId { get; set; }
        public virtual string Name { get; set; }
        public virtual string PublicHashedId { get; set; } // rename AccountLegalEntityPublicHashedId

        public virtual long AccountId { get; set; }

        //[ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
