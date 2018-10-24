using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Account : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; protected set; } = new List<AccountLegalEntity>();
        public virtual ICollection<AccountProvider> AccountProviders { get; protected set; } = new List<AccountProvider>();

        public Account(long id, string name, DateTime created)
        {
            Id = id;
            Name = name;
            Created = created;
        }

        protected Account()
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

        public AccountProvider AddProvider(Provider provider, User user)
        {
            RequiresProviderHasNotBeenAddedAlready(provider);

            var accountProvider = new AccountProvider(this, provider, user);
            
            AccountProviders.Add(accountProvider);

            return accountProvider;
        }

        private void RequiresProviderHasNotBeenAddedAlready(Provider provider)
        {
            if (AccountProviders.Any(ap => ap.ProviderUkprn == provider.Ukprn))
                throw new Exception("Requires provider has not been added already");
        }
    }
}