namespace SFA.DAS.ProviderRelationships.Models
{
    public class Permission : Entity
    {
        public Permission(long accountLegalEntityId, long ukprn, PermissionType type)
        {
            //todo: calling virtuals in ctor. issue? https://stackoverflow.com/questions/119506/virtual-member-call-in-a-constructor
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
        
        //todo: need to add created / userref / username for auditing
        // by deleting, we lose auditing from permissions table, unless we add PermissionAudit (name) table for deleted audits
        // put all changes into audit table
        
        //todo: ^^ auditing for relationships and permissions in seperate tables that are updated from events
    }
}
