using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderRelationships.Messages.Events;

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

        public void ChangeName(string name, DateTime changed)
        {
            if (IsChangeNameDateChronological(changed))
            {
                Name = name;
                Updated = changed;
                
                Publish(() => new ChangedAccountNameEvent(Id, Name, Updated.Value));
            }
        }

        public AccountProvider AddProvider(Provider provider, User user)
        {
            EnsureProviderHasNotAlreadyBeenAdded(provider);

            var accountProvider = new AccountProvider(this, provider, user);
            
            AccountProviders.Add(accountProvider);

            return accountProvider;
        }

        private void EnsureProviderHasNotAlreadyBeenAdded(Provider provider)
        {
            if (AccountProviders.Any(ap => ap.ProviderUkprn == provider.Ukprn))
            {
                throw new Exception("Requires provider has not already been added");
            }
        }

        private bool IsChangeNameDateChronological(DateTime changed)
        {
            return Updated == null || changed > Updated.Value;
        }
    }
}