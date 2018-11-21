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
        public virtual ICollection<AccountProvider> AccountProviders { get; protected set; } = new List<AccountProvider>();
        
        protected Provider()
        {
        }
    }
}