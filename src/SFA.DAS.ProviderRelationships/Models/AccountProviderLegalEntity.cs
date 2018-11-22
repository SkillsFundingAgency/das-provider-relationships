using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountProviderLegalEntity : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual AccountProvider AccountProvider { get; protected set; }
        public virtual long AccountProviderId { get; protected set; }
        public virtual AccountLegalEntity AccountLegalEntity { get; protected set; }
        public virtual long AccountLegalEntityId { get; protected set; }
        public virtual ICollection<Permission> Permissions { get; protected set; } = new List<Permission>();
        
        public AccountProviderLegalEntity(AccountProvider accountProvider, AccountLegalEntity accountLegalEntity, IEnumerable<Operation> operations)
        {
            AccountProvider = accountProvider;
            AccountProviderId = accountProvider.Id;
            AccountLegalEntity = accountLegalEntity;
            AccountLegalEntityId = accountLegalEntity.Id;

            foreach (var operation in operations)
            {
                Permissions.Add(new Permission(this, operation));
            }
        }

        protected AccountProviderLegalEntity()
        {
        }

        public void Delete()
        {
            Permissions.Clear();
        }
    }
}