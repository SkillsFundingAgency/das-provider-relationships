using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Provider
    {
        public virtual long Ukprn { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }
        public virtual ICollection<AccountLegalEntityProvider> AccountLegalEntityProviders { get; protected set; } = new List<AccountLegalEntityProvider>();
        public virtual IEnumerable<AccountProvider> AccountProviders => _accountProviders;
        
        private readonly List<AccountProvider> _accountProviders = new List<AccountProvider>();
        
        protected Provider()
        {
        }
    }
}