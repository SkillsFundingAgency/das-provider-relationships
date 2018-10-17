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
        public virtual DateTime? Updated { get; set; }
        
        /*public virtual Account Account { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<Provider> Providers { get; set; }*/
        
        public AccountLegalEntity(long id, string publicHashedId, long accountId, string name, DateTime created)
        {
            Id = id;
            PublicHashedId = publicHashedId;
            AccountId = accountId;
            Name = name;
            Created = created;
        }

        protected AccountLegalEntity()
        {
        }

        public void ChangeName(string name, DateTime changed)
        {
            if (Updated == null || changed > Updated.Value)
            {
                Name = name;
                Updated = changed;
            }
        }
    }
}
