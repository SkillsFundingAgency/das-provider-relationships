﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Account : Entity
    {
        public long Id { get; private set; }
        public string HashedId { get; private set; }
        public string PublicHashedId { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public IEnumerable<AccountLegalEntity> AccountLegalEntities => _accountLegalEntities;
        public IEnumerable<AccountProvider> AccountProviders => _accountProviders;
        
        private readonly List<AccountLegalEntity> _accountLegalEntities = new List<AccountLegalEntity>();
        private readonly List<AccountProvider> _accountProviders = new List<AccountProvider>();

        public Account(long id, string hashedId, string publicHashedId, string name, DateTime created)
        {
            Id = id;
            HashedId = hashedId;
            PublicHashedId = publicHashedId;
            Name = name;
            Created = created;
        }

        private Account()
        {
        }

        public AccountLegalEntity AddAccountLegalEntity(long accountLegalEntityId, string accountLegalEntityPublicHashedId, string name, DateTime added)
        {
            EnsureAccountLegalEntityHasNotAlreadyBeenAdded(accountLegalEntityId);
            
            var accountLegalEntity = new AccountLegalEntity(this, accountLegalEntityId, accountLegalEntityPublicHashedId, name, added);
            
            _accountLegalEntities.Add(accountLegalEntity);

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

        public AccountProvider AddProvider(Provider provider, User user, Guid? correlationId)
        {
            EnsureProviderHasNotAlreadyBeenAdded(provider);

            var accountProvider = new AccountProvider(this, provider, user, correlationId);
            
            _accountProviders.Add(accountProvider);

            return accountProvider;
        }

        public void RemoveAccountLegalEntity(AccountLegalEntity accountLegalEntity, DateTime removed)
        {
            EnsureAccountLegalEntityHasBeenAdded(accountLegalEntity);
            
            accountLegalEntity.Delete(removed);
        }

        private void EnsureAccountLegalEntityHasBeenAdded(AccountLegalEntity accountLegalEntity)
        {
            if (_accountLegalEntities.All(ale => ale.Id != accountLegalEntity.Id))
            {
                throw new InvalidOperationException("Requires account legal entity has been added");
            }
        }

        private void EnsureAccountLegalEntityHasNotAlreadyBeenAdded(long accountLegalEntityId)
        {
            if (_accountLegalEntities.Any(ale => ale.Id == accountLegalEntityId))
            {
                throw new InvalidOperationException("Requires account legal entity has not already been added");
            }
        }

        private void EnsureProviderHasNotAlreadyBeenAdded(Provider provider)
        {
            if (_accountProviders.Any(ap => ap.ProviderUkprn == provider.Ukprn))
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