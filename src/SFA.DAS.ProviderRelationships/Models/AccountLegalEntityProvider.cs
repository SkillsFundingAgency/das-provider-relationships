using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Models
{
    // Models the relationship between an AccountLegalEntity and a Provider
    public class AccountLegalEntityProvider
    {
        public virtual long AccountLegalEntityId { get; protected set; }
        public virtual long Ukprn { get; protected set; }
        
        public virtual AccountLegalEntity AccountLegalEntity { get; protected set; }
        public virtual Provider Provider { get; protected set; }
        
        public virtual ICollection<Permission> Permissions { get; protected set; } = new List<Permission>();
        
        public AccountLegalEntityProvider(long accountLegalEntityId, long ukprn)
        {
            AccountLegalEntityId = accountLegalEntityId;
            Ukprn = ukprn;
        }
        
        protected AccountLegalEntityProvider()
        {
        }
    }
}