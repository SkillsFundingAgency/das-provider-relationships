using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountLegalEntity : Entity
    {
        public long Id { get; private set; }
        public string PublicHashedId { get; private set; }
        public Account Account { get; private set; }
        public long AccountId { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public DateTime? Deleted { get; private set; }
        public IEnumerable<AccountProviderLegalEntity> AccountProviderLegalEntities => _accountProviderLegalEntities;

        private readonly List<AccountProviderLegalEntity> _accountProviderLegalEntities = new List<AccountProviderLegalEntity>();

        internal AccountLegalEntity(Account account, long id, string publicHashedId, string name, DateTime created)
        {
            Id = id;
            PublicHashedId = publicHashedId;
            Account = account;
            AccountId = account.Id;
            Name = name;
            Created = created;
        }

        private AccountLegalEntity()
        {
        }

        public void UpdateName(string name, DateTime updated)
        {
            if (IsUpdatedNameDateChronological(updated) && IsUpdatedNameDifferent(name))
            {
                EnsureAccountLegalEntityHasNotBeenDeleted();
                
                Name = name;
                Updated = updated;
            }
        }

        internal void Delete(DateTime deleted)
        {
            EnsureAccountLegalEntityHasNotBeenDeleted();

            foreach (var accountProviderLegalEntity in _accountProviderLegalEntities)
            {
                accountProviderLegalEntity.Delete(deleted);
            }
            
            _accountProviderLegalEntities.Clear();
            
            Deleted = deleted;
        }

        private void EnsureAccountLegalEntityHasNotBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires account legal entity has not been deleted");
            }
        }

        private bool IsUpdatedNameDateChronological(DateTime updated)
        {
            return (Updated == null || updated > Updated.Value) && (Deleted == null || updated > Deleted.Value);
        }

        private bool IsUpdatedNameDifferent(string name)
        {
            return name != Name;
        }
    }
}