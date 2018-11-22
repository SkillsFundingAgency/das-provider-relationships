using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

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

        public void UpdatePermissions(AccountLegalEntity accountLegalEntity, User user, HashSet<Operation> grantedOperations)
        {
            var accountProviderLegalEntity = AccountProviderLegalEntities.SingleOrDefault(aple => aple.AccountLegalEntityId == accountLegalEntity.Id);

            if (accountProviderLegalEntity == null)
            {
                AccountProviderLegalEntities.Add(new AccountProviderLegalEntity(this, accountLegalEntity, user, grantedOperations));
            }
            else
            {
                accountProviderLegalEntity.UpdatePermissions(user, grantedOperations);
            }
        }
    }
}