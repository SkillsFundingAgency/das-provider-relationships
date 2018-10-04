using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class Permission : Entity
    {
        public virtual int PermissionId { get; set; }
        public virtual PermissionType Type { get; set; }

        public virtual int AccountLegalEntityId { get; set; }

        //[ForeignKey("AccountLegalEntityId")]
        public virtual AccountLegalEntity AccountLegalEntity { get; set; }
    }
}
