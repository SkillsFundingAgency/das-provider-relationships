namespace SFA.DAS.ProviderRelationships.Models
{
    public class Permission : Entity
    {
        public virtual int Id { get; set; }
        public virtual long AccountLegalEntityId { get; set; }
        public virtual long Ukprn { get; set; }
        public virtual PermissionType Type { get; set; }
        public virtual AccountLegalEntity AccountLegalEntity { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
