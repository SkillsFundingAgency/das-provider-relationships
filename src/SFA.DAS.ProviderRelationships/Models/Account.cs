using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Account : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual string PublicHashedId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; protected set; } = new List<AccountLegalEntity>();
        public virtual ICollection<AccountProvider> AccountProviders { get; protected set; } = new List<AccountProvider>();

        public Account(long id, string publicHashedId, string name, DateTime created)
        {
            Id = id;
            PublicHashedId = publicHashedId;
            Name = name;
            Created = created;
        }

        protected Account()
        {
        }

        public AccountLegalEntity AddAccountLegalEntity(long accountLegalEntityId, string accountLegalEntityPublicHashedId, string name, DateTime added)
        {
            EnsureAccountLegalEntityHasNotAlreadyBeenAdded(accountLegalEntityId);
            
            var accountLegalEntity = new AccountLegalEntity(this, accountLegalEntityId, accountLegalEntityPublicHashedId, name, added);
            
            AccountLegalEntities.Add(accountLegalEntity);

            return accountLegalEntity;
        }

        public void UpdateName(string name, DateTime updated)
        {
            if (IsUpdatedNameDateChronological(updated) && IsUpdatedNameDifferent(name))
            {
                Name = name;
                Updated = updated;
            }
        }

        public AccountProvider AddProvider(Provider provider, User user)
        {
            EnsureProviderHasNotAlreadyBeenAdded(provider);

            var accountProvider = new AccountProvider(this, provider, user);
            
            AccountProviders.Add(accountProvider);

            return accountProvider;
        }

        public void RemoveAccountLegalEntity(AccountLegalEntity accountLegalEntity, DateTime removed)
        {
            EnsureAccountLegalEntityHasBeenAdded(accountLegalEntity);
            
            accountLegalEntity.Delete(removed);
        }

        private void EnsureAccountLegalEntityHasBeenAdded(AccountLegalEntity accountLegalEntity)
        {
            if (AccountLegalEntities.All(ale => ale.Id != accountLegalEntity.Id))
            {
                throw new InvalidOperationException("Requires account legal entity has been added");
            }
        }

        private void EnsureAccountLegalEntityHasNotAlreadyBeenAdded(long accountLegalEntityId)
        {
            if (AccountLegalEntities.Any(ale => ale.Id == accountLegalEntityId))
            {
                throw new InvalidOperationException("Requires account legal entity has not already been added");
            }
        }

        private void EnsureProviderHasNotAlreadyBeenAdded(Provider provider)
        {
            if (AccountProviders.Any(ap => ap.ProviderUkprn == provider.Ukprn))
            {
                throw new InvalidOperationException("Requires provider has not already been added");
            }
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