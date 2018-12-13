using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class CreatedAccountEventAudit : Entity
    {
        public Guid Id { get; set; }
        public long AccountId { get; set; }
        public string PublicHashedId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
        public DateTime TimeLogged { get; set; }
        public string HashedId { get; set; }
    }
}
