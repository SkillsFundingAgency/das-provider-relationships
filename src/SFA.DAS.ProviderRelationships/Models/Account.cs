using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Models
{
    //todo:
//    aleprovider needs to be an entity
//    add created and updated to fields
//    switch to ef core
//    permission comes off aleprovider
//    change eg accountid to id
//    remove dbsetstub and use ef core's in memory database instead
    // PublicHashedId in Account - needed?? no
    // change how permission works - change from row presence to flag and add created and updated? (for out-of-ordering?)
    // https://medium.com/@hoagsie/youre-all-doing-entity-framework-wrong-ea0c40e20502
    // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
    
    public class Account : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        //public virtual string PublicHashedId { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        
        public virtual ICollection<AccountLegalEntity> AccountLegalEntities { get; set; }

//        public void AddAccountLegalEntity(AccountLegalEntity accountLegalEntity)
//        {
//            AccountLegalEntities.Add(accountLegalEntity);
//        }
    }
}
