namespace SFA.DAS.ProviderRelationships.Models
{
    public class Permission : Entity
    {
        public Permission(long accountLegalEntityId, long ukprn, PermissionType type)
        {
            //todo: calling vistuals in ctor. issue? https://stackoverflow.com/questions/119506/virtual-member-call-in-a-constructor
            AccountLegalEntityId = accountLegalEntityId;
            Ukprn = ukprn;
            Type = type;
        }

        protected Permission()
        {
        }

        //Id?
        public virtual int Id { get; set; }
        public virtual long AccountLegalEntityId { get; set; }
        public virtual long Ukprn { get; set; }
        public virtual PermissionType Type { get; set; }
        public virtual AccountLegalEntity AccountLegalEntity { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
