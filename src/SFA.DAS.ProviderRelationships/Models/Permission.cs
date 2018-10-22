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
        public virtual int Id { get; protected set; }
        public virtual long AccountLegalEntityId { get; protected set; }
        public virtual long Ukprn { get; protected set; }
        public virtual PermissionType Type { get; protected set; }
        public virtual AccountLegalEntity AccountLegalEntity { get; protected set; }
        public virtual Provider Provider { get; protected set; }
        
        //todo: need to add created / userref / username for auditing
        // by deleting, we lose auditing from permissions table, unless we add PermissionAudit (name) table for deleted audits
        // put all changes into audit table
        
        //todo: ^^ auditing for relationships and permissions in separate tables that are updated from events
    }
}
