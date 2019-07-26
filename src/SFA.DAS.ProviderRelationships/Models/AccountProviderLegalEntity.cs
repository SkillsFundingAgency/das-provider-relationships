using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AccountProviderLegalEntity : Entity
    {
        public long Id { get; private set; }
        public AccountProvider AccountProvider { get; private set; }
        public long AccountProviderId { get; private set; }
        public AccountLegalEntity AccountLegalEntity { get; private set; }
        public long AccountLegalEntityId { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public IEnumerable<Permission> Permissions => _permissions;

        private readonly List<Permission> _permissions = new List<Permission>();

        public AccountProviderLegalEntity(AccountProvider accountProvider, AccountLegalEntity accountLegalEntity, User user, HashSet<Operation> grantedOperations)
        {
            AccountProvider = accountProvider;
            AccountProviderId = accountProvider.Id;
            AccountLegalEntity = accountLegalEntity;
            AccountLegalEntityId = accountLegalEntity.Id;
            
            _permissions.AddRange(grantedOperations.Select(o => new Permission(this, o)));
            
            Created = DateTime.UtcNow;
            
            Publish(() => new UpdatedPermissionsEvent(AccountProvider.AccountId, AccountLegalEntity.Id, AccountProvider.Id, Id, AccountProvider.ProviderUkprn, user.Ref, grantedOperations, Created));
        }

        private AccountProviderLegalEntity()
        {
        }

        internal void Delete(DateTime deleted)
        {
            _permissions.Clear();
            
            Publish(() => new DeletedPermissionsEventV2(Id, AccountProvider.ProviderUkprn, AccountLegalEntityId, deleted));
        }

        internal void UpdatePermissions(User user, HashSet<Operation> grantedOperations)
        {
            _permissions.Clear();
            _permissions.AddRange(grantedOperations.Select(o => new Permission(this, o)));
            
            Updated = DateTime.UtcNow;

            Guid userRef = user?.Ref ?? Guid.Empty;
            Publish(() => new UpdatedPermissionsEvent(AccountProvider.AccountId, AccountLegalEntity.Id, AccountProvider.Id, Id, AccountProvider.ProviderUkprn, userRef, grantedOperations, Updated.Value));
        }
    }
}