using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountLegalEntity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual long Id { get; set; }
        public virtual string PublicHashedId { get; set; }

        public virtual string Name { get; set; }

        public virtual long AccountId { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        
        //[ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
        
        // providers that have been set-up as having a relationships with the AccountLegalEntity
        public virtual ICollection<Provider> Providers { get; set; }
    }
}
