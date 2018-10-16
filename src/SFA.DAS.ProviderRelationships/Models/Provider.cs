using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Provider
    {
        [Key]
        public virtual long UKPRN { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        
        // AccountLegalEntities that have set up a relationship with this provider
        public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; set; }
    }
}