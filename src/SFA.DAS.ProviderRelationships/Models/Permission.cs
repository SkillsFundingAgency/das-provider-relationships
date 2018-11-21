using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Permission : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual AccountProviderLegalEntity AccountProviderLegalEntity { get; protected set; }
        public virtual long AccountProviderLegalEntityId { get; protected set; }
        public virtual Operation Operation { get; protected set; }
        
        public Permission(AccountProviderLegalEntity accountProviderLegalEntity, Operation operation)
        {
            AccountProviderLegalEntity = accountProviderLegalEntity;
            AccountProviderLegalEntityId = AccountProviderLegalEntity.Id;
            Operation = operation;
        }

        protected Permission()
        {
        }
    }
}