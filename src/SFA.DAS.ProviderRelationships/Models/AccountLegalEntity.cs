using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountLegalEntity : Entity
    {
        public virtual long Id { get; set; }
        public virtual string PublicHashedId { get; set; }
        public virtual long AccountId { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        
        /*public virtual Account Account { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<Provider> Providers { get; set; }*/
    }
}
