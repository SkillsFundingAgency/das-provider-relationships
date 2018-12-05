using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ProviderRelationships.Auditing.DataAccess.Entities
{
    public class CreatedAccountEventAudit
    {
        public long AccountId { get; set; }
        public string PublicHashedId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
        public DateTime TimeLogged { get; set; }
    }
}
