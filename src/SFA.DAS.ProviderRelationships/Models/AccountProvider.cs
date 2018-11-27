using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountProvider : Entity
    {
        public long Id { get; private set; }
        public Account Account { get; private set; }
        public long AccountId { get; private set; }
        public Provider Provider { get; private set; }
        public long ProviderUkprn { get; private set; }
        public DateTime Created { get; private set; }
        public IEnumerable<AccountProviderLegalEntity> AccountProviderLegalEntities => _accountProviderLegalEntities;

        private readonly List<AccountProviderLegalEntity> _accountProviderLegalEntities = new List<AccountProviderLegalEntity>();

        public AccountProvider(Account account, Provider provider, User user)
        {
            Account = account;
            AccountId = account.Id;
            Provider = provider;
            ProviderUkprn = provider.Ukprn;
            Created = DateTime.UtcNow;
            
            Publish(() => new AddedAccountProviderEvent(Id, Account.Id, Provider.Ukprn, user.Ref, Created));
        }

        private AccountProvider()
        {
        }

        public void UpdatePermissions(AccountLegalEntity accountLegalEntity, User user, HashSet<Operation> grantedOperations)
        {
            var accountProviderLegalEntity = _accountProviderLegalEntities.SingleOrDefault(aple => aple.AccountLegalEntityId == accountLegalEntity.Id);

            if (accountProviderLegalEntity == null)
            {
                _accountProviderLegalEntities.Add(new AccountProviderLegalEntity(this, accountLegalEntity, user, grantedOperations));
            }
            else
            {
                accountProviderLegalEntity.UpdatePermissions(user, grantedOperations);
            }
        }
    }
}