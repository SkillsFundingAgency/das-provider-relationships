using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AddedAccountProviderEventAudit
    {
        public Guid Id { get; set; }
        public long AccountProviderId { get; set; }
        public long AccountId { get; set; }
        public long ProviderUkprn { get; set; }
        public Guid UserRef { get; set; }
        public DateTime Added { get; set; }
        public DateTime TimeLogged { get; set; }
    }
}