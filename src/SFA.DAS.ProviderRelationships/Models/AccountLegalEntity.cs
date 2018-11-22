using System;
using System.Collections.Generic;

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
        public virtual ICollection<AccountProviderLegalEntity> AccountProviderLegalEntities { get; protected set; } = new List<AccountProviderLegalEntity>();

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
            }
        }

        internal void Delete(DateTime deleted)
        {
            if (IsDeleteDateChronological(deleted))
            {
                EnsureAccountLegalEntityHasNotAlreadyBeenDeleted();

                foreach (var accountProviderLegalEntity in AccountProviderLegalEntities)
                {
                    accountProviderLegalEntity.Delete(deleted);
                }
                
                AccountProviderLegalEntities.Clear();
                
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