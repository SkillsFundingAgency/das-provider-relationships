using System;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountLegalEntity : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual string PublicHashedId { get; protected set; }
        public virtual Account Account { get; protected set; }
        public virtual long AccountId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        public virtual DateTime? Deleted { get; protected set; }

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
            if (IsChangeNameDateChronological(changed))
            {
                Name = name;
                Updated = changed;
                
                Publish(() => new UpdatedLegalEntityEvent(Id, name, Updated.Value));
            }
        }

        public void Delete(DateTime deleted)
        {
            if (IsDeleteDateChronological(deleted))
            {
                EnsureAccountLegalEntityHasNotAlreadyBeenDeleted();
                
                Deleted = deleted;
            }
        }

        private void EnsureAccountLegalEntityHasNotAlreadyBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires account legal entity has not already been deleted");
            }
        }

        private bool IsChangeNameDateChronological(DateTime changed)
        {
            return Updated == null || changed > Updated.Value;
        }

        private bool IsDeleteDateChronological(DateTime deleted)
        {
            return Deleted == null || deleted > Deleted.Value;
        }
    }
}