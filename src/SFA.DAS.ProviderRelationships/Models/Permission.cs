using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Permission : Entity
    {
        public long Id { get; private set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; private set; }
        public long AccountProviderLegalEntityId { get; private set; }
        public Operation Operation { get; private set; }
        
        public Permission(AccountProviderLegalEntity accountProviderLegalEntity, Operation operation)
        {
            AccountProviderLegalEntity = accountProviderLegalEntity;
            AccountProviderLegalEntityId = accountProviderLegalEntity.Id;
            Operation = operation;
        }

        private Permission()
        {
        }
    }
}