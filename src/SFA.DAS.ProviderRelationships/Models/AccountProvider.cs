using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountProvider : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual Account Account { get; protected set; }
        public virtual long AccountId { get; protected set; }
        public virtual Provider Provider { get; protected set; }
        public virtual long ProviderUkprn { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        public virtual ICollection<AccountProviderLegalEntity> AccountProviderLegalEntities { get; protected set; } = new List<AccountProviderLegalEntity>();
        
        public AccountProvider(Account account, Provider provider, User user)
        {
            Account = account;
            AccountId = account.Id;
            Provider = provider;
            ProviderUkprn = provider.Ukprn;
            Created = DateTime.UtcNow;
            
            Publish(() => new AddedAccountProviderEvent(Id, Account.Id, Provider.Ukprn, user.Ref, Created));
        }
        
        protected AccountProvider()
        {
        }
    }
}