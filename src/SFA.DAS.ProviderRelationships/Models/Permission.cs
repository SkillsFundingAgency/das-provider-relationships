using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Permission : Entity
    {
        public virtual long Id { get; protected set; }
        public virtual AccountProvider AccountProvider { get; protected set; }
        public virtual long AccountProviderId { get; protected set; }
        public virtual Operation Operation { get; protected set; }
        
        public Permission(AccountProvider accountProvider, Operation operation)
        {
            AccountProvider = accountProvider;
            Operation = operation;
        }

        protected Permission()
        {
        }
    }
}