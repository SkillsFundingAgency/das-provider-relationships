using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Messages.Events;
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
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        
        public AccountProviderLegalEntity(AccountProvider accountProvider, AccountLegalEntity accountLegalEntity, User user, HashSet<Operation> grantedOperations)
        {
            AccountProvider = accountProvider;
            AccountProviderId = accountProvider.Id;
            AccountLegalEntity = accountLegalEntity;
            AccountLegalEntityId = accountLegalEntity.Id;

            foreach (var operation in grantedOperations)
            {
                Permissions.Add(new Permission(this, operation));
            }
            
            Created = DateTime.UtcNow;
            
            Publish(() => new UpdatedPermissionsEvent(AccountProvider.AccountId, AccountLegalEntity.Id, AccountProvider.Id, Id, AccountProvider.ProviderUkprn, user.Ref, grantedOperations, Created));
        }

        protected AccountProviderLegalEntity()
        {
        }

        internal void Delete(DateTime deleted)
        {
            Permissions.Clear();
            
            Publish(() => new DeletedPermissionsEvent(Id, AccountProvider.ProviderUkprn, deleted));
        }

        internal void UpdatePermissions(User user, HashSet<Operation> grantedOperations)
        {
            Permissions.Clear();
            
            foreach (var operation in grantedOperations)
            {
                Permissions.Add(new Permission(this, operation));
            }
            
            Updated = DateTime.UtcNow;
            
            Publish(() => new UpdatedPermissionsEvent(AccountProvider.AccountId, AccountLegalEntity.Id, AccountProvider.Id, Id, AccountProvider.ProviderUkprn, user.Ref, grantedOperations, Updated.Value));
        }
    }
}