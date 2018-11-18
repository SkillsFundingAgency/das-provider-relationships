using System;
using System.Collections.Generic;
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
        public virtual ICollection<Permission> Permissions { get; protected set; } = new List<Permission>();

        internal AccountLegalEntity(Account account, long id, string publicHashedId, string name, DateTime created)
        {
            Id = id;
            PublicHashedId = publicHashedId;
            Account = account;
            AccountId = account.Id;
            Name = name;
            Created = created;
        }

        protected AccountLegalEntity()
        {
        }

        public void UpdateName(string name, DateTime updated)
        {
            if (IsUpdatedNameDateChronological(updated) && IsUpdatedNameDifferent(name))
            {
                Name = name;
                Updated = updated;
                
                Publish(() => new UpdatedAccountLegalEntityNameEvent(Id, AccountId, Name, Updated.Value));
            }
        }

        internal void Delete(DateTime deleted)
        {
            if (IsDeleteDateChronological(deleted))
            {
                EnsureAccountLegalEntityHasNotAlreadyBeenDeleted();
                
                Deleted = deleted;
                
                Publish(() => new DeletedAccountLegalEntityEvent(Id, AccountId, Deleted.Value));
            }
        }

        private void EnsureAccountLegalEntityHasNotAlreadyBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires account legal entity has not already been deleted");
            }
        }

        private bool IsDeleteDateChronological(DateTime deleted)
        {
            return Deleted == null || deleted > Deleted.Value;
        }

        private bool IsUpdatedNameDateChronological(DateTime updated)
        {
            return Updated == null || updated > Updated.Value;
        }

        private bool IsUpdatedNameDifferent(string name)
        {
            return name != Name;
        }
    }
}