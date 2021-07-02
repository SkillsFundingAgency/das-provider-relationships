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

            Publish(() => new UpdatedPermissionsEvent(
                AccountProvider.AccountId, 
                AccountLegalEntity.Id, 
                AccountProvider.Id, 
                Id, 
                AccountProvider.ProviderUkprn, 
                user.Ref, 
                user.Email, 
                user.FirstName, 
                user.LastName, 
                grantedOperations, 
                new HashSet<Operation>(),
                Created));
        }

        private AccountProviderLegalEntity()
        {
        }

        internal void Delete(DateTime deleted)
        {
            _permissions.Clear();

            Publish(() => new DeletedPermissionsEventV2(Id, AccountProvider.ProviderUkprn, AccountLegalEntityId, deleted));
        }

        internal void RevokePermissions(User user, IList<Operation> operationsToRevoke)
        {
            EnsureAccountLegalEntityHasNotBeenDeleted();

            if (operationsToRevoke.Contains(Operation.Recruitment) &&
                !operationsToRevoke.Contains(Operation.RecruitmentRequiresReview))
            {
                operationsToRevoke.Add(Operation.RecruitmentRequiresReview);
            }

            var remainingOperations = Permissions
                .Where(x => !operationsToRevoke.Contains(x.Operation))
                .Select(x => x.Operation);

            UpdatePermissions(
                user: null,
                grantedOperations: new HashSet<Operation>(remainingOperations));
        }

        internal void UpdatePermissions(User user, HashSet<Operation> grantedOperations)
        {
            var sortedGrantedOperations = grantedOperations.OrderBy(x => x);
            var previousPermissionOperations = _permissions.Select(x => x.Operation).ToList();
            if (sortedGrantedOperations.SequenceEqual(previousPermissionOperations.OrderBy(x => x)))
                return;

            _permissions.Clear();
            _permissions.AddRange(grantedOperations.Select(o => new Permission(this, o)));

            Updated = DateTime.UtcNow;

            Publish(() => new UpdatedPermissionsEvent(
                AccountProvider.AccountId, 
                AccountLegalEntity.Id, 
                AccountProvider.Id, 
                Id, 
                AccountProvider.ProviderUkprn, 
                user?.Ref, 
                user?.Email, 
                user?.FirstName, 
                user?.LastName, 
                grantedOperations,
                new HashSet<Operation>(previousPermissionOperations),
                Updated.Value));
        }

        private void EnsureAccountLegalEntityHasNotBeenDeleted()
        {
            if (AccountLegalEntity.Deleted != null)
            {
                throw new InvalidOperationException("Requires account legal entity has not been deleted");
            }
        }
    }
}